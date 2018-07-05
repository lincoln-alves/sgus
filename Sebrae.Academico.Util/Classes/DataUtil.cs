using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sebrae.Academico.Util.Classes
{
    public class DataUtil
    {
        public static DateTime AjustarTimeZoneBR(DateTime data)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTime(data, timeZone);
        }
        public static DateTime CalcularPrazo(DateTime dataBase, int diasUteis)
        {
            DateTime dataBaseReal = dataBase;

            int diasUteisRestantes = diasUteis;
            dataBaseReal = AjustarInicio(dataBaseReal);

            DateTime dataPrazo = dataBaseReal;

            while (diasUteisRestantes > 0)
            {
                dataPrazo = dataPrazo.AddDays(1);
                dataPrazo = DesconsiderarFimDeSemana(dataPrazo);
                diasUteisRestantes--;
            }

            return dataPrazo;
        }

        private static DateTime AjustarInicio(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday)
            {
                data = data.AddDays(2);
                data = new DateTime(data.Year, data.Month, data.Day, 9, 0, 0);
            }
            else if (data.DayOfWeek == DayOfWeek.Sunday)
            {
                data = data.AddDays(1);
                data = new DateTime(data.Year, data.Month, data.Day, 9, 0, 0);
            }

            if (data.Hour > 18)
            {
                data = data.AddDays(1);
                data = new DateTime(data.Year, data.Month, data.Day, 9, 0, 0);
            }

            if (data.Hour < 9)
            {
                data = new DateTime(data.Year, data.Month, data.Day, 9, 0, 0);
            }

            return data;
        }

        private static DateTime DesconsiderarFimDeSemana(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday)
            {
                data = data.AddDays(2);
            }
            else if (data.DayOfWeek == DayOfWeek.Sunday)
            {
                data = data.AddDays(1);
            }
            return data;
        }

        public static double ToUnix(DateTime data)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(data) - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DataTable ToDataTable<T>(IList<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }
}
