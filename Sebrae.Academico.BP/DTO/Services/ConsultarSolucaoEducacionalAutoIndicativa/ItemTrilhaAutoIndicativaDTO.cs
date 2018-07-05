
namespace Sebrae.Academico.BP.DTO.Services.ConsultarSolucaoEducacionalAutoIndicativa
{
    public class ItemTrilhaAutoIndicativaDTO
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string FormaAquisicao { get; set; }
        public virtual int idObjetivo { get; set; }
        public virtual string Objetivo { get; set; }
        public virtual string NomeIndicador { get; set; }
        public virtual string EmailIndicador { get; set; }
        public virtual int IDIndicador { get; set; }
        public virtual string DataIndicacao { get; set; }
        public string UFSigla { get; set; }
        public string UF { get; set; }
        public bool Participante { get; set; }
        public string Local { get; set; }
        public string ReferenciaBibliografica { get; set; }
        public string LinkAcesso { get; set; }
        public string StatusAprovacao { get; set; }

    }
}
