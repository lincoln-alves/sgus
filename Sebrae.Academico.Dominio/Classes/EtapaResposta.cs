using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EtapaResposta : EntidadeBasica
    {
        public virtual Etapa Etapa { get; set; }
        //public virtual Permissao Permissao { get; set; }
        public virtual ProcessoResposta ProcessoResposta { get; set; }
        public virtual Usuario Analista { get; set; }
        public virtual Cargo CargoAnalista { get; set; }
        public virtual Usuario Assessor { get; set; }
        public virtual int Status { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual DateTime? DataPreenchimento { get; set; }
        public virtual DateTime? PrazoEncaminhamento { get; set; }
        public virtual IList<EtapaRespostaPermissao> PermissoesNucleoEtapaResposta { get; set; }
        public new virtual DateTime? DataAlteracao { get; set; }
        public new virtual IList<EtapaEncaminhamentoUsuario> ListaEtapaEncaminhamentoUsuario { get; set; }
        public virtual IList<CampoResposta> ListaCampoResposta { get; set; }

        public EtapaResposta()
        {
            this.Etapa = new Etapa();
            ListaEtapaEncaminhamentoUsuario = new List<EtapaEncaminhamentoUsuario>();
            //this.ProcessoResposta = new ProcessoResposta();
            //this.Analista = new Usuario();
        }

        public virtual string ObterSituacao
        {
            get
            {
                string responsavel = "";
                string status;

                switch (Status)
                {
                    case (int)enumStatusEtapaResposta.AAjustar:
                        status = "Em ajuste";
                        break;
                    case (int)enumStatusEtapaResposta.Aguardando:
                        status = "Em análise";
                        break;
                    case (int)enumStatusEtapaResposta.Aprovado:
                        status = "Aprovado ";
                        break;
                    case (int)enumStatusEtapaResposta.Concluido:
                        status = "Concluido";
                        break;
                    case (int)enumStatusEtapaResposta.Negado:
                        status = "Negado";
                        break;
                    default:
                        return "";
                }

                if (Etapa.PermissoesNucleo != null && Etapa.PermissoesNucleo.Count > 0)
                    responsavel = "pelo núcleo";

                if (Etapa.Permissoes.Any(p => p.Usuario != null))
                    responsavel = "pelo colaborador";

                if (Etapa.Permissoes.Any(p => p.ChefeImediato.HasValue && p.ChefeImediato.Value))
                    responsavel = "por chefe imediado.";

                if (Etapa.Permissoes.Any(p => p.GerenteAdjunto.HasValue && p.GerenteAdjunto.Value))
                    responsavel = "por gerente adjunto.";

                if (Etapa.Permissoes.Any(p => p.DiretorCorrespondente.HasValue && p.DiretorCorrespondente.Value))
                    responsavel = "por diretor correspondente.";

                return string.Format("{0}", status, responsavel);
            }
        }

        /// <summary>
        /// Obtém todos os analistas dessa Etapa.
        /// </summary>
        /// <param name="diretores">Diretores da UF do demandante. Precisa vir de fora pois possuem um fluxo diferente.</param>
        /// <param name="usuario">Usuário possível analista</param>
        /// <param name="cargos">OPCIONAL. Filtrar os analistas para estes cargos específicos</param>
        /// <returns></returns>
        public virtual List<Usuario> ObterAnalistas(List<UsuarioCargo> diretores, Usuario usuario = null, List<Cargo> cargos = null)
        {
            var demandante = ProcessoResposta.Usuario;

            var cargoDemandante = ProcessoResposta.ObterUltimoCargoDemandante();
            
            var analistas = new List<Usuario>();

            if (cargoDemandante == null)
                return analistas;

            var permissoesAnalise = Etapa.Permissoes.Where(x => x.Analisar == true).AsQueryable();

            // Se a etapa for especificamente para um usuário definido, adiciona o usuário como analista.
            var permissaoColaborador = permissoesAnalise.Where(x => x.Usuario != null);

            if (permissaoColaborador.Any())
            {
                analistas.AddRange(permissaoColaborador.Select(x => x.Usuario));
            }

            // Se for primeira etapa ou etapa para o solicitante, adiciona o demandante como analista.
            if (cargos == null && (Etapa.PrimeiraEtapa || permissoesAnalise.Any(x => x.Solicitante == true)))
            {
                analistas.Add(demandante);
            }

            // Se o demandante for Diretor e a etapa for para ser analisada por qualquer nível acima do demandante, os outros diretores serão os analistas.
            if (cargoDemandante.DiretoriaCargo() != null && cargoDemandante.DiretoriaCargo().Ativo && cargoDemandante.TipoCargo == EnumTipoCargo.Diretoria &&
                permissoesAnalise.Any(
                    x => x.ChefeImediato == true || x.GerenteAdjunto == true || x.DiretorCorrespondente == true))
            {
                AdicionarAnalistaAoRetorno(analistas, diretores.Where(x => x.Usuario.ID != demandante.ID).ToList(), cargos);
            }

            // Se for etapa para o Chefe Imediato, adiciona o chefe 1 nível acima do demandante.
            if (permissoesAnalise.Any(x => x.ChefeImediato == true))
            {
                UsuarioCargo chefeImediato = null;

                switch (cargoDemandante.TipoCargo)
                {
                    case EnumTipoCargo.Gabinete:
                    case EnumTipoCargo.Gerencia:
                        // Se o demandante for Chefe de Gabinete, retorna seu Diretor.
                        // Se o demandante for Gerente, retorna seu Chefe de Gabinete.
                        chefeImediato = cargoDemandante.ObterCargoPai(1);

                        break;
                    case EnumTipoCargo.GerenciaAdjunta:
                    case EnumTipoCargo.Funcionario:
                        // Se o demandante for Gerente Adjunto, retorna seus Gerentes.
                        // Se o demandante for Funcionário, retorna seus Gerentes Adjuntos.
                        var gerentes = cargoDemandante.CargoPai.UsuariosCargos;

                        if (gerentes.Any())
                            AdicionarAnalistaAoRetorno(analistas, gerentes, cargos);

                        break;
                }

                if (chefeImediato != null && chefeImediato.Cargo != null && chefeImediato.Cargo.DiretoriaCargo().Ativo)
                {
                    AdicionarAnalistaAoRetorno(analistas, chefeImediato, cargos);
                }
            }

            // Se for etapa para o Gerente Adjunto, adiciona o chefe 2 níveis acima do demandante.
            if (permissoesAnalise.Any(x => x.GerenteAdjunto == true))
            {
                UsuarioCargo gerenteAdjunto = null;

                switch (cargoDemandante.TipoCargo)
                {
                    case EnumTipoCargo.Gabinete:
                        // Se o demandante for Chefe de Gabinete, retorna os outros diretores que não são o seu Diretor.
                        var diretorGabinete = cargoDemandante.ObterCargoPai(1);

                        AdicionarAnalistaAoRetorno(analistas,
                            diretores.Where(x => x.Cargo.Ativo && diretorGabinete == null || (x.Usuario.ID != diretorGabinete.Usuario.ID))
                                .ToList(), cargos);

                        break;
                    case EnumTipoCargo.Gerencia:
                    case EnumTipoCargo.GerenciaAdjunta:
                        // Se o demandante for Gerente, retorna seu Diretor.
                        // Se o demandante for Gerente Adjunto, retorna seu Chefe de Gabinete.
                        gerenteAdjunto = cargoDemandante.ObterCargoPai(2);

                        break;
                    case EnumTipoCargo.Funcionario:
                        // Se o demandante for Funcionário, retorna seus Gerentes.
                        var gerentes = cargoDemandante.CargoPai.CargoPai.UsuariosCargos;

                        if (gerentes.Any())
                        {
                            AdicionarAnalistaAoRetorno(analistas, gerentes, cargos);
                        }

                        break;
                }

                if (gerenteAdjunto != null && gerenteAdjunto.Cargo != null && gerenteAdjunto.Cargo.DiretoriaCargo().Ativo)
                {
                    AdicionarAnalistaAoRetorno(analistas, gerenteAdjunto, cargos);
                }
            }

            // Se for etapa para o diretor correspondente, adiciona o diretor do setor do demandante.
            if (permissoesAnalise.Any(x => x.DiretorCorrespondente == true))
            {
                UsuarioCargo diretorCorrespondente = null;

                switch (cargoDemandante.TipoCargo)
                {
                    case EnumTipoCargo.Gabinete:
                        // Se o demandante for Chefe de Gabinete, retorna seu Diretor.
                        diretorCorrespondente = cargoDemandante.ObterCargoPai(1);

                        break;
                    case EnumTipoCargo.Gerencia:
                        // Se o demandante for Gerente, retorna seu Diretor.
                        diretorCorrespondente = cargoDemandante.ObterCargoPai(2);

                        break;
                    case EnumTipoCargo.GerenciaAdjunta:
                        // Se o demandante for Gerente Adjunto, retorna seu Diretor.
                        diretorCorrespondente = cargoDemandante.ObterCargoPai(3);

                        break;
                    case EnumTipoCargo.Funcionario:
                        //Se o demandante for Funcionário do Gabinete, retorna o Diretor e Chefe de Gabinete
                        if(cargoDemandante.CargoPai.TipoCargo == EnumTipoCargo.Gabinete)
                        {
                            //ADICIONA DIRETOR 
                            diretorCorrespondente = cargoDemandante.ObterCargoPai(2);

                            if (diretorCorrespondente != null)
                            {
                                AdicionarAnalistaAoRetorno(analistas, diretorCorrespondente, cargos);
                            }

                            //ADICIONA O CHEFE DE GABINETE
                            diretorCorrespondente = cargoDemandante.ObterCargoPai(1);
                        }
                        else
                        {
                            // Se o demandante for Funcionário, retorna seu Diretor.
                            diretorCorrespondente = cargoDemandante.ObterCargoPai(4);
                        }
                        break;
                }

                if (diretorCorrespondente != null && diretorCorrespondente.Cargo != null && diretorCorrespondente.Cargo.DiretoriaCargo().Ativo)
                {
                    AdicionarAnalistaAoRetorno(analistas, diretorCorrespondente, cargos);
                }

                // Caso possa ser aprovada por chefe de gabinete, obtem os chefes de gabinete do setor
                if (Etapa.PodeSerAprovadoChefeGabinete)
                {
                    UsuarioCargo chefeGabinete = null;

                    switch (cargoDemandante.TipoCargo)
                    {
                        case EnumTipoCargo.Gerencia:
                            // Se o demandante for Gerente, retorna seu Diretor.
                            chefeGabinete = cargoDemandante.ObterCargoPai(1);

                            break;
                        case EnumTipoCargo.GerenciaAdjunta:
                            // Se o demandante for Gerente Adjunto, retorna seu Diretor.
                            chefeGabinete = cargoDemandante.ObterCargoPai(2);

                            break;
                        case EnumTipoCargo.Funcionario:
                            // Se o demandante for Funcionário, retorna seu Diretor.
                            chefeGabinete = cargoDemandante.ObterCargoPai(3);
                            break;
                    }

                    if (chefeGabinete != null && chefeGabinete.Cargo != null && chefeGabinete.Cargo.DiretoriaCargo().Ativo)
                    {
                        AdicionarAnalistaAoRetorno(analistas, chefeGabinete, cargos);
                    }
                }
            }

            // Faz um agrupamento pelo ID e retorna somente o primeiro de cada agrupamento.
            // É uma forma de fazer um "DistinctBy" pelo ID para evitar analista duplicados.
            var retorno = new List<Usuario>();
            retorno.AddRange(analistas.GroupBy(x => x.ID).Select(x => x.FirstOrDefault()).OrderBy(x => x.Nome).ToList());

            if (PermissoesNucleoEtapaResposta.Any())
            {
                retorno.AddRange(PermissoesNucleoEtapaResposta.Where(x =>
                    x.EtapaPermissaoNucleo != null &&
                    x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario != null &&
                    x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo != null &&
                    x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.Ativo &&
                    x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario != null &&
                    x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.UF.ID == x.EtapaResposta.ProcessoResposta.Processo.Uf.ID)
                    .Select(x => x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario));
            }

            if (usuario != null && ListaEtapaEncaminhamentoUsuario.Any())
            {
                //ADICIONA USUARIOS COM ETAPA ENCAMINHADA PENDENTE
                var usuarioEtapaEncaminhamento = ListaEtapaEncaminhamentoUsuario
                    .Where(x => x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID == usuario.ID)
                    .Where(x => x.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aguardando)
                    .Select(x => x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario);
                retorno.AddRange(usuarioEtapaEncaminhamento);
            }

            return retorno;
        }

        public virtual bool ChefeImediato(Usuario chefe)
        {
            var cargoDemandante = ProcessoResposta.ObterUltimoCargoDemandante();
            UsuarioCargo chefeImediato = null;
            if (cargoDemandante != null)
            {
                switch (cargoDemandante.TipoCargo)
                {
                    case EnumTipoCargo.Gabinete:
                    case EnumTipoCargo.Gerencia:
                        // Se o demandante for Chefe de Gabinete, retorna seu Diretor.
                        // Se o demandante for Gerente, retorna seu Chefe de Gabinete.
                        chefeImediato = cargoDemandante.ObterCargoPai(1);

                        if (chefeImediato != null && chefeImediato.Usuario != null && chefeImediato.Usuario.ID == chefe.ID)
                        {
                            return true;
                        }

                        break;
                    case EnumTipoCargo.GerenciaAdjunta:
                    case EnumTipoCargo.Funcionario:
                        // Se o demandante for Gerente Adjunto, retorna seus Gerentes.
                        // Se o demandante for Funcionário, retorna seus Gerentes Adjuntos.
                        var gerentes = cargoDemandante.CargoPai.UsuariosCargos;

                        if (gerentes != null && gerentes.Any(x => x.Usuario.ID == chefe.ID))
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        private static void AdicionarAnalistaAoRetorno(List<Usuario> analistas, IList<UsuarioCargo> usuariosCargos , List<Cargo> cargos)
        {
            foreach (var usuarioCargo in usuariosCargos)
            {
                AdicionarAnalistaAoRetorno(analistas, usuarioCargo, cargos);
            }
        }

        private static void AdicionarAnalistaAoRetorno(List<Usuario> analistas, UsuarioCargo usuarioCargo, List<Cargo> cargos)
        {
            // Se cargo for nulo, procede pra adição do analista normalmente.
            if (cargos == null)
            {
                if (usuarioCargo.Cargo.DiretoriaCargo() != null && usuarioCargo.Cargo.DiretoriaCargo().Ativo)
                {
                    analistas.Add(usuarioCargo.Usuario);
                }
            }
            // Se cargo não for nulo, só adiciona o analista caso ele seja do cargo informado.
            else if (cargos.Any(x => x.DiretoriaCargo().Ativo && x.ID == usuarioCargo.Cargo.ID))
            {
                analistas.Add(usuarioCargo.Usuario);
            }
        }

        public virtual enumPrazoEncaminhamentoDemanda? ObterStatusEncaminhamento(DateTime? prazo)
        {
            if (prazo.HasValue)
            {
                if (prazo.Value.Date < DateTime.Now)
                    return enumPrazoEncaminhamentoDemanda.ForaDoPrazo;

                if ((prazo.Value.Date - DateTime.Today).Days <= 2)
                    return enumPrazoEncaminhamentoDemanda.AExpirar;
            }
            return enumPrazoEncaminhamentoDemanda.NoPrazo;
        }
    }
}