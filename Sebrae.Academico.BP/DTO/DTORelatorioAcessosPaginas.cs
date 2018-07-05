using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.BP.DTO
{
    public class DTORelatorioAcessosPaginas
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Pagina { get; set; }
        public string Acao { get; set; }
        public DateTime Acesso { get; set; }
        public int Quantidade { get; set; }
    }
}
