using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;


namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOEtapaEncaminhamentoUsuario
    {
        public virtual int ID_EtapaEncamihamentoUsuario { get; set; }
        public virtual int ID_EtapaResposta { get; set; }
        public virtual DTOEtapaPermissaoNucleo EtapaPermissaoNucleo { get; set; }
        public virtual DTOUsuario UsuarioEncaminhamento { get; set; }

        public virtual enumStatusEncaminhamentoEtapa StatusEncaminhamento { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? DataSolicitacaoEncaminhamento { get; set; }
        public virtual string Justificativa { get; set; }
    }
}
