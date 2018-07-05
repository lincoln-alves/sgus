using System.Linq;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.Loja
{
    public class DTOSolucaoTrilheiro
    {
        public int IdTrilhaNivel { get; set; }
        public int IdLoja { get; set; }
        public int MissaoId { get; set; }
        public int IdTipo { get; set; }
        public string Nome { get; set; }
        public string LinkConteudo { get; set; }
        public string ReferenciaBibliografica { get; set; }
        public string Orientacao { get; set; }
        public string NomeDoArquivoOriginal { get; set; }
        public string Arquivo { get; set; }
        public string CargaHoraia { private get; set; }


        public int GetCargaHoriaEmMinutos ()
        {
            var list = CargaHoraia.Split(':').ToList();

            int hora;
            int minutos;

            int.TryParse(list.FirstOrDefault(), out hora);
            int.TryParse(list.LastOrDefault(), out minutos);

            return (hora * 60) + minutos;
        }
    }
}
