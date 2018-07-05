using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioCapacitados : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.Capacitados; }
        }

        public void Dispose()
        {
            GC.Collect();
        }


        public IList<Perfil> ObterPerfilTodos()
        {
            using (var perfilBm = new BMPerfil())
            {
                return perfilBm.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBm = new BMUf())
            {
                return ufBm.ObterTodos().ToList();
            }
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (var formaAquisicaoBm = new BMFormaAquisicao())
            {
                return formaAquisicaoBm.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodos()
        {
            using (var nivelBm = new BMNivelOcupacional())
            {
                return nivelBm.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<StatusMatricula> ObterStatusMatriculaTodos()
        {
            return Enum.GetValues(typeof(enumStatusMatricula))
                .Cast<enumStatusMatricula>()
                .Select(enumStatus =>
                    new StatusMatricula()
                    {
                        ID = (int)enumStatus,
                        Nome = enumStatus.ToString()
                    }).ToList();
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int idFormaAquisicao = 0,
            Uf uf = null)
        {
            return new ManterSolucaoEducacional().ObterSolucaoEducacionalPorFormaAquisicaoUf(idFormaAquisicao, uf);
        }

        public IList<DTORelatorioCapacitados> ConsultarRelatorio(int? idPerfil, int? idUf, int? idNivelOcupacional,
            int? idFormaAquisicao, int? idSolucaoEducacional, int? idStatusMatricula, DateTime? dataInicial,
            DateTime? dataFinal, string situacaoUsuario, IEnumerable<int> pUfResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarRelatorioCapacitados(idPerfil, idUf, idNivelOcupacional,
                idFormaAquisicao, idSolucaoEducacional, idStatusMatricula, dataInicial, dataFinal, situacaoUsuario, pUfResponsavel);
        }

        public IList<DTORelatorioCapacitados> AgruparRegistros(IEnumerable<DTORelatorioCapacitados> registros,
            IEnumerable<string> camposAEsconder)
        {
            var resultado = registros.ToList();

            resultado.ForEach(r => r.TotalCapacitados = 1);
            //Inicialmente contando como 1 para ser somando nos agrupamentos

            if (camposAEsconder.Contains("SolucaoEducacional"))
            {
                //Removendo usuários duplicados que possuem registro em mais de uma Solução Educacional, pois não será exibido
                //o agrupamento por Solução Educacional
                resultado = resultado.GroupBy(g => new { g.IdUsuario, g.UF, g.NivelOcupacional, g.UFResponsavel }).Select(agrupado =>
                     new DTORelatorioCapacitados
                     {
                         UF = agrupado.Key.UF,
                         NivelOcupacional = agrupado.Key.NivelOcupacional,
                         TotalCapacitados = 1,
                         UFResponsavel = agrupado.Key.UFResponsavel
                     }).ToList();

                //Duplicados removidos, agrupando para pegar os totais
                resultado = resultado.GroupBy(g => new { g.UF, g.NivelOcupacional, g.UFResponsavel }).Select(agrupado =>
                     new DTORelatorioCapacitados
                     {
                         UF = agrupado.Key.UF,
                         NivelOcupacional = agrupado.Key.NivelOcupacional,
                         TotalCapacitados = agrupado.Sum(a => a.TotalCapacitados),
                         UFResponsavel = agrupado.Key.UFResponsavel
                     }).ToList();
            }

            if (camposAEsconder.Contains("NivelOcupacional"))
            {
                resultado = resultado.GroupBy(g => new { g.UF, g.SolucaoEducacional, g.UFResponsavel }).Select(agrupado =>
                     new DTORelatorioCapacitados
                     {
                         UF = agrupado.Key.UF,
                         SolucaoEducacional = agrupado.Key.SolucaoEducacional,
                         TotalCapacitados = agrupado.Sum(a => a.TotalCapacitados),
                         UFResponsavel = agrupado.Key.UFResponsavel
                     }).ToList();
            }

            if (camposAEsconder.Any(x => x == "UF"))
            {
                resultado = resultado.GroupBy(u => new { u.NivelOcupacional, u.SolucaoEducacional, u.UFResponsavel }).Select(agrupado =>
                     new DTORelatorioCapacitados
                     {
                         NivelOcupacional = agrupado.Key.NivelOcupacional,
                         SolucaoEducacional = agrupado.Key.SolucaoEducacional,
                         TotalCapacitados = agrupado.Sum(a => a.TotalCapacitados),
                         UFResponsavel = agrupado.Key.UFResponsavel
                     }).ToList();
            }


            //Nenhuma regra específica de agrupamento. Apenas agrupar os usuários pelos campos exibidos
            if (!camposAEsconder.Any())
            {
                resultado =
                    resultado.GroupBy(g => new { g.UF, g.NivelOcupacional, g.SolucaoEducacional, g.UFResponsavel }).Select(agrupado =>
                          new DTORelatorioCapacitados
                          {
                              UF = agrupado.Key.UF,
                              NivelOcupacional = agrupado.Key.NivelOcupacional,
                              SolucaoEducacional = agrupado.Key.SolucaoEducacional,
                              TotalCapacitados = agrupado.Sum(a => a.TotalCapacitados),
                              UFResponsavel = agrupado.Key.UFResponsavel
                          }).ToList();
            }

            return resultado;
        }
    }
}
