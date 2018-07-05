using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterRelatorioPaginaInicial : BusinessProcessBase
    {

        private BMRelatorioPaginaInicial bmRelatorioPaginaInicial = null;

        public ManterRelatorioPaginaInicial()
            : base()
        {
            bmRelatorioPaginaInicial = new BMRelatorioPaginaInicial();
        }

        public void Salvar(RelatorioPaginaInicial model)
        {
            try
            {
                bmRelatorioPaginaInicial.Salvar(model);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void Excluir(int IdModel)
        {
            try
            {
                RelatorioPaginaInicial relatorioPaginaInicial = null;

                if (IdModel > 0)
                {
                    relatorioPaginaInicial = bmRelatorioPaginaInicial.ObterPorId(IdModel);
                    bmRelatorioPaginaInicial.Excluir(relatorioPaginaInicial);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<RelatorioPaginaInicial> ObterTodas()
        {
            return bmRelatorioPaginaInicial.ObterTodos();
        }

        public RelatorioPaginaInicial ObterPorID(int IdModel)
        {
            return bmRelatorioPaginaInicial.ObterPorId(IdModel);
        }

        public RelatorioPaginaInicial ObterPorTagRelatorio(string nomeRelatorio)
        {
            return bmRelatorioPaginaInicial.ObterPorTagRelatorio(nomeRelatorio);
        }
    }
}
