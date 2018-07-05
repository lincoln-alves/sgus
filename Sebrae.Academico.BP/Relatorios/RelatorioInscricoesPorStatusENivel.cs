using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioInscricoesPorStatusENivel : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.InscricoesPorStatusENivel; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<Perfil> ObterPerfilTodos()
        {
            using (BMPerfil perfilBM = new BMPerfil())
            {
                return perfilBM.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<Uf> ObterUFs()
        {
            using (BMUf ufBM = new BMUf())
            {
                var lista = ufBM.ObterTodos().ToList();

                return lista;
            }
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (BMFormaAquisicao formaAquisicaoBM = new BMFormaAquisicao())
            {
                return formaAquisicaoBM.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalTodos(Uf uf = null)
        {
            using (var solEducBm = new BMSolucaoEducacional())
            {
                var retorno = solEducBm.ObterTodos();

                if(uf != null)
                    solEducBm.FiltrarPermissaoVisualizacao(ref retorno, uf.ID);

                return retorno;
            }
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodos()
        {
            using (BMNivelOcupacional nivelBM = new BMNivelOcupacional())
            {
                return nivelBM.ObterTodos().OrderBy(n => n.Nome).ToList();
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
                        Nome = enumStatus.ToString().QuebrarPalavrasEmUpperCases()
                    }).OrderBy(e => e.Nome).ToList();
        }

        public IList<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int idFormaAquisicao)
        {
            using (var solEduBM = new BMSolucaoEducacional())
            {
                return solEduBM.ObterPorFiltro(new SolucaoEducacional() { FormaAquisicao = new BMFormaAquisicao().ObterPorID(idFormaAquisicao) })
                    .OrderBy(n => n.Nome)
                    .ToList();
            }
        }

        public int ObterUFLogadoSeGestor()
        {
            using (var usuarioBM = new BMUsuario())
            {
                return usuarioBM.ObterUfLogadoSeGestor();
            }
        }

        public IQueryable<CategoriaConteudo> ObterCategorias()
        {
            using (var categoriaBm = new BMCategoriaConteudo())
            {
                return categoriaBm.ObterTodos();
            }
        }

        public IList<DTORelatorioInscricoesPorStatusENivel> ConsultarRelatorio(string statuses, string niveisOcupacionais, int? idUf, int? idSolucaoEducacional, DateTime? dataInicial, DateTime? dataFinal, IEnumerable<int> pUfResponsavel)
        {
            RegistrarLogExecucao();

            var resultado = (new ManterMatricula()).ConsultarRelatorioInscricoesPorStatusENivel(statuses, niveisOcupacionais, idUf,
                idSolucaoEducacional, dataInicial, dataFinal, pUfResponsavel);

            return GerarListaDeSaida(resultado);
        }

        public IList<DTORelatorioInscricoesPorStatusENivel> GerarListaDeSaida(IEnumerable<DTOProcInscricoesPorStatusENivel> registros)
        {
            var statuses = registros.Select(r => r.Status).Distinct().ToList();


            return statuses.Select(s => new DTORelatorioInscricoesPorStatusENivel()
            {
                Status = s,
                AD = registros.Where(r => r.NivelOcupacional == "AD" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                ALI = registros.Where(r => r.NivelOcupacional == "ALI" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                APA = registros.Where(r => r.NivelOcupacional == "APA" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),

                AnalistaTecnicoI = registros.Where(r => r.NivelOcupacional == "ANALISTA TÉCNICO I" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                AnalistaTecnicoII = registros.Where(r => r.NivelOcupacional == "ANALISTA TÉCNICO II" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                AnalistaTecnicoIII = registros.Where(r => r.NivelOcupacional == "ANALISTA TÉCNICO III" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                
                AOE = registros.Where(r => r.NivelOcupacional == "AOE" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                Assessor = registros.Where(r => r.NivelOcupacional == "ASSESSOR" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                
                AssistenteI = registros.Where(r => r.NivelOcupacional == "ASSISTENTE I" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                AssistenteII = registros.Where(r => r.NivelOcupacional == "ASSISTENTE II" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                AssistenteIII = registros.Where(r => r.NivelOcupacional == "ASSISTENTE III" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                
                Conselheiro = registros.Where(r => r.NivelOcupacional == "CONSELHEIRO" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                Credenciado = registros.Where(r => r.NivelOcupacional == "CREDENCIADO" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                Dirigente = registros.Where(r => r.NivelOcupacional == "DIRIGENTE" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                Estagiario = registros.Where(r => r.NivelOcupacional == "ESTAGIÁRIO" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                Gerente = registros.Where(r => r.NivelOcupacional == "GERENTE" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                
                MenorAprendiz = registros.Where(r => r.NivelOcupacional == "MENOR APRENDIZ" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                OrientadorALI = registros.Where(r => r.NivelOcupacional == "ORIENTADOR ALI" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                
                Parceiro = registros.Where(r => r.NivelOcupacional == "PARCEIRO" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                PreALI = registros.Where(r => r.NivelOcupacional == "PRÉ ALI" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                FuncionarioTemporario = registros.Where(r => r.NivelOcupacional == "Funcionário Temporário" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),
                Trainee = registros.Where(r => r.NivelOcupacional == "TRAINEE" && r.Status == s).Select(r => r.Quantidade).FirstOrDefault(),

                Total = registros.Where(r => r.Status == s).Sum(r => r.Quantidade)
            }).ToList();

        }

    }
}
