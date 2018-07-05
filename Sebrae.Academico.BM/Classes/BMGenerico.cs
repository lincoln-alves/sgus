using System;
using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM
{
    /// <summary>
    /// Classe responsável por executar uma instrução SQL.
    /// </summary>
    public sealed class BMGenerico : BaseADONET
    {

        public DataSet ProcessarQuery(string instrucaoSQL)
        {

            DataSet dsResultado = new DataSet();

            try
            {

                base.AbrirConexao();

                this.sqlCmd = new SqlCommand(instrucaoSQL, this.cnx);
                this.sqlCmd.CommandType = CommandType.Text;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(this.sqlCmd);
                dataAdapter.Fill(dsResultado);

                return dsResultado;

            }
            catch (Exception ex)
            {
                throw new AcademicoException(string.Format("Ocorreu um erro na execução da instrução SQL. Erro Original: {0}",
                                                            ex.Message));
            }
            finally
            {
                base.FecharConexao();
            }
        }


    }
}
