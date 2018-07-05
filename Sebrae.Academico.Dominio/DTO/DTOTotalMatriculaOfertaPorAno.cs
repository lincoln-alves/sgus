namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações 
    /// sobre totais de matriculas em ofertas por ano
    /// </summary>
    public class DTOTotalMatriculaOfertaPorAno
    {
        public virtual string SiglaUf { get; set; }
        public virtual int Quantidade { get; set; }
    }
}
