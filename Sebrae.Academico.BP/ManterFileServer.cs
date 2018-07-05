using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterFileServer : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMFileServer bmFileServer = null;

        #endregion

        #region "Construtor"

        public ManterFileServer()
            : base()
        {
            bmFileServer = new BMFileServer();
        }

        #endregion
        
        #region "Métodos Públicos"

        public void SalvarSemCommit(FileServer fileServer)
        {
            bmFileServer.SalvarSemCommit(fileServer);
        }

        public void Commit()
        {
            bmFileServer.Commit();
        }

        public void IncluirFileServer(FileServer pFileServer)
        {
            bmFileServer.Salvar(pFileServer);
        }

        public void AlterarFileServer(FileServer pFileServer)
        {
            bmFileServer.Salvar(pFileServer);
        }

        public FileServer ObterFileServerPorID(int pId)
        {
            return bmFileServer.ObterPorID(pId);
        }

        public IList<FileServer> ObterTodasFileServer()
        {
            return bmFileServer.ObterTodos();
        }

        public IQueryable<FileServer> ObterTodosIQueryable()
        {
            return bmFileServer.ObterTodosIQueryable();
        }

        public void ExcluirFileServer(int IdFileServer)
        {
            try
            {
                FileServer fileServer = null;

                if (IdFileServer > 0)
                {
                    fileServer = bmFileServer.ObterPorID(IdFileServer);
                }

                bmFileServer.Excluir(fileServer);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<FileServer> ObterFileServerPorFiltro(FileServer pFileServer,bool pMediaServer = false)
        {
            return bmFileServer.ObterPorFiltro(pFileServer, pMediaServer);
        }

        #endregion
    }
}
