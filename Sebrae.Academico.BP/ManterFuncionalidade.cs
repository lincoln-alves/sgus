using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterFuncionalidade : BusinessProcessBase
    {
        #region "Atributos Privados"
        
        private BMFuncionalidade bmFuncionalidade = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterFuncionalidade()
            : base()
        {
            bmFuncionalidade = new BMFuncionalidade();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirFuncionalidade(Funcionalidade pFuncionalidade)
        {
            bmFuncionalidade.Salvar(pFuncionalidade);
        }

        public void AlterarFuncionalidade(Funcionalidade pFuncionalidade)
        {
            bmFuncionalidade.Salvar(pFuncionalidade);
        }

        public IList<Funcionalidade> ObterTodasFuncionalidades()
        {
            return bmFuncionalidade.ObterTodos();
        }

        public Funcionalidade ObterFuncionalidadePorID(int pId)
        {
            return bmFuncionalidade.ObterPorID(pId);
        }

      
        #endregion
    }
}
