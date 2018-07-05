using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Data;
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.DTO
{
    [Serializable]
    public class DTOTotalizador
    {
        public string Descricao { get; set; }

        public object Dado { get; set; }

        public bool IsAgrupado { get; set; }

        public List<string> Cabecalhos { get; set; }

        public bool DadoIsLista
        {
            get { return Dado is List<DTOTotalizador> || Dado is IList<DTOTotalizador> || Dado is IEnumerable<DTOTotalizador>; }
        }


        /// <summary>
        /// Retorna o dado de acordo com o tipo que foi informado.
        /// </summary>
        /// <returns></returns>
        public string ObterDado()
        {
            if (Dado is int)
            {
                return Dado.ToString();
            }

            // Caso seja algum tipo de lista de totalizadores, retornar o dado em formato de table e concatena com o resultado.
            if (Dado is List<DTOTotalizador> || Dado is IList<DTOTotalizador> || Dado is IEnumerable<DTOTotalizador>)
            {
                var dadosList = ((List<DTOTotalizador>)Dado).ToList();

                var retorno = "";

                if (IsAgrupado) {
                    retorno += TotalizadorComposto(dadosList);
                } else {
                    retorno += "<table class=\"table-relatorio\">";
                    foreach (var dado in dadosList) {
                        retorno += "<tr><td>" + dado.Descricao + "</td><td>" + dado.ObterDado() + "</td></tr>";
                    }
                    retorno += "</table>";
                }

                return retorno;
            }

            return "N/D";
        }

        public string DadoFormatado
        {
            get { return ObterDado(); }
        }

        public string DadoReportViewer
        {
            get
            {
                if (Dado is int)
                {
                    return Dado.ToString();
                }

                // Caso seja algum tipo de lista de totalizadores, retornar o dado em formato de table e concatena com o resultado.
                if (DadoIsLista)
                {
                    var dadosList = ((List<DTOTotalizador>)Dado).ToList();

                    var retorno = "";

                    var generalSpace = "  ";

                    if (!IsAgrupado)
                    {
                        for (var i = 0; i < dadosList.Select(x => x.DadoReportViewer.Length).Max(); i++)
                        {
                            generalSpace += "  ";
                        }
                    }

                    foreach (var dado in dadosList)
                    {
                        var spaces = generalSpace;

                        if (IsAgrupado)
                        {
                            var intTotal = 0;
                            retorno += dado.Descricao;
                            retorno += Environment.NewLine;
                            var lista = ((IOrderedEnumerable<DTOTotalizador>)dado.Dado).ToList();
                            foreach (var dadoInterno in lista)
                            {
                                retorno += dadoInterno.Descricao + " -             " + dadoInterno.Dado;
                                retorno += Environment.NewLine;
                                intTotal += (int)dadoInterno.Dado;
                            }
                            retorno += "Total -             " + intTotal;
                            retorno += Environment.NewLine;
                            retorno += "-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
                            retorno += Environment.NewLine;
                        } else
                        {
                            // Remove um espaço para cada caractere existente no dado.
                            for (var i = 1; i < dado.DadoReportViewer.Length; i++)
                                spaces = spaces.Remove(0, 2);

                            retorno += dado.Descricao + " - " + spaces + dado.DadoReportViewer;
                            retorno += Environment.NewLine;
                        }                       
                    }
        
                    return retorno;
                }
                
                return "N/D";
            }
        }


        public string TotalizadorComposto(List<DTOTotalizador> listaTotalizador)
        {
            var retorno = "<table class=\"table-relatorio\">";
            retorno += TotalizadorCompostoCabecalho();
            var total = 0;
            foreach (var dadoExterno in listaTotalizador)
            {
                retorno += "<tr>";
                retorno += "<td>" + dadoExterno.Descricao+"</td>";

                foreach (var cabecalho in Cabecalhos)
                {
                    var obj = ((IOrderedEnumerable<DTOTotalizador>)dadoExterno.Dado).FirstOrDefault(x => x.Descricao == cabecalho);

                    if (obj != null)
                    {
                        retorno += "<td>" + obj.Dado + "</td>";
                        total += (int)obj.Dado;
                    }
                    else
                    {
                        retorno += "<td>0</td>";
                    }
                }

                retorno += "<td>" + total + "</td>";
                retorno += "</tr>";
            }

            retorno += "</table>";
            return retorno;
        }

        private string TotalizadorCompostoCabecalho()
        {
            var retorno = "<tr><td>&nbsp;</td>";

            foreach (var cabecalho in Cabecalhos)
            {
                retorno += "<td>" + cabecalho + "</td>";
            }

            retorno += "<td>Total</td></tr>";
            return retorno;
        }

    }

}
