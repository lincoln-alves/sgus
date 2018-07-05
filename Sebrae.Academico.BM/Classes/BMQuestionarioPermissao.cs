using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Classes
{
    public class BMQuestionarioPermissao : BusinessManagerBase
    {
        #region "Construtor"

        public BMQuestionarioPermissao()
        {
            repositorio = new RepositorioBase<QuestionarioPermissao>();
        }

        public void Salvar(QuestionarioPermissao qp)
        {
            repositorio.Salvar(qp);
        }

        #endregion

        private RepositorioBase<QuestionarioPermissao> repositorio;

        public IQueryable<QuestionarioPermissao> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
