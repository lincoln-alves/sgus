using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOQuestionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? PrazoMinutos { get; set; }
        public int? QtdQuestoesProva { get; set; }
        public int? NotaMinima { get; set; }
        public string TextoEnunciado { get; set; }
        public bool? Ativo { get; set; }
        public DTOUf Uf { get; set; }

        public int? FormaAquisicaoId { get; set; }
        public DTOTipoQuestionario TipoQuestionario { get; set; }
        public List<int> ListaPerfil { get; set; }
        public List<int> ListaNivelOcupacional { get; set; }
        public List<int> ListaUf { get; set; }
        public List<DTOCategoriaConteudo> ListaCategoriaConteudo { get; set; }
        public List<DTOItemQuestionario> ListaItemQuestionario { get; set; }
        public List<DTOQuestionarioPermissao> ListaQuestionarioPermissao { get; set; }
    }
}
