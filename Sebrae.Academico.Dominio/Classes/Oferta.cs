using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Oferta : EntidadeBasica
    {
        public virtual string NomeExibicao
        {
            get
            {
                if (!string.IsNullOrEmpty(Codigo) && !string.IsNullOrEmpty(Nome))
                    return string.Format("{0} - {1}", Nome, Codigo);

                if (!string.IsNullOrEmpty(Nome))
                    return string.Format("{0}", Nome);

                return "-";
            }
        }

        public virtual string NomeSalvo { get; set; }

        public virtual enumTipoOferta TipoOferta { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual bool FiladeEspera { get; set; }
        public virtual DateTime? DataInicioInscricoes { get; set; }
        public virtual DateTime? DataFimInscricoes { get; set; }
        public virtual bool? InscricaoOnline { get; set; }
        public virtual bool? MatriculaGestorUC { get; set; }
        public virtual bool? AlteraPeloGestorUC { get; set; }
        public virtual bool? PermiteCadastroTurmaPeloGestorUC { get; set; }
        public virtual double? ValorPrevisto { get; set; }
        public virtual double? ValorRealizado { get; set; }
        public virtual int? CodigoMoodle { get; set; }
        public virtual int? DiasPrazo { get; set; }
        public virtual int? Sequencia { get; set; }
        public virtual int? IdNodePortal { get; set; }
        public virtual string InformacaoAdicional { get; set; }

        public virtual int QuantidadeMaximaInscricoes { get; set; }

        public virtual string EmailResponsavel { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual string Link { get; set; }
        public virtual enumDistribuicaoVagasOferta? DistribuicaoVagas { get; set; }

        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual IList<MatriculaOferta> ListaMatriculaOferta { get; set; }
        public virtual IList<OfertaPermissao> ListaPermissao { get; set; }
        public virtual IList<Turma> ListaTurma { get; set; }
        public virtual CertificadoTemplate CertificadoTemplate { get; set; }
        public virtual CertificadoTemplate CertificadoTemplateProfessor { get; set; }
        public virtual IList<DTOfertaPermissao> ListaUsuariosPermitidos { get; set; }

        public virtual IList<OfertaGerenciadorVaga> ListaOfertaGerenciadorVaga { get; set; }

        public virtual IList<OfertaPublicoAlvo> ListaPublicoAlvo { get; set; }

        public virtual IList<NivelOcupacional> ListaNiveisTrancados { get; set; }

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public Oferta()
        {
            ListaPermissao = new List<OfertaPermissao>();
            ListaUsuariosPermitidos = new List<DTOfertaPermissao>();
            ListaPublicoAlvo = new List<OfertaPublicoAlvo>();
            ListaNiveisTrancados = new List<NivelOcupacional>();
        }

        /// <summary>
        /// Atualiza a lista de NiveisOcupacionais do Programa.
        /// </summary>
        /// <param name="nivelOcupacional"></param>
        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            var listaNivelOcupacional =
                ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional { ID = x.NivelOcupacional.ID });

            var ofertaPermissao = new OfertaPermissao { NivelOcupacional = nivelOcupacional, Oferta = this };

            //Antes de adicionar, verifica se já existe na lista
            if (listaNivelOcupacional.All(x => x.ID != nivelOcupacional.ID))
            {
                ListaPermissao.Add(ofertaPermissao);
            }
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            var listaNivelOcupacional =
                ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional { ID = x.NivelOcupacional.ID });

            if (listaNivelOcupacional.Any(x => x.ID == nivelOcupacional.ID))
            {
                var permissao =
                    ListaPermissao.FirstOrDefault(
                        x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID);

                ListaPermissao.Remove(permissao);
            }
        }

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            var listaPerfils =
                ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil { ID = x.Perfil.ID, Nome = x.Perfil.Nome });

            var ofertaPermissao = new OfertaPermissao { Perfil = perfil, Oferta = this };

            //Antes de adicionar, verifica se já existe na lista
            if (listaPerfils.All(x => x.ID != perfil.ID))
                ListaPermissao.Add(ofertaPermissao);
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            var listaPerfil =
                ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil { ID = x.Perfil.ID, Nome = x.Perfil.Nome });

            if (listaPerfil.Any(x => x.ID == perfil.ID))
                ListaPermissao.Remove(ListaPermissao.FirstOrDefault(x => x.Perfil != null && x.Perfil.ID == perfil.ID));
        }

        public virtual void AdicionarUfs(OfertaPermissao permissao)
        {
            var listaUfs =
                ListaPermissao.Where(x => x.Uf != null).Select(x => new Uf { ID = x.Uf.ID });

            //Antes de adicionar, verifica se já existe na lista.
            if (listaUfs.All(x => x.ID != permissao.Uf.ID))
            {
                ListaPermissao.Add(permissao);
            }
        }

        public virtual void RemoverUf(Uf uf)
        {
            var listaUfs =
                ListaPermissao.Where(x => x.Uf != null).Select(x => new Uf { ID = x.Uf.ID });

            if (listaUfs.Any(x => x.ID == uf.ID))
            {
                ListaPermissao.Remove(ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == uf.ID));
            }
        }

        public virtual void AdicionarTrancamento(int idNivelOcupacional)
        {
            //Antes de adicionar, verifica se já existe na lista.
            if (ListaNiveisTrancados.All(x => x.ID != idNivelOcupacional))
                ListaNiveisTrancados.Add(new NivelOcupacional
                {
                    ID = idNivelOcupacional
                });
        }

        public virtual void RemoverTrancamento(int idNivelOcupacional)
        {
            if (ListaNiveisTrancados.Any(x => x.ID == idNivelOcupacional))
                ListaNiveisTrancados.Remove(
                    ListaNiveisTrancados.FirstOrDefault(
                        x => x.ID == idNivelOcupacional));
        }

        public virtual string NomeSolucaoEducacional
        {
            get
            {
                var solucaoEducacional = string.Empty;

                if (SolucaoEducacional != null && SolucaoEducacional.ID > 0)
                {
                    solucaoEducacional = SolucaoEducacional.Nome;
                }

                return solucaoEducacional;
            }
        }

        public virtual string Codigo
        {
            get
            {
                var id = Sequencia != null ? Sequencia.Value : ID;

                if (SolucaoEducacional != null && SolucaoEducacional.DescricaoSequencial != "-")
                    return string.Format("{0}.OF{1}",
                        SolucaoEducacional.DescricaoSequencial,
                        id);

                return "N/D";
            }
        }

        public virtual string DescricaoSequencial
        {
            get
            {
                var id = Sequencia != null ? Sequencia.Value : ID;

                if (SolucaoEducacional != null && SolucaoEducacional.DescricaoSequencial != "-")
                    return string.Format("{1}.OF{2} - {0}",
                        Nome,
                        SolucaoEducacional.DescricaoSequencial,
                        id);

                return "N/D";
            }
        }

        /// <summary>
        /// Obter quantas vagas ainda estão disponíveis para esta oferta.
        /// </summary>
        /// <param name="uf">UF que será utilizado para buscar a quantidade de vagas.</param>
        /// <returns></returns>
        public virtual int ObterVagasDisponiveis(Uf uf = null)
        {
            return ObterQuantidadeVagas(uf) -
                   ListaMatriculaOferta.Count(
                       mo =>
                           !mo.IsCancelado() &&
                           (DistribuicaoVagas != enumDistribuicaoVagasOferta.VagasPorUf || mo.UF == uf));
        }

        /// <summary>
        /// Obtém a quantidade total de vagas nesta oferta. Caso o usuário seja informado, busca
        /// a quantidade de vagas informada nas permissões e para a UF do usuário informado.
        /// </summary>
        /// <param name="uf">UF será utilizado para buscar a quantidade de vagas.</param>
        /// <returns></returns>
        public virtual int ObterQuantidadeVagas(Uf uf = null)
        {
            OfertaPermissao permissao;

            return DistribuicaoVagas == enumDistribuicaoVagasOferta.VagasPorUf && uf != null &&
                   (permissao =
                       ListaPermissao.FirstOrDefault(
                           p => p.Uf != null && p.Uf.ID == uf.ID && p.QuantidadeVagasPorEstado > 0)) != null
                ? permissao.QuantidadeVagasPorEstado
                : QuantidadeMaximaInscricoes;
        }

        /// <summary>
        /// Verificar se o usuário tem as permissões de UF, Nível Ocupacional e Perfil para se inscrever nesta oferta.
        /// </summary>
        /// <param name="usuario">Usuário a ser verificado.</param>
        /// <returns>True: usuário pode se inscrever. False: usuário não pode se inscrever.</returns>
        public virtual bool UsuarioPossuiPermissao(Usuario usuario)
        {
            var listaPerfil = usuario.ListaPerfil.Select(x => x.Perfil.ID).ToList();

            var permissaoPerfil = ListaPermissao.Any(x => x.Perfil != null && listaPerfil.Contains(x.Perfil.ID));

            var permissaoUf = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == usuario.UF.ID);

            var permissaoNivel = ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == usuario.NivelOcupacional.ID);

            return permissaoPerfil && permissaoUf && permissaoNivel;
        }

        /// <summary>
        /// Retorna true caso a oferta possua matrículas abertas. False caso esteja 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAbertaParaInscricoes()
        {
            return (DataInicioInscricoes == null || DataInicioInscricoes.Value <= DateTime.Now) &&
                   (DataFimInscricoes == null || DataFimInscricoes.Value >= DateTime.Now);
        }

        public virtual bool IsPrazoConclusao()
        {
            return (DataFimInscricoes == null || DataFimInscricoes.Value.AddDays((double) (DiasPrazo ?? 0)) >= DateTime.Now);
        }

        public virtual bool PossuiTurmasDisponiveis()
        {
            return ListaTurma.Any(x => (!x.Status.HasValue || x.Status != enumStatusTurma.Cancelada) && x.InAberta);
        }
    }
}
