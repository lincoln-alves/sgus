using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Util.Classes
{
    public static class TotalizadorUtil
    {
        public static DTOTotalizador GetTotalizador<T>(IEnumerable<T> lista, string descricao, string coluna,
            enumTotalizacaoRelatorio operacao, bool possuiSubDados = true, string campo = null, object valor = null)
        {
            return GetTotalizador(lista.ToList(), descricao, coluna, operacao, possuiSubDados, campo, valor);
        }

        public static DTOTotalizador GetTotalizador<T>(IList<T> lista, string descricao, string coluna,
            enumTotalizacaoRelatorio operacao, bool possuiSubDados = true, string campo = null, object valor = null)
        {
            return GetTotalizador(lista.ToList(), descricao, coluna, operacao, possuiSubDados, campo, valor);
        }

        public static DTOTotalizador GetTotalizador<T>(List<T> lista, string descricao, string coluna,
            enumTotalizacaoRelatorio operacao, bool possuiSubDados = true, string campo = null, object valor = null)
        {

            var totalizador = new DTOTotalizador
            {
                Descricao = descricao
            };

            var colunas = coluna.Split(',');

            switch (operacao)
            {
                case enumTotalizacaoRelatorio.Somar:
                    if (possuiSubDados)
                    {
                        // Ainda não necessário.
                    }
                    else
                    {
                        int sum;

                        totalizador.Dado = lista.Select(x => x.GetType().GetProperty(coluna).GetValue(x))
                            .Sum(x => (int.TryParse(x.ToString(), out sum) ? sum : 0));
                    }
                    break;
                case enumTotalizacaoRelatorio.Contar:
                    if (possuiSubDados)
                    {
                        totalizador.Dado =
                            lista
                                .Select(x => x.GetType().GetProperty(coluna).GetValue(x)).Distinct().OrderBy(x => x)
                                .Select(x => new DTOTotalizador
                                {
                                    Descricao = x.ToString(),
                                    Dado =
                                        lista
                                            // Caso o parâmetro "campo" seja informado, verifica se o valor desse campo não seja nulo, e, caso não seja, soma ao contador.
                                            .Where(
                                                y =>
                                                    campo == null ||
                                                    y.GetType().GetProperty(campo).GetValue(y) != null)
                                            .Count(
                                                y =>
                                                    y.GetType().GetProperty(coluna).GetValue(y).ToString() ==
                                                    x.ToString())
                                }).ToList();
                    }
                    else
                    {
                        totalizador.Dado = lista
                            // Caso o parâmetro "campo" seja informado, verifica se o valor desse campo não seja nulo, e, caso não seja, soma ao contador.
                            .Where(
                                x =>
                                    campo == null ||
                                    x.GetType().GetProperty(campo).GetValue(x) != null)
                            .Select(x => x.GetType().GetProperty(coluna).GetValue(x))
                            .Count();
                    }
                    break;
                case enumTotalizacaoRelatorio.ContarDistintos:

                    foreach (var col in colunas)
                    {
                        if (possuiSubDados)
                        {
                            var retorno = totalizador.Dado == null
                                ? new List<DTOTotalizador>()
                                : (List<DTOTotalizador>) totalizador.Dado;

                            retorno.AddRange(
                                lista.Select(x => x.GetType().GetProperty(col).GetValue(x)).Distinct().OrderBy(x => x)
                                    .Select(x => new DTOTotalizador
                                    {
                                        Descricao = x.ToString(),
                                        Dado =
                                            lista.Where(
                                                y => y.GetType().GetProperty(col).GetValue(y).ToString() == x.ToString())
                                                .Distinct()
                                                .Count()
                                    }).ToList());

                            totalizador.Dado = retorno;
                        }
                        else
                        {
                            var ct = totalizador.Dado == null ? 0 : int.Parse(totalizador.Dado.ToString());

                            ct += lista.Where(x => x.GetType().GetProperty(col).GetValue(x) != null)
                                .Select(x => x.GetType().GetProperty(col).GetValue(x))
                                .Distinct().Count();

                            totalizador.Dado = ct;
                        }
                    }

                    break;
                case enumTotalizacaoRelatorio.ContarDistintosPorValor:
                    if (possuiSubDados)
                    {
                        // Ainda não necessário.
                    }
                    else
                    {
                        if (campo == null || valor == null)
                            break;

                        totalizador.Dado = lista.Where(x => x.GetType().GetProperty(campo).GetValue(x).ToString() == valor.ToString())
                            .Select(x => x.GetType().GetProperty(coluna).GetValue(x))
                            .Distinct()
                            .Count();
                    }

                    break;
            }

            return totalizador;
        }

        public static DTOTotalizador GetTotalizadorSimples(string descricao, object valor = null)
        {
            return new DTOTotalizador
            {
                Descricao = descricao,
                Dado = valor
            };
        }

        public static DTOTotalizador GetTotalizadorComposto<T>(IEnumerable<T> lista, string descricao,
            string campoCabecalho, string row)
        {

            return  new DTOTotalizador
            {
                Descricao = descricao,
                IsAgrupado = true,
                Cabecalhos = lista.Select(x => x.GetType().GetProperty(campoCabecalho).GetValue(x).ToString()).Distinct()
                                .OrderBy(x => x.GetType().GetProperty(campoCabecalho)).ToList(),
                Dado = lista.GroupBy(x => x.GetType().GetProperty(row).GetValue(x).ToString()).OrderBy(x => x.GetType().GetProperty(row)).Select(child => new DTOTotalizador() { Descricao = child.Key, IsAgrupado = true, Dado = child.GroupBy(y => y.GetType().GetProperty(campoCabecalho).GetValue(y).ToString()).Select(childy => new DTOTotalizador() { Descricao = childy.Key, IsAgrupado = true, Dado = childy.Count() }).OrderBy(or => or.Descricao) }).ToList()
            };
        }
    }
}
