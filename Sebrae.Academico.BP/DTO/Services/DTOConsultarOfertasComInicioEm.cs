using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOConsultarOfertasPorPeriodo
    {
        public DTOConsultarOfertasPorPeriodo()
        {
            DataConsulta = DateTime.Today;
            ListaDadosOferta = new List<DTODadosOferta>();
        }

        public DateTime DataConsulta { get; set; }
        public List<DTODadosOferta> ListaDadosOferta { get; set; }
    }

    public class DTODadosOferta
    {
        public DTODadosOferta()
        {
            Responsavel = new DTODadosResponsavel();
            ListaDadosMatriculados = new List<DTODadosMatriculados>();
        }

        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public DTODadosResponsavel Responsavel { get; set; }
        public virtual string AcessoWifi { get; set; }
        public List<DTODadosMatriculados> ListaDadosMatriculados { get; set; }
    }

    public class DTODadosResponsavel
    {
        public virtual string CPF { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string UF { get; set; }
    }

    public class DTODadosMatriculados
    {
        public string Nome { get; set; }
        public string  CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Email { get; set; }
        public string UF { get; set; }
        public string Matricula { get; set; }
    }
}
