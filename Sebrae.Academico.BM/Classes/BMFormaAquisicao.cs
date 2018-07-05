using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMFormaAquisicao : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<FormaAquisicao> repositorio;

        #endregion

        #region "Construtor"

        public BMFormaAquisicao()
        {
            repositorio = new RepositorioBase<FormaAquisicao>();
        }

        public void Salvar(FormaAquisicao pFormaAquisicao)
        {
            ValidarFormaAquisicaoInformado(pFormaAquisicao);
            this.VerificarExistenciaDeFormaAquisicao(pFormaAquisicao);
            repositorio.Salvar(pFormaAquisicao);
        }

        private void VerificarExistenciaDeFormaAquisicao(FormaAquisicao pFormaAquisicao)
        {
            FormaAquisicao formaAquisicao = this.ObterPorNome(pFormaAquisicao.Nome.Trim());

            if (formaAquisicao != null)
            {
                if (pFormaAquisicao.ID != formaAquisicao.ID)
                {
                    throw new AcademicoException(string.Format("A Forma de Aquisição '{0}' já está cadastrada", pFormaAquisicao.Nome));
                }
            }
        }

        /// <summary>
        /// Verifica se já existe uma Forma de Aquisição cadastrada.
        /// </summary>
        /// <param name="pFormaAquisicao">Objeto Forma Aquisição</param>
        /// <returns>Retorna True se já existir uma Forma de Aquisição
        /// Retorna False, senão existir uma Forma de Aquisição.</returns>
        private bool VerificarExistenciaDaFormaAquisicao(FormaAquisicao pFormaAquisicao)
        {
            bool formaAquisicaoJaExiste = false;
            var query = repositorio.session.Query<FormaAquisicao>();
            formaAquisicaoJaExiste = query.Any(x => x.Nome.Trim().ToUpper() == pFormaAquisicao.Nome.Trim().ToUpper());
            return formaAquisicaoJaExiste;
        }

        public IList<FormaAquisicao> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<FormaAquisicao>();
        }

        public IList<FormaAquisicao> ObterPorTipo(enumTipoFormaAquisicao tipo)
        {
            return repositorio.ObterTodos().Where(f => f.TipoFormaDeAquisicao == tipo).ToList();
        }

        public FormaAquisicao ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(FormaAquisicao pFormaAquisicao)
        {
            if (this.ValidarDependencias(pFormaAquisicao))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Forma de Aquisição.");

            repositorio.Excluir(pFormaAquisicao);

        }

        protected override bool ValidarDependencias(object pFormaAquisicao)
        {
            FormaAquisicao formaAquisicao = (FormaAquisicao)pFormaAquisicao;

            return ((formaAquisicao.ListaItemTrilha != null && formaAquisicao.ListaItemTrilha.Count > 0) ||
                    (formaAquisicao.ListaSolucaoEducacional != null && formaAquisicao.ListaSolucaoEducacional.Count > 0));
        }

        public IList<FormaAquisicao> ObterPorFiltro(FormaAquisicao pFormaAquisicao)
        {
            var query = repositorio.session.Query<FormaAquisicao>();

            if (pFormaAquisicao != null)
            {
                if (!string.IsNullOrWhiteSpace(pFormaAquisicao.Nome))
                    query = query.Where(x => x.Nome.Contains(pFormaAquisicao.Nome));
            }

            return query.ToList<FormaAquisicao>();
        }

        #endregion

        private void ValidarFormaAquisicaoInformado(FormaAquisicao pFormaAquisicao)
        {
            ValidarInstancia(pFormaAquisicao);

            if (string.IsNullOrWhiteSpace(pFormaAquisicao.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");
        }

        public FormaAquisicao ObterPorNome(string pNome)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<FormaAquisicao>();
            return query.FirstOrDefault(x => x.Nome == pNome);
        }
        
        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
