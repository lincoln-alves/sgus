using System;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogAcessoFornecedor : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<LogAcessoFornecedor> repositorio;

        #endregion

        #region "Construtor"

        public BMLogAcessoFornecedor()
        {
            repositorio = new RepositorioBase<LogAcessoFornecedor>();
        }

        #endregion

        #region "Métodos Privados"

        private void ValidarLogAcessoFornecedorInformado(LogAcessoFornecedor pLogAcessoFornecedor)
        {
            ValidarInstancia(pLogAcessoFornecedor);

            if (pLogAcessoFornecedor.Metodo == null)
                throw new Exception("Nome do Método Executado não informado! Campo obrigatório.");

            if (pLogAcessoFornecedor.DataAcesso == null)
                throw new Exception("Data e Hora do Log não informada! Campo obrigatório.");
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(LogAcessoFornecedor pLogAcessoFornecedor)
        {
            ValidarLogAcessoFornecedorInformado(pLogAcessoFornecedor);

            repositorio.Salvar(pLogAcessoFornecedor);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
