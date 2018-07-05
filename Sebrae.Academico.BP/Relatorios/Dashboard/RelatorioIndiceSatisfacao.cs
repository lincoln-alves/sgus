using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioIndiceSatisfacao : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOIndiceSatisfacao> ObterIndiceSatisfacao(DateTime? dataInicio, DateTime? dataFim, int idUf)
        {
            this.RegistrarLogExecucao();

            return (new ManterQuestionario()).ObterIndiceSatisfacao(dataInicio, dataFim, idUf);
        }
        public IList<DTOIndiceSatisfacao> ObterIndiceSatisfacaoCredenciados(DateTime? dataInicio, DateTime? dataFim,int idUf)
        {
            this.RegistrarLogExecucao();

            return (new ManterQuestionario()).ObterIndiceSatisfacaoCredenciados(dataInicio, dataFim, idUf);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
