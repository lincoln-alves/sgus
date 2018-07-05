using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCertificadoTemplate : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<CertificadoTemplate> repositorio;

        #endregion

        #region "Construtor"

        public BMCertificadoTemplate()
        {
            repositorio = new RepositorioBase<CertificadoTemplate>();
        }

        public void Salvar(CertificadoTemplate pCertificadoTemplate)
        {
            ValidarCertificadoTemplateInformado(pCertificadoTemplate);
            repositorio.Salvar(pCertificadoTemplate);
        }

        public IEnumerable<CertificadoTemplate> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome);
        }

        public CertificadoTemplate ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(CertificadoTemplate pCertificadoTemplate)
        {
            if (this.ValidarDependencias(pCertificadoTemplate))
                throw new AcademicoException("Exclusão de registro negada. Existem intens que utilizam este certificado.");

            repositorio.Excluir(pCertificadoTemplate);

        }

        protected override bool ValidarDependencias(object pCertificadoTemplate)
        {
            var certificadoTemplate = (CertificadoTemplate)pCertificadoTemplate;

            return ((certificadoTemplate.ListaOferta != null && certificadoTemplate.ListaOferta.Any()) ||
                     (certificadoTemplate.ListaTrilhaNivel != null && certificadoTemplate.ListaTrilhaNivel.Any()));
        }

        public IList<CertificadoTemplate> ObterPorFiltro(CertificadoTemplate pCertificadoTemplate)
        {
            var query = repositorio.session.Query<CertificadoTemplate>();

            if (pCertificadoTemplate != null)
            {
                if (!string.IsNullOrWhiteSpace(pCertificadoTemplate.Nome))
                    query = query.Where(x => x.Nome.ToLower().Contains(pCertificadoTemplate.Nome.ToLower()));
            }

            return query.ToList<CertificadoTemplate>();
        }

        #endregion

        private void ValidarCertificadoTemplateInformado(CertificadoTemplate pCertificadoTemplate)
        {
            ValidarInstancia(pCertificadoTemplate);

            if (string.IsNullOrWhiteSpace(pCertificadoTemplate.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            if (string.IsNullOrWhiteSpace(pCertificadoTemplate.TextoDoCertificado)) throw new AcademicoException("Texto do Certificado. Campo Obrigatório");

            if (string.IsNullOrWhiteSpace(pCertificadoTemplate.Imagem) && string.IsNullOrEmpty(pCertificadoTemplate.Imagem2)) throw new AcademicoException("Imagem. Campo Obrigatório");
        }

        public CertificadoTemplate ObterPorNome(string pNome)
        {
            var query = repositorio.session.Query<CertificadoTemplate>();
            return query.FirstOrDefault(x => x.Nome == pNome);
        }

        public IList<CertificadoTemplate> ObterPorCategoriaConteudo(List<int> categorias)
        {
            return
                repositorio.session.Query<CertificadoTemplate>()
                    .Where(
                        x =>
                            !x.ListaCategoriaConteudo.Any() ||
                            x.ListaCategoriaConteudo.Any(c => categorias.Contains(c.ID)))
                    .ToList();
        }

        public IList<CertificadoTemplate> ObterTemplateAtivoPorCategoriaConteudo(List<int> categorias)
        {
            return
                repositorio.session.Query<CertificadoTemplate>()
                    .Where(
                        x =>
                            !x.ListaCategoriaConteudo.Any() ||
                            x.ListaCategoriaConteudo.Any(c => categorias.Contains(c.ID))).Where(x => x.Ativo)
                    .ToList();
        }

        public IList<CertificadoTemplate> ObterTodosAtivos()
        {
            return
                repositorio.session.Query<CertificadoTemplate>()
                    .Where(
                        x =>
                           x.Ativo)
                    .ToList();
        }

        public bool NomeExiste(string nomeCertificado)
        {
            return
                repositorio.session.Query<CertificadoTemplate>().Any(x => x.Nome == nomeCertificado);
        }

        public CertificadoTemplate ObterCertificadoTutorPorOferta(int idOferta)
        {
            var oferta =
                repositorio.session.Query<Oferta>()
                    .Fetch(x => x.CertificadoTemplateProfessor)
                    .FirstOrDefault(x => x.ID == idOferta);

            return oferta != null ? oferta.CertificadoTemplateProfessor : null;
        }

        public IQueryable<CertificadoTemplate> ObterTodosCertificadosSomenteIdNome()
        {
            return from c in repositorio.session.Query<CertificadoTemplate>()
                select new CertificadoTemplate
                {
                    ID = c.ID,
                    Nome = c.Nome
                };
        }
    }
}