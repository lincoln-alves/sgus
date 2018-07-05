using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMEmailEnvio : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<EmailEnvio> repositorio;


        public BMEmailEnvio()
        {
            repositorio = new RepositorioBase<EmailEnvio>();

        }

        private void ValidarEmailEnvio(EmailEnvio email)
        {

            if (string.IsNullOrWhiteSpace(email.Texto)) throw new AcademicoException("Texto. Campo Obrigatório");

            if (string.IsNullOrWhiteSpace(email.Assunto)) throw new AcademicoException("Assunto. Campo Obrigatório");

        }

        public void Excluir(EmailEnvio EmailEnvio)
        {
            repositorio.Excluir(EmailEnvio);
        }

        public IList<EmailEnvio> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public EmailEnvio ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<EmailEnvio> ObterPorFiltro(EmailEnvio registro)
        {
            var query = repositorio.session.Query<EmailEnvio>();
            if (registro != null)
            {
                if (!string.IsNullOrWhiteSpace(registro.Texto))
                    query = query.Where(x => x.Texto.ToUpper().Contains(registro.Texto.ToUpper()));

                if (!string.IsNullOrWhiteSpace(registro.Assunto))
                    query = query.Where(x => x.Assunto.ToUpper().Contains(registro.Assunto.ToUpper()));

                if (registro.ID > 0)
                    query = query.Where(x => x.ID == registro.ID);

                if (registro.Uf != null)
                    query = query.Where(x => x.Uf != null && x.Uf.ID == registro.Uf.ID);
            }

            return query.ToList<EmailEnvio>();
        }

        public void ExcluirTodos(IQueryable<Email> emails)
        {
            new RepositorioBase<Email>().ExcluirTodos(emails);
        }

        public void Salvar(EmailEnvio EmailEnvio)
        {
            ValidarEmailEnvioInformada(EmailEnvio);
            repositorio.Salvar(EmailEnvio);
        }

        private void ValidarEmailEnvioInformada(EmailEnvio EmailEnvio)
        {

            ValidarDependencias(EmailEnvio);

            if (string.IsNullOrWhiteSpace(EmailEnvio.Texto)) throw new AcademicoException("Texto. Campo Obrigatório.");

            if (string.IsNullOrWhiteSpace(EmailEnvio.Assunto)) throw new AcademicoException("Assunto. Campo Obrigatório.");

        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void LimparSessao()
        {
            repositorio.LimparSessao();
        }
    }
}
