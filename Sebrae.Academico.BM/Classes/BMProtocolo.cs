using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BM.Classes
{
    public class BMProtocolo : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Protocolo> repositorio;

        #endregion

        public BMProtocolo()
        {
            repositorio = new RepositorioBase<Protocolo>();
        }

        public IList<Protocolo> ObterTodos()
        {
            var query = repositorio.session.Query<Protocolo>();
            return query.ToList();
        }

        public void SalvarSemCommit(Protocolo protocolo)
        {
            repositorio.SalvarSemCommit(protocolo);
        }

        public void Commit()
        {
            repositorio.Commit();
        }

        public IQueryable<Protocolo> ObterTodosIqueryable()
        {
            return repositorio.session.Query<Protocolo>();
        }

        public IQueryable<Protocolo> ObterTodosPorNumero(int numeroProtocolo)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.Where(x => x.Numero == numeroProtocolo).AsQueryable();
            return protocolo;
        }

        public Protocolo ObterPorId(int pId)
        {
            var query = repositorio.session.Query<Protocolo>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public Protocolo ObterPorNumero(int numero)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.FirstOrDefault(x => x.Numero == numero);

            return protocolo;
        }

        public Protocolo ObterPorDescricao(string descricao)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.FirstOrDefault(x => x.Descricao.ToLower().Contains(descricao.ToLower()));

            if (protocolo == null)
                throw new AcademicoException("O Protocolo não foi encontrado");

            return protocolo;
        }

        public Protocolo ObterPorRemetente(string remetente)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.FirstOrDefault(x => x.Remetente != null && x.Remetente.Nome.ToLower().Contains(remetente.ToLower()));

            if (protocolo == null)
                throw new AcademicoException("O Protocolo não foi encontrado");

            return protocolo;
        }

        public Protocolo ObterPorDestinatario(string destinatario)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.FirstOrDefault(x => x.Destinatario != null && x.Destinatario.Nome.ToLower().Contains(destinatario.ToLower()));

            if (protocolo == null)
                throw new AcademicoException("O Protocolo não foi encontrado");

            return protocolo;
        }

        public IList<Protocolo> ObterPendentes(string cpf)
        {
            var query = repositorio.session.Query<Protocolo>();
            query = query.Where(x => x.Destinatario.CPF == cpf && x.DataRecebimento == null);
            return query.ToList();
        }

        public IQueryable<Protocolo> ObterPorDataEnvio(DateTime dateTime)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.Where(x => x.DataEnvio != null && x.DataEnvio == dateTime);

            return protocolo;
        }

        public IQueryable<Protocolo> ObterPorDataRecebimento(DateTime dateTime)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.Where(x => x.DataRecebimento != null && x.DataRecebimento == dateTime);

            return protocolo;
        }

        public Protocolo ObterPorUnidade(string unidade)
        {
            var query = repositorio.session.Query<Protocolo>();
            var protocolo = query.FirstOrDefault(x => x.Destinatario != null && x.Destinatario.Nome.ToLower().Contains(unidade.ToLower()));

            if (protocolo == null)
                throw new AcademicoException("O Protocolo não foi encontrado");

            return protocolo;
        }


        public void Salvar(Protocolo model)
        {
            repositorio.Salvar(model);
        }

        public void Excluir(Protocolo model)
        {
            repositorio.Excluir(model);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
