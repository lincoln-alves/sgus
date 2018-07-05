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
using webaula = Sebrae.Academico.BP.waIntegracao;
using fgvOCW = Sebrae.Academico.BP.fgvIntegracaoOCW;
using moodle = Sebrae.Academico.BP.moodleIntegracao;


namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterSolucaoEducacional : BusinessProcessServicesBase
    {

        private BMSolucaoEducacional solucaoEducacionalBM;


        public IList<DTOSolucEducFormaAquisicao> ConsultarSolucaoEducacional(int pUsuario, int pFornecedor, int pFormaAquisicao)
        {
            solucaoEducacionalBM = new BMSolucaoEducacional();

            IList<SolucaoEducacional> lstSolEduc = solucaoEducacionalBM.ConsultarSolucaoEducacionalWebServices(pUsuario, pFornecedor, pFormaAquisicao);



            IList<FormaAquisicao> lstFormaAquisicao = (from fa in lstSolEduc
                                                       select fa.FormaAquisicao).ToList();




            IList<DTOSolucEducFormaAquisicao> lstResult = new List<DTOSolucEducFormaAquisicao>();


            foreach (FormaAquisicao fa in lstFormaAquisicao)
            {
                DTOSolucEducFormaAquisicao dtofa = new DTOSolucEducFormaAquisicao()
                {
                    Nome = fa.Nome,
                    CodigoFormaAquisicao = fa.ID,
                    ListaSolucaoEducacional = new List<DTOSolucEducSolucaoEducacional>()
                };


                IList<SolucaoEducacional> lstSolEducFA = (from se in lstSolEduc
                                                          where se.FormaAquisicao.ID == fa.ID
                                                          select se).ToList();

                foreach (SolucaoEducacional se in lstSolEducFA)
                {



                    MatriculaOferta mo = (from of in (new BMOferta()).ObterPorFiltro(new Oferta() { SolucaoEducacional = se })
                                          select of.ListaMatriculaOferta).FirstOrDefault().Where(x => x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                                                                      x.StatusMatricula != enumStatusMatricula.CanceladoAluno)
                                                                                                      .OrderByDescending(x => x.DataSolicitacao).FirstOrDefault();



                    DTOSolucEducSolucaoEducacional dtoSE = new DTOSolucEducSolucaoEducacional()
                    {
                        CodigoSolucaoEducacional = se.ID,
                        Nome = se.Nome,
                        SolucaoEducacionalMatricula = mo == null ? null : (new DTOSolucEducSolucaoEducacionalMatricula()
                        {
                            DataSolicitacao = mo.DataSolicitacao,
                            StatusMatricula = mo.StatusMatricula.ToString()
                        })
                    };



                    dtofa.ListaSolucaoEducacional.Add(dtoSE);

                }


                lstResult.Add(dtofa);
            }

            return lstResult;
        }

        public void CancelarMatriculaSolucaoEducacional(int idMatriculaOferta, AuthenticationRequest autenticacao)
        {
            MatriculaOferta matriculaOferta = new BMMatriculaOferta().ObterPorID(idMatriculaOferta);

            if (matriculaOferta != null)
            {

                TimeSpan diasMatriculados = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(matriculaOferta.DataSolicitacao);
                if (diasMatriculados.Days > int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro))
                {
                    //Passou do Limite para cancelamento
                    throw new AcademicoException("Prazo expirado para o cancelamento");
                }

                if (matriculaOferta.MatriculaTurma != null)
                {

                    //matriculaOferta.MatriculaTurma.StatusMatricula = enumStatusMatricula.CanceladoAluno;
                    //matriculaOferta.MatriculaTurma.Auditoria = new Auditoria(autenticacao.Login);
                }
                matriculaOferta.StatusMatricula = enumStatusMatricula.CanceladoAluno;
                if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                    matriculaOferta.MatriculaTurma.Clear();

                matriculaOferta.Auditoria = new Auditoria(autenticacao.Login);
                new BMMatriculaOferta().Salvar(matriculaOferta);

            }
            else
            {
                throw new AcademicoException("Matrícula não encontrada");
            }
        }

        public void MatricularSolucaoEducacional(int idUsuario, int idSolucaoEducacional, int idOferta,
                                                List<int> pListaIdMetaIndividualAssociada, List<int> pListaIdMetaInstitucionalAssociada, AuthenticationRequest autenticacao)
        {

            Usuario userSelected = new BMUsuario().ObterPorId(idUsuario);
            SolucaoEducacional solucaoEducacional = new BMSolucaoEducacional().ObterPorId(idSolucaoEducacional);
            BMMatriculaOferta moBM = new BMMatriculaOferta();
            if (solucaoEducacional == null)
                throw new AcademicoException("Solução Educacional não encontrada");

            //VALIDAR SE ELE TEM ALGUMA OFERTA EXCLUSIVA PENDENTE DE CONFIRMACAO
            if (userSelected.ListaMatriculaOferta.Any(x => x.Oferta.ID == idOferta && x.Oferta.SolucaoEducacional.ID == idSolucaoEducacional && x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno && x.Oferta.DataFim.Value.Date >= DateTime.Now.Date))
            {


                MatriculaOferta mo = userSelected.ListaMatriculaOferta.Where(x => x.Oferta.SolucaoEducacional.ID == idSolucaoEducacional && x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno).FirstOrDefault();

                if (mo != null)
                {
                    mo.StatusMatricula = enumStatusMatricula.Inscrito;
                    mo.Auditoria = new Auditoria(autenticacao.Login);
                    moBM.Salvar(mo);
                    ValidarMetaIndividual(idUsuario, idSolucaoEducacional, pListaIdMetaIndividualAssociada, autenticacao);
                    ValidarMetaInstitucional(idUsuario, idSolucaoEducacional, pListaIdMetaInstitucionalAssociada, autenticacao);

                    if (!(mo.MatriculaTurma != null && mo.MatriculaTurma.Count > 0))
                    {
                        try
                        {
                            if (mo.Oferta.TipoOferta.Equals(enumTipoOferta.Continua))
                            {
                                string retornows;
                                switch (mo.Oferta.SolucaoEducacional.Fornecedor.ID)
                                {
                                    case (int)enumFornecedor.MoodleSebrae:
                                        moodle.IntegracaoSoapClient soapCliente = new moodle.IntegracaoSoapClient();
                                        retornows = soapCliente.MatricularAluno(
                                                userSelected.Nome, 
                                                userSelected.CPF, 
                                                userSelected.Email, 
                                                userSelected.Cidade, 
                                                mo.Oferta.SolucaoEducacional.IDChaveExterna.ToString(), 
                                                mo.MatriculaTurma.FirstOrDefault().Turma.IDChaveExterna.ToString());
                                        break;
                                    case (int)enumFornecedor.WebAula:
                                        Turma turma = mo.MatriculaTurma.FirstOrDefault().Turma;
                                        webaula.waIntegracaoSoapClient wa = new webaula.waIntegracaoSoapClient();
                                        webaula.AuthenticationProviderRequest aut = new webaula.AuthenticationProviderRequest();
                                        webaula.DTOUsuario dtoUsuario = new webaula.DTOUsuario();
                                        webaula.DTOTurma dtoTurma = new webaula.DTOTurma();
                                        dtoTurma.IDChaveExterna = turma.IDChaveExterna;
                                        dtoUsuario.CPF = userSelected.CPF;
                                        dtoUsuario.Email = userSelected.Email;
                                        dtoUsuario.Nome = userSelected.Nome;
                                        dtoUsuario.Sexo = userSelected.Sexo;
                                        dtoUsuario.UF = userSelected.UF.Sigla;
                                        aut.Login = mo.Oferta.SolucaoEducacional.Fornecedor.Login;
                                        aut.Senha = CriptografiaHelper.Decriptografar(mo.Oferta.SolucaoEducacional.Fornecedor.Senha);
                                        retornows = wa.Matricular(aut, dtoUsuario, dtoTurma);
                                        break;
                                    

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            ErroUtil.Instancia.TratarErro(ex);
                        }
                    }

                    if (!string.IsNullOrEmpty(mo.Usuario.Email))
                    {

                        Template mensagemRecuperacaoDeSenhaSemConfirmacao = TemplateUtil.ObterInformacoes(enumTemplate.InscricaoSESucesso);
                        string assuntoDoEmail = mensagemRecuperacaoDeSenhaSemConfirmacao.DescricaoTemplate.Substring(0, mensagemRecuperacaoDeSenhaSemConfirmacao.DescricaoTemplate.IndexOf(Environment.NewLine));

                        Dictionary<string, string> registros = new Dictionary<string, string>();
                        registros.Add("NOMESOLUCAOEDUCACIONAL", mo.Oferta.SolucaoEducacional.Nome);
                        registros.Add("DATASISTEMA", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm"));
                        registros.Add("NOMEALUNO", mo.Usuario.Nome);
                        //Envia e-mail para o usuário 
                        EmailUtil.Instancia.EnviarEmail(mo.Usuario.Email,
                                                            assuntoDoEmail,
                                                            this.FormatarTextoEmail(registros, mensagemRecuperacaoDeSenhaSemConfirmacao.DescricaoTemplate)
                                                            );

                    }

                    return;
                }

            }

            //Verificando se existe alguma matricula na SE.
            MatriculaOferta buscaMatricula = new MatriculaOferta();
            buscaMatricula.Usuario = new Usuario();
            buscaMatricula.Usuario.ID = userSelected.ID;
            List<MatriculaOferta> possiveisMatriculas = new BMMatriculaOferta().ObterPorFiltro(buscaMatricula).ToList();
            if (possiveisMatriculas != null && possiveisMatriculas.Any(y => y.Oferta.SolucaoEducacional.ID == idSolucaoEducacional && !(y.StatusMatricula == enumStatusMatricula.CanceladoAdm || y.StatusMatricula == enumStatusMatricula.CanceladoAluno)))
            {
                throw new AcademicoException("Erro: O usuário já está matriculado em uma oferta desta Solução Educacional");
            }

            //VALIDAR SE O USUARIO POSSUI ACESSO A SE
            bool usuarioPossuiPermissao = new BMSolucaoEducacional().VerificarSeUsuarioPossuiPermissao(idUsuario, solucaoEducacional.ID); // .ObterListaUsuariosPermitidos();
            if (!usuarioPossuiPermissao)
            {
                throw new AcademicoException("Erro: O usuário Informado não possui permissão à Solução Educacional");
            }

            //VALIDAR SE O USUARIO ESTA CURSANDO OUTRA SE
            if (userSelected.ListaMatriculaOferta.Any(y => y.Usuario.ID == userSelected.ID && y.StatusMatricula == enumStatusMatricula.Inscrito))
            {
                throw new AcademicoException("Erro: O usuário já está inscrito em outra oferta.");
            }

            //VALIDAR SE O USUARIO ESTA COM ALGUM ABANDONO ATIVO
            if (new BMUsuarioAbandono().ValidarAbandonoAtivo(idUsuario))
            {
                throw new AcademicoException("Erro: Existe um abandono registrado para este usuário!");
            }


            Oferta oferta = new Oferta();
            oferta = solucaoEducacional.ListaOferta.FirstOrDefault(x => x.ID == idOferta);
            if (oferta == null)
                throw new AcademicoException("Erro: Oferta não encontrada");

            //VALIDADO OFERTA CONTINUA.
            if (oferta.TipoOferta.Equals(enumTipoOferta.Continua))
            {
                Turma t = null;
                if (oferta.SolucaoEducacional.Fornecedor.PermiteGestaoSGUS)
                    t = oferta.ListaTurma.FirstOrDefault(x => x.DataFinal == null || x.DataFinal.Value.Date >= DateTime.Now.Date && x.InAberta);

                int qtdInscritosNaOferta = oferta.ListaMatriculaOferta.Where(x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                                                    x.StatusMatricula != enumStatusMatricula.CanceladoAluno)).Count();
                MatriculaOferta matriculaOferta = new MatriculaOferta()
                {
                    Oferta = new BMOferta().ObterPorId(oferta.ID),
                    Usuario = new BMUsuario().ObterPorId(userSelected.ID),
                    Auditoria = new Auditoria(autenticacao.Login),
                    DataSolicitacao = DateTime.Now,
                    UF = new BMUf().ObterPorId(userSelected.UF.ID),
                    NivelOcupacional = new BMNivelOcupacional().ObterPorID(userSelected.NivelOcupacional.ID)
                };

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

                qtdInscritosNaOferta++;

                if (t != null)
                {
                    MatriculaTurma matriculaTurma = new MatriculaTurma()
                    {
                        Turma = new BMTurma().ObterPorID(t.ID),
                        Auditoria = new Auditoria(autenticacao.Login),
                        DataMatricula = DateTime.Now,
                        MatriculaOferta = matriculaOferta,
                        DataLimite = DateTime.Today.AddDays(oferta.DiasPrazo)
                    };
                    if (matriculaOferta.MatriculaTurma == null)
                        matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();
                    matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                }

                moBM.Salvar(matriculaOferta);


                //validando se a turma já está chegando ao limite.
                if (qtdInscritosNaOferta > (oferta.QuantidadeMaximaInscricoes - int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta).Registro)))
                {
                    EnviarEmailLimiteOferta(oferta, matriculaOferta);
                }


                ValidarMetaIndividual(idUsuario, idSolucaoEducacional, pListaIdMetaIndividualAssociada, autenticacao);
                ValidarMetaInstitucional(idUsuario, idSolucaoEducacional, pListaIdMetaInstitucionalAssociada, autenticacao);

                try
                {
                    if (matriculaOferta.Oferta.TipoOferta.Equals(enumTipoOferta.Continua) && matriculaOferta.MatriculaTurma != null)
                    {
                        string retornows;
                        switch (matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID)
                        {
                            case (int)enumFornecedor.MoodleSebrae:
                                moodle.IntegracaoSoapClient soapCliente = new moodle.IntegracaoSoapClient();
                                retornows = soapCliente.MatricularAluno(userSelected.Nome, userSelected.CPF, userSelected.Email, userSelected.Cidade, matriculaOferta.Oferta.SolucaoEducacional.IDChaveExterna.ToString(), matriculaOferta.MatriculaTurma.FirstOrDefault().Turma.IDChaveExterna.ToString());
                                break;
                            case (int)enumFornecedor.WebAula:
                                Turma turma = matriculaOferta.MatriculaTurma.FirstOrDefault().Turma;
                                webaula.waIntegracaoSoapClient wa = new webaula.waIntegracaoSoapClient();
                                webaula.AuthenticationProviderRequest aut = new webaula.AuthenticationProviderRequest();
                                webaula.DTOUsuario dtoUsuario = new webaula.DTOUsuario();
                                webaula.DTOTurma dtoTurma = new webaula.DTOTurma();
                                dtoTurma.IDChaveExterna = turma.IDChaveExterna;
                                dtoUsuario.CPF = userSelected.CPF;
                                dtoUsuario.Email = userSelected.Email;
                                dtoUsuario.Nome = userSelected.Nome;
                                dtoUsuario.Sexo = userSelected.Sexo;
                                dtoUsuario.UF = userSelected.UF.Sigla;
                                aut.Login = matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.Login;
                                aut.Senha = CriptografiaHelper.Decriptografar(matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.Senha);
                                retornows = wa.Matricular(aut, dtoUsuario, dtoTurma);
                                break;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErro(ex);
                }
                return;

            }

            //VALIDANDO A OFETA NORMAL
            if (oferta.TipoOferta.Equals(enumTipoOferta.Normal))
            {

                int qtdInscritosNaOferta = oferta.ListaMatriculaOferta.Where(x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                                                    x.StatusMatricula != enumStatusMatricula.CanceladoAluno)).Count();
                MatriculaOferta matriculaOferta = new MatriculaOferta()
                {
                    Oferta = new BMOferta().ObterPorId(oferta.ID),
                    Usuario = new BMUsuario().ObterPorId(userSelected.ID),
                    Auditoria = new Auditoria(autenticacao.Login),
                    DataSolicitacao = DateTime.Now,
                    UF = new BMUf().ObterPorId(userSelected.UF.ID),
                    NivelOcupacional = new BMNivelOcupacional().ObterPorID(userSelected.NivelOcupacional.ID)
                };

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


                BMMatriculaOferta bmMatriculaOferta = new BMMatriculaOferta();
                bmMatriculaOferta.Salvar(matriculaOferta);
                qtdInscritosNaOferta++;

                //validando se a turma já está chegando ao limite.
                if (qtdInscritosNaOferta > (oferta.QuantidadeMaximaInscricoes - int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta).Registro)))
                {
                    EnviarEmailLimiteOferta(oferta, matriculaOferta);
                }


                ValidarMetaIndividual(idUsuario, idSolucaoEducacional, pListaIdMetaIndividualAssociada, autenticacao);
                ValidarMetaInstitucional(idUsuario, idSolucaoEducacional, pListaIdMetaInstitucionalAssociada, autenticacao);
                return;
            }
            throw new Exception("Não foi encontrado ofertas para esse usuário!");

        }

        private static void EnviarEmailLimiteOferta(Oferta oferta, MatriculaOferta matriculaOferta)
        {
            try
            {
                Template template = TemplateUtil.ObterInformacoes(enumTemplate.NotificacaoLimiteOferta);

                string assuntoDoEmail = template.DescricaoTemplate.Substring(0, template.DescricaoTemplate.IndexOf(Environment.NewLine));

                string textoEmail = template.TextoTemplate;

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

                MetaIndividual mi = null;
                if (pListaIdMetaIndividualAssociada != null && pListaIdMetaIndividualAssociada.Count > 0)
                {

                    foreach (int IdMetaIndividualAssociada in pListaIdMetaIndividualAssociada)
                    {

                        using (BMMetaIndividual miBM = new BMMetaIndividual())
                            mi = miBM.ObterPorID(IdMetaIndividualAssociada);


                        if (!mi.ListaItensMetaIndividual.Any(x => x.SolucaoEducacional.ID == pSolucaoEducacional))
                        {
                            mi.ListaItensMetaIndividual.Add(new ItemMetaIndividual()
                            {
                                Auditoria = new Auditoria(autenticacao.Login),
                                MetaIndividual = new BMMetaIndividual().ObterPorID(mi.ID),
                                SolucaoEducacional = new BMSolucaoEducacional().ObterPorId(pSolucaoEducacional),
                            });

                            using (BMMetaIndividual miBM = new BMMetaIndividual())
                                miBM.Salvar(mi);
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
                                DataValidade = mi.DataValidade,
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


                        if (!mi.ListaItensMetaInstitucional.Any(x => x.Usuario.ID == pUsuario && x.SolucaoEducacional.ID == pSolucaoEducacional))
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

        public void ManterExternoSolucaoEducacional(DTOSolucaoEducacional pDTOSolucaoEducacional, AuthenticationProviderRequest pAutenticacao)
        {

            try
            {
                this.RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login, Senha = pAutenticacao.Senha }).FirstOrDefault(),
                                                               "CadastrarSolucaoEducacional");

                SolucaoEducacional solucaoEducacional = new BMSolucaoEducacional().ObterPorIDFornecedorEIdChaveExterna(pAutenticacao.Login, pDTOSolucaoEducacional.IDChaveExterna);

                if (solucaoEducacional == null)
                {
                    //CRIACAO
                    ValidarCamposSolucaoEducacional(pDTOSolucaoEducacional);

                    solucaoEducacional = PreencherObjetoSolucaoEducacional(pDTOSolucaoEducacional, pAutenticacao, solucaoEducacional);
                    solucaoEducacional.DataCadastro = DateTime.Now;
                    if (solucaoEducacional.FormaAquisicao == null)
                        throw new AcademicoException("A forma de aquisição não foi encontrada");
                    if (solucaoEducacional.CategoriaConteudo == null)
                        throw new AcademicoException("A categoria não foi encontrada");

                    new BMSolucaoEducacional().Salvar(solucaoEducacional);

                }
                else
                {
                    ValidarCamposSolucaoEducacional(pDTOSolucaoEducacional);
                    solucaoEducacional = PreencherObjetoSolucaoEducacional(pDTOSolucaoEducacional, pAutenticacao, solucaoEducacional);
                    new BMSolucaoEducacional().Salvar(solucaoEducacional);
                }


            }

            catch (AcademicoException ex)
            {
                throw new AcademicoException(ex.Message);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private static void ValidarCamposSolucaoEducacional(DTOSolucaoEducacional pDTOSolucaoEducacional)
        {
            if (!(pDTOSolucaoEducacional.IdFormaAquisicao > 0))
                throw new AcademicoException("A forma de aquisição é obrigatória");
            if (!(pDTOSolucaoEducacional.IdCategoriaSolucaoEducacional > 0))
                throw new AcademicoException("A categoria é obrigatória");
            if (string.IsNullOrWhiteSpace(pDTOSolucaoEducacional.IDChaveExterna))
                throw new AcademicoException("A chave externa é obrigatória");
            if (string.IsNullOrWhiteSpace(pDTOSolucaoEducacional.Nome))
                throw new AcademicoException("O nome da solução é obrigatório");
        }

        private static SolucaoEducacional PreencherObjetoSolucaoEducacional(DTOSolucaoEducacional pDTOSolucaoEducacional, AuthenticationProviderRequest pAutenticacao, SolucaoEducacional solucaoEducacional)
        {
            solucaoEducacional = new SolucaoEducacional()
            {
                Apresentacao = pDTOSolucaoEducacional.Apresentacao,
                Autor = pDTOSolucaoEducacional.Autor,
                CategoriaConteudo = pDTOSolucaoEducacional.IdCategoriaSolucaoEducacional > 0 ? (new BMCategoriaConteudo()).ObterPorID(2) : null,
                Ementa = pDTOSolucaoEducacional.Ementa,
                FormaAquisicao = (new BMFormaAquisicao()).ObterPorID(pDTOSolucaoEducacional.IdFormaAquisicao),
                Fornecedor = (new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login }).FirstOrDefault(),
                Nome = pDTOSolucaoEducacional.Nome,
                Objetivo = pDTOSolucaoEducacional.Objetivo,
                Ativo = pDTOSolucaoEducacional.Ativo,
                TemMaterial = pDTOSolucaoEducacional.TemMaterial,
                IDChaveExterna = pDTOSolucaoEducacional.IDChaveExterna,
                Auditoria = new Auditoria(pAutenticacao.Login),
            };
            return solucaoEducacional;
        }

        public List<DTOSolucaoEducacional> ConsultarSolucaoEducacionalPorFornecedor(string idChaveExterna, AuthenticationProviderRequest autenticacao)
        {
            solucaoEducacionalBM = new BMSolucaoEducacional();

            SolucaoEducacional solucaoFiltro = new SolucaoEducacional();
            solucaoFiltro.Fornecedor = new BMFornecedor().ObterPorLogin(autenticacao.Login);
            solucaoFiltro.IDChaveExterna = idChaveExterna;

            var listaSolucao = solucaoEducacionalBM.ObterPorFiltro(solucaoFiltro);

            List<DTOSolucaoEducacional> listaRetorno = new List<DTOSolucaoEducacional>();
            foreach (var registro in listaSolucao)
            {
                DTOSolucaoEducacional dtoRegistro = new DTOSolucaoEducacional();
                dtoRegistro.Apresentacao = registro.Apresentacao;
                dtoRegistro.Ativo = registro.Ativo;
                dtoRegistro.Autor = registro.Autor;
                dtoRegistro.Ementa = registro.Ementa;
                dtoRegistro.ID = registro.ID;
                dtoRegistro.IdCategoriaSolucaoEducacional = registro.CategoriaConteudo.ID;
                dtoRegistro.IDChaveExterna = registro.IDChaveExterna;
                dtoRegistro.IdFormaAquisicao = registro.FormaAquisicao.ID;
                dtoRegistro.Nome = registro.Nome;
                dtoRegistro.Objetivo = registro.Objetivo;
                dtoRegistro.TemMaterial = registro.TemMaterial;


                listaRetorno.Add(dtoRegistro);
            }
            return listaRetorno;
        }

        public string FormatarTextoEmail(IDictionary<string, string> registros, string textoEmail)
        {
            string textoEmailFormatado = textoEmail;

            foreach (string chave in registros.Keys)
            {
                textoEmailFormatado = textoEmailFormatado.Replace("#" + chave + "#", registros[chave]);
            }

            return textoEmailFormatado;
        }
    }
}

