using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcSolucoesEducacionaisPorCategoria : ProcBase<DtoSolucoesEducacionaisPorCategoria>
    {
        public IList<DtoSolucoesEducacionaisPorCategoria> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_SolucoesEducacionaisPorCategoria");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoSolucoesEducacionaisPorCategoria>();

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

        private DtoSolucoesEducacionaisPorCategoria ObterObjetoDTO(System.Data.IDataReader dr)
        {

            var row = new DtoSolucoesEducacionaisPorCategoria();

            row.Categoria = dr["Categoria"].ToString();
            row.SolucaoEducacional = dr["SolucaoEducacional"].ToString();
            row.Concluintes = int.Parse(dr["Concluintes"].ToString());

            return row;
        }



    }
}
