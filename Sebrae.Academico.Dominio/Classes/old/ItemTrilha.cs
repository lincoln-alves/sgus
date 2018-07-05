
namespace Sebrae.Academico.Trilhas.Dominio.Classes
{

    public class ItemTrilha : EntidadeBasicaPorId
    {
        //Todo -> Trocar este Id pela entidade referente a SolucaoEducacional (SgusSolucaoEducacional)
        public virtual int CodSolucaoEducacional { get; set; }

        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual TrilhaTopicoTematico TrilhaTopicoTematico { get; set; }
        public virtual TrilhaFormaAprendizagem TrilhaFormaAprendizagem { get; set; }
        public virtual Trilha Trilha { get; set; }
        public virtual decimal PontosParticipacao { get; set; }

    }


}
