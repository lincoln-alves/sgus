using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MonitoramentoIndicadoresValores : EntidadeBasicaPorId
    {
        public virtual MonitoramentoIndicadores MonitoramentoIndicador { get; set; }
        public virtual string Registro { get; set; }
        public virtual string Descricao { get; set; }
    }
}