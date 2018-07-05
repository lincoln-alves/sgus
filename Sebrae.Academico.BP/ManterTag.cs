using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterTag : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMTag bmTag;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTag()
            : base()
        {
            bmTag = new BMTag();
        }


        #endregion

        #region "Métodos Públicos"

        public void IncluirTag(Tag ptag)
        {
            base.PreencherInformacoesDeAuditoria(ptag);
            bmTag.Salvar(ptag);
        }

        public void AlterarTag(Tag ptag)
        {
            base.PreencherInformacoesDeAuditoria(ptag);
            bmTag.Salvar(ptag);
        }

        public IList<Tag> ObterTodasTag()
        {
            return bmTag.ObterTodos();
        }

        public IList<Tag> ObterTodasTagNaoSinonimas()
        {
            return bmTag.ObterTodasTagNaoSinonimas();
        }

        public Tag ObterTagPorID(int pId)
        {
            return bmTag.ObterPorID(pId);
        }

        public bool VerificarExistenciaPorNome(string nomeTag)
        {

            if (string.IsNullOrWhiteSpace(nomeTag))
            {
                throw new AcademicoException("Nome. Campo Obrigatório");
            }

            return bmTag.VerificarExistenciaPorNome(nomeTag);
        }
        
        public void ExcluirTag(int IdTag)
        {
            try
            {
                Tag tag = null;

                if (IdTag > 0)
                {
                    tag = bmTag.ObterPorID(IdTag);
                }

                bmTag.ExcluirTag(tag);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<Tag> ObterTagPorFiltro(Tag pTag)
        {
            return bmTag.ObterPorFiltro(pTag);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        #endregion

    }
}
