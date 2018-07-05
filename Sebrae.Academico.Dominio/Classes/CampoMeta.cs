using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CampoMeta : EntidadeBasica
    {
        public virtual byte? CampoTipoDado { get; set; }
        public virtual byte? CampoTipo { get; set; }
        public virtual string MetaNome { get; set; }
        public virtual string MetaKey { get; set; }
        public virtual string MetaDescription { get; set; }

        public virtual IList<CampoMetaValue> ListaMetaValues { get; set; }

        public CampoMeta()
        {
            this.ListaMetaValues = new List<CampoMetaValue>();
        }


        public virtual string FirstMetaValue
        {
            get
            {
                if (this.ListaMetaValues.Count() > 0)
                {
                    return this.ListaMetaValues.FirstOrDefault().MetaValue;
                }
                else
                {
                    return "";
                }
                
            }
        }


        public virtual string FirstMetaValueId
        {
            get
            {
                if (this.ListaMetaValues.Count() > 0)
                {
                    return this.ListaMetaValues.FirstOrDefault().ID.ToString();
                }
                else
                {
                    return "";
                }

            }
        }
    }
}
