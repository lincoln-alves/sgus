using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{

    public class BMEmail : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Email> repositorio;


        public BMEmail()
        {
            repositorio = new RepositorioBase<Email>();

        }

        public void SalvarSemCommit(Email registro)
        {
            repositorio.SalvarSemCommit(registro);
        }

        public void Commit()
        {
            repositorio.Commit();
        }

        public void Excluir(Email registro)
        {
            repositorio.Excluir(registro);
        }

        public Email ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IQueryable<Email> ObterPorEmailEnvio(int idEnvioEmail, bool? enviado)
        {
            var query = repositorio.session.Query<Email>().Where(x => x.EmailEnvio.ID == idEnvioEmail);

            if (enviado.HasValue)
                query = query.Where(x => x.Enviado == enviado.Value);
            
            return query;
        }

        public void Salvar(Email registro)
        {
            ValidarEmailInformado(registro);
            repositorio.Salvar(registro);
        }

        private void ValidarEmailInformado(Email registro)
        {
            if (string.IsNullOrWhiteSpace(registro.TextoEmail)) throw new AcademicoException("Texto. Campo Obrigatório.");
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Merge(EmailEnvio emailEnvio)
        {
            repositorio.FazerMerge(emailEnvio);
        }

        public IEnumerable<Email> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IQueryable<Email> ObterTodosIqueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
