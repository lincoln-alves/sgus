using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Web;
using System.IO;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.AtividadeFormativa
{
    public partial class EdicaoAtividadeFormativaParticipacao : System.Web.UI.Page
    {
        private classes.TrilhaAtividadeFormativaParticipacao trilhaAtividadeInformativaParticipacao = null;
        private ManterTrilhaAtividadeFormativaParticipacao manterTrilhaAtividadeFormativaParticipacao;

        private string ObterInformacaoDoArray(string[] arrayDeIDs, string id)
        {
            var resultado = arrayDeIDs.FirstOrDefault(x => x != null && x.Contains(id));
            string valordoID = null;

            if (!string.IsNullOrWhiteSpace(resultado))
            {
                //Ex: idtrilha=2
                string[] array = resultado.Split('=');

                //Ex: 2
                //valordoID = int.Parse(array[1]);
                valordoID = array[1];
            }

            return valordoID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PreencherCombos();

                if (Request["Id"] != null)
                {
                    int idAtividadeFormativaParticipacao = int.Parse(Request["Id"].ToString());
                    trilhaAtividadeInformativaParticipacao = manterTrilhaAtividadeFormativaParticipacao.ObterTrilhaAtividadeFormativaParticipacaoPorID(idAtividadeFormativaParticipacao);
                    PreencherCampos(trilhaAtividadeInformativaParticipacao);
                }
                else
                {
                    if (Request["IdConcatenado"] != null)
                    {
                        PreencherCamposParaUsuariosSemParticipacao();
                    }
                }
            }
        }

        private void PreencherCamposParaUsuariosSemParticipacao()
        {
            string[] arrayDeIDs = Request["IdConcatenado"].Split('|');
            string idtrilha, idtniv, idtop, idusu, idusuarioTrilha;

            //Trilha
            idtrilha = this.ObterInformacaoDoArray(arrayDeIDs, "idtrilha");
            if (string.IsNullOrWhiteSpace(idtrilha)) throw new AcademicoException("Ocorreu um Erro ao Obter o ID da Trilha");

            //Trilha Nível
            idtniv = this.ObterInformacaoDoArray(arrayDeIDs, "idtniv");
            if (string.IsNullOrWhiteSpace(idtniv)) throw new AcademicoException("Ocorreu um Erro ao Obter o ID da Trilha Nível");

            //Tópico Temático
            idtop = this.ObterInformacaoDoArray(arrayDeIDs, "idtop");
            if (string.IsNullOrWhiteSpace(idtop)) throw new AcademicoException("Ocorreu um Erro ao Obter o ID do Tópico Temático");

            //Usuário
            idusu = this.ObterInformacaoDoArray(arrayDeIDs, "idusu");
            if (string.IsNullOrWhiteSpace(idusu)) throw new AcademicoException("Ocorreu um Erro ao Obter o ID do Usuário");

            //Usuario Trilha
            idusuarioTrilha = this.ObterInformacaoDoArray(arrayDeIDs, "idusuarioTrilha");
            if (string.IsNullOrWhiteSpace(idusuarioTrilha)) throw new AcademicoException("Ocorreu um Erro ao Obter o ID do Usuário Trilha");

            //Trilha
            SetarValorNaComboTrilha(idtrilha);

            //Trilha Nível
            SetarValorNaComboTrilhaNivel(idtniv, null);

            //Trilha Tópico Temático
            SetarValorNaComboTrilhaTopicoTematico(idtop, null);

            ExibirCamposDinamicos(idtop);

            //Usuário
            SetarValorNaComboUsuario(idusu);

            //Guarda o idUsuarioTrilha no campo Hidden para usar no cadastro da participação do usuário
            hdfIdUsuarioTrilha.Value = idusuarioTrilha;


        }


        private void ExibirCamposDinamicos(string idTopicoTematico)
        {

            classes.TrilhaTopicoTematico topicoTematico = new ManterTrilhaTopicoTematico().ObterTrilhaTopicoTematicoPorID(int.Parse(idTopicoTematico));

            if (topicoTematico != null)
            {
                if (!string.IsNullOrWhiteSpace(topicoTematico.DescricaoTextoEnvio))
                {
                    lblTextoParticipacao.Text = topicoTematico.DescricaoTextoEnvio;
                    this.trTextoParticipacao.Visible = true;
                }
                else
                {
                    this.trTextoParticipacao.Visible = false;
                }

                if (!string.IsNullOrWhiteSpace(topicoTematico.DescricaoArquivoEnvio))
                {
                    lblArquivoEnvioDe.Text = topicoTematico.DescricaoArquivoEnvio;
                    this.trArquivoEnvio.Visible = true;
                }
                else
                {
                    this.trArquivoEnvio.Visible = false;
                }

            }
        }


        private void SetarValorNaComboUsuario(string idusu)
        {
            if (!string.IsNullOrWhiteSpace(idusu))
            {
                ManterUsuario manterUsuario = new ManterUsuario();
                Usuario usuario = manterUsuario.ObterUsuarioPorID(int.Parse(idusu));
                ddlNomeAluno.Items.Add(new ListItem(usuario.Nome, usuario.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(usuario.ID.ToString(), ddlNomeAluno, true);
            }
        }

        private void SetarValorNaComboUsuario(UsuarioTrilha usuarioTrilha)
        {
            if (usuarioTrilha != null && usuarioTrilha.Usuario != null)
            {
                ddlNomeAluno.Items.Add(new ListItem(usuarioTrilha.Usuario.Nome, usuarioTrilha.Usuario.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(usuarioTrilha.Usuario.ID.ToString(), ddlNomeAluno, true);
            }
        }

        private void SetarValorNaComboTrilhaTopicoTematico(string idtop, TrilhaTopicoTematico trilhaTopicoTematico)
        {
            ManterTrilhaTopicoTematico manterTrilhaTopicoTematico = new ManterTrilhaTopicoTematico();

            if (!string.IsNullOrWhiteSpace(idtop))
            {
                trilhaTopicoTematico = manterTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorID(int.Parse(idtop));
                ddlTopicoTematico.Items.Add(new ListItem(trilhaTopicoTematico.Nome, trilhaTopicoTematico.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(trilhaTopicoTematico.ID.ToString(), ddlTopicoTematico, true);
            }
            else if (trilhaTopicoTematico != null)
            {
                ddlTopicoTematico.Items.Add(new ListItem(trilhaTopicoTematico.Nome, trilhaTopicoTematico.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(trilhaTopicoTematico.ID.ToString(), ddlTopicoTematico, true);
            }
        }

        private void SetarValorNaComboTrilha(string idtrilha)
        {
            if (!string.IsNullOrWhiteSpace(idtrilha))
            {
                WebFormHelper.SetarValorNaCombo(idtrilha, ddlTrilha, true);
            }
        }

        private void SetarValorNaComboTrilhaNivel(string idtniv, classes.TrilhaNivel trilhaNivel)
        {
            ManterTrilhaNivel manterTrilhaNivel = new ManterTrilhaNivel();

            if (!string.IsNullOrWhiteSpace(idtniv))
            {
                trilhaNivel = manterTrilhaNivel.ObterTrilhaNivelPorID(int.Parse(idtniv));
                ddlTrilhaNivel.Items.Add(new ListItem(trilhaNivel.Nome, trilhaNivel.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(trilhaNivel.ID.ToString(), ddlTrilhaNivel, true);
            }
            else if (trilhaNivel != null)
            {
                ddlTrilhaNivel.Items.Add(new ListItem(trilhaNivel.Nome, trilhaNivel.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(trilhaNivel.ID.ToString(), ddlTrilhaNivel, true);
            }

        }

        private void PreencherCampos(classes.TrilhaAtividadeFormativaParticipacao trilhaAtividadeFormativaParticipacao)
        {
            if (trilhaAtividadeFormativaParticipacao != null)
            {

                //Trilha
                SetarValorNaComboTrilha(trilhaAtividadeFormativaParticipacao.UsuarioTrilha.TrilhaNivel.Trilha.ID.ToString());

                //Trilha Nível
                SetarValorNaComboTrilhaNivel(null, trilhaAtividadeFormativaParticipacao.UsuarioTrilha.TrilhaNivel);

                //Tópico Temático
                SetarValorNaComboTrilhaTopicoTematico(null, trilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico);

                //Usuário
                SetarValorNaComboUsuario(trilhaAtividadeFormativaParticipacao.UsuarioTrilha);

                string idtop = this.ddlTopicoTematico.SelectedItem.Value;

                ExibirCamposDinamicos(idtop);

                //Texto Participação
                if (!string.IsNullOrWhiteSpace(trilhaAtividadeFormativaParticipacao.TextoParticipacao))
                {
                    lblTextoParticipacao.Text = trilhaAtividadeInformativaParticipacao.TrilhaTopicoTematico.DescricaoTextoEnvio;
                    trTextoParticipacao.Visible = true;
                    txtTextoParticipacao.Text = trilhaAtividadeFormativaParticipacao.TextoParticipacao;
                }

                //Arquivo de Envio 
                if (trilhaAtividadeInformativaParticipacao.FileServer != null)
                {
                    lkbArquivo.Text = string.Concat("Abrir arquivo ", trilhaAtividadeInformativaParticipacao.FileServer.NomeDoArquivoOriginal);
                }

                //Guarda o idUsuarioTrilha no campo Hidden para usar no cadastro da participação do usuário
                hdfIdUsuarioTrilha.Value = trilhaAtividadeInformativaParticipacao.UsuarioTrilha.ID.ToString();

                //Adiciona o id da atividade formativa participação no viewstate
                ViewState.Add("idtrilhaAtividadeInformativaParticipacao", trilhaAtividadeInformativaParticipacao.ID);

            }

        }

        private void BaixarArquivo()
        {
            int idtrilhaAtividadeInformativaParticipacao = 0;

            if (ViewState["idtrilhaAtividadeInformativaParticipacao"] != null)
            {
                //Obtém o Id da atividade formativa participação do viewstate
                idtrilhaAtividadeInformativaParticipacao = (int)ViewState["idtrilhaAtividadeInformativaParticipacao"];
            }

            if (idtrilhaAtividadeInformativaParticipacao > 0)
            {

                classes.TrilhaAtividadeFormativaParticipacao trilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao().ObterTrilhaAtividadeFormativaParticipacaoPorID(idtrilhaAtividadeInformativaParticipacao);

                if (trilhaAtividadeFormativaParticipacao != null && trilhaAtividadeFormativaParticipacao.FileServer != null)
                {
                    string caminhoFisicoDoDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    //string caminhoLogicoDoDiretorioDeUpload = WebFormHelper.ObterCaminhoVirtualDoDiretorioDeUpload(caminhoFisicoDoDiretorioDeUpload);
                    string caminhoLogicoDoArquivo = string.Concat(caminhoFisicoDoDiretorioDeUpload, "\\" + trilhaAtividadeFormativaParticipacao.FileServer.NomeDoArquivoNoServidor);

                    if (!File.Exists(caminhoLogicoDoArquivo))
                        throw new FileNotFoundException("Arquivo não encontrado no servidor!");

                    Response.ContentType = trilhaAtividadeFormativaParticipacao.FileServer.TipoArquivo;
                    Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", trilhaAtividadeFormativaParticipacao.FileServer.NomeDoArquivoOriginal));
                    HttpContext.Current.Response.TransmitFile(caminhoLogicoDoArquivo);
                    Response.End();
                }

            }

        }
             
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
                trilhaAtividadeInformativaParticipacao = this.ObterObjetoTrilhaAtividadeFormativaParticipacao();

                if (Request["Id"] == null)
                {
                    manterTrilhaAtividadeFormativaParticipacao.IncluirTrilhaAtividadeFormativaParticipacao(trilhaAtividadeInformativaParticipacao);
                }
                else
                {
                    manterTrilhaAtividadeFormativaParticipacao.AlterarTrilhaAtividadeFormativaParticipacao(trilhaAtividadeInformativaParticipacao);
                }

                //Session.Remove("AtividadeFormativaParticipacaoEdit");
            }
            catch (AcademicoException ex)
            {
                //TODO -> Exibir mensagem de Erro em um alert ? -> Precisamos definir isso
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarAtividadeFormativaParticipacao.aspx");
            
        }
     
        #region "Métodos Privados"

        private classes.TrilhaAtividadeFormativaParticipacao ObterObjetoTrilhaAtividadeFormativaParticipacao()
        {

            classes.TrilhaAtividadeFormativaParticipacao trilhaAtividadeFormativaParticipacao;

            if (Request["Id"] != null)
            {
                trilhaAtividadeFormativaParticipacao = new BP.ManterTrilhaAtividadeFormativaParticipacao().ObterTrilhaAtividadeFormativaParticipacaoPorID(int.Parse(Request["Id"]));
            }
            else
            {
                trilhaAtividadeFormativaParticipacao = new Dominio.Classes.TrilhaAtividadeFormativaParticipacao();
            }
            //Arquivo de Envio
            if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null && fupldArquivoEnvio.PostedFile.ContentLength > 0)
            {

                try
                {

                    string diretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    string nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria(); // Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8);
                    string diretorioDeUploadComArquivo = string.Concat(diretorioDeUpload, @"\", nomeAleatorioDoArquivoParaUploadCriptografado);

                    try
                    {
                        //Salva o arquivo no caminho especificado
                        fupldArquivoEnvio.PostedFile.SaveAs(diretorioDeUploadComArquivo);
                    }
                    catch
                    {
                        //Todo: -> Logar o Erro
                        throw new AcademicoException("Ocorreu um erro ao Salvar o arquivo");
                    }

                    trilhaAtividadeFormativaParticipacao.FileServer.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                    trilhaAtividadeFormativaParticipacao.FileServer.NomeDoArquivoOriginal = fupldArquivoEnvio.FileName;
                    trilhaAtividadeFormativaParticipacao.FileServer.TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType;

                }
                catch (AcademicoException ex)
                {
                    throw ex;
                }
                catch
                {
                    //Todo: -> Logar erro
                    throw new AcademicoException("Ocorreu um Erro ao Salvar o arquivo");
                }

            }
            else
            {
                trilhaAtividadeFormativaParticipacao.FileServer = null;
            }
            
            //Usuário Trilha
            if (ddlNomeAluno.SelectedItem != null)
            {
                trilhaAtividadeFormativaParticipacao.UsuarioTrilha = new UsuarioTrilha() { ID = int.Parse(this.ddlNomeAluno.SelectedItem.Value) };
            }

            //Texto Participacao
            trilhaAtividadeFormativaParticipacao.TextoParticipacao = txtTextoParticipacao.Text;

            //Tópico Temático
            if (!string.IsNullOrWhiteSpace(ddlTopicoTematico.SelectedItem.Value))
            {
                trilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico = new TrilhaTopicoTematico() { ID = int.Parse(this.ddlTopicoTematico.SelectedItem.Value) };
            }
            else
            {
                trilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico = null;
            }

            trilhaAtividadeFormativaParticipacao.UsuarioTrilha = new UsuarioTrilha() { ID = int.Parse(hdfIdUsuarioTrilha.Value) };

            return trilhaAtividadeFormativaParticipacao;
        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTrilhas();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherComboTrilhas()
        {
            manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
            IList<Trilha> ListaTrilhas = manterTrilhaAtividadeFormativaParticipacao.ObterTilhas();
            WebFormHelper.PreencherLista(ListaTrilhas, this.ddlTrilha, false, true);
        }

        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session["AtividadeFormativaParticipacaoEdit"] = null;
            Response.Redirect("ListarAtividadeFormativaParticipacao.aspx");
        }

        private ViewTrilha ObterObjetoViewTrilha()
        {
            ViewTrilha viewTrilha = new ViewTrilha();

            //Trilha
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                viewTrilha.TrilhaOrigem = new Trilha() { ID = int.Parse(this.ddlTrilha.SelectedItem.Value) };
            }

            //Trilha Nível
            if (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value))
            {
                viewTrilha.TrilhaNivelOrigem = new classes.TrilhaNivel() { ID = int.Parse(this.ddlTrilhaNivel.SelectedItem.Value) };
            }

            //Tópico Temático
            if (!string.IsNullOrWhiteSpace(ddlTopicoTematico.SelectedItem.Value))
            {
                viewTrilha.TopicoTematico = new TrilhaTopicoTematico() { ID = int.Parse(this.ddlTopicoTematico.SelectedItem.Value) };
            }

            //Aluno
            if (!string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value))
            {
                viewTrilha.UsuarioOrigem = new Usuario()
                {
                    ID = int.Parse(this.ddlNomeAluno.SelectedItem.Value)
                };
            }

            return viewTrilha;
        }

        protected void ddlTopicoTematico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.ddlTopicoTematico.SelectedItem.Value))
            {

                int idTopicoTematico = int.Parse(this.ddlTopicoTematico.SelectedItem.Value);
                ManterTrilhaTopicoTematico manterTrilhaTopicoTematico = new ManterTrilhaTopicoTematico();
                TrilhaTopicoTematico topicoTematico = manterTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorID(idTopicoTematico);

                if (topicoTematico != null)
                {
                    lblTextoParticipacao.Text = topicoTematico.DescricaoTextoEnvio;
                    lblArquivoEnvioDe.Text = topicoTematico.DescricaoArquivoEnvio;
                    this.trTextoParticipacao.Visible = true;
                    this.trArquivoEnvio.Visible = true;
                }
            }
            else
            {
                this.trTextoParticipacao.Visible = false;
                this.trArquivoEnvio.Visible = false;
            }
        }

        protected void lkbArquivo_Click(object sender, EventArgs e)
        {
            BaixarArquivo();
        }
    }
}