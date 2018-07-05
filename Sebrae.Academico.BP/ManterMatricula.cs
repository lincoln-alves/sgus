using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using System;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

using System.Linq;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;

namespace Sebrae.Academico.BP
{
    public class ManterMatricula : BusinessProcessBase
    {
        public ManterMatricula()
        {

        }

        public ManterMatricula(int idUsuario, int idSolucaoEducacional, int idOferta,
            List<int> pListaIdMetaIndividualAssociada, List<int> pListaIdMetaInstitucionalAssociada,
            AuthenticationRequest autenticacao)
        {
            var usuario = new BMUsuario().ObterPorId(idUsuario);

            if (usuario != null)
            {
                ValidarMatriculaExistente(idUsuario, idSolucaoEducacional);
                ValidarPermissaoUsuario(idUsuario, idSolucaoEducacional);
                ValidarSeUsuarioJaEstaMatriculado(usuario.ListaMatriculaOferta);
                ValidarAbandonoAtivo(idUsuario);
                ValidarOferta(idSolucaoEducacional, idOferta);
                ValidarQuantidadeDeInscricoes(idOferta, idUsuario, pListaIdMetaIndividualAssociada,
                    pListaIdMetaInstitucionalAssociada, autenticacao);
            }
        }

        public IList<DTOProcInscricoesPorStatusENivel> ConsultarRelatorioInscricoesPorStatusENivel(string statuses,
            string niveisOcupacionais, int? idUf, int? idSolucaoEducacional, DateTime? dataInicial, DateTime? dataFinal, IEnumerable<int> pUfResponsavel)
        {
            var parametros = new Dictionary<string, object>
            {
                {
                    "p_Statuses", statuses
                },
                {
                    "p_NiveisOcupacionais", niveisOcupacionais
                },
                {
                    "p_UF", idUf
                },
                {
                    "p_SolucaoEducacional", idSolucaoEducacional
                },
                {
                    "p_DataInicioMatricula", dataInicial
                },
                {
                    "p_DataFimMatricula", dataFinal
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                parametros.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                parametros.Add("P_UFResponsavel", DBNull.Value);

            var bmMatriculaOferta = new BMMatriculaOferta();
            return
                bmMatriculaOferta.ExecutarProcedure<DTOProcInscricoesPorStatusENivel>(
                    "SP_REL_INSCRICOES_POR_STATUS_E_NIVEL", parametros);
        }

        public IList<DTOConcluintesEspacoOcupacional> ObterConcluintesExternos(DateTime inicio, DateTime fim, int idUf)
        {
            var lstParam = new Dictionary<string, object>
            {
                {"DataInicio", inicio},
                {"DataFim", fim}
            };

            if (idUf != 0)
            {
                lstParam.Add("IdUf", idUf);
            }

            var bmMatriculaOferta = new BMMatriculaOferta();
            return
                bmMatriculaOferta.ExecutarProcedure<DTOConcluintesEspacoOcupacional>(
                    "DASHBOARD_REL_CONCLUINTES_EXTERNOS", lstParam);
        }

        public IList<DTODesempenhoAcademico> ConsultarDesempenhoAcademico(IDictionary<string, object> dicParams)
        {
            return new BMMatriculaOferta().ExecutarProcedure<DTODesempenhoAcademico>("SP_REL_DESEMPENHO_ACADEMICO", dicParams);
        }

        public IList<DTOMatriculasPorUF> ObterMatriculasPorUf(DateTime? dataInicio, DateTime? dataFim)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("pDataInicio",
                (!dataInicio.HasValue ? "01/01/" + DateTime.Now.Year : dataInicio.Value.ToString("MM/dd/yyyy")) +
                " 00:00:00");
            lstParam.Add("pDataFim",
                (!dataFim.HasValue ? DateTime.Now.Date.ToString("MM/dd/yyyy") : dataFim.Value.ToString("MM/dd/yyyy")) +
                " 23:59:59");

            var bmMatriculaOferta = new BMMatriculaOferta();
            return bmMatriculaOferta.ExecutarProcedure<DTOMatriculasPorUF>("DASHBOARD_REL_GraficoMatriculadosUF",
                lstParam);
        }

        public IList<DTOMatriculasPorMes> ObterMatriculasPorMes(DateTime? dataInicio, DateTime? dataFim, int idUf)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("pDataInicio",
                (!dataInicio.HasValue ? "01/01/" + DateTime.Now.Year : dataInicio.Value.ToString("MM/dd/yyyy")) +
                " 00:00:00");
            lstParam.Add("pDataFim",
                (!dataFim.HasValue ? DateTime.Now.Date.ToString("MM/dd/yyyy") : dataFim.Value.ToString("MM/dd/yyyy")) +
                " 23:59:59");

            if (idUf != 0)
            {
                lstParam.Add("IdUf", idUf);
            }

            var bmMatriculaOferta = new BMMatriculaOferta();
            return bmMatriculaOferta.ExecutarProcedure<DTOMatriculasPorMes>("DASHBOARD_REL_MatriculasPorMes", lstParam);
        }

        public DTOMatriculas ObterMatriculas(DateTime? inicio, DateTime? fim, int idUf)
        {
            inicio = inicio.HasValue ? inicio : DateTime.Now.AddYears(-1);

            fim = fim.HasValue ? fim : DateTime.Now;

            var listPrm = new Dictionary<string, object>
            {
                {"DataInicio", inicio},
                {"DataFim", fim}
            };

            if (idUf != 0) {
                listPrm.Add("IdUf",idUf);
            }

            var bmMatriculaOferta = new BMMatriculaOferta();
            return
                bmMatriculaOferta.ExecutarProcedure<DTOMatriculas>("DASHBOARD_REL_MATRICULADOS", listPrm)
                    .FirstOrDefault();
        }

        public IList<DTOConcluintesEspacoOcupacional> ObterConcluintesEmpregados(DateTime inicio, DateTime fim,int idUf)
        {
            var lstParam = new Dictionary<string, object>
            {
                {"DataInicio", inicio},
                {"DataFim", fim}
            };

            if (idUf != 0)
            {
                lstParam.Add("IdUf", idUf);
            }

            var bmMatriculaOferta = new BMMatriculaOferta();
            return
                bmMatriculaOferta.ExecutarProcedure<DTOConcluintesEspacoOcupacional>(
                    "DASHBOARD_REL_CONCLUINTES_EMPREGADOS", lstParam);
        }

        public DTOMatriculas ObterMatriculasConcluintes(DateTime? inicio, DateTime? fim, int idUf)
        {
            inicio = inicio.HasValue ? inicio : DateTime.Now.AddYears(-1);

            fim = fim.HasValue ? fim : DateTime.Now;

            var listPrm = new Dictionary<string, object>
            {
                {"DataInicio", inicio},
                {"DataFim", fim}
            };

            if (idUf != 0)
            {
                listPrm.Add("IdUf", idUf);
            }

            var bmMatriculaOferta = new BMMatriculaOferta();

            return
                bmMatriculaOferta.ExecutarProcedure<DTOMatriculas>("DASHBOARD_REL_CONCLUINTES", listPrm)
                    .FirstOrDefault();
        }

        public void ValidarMatriculaExistente(int idUsuario, int idSolucaoEducacional)
        {
            MatriculaOferta buscaMatricula = new MatriculaOferta();
            buscaMatricula.Usuario = new Usuario();
            buscaMatricula.Usuario.ID = idUsuario;
            List<MatriculaOferta> possiveisMatriculas = new BMMatriculaOferta().ObterPorFiltro(buscaMatricula).ToList();
            if (possiveisMatriculas != null &&
                possiveisMatriculas.Any(
                    y =>
                        y.Oferta.SolucaoEducacional.ID == idSolucaoEducacional &&
                        !(y.StatusMatricula == enumStatusMatricula.CanceladoAdm ||
                          y.StatusMatricula == enumStatusMatricula.CanceladoAluno)))
            {
                throw new AcademicoException(
                    "Erro: O usuário já está matriculado em uma oferta desta Solução Educacional");
            }
        }

        public void ValidarPermissaoUsuario(int idUsuario, int idSolucaoEducacional)
        {
            bool usuarioPossuiPermissao = new BMSolucaoEducacional().VerificarSeUsuarioPossuiPermissao(idUsuario,
                idSolucaoEducacional);
            if (!usuarioPossuiPermissao)
            {
                throw new AcademicoException("Erro: O usuário Informado não possui permissão à Solução Educacional");
            }
        }

        public void ValidarSeUsuarioJaEstaMatriculado(IList<MatriculaOferta> listaMatriculaOferta)
        {
            IList<MatriculaOferta> ListaMatriculaOferta = listaMatriculaOferta;
            int cursosEmAndamento =
                ListaMatriculaOferta.Where(x => x.StatusMatricula.Equals(enumStatusMatricula.Inscrito)).Count();
            int limteCursosSimultaneos =
                int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.CursosSimultaneos).Registro);
            if (cursosEmAndamento >= limteCursosSimultaneos)
            {
                throw new AcademicoException(string.Format("Você já está matriculado em {0} ou mais soluções",
                    limteCursosSimultaneos.ToString()));
            }
        }

        public void ValidarAbandonoAtivo(int idUsuario)
        {
            if (new BMUsuarioAbandono().ValidarAbandonoAtivo(idUsuario))
            {
                throw new AcademicoException("Erro: Existe um abandono registrado para este usuário!");
            }
        }

        public void ValidarOferta(int idSolucaoEducacional, int idOferta)
        {
            SolucaoEducacional solucaoEducacional = new BMSolucaoEducacional().ObterPorId(idSolucaoEducacional);
            Oferta oferta = new Oferta();
            oferta = solucaoEducacional.ListaOferta.FirstOrDefault(x => x.ID == idOferta);
            if (oferta == null)
                throw new AcademicoException("Erro: Oferta não encontrada");
        }

        public void ValidarQuantidadeDeInscricoes(int idOferta, int idUsuario, List<int> pListaIdMetaIndividualAssociada,
            List<int> pListaIdMetaInstitucionalAssociada, AuthenticationRequest autenticacao)
        {
            Oferta oferta = new BMOferta().ObterPorId(idOferta);

            if (oferta == null)
                throw new AcademicoException("Erro: Oferta não encontrada");

            if (oferta.TipoOferta.Equals(enumTipoOferta.Continua))
            {
                ValidarOfertaContinua(oferta, idUsuario, pListaIdMetaIndividualAssociada,
                    pListaIdMetaInstitucionalAssociada, autenticacao);
            }
            if (oferta.TipoOferta.Equals(enumTipoOferta.Normal))
            {
                ValidarOfertaNormal(oferta, idUsuario, pListaIdMetaIndividualAssociada,
                    pListaIdMetaInstitucionalAssociada, autenticacao);
            }
        }

        private void ValidarOfertaNormal(Oferta oferta, int idUsuario, List<int> pListaIdMetaIndividualAssociada,
            List<int> pListaIdMetaInstitucionalAssociada, AuthenticationRequest autenticacao)
        {
            int qtdInscritosNaOferta =
                oferta.ListaMatriculaOferta.Where(x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                        x.StatusMatricula != enumStatusMatricula.CanceladoAluno))
                    .Count();

            Usuario usuario = new BMUsuario().ObterPorId(idUsuario);

            MatriculaOferta matriculaOferta = new MatriculaOferta()
            {
                Oferta = new BMOferta().ObterPorId(oferta.ID),
                Usuario = usuario,
                Auditoria = new Auditoria(autenticacao.Login),
                DataSolicitacao = DateTime.Now,
                UF = new BMUf().ObterPorId(usuario.UF.ID),
                NivelOcupacional = new BMNivelOcupacional().ObterPorID(usuario.NivelOcupacional.ID)
            };

            if ((oferta.QuantidadeMaximaInscricoes > 0) && qtdInscritosNaOferta >= oferta.QuantidadeMaximaInscricoes)
            {
                if (oferta.FiladeEspera)
                {
                    matriculaOferta.StatusMatricula = enumStatusMatricula.FilaEspera;
                }
                else
                {
                    throw new AcademicoException("Erro: A quantidade máxima de alunos foi atingida");
                }
            }
            else
            {
                matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
            }


            BMMatriculaOferta bmMatriculaOferta = new BMMatriculaOferta();
            bmMatriculaOferta.Salvar(matriculaOferta);
            qtdInscritosNaOferta++;

            //validando se a turma já está chegando ao limite.
            if (qtdInscritosNaOferta >
                (oferta.QuantidadeMaximaInscricoes -
                 int.Parse(
                     ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta)
                         .Registro)))
            {
                EnviarEmailLimiteOferta(oferta, matriculaOferta);
            }


            ValidarMetaIndividual(idUsuario, oferta.SolucaoEducacional.ID, pListaIdMetaIndividualAssociada, autenticacao);
            ValidarMetaInstitucional(idUsuario, oferta.SolucaoEducacional.ID, pListaIdMetaInstitucionalAssociada,
                autenticacao);
        }

        private void ValidarOfertaContinua(Oferta oferta, int idUsuario, List<int> pListaIdMetaIndividualAssociada,
            List<int> pListaIdMetaInstitucionalAssociada, AuthenticationRequest autenticacao)
        {
            bool fornecedorNotificado = false;

            Turma t = null;
            if (oferta.SolucaoEducacional.Fornecedor.PermiteGestaoSGUS ||
                oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW)
                t =
                    oferta.ListaTurma.FirstOrDefault(
                        x => x.DataFinal == null || x.DataFinal.Value.Date >= DateTime.Now.Date && x.InAberta);

            int qtdInscritosNaOferta =
                oferta.ListaMatriculaOferta.Where(x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                        x.StatusMatricula != enumStatusMatricula.CanceladoAluno) &&
                                                       x.StatusMatricula != enumStatusMatricula.FilaEspera).Count();

            var usuarioLogado = new BMUsuario().ObterPorId(idUsuario);

            MatriculaOferta matriculaOferta = new MatriculaOferta()
            {
                Oferta = new BMOferta().ObterPorId(oferta.ID),
                Usuario = new BMUsuario().ObterPorId(idUsuario),
                Auditoria = new Auditoria(new BMUsuario().ObterUsuarioLogado().CPF),
                DataSolicitacao = DateTime.Now,
                UF = new BMUf().ObterPorId(usuarioLogado.UF.ID),
                NivelOcupacional = new BMNivelOcupacional().ObterPorID(usuarioLogado.NivelOcupacional.ID)
            };

            if (!fornecedorNotificado)
            {
                try
                {
                    if (oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW &&
                        matriculaOferta != null)
                    {
                        //MatriculaOferta mo = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(matriculaOferta.ID);
                        NotificaFornecedor.Instancia.Notificar(matriculaOferta);
                        if (matriculaOferta.ID > 0)
                            new ManterMatriculaOferta().AtualizarMatriculaOferta(matriculaOferta, false);
                    }
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErro(ex);
                    throw new AcademicoException(
                        "Erro: Ocorreu um erro ao matricular neste curso. Por favor, entre em contato com o atendimento ou tente novamente mais tarde. Obrigado");
                }
            }

            if (oferta.QuantidadeMaximaInscricoes > 0)
            {
                if (qtdInscritosNaOferta >= oferta.QuantidadeMaximaInscricoes)
                {
                    if (oferta.FiladeEspera)
                    {
                        matriculaOferta.StatusMatricula = enumStatusMatricula.FilaEspera;
                    }
                    else
                    {
                        throw new AcademicoException("Erro: A quantidade máxima de alunos foi atingida");
                    }
                }
                else
                {
                    matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                }

            }
            else
            {
                matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
            }

            qtdInscritosNaOferta++;

            var ofertaEstado = oferta.ListaPermissao.FirstOrDefault(f => f.Uf != null && f.Uf.ID == usuarioLogado.UF.ID);
            if (ofertaEstado == null)
            {
                throw new AcademicoException("Erro: A vaga não é permitida para o seu estado");
            }
            else
            {
                if (ofertaEstado.QuantidadeVagasPorEstado > 0)
                {
                    int qtdMatriculaOfertaPorEstado =
                        oferta.ListaMatriculaOferta.Count(x => !x.IsUtilizado() && x.Usuario.ID == usuarioLogado.UF.ID);

                    if (qtdMatriculaOfertaPorEstado >= ofertaEstado.QuantidadeVagasPorEstado && !oferta.FiladeEspera)
                    {
                        throw new AcademicoException("Erro: As vagas já foram preenchidas para o seu estado");
                    }
                }
            }

            if (t != null)
            {
                var matriculaTurma = new MatriculaTurma
                {
                    Turma = new BMTurma().ObterPorID(t.ID),
                    Auditoria = new Auditoria(usuarioLogado.CPF),
                    DataMatricula = DateTime.Now,
                    MatriculaOferta = matriculaOferta
                };

                // Obter a DataLimite a partir dos cálculos pela Oferta.
                matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite();

                if (matriculaOferta.MatriculaTurma == null)
                    matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();

                matriculaOferta.MatriculaTurma.Add(matriculaTurma);
            }

            matriculaOferta.FornecedorNotificado = false;
            //moBM.Salvar(matriculaOferta);

            var turma = new BMTurma().ObterPorID(t.ID);
            if (turma.QuantidadeMaximaInscricoes > 0)
            {
                if (turma.QuantidadeAlunosMatriculadosNaTurma >= turma.QuantidadeMaximaInscricoes)
                {
                    throw new AcademicoException("Erro: As vagas para esta turma já foram preenchidas");
                }
            }

            //validando se a turma já está chegando ao limite.
            if (qtdInscritosNaOferta >
                (oferta.QuantidadeMaximaInscricoes -
                 int.Parse(
                     ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta)
                         .Registro)))
            {
                EnviarEmailLimiteOferta(oferta, matriculaOferta);
            }

            string parametrosControle = string.Empty;

            try
            {
                string.Format("Notificar Usuário <br /> Oferta: {0} <br /> Aluno: {1} <br />",
                    matriculaOferta.Oferta.ID.ToString(), usuarioLogado.ID);
                if (matriculaOferta.Oferta.TipoOferta.Equals(enumTipoOferta.Continua) &&
                    matriculaOferta.MatriculaTurma != null)
                {
                    NotificaFornecedor.Instancia.Notificar(matriculaOferta);

                }
            }
            catch (Exception)
            {
                //ErroUtil.Instancia.TratarErro(ex, parametrosControle);
            }
            BMMatriculaOferta moBM = new BM.Classes.BMMatriculaOferta();
            moBM.Salvar(matriculaOferta);
            ValidarMetaIndividual(idUsuario, oferta.SolucaoEducacional.ID, pListaIdMetaIndividualAssociada, autenticacao);
            ValidarMetaInstitucional(idUsuario, oferta.SolucaoEducacional.ID, pListaIdMetaInstitucionalAssociada,
                autenticacao);
        }

        public static void EnviarEmailLimiteOferta(Oferta oferta, MatriculaOferta matriculaOferta)
        {
            try
            {
                var template = TemplateUtil.ObterInformacoes(enumTemplate.NotificacaoLimiteOferta);

                var assuntoDoEmail = template.Assunto;

                var textoEmail = template.TextoTemplate;

                textoEmail = textoEmail
                    .Replace("#NOMESOLUCAOEDUCACIONAL#", matriculaOferta.Oferta.SolucaoEducacional.Nome)
                    .Replace("#NOMEOFERTA#", matriculaOferta.Oferta.Nome)
                    .Replace("#QTDINSCRITOSOFERTA#", oferta.ListaMatriculaOferta.Count.ToString());

                EmailUtil.Instancia.EnviarEmail(oferta.EmailResponsavel,
                    assuntoDoEmail,
                    textoEmail);
            }
            catch
            {
                //NADA A FAZER
            }
        }

        private void ValidarMetaIndividual(int pUsuario, int pSolucaoEducacional,
            List<int> pListaIdMetaIndividualAssociada, AuthenticationRequest autenticacao)
        {
            try
            {

                MetaIndividual metaIndividual = null;
                if (pListaIdMetaIndividualAssociada != null && pListaIdMetaIndividualAssociada.Count > 0)
                {

                    foreach (int IdMetaIndividualAssociada in pListaIdMetaIndividualAssociada)
                    {

                        using (BMMetaIndividual miBM = new BMMetaIndividual())
                            metaIndividual = miBM.ObterPorID(IdMetaIndividualAssociada);


                        if (
                            !metaIndividual.ListaItensMetaIndividual.Any(
                                x => x.SolucaoEducacional.ID == pSolucaoEducacional))
                        {
                            metaIndividual.ListaItensMetaIndividual.Add(new ItemMetaIndividual()
                            {
                                Auditoria = new Auditoria(autenticacao.Login),
                                MetaIndividual = new BMMetaIndividual().ObterPorID(metaIndividual.ID),
                                SolucaoEducacional = new BMSolucaoEducacional().ObterPorId(pSolucaoEducacional),
                            });

                            using (BMMetaIndividual miBM = new BMMetaIndividual())
                                miBM.Salvar(metaIndividual);
                        }

                    }


                    SolucaoEducacional se = null;
                    using (BMSolucaoEducacional seBM = new BMSolucaoEducacional())
                        se = seBM.ObterPorId(pSolucaoEducacional);

                    Usuario user = null;
                    using (BMUsuario userBM = new BMUsuario())
                    {
                        user = userBM.ObterPorId(pUsuario);
                    }

                    bool listaAlterada = false;
                    foreach (var tagSe in se.ListaTags)
                    {
                        if (!user.ListaTag.Any(x => x.Tag.ID == tagSe.ID))
                        {
                            user.ListaTag.Add(new UsuarioTag()
                            {
                                Usuario = user,
                                Auditoria = new Auditoria(autenticacao.Login),
                                Tag = new BMTag().ObterPorID(tagSe.Tag.ID),
                                DataValidade = metaIndividual.DataValidade,
                                Adicionado = false
                            });
                            listaAlterada = true;
                        }
                    }
                    if (listaAlterada)
                    {
                        using (BMUsuario userBM = new BMUsuario())
                            userBM.Salvar(user);
                    }

                }
            }
            catch
            {
                //TODO: Verificar se cabe alguma ação
            }
        }


        private void ValidarMetaInstitucional(int pUsuario, int pSolucaoEducacional,
            List<int> pListaIdMetaInstitucionalAssociada,
            AuthenticationRequest autenticacao)
        {
            try
            {
                MetaInstitucional mi = null;
                if (pListaIdMetaInstitucionalAssociada != null && pListaIdMetaInstitucionalAssociada.Count > 0)
                {

                    foreach (int IdMetaIndividualAssociada in pListaIdMetaInstitucionalAssociada)
                    {

                        mi = new BMMetaInstitucional().ObterPorID(IdMetaIndividualAssociada);


                        if (
                            !mi.ListaItensMetaInstitucional.Any(
                                x => x.Usuario.ID == pUsuario && x.SolucaoEducacional.ID == pSolucaoEducacional))
                        {
                            mi.ListaItensMetaInstitucional.Add(new ItemMetaInstitucional()
                            {
                                Auditoria = new Auditoria(autenticacao.Login),
                                MetaInstitucional = new BMMetaInstitucional().ObterPorID(mi.ID),
                                SolucaoEducacional = new BMSolucaoEducacional().ObterPorId(pSolucaoEducacional),
                                Usuario = new BMUsuario().ObterPorId(mi.ID),
                            });

                            using (BMMetaInstitucional miBM = new BMMetaInstitucional())
                                miBM.Salvar(mi);
                        }

                    }

                    SolucaoEducacional se = null;
                    using (BMSolucaoEducacional seBM = new BMSolucaoEducacional())
                        se = seBM.ObterPorId(pSolucaoEducacional);

                    Usuario user = null;
                    using (BMUsuario userBM = new BMUsuario())
                        user = userBM.ObterPorId(pUsuario);

                    foreach (var tagSe in se.ListaTags)
                    {
                        UsuarioTag ut = user.ListaTag.FirstOrDefault(x => x.Tag.ID == tagSe.ID);
                        if (ut == null)
                        {
                            user.ListaTag.Add(new UsuarioTag()
                            {
                                Usuario = new BMUsuario().ObterPorId(pUsuario),
                                Auditoria = new Auditoria(autenticacao.Login),
                                Tag = new BMTag().ObterPorID(tagSe.Tag.ID),
                                Adicionado = false
                            });
                        }

                        using (BMUsuario userBM = new BMUsuario())
                            userBM.Salvar(user);
                    }
                }
            }
            catch
            {

            }
        }
    }
}