using System;
using FluentMigrator.Builders.Execute;
using Sebrae.Academico.SQL.Enums;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Extensions
{
    public static class MigrationExtensions
    {
        public static void Procedure(this IExecuteExpressionRoot execute, string procedureName)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new Exception($"Procedure name can't be null or empty");
            }

            var preScriptProcedure = BasicScriptsResources.PreProcedureScript.Replace("#SCRIPT_NAME#", procedureName);

            var rm = StoredProceduresResources.ResourceManager;
            var script = rm.GetString(procedureName);

            if (string.IsNullOrWhiteSpace(script))
            {
                throw new Exception(
                    $"Procedure \"{procedureName}\" not found in {StoredProceduresResources.ResourceManager.BaseName}");
            }

            var finalScript = string.Concat(preScriptProcedure, script);

            execute.Sql(finalScript);
        }

        public static void Function(this IExecuteExpressionRoot execute, string functionName)
        {
            if (string.IsNullOrWhiteSpace(functionName))
            {
                throw new Exception($"Function name can't be null or empty");
            }

            var preScriptFunction = BasicScriptsResources.PreFunctionScript.Replace("#SCRIPT_NAME#", functionName);

            var rm = FunctionResources.ResourceManager;
            var script = rm.GetString(functionName);

            if (string.IsNullOrWhiteSpace(script))
            {
                throw new Exception(
                    $"Function \"{functionName}\" not found in {FunctionResources.ResourceManager.BaseName}");
            }

            var finalScript = string.Concat(preScriptFunction, script);

            execute.Sql(finalScript);
        }

        public static void RemoveForeignKey(this IExecuteExpressionRoot execute, string tableName, string columnName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Table name can't be null or empty");
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new Exception("Column name can't be null or empty");
            }

            var script =
                BasicScriptsResources.RemoveForeignKeyScript.Replace("#TABLE_NAME#", tableName)
                    .Replace("#COLUMN_NAME#", columnName);

            execute.Sql(script);
        }

        public static void RemovePrimaryKey(this IExecuteExpressionRoot execute, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Table name can't be null or empty");
            }

            var script = BasicScriptsResources.RemovePrimaryKeyScript.Replace("#TABLE_NAME#", tableName);

            execute.Sql(script);
        }

        public static void AdicionarPagina(this IExecuteExpressionRoot execute, string campoReferencia,
            TipoDeExpressaoAdicionarPagina tipoDeExpressaoAdicionarPagina, string sqlNovaPagina,
            string caminhoNovaPagina)
        {
            string expressao;

            switch (tipoDeExpressaoAdicionarPagina)
            {
                case TipoDeExpressaoAdicionarPagina.PeloCaminhoDoPai:
                    expressao = $"SELECT QT_Right FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{campoReferencia}'";
                    break;
                case TipoDeExpressaoAdicionarPagina.PeloNomeDoPai:
                    expressao = $"SELECT QT_Right FROM TB_Pagina WHERE TX_Nome LIKE '{campoReferencia}'";
                    break;
                case TipoDeExpressaoAdicionarPagina.PeloCaminhoDoIrmao:
                    expressao = $"SELECT QT_Right + 1 FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{campoReferencia}'";
                    break;
                default:
                    throw new Exception("Tipo de expressão não implementada.");
            }

            var script =
                BasicScriptsResources.AdicionarPaginaScript.Replace("#EXPRESSAO#", expressao)
                    .Replace("#CAMINHO_NOVA_PAGINA#", caminhoNovaPagina);

            // Abrir espaço pra nova página.
            execute.Sql(script);

            // Incluir nova página.
            execute.Sql(sqlNovaPagina);
        }

        public static void RemoverPagina(this IExecuteExpressionRoot execute, string caminhoRelativo)
        {
            var left = $"SELECT TOP 1 QT_Left FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{caminhoRelativo}'";

            var script = BasicScriptsResources.RemoverPaginaScript.Replace("#EXPRESSAO#", left);

            // Tirar o espaço da página removida.
            execute.Sql(script);

            // Remover página e sub-páginas.
            execute.Sql($"DELETE FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{caminhoRelativo}'");
        }

        public static string GerarScriptInsercaoMenu(this IExecuteExpressionRoot execute, bool paginaInicial,
            string estilo,
            string nome, string titulo, string caminhoRelativo, string icone, string descricao = null)
        {
            var script = "INSERT INTO (#COLUMN#) VALUES (#VALUES#)";

            AdicionarColuna("IN_PaginaInicial", paginaInicial, ref script);
            AdicionarColuna("TX_Estilo", estilo, ref script);
            AdicionarColuna("TX_Nome", nome, ref script);
            AdicionarColuna("TX_Titulo", titulo, ref script);
            AdicionarColuna("TX_CaminhoRelativo", caminhoRelativo, ref script);
            AdicionarColuna("TX_IconeMenu", icone, ref script);

            if (string.IsNullOrWhiteSpace(descricao) == false)
            {
                AdicionarColuna("TX_Descricao", descricao, ref script);
            }

            InserirDadosPadrao(ref script);

            return script.Replace(",#COLUMN#", "").Replace(",#VALUES#", "");
        }

        public static string GerarScriptInsercaoAgrupador(this IExecuteExpressionRoot execute, string nome, string icone,
            string descricao = null)
        {
            var script = "INSERT INTO (#COLUMN#) VALUES (#VALUES#)";

            AdicionarColuna("TX_Nome", nome, ref script);
            AdicionarColuna("TX_IconeMenu", icone, ref script);

            if (string.IsNullOrWhiteSpace(descricao) == false)
            {
                AdicionarColuna("TX_Descricao", descricao, ref script);
            }

            InserirDadosPadrao(ref script);

            return script.Replace(",#COLUMN#", "").Replace(",#VALUES#", "");
        }

        public static string GerarScriptInsercaoPagina(this IExecuteExpressionRoot execute, string nome, string titulo,
            string caminhoRelativo, TipoDeExpressaoAdicionarPagina tipoDeExpressaoAdicionarPagina,
            string campoReferencia,
            string descricao = null)
        {
            var script = "INSERT INTO TB_Pagina (#COLUMN#) VALUES (#VALUES#)";

            AdicionarColuna("TX_Nome", nome, ref script);
            AdicionarColuna("TX_Titulo", titulo, ref script);
            AdicionarColuna("TX_CaminhoRelativo", caminhoRelativo, ref script);

            if (string.IsNullOrWhiteSpace(descricao) == false)
            {
                AdicionarColuna("TX_Descricao", descricao, ref script);
            }

            string expressaoLeft, expressaoRight;

            switch (tipoDeExpressaoAdicionarPagina)
            {
                case TipoDeExpressaoAdicionarPagina.PeloCaminhoDoPai:
                    expressaoLeft =
                        $"SELECT TOP 1 QT_Right FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{campoReferencia}'";
                    expressaoRight =
                        $"SELECT TOP 1 QT_Right + 1 FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{campoReferencia}'";
                    break;
                case TipoDeExpressaoAdicionarPagina.PeloNomeDoPai:
                    expressaoLeft = $"SELECT TOP 1 QT_Right FROM TB_Pagina WHERE TX_Nome LIKE '{campoReferencia}'";
                    expressaoRight = $"SELECT TOP 1 QT_Right + 1 FROM TB_Pagina WHERE TX_Nome LIKE '{campoReferencia}'";
                    break;
                case TipoDeExpressaoAdicionarPagina.PeloCaminhoDoIrmao:
                    expressaoLeft =
                        $"SELECT TOP 1 QT_Right + 1 FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{campoReferencia}'";
                    expressaoRight =
                        $"SELECT TOP 1 QT_Right + 2 FROM TB_Pagina WHERE TX_CaminhoRelativo LIKE '{campoReferencia}'";
                    break;
                default:
                    throw new Exception("Tipo de expressão não implementada.");
            }

            AdicionarLeftRight(expressaoLeft, expressaoRight, ref script);

            InserirDadosPadrao(ref script);

            return script.Replace(",#COLUMN#", "").Replace(",#VALUES#", "");
        }

        private static void InserirDadosPadrao(ref string script)
        {
            AdicionarColuna("IN_TodosPerfis", true, ref script);
            AdicionarColuna("IN_TodasUfs", true, ref script);
            AdicionarColuna("IN_TodosNiveisOcupacionais", true, ref script);
        }

        private static void AdicionarColuna(string coluna, object valor, ref string script)
        {
            var valorString = ObterValorEmString(valor);

            script = script
                .Replace("#COLUMN#", $"{coluna},#COLUMN#")
                .Replace("#VALUES#", $"{valorString},#VALUES#");
        }

        private static void AdicionarLeftRight(string expressaoLeft, string expressaoRight, ref string script)
        {
            script = script
                .Replace("#COLUMN#", "QT_Left,#COLUMN#")
                .Replace("#COLUMN#", "QT_Right,#COLUMN#")
                .Replace("#VALUES#", $"({expressaoLeft}),#VALUES#")
                .Replace("#VALUES#", $"({expressaoRight}),#VALUES#");
        }

        private static string ObterValorEmString(object valor)
        {
            string valorString = null;

            if (valor is string)
            {
                valorString = $"'{valor}'";
            }

            if (valor is int)
            {
                valorString = valor.ToString();
            }

            if (valor is bool)
            {
                valorString = (bool) valor ? "1" : "0";
            }

            if (valor is DateTime)
            {
                var dateValue = (DateTime) valor;
                valorString =
                    $"'{dateValue.Year}-{dateValue.Month}-{dateValue.Day} {dateValue.Hour}:{dateValue.Minute}'";
            }

            if (valorString == null)
            {
                throw new Exception($"Tipo {valor.GetType().Name} não implementado na conversão do tipo.");
            }

            return valorString;
        }
    }
}