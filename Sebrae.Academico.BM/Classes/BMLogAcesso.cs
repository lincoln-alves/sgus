using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogAcessosPaginas : BusinessManagerBase
    {
        #region "Atributos Privados"

        private RepositorioBase<LogAcesso> repositorio;

        #endregion

        #region "Construtor"

        public BMLogAcessosPaginas()
        {
            repositorio = new RepositorioBase<LogAcesso>();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<LogAcesso> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public LogAcesso ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void ValidarLog(LogAcesso pLogAcesso)
        {
            this.ValidarInstancia(pLogAcesso);

            if (pLogAcesso.IDUsuario == 0)
                throw new AcademicoException("Usuário não encontrado. Campo Obrigatório");
        }

        public void Salvar(LogAcesso pLogAcesso)
        {
            ValidarLog(pLogAcesso);
            pLogAcesso.DataAcesso = DateTime.Now;
            repositorio.Salvar(pLogAcesso);
        }

        public IList<LogAcesso> ObterUltimosAcessosDosUsuario(int idUsuario, int quantidade)
        {
            IList<LogAcesso> ListaLogAcesso = null;
            var query = repositorio.session.Query<LogAcesso>();
            ListaLogAcesso = query.Where(x => x.IDUsuario == idUsuario).OrderByDescending(x => x.DataAcesso)
                                  .Take(quantidade).ToList<LogAcesso>();
            return ListaLogAcesso;
        }

        #endregion
    }
}
