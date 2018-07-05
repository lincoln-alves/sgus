using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MissaoMedalha
    {        
        public virtual int ID { get; set; }
        public virtual int Medalhas { get; set; }
        public virtual Missao Missao { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual DateTime DataRegistro { get; set; }
    }
}
