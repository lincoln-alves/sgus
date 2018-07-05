using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EnvioInforme
    {
        public EnvioInforme()
        {
            
        }

        public EnvioInforme(Informe informe)
        {
            Informe = informe;
        }

        public virtual int ID { get; set; }

        public virtual Informe Informe { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual DateTime? DataEnvio { get; set; }

        public virtual bool Enviado()
        {
            return DataEnvio.HasValue;
        }

        public virtual IList<Perfil> Perfis { get; set; }

        public virtual IList<NivelOcupacional> NiveisOcupacionais { get; set; }

        public virtual IList<Uf> Ufs { get; set; }
    }
}
