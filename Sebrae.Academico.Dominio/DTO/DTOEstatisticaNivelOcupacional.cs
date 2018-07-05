namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações de estatísticas
    /// </summary>
    public class DTOEstatisticaNivelOcupacional
    {
        public virtual string NomeNivelOcupacional { get; set; }
        public virtual int QuantidadeAtivos { get; set; }
        public virtual int QuantidadeAtivosComInscricao { get; set; }
        public virtual double Percentual { get; set; }        
    }
}
