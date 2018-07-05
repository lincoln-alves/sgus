
namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações 
    /// sobre matriculas em oferta no ano.
    /// </summary>
    public class DTOMatriculaOfertaNoAno
    {
        public virtual int QuantidadeMatriculados { get; set; }
        public virtual string NomeFormaAquisicao { get; set; }
    }
}
