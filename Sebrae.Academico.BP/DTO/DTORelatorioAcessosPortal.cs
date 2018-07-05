using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO
{
    public class DTORelatorioAcessosPortal
    {
        public Usuario Usuario { get; set; }
        public string Nome { get { return Usuario != null ? Usuario.Nome : "SISTEMA" ; } }
        public string CPF { get { return Usuario != null ? Usuario.CPF : "-"; }  }
        public string UF { get { return Usuario != null ? Usuario.UF.Nome : "-" ; } }
        public string Pagina { get; set; }
        public string Acao { get; set; }
        public string IP { get; set; }
        public DateTime Acesso { get; set; }
        public int Quantidade { get; set; }
    }
}
