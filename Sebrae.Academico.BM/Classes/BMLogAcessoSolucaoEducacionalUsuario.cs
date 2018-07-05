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
    public class BMLogAcessoSolucaoEducacionalUsuario : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<LogAcessoSolucaoEducacionalUsuario> repositorio;

        public BMLogAcessoSolucaoEducacionalUsuario()
        {
            this.repositorio = new RepositorioBase<LogAcessoSolucaoEducacionalUsuario>();
        }

        public IList<LogAcessoSolucaoEducacionalUsuario> ObterTodos()
        {
            var query = repositorio.session.Query<LogAcessoSolucaoEducacionalUsuario>();
            return query.ToList<LogAcessoSolucaoEducacionalUsuario>();
        }

        public LogAcessoSolucaoEducacionalUsuario ObterPorId(int pId)
        {
            var query = repositorio.session.Query<LogAcessoSolucaoEducacionalUsuario>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IQueryable<LogAcessoSolucaoEducacionalUsuario> ObterLog(int idUsuario, int idTurma, int idOferta,
            int idSolucaoEducacional)
        {
            return repositorio.session.Query<LogAcessoSolucaoEducacionalUsuario>()
                .Where(x =>
                    x.ID_Oferta == idOferta
                    && x.ID_Turma == idTurma
                    && x.ID_SolucaoEducacional == idSolucaoEducacional
                    && x.ID_Usuario == idUsuario
                );
        }


        public void Salvar(LogAcessoSolucaoEducacionalUsuario log)
        {
            //ValidarUsuarioInformado(log);
            repositorio.Salvar(log);
        }

        public int ObterQuantidadeDeAcessos(int idUsuario, int idTurma, int idOferta, int idSolucaoEducacional)
        {
            var logs = ObterLog(idUsuario, idTurma, idOferta, idSolucaoEducacional).Select(x => x.ID);
            return logs.Count();
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
