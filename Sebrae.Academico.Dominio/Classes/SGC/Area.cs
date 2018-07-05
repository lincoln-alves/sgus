using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes.SGC
{
    public class Area
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string CodigoSituacao { get; set; }
        public virtual string DescricaoSituacao { get; set; }
        public virtual DateTime? Alteracao { get; set; }

        public virtual IList<Subarea> Subareas { get; set; }
        public virtual IList<CredenciadoArea> CredenciadoArea { get; set; }
    }
}
