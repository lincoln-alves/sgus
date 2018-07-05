using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.Services
{
    public class VersaoAttribute : Attribute
    {
        public string version;

        public VersaoAttribute(string version)
        {
            this.version = version;
        }
    }
}
