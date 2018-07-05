using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CampoResposta : EntidadeBasica
    {
        public virtual Campo Campo { get; set; }
        public virtual string Resposta { get; set; }
        public virtual EtapaResposta EtapaResposta { get; set; }
        //public virtual IList<AlternativaResposta> ListaAlternativasRespostas { get; set; }


        public CampoResposta()
        {
            this.Campo = new Campo();
            this.EtapaResposta = new EtapaResposta();
            //ListaAlternativasRespostas = new List<AlternativaResposta>();
        }
    }
}
