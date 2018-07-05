using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterMetaInstitucional : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMMetaInstitucional bmMetaInstitucional = null;

        #endregion

        #region "Construtor"

        public ManterMetaInstitucional()
            : base()
        {
            bmMetaInstitucional = new BMMetaInstitucional();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<MetaInstitucional> PesquisarMetasInstitucionais(Dominio.Classes.MetaInstitucional mi)
        {
            return bmMetaInstitucional.ObterPorFiltro(mi);
        }

        public void Salvar(MetaInstitucional mi)
        {
            base.PreencherInformacoesDeAuditoria(mi);
            bmMetaInstitucional.Salvar(mi);
        }

        public MetaInstitucional ObterPorID(int pId)
        {
            return bmMetaInstitucional.ObterPorID(pId);
        }

        public void ExcluirMetaInstitucional(int IdMetaInstitucional)
        {

            try
            {
                MetaInstitucional metaInstitucional = null;

                if (IdMetaInstitucional > 0)
                {
                    metaInstitucional = bmMetaInstitucional.ObterPorID(IdMetaInstitucional);
                }

                bmMetaInstitucional.Excluir(metaInstitucional);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
