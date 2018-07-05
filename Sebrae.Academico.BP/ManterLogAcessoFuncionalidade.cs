using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterLogAcessoFuncionalidade : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMLogAcessoFuncionalidade bmLogAcessoFuncionalidade = null;

        #endregion

        #region "Construtor"

        public ManterLogAcessoFuncionalidade()
            : base()
        {
            bmLogAcessoFuncionalidade = new BMLogAcessoFuncionalidade();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirLogAcessoFuncionalidade(LogAcessoFuncionalidade pLogAcessoFuncionalidade)
        {
            bmLogAcessoFuncionalidade.Salvar(pLogAcessoFuncionalidade);
        }

        public void AlterarLogAcessoFuncionalidade(LogAcessoFuncionalidade pLogAcessoFuncionalidade)
        {
            bmLogAcessoFuncionalidade.Salvar(pLogAcessoFuncionalidade);
        }

        public IList<LogAcessoFuncionalidade> ObterTodosLogsDeFuncionalidades()
        {
            return bmLogAcessoFuncionalidade.ObterTodos();
        }

        public LogAcessoFuncionalidade ObterLogAcessoFuncionalidadePorID(int pId)
        {
            return bmLogAcessoFuncionalidade.ObterPorID(pId);
        }


        #endregion
    }
}

