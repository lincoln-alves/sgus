using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcSolucoesEConcluintesPorCategoria : ProcBase<DtoSolucoesEConcluintesPorCategoria>
    {
        public IList<DtoSolucoesEConcluintesPorCategoria> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_SOLUCOES_E_CONCLUINTES_POR_CATEGORIA");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoSolucoesEConcluintesPorCategoria>();

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    lstResult.Add(ObterObjetoDTO(dr));
                }

                return lstResult;

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

        private DtoSolucoesEConcluintesPorCategoria ObterObjetoDTO(System.Data.IDataReader dr)
        {
            var row = new DtoSolucoesEConcluintesPorCategoria();

            row.Categoria = dr["Categoria"].ToString();
            row.QtdSolucoes = int.Parse(dr["QtdSolucoes"].ToString());
            row.Concluintes = int.Parse(dr["Concluintes"].ToString());

            return row;
        }


    }
}
