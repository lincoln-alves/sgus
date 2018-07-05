using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BusinessManagerBase
    {
        public void ValidarInstancia(object pObj)
        {
            if (pObj == null)
            {
                throw new Exception("Nenhum registro encontrado. Verifique se a Instância informada está nula");
            }
        }

        protected TransactionRequired Transacao { get; set; }

        protected virtual bool ValidarDependencias(object pParametro)
        {
            return false;
        }

        protected DataTable ObterTabela(IDataReader reader)
        {
            var tbEsquema = reader.GetSchemaTable();
            var tbRetorno = new DataTable();
            if (tbEsquema == null) return tbRetorno;
            foreach (DataRow r in tbEsquema.Rows)
            {
                if (tbRetorno.Columns.Contains(r["ColumnName"].ToString())) continue;
                var col = new DataColumn
                {
                    ColumnName = r["ColumnName"].ToString(),
                    Unique = Convert.ToBoolean(r["IsUnique"]),
                    AllowDBNull = Convert.ToBoolean(r["AllowDBNull"]),
                    ReadOnly = Convert.ToBoolean(r["IsReadOnly"])
                };
                tbRetorno.Columns.Add(col);
            }
            while (reader.Read())
            {
                var novaLinha = tbRetorno.NewRow();
                for (var i = 0; i < tbRetorno.Columns.Count; i++)
                {
                    novaLinha[i] = reader.GetValue(i);
                }
                tbRetorno.Rows.Add(novaLinha);
            }
            return tbRetorno;
        }

        public DataTable ExecutarProcedureTable(string pProcedureName, IDictionary<string, object> pParametros)
        {
            SqlConnection cnx = null;
            try
            {
                cnx = new SqlConnection(CommonHelper.ConnectionString);
                cnx.Open();

                var sqlCmd = new SqlCommand(pProcedureName, cnx)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (pParametros != null)
                {
                    foreach (var parametro in pParametros)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value ?? DBNull.Value));
                    }
                }

                var dr = sqlCmd.ExecuteReader();

                //RegistrarLogExecucao();

                return ObterTabela(dr);
            }
            finally
            {
                if (cnx != null && cnx.State == ConnectionState.Open)
                    cnx.Close();
            }
        }

        /// <summary>
        /// Executa a Stored Procedure, convertendo o resultado automaticamente na lista Tipada informada no parâmetro.
        /// </summary>
        /// <param name="pProcedureName">Nome da Procedure a ser evocada</param>
        /// <param name="pParametros">Lista de parâmetros a serem passados</param>
        /// <param name="timeout">Timeout da procedure</param>
        /// <returns></returns>
        public IList<V> ExecutarProcedure<V>(string pProcedureName, IDictionary<string, object> pParametros = null, int timeout = 36)
        {
            SqlConnection cnx = null;
            try
            {
                cnx = new SqlConnection(CommonHelper.ConnectionString);
                cnx.Open();

                var sqlCmd = new SqlCommand(pProcedureName, cnx)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = timeout
                };

                if (pParametros != null)
                {
                    foreach (var parametro in pParametros)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value ?? DBNull.Value));
                    }
                }

                var dr = sqlCmd.ExecuteReader();

                IList<V> lstResult = new List<V>();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var constructorInfo = typeof(V).GetConstructor(Type.EmptyTypes);
                        if (constructorInfo == null) continue;
                        var obj = constructorInfo.Invoke(null);

                        foreach (var property in typeof(V).GetProperties())
                        {
                            try
                            {
                                if (!ColunaExiste(dr, property.Name) ||
                                    dr[property.Name].GetType().Name.ToLower().IndexOf("null", StringComparison.Ordinal) > -1) continue;
                            }
                            catch
                            {
                                continue;
                            }

                            obj.GetType().GetProperty(property.Name).SetValue(obj, dr[property.Name], null);
                        }

                        lstResult.Add((V)obj);
                    }
                }
                //RegistrarLogExecucao();

                return lstResult;

            }
            finally
            {
                if (cnx != null && cnx.State == ConnectionState.Open)
                    cnx.Close();
            }
        }

        public void ExecutarProcedure(string pProcedureName, IDictionary<string, object> pParametros = null, int timeout = 36)
        {
            SqlConnection cnx = null;
            try
            {
                cnx = new SqlConnection(CommonHelper.ConnectionString);
                cnx.Open();

                var sqlCmd = new SqlCommand(pProcedureName, cnx)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = timeout
                };

                if (pParametros != null)
                {
                    foreach (var parametro in pParametros)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value ?? DBNull.Value));
                    }
                }

                sqlCmd.ExecuteReader();
            }
            finally
            {
                if (cnx != null && cnx.State == ConnectionState.Open)
                    cnx.Close();
            }
        }

        private static bool ColunaExiste(IDataReader reader, string nomeColuna)
        {
            var schemaTable = reader.GetSchemaTable();
            return schemaTable == null ? false : schemaTable.Rows.OfType<DataRow>().Any(row => row["ColumnName"].ToString() == nomeColuna);
        }
    }
}
