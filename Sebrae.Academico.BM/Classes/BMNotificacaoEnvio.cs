using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMNotificacaoEnvio : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<NotificacaoEnvio> repositorio;


        public BMNotificacaoEnvio()
        {
            repositorio = new RepositorioBase<NotificacaoEnvio>();

        }

        private void ValidarTurma(NotificacaoEnvio notificacaoEnvio)
        {

            if (string.IsNullOrWhiteSpace(notificacaoEnvio.Texto)) throw new AcademicoException("Texto. Campo Obrigatório");

            if (string.IsNullOrWhiteSpace(notificacaoEnvio.Link)) throw new AcademicoException("Link. Campo Obrigatório");

            //if (notificacaoEnvio.Oferta == null || (notificacaoEnvio.Oferta != null && notificacaoEnvio.Oferta.ID == 0)) throw new AcademicoException("Oferta. Campo Obrigatório");

        }

        public void Excluir(NotificacaoEnvio notificacaoEnvio)
        {
            repositorio.Excluir(notificacaoEnvio);
        }

        public IList<NotificacaoEnvio> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IQueryable<NotificacaoEnvio> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public NotificacaoEnvio ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<NotificacaoEnvio> ObterPorFiltro(NotificacaoEnvio notificacaoEnvio)
        {
            var query = repositorio.session.Query<NotificacaoEnvio>();
            if (notificacaoEnvio != null)
            {
                if (!string.IsNullOrWhiteSpace(notificacaoEnvio.Texto))
                    query = query.Where(x => x.Texto.ToUpper().Contains(notificacaoEnvio.Texto.ToUpper()));

                if (!string.IsNullOrWhiteSpace(notificacaoEnvio.Link))
                    query = query.Where(x => x.Link.ToUpper().Contains(notificacaoEnvio.Link.ToUpper()));

                if (notificacaoEnvio.ID > 0)
                    query = query.Where(x => x.ID == notificacaoEnvio.ID);

                if (notificacaoEnvio.Uf != null)
                    query = query.Where(x => x.Uf.ID == notificacaoEnvio.Uf.ID);
            }

            return query.ToList<NotificacaoEnvio>();
        }

        public void Salvar(NotificacaoEnvio notificacaoEnvio)
        {
            ValidarNotificacaoEnvioInformada(notificacaoEnvio);
            repositorio.Salvar(notificacaoEnvio);
        }

        private void ValidarNotificacaoEnvioInformada(NotificacaoEnvio notificacaoEnvio)
        {

            ValidarDependencias(notificacaoEnvio);

            if (string.IsNullOrWhiteSpace(notificacaoEnvio.Texto)) throw new AcademicoException("Texto. Campo Obrigatório.");

            if (string.IsNullOrWhiteSpace(notificacaoEnvio.Link)) throw new AcademicoException("Link. Campo Obrigatório.");

        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
