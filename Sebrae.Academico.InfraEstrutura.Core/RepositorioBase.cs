using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.UnitOfWork;
using System.Data;
using System.Collections;

namespace Sebrae.Academico.InfraEstrutura.Core
{
    public class RepositorioBase<T> : IRepositorioBase<T>, IDisposable where T : class
    {

        #region "Atributos"

        protected readonly INHibernateUnitOfWork _unitOfWork;
        protected ISession session;

        public ISession Session { get { return session; } }

        #endregion

        public RepositorioBase(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (INHibernateUnitOfWork)unitOfWork;
            this._unitOfWork.Context.Clear();
           //this.session = this._unitOfWork.Context;

        }


        public int CountByProperty(string pPropriedade, object pValor)
        {
            ICriteria criteria = this.session.CreateCriteria<T>().Add(Expression.Eq(pPropriedade, pValor)).SetProjection(Projections.RowCount());
            return ((int)criteria.List()[0]);
        }

        public void Excluir(T objeto)
        {
            try
            {
                limpesaOperacaoAlteracaoExclusao(objeto);
                this.session.Delete(objeto);
                this.session.FlushMode = FlushMode.Auto;
                this.session.Flush();
            }
            catch (Exception ex)
            {
                this.session.Clear();
                AcademicoException.TratarExcecao(ex);
            }
        }

        public T Salvar(T objeto)
        {
            try
            {
                limpesaOperacaoAlteracaoExclusao(objeto);
                this.session.SaveOrUpdate(objeto);
                this.session.FlushMode = FlushMode.Auto;
                this.session.Flush();
                this.session.Clear();
            }
            catch (Exception ex)
            {
                this.session.Clear();
                AcademicoException.TratarExcecao(ex);
            }


            return objeto;
        }

        private void limpesaOperacaoAlteracaoExclusao(T objeto)
        {
            int id = (int)objeto.GetType().GetProperty("ID").GetValue(objeto, null);
            if (id > 0)
                this.session.Clear();
        }

        public T ObterPorID(int id)
        {
            try
            {
                return this.session.Get<T>(id);
            }
            catch (Exception ex)
            {
                // cUtil.WriteLog(ex, "Erro ao ObterPorID. Erro aconteceu no método ObterPorID da classe " + this.GetType().Name);
                AcademicoException.TratarExcecao(ex);
                return null;
            }


        }

        public System.Collections.Generic.IList<T> ListarTodos()
        {
            //  ISession sessao = null;
            IList<T> resultado = null;

            try
            {
                // sessao = NHibernateHelper.AbrirSessao(false);
                resultado = (from t in this.session.Linq<T>() select t).AsParallel().ToList();
            }
            catch (Exception ex)
            {
                //  cUtil.WriteLog(ex, "Erro ao Listar Todos os registros. Erro aconteceu no método Listar da classe " + this.GetType().Name);
                AcademicoException.TratarExcecao(ex);
            }

            return resultado;
        }

        public void Dispose()
        {
            this.session.Clear();
            this.session.Close();
        }

        public IList<T> GetByProperty(string pPropriedade, object pValor)
        {
            return this.session.CreateCriteria<T>().Add(Expression.Eq(pPropriedade, pValor)).List<T>();
        }

        public IList<T> ObterTodos()
        {
            return this.session.CreateCriteria<T>().List<T>();
        }

        public IList<T> LikeByProperty(string pPropriedade, object pValor)
        {
            return this.session.CreateCriteria<T>().Add(Expression.Like(pPropriedade, "%" + pValor + "%")).List<T>();
        }
    }
}
