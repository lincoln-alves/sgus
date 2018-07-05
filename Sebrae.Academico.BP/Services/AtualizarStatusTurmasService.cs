using System;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Services
{
    public class AtualizarStatusTurmasService : BusinessProcessServicesBase
    {
        public RetornoWebService AtualizarStatusTurmas()
        {
            var retorno = new RetornoWebService();

            var manter = new ManterTurma();

            var contador = manter.ObterTodasTurma()
                        .Count(
                            t => t.Status != null &&
                                 // Vai de Prevista direto para Em andamento. Não concordo, mas o cliente insistiu.
                                 (((t.Status == enumStatusTurma.Prevista || t.Status == enumStatusTurma.Confirmada) &&
                                  t.DataInicio <= DateTime.Now)
                                 ||
                                 (t.Status == enumStatusTurma.EmAndamento && t.DataFinal.HasValue &&
                                  t.DataFinal.Value < DateTime.Now)));


            for (int i = 10; i < contador; i += 10)
            {
                // Atualiza registros de 10 em 10
                int skip = i == 10 ? 0 : i - 10;

                // Obter todas as turmas cujas datas início ou final são hoje ou anteriores e que possuam status de Confirmada ou Em Andamento.
                var turmas = manter.ObterTodasTurma()
                        .Where(
                            t => t.Status != null &&
                                 // Vai de Prevista direto para Em andamento. Não concordo, mas o cliente insistiu.
                                 (((t.Status == enumStatusTurma.Prevista || t.Status == enumStatusTurma.Confirmada) &&
                                  t.DataInicio <= DateTime.Now)
                                 ||
                                 (t.Status == enumStatusTurma.EmAndamento && t.DataFinal.HasValue &&
                                  t.DataFinal.Value < DateTime.Now))
                        ).Select(t => t.ID).Skip(skip).Take(10).ToList();

                foreach (var id in turmas)
                {
                    try
                    {
                        // Buscar a turma novamente, pois o NHibernate está mal configurado e pira nessa parte.
                        var turma = new ManterTurma().ObterTurmaPorID(id);

                        AtualizarStatusTurma(turma);
                    }
                    catch
                    {
                        retorno.Erro++;
                    }
                }
            }

            if (retorno.Erro > 0)
                retorno.Mensagem = string.Format(
                    "Houve{0} erro{1} na{1} sincronia{1} de {2} turma{1}. Tente novamente.",
                    retorno.Erro > 1 ? "ram" : "", retorno.Erro > 1 ? "s" : "", retorno.Erro);

            return retorno;
        }

        private void AtualizarStatusTurma(Turma turma)
        {
            var statusAnterior = turma.Status;

            switch (turma.Status)
            {
                case enumStatusTurma.Prevista:
                    // Vai de Prevista direto para Em andamento. Não concordo, mas o cliente insistiu.
                    turma.Status = enumStatusTurma.EmAndamento;
                    break;
                case enumStatusTurma.Confirmada:
                    turma.Status = enumStatusTurma.EmAndamento;
                    break;
                case enumStatusTurma.EmAndamento:
                    turma.Status = enumStatusTurma.Realizada;
                    break;
            }

            // Caso a data final ainda seja anterior ao presente e o status tenha sido alterado para Em Andamento, alterar diretamente para realizada.
            if (turma.DataFinal != null &&
                (turma.DataFinal.Value.Date < DateTime.Today && turma.Status == enumStatusTurma.EmAndamento))
                turma.Status = enumStatusTurma.Realizada;

            new ManterTurma().AlterarTurma(turma, statusAnterior: statusAnterior);
        }
    }
}