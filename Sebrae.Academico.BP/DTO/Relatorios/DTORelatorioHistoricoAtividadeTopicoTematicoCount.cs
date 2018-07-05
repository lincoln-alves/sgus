
namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioHistoricoAtividadeTopicoTematicoCount
    {
        public int IDTopicoTematico { get; set; }
        public string TopicoTomatico { get; set; }
        public string Objetivo { get; set; }

        public virtual bool IsAutoIndicativa { get; set; }
        public virtual bool TemParticipacaoNaSolucao { get; set; }
        public virtual bool TemParticipacaoNoTopico { get; set; }
    }
}
