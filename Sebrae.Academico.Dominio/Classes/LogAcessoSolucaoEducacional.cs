using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcessoSolucaoEducacional
    {
        public virtual int ID { get; set; }
        public virtual int ID_SolucaoEducacional { get; set; }
        public virtual int ID_Oferta { get; set; }
        public virtual int ID_Turma { get; set; }
        public virtual DateTime DataAcesso { get; set; }
        public virtual int QuantidadeDeAcessos { get; set; }
    }
}
