using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaTopicoTematico : BusinessManagerBase
    {
        #region Atributos

        private RepositorioBase<TrilhaTopicoTematico> repositorio = null;

        #endregion

        #region Construtor

        public BMTrilhaTopicoTematico()
        {
            repositorio = new RepositorioBase<TrilhaTopicoTematico>();
        }

        #endregion

        public void Salvar(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            ValidarTrilhaTopicoTematicoInformado(pTrilhaTopicoTematico);
            repositorio.Salvar(pTrilhaTopicoTematico);
        }

        public IQueryable<TrilhaTopicoTematico> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).AsQueryable();
        }

        public TrilhaTopicoTematico ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        /// <summary>
        /// Validação das informações de um Tópico Temático.
        /// </summary>
        /// <param name="pItemTrilha"></param>
        public void ValidarTrilhaTopicoTematicoInformado(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            ValidarInstancia(pTrilhaTopicoTematico);

            if (string.IsNullOrWhiteSpace(pTrilhaTopicoTematico.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            if (string.IsNullOrWhiteSpace(pTrilhaTopicoTematico.DescricaoTextoEnvio) && string.IsNullOrWhiteSpace(pTrilhaTopicoTematico.DescricaoArquivoEnvio)) throw new AcademicoException("Favor informar a descrição do texto ou do arquivo de envio.");

            this.VerificarExistenciaDoTopicoTematico(pTrilhaTopicoTematico);
        }

        private void VerificarExistenciaDoTopicoTematico(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            TrilhaTopicoTematico trilhaTopicoTematico = this.ObterPorNome(pTrilhaTopicoTematico);

            if (trilhaTopicoTematico != null)
            {
                if (pTrilhaTopicoTematico.ID != trilhaTopicoTematico.ID)
                {
                    throw new AcademicoException(string.Format("O Tópico Temático '{0}' já está cadastrado",
                                                 pTrilhaTopicoTematico.Nome.Trim()));
                }
            }

        }

        private TrilhaTopicoTematico ObterPorNome(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            //return repositorio.GetByProperty("Nome", pTrilhaTopicoTematico.Nome).FirstOrDefault();
            var query = repositorio.session.Query<TrilhaTopicoTematico>();
            return query.FirstOrDefault(x => x.Nome == pTrilhaTopicoTematico.Nome);
        }


        public void Excluir(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            if (this.ValidarDependencias(pTrilhaTopicoTematico))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Tópico.");

            repositorio.Excluir(pTrilhaTopicoTematico);

        }

        public TrilhaTopicoTematico ObterTrilhaTopicoTematicoPorObjetivoTrilhaNivel(string chaveExternaObjetivo, string token)
        {
            var query = repositorio.session.Query<TrilhaTopicoTematico>();
            query = query.Where(x => x.ListaItemTrilha.Any(y => y.Missao.PontoSebrae.TrilhaNivel.ListaUsuarioTrilha.Any(z => z.Token == token) && y.Objetivo.ChaveExterna == chaveExternaObjetivo) );
            return query.FirstOrDefault();
        }

        public IList<TrilhaTopicoTematico> ObterPorNome(string pNome)
        {
            //return repositorio.GetByProperty("Nome", pNome);
            var query = repositorio.session.Query<TrilhaTopicoTematico>();
            return query.Where(x => x.Nome == pNome).ToList<TrilhaTopicoTematico>();
        }

        protected override bool ValidarDependencias(object pTrilhaTopicoTematico)
        {
            TrilhaTopicoTematico trilhaTopicoTematico = (TrilhaTopicoTematico)pTrilhaTopicoTematico;

            return ((trilhaTopicoTematico.ListaItemTrilha != null && trilhaTopicoTematico.ListaItemTrilha.Count > 0) ||
                    (trilhaTopicoTematico.TrilhaAtividadeFormativaParticipacao != null));
        }

        public IList<TrilhaTopicoTematico> ObterPorFiltro(TrilhaTopicoTematico ptrilhaTopicoTematico)
        {
            var query = repositorio.session.Query<TrilhaTopicoTematico>();

            if (ptrilhaTopicoTematico != null)
            {
                if (!string.IsNullOrWhiteSpace(ptrilhaTopicoTematico.Nome))
                    query = query.Where(x => x.Nome.Contains(ptrilhaTopicoTematico.Nome.ToUpper()));

            }

            return query.ToList<TrilhaTopicoTematico>();
        }

        public TrilhaTopicoTematico ObterPorNomeExibicao(string nomeExibicao)
        {
            var query = repositorio.session.Query<TrilhaTopicoTematico>();
            TrilhaTopicoTematico trilhaTopicoTematico = query.FirstOrDefault(x => x.NomeExibicao != null && x.NomeExibicao.ToUpper() == nomeExibicao.ToUpper());
            return trilhaTopicoTematico;
        }
    }
}
