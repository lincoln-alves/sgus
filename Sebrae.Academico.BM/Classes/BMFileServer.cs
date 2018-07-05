using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMFileServer : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<FileServer> repositorio;

        #endregion


        public BMFileServer()
        {
            repositorio = new RepositorioBase<FileServer>();
        }

        public void Salvar(FileServer pFileServer)
        {
            repositorio.Salvar(pFileServer);
        }

        public void SalvarSemCommit(FileServer fileServer)
        {
            repositorio.SalvarSemCommit(fileServer);
        }

        public void Commit()
        {
            repositorio.Commit();
        }
              
        public IList<FileServer> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.NomeDoArquivoOriginal).ToList<FileServer>();
        }

        public FileServer ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(FileServer pFileServer)
        {
            if (this.ValidarDependencias(pFileServer))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Arquivo.");

            repositorio.Excluir(pFileServer);
        }

        protected override bool ValidarDependencias(object pFileServer)
        {
            var fileServer = (FileServer)pFileServer;

            return fileServer.ListaItemTrilha != null && fileServer.ListaItemTrilha.Count > 0 &&
                   fileServer.ListaTrilhaAtividadeFormativaParticipacao != null &&
                   fileServer.ListaTrilhaAtividadeFormativaParticipacao.Count > 0;
        }

        public IList<FileServer> ObterPorFiltro(FileServer pFileServer,bool pMediaServer = false)
        {
            var query = repositorio.session.Query<FileServer>();

            if (pFileServer == null) return query.ToList<FileServer>();
            if (!string.IsNullOrWhiteSpace(pFileServer.NomeDoArquivoOriginal))
                query = query.Where(x => x.NomeDoArquivoOriginal.Contains(pFileServer.NomeDoArquivoOriginal));
            if (!string.IsNullOrWhiteSpace(pFileServer.NomeDoArquivoNoServidor))
                query = query.Where(x => x.NomeDoArquivoNoServidor.Contains(pFileServer.NomeDoArquivoNoServidor));
            if (pMediaServer)
                query = query.Where(x => x.MediaServer == pFileServer.MediaServer);
            if (pFileServer.ID > 0)
                query = query.Where(x => x.ID == pFileServer.ID);

            return query.ToList<FileServer>();
        }


        private void ValidarFileServer(FileServer pFileServer)
        {
            ValidarInstancia(pFileServer);

            //if (string.IsNullOrWhiteSpace(pFileServer.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");
        }
            
        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }


        public IQueryable<FileServer> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
