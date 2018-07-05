using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.GestorUC;
using Sebrae.Academico.Extensions;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterMatriculaOfertaService : BusinessProcessServicesBase
    {

        public DTOGestorUC ConsultarDisponibilidadeGestorUC(int idSolucaoEducacional, string cpf, string nome, int idOferta, int turmaId, string cpfGestor)
        {
            Usuario usuario = new BMUsuario().ObterPorCPF(cpfGestor);

            if (usuario == null)
                throw new AcademicoException("CPF não localizado na base");

            if (usuario.ListaPerfil == null)
                throw new AcademicoException("Usuário sem permissão de acesso à funcionalidade");

            if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.GestorUC))
                throw new AcademicoException("Usuário sem perfil de gestor");

            var listaDTOPermissaoOfertas = new BMOferta().ObterListaDePermissoes(usuario.ID);

            if (listaDTOPermissaoOfertas == null || !listaDTOPermissaoOfertas.Any())
                throw new AcademicoException("Não há ofertas disponíveis");

            var idOfertas = listaDTOPermissaoOfertas.Where(x => x.PermiteGestorUC &&
                                                                ((x.DataFim.HasValue &&
                                                                  x.DataFim.Value.Date >= DateTime.Today) ||
                                                                 (!x.DataFim.HasValue)) &&
                                                                (x.DataInicioInscricoes.HasValue &&
                                                                 x.DataInicioInscricoes.Value.Date <=
                                                                 DateTime.Today) &&
                                                                x.IdSolucaoEducacional == idSolucaoEducacional)
                .OrderBy(x => x.DataInicio)
                .Select(x => x.IdOferta).Distinct().ToList();

            if (idOfertas == null || idOfertas.Count() == 0)
                throw new AcademicoException("Não há ofertas disponíveis");

            if (idOferta == 0)
                idOferta = idOfertas.FirstOrDefault();

            Oferta oferta = new BMOferta().ObterPorId(idOferta);

            //if (usuario.UF.ID > 1 && !(oferta.ListaPermissao.Any(x => x.Uf.ID == usuario.UF.ID)))
            //    oferta = null;

            if (oferta == null)
                throw new AcademicoException("Erro ao recuperar oferta");

            int qtdInscritosNaOferta = oferta.ListaMatriculaOferta.Where(x =>
                                                                                (x.StatusMatricula != enumStatusMatricula.CanceladoAdm ||
                                                                                x.StatusMatricula != enumStatusMatricula.CanceladoAluno) ||
                                                                                x.StatusMatricula != enumStatusMatricula.FilaEspera).Count();

            DTOGestorUC retorno = new DTOGestorUC();

            foreach (var item in idOfertas)
            {
                var objOferta = new Sebrae.Academico.BP.DTO.Dominio.DTOOferta();
                var objOfertaRecuperada = new BMOferta().ObterPorId(item);
                if (item == idOferta)
                {
                    objOferta.ID = oferta.ID;
                    objOferta.Nome = oferta.Nome;
                }
                else
                {
                    objOferta.ID = objOfertaRecuperada.ID;
                    objOferta.Nome = objOfertaRecuperada.Nome;
                }
                retorno.ListaOfertas.Add(objOferta);
            }


            retorno.IdOferta = oferta.ID;
            retorno.NomeOferta = oferta.Nome;
            retorno.DataInicioInscricoes = oferta.DataInicioInscricoes;
            retorno.DataFimInscricoes = oferta.DataFimInscricoes;
            retorno.QtdVagasDisponiveis = oferta.QuantidadeMaximaInscricoes - qtdInscritosNaOferta;
            retorno.PermiteAlterarStatusPeloGestor = (oferta.AlteraPeloGestorUC.HasValue ? oferta.AlteraPeloGestorUC.Value : true);

            if (retorno.QtdVagasDisponiveis < 0)
                retorno.QtdVagasDisponiveis = 0;

            IList<MatriculaOferta> listaMatriculaOferta = oferta.ListaMatriculaOferta.Where(f => f.StatusMatricula != enumStatusMatricula.CanceladoAdm).ToList();

            if (usuario.UF.ID == 1)
                listaMatriculaOferta = oferta.ListaMatriculaOferta;
            else if (usuario.UF.ID > 1)
                listaMatriculaOferta = oferta.ListaMatriculaOferta.Where(f => f.Usuario.UF.ID == usuario.UF.ID).ToList();



            if (oferta.ListaMatriculaOferta != null && oferta.ListaMatriculaOferta.Count > 0)
            {
                if (!string.IsNullOrEmpty(nome))
                    listaMatriculaOferta = listaMatriculaOferta.Where(f => (f.Usuario.Nome.Contains(nome) || f.Usuario.Nome.Contains(nome.ToUpper()))).ToList();

                if (!string.IsNullOrEmpty(cpf))
                    listaMatriculaOferta = listaMatriculaOferta.Where(f => f.Usuario.CPF.Contains(cpf)).ToList();

                if (turmaId > 0)
                    listaMatriculaOferta = listaMatriculaOferta.Where(f => f.MatriculaTurma.Any(c => c.Turma.ID == turmaId)).ToList();
            }

            foreach (MatriculaOferta matriculaOferta in listaMatriculaOferta.OrderBy(x => x.Usuario.Nome))
            {
                DTOGestorUCUsuario usuarioInscrito = new DTOGestorUCUsuario();
                usuarioInscrito.NomeUsuario = matriculaOferta.Usuario.Nome;
                usuarioInscrito.CPFUsuario = matriculaOferta.Usuario.CPF;
                usuarioInscrito.DataSolicitacao = matriculaOferta.DataSolicitacao;
                usuarioInscrito.IdMatriculaOferta = matriculaOferta.ID;
                usuarioInscrito.NivelOcupacional = matriculaOferta.Usuario.NivelOcupacional.Nome;
                usuarioInscrito.StatusMatriculaID = (int)matriculaOferta.StatusMatricula;
                usuarioInscrito.StatusMatricula = matriculaOferta.StatusMatriculaFormatado;
                usuarioInscrito.UF = matriculaOferta.Usuario.UF.Sigla;

                if (matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
                    usuarioInscrito.PermiteCancelamento = true;
                else
                    usuarioInscrito.PermiteCancelamento = false;

                if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count() > 0)
                {
                    usuarioInscrito.Turma.ID = matriculaOferta.MatriculaTurma.FirstOrDefault().Turma.ID;
                    usuarioInscrito.Turma.IDChaveExternaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault().Turma.IDChaveExterna;
                    usuarioInscrito.Turma.Nome = matriculaOferta.NomeTurma;
                }

                retorno.ListaMatriculados.Add(usuarioInscrito);
            }

            return retorno;
        }

        public string MatriculaSolucaoEducacionalGestorUC(int idOferta, string cpf, int idStatusMatricula, int idTurma, string solicitante, string dataInscricao = "", string dataConclusao = "")
        {
            var manterUsuario = new ManterUsuario();
            var usuario = manterUsuario.ObterPorCPF(cpf);
            var usuarioLogado = new BMUsuario().ObterPorCPF(solicitante);
            if (!(idStatusMatricula == 2 || idStatusMatricula == 6 || idStatusMatricula == 8))
                throw new AcademicoException("Status da matrícula inválido");
            if (usuario == null)
                throw new AcademicoException("CPF não localizado na base");

            var oferta = new BMOferta().ObterPorId(idOferta);

            if (oferta == null)
                throw new AcademicoException("Oferta não localizada na base");

            if (oferta.ListaMatriculaOferta == null)
                oferta.ListaMatriculaOferta = new List<MatriculaOferta>();

            if (oferta.ListaMatriculaOferta.Any(x => x.Usuario.ID == usuario.ID && (x.StatusMatricula == enumStatusMatricula.Inscrito || x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)))
                throw new AcademicoException("Usuário já inscrito nesta oferta");

            //VERIFICA PRÉ-REQUISITO
            if (oferta.SolucaoEducacional.ListaPreRequisito.Any())
            {
                var aprovados = new List<enumStatusMatricula> {
                    enumStatusMatricula.Aprovado,
                    enumStatusMatricula.Concluido
                };

                foreach (var item in oferta.SolucaoEducacional.ListaPreRequisito)
                {
                    if (!usuario.ListaMatriculaOferta.Any(x => aprovados.Contains(x.StatusMatricula) && x.Oferta.SolucaoEducacional.ID == item.PreRequisito.ID))
                    {
                        throw new AcademicoException("Erro: Existem soluções como pré-requisito que não estão cursadas");
                    }
                }
            }

            if (usuarioLogado.UF.ID != (int)enumUF.NA)
            {
                if (usuario.UF.ID != usuarioLogado.UF.ID) throw new AcademicoException("Este usuário não é do seu estado");
            }

            var ofertaEstado = oferta.ListaPermissao.FirstOrDefault(f => f.Uf != null && f.Uf.ID == usuario.UF.ID);
            if (ofertaEstado == null)
            {
                throw new AcademicoException("Erro: A vaga não é permitida para o seu estado");
            }
            if (ofertaEstado.QuantidadeVagasPorEstado > 0)
            {
                var qtdMatriculaOfertaPorEstado = oferta.ListaMatriculaOferta.Count(x => !x.IsUtilizado() && x.UF.ID == ofertaEstado.Uf.ID);

                if (qtdMatriculaOfertaPorEstado >= ofertaEstado.QuantidadeVagasPorEstado && !oferta.FiladeEspera)
                {
                    throw new AcademicoException("Erro: As vagas já foram preenchidas para o seu estado");
                }
            }

            if (usuario.ListaMatriculaOferta.Any(x => x.Usuario.ID == usuario.ID && x.Oferta.ID == oferta.ID && x.Oferta.SolucaoEducacional.ID == oferta.SolucaoEducacional.ID && (x.StatusMatricula == enumStatusMatricula.Inscrito ||
                                                                                                                            x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno ||
                                                                                                                            x.StatusMatricula == enumStatusMatricula.Aprovado ||
                                                                                                                            x.StatusMatricula == enumStatusMatricula.Concluido
                                                                                                                            )))
            {
                throw new AcademicoException("Usuário já cursou esta Solução Educacional");
            }

            var matriculaOferta = new MatriculaOferta
            {
                Usuario = usuario,
                Oferta = oferta,
                DataSolicitacao = DateTime.Now,
                StatusMatricula = (enumStatusMatricula)idStatusMatricula,
                UF = usuario.UF,
                NivelOcupacional = usuario.NivelOcupacional,
                Auditoria = new Auditoria(solicitante)
            };

            (new ManterMatriculaOferta()).VerificarPoliticaDeConsequencia(matriculaOferta);

            if (matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
            {
                Turma turma;
                if (idTurma > 0)
                {
                    turma = oferta.ListaTurma.FirstOrDefault(x => x.ID == idTurma);
                    if (turma == null) throw new AcademicoException("A turma informada não pertence a oferta informada");
                }
                else
                {
                    turma = oferta.ListaTurma.FirstOrDefault();
                }

                if (turma != null)
                {
                    if (!turma.InAberta)
                    {
                        throw new AcademicoException("A turma não está aberta para inscrições");
                    }

                    if (!usuarioLogado.IsGestor() && (turma.Oferta.DataInicioInscricoes > DateTime.Now || (turma.Oferta.DataFimInscricoes.HasValue && turma.Oferta.DataFimInscricoes.Value < DateTime.Now)))
                        throw new AcademicoException("A oferta selecionada está fora do período de inscrição");

                    //Verifica se a quantidade de matriculas para a turma foi preenchida.
                    if (turma.QuantidadeMaximaInscricoes > 0 && turma.QuantidadeAlunosMatriculadosNaTurma >= turma.QuantidadeMaximaInscricoes)
                        throw new AcademicoException("Todas as vagas para esta turma foram preenchidas");

                    var matriculaTurma = new MatriculaTurma
                    {
                        Turma = turma,
                        MatriculaOferta = matriculaOferta,
                        DataMatricula = DateTime.Now,
                        Auditoria = new Auditoria(solicitante)
                    };

                    matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite(matriculaOferta.Oferta);

                    matriculaTurma.DataMatricula = DateTime.Now;
                    if (!usuarioLogado.IsGestor() && matriculaOferta.Oferta.DataInicioInscricoes.HasValue && matriculaOferta.Oferta.DataFimInscricoes.HasValue && !matriculaTurma.DataMatricula.Date.Between(matriculaOferta.Oferta.DataInicioInscricoes.Value, matriculaOferta.Oferta.DataFimInscricoes.Value))
                    {
                        throw new AcademicoException(
                            "Data de matrícula fora do periodo de inscrição da turma que é de " +
                            matriculaTurma.Turma.DataInicio.ToString("dd/MM/yyyy") + " a " +
                            (matriculaTurma.Turma.DataFinal != null
                                ? matriculaTurma.Turma.DataFinal.Value.ToString("dd/MM/yyyy")
                                : ""));
                    }

                    if (matriculaOferta.MatriculaTurma == null) matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();

                    matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                }
            }

            new BMMatriculaOferta().Salvar(matriculaOferta);

            try
            {
                if (matriculaOferta.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)
                {
                    var templateMensagemEmailOfertaExclusiva = TemplateUtil.ObterInformacoes(enumTemplate.EnvioEmailParaPendenteDeConfirmacao);

                    var assuntoDoEmail = templateMensagemEmailOfertaExclusiva.Assunto;

                    var corpoEmail = templateMensagemEmailOfertaExclusiva.TextoTemplate;

                    var emailDoDestinatario = matriculaOferta.Usuario.Email;

                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)
                    {
                        assuntoDoEmail = assuntoDoEmail.Replace("#NOME_CURSO", matriculaOferta.NomeSolucaoEducacional);
                        corpoEmail = new ManterOferta().CorpoEmail(corpoEmail, matriculaOferta);

                        EmailUtil.Instancia.EnviarEmail(emailDoDestinatario.Trim(), assuntoDoEmail, corpoEmail);
                    }
                }

            }
            catch
            {
                // ignored
            }

            return string.Empty;
        }

        public string CancelaMatriculaSolucaoEducacionalGestorUC(int idMatriculaOferta, string solicitante)
        {
            BMMatriculaOferta bmMatriculaOferta = new BMMatriculaOferta();
            MatriculaOferta matriculaOferta = bmMatriculaOferta.ObterPorID(idMatriculaOferta);
            if (matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
            {
                matriculaOferta.StatusMatricula = enumStatusMatricula.CanceladoAdm;
                if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                    matriculaOferta.MatriculaTurma.Clear();
                matriculaOferta.Auditoria = new Auditoria(solicitante);
                bmMatriculaOferta.Salvar(matriculaOferta);
            }
            else if (matriculaOferta.StatusMatricula == enumStatusMatricula.CanceladoAdm || matriculaOferta.StatusMatricula == enumStatusMatricula.CanceladoAluno)
            {
                throw new AcademicoException("A matrícula já se encontra cancelada");
            }
            else
            {
                throw new AcademicoException("O status atual da matrícula não permite o cancelamento.");
            }

            return string.Empty;
        }

        public DTOConsultarStatusMatricula ConsultarStatusMatricula()
        {
            var retorno = new DTOConsultarStatusMatricula();

            var listaStatus = new BMStatusMatricula().ObterTodos();
            retorno.Lista.AddRange(listaStatus.Select(f => new DTOStatusMatricula { ID = f.ID, Nome = f.Nome }));

            return retorno;
        }

        public string ManterStatusMatriculaGestorUC(int idMatriculaOferta, int idTurma)
        {
            string retorno = "";

            if (idTurma == 0)
            {
                /* Quando o usuário escolher a turma vazia, ou seja, a opção "Selecione",
                   um alert deverá avisar que os dados do aluno como nota e presença serão perdidos. */
                //comboTurma.Attributes.Add("OnClientClick="return confirm('Deseja Realmente Excluir este Registro?');"
                this.ExcluirAlunoDaTurma(idMatriculaOferta);

                retorno = "O usuário foi excluído da turma";

            }
            else
            {
                retorno = this.MatricularAlunoNaTurma(idMatriculaOferta, idTurma);
            }

            return retorno;
        }

        private string MatricularAlunoNaTurma(int idMatriculaOferta, int idTurma)
        {
            string retorno = "Ocorreu um erro no processo";

            MatriculaOferta matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);

            if (matriculaOferta != null)
            {
                MatriculaTurma matriculaTurma = this.ObterObjetoMatriculaTurma(idTurma, matriculaOferta);
                ManterMatriculaOferta manterMatriculaOferta = new ManterMatriculaOferta();
                if (matriculaOferta.MatriculaTurma.Count == 0)
                    matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                else
                    matriculaOferta.MatriculaTurma[0] = matriculaTurma;
                manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta);

                retorno = string.Format("O usuário '{0}' foi matriculado na turma '{1}'", matriculaOferta.Usuario.Nome, matriculaTurma.Turma.Nome);
            }

            return retorno;
        }

        private void ExcluirAlunoDaTurma(int idMatriculaOferta)
        {
            MatriculaOferta matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);

            if (matriculaOferta != null && matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
            {
                matriculaOferta.MatriculaTurma.Clear();
                AtualizarStatusDaOferta(matriculaOferta.StatusMatricula, matriculaOferta);
            }
        }

        private void AtualizarStatusDaOferta(enumStatusMatricula statusMatriculaOferta, MatriculaOferta matriculaOferta)
        {
            //Atualiza o status da Oferta
            ManterMatriculaOferta manterMatriculaOferta = new ManterMatriculaOferta();
            matriculaOferta.StatusMatricula = statusMatriculaOferta;
            manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false);
        }

        private MatriculaTurma ObterObjetoMatriculaTurma(int idTurma, MatriculaOferta matriculaOferta)
        {
            MatriculaTurma matriculaTurma = null;
            if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

            //Se o usuário não estiver matriculado em nenhuma turma, preenche o objeto matricula turma com os dados da oferta.
            if (matriculaTurma == null)
            {
                matriculaTurma = new MatriculaTurma
                {
                    MatriculaOferta = matriculaOferta,
                    Turma = new ManterTurma().ObterTurmaPorID(idTurma),
                    DataMatricula = DateTime.Now
                };

                matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite(matriculaOferta.Oferta);
            }
            else
            {
                //Troca a turma, pois o usuário informou uma nova turma
                int idTurmaEscolhidaNaCombo = idTurma;
                if (!matriculaTurma.ID.Equals(idTurmaEscolhidaNaCombo))
                {
                    matriculaTurma.TurmaAnterior = matriculaTurma.Turma;

                    /* Troca a Turma do usuário (ou seja, matricula o aluno em uma nova turma), 
                       pois ele escolheu uma nova turma na combo.*/
                    matriculaTurma.Turma = new ManterTurma().ObterTurmaPorID(idTurma);

                }
            }

            return matriculaTurma;
        }

    }
}
