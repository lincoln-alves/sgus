using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMFornecedor : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<Fornecedor> repositorio = null;

        #endregion

        #region "Construtor"

        public BMFornecedor()
        {
            repositorio = new RepositorioBase<Fornecedor>();
        }

        #endregion

        #region "Métodos Privados"

        private void VerificarExistenciaDeFornecedor(Fornecedor pFornecedor)
        {
            //Verifica se o usuário informado já existe
            Fornecedor fornecedor = this.ObterPorLoginFornecedor(pFornecedor.Login);

            if (fornecedor != null)
            {
                if (pFornecedor.ID != fornecedor.ID)
                {
                    throw new AcademicoException(string.Format("Já existe um usuário com o Login {0}", pFornecedor.Login));
                }
            }

        }

        /// <summary>
        /// Validação das informações de um Fornecedor.
        /// </summary>
        /// <param name="pFornecedor"></param>
        private void ValidarFornecedorInformado(Fornecedor pFornecedor)
        {
            ValidarInstancia(pFornecedor);

            //Verifica se o nome do Fornecedor está nulo
            if (string.IsNullOrWhiteSpace(pFornecedor.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            //Usuário
            if (string.IsNullOrWhiteSpace(pFornecedor.Login)) throw new AcademicoException("Login. Campo Obrigatório");

            //Senha
            if (string.IsNullOrWhiteSpace(pFornecedor.Senha)) throw new AcademicoException("Senha. Campo Obrigatório");

            //Todo-> Validar Senha

        }

        private Fornecedor ObterPorLoginFornecedor(string Login)
        {
            Fornecedor formaAquisicao = null;
            var query = repositorio.session.Query<Fornecedor>();
            formaAquisicao = query.FirstOrDefault(x => x.Login == Login);
            return formaAquisicao;
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(Fornecedor pFornecedor)
        {
            ValidarFornecedorInformado(pFornecedor);
            VerificarExistenciaDeFornecedor(pFornecedor);
            repositorio.Salvar(pFornecedor);
        }

        public IQueryable<Fornecedor> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable().OrderBy(x => x.Nome).AsQueryable();
        }

        public IList<Fornecedor> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<Fornecedor>();
        }

        public Fornecedor ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(Fornecedor pFornecedor)
        {

            this.ValidarInstancia(pFornecedor);

            if (this.ValidarDependencias(pFornecedor))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Fornecedor.");

            repositorio.Excluir(pFornecedor);
        }

        public IList<Fornecedor> ObterPorFiltro(Fornecedor pFornecedor)
        {
            var query = repositorio.session.Query<Fornecedor>();

            if (pFornecedor != null)
            {
                if (!string.IsNullOrWhiteSpace(pFornecedor.Nome))
                    query = query.Where(x => x.Nome.Contains(pFornecedor.Nome));

                if (!String.IsNullOrWhiteSpace(pFornecedor.Login))
                    query = query.Where(x => x.Login.ToUpper() == pFornecedor.Login.ToUpper());

                //if (!string.IsNullOrWhiteSpace(pFornecedor.Senha))
                //{
                //    MD5CryptoServiceProvider critographer = new MD5CryptoServiceProvider();
                //    byte[] dados = System.Text.Encoding.ASCII.GetBytes(pFornecedor.Senha);
                //    query = query.Where(x => x.Senha == System.Text.Encoding.ASCII.GetString(dados));
                //}
            }


            return query.ToList<Fornecedor>();
        }

        public Fornecedor ObterPorLogin(string login)
        {
            var query = repositorio.session.Query<Fornecedor>();
            query = query.Where(x => x.Login.Contains(login));
            return query.FirstOrDefault();
        }

        public bool AutenticarFornecedor(string pLogin, string pSenha)
        {

            var query = repositorio.session.Query<Fornecedor>();

            //query = query.Where(x => x.Login.ToUpper() == pLogin.ToUpper() &&
            //                         x.Senha == WebFormHelper.ObterHashMD5(pSenha));

            query = query.Where(x => x.Login.ToUpper() == pLogin.ToUpper() &&
                                     x.Senha == CriptografiaHelper.Criptografar(pSenha, "UniversidadeCorporativaSEBRAE200"));

            return query.Count() > 0;

        }

        public Fornecedor ObterFornecedorSistema(enumFornecedor fornecedor)
        {
            return repositorio.session.Query<Fornecedor>().FirstOrDefault(x => x.FornecedorSistema == fornecedor);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Métodos Protected"

        protected override bool ValidarDependencias(object pFornecedor)
        {
            Fornecedor fornecedor = (Fornecedor)pFornecedor;
            return (fornecedor.ListaSolucaoEducacional != null && fornecedor.ListaSolucaoEducacional.Count > 0);
        }

      

        #endregion

    }
}
