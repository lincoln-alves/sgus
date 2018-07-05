using System;

namespace Sebrae.Academico.Dominio.Classes.SGC
{
    public class CredenciadoArea
    {
        public virtual int ID { get; set; }
        public virtual string CPF { get; set; }
        public virtual string CodigoVinculo { get; set; }
        public virtual string DescricaoVinculo { get; set; }
        public virtual string CodigoNatureza { get; set; }
        public virtual string DescricaoNatureza { get; set; }
        public virtual DateTime? Alteracao { get; set; }

        public virtual Area Area { get; set; }
        public virtual Subarea Subarea { get; set; }
    }
}
