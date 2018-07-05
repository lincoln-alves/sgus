using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MonitoramentoIndicadores: EntidadeBasicaPorId
    {
        public virtual int Ano { get; set; }
        public virtual IList<MonitoramentoIndicadoresValores> ListaValores { get; set; }

        public MonitoramentoIndicadores(){
            ListaValores = new List<MonitoramentoIndicadoresValores>();
        }
    }
}