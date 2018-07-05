
namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class TrilhaAtividadeInformativaParticipacao: EntidadeBasicaPorId
    {
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual TrilhaTopicoTematico TrilhaTopicoTematico { get; set; }

        public virtual string Status { get; set; }
        public virtual string TextoParticipacao { get; set; }
        public virtual string ArquivoParticipacao { get; set; }
        
        //public override int GetHashCode()
        //{
        //    return UsuarioTrilha.ID.GetHashCode() + TrilhaTopicoTematico.ID.GetHashCode();
        //}


    }
}
