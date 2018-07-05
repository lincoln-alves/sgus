using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterPublicoAlvo : BusinessProcessBase
    {
        private BMPublicoAlvo bmPublicoAlvo = null;

        public ManterPublicoAlvo()
        {
            bmPublicoAlvo = new BMPublicoAlvo();
        }
		
		public PublicoAlvo ObterPorID(int id)
		{
			return bmPublicoAlvo.ObterPorID(id);
		}
		
		public void Salvar(PublicoAlvo publicoAlvo)
		{
			bmPublicoAlvo.Salvar(publicoAlvo);
		}
    }
}