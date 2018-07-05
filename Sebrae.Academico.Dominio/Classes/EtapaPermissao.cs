using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Attributes.TestValidationAttributes.EtapaPermissao;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EtapaPermissao : EntidadeBasica
    {
        [IsNotEtapaPermissaoField]
        public virtual Etapa Etapa { get; set; }

        [IsSingularEtapaPermissaoField]
        public virtual Perfil Perfil { get; set; }
        [IsSingularEtapaPermissaoField]
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        [IsSingularEtapaPermissaoField]
        public virtual Uf Uf { get; set; }

        [IsNotEtapaPermissaoField]
        public virtual bool? Notificar { get; set; }
        [IsNotEtapaPermissaoField]
        public virtual bool? Analisar { get; set; }

        [IsCompositeEtapaPermissaoField]
        public virtual Usuario Usuario { get; set; }

        [IsCompositeEtapaPermissaoField]
        public virtual bool? ChefeImediato { get; set; }
        [IsCompositeEtapaPermissaoField]
        public virtual bool? DiretorCorrespondente { get; set; }
        [IsCompositeEtapaPermissaoField]
        public virtual bool? GerenteAdjunto { get; set; }
        [IsCompositeEtapaPermissaoField]
        public virtual bool? Solicitante { get; set; }

        public virtual KeyValuePair<enumTipoEtapaPermissao, object> ObterTipoEtapaPermissao()
        {
            if (Perfil != null)
                return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.Perfil, Perfil.ID);

            if (NivelOcupacional != null)
                return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.NivelOcupacional,
                    NivelOcupacional.ID);

            if (Uf != null)
                return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.Uf, Uf.ID);

            if (Notificar == true)
            {
                if (Usuario != null)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.NotificarUsuario, Usuario.ID);

                if (Solicitante == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.NotificarSolicitante, Solicitante);

                if (ChefeImediato == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.NotificarChefeImediato, ChefeImediato);

                if (GerenteAdjunto == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.NotificarGerenteAdjunto, GerenteAdjunto);

                if (DiretorCorrespondente == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.NotificarDiretorCorrespondente, DiretorCorrespondente);
            }

            if (Analisar == true)
            {
                if (Usuario != null)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.AnalisarUsuario, Usuario.ID);

                if (Solicitante == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.AnalisarSolicitante, Solicitante);

                if (ChefeImediato == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.AnalisarChefeImediato, ChefeImediato);

                if (GerenteAdjunto == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.AnalisarGerenteAdjunto, GerenteAdjunto);

                if (DiretorCorrespondente == true)
                    return new KeyValuePair<enumTipoEtapaPermissao, object>(enumTipoEtapaPermissao.AnalisarDiretorCorrespondente, DiretorCorrespondente);
            }

            throw new InvalidCastException("Nova Permissão não implementada");
        }

        public virtual bool NaoAnalisavelPorNucleoUC()
        {
            return (Analisar.HasValue && Analisar.Value) && (
                   (Usuario != null) ||
                   (ChefeImediato.HasValue && ChefeImediato.Value) ||
                   (DiretorCorrespondente.HasValue && DiretorCorrespondente.Value) ||
                   (GerenteAdjunto.HasValue && GerenteAdjunto.Value) ||
                   (Solicitante.HasValue && Solicitante.Value));
        }
    }
}
