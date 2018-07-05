using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcInscritosPorCategoria : ProcBase<DTOInscritosPorCategoria>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOInscritosPorCategoria.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOInscritosPorCategoria</returns>
        public IList<DTOInscritosPorCategoria> ObterInscritosPorCategoria(int? ano, int? uf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_REL_INSCRITOS_POR_CATEGORIA");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));

                if (uf.HasValue)
                    this.sqlCmd.Parameters.Add(new SqlParameter("@@ID_UF", uf));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOInscritosPorCategoria> lstResult = new List<DTOInscritosPorCategoria>();
                DTOInscritosPorCategoria dtoResultado = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    dtoResultado = ObterObjetoDTO(dr);
                    lstResult.Add(dtoResultado);
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

        #endregion

        private DTOInscritosPorCategoria ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOInscritosPorCategoria resultado = new DTOInscritosPorCategoria();

            resultado.Categoria = dr["NM_CategoriaConteudo"].ToString();
            resultado.QuantidadeInscritos = int.Parse(dr["Quantidade"].ToString());

            return resultado;
        }
    }
}
