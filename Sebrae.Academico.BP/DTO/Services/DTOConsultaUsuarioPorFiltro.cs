using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOConsultaUsuarioPorFiltro : RetornoWebService
    {
        public DTOConsultaUsuarioPorFiltro()
        {
            ListaConsultaUsuario = new List<DTOConsultaUsuarioPorFiltroItem>();
        }
        public List<DTOConsultaUsuarioPorFiltroItem> ListaConsultaUsuario { get; set; }        
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int totalRegistros { get; set; }
    }

    public class DTOConsultaUsuarioPorFiltroItem
    {
        public string Nome { get; set; }
        public string Situacao { get; set; }
        public string SenhaMD5 { get; set; }
        public string Sexo { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Email { get; set; }
        public string SID_Usuario { get; set; }
        public string CPF { get; set; }
    }
}
