using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterMatriculaCapacitacaoService : BusinessProcessServicesBase
    {

        public void CancelarMatriculaCapacitacao(int idMatriculaCapacitacao, AuthenticationRequest autenticacao)
        {

            MatriculaCapacitacao matriculaCapacitacao = new BMMatriculaCapacitacao().ObterPorId(idMatriculaCapacitacao);

            if (matriculaCapacitacao != null
                    && matriculaCapacitacao.StatusMatricula == enumStatusMatricula.Inscrito)
            {
                TimeSpan diasMatriculados = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(matriculaCapacitacao.DataInicio);
                if (diasMatriculados.Days > int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro))
                {
                    //Passou do Limite para cancelamento
                    throw new AcademicoException("O prazo de cancelamento expirou");
                }

                if (matriculaCapacitacao.ListaMatriculaTurmaCapacitacao != null && matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.Count > 0)
                {
                    // Embora esteja em uma lista ele sempre terá somente uma matrícula turma para cada matrícula capacitação por regra de negócio
                    new BMMatriculaTurmaCapacitacao().ExcluirLista(matriculaCapacitacao.ListaMatriculaTurmaCapacitacao);
                    matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.Clear();
                }

                matriculaCapacitacao.DataFim = DateTime.Now;
                matriculaCapacitacao.StatusMatricula = enumStatusMatricula.CanceladoAluno;
                matriculaCapacitacao.Auditoria = new Auditoria(autenticacao.Login);
            }
            else
            {
                throw new AcademicoException("Não foi encontrada nenhuma matrícula com cancelamento permitido");
            }

            new BMMatriculaCapacitacao().Salvar(matriculaCapacitacao);

        }

        public DTOCapacitacao AprovacoesSolucoesEducacionais(MatriculaCapacitacao matriculaCapacitacao, int pIdUsuario)
        {
            DTOCapacitacao dtoCapacitacao = new DTOCapacitacao
            {
                NomeCapacitacao = matriculaCapacitacao.Capacitacao.Nome,
                idMatricula = matriculaCapacitacao.ID,
                DataInicio = matriculaCapacitacao.DataInicio.ToString("dd/MM/yyyy"),
                DataFim = matriculaCapacitacao.DataFim.HasValue ? matriculaCapacitacao.DataFim.Value.ToString("dd/MM/yyyy") : ""
            };

            var bmMatricula = new ManterMatriculaOferta();
            ConsultarMeusCursos ConsultaMeusCursos = new ConsultarMeusCursos();

            int totalAprovacoes = 0;
            int totalSolEdu = 0;

            var manterPreRequisito = new ManterModuloPreRequisito();

            foreach (var modulo in matriculaCapacitacao.Capacitacao.ListaModulos.Distinct())
            {
                DTOModulo dtoModulo = new DTOModulo {
                    ID = modulo.ID,
                    Nome = modulo.Nome,
                    Descricao = modulo.Descricao,
                    DataInicio = modulo.DataInicio.ToString("dd/MM/yyyy"), 
                    DataFim = modulo.DataFim.HasValue ? modulo.DataFim.Value.ToString("dd/MM/yyyy") : ""
                };
                var lsSolucoesModulo = modulo.ListaSolucaoEducacional.Distinct();
                var totalAprovadoModulo = 0;
                foreach (var solucao in lsSolucoesModulo)
                {
                    var aprovado = bmMatricula.AprovacaoPorUsuarioESolucaoEducacional(pIdUsuario, solucao.SolucaoEducacional.ID);

                    if (aprovado)
                    {
                        totalAprovacoes++;
                        totalAprovadoModulo++;
                    }

                    var matOfertas = bmMatricula.ObterPorUsuarioESolucaoEducacional(pIdUsuario, solucao.SolucaoEducacional.ID).ToList();

                    var meusCursos = new List<DTOItemMeusCursos>();

                    // Se tiver Status inscrito ou pendente de confirmação segue o fluxo normal de matrículas em oferta
                    if (matOfertas.Any(x => (x.StatusMatricula == enumStatusMatricula.Inscrito || x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)))
                    {
                        ConsultaMeusCursos.PreencherDTOComInformacoesDaMatriculaOferta(meusCursos, matOfertas);

                        //dtoModulo.SolucoesModulo.Add(new DTOItemMeusCursos { Nome = solucao.SolucaoEducacional.Nome, Concluido = aprovado, IDNode = solucao.SolucaoEducacional.IdNode });                   
                        foreach (var curso in meusCursos)
                        {
                            dtoModulo.SolucoesModulo.Add(curso);
                        }
                    }
                    // Se não tiver inscrito escreve as informações básicas
                    else 
                    {
                        DTOItemMeusCursos dtoItemMeusCursos = new DTOItemMeusCursos();
                        dtoItemMeusCursos.ID = solucao.SolucaoEducacional.ID;
                        dtoItemMeusCursos.NomeSolucao = solucao.SolucaoEducacional.Nome;
                        dtoItemMeusCursos.Fornecedor = solucao.SolucaoEducacional.Fornecedor.Nome;

                        if (aprovado)
                        {
                            dtoItemMeusCursos.SituacaoID = 9;
                            dtoItemMeusCursos.Situacao = "Aprovado";
                        }
                        else
                        {
                            dtoItemMeusCursos.SituacaoID = 0;
                            dtoItemMeusCursos.Situacao = "Você não está inscrito nesse curso";
                        }

                        dtoItemMeusCursos.IDChaveExterna = solucao.SolucaoEducacional.IDChaveExterna;
                        dtoItemMeusCursos.IDNode = solucao.SolucaoEducacional.IdNode;

                        dtoModulo.SolucoesModulo.Add(dtoItemMeusCursos);
                    }
                }
                var totalSeMod = modulo.ListaSolucaoEducacional.Count();
                dtoModulo.PorcentagemConclusaoModulo = (int)(Math.Round((double)totalAprovadoModulo / (double)totalSeMod, 2) * 100);
                dtoModulo.TextoConclusaoModulo = totalAprovadoModulo.ToString() + "/" + totalSeMod.ToString();
                // Verificar pré requisitos do módulo
                dtoModulo.PreRequisitoPendente = manterPreRequisito.VerificarPreRequisitoPendente(modulo, pIdUsuario);
                totalSolEdu += totalSeMod;

                // Insere o Módulo no DTO
                dtoCapacitacao.ModulosCapacitacao.Add(dtoModulo);
            }

            // CAPACITAÇÕES
            int diasCancelamento = int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro.ToString());

            bool habilitaCancelamento;

            if (matriculaCapacitacao.Capacitacao.PermiteCancelarMatricula && matriculaCapacitacao.DataInicio.Date.AddDays(diasCancelamento) >= DateTime.Now)
            {
                habilitaCancelamento = true;
            }
            else
            {
                habilitaCancelamento = false;
            }

            int percentage = 0;

            // Se não tiver soluções disponíveis ou se não tiver feito nada retorna 0
            if (totalAprovacoes != 0 && totalSolEdu != 0)
            {
                percentage = (int)(Math.Round((double)totalAprovacoes / (double)totalSolEdu, 2) * 100);
            }

            // Insere o resto dos dados da capacitação
            dtoCapacitacao.PorcentagemConclusaoCapacitacao = percentage;
            dtoCapacitacao.TextoConclusaoCapacitacao = totalAprovacoes.ToString() + "/" + totalSolEdu.ToString();
            dtoCapacitacao.HabilitaCancelamento = habilitaCancelamento;

            return dtoCapacitacao;            
        }

        public IList<MatriculaCapacitacao> ObterPorUsuario(int idUsuario) {
            var bmMatriculaCapacitacao = new BMMatriculaCapacitacao();

            return bmMatriculaCapacitacao.ObterPorUsuario(idUsuario);
        } 
    }
}
