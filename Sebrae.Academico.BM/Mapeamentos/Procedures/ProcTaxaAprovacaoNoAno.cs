using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
        
    public sealed class ProcTaxaAprovacaoNoAno : ProcBase<DTOMatriculaOferta>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculaOferta.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOferta</returns>
        public IList<DTOMatriculaOferta> ObterTaxaDeAprovacaoNoAno(int? ano, int? idUf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_TAXA_DE_APROVACAO_NO_ANO");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UF", idUf));

                //Parâmetro de output - INÍCIO
                SqlParameter parametroTotalInscritos = new SqlParameter();
                parametroTotalInscritos.SqlDbType = System.Data.SqlDbType.Int;
                parametroTotalInscritos.ParameterName = "@TOTALMATRICULADOS";
                parametroTotalInscritos.Direction = System.Data.ParameterDirection.Output;
                //Parâmetro de output - FIM

                this.sqlCmd.Parameters.Add(parametroTotalInscritos);
                
                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                int totalInscritos = int.Parse(sqlCmd.Parameters["@TOTALMATRICULADOS"].Value.ToString());

                IList<DTOMatriculaOferta> lstResult = new List<DTOMatriculaOferta>();
                DTOMatriculaOferta matriculaOferta = null;

                //TODO: Nardo - Ver como chamar o parâmetro de output
                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    matriculaOferta = ObterObjetoDTO(dr);
                    lstResult.Add(matriculaOferta);
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

        private DTOMatriculaOferta ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOMatriculaOferta matriculaOferta = new DTOMatriculaOferta();

            matriculaOferta.NomeStatusMatricula = dr["NM_StatusMatricula"].ToString();
            matriculaOferta.PercentualMatriculados = double.Parse(dr["PERCENTUAL"].ToString());
            //matriculaOferta.QuantidadeMatriculados = int.Parse(dr["QTD"].ToString());

            return matriculaOferta;
        }


    }
}
