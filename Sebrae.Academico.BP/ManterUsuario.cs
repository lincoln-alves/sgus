using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Web.Services3.Security.Tokens;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.wsSEBRAE;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BP
{

    public class ManterUsuario : RepositorioBase<Usuario>, IDisposable
    {

        private BMUsuario bmUsuario { get; set; }

        private RepositorioBase<Usuario> repositorio;

        public ManterUsuario()
        {
            bmUsuario = new BMUsuario();
            repositorio = new RepositorioBase<Usuario>();
        }

        #region "Código do antigo UsuarioFacade"

        #region "Configurações para Acesso ao WebService da Totvus"

        /// <summary>
        /// Configurações para acesso ao WebService de carga.
        /// </summary>
        private UsernameToken tokenParaAcessoAoWebServiceDeCarga = new UsernameToken("webservice.sgtc", "W0BzZ3RjMjAxMkBd", PasswordOption.SendPlainText);
        private wsSGTC webServiceSebrae = null;

        #endregion

        public bool PerfilGestor()
        {
            return bmUsuario.PerfilGestor();
        }

        public ManterUsuario(bool iniciarConfiguracoesParaImportacao = false)
            : base()
        {
            //if (iniciarConfiguracoesParaImportacao)
            //{
            //    //Cria uma instância do serviço
            //    this.webServiceSebrae = new wsSEBRAE.wsSGTC();
            //    this.webServiceSebrae.RequestSoapContext.Security.Tokens.Add(this.tokenParaAcessoAoWebServiceDeCarga);
            //    this.webServiceSebrae.RequestSoapContext.Security.Elements.Add(new MessageSignature(this.tokenParaAcessoAoWebServiceDeCarga));
            //    this.webServiceSebrae.RequestSoapContext.Security.Timestamp.TtlInSeconds = 300;
            //}
        }

        //


        #endregion

        public IList<DTOUsuarioPorFiltro> ConsultarUsuarioPorFiltro(int pPerfil, int pNivelOcupacional, int pUF)
        {
            var lstParam = new Dictionary<string, object>
            {
                {"pPerfil", pPerfil == 0 ? (int?)null: pPerfil },
                {"pNivelOcupacional", pNivelOcupacional == 0 ? (int?)null : pNivelOcupacional },
                {"pUF", pUF == 0 ? (int?)null : pUF}
            };

            return bmUsuario.ExecutarProcedure<DTOUsuarioPorFiltro>("SP_REL_QUANTIDADE_POR_PERFIL", lstParam);
        }

        public IList<DTORelatorioUsuarioPagante> ConsultarRelatorioUsuariosPagantes(string nome, string cpf, int? idUf, int? idNivelOcupacional, int? tipo)
        {
            var parametros = new Dictionary<string, object>
            {
                {"p_Nome", nome},
                {"p_CPF", cpf},
                {"p_UF", idUf},
                {"p_NivelOcupacional", idNivelOcupacional},
                {"p_tipo", tipo}
            };


            return bmUsuario.ExecutarProcedure<DTORelatorioUsuarioPagante>("SP_REL_USUARIOS_PAGANTES", parametros).OrderBy(c => c.Nome).ToList();
        }

        public IList<DTORelatorioDadosPessoais> ConsultarDadosPessoais(string nome, string cpf, List<int> nivelOcupacionalIds, List<int> ufIds, List<int> perfilIds)
        {
            var lstParam = new Dictionary<string, object>
            {
                {"p_Nome", nome},
                {"p_CPF", cpf}   
            };

            if (ufIds != null && ufIds.Any())
                lstParam.Add("P_UF", string.Join(", ", ufIds));
            else
                lstParam.Add("P_UF", DBNull.Value);

            if (nivelOcupacionalIds != null && nivelOcupacionalIds.Any())
                lstParam.Add("P_Nivel_Ocupacional", string.Join(", ", nivelOcupacionalIds));
            else
                lstParam.Add("P_Nivel_Ocupacional", DBNull.Value);

            if (perfilIds != null && perfilIds.Any())
                lstParam.Add("P_Perfil", string.Join(", ", perfilIds));
            else
                lstParam.Add("P_Perfil", DBNull.Value);            

            return bmUsuario.ExecutarProcedure<DTORelatorioDadosPessoais>("SP_REL_DADOS_PESSOAIS", lstParam);
        }

        public void ProcessarRecuperacaoSenhaComConfirmacao(string cpf)
        {
            try
            {

                Usuario usuario = new BMUsuario().ObterPorCPF(cpf);

                if (usuario != null)
                {
                    BMSolicitacaoSenha BmSolicitacaoSenha = new BMSolicitacaoSenha();
                    SolicitacaoSenha solicitacaoSenha = ObterObjetoSolicitacaoSenha(usuario);

                    //Insere um registro na tabela solicitacaosenha
                    BmSolicitacaoSenha.Salvar(solicitacaoSenha);

                    ConfiguracaoSistema configuracaoSistema = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);

                    string urlServidor = configuracaoSistema.Registro;

                    Template mensagemRecuperacaoDeSenhaComConfirmacao = TemplateUtil.ObterInformacoes(enumTemplate.RecuperacaoSenhaComConfirmacao);

                    //Envia e-mail para o usuário com o token
                    EmailUtil.Instancia.EnviarEmail(solicitacaoSenha.Usuario.Email.Trim(),
                                                      "Recuperação de senha",
                                                      CommonHelper.FormatarTextoRecuperacaoSenhaComConfirmacao(solicitacaoSenha, urlServidor,
                                                      mensagemRecuperacaoDeSenhaComConfirmacao.TextoTemplate));
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }


        public static void ProcessarRecuperacaoSenhaSemConfirmacao(string cpf, string login)
        {
            try
            {

                Usuario usuario = new BMUsuario().ObterPorCPF(cpf);

                if (usuario != null)
                {

                    string senha = string.Empty;
                    if (string.IsNullOrEmpty(usuario.Senha))
                    {
                        //CRIAR SENHA
                        senha = WebFormHelper.ObterSenhaAleatoria();
                        usuario.Senha = CriptografiaHelper.Criptografar(senha);
                        usuario.Auditoria = new Auditoria(login);
                        BMUsuario usuarioBM = new BMUsuario();
                        usuarioBM.Salvar(usuario);
                    }
                    else
                    {
                        //ENVIAR SENHA ESQUECIDA
                        senha = CriptografiaHelper.Decriptografar(usuario.Senha);
                    }

                    Template mensagemRecuperacaoDeSenhaSemConfirmacao = TemplateUtil.ObterInformacoes(enumTemplate.RecuperacaoSenhaSemConfirmacao);

                    ConfiguracaoSistema configuracaoSistema = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SMTPServer);

                    string urlServidor = configuracaoSistema.Registro;


                    //Envia e-mail para o usuário com a senha
                    EmailUtil.Instancia.EnviarEmail(usuario.Email.Trim(),
                                                      "Recuperação de senha",
                                                      CommonHelper.FormatarTextoRecuperacaoSenhaSemConfirmacao(usuario.Nome, usuario.Email, senha,
                                                      configuracaoSistema.Registro,
                                                      mensagemRecuperacaoDeSenhaSemConfirmacao.TextoTemplate));

                    //BMSolicitacaoSenha BmSolicitacaoSenha = new BMSolicitacaoSenha();
                    //SolicitacaoSenha solicitacaoSenha = ObterObjetoSolicitacaoSenha(usuario);

                    ////Insere um registro na tabela solicitacaosenha
                    //BmSolicitacaoSenha.Salvar(solicitacaoSenha);

                    //ConfiguracaoSistema configuracaoSistema = ConfiguracaoSistemaFacade.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);

                    //string urlServidor = configuracaoSistema.Registro;

                    //Template templateMensagemRecuperacaoDeSenha = TemplateFacade.ObterInformacoes(enumTemplate.TemplateRecuperacaoSenha);

                    ////Envia e-mail para o usuário com o token
                    //EmailFacade.Instancia.EnviarEmail(solicitacaoSenha.Usuario.Email.Trim(),
                    //                                  "Recuperação de senha",
                    //                                  CommonHelper.FormatarTextoRecuperacaoSenha(solicitacaoSenha, urlServidor,
                    //                                  templateMensagemRecuperacaoDeSenha.TextoTemplate));



                }
                else
                {
                    throw new AcademicoException("CPF não localizado");
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private static SolicitacaoSenha ObterObjetoSolicitacaoSenha(Usuario usuario)
        {
            SolicitacaoSenha solicitacaoSenha = new SolicitacaoSenha()
            {
                Usuario = usuario,
                Auditoria = new Auditoria(null),
                DataValidade = DateTime.Now.AddDays(2) // D+2
            };

            //Gerar o Token
            solicitacaoSenha.Token = WebFormHelper.ObterStringAleatoria();

            return solicitacaoSenha;
        }

        public string CadastrarImagem(int idUsuario, string imagem, string login)
        {
            BMUsuario usuarioBm = new BMUsuario();
            Usuario usuario = usuarioBm.ValidarInformacoesParaSalvarImagem(idUsuario, imagem);
            MemoryStream memoryStream = CommonHelper.ObterMemoryStream(imagem);

            usuario.FileServer = CommonHelper.ObterObjetoFileServer(memoryStream);

            //Define o tipo de arquivo (/Quebra a string para obter o tipo do arquivo. Ex: bmp, jpeg, etc...)
            usuario.FileServer.TipoArquivo = CommonHelper.ObterTipoDoArquivo(imagem);
            usuario.FileServer.MediaServer = false;
            usuario.FileServer.NomeDoArquivoOriginal = "ImagemUsuario.jpg";
            usuario.FileServer.Auditoria = new Auditoria(login);
            usuario.Auditoria = new Auditoria(login);

            //Salva a imagem no disco
            ConfiguracaoSistema caminhoParaDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload);
            this.SalvarImagemNoDisco(memoryStream, caminhoParaDiretorioDeUpload.Registro, usuario.FileServer.NomeDoArquivoNoServidor);

            this.SalvarImagemNoBanco(imagem, usuarioBm, usuario, caminhoParaDiretorioDeUpload);

            //string linkParaImagem = CommonHelper.ObterLinkParaArquivoDeImagem(caminhoParaDiretorioDeUpload.Registro, usuario.FileServer.ID);

            string linkParaImagem = this.ObterLinkParaImagem(usuario);

            //Result.LinkImagem = string.Format(String.Concat(configuracaoSistema.Registro, "/MediaServer.ashx?Identificador={0}"), us.FileServer.ID);

            return linkParaImagem;
        }

        public string ObterLinkParaImagem(Usuario usuario)
        {
            ConfiguracaoSistema caminhoParaDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);
            string linkParaImagem = string.Empty;

            if (usuario.FileServer != null)
            {
                linkParaImagem = CommonHelper.ObterLinkParaArquivoDeImagem(caminhoParaDiretorioDeUpload.Registro, usuario.FileServer.NomeDoArquivoNoServidor);
            }

            return linkParaImagem;
        }

        private void SalvarImagemNoBanco(string imagem, BMUsuario usuarioBm, Usuario usuario, ConfiguracaoSistema caminhoParaDiretorioDeUpload)
        {
            usuario.FileServer.Auditoria = new Auditoria(usuario.LoginLms);
            //Salva a imagem no Banco
            usuarioBm.Salvar(usuario);
        }

        private void SalvarImagemNoDisco(MemoryStream memoryStream, string diretorioParaSalvarArquivo, string nomeArquivo)
        {
            try
            {

                this.ValidarInformacoesParaSalvarAruivo(memoryStream, diretorioParaSalvarArquivo);

                string caminhoCompletoComNomeDoArquivo = string.Concat(diretorioParaSalvarArquivo, "\\", nomeArquivo);

                //Salva a imagem no disco
                using (FileStream file = new FileStream(caminhoCompletoComNomeDoArquivo, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(file);
                    file.Close();
                };

            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        private void ValidarInformacoesParaSalvarAruivo(MemoryStream memoryStream, string diretorioParaSalvarArquivo)
        {
            if (memoryStream == null)
            {
                throw new AcademicoException("Arquivo não informado. Informe um arquivo.");
            }

            if (string.IsNullOrWhiteSpace(diretorioParaSalvarArquivo))
            {
                throw new AcademicoException("Diretório para salvar o arquivo não encontrado.");
            }
        }

        #region "Encapsula acessos a outras funcionalidades"


        private IList<Uf> ListaUfs = null;


        public void ImportarALI(string cpf)
        {
            ZALI[] listaZALI = null;

            if (string.IsNullOrEmpty(cpf))
            {
                if (!(ListaUfs != null && ListaUfs.Count > 0))
                    this.ListaUfs = new BMUf().ObterTodos();

                foreach (Uf uf in ListaUfs)
                {
                    try
                    {
                        this.webServiceSebrae.Timeout = 36000;
                        listaZALI = this.webServiceSebrae.RetornaParticipantesZALIList(1, string.Format("ZALI.UF = '{0}' ", uf.Sigla));
                        if (listaZALI != null && listaZALI.Length > 0)
                        {
                            this.InserirOuAtualizarALI(listaZALI, uf.ID);

                        }
                    }
                    catch (Exception ex)
                    {
                        ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarALI do webservice wsSGTC. Erro Original: ", ex.Message));
                    }
                }
            }
            else
            {
                try
                {
                    this.webServiceSebrae.Timeout = 36000;
                    listaZALI = this.webServiceSebrae.RetornaParticipantesZALIList(1, string.Format("ZALI.CPF = '{0}' ", cpf));
                    if (listaZALI != null && listaZALI.Length > 0)
                    {
                        this.InserirOuAtualizarALI(listaZALI, 0);

                    }
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarALI do webservice wsSGTC. Erro Original: ", ex.Message));
                }
            }
        }

        public void ImportarADL(string cpf)
        {
            ZADL[] listaZADL = null;

            if (string.IsNullOrEmpty(cpf))
            {
                if (!(ListaUfs != null && ListaUfs.Count > 0))
                    this.ListaUfs = new BMUf().ObterTodos();

                foreach (Uf uf in ListaUfs)
                {
                    try
                    {
                        this.webServiceSebrae.Timeout = 36000;
                        listaZADL = this.webServiceSebrae.RetornaParticipantesZADLList(1, string.Format("ZADL.uf ='{0}'", uf.Sigla));
                        if (listaZADL != null && listaZADL.Length > 0)
                        {
                            this.InserirOuAtualizarADL(listaZADL, uf.ID);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarADL do webservice wsSGTC. Erro Original: ", ex.Message));
                    }
                }
            }
            else
            {
                try
                {
                    listaZADL = this.webServiceSebrae.RetornaParticipantesZADLList(1, string.Format("ZADL.CPF ='{0}'", cpf));
                    if (listaZADL != null && listaZADL.Length > 0)
                    {
                        this.InserirOuAtualizarADL(listaZADL, 0);
                    }
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarADL do webservice wsSGTC. Erro Original: ", ex.Message));
                }
            }
        }

        public void ImportarColaborador(string cpf)
        {
            ZPFUNC[] listaZPFUNC = null;
            if (string.IsNullOrEmpty(cpf))
            {
                if (!(ListaUfs != null && ListaUfs.Count > 0))
                    this.ListaUfs = new BMUf().ObterTodos();

                foreach (Uf uf in ListaUfs)
                {
                    try
                    {
                        this.webServiceSebrae.Timeout = 36000;
                        listaZPFUNC = this.webServiceSebrae.RetornaFuncionariosZPFUNCList(1, string.Format("ZPFUNC.UF = '{0}'", uf.Sigla));
                        if (listaZPFUNC != null && listaZPFUNC.Length > 0)
                        {
                            this.InserirOuAtualizarColaborador(listaZPFUNC, uf.ID);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarColaborador do webservice wsSGTC. Erro Original: ", ex.Message));
                    }
                }
            }
            else
            {
                try
                {
                    this.webServiceSebrae.Timeout = 36000;
                    listaZPFUNC = this.webServiceSebrae.RetornaFuncionariosZPFUNCList(1, string.Format("ZPFUNC.CPF = '{0}'", cpf));
                    if (listaZPFUNC != null && listaZPFUNC.Length > 0)
                    {
                        this.InserirOuAtualizarColaborador(listaZPFUNC, 0);
                    }
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarColaborador do webservice wsSGTC. Erro Original: ", ex.Message));
                }
            }
        }

        public void ImportarCredenciado(string cpf)
        {
            ZCREDENCIADOS[] listaZCREDENCIADOS = null;

            if (string.IsNullOrEmpty(cpf))
            {
                if (!(ListaUfs != null && ListaUfs.Count > 0))
                    this.ListaUfs = new BMUf().ObterTodos();

                foreach (Uf uf in ListaUfs)
                {
                    try
                    {
                        this.webServiceSebrae.Timeout = 36000;
                        listaZCREDENCIADOS = this.webServiceSebrae.RetornaParticipantesZCREDENCIADOSList(1, string.Format("ZCREDENCIADOS.UF= '{0}'", uf.Sigla));
                        if (listaZCREDENCIADOS != null && listaZCREDENCIADOS.Length > 0)
                        {
                            this.InserirOuAtualizarCredenciado(listaZCREDENCIADOS, uf.ID);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarCredenciado do webservice wsSGTC. Erro Original: ", ex.Message));
                    }
                }
            }
            else
            {
                try
                {
                    this.webServiceSebrae.Timeout = 36000;
                    listaZCREDENCIADOS = this.webServiceSebrae.RetornaParticipantesZCREDENCIADOSList(1, string.Format("ZCREDENCIADOS.CPF= '{0}'", cpf));
                    if (listaZCREDENCIADOS != null && listaZCREDENCIADOS.Length > 0)
                    {
                        this.InserirOuAtualizarCredenciado(listaZCREDENCIADOS, 0);
                    }
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErroeContuniar(ex, string.Concat("Erro ocorreu na chamada do método ImportarCredenciado do webservice wsSGTC. Erro Original: ", ex.Message));
                }

            }

        }


        private void InserirOuAtualizarALI(ZALI[] listaZALI, int idUF)
        {
            BMUsuario bmUsuario = new BMUsuario();
            Usuario aluno = null;

            List<Usuario> listaUsuario = new BMUsuario().ObterPorEstadoComPerfil(idUF).ToList();

            foreach (ZALI zali in listaZALI)
            {
                TratarXMLRecebidoAli(zali);
                if (string.IsNullOrEmpty(zali.CPF))
                    continue; //Usuario sem CPF ignorar

                if (zali.EMAIL.Length >= 50)
                    continue; //Usuario com email maior que o permitido, ignorar

                zali.CPF = zali.CPF.Replace(".", "").Replace("-", "");

                if (listaUsuario != null && listaUsuario.Count > 0)
                    aluno = listaUsuario.Where(x => x.CPF == zali.CPF).FirstOrDefault();

                if (aluno == null)
                    aluno = new BMUsuario().ObterPorCPF(zali.CPF); //VOLTAR NO BANCO PARA PEGAR CASOS DE MUDANCA DE UF
                if (aluno != null)
                {
                    bool usuarioPossuiAlgumaInformacaoNova = this.ObterDadosALI(zali, aluno);

                    if (usuarioPossuiAlgumaInformacaoNova)
                    {
                        bmUsuario.SalvarSemValidacao(aluno);
                    }
                }
                else
                {
                    aluno = new Usuario();
                    this.ObterDadosALI(zali, aluno);
                    bmUsuario.SalvarSemValidacao(aluno);
                }
            }
            bmUsuario.Commit();
        }

        private void InserirOuAtualizarColaborador(ZPFUNC[] listaZPFUNC, int idUF)
        {
            BMUsuario bmUsuario = new BMUsuario();
            Usuario aluno = null;

            List<Usuario> listaUsuario = new BMUsuario().ObterPorEstadoComPerfil(idUF).ToList();

            foreach (ZPFUNC zpFunc in listaZPFUNC)
            {
                TratarXMLRecebidoColaborador(zpFunc);

                if (string.IsNullOrEmpty(zpFunc.CPF))
                    continue; //Usuario sem CPF ignorar

                if (zpFunc.EMAIL.Length >= 50)
                    continue; //Usuario com email maior que o permitido, ignorar

                zpFunc.CPF = zpFunc.CPF.Replace(".", "").Replace("-", "");

                if (listaUsuario != null && listaUsuario.Count > 0)
                    aluno = listaUsuario.Where(x => x.CPF == zpFunc.CPF).FirstOrDefault();

                if (aluno == null)
                    aluno = new BMUsuario().ObterPorCPF(zpFunc.CPF); //VOLTAR NO BANCO PARA PEGAR CASOS DE MUDANCA DE UF

                if (aluno != null)
                {
                    bool usuarioPossuiAlgumaInformacaoNova = this.ObterDadosColaborador(zpFunc, aluno);

                    if (usuarioPossuiAlgumaInformacaoNova)
                    {
                        if (aluno.UF.ID > 0 && aluno.NivelOcupacional.ID > 0)
                        {
                            bmUsuario.SalvarSemValidacao(aluno);
                        }
                    }
                }
                else
                {
                    aluno = new Usuario();
                    this.ObterDadosColaborador(zpFunc, aluno);
                    if (aluno.UF.ID > 0 && aluno.NivelOcupacional.ID > 0)
                    {
                        bmUsuario.SalvarSemValidacao(aluno);
                    }
                }

            }
            bmUsuario.Commit();

        }


        private void InserirOuAtualizarCredenciado(ZCREDENCIADOS[] listaZCREDENCIADOS, int idUF)
        {
            BMUsuario bmUsuario = new BMUsuario();
            Usuario aluno = null;

            List<Usuario> listaUsuario = new BMUsuario().ObterPorEstadoComPerfil(idUF).ToList();

            foreach (ZCREDENCIADOS credenciado in listaZCREDENCIADOS)
            {

                TratarXMLRecebidoCredenciado(credenciado);

                if (string.IsNullOrEmpty(credenciado.CPF))
                    continue; //Usuario sem CPF ignorar

                if (credenciado.EMAIL.Length >= 50)
                    continue; //Usuario com email maior que o permitido, ignorar

                credenciado.CPF = credenciado.CPF.Replace(".", "").Replace("-", "");

                if (listaUsuario != null && listaUsuario.Count > 0)
                    aluno = listaUsuario.Where(x => x.CPF == credenciado.CPF).FirstOrDefault();

                if (aluno == null)
                    aluno = new BMUsuario().ObterPorCPF(credenciado.CPF); //VOLTAR NO BANCO PARA PEGAR CASOS DE MUDANCA DE UF

                if (aluno != null)
                {
                    bool usuarioPossuiAlgumaInformacaoNova = this.ObterDadosCredenciado(credenciado, aluno);

                    if (usuarioPossuiAlgumaInformacaoNova)
                    {
                        bmUsuario.SalvarSemValidacao(aluno);
                    }
                }
                else
                {
                    aluno = new Usuario();
                    this.ObterDadosCredenciado(credenciado, aluno);
                    bmUsuario.SalvarSemValidacao(aluno);
                }
            }
            bmUsuario.Commit();
        }

        private void InserirOuAtualizarADL(ZADL[] listaZADL, int idUF)
        {

            BMUsuario bmUsuario = new BMUsuario();
            Usuario aluno = null;


            List<Usuario> listaUsuario = new BMUsuario().ObterPorEstadoComPerfil(idUF).ToList();

            foreach (ZADL zadL in listaZADL)
            {

                if (zadL.EMAIL.Length >= 50)
                    continue; //Usuario com email maior que o permitido, ignorar 

                if (listaUsuario != null && listaUsuario.Count > 0)
                    aluno = listaUsuario.Where(x => x.CPF == zadL.CPF).FirstOrDefault();

                zadL.CPF = zadL.CPF.Replace(".", "").Replace("-", "");

                if (aluno == null)
                    aluno = new BMUsuario().ObterPorCPF(zadL.CPF); //VOLTAR NO BANCO PARA PEGAR CASOS DE MUDANCA DE UF

                if (aluno != null)
                {
                    bool usuarioPossuiAlgumaInformacaoNova = this.ObterDadosADL(zadL, aluno);

                    if (usuarioPossuiAlgumaInformacaoNova)
                    {
                        bmUsuario.SalvarSemValidacao(aluno);
                    }
                }
                else
                {
                    aluno = new Usuario();
                    this.ObterDadosADL(zadL, aluno);
                    bmUsuario.SalvarSemValidacao(aluno);
                }
            }
            bmUsuario.Commit();
        }


        private bool ObterDadosALI(ZALI usuarioWS, Usuario usuario)
        {
            int contDadosAlterados = 0;
            bool usuarioPossuiAlgumaInformacaoNova = false;

            if (usuario.DataAtualizacaoCarga.HasValue && usuarioWS.DATAALTERACAO < usuario.DataAtualizacaoCarga)
                return false;

            if (usuario.UF == null)
            {
                usuario.UF = new Uf();
                usuario.UF.Sigla = string.Empty;
            }
            if (usuario.UF.Sigla.ToUpper() != usuarioWS.UF.Trim().ToUpper())
            {
                contDadosAlterados++;
                usuario.UF = new Uf() { ID = (int)Enum.Parse(typeof(enumUF), usuarioWS.UF) };
            }
            if (string.IsNullOrEmpty(usuario.Nome)) usuario.Nome = string.Empty;
            if (usuario.Nome != usuarioWS.NOME)
            {
                contDadosAlterados++;
                usuario.Nome = usuarioWS.NOME;
            }

            if (string.IsNullOrEmpty(usuario.CPF))
            {
                contDadosAlterados++;
                usuario.CPF = usuarioWS.CPF;
            }

            if (string.IsNullOrEmpty(usuario.Email)) usuario.Email = string.Empty;
            if (usuario.Email != usuarioWS.EMAIL)
            {
                contDadosAlterados++;
                usuario.Email = usuarioWS.EMAIL;
            }

            if (string.IsNullOrEmpty(usuario.Situacao)) usuario.Situacao = string.Empty;
            if (usuario.Situacao != usuarioWS.DESCSITUACAO)
            {
                contDadosAlterados++;
                usuario.Situacao = usuarioWS.DESCSITUACAO;
            }

            if (usuario.NivelOcupacional == null)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = (int)enumNivelOcupacional.OrientadorALI };

            }
            else if (usuario.NivelOcupacional.ID != (int)enumNivelOcupacional.OrientadorALI)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = (int)enumNivelOcupacional.OrientadorALI };
            }

            usuario.DataAtualizacaoCarga = usuarioWS.DATAALTERACAO;

            if (usuario.ListaPerfil == null)
                usuario.ListaPerfil = new List<UsuarioPerfil>();

            if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Terceiro))
            {
                contDadosAlterados++;
                UsuarioPerfil usuarioPerfil = new UsuarioPerfil();
                usuarioPerfil.Usuario = usuario;
                usuarioPerfil.Perfil = new Perfil() { ID = (int)enumPerfil.Terceiro };
                usuario.ListaPerfil.Add(usuarioPerfil);
            }

            if (contDadosAlterados > 0)
            {
                usuarioPossuiAlgumaInformacaoNova = true;
                usuario.Auditoria = new Auditoria("Carga SGO");
            }

            return usuarioPossuiAlgumaInformacaoNova;
        }

        private bool ObterDadosCredenciado(ZCREDENCIADOS usuarioWS, Usuario usuario)
        {
            int contDadosAlterados = 0;
            bool usuarioPossuiAlgumaInformacaoNova = false;

            if (usuario.DataAtualizacaoCarga.HasValue && usuarioWS.DATAALTERACAO < usuario.DataAtualizacaoCarga)
                return false;

            if (usuario.UF == null)
            {
                usuario.UF = new Uf();
                usuario.UF.Sigla = string.Empty;
            }
            if (usuario.UF.Sigla != usuarioWS.UF)
            {
                contDadosAlterados++;
                usuario.UF = new Uf() { ID = (int)Enum.Parse(typeof(enumUF), usuarioWS.UF) };
            }

            if (string.IsNullOrEmpty(usuario.Nome)) usuario.Nome = string.Empty;
            if (usuario.Nome != usuarioWS.NOME)
            {
                contDadosAlterados++;
                usuario.Nome = usuarioWS.NOME;
            }

            if (string.IsNullOrEmpty(usuario.CPF))
            {
                contDadosAlterados++;
                usuario.CPF = usuarioWS.CPF;
            }

            if (string.IsNullOrEmpty(usuario.Email)) usuario.Email = string.Empty;
            if (!string.IsNullOrEmpty(usuarioWS.EMAIL) && usuario.Email != usuarioWS.EMAIL)
            {
                contDadosAlterados++;
                usuario.Email = usuarioWS.EMAIL;
            }

            if (string.IsNullOrEmpty(usuario.Situacao)) usuario.Situacao = string.Empty;
            if (!string.IsNullOrEmpty(usuarioWS.DESCSITUACAO) && usuario.Situacao != usuarioWS.DESCSITUACAO)
            {
                contDadosAlterados++;
                usuario.Situacao = usuarioWS.DESCSITUACAO;
            }

            if (usuario.NivelOcupacional == null)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = (int)enumNivelOcupacional.Credenciado };

            }
            else if (usuario.NivelOcupacional.ID != (int)enumNivelOcupacional.Credenciado)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = (int)enumNivelOcupacional.Credenciado };
            }

            usuario.DataAtualizacaoCarga = usuarioWS.DATAALTERACAO;

            if (usuario.ListaPerfil == null)
                usuario.ListaPerfil = new List<UsuarioPerfil>();

            if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Terceiro))
            {
                contDadosAlterados++;
                UsuarioPerfil usuarioPerfil = new UsuarioPerfil();
                usuarioPerfil.Usuario = usuario;
                usuarioPerfil.Perfil = new Perfil() { ID = (int)enumPerfil.Terceiro };
                usuario.ListaPerfil.Add(usuarioPerfil);
            }

            if (contDadosAlterados > 0)
            {
                usuario.Auditoria = new Auditoria("Carga SGO");
                usuarioPossuiAlgumaInformacaoNova = true;
            }

            return usuarioPossuiAlgumaInformacaoNova;
        }

        private bool ObterDadosADL(ZADL usuarioWS, Usuario usuario)
        {
            int contDadosAlterados = 0;
            bool usuarioPossuiAlgumaInformacaoNova = false;

            if (usuario.DataAtualizacaoCarga.HasValue && usuarioWS.DATAALTERACAO < usuario.DataAtualizacaoCarga)
                return false;

            if (usuario.UF == null)
            {
                usuario.UF = new Uf();
                usuario.UF.Sigla = string.Empty;
            }
            if (usuario.UF.Sigla.ToUpper() != usuarioWS.UF.Trim().ToUpper())
            {
                contDadosAlterados++;
                usuario.UF = new Uf() { ID = (int)Enum.Parse(typeof(enumUF), usuarioWS.UF) };
            }

            if (string.IsNullOrEmpty(usuario.Nome)) usuario.Nome = string.Empty;
            if (usuario.Nome != usuarioWS.NOME)
            {
                contDadosAlterados++;
                usuario.Nome = usuarioWS.NOME;
            }

            if (string.IsNullOrEmpty(usuario.CPF))
            {
                contDadosAlterados++;
                usuario.CPF = usuarioWS.CPF;
            }

            if (string.IsNullOrEmpty(usuario.Email)) usuario.Email = string.Empty;
            if (usuario.Email != usuarioWS.EMAIL)
            {
                contDadosAlterados++;
                usuario.Email = usuarioWS.EMAIL;
            }

            if (string.IsNullOrEmpty(usuario.Situacao)) usuario.Situacao = string.Empty;
            if (usuario.Situacao != usuarioWS.DESCSITUACAO)
            {
                contDadosAlterados++;
                usuario.Situacao = usuarioWS.DESCSITUACAO;
            }

            if (usuario.NivelOcupacional == null)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = (int)enumNivelOcupacional.ADL };

            }
            else if (usuario.NivelOcupacional.ID != (int)enumNivelOcupacional.ADL)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = (int)enumNivelOcupacional.ADL };
            }

            usuario.DataAtualizacaoCarga = usuarioWS.DATAALTERACAO;

            if (usuario.ListaPerfil == null)
                usuario.ListaPerfil = new List<UsuarioPerfil>();

            if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Terceiro))
            {
                contDadosAlterados++;
                UsuarioPerfil usuarioPerfil = new UsuarioPerfil();
                usuarioPerfil.Usuario = usuario;
                usuarioPerfil.Perfil = new Perfil() { ID = (int)enumPerfil.Terceiro };
                usuario.ListaPerfil.Add(usuarioPerfil);
            }

            if (contDadosAlterados > 0)
            {
                usuario.Auditoria = new Auditoria("Carga SGO");
                usuarioPossuiAlgumaInformacaoNova = true;
            }

            return usuarioPossuiAlgumaInformacaoNova;
        }


        private bool ObterDadosColaborador(ZPFUNC usuarioWS, Usuario usuario)
        {
            int contDadosAlterados = 0;
            bool usuarioPossuiAlgumaInformacaoNova = false;

            if (usuario.DataAtualizacaoCarga.HasValue && usuarioWS.DATAATUALIZACAO < usuario.DataAtualizacaoCarga)
                return false;

            if (usuario.UF == null)
            {
                usuario.UF = new Uf();
            }
            int idUf = (int)Enum.Parse(typeof(enumUF), usuarioWS.UF);
            if (usuario.UF.ID != idUf)
            {
                contDadosAlterados++;
                usuario.UF = new Uf() { ID = idUf };
            }

            if (string.IsNullOrEmpty(usuario.Nome)) usuario.Nome = string.Empty;
            if (usuario.Nome != usuarioWS.NOME)
            {
                contDadosAlterados++;
                usuario.Nome = usuarioWS.NOME;
            }

            if (usuario.Matricula != usuarioWS.CHAPA)
            {
                contDadosAlterados++;
                usuario.Matricula = usuarioWS.CHAPA;
            }

            if (usuario.Unidade != usuarioWS.UNIDADE)
            {
                contDadosAlterados++;
                usuario.Unidade = usuarioWS.UNIDADE;
            }

            if (string.IsNullOrEmpty(usuario.CPF))
            {
                contDadosAlterados++;
                usuario.CPF = usuarioWS.CPF;
            }

            if (string.IsNullOrEmpty(usuario.Email)) usuario.Email = string.Empty;
            if (usuario.Email != usuarioWS.EMAIL)
            {
                contDadosAlterados++;
                usuario.Email = usuarioWS.EMAIL;
                if (usuario.Email.Length >= 50)
                    usuario.Email = usuario.Email.Substring(0, 49);
            }

            if (string.IsNullOrEmpty(usuario.Situacao)) usuario.Situacao = string.Empty;
            if (usuario.Situacao != usuarioWS.DESC_SITUACAO)
            {
                contDadosAlterados++;
                usuario.Situacao = usuarioWS.DESC_SITUACAO;
            }

            if (usuario.NivelOcupacional == null)
            {
                usuario.NivelOcupacional = new NivelOcupacional();

            }

            int idNivelOcupacional = ObterNivelOcupacional(usuarioWS.NOMEFUNCAO);

            if (usuario.NivelOcupacional.ID != idNivelOcupacional && idNivelOcupacional > 0)
            {
                contDadosAlterados++;
                usuario.NivelOcupacional = new NivelOcupacional() { ID = idNivelOcupacional };
            }

            if (!usuarioWS.DATAADMINISSAO.Equals(DateTime.MinValue))
            {
                usuario.DataAdmissao = usuarioWS.DATAADMINISSAO;
            }

            if (!usuarioWS.DTNASCIMENTO.Equals(DateTime.MinValue))
            {
                usuario.DataNascimento = usuarioWS.DTNASCIMENTO;
            }

            usuario.DataAtualizacaoCarga = usuarioWS.DATAATUALIZACAO;

            if (usuario.ListaPerfil == null)
                usuario.ListaPerfil = new List<UsuarioPerfil>();

            if (usuario.NivelOcupacional.ID == (int)enumNivelOcupacional.Parceiro)
            {
                if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Terceiro))
                {
                    contDadosAlterados++;
                    UsuarioPerfil usuarioPerfil = new UsuarioPerfil();
                    usuarioPerfil.Usuario = usuario;
                    usuarioPerfil.Perfil = new Perfil() { ID = (int)enumPerfil.Terceiro };
                    usuario.ListaPerfil.Add(usuarioPerfil);
                }
            }
            else
            {
                if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Colaborador))
                {
                    contDadosAlterados++;
                    UsuarioPerfil usuarioPerfil = new UsuarioPerfil();
                    usuarioPerfil.Usuario = usuario;
                    usuarioPerfil.Perfil = new Perfil() { ID = (int)enumPerfil.Colaborador };
                    usuario.ListaPerfil.Add(usuarioPerfil);
                }
            }

            if (contDadosAlterados > 0)
            {
                usuario.Auditoria = new Auditoria("Carga SGO");
                usuarioPossuiAlgumaInformacaoNova = true;

            }

            return usuarioPossuiAlgumaInformacaoNova;
        }

        private static void TratarXMLRecebidoColaborador(ZPFUNC zpFunc)
        {
            if (string.IsNullOrEmpty(zpFunc.CPF))
                zpFunc.CPF = string.Empty;
            zpFunc.CPF = zpFunc.CPF.Replace(".", "").Replace("-", "").Trim();

            if (string.IsNullOrEmpty(zpFunc.UF))
                zpFunc.UF = string.Empty;
            zpFunc.UF = zpFunc.UF.Trim().ToUpper();

            if (string.IsNullOrEmpty(zpFunc.NOME))
                zpFunc.NOME = string.Empty;
            zpFunc.NOME = zpFunc.NOME.Trim().ToUpper();

            if (string.IsNullOrEmpty(zpFunc.CHAPA))
                zpFunc.CHAPA = string.Empty;
            zpFunc.CHAPA = zpFunc.CHAPA.Trim().ToUpper();

            if (string.IsNullOrEmpty(zpFunc.UNIDADE))
                zpFunc.UNIDADE = string.Empty;
            zpFunc.UNIDADE = zpFunc.UNIDADE.Trim().ToUpper();

            if (string.IsNullOrEmpty(zpFunc.EMAIL))
                zpFunc.EMAIL = string.Empty;
            zpFunc.EMAIL = zpFunc.EMAIL.Trim().ToLower();

            if (string.IsNullOrEmpty(zpFunc.DESC_SITUACAO))
                zpFunc.DESC_SITUACAO = string.Empty;
            zpFunc.DESC_SITUACAO = zpFunc.DESC_SITUACAO.Trim().ToLower();

            if (string.IsNullOrEmpty(zpFunc.NOMEFUNCAO))
                zpFunc.NOMEFUNCAO = string.Empty;
            zpFunc.NOMEFUNCAO = zpFunc.NOMEFUNCAO.Trim().ToUpper();

        }

        private static void TratarXMLRecebidoAli(ZALI registro)
        {
            if (string.IsNullOrEmpty(registro.CPF))
                registro.CPF = string.Empty;
            registro.CPF = registro.CPF.Replace(".", "").Replace("-", "").Trim();

            if (string.IsNullOrEmpty(registro.UF))
                registro.UF = string.Empty;
            registro.UF = registro.UF.Trim().ToUpper();

            if (string.IsNullOrEmpty(registro.NOME))
                registro.NOME = string.Empty;
            registro.NOME = registro.NOME.Trim().ToUpper();

            if (string.IsNullOrEmpty(registro.EMAIL))
                registro.EMAIL = string.Empty;
            registro.EMAIL = registro.EMAIL.Trim().ToLower();

            if (string.IsNullOrEmpty(registro.DESCSITUACAO))
                registro.DESCSITUACAO = string.Empty;
            registro.DESCSITUACAO = registro.DESCSITUACAO.Trim().ToLower();

        }

        private static void TratarXMLRecebidoCredenciado(ZCREDENCIADOS registro)
        {
            if (string.IsNullOrEmpty(registro.CPF))
                registro.CPF = string.Empty;
            registro.CPF = registro.CPF.Replace(".", "").Replace("-", "").Trim();

            if (string.IsNullOrEmpty(registro.UF))
                registro.UF = string.Empty;
            registro.UF = registro.UF.Trim().ToUpper();

            if (string.IsNullOrEmpty(registro.NOME))
                registro.NOME = string.Empty;
            registro.NOME = registro.NOME.Trim().ToUpper();

            if (string.IsNullOrEmpty(registro.EMAIL))
                registro.EMAIL = string.Empty;
            registro.EMAIL = registro.EMAIL.Trim().ToLower();

            if (string.IsNullOrEmpty(registro.DESCSITUACAO))
                registro.DESCSITUACAO = string.Empty;
            registro.DESCSITUACAO = registro.DESCSITUACAO.Trim().ToLower();

        }

        private static void TratarXMLRecebidoAdl(ZADL registro)
        {
            if (string.IsNullOrEmpty(registro.CPF))
                registro.CPF = string.Empty;
            registro.CPF = registro.CPF.Replace(".", "").Replace("-", "").Trim();

            if (string.IsNullOrEmpty(registro.UF))
                registro.UF = string.Empty;
            registro.UF = registro.UF.Trim().ToUpper();

            if (string.IsNullOrEmpty(registro.NOME))
                registro.NOME = string.Empty;
            registro.EMAIL = registro.EMAIL.Trim().ToUpper();


            if (string.IsNullOrEmpty(registro.EMAIL))
                registro.EMAIL = string.Empty;
            registro.EMAIL = registro.EMAIL.Trim().ToLower();

            if (string.IsNullOrEmpty(registro.DESCSITUACAO))
                registro.DESCSITUACAO = string.Empty;
            registro.DESCSITUACAO = registro.DESCSITUACAO.Trim().ToLower();

        }

        private int ObterNivelOcupacional(string pNivelOcupacional)
        {
            int nivelOcupacional = 0;

            if (string.IsNullOrEmpty(pNivelOcupacional))
                return nivelOcupacional;

            switch (pNivelOcupacional)
            {

                case "ANALISTA TECNICO I":
                case "ANALISTA TÉCNICO I":
                    nivelOcupacional = (int)enumNivelOcupacional.AnalistaTecnicoI;
                    break;

                case "ANALISTA TECNICO II":
                case "ANALISTA TÉCNICO II":
                    nivelOcupacional = (int)enumNivelOcupacional.AnalistaTecnicoII;
                    break;

                case "ANALISTA TECNICO III":
                case "ANALISTA TÉCNICO III":
                    nivelOcupacional = (int)enumNivelOcupacional.AnalistaTecnicoIII;
                    break;

                case "ASSISTENTE I":
                    nivelOcupacional = (int)enumNivelOcupacional.AssistenteI;
                    break;

                case "ASSISTENTE II":
                    nivelOcupacional = (int)enumNivelOcupacional.AssistenteII;
                    break;

                case "ASSISTENTE III":
                    nivelOcupacional = (int)enumNivelOcupacional.AssistenteIII;
                    break;

                case "CONSELHEIRO":
                    nivelOcupacional = (int)enumNivelOcupacional.Conselheiro;
                    break;

                case "TRAINEE":
                    nivelOcupacional = (int)enumNivelOcupacional.Trainee;
                    break;

                case "ESTAGIARIO":
                case "ESTAGIÁRIO":
                    nivelOcupacional = (int)enumNivelOcupacional.Estagiario;
                    break;

                case "APRENDIZ":
                case "MENOR APRENDIZ":
                    nivelOcupacional = (int)enumNivelOcupacional.MenorAprendiz;
                    break;

                case "DIRIGENTE":
                    nivelOcupacional = (int)enumNivelOcupacional.Dirigente;
                    break;

                case "ASSESSOR":
                case "ASSESSOR DE DIRETORIA":
                    nivelOcupacional = (int)enumNivelOcupacional.Assessor;
                    break;

                case "GERENTE":
                    nivelOcupacional = (int)enumNivelOcupacional.Gerente;
                    break;

                case "TERCEIRIZADO":
                    nivelOcupacional = (int)enumNivelOcupacional.Parceiro;
                    break;

            }

            return nivelOcupacional;
        }





        #endregion


        public Usuario ObterUsuarioLogado(bool obterPerfisOriginais = false)
        {
            return new BMUsuario().ObterUsuarioLogado(obterPerfisOriginais);
        }

        public bool PerfilAdministrador()
        {
            return new BMUsuario().PerfilAdministrador();
        }

        /// <summary>
        /// Verifica se o usuário logado possui o perfil para acessar a funcionalidade desejada
        /// </summary>
        /// <param name="perfis">perfis exigidos do usuário</param>
        /// <returns>Retorna true, se o usuário possuir o perfil. 
        /// Retorna false, se o usuário não possuir o perfil</returns>
        public bool VerificarPermissao(IList<enumPerfil> perfis)
        {
            Usuario usuarioLogado = ObterUsuarioLogado();
            bool temPermissao = false;

            //Se o usuário estiver logado, verifica se ele possui o perfil
            if (usuarioLogado != null)
            {
                //Verifica se o usuário possui o perfil (informado atraves do parâmetro perfil)
                if (usuarioLogado.ListaPerfil.Any(x => perfis.Any(y => y == (enumPerfil)x.ID)))
                {
                    temPermissao = true;
                }
            }

            return temPermissao;
        }



        public Usuario ObterUsuarioPorID(int pId)
        {
            try
            {
                return new BMUsuario().ObterPorId(pId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Usuario> ObterTodos()
        {
            return new BMUsuario().ObterTodos();
        }

        public IQueryable<Usuario> ObterTodosPorPerfilIQueryable(enumPerfil perfil)
        {
            return new BMUsuario().ObterTodosPorPerfilIQueryable(perfil);
        }

        public IList<Usuario> ObterTodosPorPerfil(enumPerfil perfil)
        {
            return new BMUsuario().ObterTodosPorPerfil(perfil);
        }

        public Usuario ObterPorCPF(string cpf)
        {

            Usuario usuario = null;

            try
            {
                usuario = new BMUsuario().ObterPorCPF(cpf);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

            return usuario;
        }

        public void AlterarSenha(Usuario pUsuario, int pCodigoInternoSemToken)
        {
            AlterarSenha(pUsuario, pCodigoInternoSemToken.ToString());
        }

        public void RecuperarSenha(Usuario pUsuario, string pToken)
        {
            try
            {
                Usuario usuarioEncontrado = new BMUsuario().ObterPorCPF(pUsuario);
                usuarioEncontrado.Senha = pUsuario.Senha;
                usuarioEncontrado.ConfirmarSenhaLms = pUsuario.ConfirmarSenhaLms;
                this.AlterarSenha(usuarioEncontrado, pToken);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarSenha(Usuario pUsuario, string pToken)
        {
            try
            {

                Usuario us = new BMUsuario().ObterPorId(pUsuario.ID);
                us.Senha = pUsuario.Senha;
                us.ConfirmarSenhaLms = pUsuario.ConfirmarSenhaLms;

                if (pUsuario == null) throw new AcademicoException("Usuário. Campo Obrigatório");

                if (string.IsNullOrWhiteSpace(pToken)) throw new AcademicoException("Token Não identificado. A Senha não foi alterada");

                //Nova Senha
                if (string.IsNullOrWhiteSpace(us.Senha)) throw new AcademicoException("Nova Senha. Campo Obrigatório");

                //Confirmar Nova Senha
                if (string.IsNullOrWhiteSpace(us.ConfirmarSenhaLms)) throw new AcademicoException("Confirmar Nova Senha. Campo Obrigatório");

                //Se a Nova Senha e a Confirmação de senha forem diferentes, exibe mensagem de erro
                if (!us.Senha.Trim().Equals(pUsuario.ConfirmarSenhaLms.Trim())) throw new AcademicoException("Nova Senha e a Confirmação da Nova Senha devem ser iguais.");

                string senhaAux = us.Senha.Trim();

                BMSolicitacaoSenha bmSolicitacaoSenha = new BMSolicitacaoSenha();

                //Ignora passagem caso o método seja evocado pela tela de manter usuário no Admin.
                //TODO: validar com o marcelo se esta chave ficará externa no arquivo de configuração ou fixo no código.
                if (pToken != "1981245348")
                {
                    //Obtém o Usuário pelo Id do Token
                    SolicitacaoSenha solicitacaoSenha = bmSolicitacaoSenha.ObterPorToken(pToken);

                    if (solicitacaoSenha != null)
                    {
                        us = solicitacaoSenha.Usuario;
                    }
                }

                PreencherInformacoesDeAuditoria(us);
                us.Senha = senhaAux;
                this.CriptografarSenha(us);

                new BMUsuario().Salvar(us);

                //Atualiza a Solicitação de senha para invalidar o token
                ManterSolicitacaoSenha manterSolicitacaoSenha = new ManterSolicitacaoSenha();
                SolicitacaoSenha infoSolicitacaoSenha = new SolicitacaoSenha() { Token = pToken };
                manterSolicitacaoSenha.InvalidarToken(infoSolicitacaoSenha);

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void CriptografarSenha(Usuario pUsuario)
        {
            //Criptografa a senha
            //pUsuario.SenhaLms = WebFormHelper.ObterHashMD5(pUsuario.SenhaLms);
            pUsuario.Senha = CriptografiaHelper.Criptografar(pUsuario.Senha);
        }

        public IList<Usuario> ObterPorFiltro(Usuario pUsuario)
        {
            return new BMUsuario().ObterPorFiltros(pUsuario);
        }

        public void EnviarEmailBoasVindas(Usuario usuario)
        {
            Template templateMensagemEmailOfertaExclusiva = new Template();
            string corpoEmail = string.Empty;

            if (usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.GestorUC || x.Perfil.ID == (int)enumPerfil.Administrador))
            {
                templateMensagemEmailOfertaExclusiva = TemplateUtil.ObterInformacoes(enumTemplate.EnvioEmailParaNovosCadastrosGestoresAdministradores);
            }
            else
            {
                templateMensagemEmailOfertaExclusiva = TemplateUtil.ObterInformacoes(enumTemplate.EnvioEmailParaNovosCadastrosUsuarios);
            }

            string assuntoDoEmail = templateMensagemEmailOfertaExclusiva.Assunto;

            corpoEmail = ObterCorpoEmail(usuario, templateMensagemEmailOfertaExclusiva.TextoTemplate);

            EmailUtil.Instancia.EnviarEmail(usuario.Email, assuntoDoEmail, corpoEmail);
        }

        private string ObterCorpoEmail(Usuario usuario, string textoTemplate)
        {
            StringBuilder sb = new StringBuilder();

            string corpo = textoTemplate.Replace("#NOME", usuario.Nome);
            corpo += corpo.Replace("#URL_PORTAL", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal).Registro);
            corpo += corpo.Replace("#URL_SGUS", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro);
            corpo += corpo.Replace("#CPF", usuario.CPF);
            corpo += corpo.Replace("#SENHA", CriptografiaHelper.Decriptografar(usuario.Senha));

            return corpo;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public void Salvar(Usuario usuario, bool ignorarAuditoria = false)
        {
            if (ignorarAuditoria == false)
            {
                PreencherInformacoesDeAuditoria(usuario);

                if (usuario.FileServer != null)
                {
                    PreencherInformacoesDeAuditoria(usuario.FileServer);
                }

                usuario.ListaTag.ToList().ForEach(x => PreencherInformacoesDeAuditoria(x));

                foreach (UsuarioPerfil usuarioPerfil in usuario.ListaPerfil)
                {
                    PreencherInformacoesDeAuditoria(usuarioPerfil);
                    PreencherInformacoesDeAuditoria(usuarioPerfil.Perfil);
                }
            }

            new BMUsuario().Salvar(usuario, ignorarAuditoria);
        }

        public void SalvarSemValidacao(Usuario usuario)
        {
            new BMUsuario().SalvarSemValidacao(usuario);

        }
        private FileServer ObterObjetoFileServer(MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }

        public bool EstaLogado()
        {
            return ObterUsuarioLogado() != null;
        }

        public IList<DTOSistemaExternoPermissao> ObterListaPermissaoSistemasExternos(int idUsuario)
        {
            return new ProcSistemaExternoPermissao().Executar(idUsuario);
        }

        public void NotificarMonitoresAtraso()
        {
            BMUsuario usuarioBM = new BMUsuario();
            List<Usuario> listaMonitores = usuarioBM.ObterMonitoresParaNotificar();
            if (listaMonitores != null && listaMonitores.Count > 0)
            {
                int idMonitorAtual = 0;
                foreach (var monitor in listaMonitores)
                {
                    if (idMonitorAtual != monitor.ID)
                    {
                        idMonitorAtual = monitor.ID;
                        List<ItemTrilhaParticipacao> listaItem = new BMItemTrilhaParticipacao().ObterParticipacoesForaPrazoMonitor(monitor.ID).ToList();
                        List<TrilhaAtividadeFormativaParticipacao> listaSprint = new BMTrilhaAtividadeFormativaParticipacao().ObterParticipacoesForaPrazoMonitor(monitor.ID).ToList();
                        if (listaSprint.Count > 0 || listaItem.Count > 0)
                        {
                            try
                            {
                                Template template = TemplateUtil.ObterInformacoes(enumTemplate.MensagemAlertaMonitorTrilha);
                                string mensagem = template.TextoTemplate;
                                string htmlItem = string.Empty;
                                string htmlSprint = string.Empty;

                                foreach (var registro in listaSprint)
                                {
                                    htmlSprint += string.Format("{0} - {1} - Participante: {2} Data envio: {3} <br />", registro.UsuarioTrilha.TrilhaNivel.Trilha.Nome, registro.UsuarioTrilha.TrilhaNivel.Nome, registro.UsuarioTrilha.Usuario.Nome, registro.DataEnvio.ToString("dd/MM/yyyy"));
                                }
                                foreach (var registro in listaItem)
                                {
                                    htmlItem += string.Format("{0} - {1} - Participante: {2} Data envio: {3} <br />", registro.UsuarioTrilha.TrilhaNivel.Trilha.Nome, registro.UsuarioTrilha.TrilhaNivel.Nome, registro.UsuarioTrilha.Usuario.Nome, registro.DataEnvio.ToString("dd/MM/yyyy"));
                                }

                                mensagem = mensagem.Replace("#LISTASPRINT", htmlSprint)
                                                    .Replace("#LISTAITEM", htmlItem)
                                                    .Replace("#NOMEMONITOR", monitor.Nome)
                                                    ;

                                string destinatario = monitor.Email;
                                string assunto = "Alerta de itens não verificados";
                                EmailUtil.Instancia.EnviarEmail(destinatario, assunto, mensagem);
                            }
                            catch
                            {
                                throw new EmailException("Erro ao enviar o email");
                            }
                        }
                    }
                }
            }
        }

        public void SetarPerfilSimulado(int IdPerfil)
        {
            var bmUsuario = new BMUsuario();

            var perfil = new BMPerfil().ObterPorId(IdPerfil);

            if (perfil == null)
                throw new AcademicoException("Perfil inválido");

            var usuarioPerfil = new UsuarioPerfil()
            {
                Usuario = bmUsuario.ObterUsuarioLogado(),
                Perfil = perfil
            };

            bmUsuario.SetarPerfilSimulado(usuarioPerfil);
        }

        public void SetarPerfisOriginais()
        {
            new BMUsuario().SetarPerfisOriginais();
        }

        public bool IsSimulandoPerfil()
        {
            return new BMUsuario().IsSimulandoPerfil();
        }

        public Perfil ObterPerfilSimulado()
        {
            return new BMUsuario().ObterPerfilSimulado();
        }

        public int ObterUfLogadoSeGestor()
        {
            return new BMUsuario().ObterUfLogadoSeGestor();
        }

        public string pegaLinkHistoricoAcademico(int pIdUsuario)
        {
            var link = "";

            using (RelatorioHistoricoAcademico relHisAcad = new RelatorioHistoricoAcademico())
            {

                var DadosGerais = relHisAcad.ConsultarHistoricoAcademicoDadosGerais(pIdUsuario);
                var lstCursos = relHisAcad.ConsultaHistoricoAcademicoCursos(pIdUsuario);
                var lstExtracurricular = relHisAcad.ConsultarHistoricoAcademicoExtracurricular(pIdUsuario);
                var lstPrograma = relHisAcad.ConsultarHistoricoAcademicoPrograma(pIdUsuario);
                var lstTrilha = relHisAcad.ConsultarHistoricoAcademicoTrilha(pIdUsuario);


                byte[] pdf_bytes = RelatoriosHelper.GerarReportViewerHistoricoAcademicoBytes("HistoricoAcademico.rptHistoricoAcademico.rdlc",
                                                                                       DadosGerais, lstCursos,
                                                                                       lstTrilha, lstPrograma,
                                                                                       lstExtracurricular);
                if (pdf_bytes != null)
                {

                    var file = new FileServer();
                    ManterFileServer manterFileServer = new ManterFileServer();

                    try
                    {
                        string caminhoDiretorioUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                        string nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
                        string diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\", nomeAleatorioDoArquivoParaUploadCriptografado);
                        try
                        {
                            //Salva o arquivo no caminho especificado                            
                            File.WriteAllBytes(diretorioDeUploadComArquivo, pdf_bytes);

                        }
                        catch
                        {
                            throw new Exception("Ocorreu um erro ao Salvar o arquivo");
                            //TODO: ser uma mensagem mais amigavel para o usuario ou tratar melhor o erro
                        }

                        file.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                        file.NomeDoArquivoOriginal = "HistoricoAcademico" + DadosGerais.First().CPF + ".pdf";
                        file.TipoArquivo = "application/pdf";
                        file.MediaServer = false;

                        manterFileServer.IncluirFileServer(file);

                        if (file.ID > 0)
                        {
                            //link = file.;
                            link = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + "/ExibirFileServer.ashx?Identificador=" + file.NomeDoArquivoNoServidor;
                        }

                    }
                    catch (AcademicoException ex)
                    {
                        throw ex;
                    }
                    catch
                    {
                        //Todo: -> Logar erro
                        //throw new AcademicoException("Ocorreu um Erro ao Salvar o arquivo");
                        //TODO: ser uma mensagem mais amigavel para o usuario ou tratar melhor o erro 
                        return "Erro ao gerar o Histórico Acadêmico.";
                    }

                }
            }
            return link;
        }

        public Usuario ObterPerfilOriginal()
        {
            if (IsSimulandoPerfil())
            {
                return ObterUsuarioPorID(ObterUsuarioLogado().ID);
            }

            return ObterUsuarioLogado();
        }

        public IEnumerable<Usuario> ObterPorNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            return bmUsuario.ObterPorNivelOcupacional(nivelOcupacional);
        }

        public IEnumerable<Usuario> ObterPorNiveisOcupacionais(List<int> niveisOcupacionais)
        {
            return bmUsuario.ObterPorNiveisOcupacionais(niveisOcupacionais);
        }

        public IQueryable<Usuario> ObterPorUfsNiveisPerfis(List<Uf> ufs, List<NivelOcupacional> niveis, List<Perfil> perfis)
        {
            return bmUsuario.ObterPorUfsNiveisPerfis(ufs, niveis, perfis);
        }

        public DateTime? AdicionarTempoTokenTrilha(Usuario u, int horas = 1)
        {
            u.TrilhaTokenExpiry = DateTime.Now.AddDays(horas);

            SalvarTokenTrilha(ref u, false);

            return u.TrilhaTokenExpiry;
        }

        public string GerarTokenTrilha(Usuario u, int horas = 1)
        {
            u.TrilhaTokenExpiry = DateTime.Now.AddDays(horas);
            
            SalvarTokenTrilha(ref u);

            return u.TrilhaToken;
        }

        private static void SalvarTokenTrilha(ref Usuario u, bool gerarNovoToken = true)
        {
            var lstParams = new Dictionary<string, object>
            {
                {"ID_Usuario", u.ID},
                {"TrilhaTokenExpiry", u.TrilhaTokenExpiry}
            };

            if (gerarNovoToken)
            {
                u.TrilhaToken = Guid.NewGuid().ToString();
                lstParams.Add("TrilhaToken", u.TrilhaToken);
            }

            new BusinessManagerBase().ExecutarProcedure("SP_ATUALIZAR_TOKEN_TRILHA", lstParams);
        }

        public string RenovarToken(string token)
        {
            var usuario =
                bmUsuario.ObterTodosIQueryable()
                    .FirstOrDefault(
                        x =>
                            x.TrilhaToken == token && x.TrilhaTokenExpiry.HasValue &&
                            x.TrilhaTokenExpiry.Value > DateTime.Now);

            if (usuario == null)
                return null;

            return GerarTokenTrilha(usuario);
        }

        public virtual string ObterUrlImagem(int usuarioId, string enderecoSgus = null)
        {
            var fileServer = ObterFileServerPorUserId(usuarioId);

            if (fileServer == null)
                return null;

            if(string.IsNullOrWhiteSpace(enderecoSgus))
                enderecoSgus =
                    ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro;

            return enderecoSgus + "ExibirThumbnail.ashx?Identificador=" + fileServer.NomeDoArquivoNoServidor;
        }

        public virtual string ObterImagem(int usuarioId, int largura, string nomeDoArquivoNoServidor,
            string repositorioUpload = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeDoArquivoNoServidor))
                    return null;

                if (string.IsNullOrWhiteSpace(repositorioUpload))
                    repositorioUpload =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

                var imageFile = Path.Combine(repositorioUpload, nomeDoArquivoNoServidor);

                if (!File.Exists(imageFile))
                    return null;

                using (var imageStream = new MemoryStream())
                {
                    var imagem = Image.FromFile(imageFile);

                    var destRect = new Rectangle(0, 0, largura, largura);
                    var destImage = new Bitmap(largura, largura);

                    destImage.SetResolution(imagem.HorizontalResolution, imagem.VerticalResolution);

                    using (var graphics = Graphics.FromImage(destImage))
                    {
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        using (var wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                            graphics.DrawImage(imagem, destRect, 0, 0, imagem.Width, imagem.Height, GraphicsUnit.Pixel,
                                wrapMode);
                        }
                    }

                    destImage.Save(imageStream, ImageFormat.Jpeg);

                    var imageBytes = imageStream.ToArray();

                    return string.Concat("data:image/jpeg;base64,", Convert.ToBase64String(imageBytes));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private FileServer ObterFileServerPorUserId(int usuarioId)
        {
            return ObterTodosIQueryable().Where(x => x.ID == usuarioId && x.FileServer != null)
                .Select(x => new
                {
                    FileServer = x.FileServer != null
                        ? new FileServer
                        {
                            ID = x.FileServer.ID,
                            NomeDoArquivoNoServidor = x.FileServer.NomeDoArquivoNoServidor
                        }
                        : null
                })
                .FirstOrDefault()?.FileServer;
        }

        public void SalvarEmLote(List<Usuario> usuarios, int bathSize)
        {
            bmUsuario.SalvarEmLote(usuarios, bathSize);
        }

        public bool PerfilAdministradorTrilha()
        {
            return new BMUsuario().PerfilAdministradorTrilha();
        }

        private void ValidarDadosDeLogin(string usuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new AcademicoException("Usuário. Campo Obrigatório!");

            if (string.IsNullOrWhiteSpace(senha))
                throw new AcademicoException("Senha. Campo Obrigatório!");
        }

        private bool ValidarSenhaPorUsuarioUf(Usuario usuario, SenhaEmergencia senha)
        {
            if (senha.UF.ID == (int)enumUF.NA || usuario.UF.ID == senha.UF.ID)
                return true;
            else
                return false;
        }

        private static void ValidarSituacaoUsuario(Usuario usuarioEncontrado)
        {
            if (!string.IsNullOrEmpty(usuarioEncontrado.Situacao))
            {
                switch (usuarioEncontrado.Situacao.ToLower())
                {
                    case "licença mater. compl. 180 dias":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença maternidade, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "outros":
                        throw new AcademicoException("Usuário inativo no sistema!");
                    case "licença s/venc":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença sem vencimento, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "licenca mater.":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença maternidade, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "admissão prox.mês":
                        throw new AcademicoException("Acesso bloqueado pois usuário ainda não está ativo na base de empregados, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "demitido":
                        throw new AcademicoException("Para mais informações entre em contato com o suporte (61) 3208-1124");
                    case "Af.Ac.Trabalho":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em afastamento, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "apos. invalidez":
                        throw new AcademicoException("Acesso bloqueado pois usuário está aposentado, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    //case "férias":
                    //    throw new AcademicoException("Acesso bloqueado pois usuário está férias, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "licença mater.":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença maternidade, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "licença remun.":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença remunerada, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "af.previdencia":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença médica, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "inativo":
                        throw new AcademicoException("Acesso bloqueado pois usuário ainda não está ativo na base de empregados, caso não seja este o caso, procure o gestor da UC em seu estado");
                }

            }
            if (!usuarioEncontrado.Ativo)
            {
                throw new AcademicoException("Usuário inativo no sistema!");
            }
        }

        public Usuario Login(string cpf, string senha, bool salvarLog = false, bool incluirNaSessao = true)
        {
            var bmUsuario = new BMUsuario();

            ValidarDadosDeLogin(cpf, senha);

            string senhaCriptografada = CriptografiaHelper.Criptografar(senha);

            Usuario usuarioEncontrado = ObterPorCPF(cpf);

            if (usuarioEncontrado == null)
            {
                throw new AcademicoException("Usuário não encontrado!");
            }

            //Usuario sem senha cadastrada
            if (string.IsNullOrEmpty(usuarioEncontrado.Senha))
            {
                //Gerar Senha
                usuarioEncontrado = GerarSenhaAleatoria(usuarioEncontrado);

                var senhaEmergencia = new BMSenhaEmergencia().ObterSenhaAtiva();

                if (!ValidarSenhaPorUsuarioUf(usuarioEncontrado, senhaEmergencia) && !senhaEmergencia.Senha.Trim().Equals(senhaCriptografada.Trim()))
                {
                    return null;
                }
            }

            //Se as senhas forem diferentes, faz a autenticação na tabela de senha do dia.
            if (!usuarioEncontrado.Senha.Trim().Equals(senhaCriptografada.Trim()))
            {
                //Busca na tabela de senha de emergencia.
                //Demanda 761 - Desabilitar Acesso ao Portal por Senha de Emergência
                var senhaEmergencia = new BMSenhaEmergencia().ObterSenhaAtiva();

                if (!this.ValidarSenhaPorUsuarioUf(usuarioEncontrado, senhaEmergencia) || !senhaEmergencia.Senha.Trim().Equals(senhaCriptografada.Trim()))
                {
                    return null;
                }
            }

            var usuarioPDI = ObterUsuarioNoProgramaPDI(usuarioEncontrado, bmUsuario.ObterUsuariosPDI());

            if (usuarioPDI == null || !usuarioPDI.AtivoNoPDI())
            {
                ValidarSituacaoUsuario(usuarioEncontrado);
            }
            else
            {
                throw new AcademicoException("Para mais informações entre em contato com o suporte (61) 3208-1124");
            }

            if (salvarLog)
            {
                CriarLogAcesso(usuarioEncontrado);
            }

            if (incluirNaSessao)
            {
                // Persiste o usuário na sessão do sistema
                SetUsuarioNaSessao(usuarioEncontrado);
            }

            return usuarioEncontrado;
        }

        private void SetUsuarioNaSessao(Usuario usuario)
        {
            //Guarda o usuário em variável de sessão.
            BMUsuario bmUsuario = new BMUsuario();
            bmUsuario.SetarUsuarioLogado(usuario);
        }

        private Usuario GerarSenhaAleatoria(Usuario usuario)
        {
            usuario.Senha = CriptografiaHelper.Criptografar(WebFormHelper.ObterSenhaAleatoria());
            Salvar(usuario);

            return usuario;
        }

        private Usuario ObterUsuarioNoProgramaPDI(Usuario usuario, IList<Usuario> usuariosPDI)
        {
            return usuariosPDI.FirstOrDefault(x => x.CPF == usuario.CPF);
        }

        public Usuario CriarLogAcesso(Usuario usuario)
        {
            try
            {
                if (usuario != null && usuario.ListaPerfil != null && usuario.ListaPerfil.Count > 0)
                {
                    if (usuario.ListaPerfil.Any()
                        &&
                            (usuario.ListaPerfil.Any(x => x.Perfil == enumPerfil.Administrador)
                            || usuario.ListaPerfil.Any(x => x.Perfil == enumPerfil.GestorUC)
                            || usuario.ListaPerfil.Any(x => x.Perfil == enumPerfil.AdministradorTrilha)
                            || usuario.ListaPerfil.Any(x => x.Perfil == enumPerfil.MonitorTrilha)
                            || usuario.ListaPerfil.Any(x => x.Perfil == enumPerfil.ConsultorEducacional)
                            )
                        )
                    {
                        LogAcesso logAcesso = new LogAcesso(usuario.ID, false, string.Empty, string.Empty);
                        logAcesso.IDUsuario = usuario.ID;
                        logAcesso.INSGUS = true;

                        BMLogAcessosPaginas bmLogAcesso = new BMLogAcessosPaginas();
                        bmLogAcesso.Salvar(logAcesso);


                    }
                    else
                    {
                        throw new AcademicoException("Você não possui permissão de acesso a esta Funcionalidade");
                    }
                }
                else
                {
                    throw new AcademicoException("Não foi possível efetuar o login");
                }


                return usuario;
            }
            catch (AcademicoException ex)
            {
                throw new AcademicoException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void PreencherInformacoesDeAuditoria(EntidadeBasicaPorId entidade)
        {
            entidade.Auditoria.UsuarioAuditoria = ObterCpfDoUsuarioLogado();
            entidade.Auditoria.DataAuditoria = DateTime.Now;
        }

        protected string ObterCpfDoUsuarioLogado()
        {
            BMUsuario bmUsuario = new BMUsuario();
            Usuario usuario = this.ObterUsuarioLogado();
            string cpfDoUsuarioLogado = null;

            if (usuario != null)
            {
                cpfDoUsuarioLogado = usuario.CPF;
            }

            return cpfDoUsuarioLogado;
        }
    }
}
