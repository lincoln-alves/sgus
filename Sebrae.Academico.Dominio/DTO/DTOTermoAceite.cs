using Sebrae.Academico.Dominio.Classes;
namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOTermoAceite : EntidadeBasica
    {
        public DTOTermoAceite()
        {
        }

        public DTOTermoAceite(TermoAceite t, int idSe, int idCategoria)
        {
            IdTermoAceite = t.ID;
            IdSolucaoEducacional = idSe;
            IdCategoriaConteudo = idCategoria;
            Nome = t.Nome;
            TextoTermoAceite = t.Texto;
            TextoPoliticaConsequencia = t.PoliticaConsequencia;
        }

        public int IdTermoAceite { get; set; }
        public int IdSolucaoEducacional { get; set; }
        public int IdCategoriaConteudo { get; set; }
        public string Nome { get; set; }
        public string TextoTermoAceite { get; set; }
        public string TextoPoliticaConsequencia { get; set; }
    }
}
