using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;

namespace Sebrae.Academico.BP
{
    public class ManterValorSistema : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMValorSistema bmValorSistema = null;

        #endregion

        #region "Construtor"

        public ManterValorSistema()
            : base()
        {
            bmValorSistema = new BMValorSistema();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<DTOValorSistema> ObterValorSistema(DateTime? dataInicio, DateTime? dataFim) {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("pDataInicio", (!dataInicio.HasValue ? "01/01/" + DateTime.Now.Year : dataInicio.Value.ToString("MM/dd/yyyy")) + " 00:00:00");
            lstParam.Add("pDataFim", (!dataFim.HasValue ? DateTime.Now.Date.ToString("MM/dd/yyyy") : dataFim.Value.ToString("MM/dd/yyyy")) + " 23:59:59");

            return bmValorSistema.ExecutarProcedure<DTOValorSistema>("DASHBOARD_REL_ValoresSistema", lstParam);
        }

        public void AtualizarValorSistema(ValorSistema pValorSistema)
        {
            base.PreencherInformacoesDeAuditoria(pValorSistema);
            bmValorSistema.Salvar(pValorSistema);
        }

        public void IncluirValorSistema(ValorSistema pValorSistema)
        {
            base.PreencherInformacoesDeAuditoria(pValorSistema);
            bmValorSistema.Salvar(pValorSistema);
        }

        public void IncluirValorSistema(IList<ValorSistema> pListaValorSistema)
        {
            pListaValorSistema.ToList().ForEach(x => base.PreencherInformacoesDeAuditoria(x));
            bmValorSistema.Salvar(pListaValorSistema);
        }

        public ValorSistema ObterValorSistemaPorID(int pId)
        {
            return bmValorSistema.ObterPorID(pId);
        }

        public IList<ValorSistema> ObterTodosValoresSistema()
        {
            return bmValorSistema.ObterTodos();
        }
        #endregion
    }
}
