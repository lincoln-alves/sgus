using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;


namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcMatriculasPorMes : ProcBase<DTOMatriculasPorMes>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOMatriculasPorMes> ObterMatriculasPorMes(int? ano, int? idUf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_MATRICULAS_MES_POR_ANO_UF");

                //Passa o id do usuário para o parâmetro da procedure                
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));
                if (idUf > 0) { this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UF", idUf)); }

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOMatriculasPorMes> lstResult = new List<DTOMatriculasPorMes>();
                DTOMatriculasPorMes registro = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    registro = ObterObjetoDTO(dr);
                    lstResult.Add(registro);
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

        private DTOMatriculasPorMes ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOMatriculasPorMes registro = new DTOMatriculasPorMes();

            registro.Mes = dr["Mes"].ToString();
            registro.qtdCursoInCompany = int.Parse(dr["CursoInCompany"].ToString());
            registro.qtdCursoOnline = int.Parse(dr["CursoOnline"].ToString());

            return registro;
        }
    }
}
