using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.WebForms.Helpers
{
    public static class Util
    {
        public static string ObterListaAutocomplete(IQueryable<CategoriaMoodle> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.AsEnumerable().Select(
                        x =>
                            new
                            {
                                data = x.ID.ToString(),
                                value = x.Nome,
                                exibirUf = false
                            }));
        }
        public static string ObterListaAutocomplete(IQueryable<SolucaoEducacional> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.AsEnumerable().Select(
                        x =>
                            new
                            {
                                data = x.ID.ToString(),
                                value = x.Nome,
                                uf = x.UFGestor != null ? x.UFGestor.Sigla : null
                            }));
        }

        public static string ObterListaAutocomplete(IQueryable<Oferta> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(
                        x =>
                            new
                            {
                                data = x.ID.ToString(),
                                value = x.Nome,
                                uf = x.SolucaoEducacional.UFGestor != null ? x.SolucaoEducacional.UFGestor.Sigla : "N/D"
                            }));
        }

        public static string ObterListaAutocomplete(IQueryable<Turma> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.OrderBy(x => x.Nome).Select(
                        x =>
                            new
                            {
                                data = x.ID.ToString(),
                                value = x.Nome,
                                uf =
                                    x.Oferta.SolucaoEducacional.UFGestor != null
                                        ? x.Oferta.SolucaoEducacional.UFGestor.Sigla
                                        : "N/D"
                            }));
        }

        public static string ObterListaAutocomplete(IQueryable<Programa> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(x => new {data = x.ID.ToString(), value = x.Nome, exibirUf = false}));
        }

        public static string ObterListaAutocomplete(IQueryable<Capacitacao> lista)
        {
            if (lista == null)
                return null;

            return new JavaScriptSerializer().Serialize(lista.Select(x => new {data = x.ID.ToString(), value = x.Nome}));
        }

        public static string ObterListaAutocomplete(IQueryable<Modulo> lista)
        {
            if (lista == null)
                return null;

            return new JavaScriptSerializer().Serialize(lista.Select(x => new {data = x.ID.ToString(), value = x.Nome}));
        }

        public static string ObterListaAutocomplete(IList<Questionario> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(x => new {data = x.ID.ToString(), value = x.Nome, exibirUf = false}));
        }

        public static string ObterListaAutocomplete(IList<Processo> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(x => new { data = x.ID.ToString(), value = x.Nome, exibirUf = false }));
        }

        public static StringBuilder ObterListaPrograma(IList<Programa> lista)
        {
            var sb = new StringBuilder();

            foreach (var item in lista.ToList())
            {
                if (sb.Length != 0)
                {
                    sb.Append(",");
                }
                sb.Append("'" + item.ID + "'" + " : " + "'" + item.Nome + "'");
            }

            return sb;
        }

        public static string ObterListaAutocomplete(IQueryable<Usuario> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(x => new {data = x.ID.ToString(), value = x.Nome, exibirUf = false}));
        }

        public static string ObterListaAutocomplete(IQueryable<TrilhaNivel> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(x => new { data = x.ID.ToString(), value = x.Nome, exibirUf = false }));
        }

        public static string ObterListaAutocomplete(IQueryable<Trilha> lista)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.Select(x => new { data = x.ID.ToString(), value = x.Nome, exibirUf = false }));
        }

        public static bool ObterVigente(DateTime? dtIni, DateTime? dtFim)
        {
            return ((!dtIni.HasValue || dtIni <= DateTime.Now) && (!dtFim.HasValue || dtFim >= DateTime.Now));
        }
    }
}