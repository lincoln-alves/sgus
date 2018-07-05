using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterMetaIndividual : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMMetaIndividual bmMetaIndividual = null;

        #endregion

        #region "Construtor"

        public ManterMetaIndividual()
            : base()
        {
            bmMetaIndividual = new BMMetaIndividual();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<MetaIndividual> ObterPorDataValidade(string pNome, Usuario pIdUsuario, DateTime? pDataValidadeInicial, DateTime? pDataValidadeFinal)
        {
            return bmMetaIndividual.ObterPorDataValidade(pNome, pIdUsuario, pDataValidadeInicial, pDataValidadeFinal);
        }

        public void ExcluirMetaIndividual(int IdMetaIndividual)
        {

            try
            {
                MetaIndividual metaIndividual = null;

                if (IdMetaIndividual > 0)
                {
                    metaIndividual = bmMetaIndividual.ObterPorID(IdMetaIndividual);
                }

                bmMetaIndividual.Excluir(metaIndividual);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public MetaIndividual ObterPorID(int pId)
        {
            return bmMetaIndividual.ObterPorID(pId);
        }

        public void Salvar(MetaIndividual pMetaIndividual)
        {
            bmMetaIndividual.Salvar(pMetaIndividual);
        }

        #endregion

    }
}
