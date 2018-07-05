using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
        
    public sealed class ProcTotalMatriculasPorAno : ProcBase<DTOMatriculaOferta>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOTotalMatriculaOfertaPorAno.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOTotalMatriculaOfertaPorAno</returns>
        public IList<DTOTotalMatriculaOfertaPorAno> ObterTotalMatriculasNoAnoPorUf(int? ano, int? idUf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_TOTAL_MATRICULAS_POR_ANO_UF");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UF", idUf));
                   
                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOTotalMatriculaOfertaPorAno> lstResult = new List<DTOTotalMatriculaOfertaPorAno>();
                DTOTotalMatriculaOfertaPorAno totalMatriculaOfertaPorAno = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    totalMatriculaOfertaPorAno = ObterObjetoDTO(dr);
                    lstResult.Add(totalMatriculaOfertaPorAno);
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

        private DTOTotalMatriculaOfertaPorAno ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOTotalMatriculaOfertaPorAno matriculaOferta = new DTOTotalMatriculaOfertaPorAno();

            matriculaOferta.SiglaUf = dr["SG_UF"].ToString();
            matriculaOferta.Quantidade = int.Parse(dr["QTD"].ToString());

            return matriculaOferta;
        }


    }
}
