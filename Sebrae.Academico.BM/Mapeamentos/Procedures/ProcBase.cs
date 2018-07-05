using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public abstract class ProcBase<T>: BaseADONET
                where T : class
    {
     
        protected void DefinirNomeProcedure(string nomeProcedure)
        {
            this.sqlCmd = new SqlCommand(nomeProcedure, this.cnx);
            this.sqlCmd.CommandType = CommandType.StoredProcedure;
        }

        protected IList<T> ExecutarProcedure(string pProcedureName, IDictionary<string, object> pParametros)
        {
            try
            {

                cnx = new SqlConnection(CommonHelper.ConnectionString);
                cnx.Open();

                sqlCmd = new SqlCommand(pProcedureName, cnx);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (pParametros != null)
                {
                    foreach (var parametro in pParametros)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value == null ? DBNull.Value : parametro.Value));
                    }
                }

                dr = sqlCmd.ExecuteReader();

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

                RegistrarLogExecucao();

                return lstResult;

            }
            finally
            {
                if (cnx.State == ConnectionState.Open)
                    cnx.Close();
            }
        }


        private bool ColunaExiste(IDataReader reader, string nomeColuna)
        {
            return reader.GetSchemaTable()
                   .Rows
                   .OfType<DataRow>()
                   .Any(row => row["ColumnName"].ToString() == nomeColuna);
        }

        protected void RegistrarLogExecucao()
        {
            try
            {
                using (BMLogGeracaoRelatorio lgExec = new BMLogGeracaoRelatorio())
                {
                    int IdUsuarioLogado = ((Usuario)HttpContext.Current.Session["usuarioSGUS"]).ID;
                    LogGeracaoRelatorio lg = new LogGeracaoRelatorio()
                    {
                        DTGeracao = DateTime.Now,
                        //IDRelatorio = this.IdRelatorio,
                        Usuario = new Usuario() { ID = IdUsuarioLogado }
                        //IDUsuario = IdUsuarioLogado
                    };

                    lgExec.Salvar(lg);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
