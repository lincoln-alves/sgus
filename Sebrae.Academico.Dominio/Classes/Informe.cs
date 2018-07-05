using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Informe
    {
        public virtual int ID { get; set; }
        public virtual int Numero { get; set; }
        public virtual int Mes { get; set; }
        public virtual int Ano { get; set; }

        public virtual string UltimoEnvio
        {
            get
            {
                var maiorData = Envios.Max(x => x.DataEnvio);

                return maiorData.HasValue ? maiorData.Value.ToShortDateString() : "";
            }
        }

        public virtual string ObterMesAno()
        {
            return Mes.ToString().PadLeft(2, '0') + "/" + Ano;
        }

        public virtual IList<Turma> Turmas { get; set; }

        public virtual IList<EnvioInforme> Envios { get; set; }
    }
}
