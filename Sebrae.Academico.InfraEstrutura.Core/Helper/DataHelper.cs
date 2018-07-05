using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class DataHelper
    {

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            table.TableName = "ListaConvertida";
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public static DataTable OrdenarLista(DataTable pLista, string Coluna)
        {
            DataRow[] aDr = pLista.Copy().Select("", Coluna);
            DataTable result = new DataTable("TabelaOrdenada");
            foreach (DataColumn dc in pLista.Columns)
            {
                result.Columns.Add(new DataColumn(dc.ColumnName,dc.DataType));
            }


            foreach (DataRow dr in aDr)
            {
                DataRow dri = result.NewRow();

                foreach (DataColumn dc in result.Columns)
                {
                    dri[dc.ColumnName] = dr[dc.ColumnName];
                }

                result.Rows.Add(dri);

            }


            return result;
        }

        
    }
}
