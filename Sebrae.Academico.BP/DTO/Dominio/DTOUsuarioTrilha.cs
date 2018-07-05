using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOUsuarioTrilha : DTOEntidadeBasicaPorId
    {
        public DTOUsuarioTrilha()
        {
            Usuario = new DTOUsuario();
            Trilha = new DTOTrilha();
            TrilhaNivel = new DTOTrilhaNivel();
            Uf = new DTOUf();
            StatusMatricula = new DTOStatusMatriculaDominio();
            NivelOcupacional = new DTONivelOcupacional();
        }

        public virtual DTOUsuario Usuario { get; set; }
        public virtual DTOTrilha Trilha { get; set; }
        public virtual DTOTrilhaNivel TrilhaNivel { get; set; }
        public virtual DTOUf Uf { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataLimite { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual decimal? NotaProva { get; set; }
        public virtual DTOStatusMatriculaDominio StatusMatricula { get; set; }
        public virtual string NomeUsuarioAtualizacao { get; set; }
        public virtual DTONivelOcupacional NivelOcupacional { get; set; }
       
        //public virtual List<DTOItemTrilhaParticipacao> ListaItemTrilhaParticipacao { get; set; }
        //public virtual List<DTOTrilhaAtividadeInformativaParticipacao> ListaTrilhaAtividadeInformativaParticipacao { get; set; }
    }
}
