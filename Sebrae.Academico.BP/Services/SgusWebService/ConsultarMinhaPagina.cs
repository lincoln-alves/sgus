using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.HistoricoAcademico;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Extensions;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarMinhaPagina : BusinessProcessServicesBase
    {
        public DTOAgenda ObterAgenda(int idUsuario, int mes, int ano)
        {
            var listaErros = new List<string>();

            var dataInicioMes = (new DateTime(ano, mes, 1));
            var dataFimMes = dataInicioMes.AddMonths(1).AddDays(-1);
            var diaSemana = ((int) DateTime.Now.DayOfWeek);
            if (diaSemana == 0) diaSemana = 6;
            else diaSemana = diaSemana - 1;

            var diaSemanaInicioMes = (int) dataInicioMes.DayOfWeek;
            if (diaSemanaInicioMes == 0) diaSemanaInicioMes = 6;
            else diaSemanaInicioMes = diaSemanaInicioMes - 1;
            var listaEventos = new List<DTOEventoAgenda>();
            var manterMatriculaTurma = new ManterMatriculaTurma();
            DateTime? dataInicio = DateTime.Now.Date;
            DateTime? dataFim = DateTime.Now.Date;
            IList<MatriculaTurma> listaCursoInscritos;
            var anoInicial = DateTime.Now.Year;
            try
            {
                listaCursoInscritos = manterMatriculaTurma.ObterMatriculasInscritas(idUsuario);
            }
            catch (Exception ex)
            {
                listaErros.Add(ex.ToString());
                listaCursoInscritos = new List<MatriculaTurma>();
                //TODO: LOGAR ERROR
                //TODO: MELHORAR TRATAMENTO DE COMO RECUPERAR OS DADOS
            }
            //TODO: MELHORAR pois não achei melhor solução

            try
            {
                foreach (var matriculaTurma in listaCursoInscritos)
                {
                    var data = matriculaTurma.Turma.DataInicio;
                    
                    if (data.Year < anoInicial) anoInicial = data.Year;
                    try
                    {
                        data = matriculaTurma.MatriculaOferta.DataSolicitacao.Date > data.Date
                            ? matriculaTurma.MatriculaOferta.DataSolicitacao.Date
                            : data;

                        if (data.Date.Between(dataInicioMes.Date, dataFimMes.Date))
                        {
                            listaEventos.Add(new DTOEventoAgenda
                            {
                                TipoEvento = (int) enumTipoEventoAgenda.Inicio,
                                Data = data.ToString("dd/MM/yyyy"),
                                Nome = matriculaTurma.Turma.Oferta.SolucaoEducacional.Nome,
                                IdSolucaoEducacional = matriculaTurma.Turma.Oferta.SolucaoEducacional.ID,
                                IdTurma = matriculaTurma.Turma.ID,
                                IdOferta = matriculaTurma.Turma.Oferta.ID,
                                IdMatriculaOferta = matriculaTurma.MatriculaOferta.ID,
                                IdMatriculaTurma = matriculaTurma.ID.ToString()
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        listaErros.Add(ex.ToString());
                    }

                    dataInicio = dataInicio.Value.Date > data.Date ? data.Date : dataInicio;

                    try
                    {
                        data = matriculaTurma.CalcularDataLimite();

                        if (data.Date.Between(dataInicioMes.Date, dataFimMes.Date))
                        {
                            listaEventos.Add(new DTOEventoAgenda
                            {
                                TipoEvento = (int) enumTipoEventoAgenda.Fim,
                                Data = data.ToString("dd/MM/yyyy"),
                                Nome = matriculaTurma.Turma.Oferta.SolucaoEducacional.Nome,
                                IdSolucaoEducacional = matriculaTurma.Turma.Oferta.SolucaoEducacional.ID,
                                IdTurma = matriculaTurma.Turma.ID,
                                IdOferta = matriculaTurma.Turma.Oferta.ID,
                                IdMatriculaOferta = matriculaTurma.MatriculaOferta.ID,
                                IdMatriculaTurma = matriculaTurma.ID.ToString()
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        listaErros.Add(ex.ToString());
                    }
                    dataFim = dataFim.Value.Date < data.Date ? data.Date : dataFim;
                }
            }
            catch (Exception ex)
            {
                listaErros.Add(ex.ToString());
            }

            try
            {
                var listaUsuarioTrilha =
                    (new BMUsuarioTrilha()).ObterPorUsuario(idUsuario)
                        .Where(p => p.StatusMatricula == enumStatusMatricula.Inscrito)
                        .ToList();
                foreach (var item in listaUsuarioTrilha)
                {
                    var data = item.DataInicio;
                    if (data.Year < anoInicial) anoInicial = data.Year;
                    if (data.Date.Between(dataInicioMes.Date, dataFimMes.Date))
                    {
                        listaEventos.Add(new DTOEventoAgenda
                        {
                            TipoEvento = (int) enumTipoEventoAgenda.Inicio,
                            Data = data.ToString("dd/MM/yyyy"),
                            Nome = string.Concat(item.TrilhaNivel.Trilha.Nome, " - ", item.TrilhaNivel.Nome),
                            IdSolucaoEducacional = 0,
                            IdTurma = 0,
                            IdOferta = 0,
                            IdMatriculaOferta = 0,
                            IdMatriculaTurma = item.ID + "000" + item.TrilhaNivel.ID
                        });
                    }
                    data = item.DataFim.HasValue
                        ? item.DataFim.Value
                        : (item.TrilhaNivel.LimiteCancelamento > 0
                            ? item.DataInicio.AddDays(item.TrilhaNivel.LimiteCancelamento)
                            : item.DataInicio.AddDays(item.TrilhaNivel.QuantidadeDiasPrazo));
                    if (data.Date.Between(dataInicioMes.Date, dataFimMes.Date))
                    {
                        listaEventos.Add(new DTOEventoAgenda
                        {
                            TipoEvento = (int) enumTipoEventoAgenda.Fim,
                            Data = data.ToString("dd/MM/yyyy"),
                            Nome = string.Concat(item.TrilhaNivel.Trilha.Nome, " - ", item.TrilhaNivel.Nome),
                            IdSolucaoEducacional = 0,
                            IdTurma = 0,
                            IdOferta = 0,
                            IdMatriculaOferta = 0,
                            IdMatriculaTurma = item.ID + "000" + item.TrilhaNivel.ID
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                listaErros.Add(ex.ToString());
            }

            try
            {
                var listaMatriculaCapacitacao =
                    new BMMatriculaCapacitacao().ObterPorUsuario(idUsuario)
                        .Where(x => x.StatusMatricula.Equals(enumStatusMatricula.Inscrito))
                        .ToList();
                var manterMatCapacitacao = new ManterMatriculaCapacitacaoService();
                var culture = new CultureInfo("pt-BR");
                foreach (var itemHistorico in listaMatriculaCapacitacao)
                {
                    var dtoCapacitacao = manterMatCapacitacao.AprovacoesSolucoesEducacionais(itemHistorico, idUsuario);
                    foreach (var item in dtoCapacitacao.ModulosCapacitacao)
                    {
                        DateTime data;
                        if (!string.IsNullOrEmpty(item.DataInicio))
                        {
                            data = Convert.ToDateTime(item.DataInicio, culture);
                            if (data.Year < anoInicial) anoInicial = data.Year;
                            if (data.Date.Between(dataInicioMes.Date, dataFimMes.Date))
                            {
                                listaEventos.Add(new DTOEventoAgenda
                                {
                                    TipoEvento = (int) enumTipoEventoAgenda.Inicio,
                                    Data = data.ToString("dd/MM/yyyy"),
                                    Nome = string.Concat(itemHistorico.Capacitacao.Programa.Nome, " - ", item.Nome),
                                    IdSolucaoEducacional = 0,
                                    IdTurma = 0,
                                    IdOferta = 0,
                                    IdMatriculaOferta = 0,
                                    IdMatriculaTurma =
                                        itemHistorico.ID + itemHistorico.Capacitacao.Programa.ID.ToString() +
                                        itemHistorico.Capacitacao.ID.ToString() + "000" + item.ID
                                });
                            }
                        }
                        if (string.IsNullOrEmpty(item.DataFim)) continue;
                        data = Convert.ToDateTime(item.DataFim, culture);
                        if (data.Date.Between(dataInicioMes.Date, dataFimMes.Date))
                        {
                            listaEventos.Add(new DTOEventoAgenda
                            {
                                TipoEvento = (int) enumTipoEventoAgenda.Fim,
                                Data = data.ToString("dd/MM/yyyy"),
                                Nome = string.Concat(itemHistorico.Capacitacao.Programa.Nome, " - ", item.Nome),
                                IdSolucaoEducacional = 0,
                                IdTurma = 0,
                                IdOferta = 0,
                                IdMatriculaOferta = 0,
                                IdMatriculaTurma =
                                    itemHistorico.ID + itemHistorico.Capacitacao.Programa.ID.ToString() +
                                    itemHistorico.Capacitacao.ID.ToString() + "000" + item.ID
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listaErros.Add(ex.ToString());
            }

            return new DTOAgenda
            {
                DataAtual = DateTime.Now.ToString("dd/MM/yyyy"),
                DataInicioMes = dataInicioMes.ToString("dd/MM/yyyy"),
                DataFimMes = dataFimMes.ToString("dd/MM/yyyy"),
                DiaSemana = diaSemana.ToString(),
                DiaSemanaInicioMes = diaSemanaInicioMes.ToString(),
                Eventos = listaEventos.OrderByDescending(x => x.TipoEvento).ToList(),
                AnoInicial = anoInicial,
                AnoFinal = dataFim.Value.Year,
                Erros = listaErros
            };
        }

        public DTOMinhaPagina ObterMinhaPagina(int idUsuario)
        {
            var resultado = new DTOMinhaPagina();
            if (idUsuario <= 0) return resultado;
            var historicoAcademicoServices = new HistoricoAcademicoServices();
            try
            {
                resultado.ListaHistoricoAcademico = historicoAcademicoServices.ConsultarHistorico(idUsuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                resultado.ListaHistoricoAcademico = new List<DTOItemHistoricoAcademico>();
            }
            try
            {
                resultado.ListaMinhasInscricoes = (new ConsultarMeusCursos()).ObterMeusCursos(idUsuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                resultado.ListaMinhasInscricoes = new List<DTOItemMeusCursos>();
            }
            try
            {
                resultado.ListaSistemasExternos =
                    (List<DTOSistemaExterno>) new SistemaExternoServices().ObterTodosPorUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                resultado.ListaSistemasExternos = new List<DTOSistemaExterno>();
            }
            resultado.DadosAgenda = ObterAgenda(idUsuario, DateTime.Now.Month, DateTime.Now.Year);

            return resultado;
        }
    }
}