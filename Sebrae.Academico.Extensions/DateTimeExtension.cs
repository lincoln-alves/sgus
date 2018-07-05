using System;

namespace Sebrae.Academico.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            var modTicks = dt.Ticks % d.Ticks;
            var delta = modTicks != 0 ? d.Ticks - modTicks : 0;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        public static DateTime RoundToNearest(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;
            var offset = roundUp ? d.Ticks : 0;

            return new DateTime(dt.Ticks + offset - delta, dt.Kind);
        }
        /// <summary>
        /// DateTime.Date.Between(DateTime date1, DateTime date2)
        /// </summary>
        /// <param name="date1">Data menor para comparação.</param>
        /// <param name="date2">Data maior para comparação.</param>
        /// <returns>O metodo retorna true ou false, dependendo de se o objeto for menor ou igual ou maior ou igual as datas referidas.</returns>
        public static bool Between(this DateTime input, DateTime date1, DateTime date2)
        {
            return (input >= date1 && input <= date2);
        }

        /// <summary>
        /// DateTime.Date.CalcularPrazo(int diasUteis)
        /// </summary>
        /// <param name="diasUteis">Calcula e retorna a data certa com dias úteis inseridos, em base no datetime extendido.</param>
        /// <returns>O metodo retorna DateTime com os dias cálculados.</returns>
        public static DateTime CalcularPrazo(this DateTime dataBase, int diasUteis)
        {
            DateTime dataBaseReal = AjustarInicio(dataBase);
            for (int dia = diasUteis; dia > 0; dia--)
            {
                dataBaseReal = dataBaseReal.AddDays(1).JumpWeekend();
                dia--;
            }
            return dataBaseReal;
        }

        /// <summary>
        /// DateTime?.Date.CalcularPrazo(int diasUteis)
        /// </summary>
        /// <param name="diasUteis">Calcula e retorna a data certa com dias úteis inseridos, em base no datetime extendido.</param>
        /// <returns>O metodo retorna DateTime com os dias cálculados.</returns>
        public static DateTime? CalcularPrazo(this DateTime? dataBase, int diasUteis)
        {
            if (dataBase.HasValue)
                return dataBase.Value.CalcularPrazo(diasUteis);
            return null;
        }

        /// <summary>
        /// Ajusta cálculo de ínicio da contagem de días uteis.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static DateTime AjustarInicio(DateTime data)
        {
            if (data.Hour > 18)
                data = data.AddDays(1);
            
            data = data.JumpWeekend();

            return new DateTime(data.Year, data.Month, data.Day, 9, 0, 0);
        }
        /// <summary>
        /// Calcula os dias desconsiderando os Finais de Semana
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime JumpWeekend(this DateTime data)
        {
            switch (data.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return data.AddDays(2);
                case DayOfWeek.Sunday:
                    return data.AddDays(1);
                default:
                    return data;
            }
        }

        public static string ObterNomeMes(this DateTime data)
        {
            switch (data.Month)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
            }

            return "";
        }
    }
}
