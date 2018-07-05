using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogAcessoSolucaoEducacional : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<LogAcessoSolucaoEducacional> repositorio;

        public BMLogAcessoSolucaoEducacional()
        {
            this.repositorio = new RepositorioBase<LogAcessoSolucaoEducacional>();
        }

        public IList<LogAcessoSolucaoEducacional> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<LogAcessoSolucaoEducacional>();
            return query.ToList<LogAcessoSolucaoEducacional>();
        }

        public LogAcessoSolucaoEducacional ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<LogAcessoSolucaoEducacional>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public LogAcessoSolucaoEducacional ObterLog(int idTurma, int idOferta, int idSolucaoEducacional)
        {
            var query = repositorio.session.Query<LogAcessoSolucaoEducacional>();
            return
                query.Where(
                    x =>
                        x.ID_Oferta == idOferta && x.ID_Turma == idTurma &&
                        x.ID_SolucaoEducacional == idSolucaoEducacional).FirstOrDefault();
        }


        public void Salvar(LogAcessoSolucaoEducacional log)
        {
            //ValidarUsuarioInformado(log);
            repositorio.Salvar(log);
        }

        public int ObterQuantidadeDeAcessos(int idTurma, int idOferta, int idSolucaoEducacional)
        {
            var log = this.ObterLog(idTurma, idOferta, idSolucaoEducacional);
            var acessos = log != null ? log.QuantidadeDeAcessos : 0;
            return acessos;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
