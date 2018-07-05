using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTag : BusinessManagerBase
    {

        private RepositorioBase<Tag> repositorio;

        public BMTag()
        {
            repositorio = new RepositorioBase<Tag>();
        }

        private void ValidarTagInformada(Tag pTag)
        {
            this.ValidarInstancia(pTag);

            if (string.IsNullOrWhiteSpace(pTag.Nome))
            {
                throw new AcademicoException("Nome. Campo Obrigatório");
            }

            this.VerificarExistenciaDaTag(pTag);

        }

        public void Salvar(Tag pTag)
        {
            ValidarTagInformada(pTag);
            repositorio.Salvar(pTag);
        }


        private void VerificarExistenciaDaTag(Tag pTag)
        {
            Tag tag = this.ObterPorNome(pTag);

            if (pTag != null)
            {
                if (tag != null && pTag.ID != tag.ID)
                {
                    throw new AcademicoException(string.Format("A tag '{0}' já está cadastrado",
                                                 pTag.Nome.Trim()));
                }

                //if (pTag.TagPai != null)
                //{

                //    pTag.TagPai = new BMTag().ObterPorID(pTag.TagPai.ID);

                //    if (pTag.TagPai.NumeroNivel.HasValue)
                //    {
                //        pTag.NumeroNivel = ++pTag.TagPai.NumeroNivel;
                //    }
                //    else
                //    {
                //        pTag.NumeroNivel = 1;
                //    }
                //}
                //else
                //{
                //    pTag.NumeroNivel = 0;
                //}

            }
        }

        public Tag ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<Tag> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IList<Tag> ObterTodasTagNaoSinonimas()
        {
            var query = repositorio.session.Query<Tag>();
            query = query.Where(x => x.InSinonimo == false);
            return query.ToList<Tag>();
        }

        public IList<Tag> ObterTodosNivelZero()
        {
            var query = repositorio.session.Query<Tag>();
            query = query.Where(x => x.NumeroNivel == 0);
            return query.ToList<Tag>();
        }

        public IList<Tag> ObterPorFiltro(Tag pTag)
        {
            var query = repositorio.session.Query<Tag>();

            if (pTag != null)
            {
                if (!string.IsNullOrWhiteSpace(pTag.Nome))
                    query = query.Where(x => x.Nome.Contains(pTag.Nome));
            }

            return query.ToList<Tag>();
        }

        protected override bool ValidarDependencias(object pTag)
        {
            Tag tag = (Tag)pTag;

            
            return false;
        }

        public void ExcluirTag(Tag pTag)
        {
            if (ValidarDependencias(pTag))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Tag.");

            repositorio.Excluir(pTag);
        }

        private Tag ObterPorNome(Tag pPrograma)
        {
            //return repositorio.GetByProperty("Nome", pPrograma.Nome).FirstOrDefault();
            var query = repositorio.session.Query<Tag>();
            return query.FirstOrDefault(x => x.Nome == pPrograma.Nome);
        }
        
        public bool VerificarExistenciaPorNome(string nomeTag)
        {
            var query = repositorio.session.Query<Tag>();
            bool tagExiste = query.Any(x => x.Nome == nomeTag);
            return tagExiste;
        }
    }
}
