using System.Collections.Generic;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using System.Linq;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterDashBoard : BusinessProcessBase
    {

        #region "Atributos Privados"

        //private BMConfiguracaoSistema bmConfiguracaoSistema = null;

        #endregion

        #region "Construtor"

        public ManterDashBoard()
            : base()
        {
            this.usuUsuario = new ManterUsuario().ObterUsuarioLogado();
        }

        #endregion

        private Usuario usuUsuario = null;
        public Usuario Usuario
        {
            get
            {
                return this.usuUsuario;
            }
        }

        #region "Métodos Públicos"

        #region "Métodos utilizados para exibição de dados nos gráficos da tela de DashBoard"

        public IList<DTOMatriculaOferta> ObterTaxaDeAprovacaoNoAno(int? ano, int? idUf)
        {
            return new ProcTaxaAprovacaoNoAno().ObterTaxaDeAprovacaoNoAno(ano, idUf);
        }

        public IList<DTOMatriculaOfertaNoAno> ObterTotalMatriculasNoAnoPorFormaAquisicao(int? ano, int? idUf)
        {
            if (idUf >= 0)
            {
                return new ProcTotalMatriculasNoAnoPorFormaAquisicao().ObterTotalMatriculasNoAnoPorFormaAquisicao(ano, idUf);
            }
            else
            {
                return new ProcTotalMatriculasNoAnoPorFormaAquisicao().ObterTotalMatriculasNoAnoPorFormaAquisicao(ano, null);
            }

        }


        public IList<DTOMatriculasPorMes> ObterMatriculasPorMes(int? ano, int? idUf)
        {
            if (idUf >= 0)
            {
                return new ProcMatriculasPorMes().ObterMatriculasPorMes(ano, idUf);
            }
            else
            {
                return new ProcMatriculasPorMes().ObterMatriculasPorMes(ano, null);
            }

        }


        public IList<DTOConcluintesPorFormaAquisicao> ObterConcluintesPorFormaAquisicao(int? ano, int? idUf)
        {
            if (idUf >= 0)
            {
                return new ProcConcluintesPorFormaAquisicao().ObterConcluintesPorFormaAquisicao(ano, idUf);
            }
            else
            {
                return new ProcConcluintesPorFormaAquisicao().ObterConcluintesPorFormaAquisicao(ano, null);
            }

        }

        public IList<DTOConcluintesPorSolucaoEducacional> ObterConcluintesPorSolucaoEducacional(int ano = 0)
        {
            return new ProcConcluintesPorSolucaoEducacional().ObterConcluintesPorSolucaoEducacional(ano);
        }

        public IList<DTOPerfilDoPublicoAtendido> ObterPerfilDoPublicoAtendido(int ano = 0)
        {
            var lista = new ProcPerfilDoPublicAtendido().ObterPerfilDoPublicoAtendido(ano);
             
            List<DTOPerfilDoPublicoAtendido> final = new List<DTOPerfilDoPublicoAtendido>();
            final.Add(new DTOPerfilDoPublicoAtendido() { Perfil = "DIRIGENTE, GERENTE, ASSESSOR E CHEFE DE GABINETE" });
            final.Add(new DTOPerfilDoPublicoAtendido() { Perfil = "ANALISTA" });
            final.Add(new DTOPerfilDoPublicoAtendido() { Perfil = "ASSISTENTE" });
            final.Add(new DTOPerfilDoPublicoAtendido() { Perfil = "ESTAGIARIO E MENOR APRENDIZ" });
            final.Add(new DTOPerfilDoPublicoAtendido() { Perfil = "ADL E ALI" });
            final.Add(new DTOPerfilDoPublicoAtendido() { Perfil = "CREDENCIADO" });

            foreach (var item in lista)
            {
                if (item.Perfil.ToUpper().Contains("DIRIGENTE") || item.Perfil.ToUpper().Contains("GERENTE") ||
                    item.Perfil.ToUpper().Contains("ASSESSOR") || item.Perfil.ToUpper().Contains("DIRIGENTE") ||
                    item.Perfil.ToUpper().Contains("CHEFE DE GABINETE"))
                    final[0].Quantidade += item.Quantidade;

                if (item.Perfil.ToUpper().Contains("ANALISTA"))
                    final[1].Quantidade += item.Quantidade;

                if (item.Perfil.ToUpper().Contains("ASSISTENTE"))
                    final[2].Quantidade += item.Quantidade;

                if (item.Perfil.ToUpper().Contains("ESTAGIARIO") || item.Perfil.ToUpper().Contains("MENOR APRENDIZ"))
                    final[3].Quantidade += item.Quantidade;

                if (item.Perfil.ToUpper().Equals("AD") || item.Perfil.ToUpper().Equals("ALI"))
                    final[4].Quantidade += item.Quantidade;

                if (item.Perfil.ToUpper().Contains("CREDENCIADO"))
                    final[5].Quantidade += item.Quantidade;
            }

            return final;
        }

        public IList<DTOFaixaEtaria> ObterFaixaEtaria(int ano = 0)
        {
            var lista = new ProcFaixaEtaria().ObterFaixaEtaria(ano);
            List<DTOFaixaEtaria> final = new List<DTOFaixaEtaria>();

            final.Add(new DTOFaixaEtaria() { Nome = "Menos de 18 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "18 a 24 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "25 a 29 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "30 a 34 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "35 a 39 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "40 a 44 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "45 a 49 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "50 a 54 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "55 a 59 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "60 a 64 anos" });
            final.Add(new DTOFaixaEtaria() { Nome = "65 ou mais" });

            int total = 0;

            foreach (var item in lista)
            {
                total += (int)item.Quantidade;
                if (item.Idade < 18)
                    final[0].Quantidade += item.Quantidade;

                if (item.Idade >= 18 && item.Idade <= 24)
                    final[1].Quantidade += item.Quantidade;

                if (item.Idade >= 25 && item.Idade <= 29)
                    final[2].Quantidade += item.Quantidade;

                if (item.Idade >= 30 && item.Idade <= 34)
                    final[3].Quantidade += item.Quantidade;

                if (item.Idade >= 35 && item.Idade <= 39)
                    final[4].Quantidade += item.Quantidade;

                if (item.Idade >= 40 && item.Idade <= 44)
                    final[5].Quantidade += item.Quantidade;

                if (item.Idade >= 45 && item.Idade <= 49)
                    final[6].Quantidade += item.Quantidade;

                if (item.Idade >= 50 && item.Idade <= 54)
                    final[7].Quantidade += item.Quantidade;

                if (item.Idade >= 55 && item.Idade <= 59)
                    final[8].Quantidade += item.Quantidade;

                if (item.Idade >= 60 && item.Idade <= 64)
                    final[9].Quantidade += item.Quantidade;

                if (item.Idade >= 65)
                    final[10].Quantidade += item.Quantidade;
            }

            foreach (var item in final)
            {
                item.Quantidade = ((item.Quantidade * 100) / total);
            }

            return final;
        }

        public IList<DTOConcluintesPorRegiao> ObterConcluintesPorRegiao(int ano = 0)
        {
            return new ProcConcluintesPorRegiao().ObterConcluintesPorRegiao(ano);
        }

        public IList<DTOUsuariosPorUF> ObterUsuariosPorUF(int ano = 0)
        {
            var lista = new ProcUsuariosPorUF().ObterUsuariosPorUF(ano);

            float total = 0;
            lista.ToList().ForEach(d => total += d.Quantidade);
            lista.ToList().ForEach(d => d.Quantidade = (d.Quantidade * 100) / total);

            return lista;
        }

        public IList<DTOTempoDeSebrae> ObterTempoDeSebrae()
        {
            return new ProcTempoDeSebrae().ObterTempoDeSebrae();
        }

        public IList<DTONumeroDeCertificacoesPorColaborador> ObterNumeroDeCertificacoesPorColaborador()
        {
            return new ProcNumeroDeCertificacoesPorColaborador().ObterNumeroDeCertificacoesPorColaborador();
        }

        public IList<DTOInscritosPorQuadroTotal> ObterInscritosPorQuadroTotal()
        {
            return new ProcInscritosPorQuadroTotal().ObterInscritosPorQuadroTotal();
        }

        public IList<DTOCertificadosPorNaoCertificados> ObterCertificadosPorNaoCertificados()
        {
            return new ProcCertificadosPorNaoCertificados().ObterCertificadosPorNaoCertificados();
        }

        public IList<DTOCertificadosPorNivelOperacional> ObterCertificadosPorNivelOperacional()
        {
            return new ProcCertificadosPorNivelOperacional().ObterCertificadosPorNivelOperacional();
        }

        public IList<DTOTotalMatriculaOfertaPorAno> ObterTotalMatriculasNoAnoPorUf(int? ano, int? idUf)
        {
            if (idUf >= 0)
            {
                return new ProcTotalMatriculasPorAno().ObterTotalMatriculasNoAnoPorUf(ano, idUf);
            }
            else
            {
                return new ProcTotalMatriculasPorAno().ObterTotalMatriculasNoAnoPorUf(ano, null);
            }

        }


        public IList<DTOInscritosPorCategoria> ObterInscritosPorCategoria(int? ano, int? idUf)
        {
            if (idUf >= 0)
            {
                return new ProcInscritosPorCategoria().ObterInscritosPorCategoria(ano, idUf);
            }
            else
            {
                return new ProcInscritosPorCategoria().ObterInscritosPorCategoria(ano, null);
            }

        }

        public IList<DTOInscritosTotalUF> ObterInscritosTotalUF(int? ano)
        {
            return new ProcInscritosTotalUF().ObterTotalMatriculasNoAnoPorUf(ano);

        }

        public IList<DTOPublicoAlvo> ObterTotalPublicoAlvo()
        {
            List<DTOPublicoAlvo> retorno = new List<DTOPublicoAlvo>();

            DTOPublicoAlvo registro = new DTOPublicoAlvo();
            registro.Publico = "Conselheiros";
            registro.Quantidade = new BMUsuario().ContarAtivosPorNivelOcupacional(15);
            retorno.Add(registro);

            registro = new DTOPublicoAlvo();
            registro.Publico = "Dirigentes e gerentes";
            registro.Quantidade = new BMUsuario().ContarAtivosPorNivelOcupacional(6) + new BMUsuario().ContarAtivosPorNivelOcupacional(8);
            retorno.Add(registro);

            registro = new DTOPublicoAlvo();
            registro.Publico = "Coloboradores";
            registro.Quantidade = new BMUsuario().ContarAtivosPorPerfil(1);
            retorno.Add(registro);

            registro = new DTOPublicoAlvo();
            registro.Publico = "Credenciados";
            registro.Quantidade = new BMUsuario().ContarAtivosPorNivelOcupacional(21);
            retorno.Add(registro);

            registro = new DTOPublicoAlvo();
            registro.Publico = "Parceiros";
            registro.Quantidade = new BMUsuario().ContarAtivosPorNivelOcupacional(4);
            retorno.Add(registro);

            return retorno;

        }

        public IList<DTOSituacaoCursos> ObterSituacoesAlunosCursos(int? ano)
        {
            return new RelatorioSituacaoCursos().ConsultarSituacoes(null, ano);

        }

        public DTOTop5Cursos ObterTop5Cursos(DateTime dataInicio, DateTime dataFim,int idUf)
        {
            DTOTop5Cursos dto = new DTOTop5Cursos();
            dto.CursosOnline = new RelatorioTop5CursoOnline().ObterCursosOnline(dataInicio, dataFim, idUf).ToList();
            dto.CursosPresenciais = new RelatorioTop5CursoPresencial().ObterCursoPresencial(dataInicio, dataFim, idUf).ToList();
            return dto;
        }

        #endregion


        #region "Métodos utilizados para exibição de dados nas tabelas da tela de DashBoard"

        public IList<DTOGeracaoRelatorio> ListarRelatoriosMaisAcessadosPorUsuario()
        {
            return new ProcRelatoriosMaisAcessados().ListarRelatoriosMaisAcessadosPorUsuario(this.Usuario.ID);
        }

        public IList<DTOFuncionalidade> ListarFuncionalidadesMaisAcessadasPorUsuario()
        {
            return new ProcLogAcessoFuncionalidade().ListarFuncionalidadesMaisAcessadasPorUsuario(this.Usuario.ID);
        }

        public IList<DTONotificacao> ListaNotificacoesMaisAcessadasPorUsuario()
        {
            return new ProcNotificacoes().ListaNotificacoesMaisAcessadasPorUsuario(this.Usuario.ID);
        }

        public IList<DTOEstatisticaNivelOcupacional> ObterParticipacaoProporcionalAoNumeroDeFuncionarios(int? ano, int? idUf)
        {
            return new ProcEstatisticasDeNivelOcupacionalPorAno().ObterParticipacaoProporcionalAoNumeroDeFuncionarios(ano, idUf);
        }

        #endregion

        #endregion
    }
}
