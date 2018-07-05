using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterHierarquiaNucleo : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMHierarquiaNucleo bmHierarquiaNucleo = null;

        #endregion

        #region "Construtor"

        public ManterHierarquiaNucleo()
            : base()
        {
            bmHierarquiaNucleo = new BMHierarquiaNucleo();
        }

        #endregion

        public HierarquiaNucleo ObterPorId(int idHierarquiaNucleo)
        {
            return bmHierarquiaNucleo.ObterPorId(idHierarquiaNucleo);
        }

        public IList<HierarquiaNucleo> ObterTodos()
        {
            return bmHierarquiaNucleo.ObterTodos().ToList();
        }

        public IList<HierarquiaNucleo> ObterPorUf(Uf uf)
        {
            return bmHierarquiaNucleo.ObterPorUf(uf).ToList();
        }

        public void Dispose()
        {
            bmHierarquiaNucleo.Dispose();
        }

        public void Salvar(HierarquiaNucleo hierarquiaNucleo)
        {
            bmHierarquiaNucleo.Salvar(hierarquiaNucleo);
        }


    }
}