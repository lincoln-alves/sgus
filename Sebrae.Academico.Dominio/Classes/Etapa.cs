using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sebrae.Academico.Dominio.Attributes.TestValidationAttributes.EtapaPermissao;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Etapa : EntidadeBasica
    {
        public Etapa()
        {
            ListaEtapaResposta = new List<EtapaResposta>();
            ListaCampos = new List<Campo>();
            Permissoes = new List<EtapaPermissao>();
        }

        public virtual int OriginalID { get; set; }
        public virtual int? EtapaRetornoOriginalID { get; set; }

        public virtual Processo Processo { get; set; }
        public virtual bool RequerAprovacao { get; set; }
        public virtual Etapa EtapaRetorno { get; set; }
        public virtual byte Ordem { get; set; }
        public virtual IList<EtapaResposta> ListaEtapaResposta { get; set; }
        public virtual IList<Campo> ListaCampos { get; set; }
        public virtual bool PrimeiraEtapa { get; set; }
        public virtual bool VisivelImpressao { get; set; }
        public virtual Usuario UsuarioAssinatura { get; set; }
        public virtual FileServer FileServer { get; set; }
        public virtual NomeFinalizacaoEtapa NomeFinalizacaoEtapa { get; set; }
        public virtual string NomeBotaoAjuste { get; set; }
        public virtual string NomeBotaoFinalizacao { get; set; }
        public virtual NomeReprovacaoEtapa NomeReprovacaoEtapa { get; set; }
        public virtual string NomeBotaoReprovacao { get; set; }
        public virtual bool PodeSerReprovada { get; set; }
        public virtual bool PodeSerAprovadoChefeGabinete { get; set; }
        public virtual bool NotificaDiretorAnalise { get; set; }
        public virtual bool NotificarNucleo { get; set; }
        // Prazo para encaminhamento em dias
        public virtual int? PrazoEncaminhamento { get; set; }
        public virtual IList<EtapaPermissao> Permissoes { get; set; }
        public virtual IList<EtapaPermissaoNucleo> PermissoesNucleo { get; set; }

        public virtual void AdicionarPermissao(EtapaPermissao permissao)
        {
            if (Permissoes.Any() == false || PermissaoExiste(permissao, Permissoes.ToList()) == false)
            {
                Permissoes.Add(permissao);
            }
        }

        /// <summary>
        /// Remove a permissão, assumindo que a Etapa só terá uma permissão daquele tipo e valor.
        /// </summary>
        public virtual void RemoverPermissao(KeyValuePair<enumTipoEtapaPermissao, object> permissaoTipoValor)
        {
            var permissaoRemover =
                Permissoes.FirstOrDefault(
                    x => TipoKeyValueEqual(x.ObterTipoEtapaPermissao(), permissaoTipoValor));

            if (permissaoRemover != null)
            {
                Permissoes.Remove(permissaoRemover);
            }
        }

        public static bool TipoKeyValueEqual(KeyValuePair<enumTipoEtapaPermissao, object> origem, KeyValuePair<enumTipoEtapaPermissao, object> destino)
        {
            return origem.Key == destino.Key && origem.Value.Equals(destino.Value);
        }

        public virtual string ObterNomeFinalizacaoBotao()
        {
            return NomeFinalizacaoEtapa != null ? NomeFinalizacaoEtapa.Nome : (NomeBotaoFinalizacao ?? "Aprovar");
        }

        public virtual string ObterNomeReprovacaoBotao()
        {
            return NomeReprovacaoEtapa != null ? NomeReprovacaoEtapa.Nome : (NomeBotaoReprovacao ?? "Reprovar");
        }

        public virtual DateTime? ObterPrazoParaEncaminhamentoDaDemanda(DateTime? dataUltimaIteracao)
        {
            return PrazoEncaminhamento != null
                ? dataUltimaIteracao.Value.AddDays(PrazoEncaminhamento.Value)
                : dataUltimaIteracao.Value;
        }

        /// <summary>
        /// Verificar se a permissão existe na listagem de permissões de 
        /// </summary>
        /// <returns></returns>
        private bool PermissaoExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            // Verificar dinamicamente as permissões SIMPLES.
            if (PermissaoNivelOcupacionalExiste(permissao, permissoes))
                return true;

            if (PermissaoUfExiste(permissao, permissoes))
                return true;

            if (PermissaoPerfilExiste(permissao, permissoes))
                return true;

            // Verificar dinamicamente as permissões COMPOSTAS.
            if (PermissaoUsuarioAnalisarExiste(permissao, permissoes))
                return true;

            if (PermissaoUsuarioNotificarExiste(permissao, permissoes))
                return true;

            if (PermissaoChefeImediatoAnalisarExiste(permissao, permissoes))
                return true;

            if (PermissaoChefeImediatoNotificarExiste(permissao, permissoes))
                return true;

            if (PermissaoDiretorCorrespondenteAnalisarExiste(permissao, permissoes))
                return true;

            if (PermissaoDiretorCorrespondenteNotificarExiste(permissao, permissoes))
                return true;

            if (PermissaoGerenteAdjuntoAnalisarExiste(permissao, permissoes))
                return true;

            if (PermissaoGerenteAdjuntoNotificarExiste(permissao, permissoes))
                return true;

            if (PermissaoSolicitanteAnalisarExiste(permissao, permissoes))
                return true;

            if (PermissaoSolicitanteNotificarExiste(permissao, permissoes))
                return true;

            return false;
        }

        // Implementações singulares.

        [ShouldImplementOneEtapaPermissaoField]
        private bool PermissaoNivelOcupacionalExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.NivelOcupacional != null &&
                   PermissaoComplexaExiste(nameof(permissao.NivelOcupacional), nameof(permissao.NivelOcupacional.ID),
                       permissao.NivelOcupacional.ID, permissoes);
        }

        [ShouldImplementOneEtapaPermissaoField]
        private bool PermissaoUfExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Uf != null &&
                   PermissaoComplexaExiste(nameof(permissao.Uf), nameof(permissao.Uf.ID), permissao.Uf.ID, permissoes);
        }

        [ShouldImplementOneEtapaPermissaoField]
        private bool PermissaoPerfilExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Perfil != null &&
                   PermissaoComplexaExiste(nameof(permissao.Perfil), nameof(permissao.Perfil.ID), permissao.Perfil.ID, permissoes);
        }

        // Implementações duplas.

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoUsuarioAnalisarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Analisar == true && permissao.Usuario != null &&
                   PermissaoAnalisarComplexaExiste(nameof(permissao.Usuario), nameof(permissao.Usuario.ID),
                       permissao.Usuario.ID, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoUsuarioNotificarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Notificar == true && permissao.Usuario != null &&
                   PermissaoNotificarComplexaExiste(nameof(permissao.Usuario), nameof(permissao.Usuario.ID),
                       permissao.Usuario.ID, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoChefeImediatoAnalisarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Analisar == true && permissao.ChefeImediato != null &&
                   PermissaoSimplesAnalisarExiste(nameof(permissao.ChefeImediato), permissao.ChefeImediato, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoChefeImediatoNotificarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Notificar == true && permissao.ChefeImediato != null &&
                   PermissaoSimplesNotificarExiste(nameof(permissao.ChefeImediato), permissao.ChefeImediato, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoDiretorCorrespondenteAnalisarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Analisar == true && permissao.DiretorCorrespondente != null &&
                   PermissaoSimplesAnalisarExiste(nameof(permissao.DiretorCorrespondente), permissao.DiretorCorrespondente, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoDiretorCorrespondenteNotificarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Notificar == true && permissao.DiretorCorrespondente != null &&
                   PermissaoSimplesNotificarExiste(nameof(permissao.DiretorCorrespondente), permissao.DiretorCorrespondente, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoGerenteAdjuntoAnalisarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Analisar == true && permissao.GerenteAdjunto != null &&
                   PermissaoSimplesAnalisarExiste(nameof(permissao.GerenteAdjunto), permissao.GerenteAdjunto, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoGerenteAdjuntoNotificarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Notificar == true && permissao.GerenteAdjunto != null &&
                   PermissaoSimplesNotificarExiste(nameof(permissao.GerenteAdjunto), permissao.GerenteAdjunto, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoSolicitanteAnalisarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Analisar == true && permissao.Solicitante != null &&
                   PermissaoSimplesAnalisarExiste(nameof(permissao.Solicitante), permissao.Solicitante, permissoes);
        }

        [ShouldImplementTwoEtapaPermissaoField]
        private bool PermissaoSolicitanteNotificarExiste(EtapaPermissao permissao, List<EtapaPermissao> permissoes)
        {
            return permissao.Notificar == true && permissao.Solicitante != null &&
                   PermissaoSimplesNotificarExiste(nameof(permissao.Solicitante), permissao.Solicitante, permissoes);
        }

        private bool PermissaoNotificarComplexaExiste(string campo, string id, object valor, List<EtapaPermissao> permissoes)
        {
            return PermissaoComplexaExiste(campo, id, valor, permissoes, "Notificar");
        }

        private bool PermissaoAnalisarComplexaExiste(string campo, string id, object valor, List<EtapaPermissao> permissoes)
        {
            return PermissaoComplexaExiste(campo, id, valor, permissoes, "Analisar");
        }

        private bool PermissaoComplexaExiste(string campo, string id, object valor, List<EtapaPermissao> permissoes, string segundoCampo = null)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var permissao in ObterPermissoesPorCampo(campo, permissoes))
            {
                var campoProperty =
                    permissao.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(x => x.Name == campo);

                if (campoProperty == null)
                    continue;

                var complexObject = campoProperty.GetValue(permissao);

                var idProperty =
                    complexObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(x => x.Name == id);

                if (idProperty == null)
                    continue;

                var idValue = idProperty.GetValue(complexObject);

                if (idValue.Equals(valor) && (segundoCampo == null || ValorSimplesIsEqual(segundoCampo, true, permissao)))
                    return true;
            }

            return false;
        }

        private bool PermissaoSimplesAnalisarExiste(string campo, object valor, List<EtapaPermissao> permissoes)
        {
            return PermissaoSimplesExiste(campo, valor, permissoes, "Analisar");
        }

        private bool PermissaoSimplesNotificarExiste(string campo, object valor, List<EtapaPermissao> permissoes)
        {
            return PermissaoSimplesExiste(campo, valor, permissoes, "Notificar");
        }

        private bool PermissaoSimplesExiste(string campo, object valor, List<EtapaPermissao> permissoes, string segundoCampo = null)
        {
            var retorno =
                ObterPermissoesPorCampo(campo, permissoes).Any(
                    p =>
                        ValorSimplesIsEqual(campo, valor, p) &&
                        (segundoCampo == null || ValorSimplesIsEqual(segundoCampo, true, p)));

            return retorno;
        }

        private static bool ValorSimplesIsEqual(string campo, object valor, EtapaPermissao p)
        {
            var value = p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(x => x.Name == campo)?
                .GetValue(p);

            if (value == null)
                return false;

            return value.Equals(valor);
        }

        private IEnumerable<EtapaPermissao> ObterPermissoesPorCampo(string campo, List<EtapaPermissao> permissoes)
        {
            var retorno = from permissao in permissoes
                          let props = permissao.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          let propCampo = props.FirstOrDefault(x => x.Name == campo)
                          where propCampo?.GetValue(permissao) != null
                          select permissao;

            return retorno;
        }
    }
}