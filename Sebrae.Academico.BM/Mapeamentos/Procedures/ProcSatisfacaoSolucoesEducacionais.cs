using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcSatisfacaoSolucoesEducacionais : ProcBase<DtoSatisfacaoSolucoesEducacionais>
    {
        public IList<DtoSatisfacaoSolucoesEducacionais> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_SATISFACAO_SOLUCOES_EDUCACIONAIS");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoSatisfacaoSolucoesEducacionais>();

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    lstResult.Add(ObterObjetoDTO(dr));
                }

                return lstResult.Where(r => r.Concluintes > 0).ToList();

            }
            catch
            {
                throw new AcademicoException("Ocorreu um erro na execução da Solicitação");
            }
            finally
            {
                base.FecharConexao();
            }


        }

        private DtoSatisfacaoSolucoesEducacionais ObterObjetoDTO(System.Data.IDataReader dr)
        {
            var row = new DtoSatisfacaoSolucoesEducacionais();

            row.SolucaoEducacional = dr["SolucaoEducacional"].ToString();
            row.Concluintes = int.Parse(dr["Concluintes"].ToString());

            if (dr["Satisfacao"].ToString() != string.Empty)
                row.Satisfacao = double.Parse(dr["Satisfacao"].ToString());

            return row;
        }
    }
}
