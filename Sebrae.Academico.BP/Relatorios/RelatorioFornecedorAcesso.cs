using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioFornecedorAcesso : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.RelacaoDeFornecedores; }
        }

        public IList<DTORelatorioFornecedorAcesso> ConsultarForneceorAcesso(string pNome)
        {

            this.RegistrarLogExecucao();

            return (new ManterFornecedor()).ConsultarForneceorAcesso(pNome);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
