using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Data;

namespace Sebrae.Academico.Util.Classes
{
    public static class SQLUtil
    {
        public static IList<T> ExecutarProcedure<T>(string nomeProcedure, IDictionary<string, object> parametros)
        {
            SqlConnection cnx = null;
            SqlCommand sqlCmd = null;

            try
            {

                cnx = new SqlConnection(CommonHelper.ConnectionString);
                cnx.Open();

                sqlCmd = new SqlCommand(nomeProcedure, cnx);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value));
                    }
                }

                var dr = sqlCmd.ExecuteReader();

                IList<T> lstResult = new List<T>();

                while (dr.Read())
                {

                    var obj = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null);

                    foreach (var property in typeof(T).GetProperties())
                    {
                        // if (dr[property.Name].GetType().Name.ToLower().IndexOf("null") > -1) continue;
                        //if (dr[property.Name] == DBNull.Value) continue;
                        //if (!dr.GetSchemaTable().Columns.Contains(property.Name)) continue;

                        try
                        {
                            if (!ColunaExiste(dr, property.Name) ||
                                dr[property.Name].GetType().Name.ToLower().IndexOf("null") > -1) continue;
                        }
                        catch
                        {
                            continue;
                        }

                        obj.GetType().GetProperty(property.Name).SetValue(obj, dr[property.Name], null);
                    }

                    lstResult.Add((T)obj);

                }

                return lstResult;

            }
            finally
            {
                if (cnx.State == ConnectionState.Open)
                    cnx.Close();
            }
        }

        private static bool ColunaExiste(IDataReader reader, string nomeColuna)
        {
            return reader.GetSchemaTable()
                   .Rows
                   .OfType<DataRow>()
                   .Any(row => row["ColumnName"].ToString() == nomeColuna);
        }
    }
}
