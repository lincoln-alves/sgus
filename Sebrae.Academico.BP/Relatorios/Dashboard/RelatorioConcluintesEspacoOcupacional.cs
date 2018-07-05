using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioConcluintesEspacoOcupacional : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOConcluintesEspacoOcupacional> ObterConcluintesEmpregados(DateTime inicio, DateTime fim, int idUf)
        {
            RegistrarLogExecucao();

            var manterMatricula = new ManterMatricula();
            return AgruparDadosInternos(manterMatricula.ObterConcluintesEmpregados(inicio,fim,idUf));
        }

        public IList<DTOConcluintesEspacoOcupacional> ObterConcluintesExternos(DateTime inicio, DateTime fim, int idUf)
        {
            RegistrarLogExecucao();
            
            var manterMatricula = new ManterMatricula();
            return AgruparDadosExternos(manterMatricula.ObterConcluintesExternos(inicio, fim, idUf));
        }

        private static IList<DTOConcluintesEspacoOcupacional> AgruparDadosInternos(IList<DTOConcluintesEspacoOcupacional> lista)
        {
            var retorno = new List<DTOConcluintesEspacoOcupacional>();

            // Agrupar Liderança.
            retorno.Add(new DTOConcluintesEspacoOcupacional
            {
                NomeNivelOcupacional = "LIDERANÇA",
                QTDConcluintes = lista.Where(x =>
                x.NomeNivelOcupacional == "CONSELHEIRO" ||
                x.NomeNivelOcupacional == "DIRIGENTE" ||
                x.NomeNivelOcupacional == "ASSESSOR" ||
                x.NomeNivelOcupacional == "GERENTE"
                ).Sum(x => x.QTDConcluintes)
            });

            // Agrupar analistas.
            retorno.Add(new DTOConcluintesEspacoOcupacional
            {
                NomeNivelOcupacional = "ANALISTA",
                QTDConcluintes = lista.Where(x =>
                x.NomeNivelOcupacional.StartsWith("ANALISTA")
                ).Sum(x => x.QTDConcluintes)
            });

            // Agrupar assistentes.
            retorno.Add(new DTOConcluintesEspacoOcupacional
            {
                NomeNivelOcupacional = "ASSISTENTE",
                QTDConcluintes = lista.Where(x =>
                x.NomeNivelOcupacional.StartsWith("ASSISTENTE")
                ).Sum(x => x.QTDConcluintes)
            });

            // Adicionar Trainees
            retorno.Add(new DTOConcluintesEspacoOcupacional
            {
                NomeNivelOcupacional = "TRAINEE",
                QTDConcluintes = lista.Where(x => x.NomeNivelOcupacional == "TRAINEE").Sum(x => x.QTDConcluintes)
            });

            // Adicionar Estagiários
            retorno.Add(new DTOConcluintesEspacoOcupacional
            {
                NomeNivelOcupacional = "ESTAGIÁRIO",
                QTDConcluintes = lista.Where(x => x.NomeNivelOcupacional == "ESTAGIÁRIO").Sum(x => x.QTDConcluintes)
            });

            // Adicionar Menor Aprendiz
            retorno.Add(new DTOConcluintesEspacoOcupacional
            {
                NomeNivelOcupacional = "MENOR APRENDIZ",
                QTDConcluintes = lista.Where(x => x.NomeNivelOcupacional == "MENOR APRENDIZ").Sum(x => x.QTDConcluintes)
            });

            return retorno.OrderByDescending(x => x.QTDConcluintes).ToList();
        }

        private static IList<DTOConcluintesEspacoOcupacional> AgruparDadosExternos(IList<DTOConcluintesEspacoOcupacional> lista)
        {
            var listaFiltrada = new List<DTOConcluintesEspacoOcupacional>();

            #region Removendo níveis da lista principal

            listaFiltrada.AddRange(lista.Where(x => !x.NomeNivelOcupacional.Equals("AD")
             && !x.NomeNivelOcupacional.Equals("APA")
             && !x.NomeNivelOcupacional.Equals("ALI")
             && !x.NomeNivelOcupacional.Equals("AOE")
             && !x.NomeNivelOcupacional.Equals("PTEC")
             && !x.NomeNivelOcupacional.Equals("PAGRO")));

            var rejeitados = lista.Where(x => x.NomeNivelOcupacional.Equals("AD")
           || x.NomeNivelOcupacional.Equals("APA")
           || x.NomeNivelOcupacional.Equals("ALI")
           || x.NomeNivelOcupacional.Equals("AOE")
           || x.NomeNivelOcupacional.Equals("PTEC")
           || x.NomeNivelOcupacional.Equals("PAGRO"));

            #endregion

            #region Agrupando níveis ocupacionais

            listaFiltrada.Add(new DTOConcluintesEspacoOcupacional()
            {
                NomeNivelOcupacional = "AGENTE",
                QTDConcluintes = rejeitados.Where(x => x.NomeNivelOcupacional.Contains("AD")
                || x.NomeNivelOcupacional.Contains("ALI")
                || x.NomeNivelOcupacional.Contains("AOE")
                || x.NomeNivelOcupacional.Contains("APA")
                || x.NomeNivelOcupacional.Contains("PTEC")
                || x.NomeNivelOcupacional.Contains("PAGRO")).Sum(x => x.QTDConcluintes)
            });

            #endregion

            return listaFiltrada.OrderByDescending(x => x.QTDConcluintes).ToList();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
