using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public abstract class BaseADONET
    {

        #region "Atributos Privados

        protected IDataReader dr;
        protected SqlCommand sqlCmd;
        protected SqlConnection cnx;

        #endregion

        protected internal void AbrirConexao()
        {
            this.cnx = new SqlConnection(CommonHelper.ConnectionString);
            this.cnx.Open();
        }

        protected internal void FecharConexao()
        {
            if (cnx.State == ConnectionState.Open)
                cnx.Close();
        }
    }
}
