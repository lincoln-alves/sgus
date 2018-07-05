using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Collections.Generic;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMQuestionarioAssociacaoEnvio : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<QuestionarioAssociacaoEnvio> repositorio;

        public BMQuestionarioAssociacaoEnvio()
        {
            repositorio = new RepositorioBase<QuestionarioAssociacaoEnvio>();

        }

        public void Salvar(QuestionarioAssociacaoEnvio questionarioAssociacaoEnvio)
        {
            repositorio.Salvar(questionarioAssociacaoEnvio);
        }


        public IQueryable<QuestionarioAssociacaoEnvio> ObterTodosIQueryable()
        {
            return repositorio.session.Query<QuestionarioAssociacaoEnvio>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
