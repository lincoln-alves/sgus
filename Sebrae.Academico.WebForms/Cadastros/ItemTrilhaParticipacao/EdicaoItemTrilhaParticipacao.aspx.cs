using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using System.IO;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoItemTrilhaParticipacao : System.Web.UI.Page
    {
        private classes.ItemTrilhaParticipacao itemTrilhaParticipacao = null;
        private ManterItemTrilhaParticipacao manterItemTrilhaParticipacao;

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
                    int idItemTrilhaParticipacao = int.Parse(Request["Id"].ToString());
                    itemTrilhaParticipacao = manterItemTrilhaParticipacao.ObterItemTrilhaParticipacaoPorID(idItemTrilhaParticipacao);
                    PreencherCampos(itemTrilhaParticipacao);
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
            string idtrilha, idtniv, idtop, idusu, iditemtrilha, idusuarioTrilha;

            //Trilha
            idtrilha = this.ObterInformacaoDoArray(arrayDeIDs, "idtrilha");
            if (string.IsNullOrWhiteSpace(idtrilha)) throw new AcademicoException("Ocorreu um Erro ao Obter o id da Trilha");

            //Trilha Nível
            idtniv = this.ObterInformacaoDoArray(arrayDeIDs, "idtniv");
            if (string.IsNullOrWhiteSpace(idtniv)) throw new AcademicoException("Ocorreu um Erro ao Obter o id da Trilha Nível");

            //Tópico Temático
            idtop = this.ObterInformacaoDoArray(arrayDeIDs, "idtop");
            if (string.IsNullOrWhiteSpace(idtop)) throw new AcademicoException("Ocorreu um Erro ao Obter o id da Tópico Temático");

            //Usuário
            idusu = this.ObterInformacaoDoArray(arrayDeIDs, "idusu");
            if (string.IsNullOrWhiteSpace(idusu)) throw new AcademicoException("Ocorreu um Erro ao Obter o id do Usuário");

            //Item Trilha
            iditemtrilha = this.ObterInformacaoDoArray(arrayDeIDs, "iditemtrilha");
            if (string.IsNullOrWhiteSpace(iditemtrilha)) throw new AcademicoException("Ocorreu um Erro ao Obter o id do Item Trilha");

            //Usuario Trilha
            idusuarioTrilha = this.ObterInformacaoDoArray(arrayDeIDs, "idusuarioTrilha");
            if (string.IsNullOrWhiteSpace(idusuarioTrilha)) throw new AcademicoException("Ocorreu um Erro ao Obter o id do Usuário Trilha");

            //Trilha
            SetarValorNaComboTrilha(idtrilha);

            //Trilha Nível
            SetarValorNaComboTrilhaNivel(idtniv, null);

            //Trilha Tópico Temático
            SetarValorNaComboTrilhaTopicoTematico(idtop, null);

            //Usuário
            SetarValorNaComboUsuario(idusu);

            //Item Trilha
            SetarValorNaComboItemTrilha(iditemtrilha, null);

            //Guarda o idUsuarioTrilha no campo Hidden para usar no cadastro da participação do usuário
            hdfIdUsuarioTrilha.Value = idusuarioTrilha;
        }

        private void SetarValorNaComboItemTrilha(string iditemtrilha, classes.ItemTrilha itemTrilha)
        {
            ManterItemTrilha manterItemTrilha = new ManterItemTrilha();

            if (!string.IsNullOrWhiteSpace(iditemtrilha))
            {
                itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(int.Parse(iditemtrilha));
                ddlItemTrilha.Items.Add(new ListItem(itemTrilha.Nome, itemTrilha.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(iditemtrilha.ToString(), ddlItemTrilha, true);
            }
            else if (itemTrilha != null)
            {
                ddlItemTrilha.Items.Add(new ListItem(itemTrilha.Nome, itemTrilha.ID.ToString()));
                WebFormHelper.SetarValorNaCombo(itemTrilha.ID.ToString(), ddlItemTrilha, true);
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

        private void PreencherCampos(classes.ItemTrilhaParticipacao itemTrilhaParticipacao)
        {
            if (itemTrilhaParticipacao != null)
            {

                // Seta o valor padrão de Trilha
                SetarValorNaComboTrilha(itemTrilhaParticipacao.UsuarioTrilha.TrilhaNivel.Trilha.ID.ToString());

                //Trilha Nível
                SetarValorNaComboTrilhaNivel(null, itemTrilhaParticipacao.ItemTrilha.Missao.PontoSebrae.TrilhaNivel);

                //Tópico Temático
                SetarValorNaComboTrilhaTopicoTematico(null, itemTrilhaParticipacao.ItemTrilha.TrilhaTopicoTematico);

                //Usuário
                SetarValorNaComboUsuario(itemTrilhaParticipacao.UsuarioTrilha);

                //Item Trilha
                SetarValorNaComboItemTrilha(null, itemTrilhaParticipacao.ItemTrilha);

                //Texto Participação
                if (!string.IsNullOrWhiteSpace(itemTrilhaParticipacao.TextoParticipacao))
                    txtTextoParticipacao.Text = itemTrilhaParticipacao.TextoParticipacao;

                //Arquivo de Envio 
                if (itemTrilhaParticipacao.FileServer != null && !string.IsNullOrWhiteSpace(itemTrilhaParticipacao.FileServer.NomeDoArquivoNoServidor))
                {
                    lkbArquivo.Text = string.Concat("Abrir arquivo ", itemTrilhaParticipacao.FileServer.NomeDoArquivoOriginal);
                }

                //Guarda o idUsuarioTrilha no campo Hidden para usar no cadastro da participação do usuário
                hdfIdUsuarioTrilha.Value = itemTrilhaParticipacao.UsuarioTrilha.ID.ToString();

                ViewState.Add("iditemTrilhaParticipacao", itemTrilhaParticipacao.ID);

            }

        }


        private void BaixarArquivo()
        {
            int iditemTrilhaParticipacao = 0;

            if (ViewState["iditemTrilhaParticipacao"] != null)
            {
                //Obtém o Id do Item Trilha participação do viewstate
                iditemTrilhaParticipacao = (int)ViewState["iditemTrilhaParticipacao"];
            }

            if (iditemTrilhaParticipacao > 0)
            {

                classes.ItemTrilhaParticipacao itemTrilhaParticipacao = new ManterItemTrilhaParticipacao().ObterItemTrilhaParticipacaoPorID(iditemTrilhaParticipacao);

                if (itemTrilhaParticipacao != null && itemTrilhaParticipacao.FileServer != null)
                {
                    string caminhoFisicoDoDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    string caminhoLogicoDoArquivo = string.Concat(caminhoFisicoDoDiretorioDeUpload, "\\" + itemTrilhaParticipacao.FileServer.NomeDoArquivoNoServidor);

                    if (!File.Exists(caminhoLogicoDoArquivo))
                        throw new FileNotFoundException("Arquivo não encontrado no servidor!");


                    Response.ContentType = itemTrilhaParticipacao.FileServer.TipoArquivo;
                    Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", itemTrilhaParticipacao.FileServer.NomeDoArquivoOriginal));
                    HttpContext.Current.Response.TransmitFile(caminhoLogicoDoArquivo);
                    Response.End();
                }

            }

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
                itemTrilhaParticipacao = this.ObterObjetoItemTrilhaParticipacao();

                if (Request["Id"] == null)
                {
                    manterItemTrilhaParticipacao.IncluirItemTrilhaParticipacao(itemTrilhaParticipacao);
                }
                else
                {
                    manterItemTrilhaParticipacao.AlterarItemTrilhaParticipacao(itemTrilhaParticipacao);
                }

                Session.Remove("ItemTrilhaParticipacaoEdit");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarItemTrilhaParticipacao.aspx");
        }

        #region "Métodos Privados"

        private classes.ItemTrilhaParticipacao ObterObjetoItemTrilhaParticipacao()
        {

            classes.ItemTrilhaParticipacao itemTrilhaParticipacao;

            if (Request["Id"] != null)
            {
                itemTrilhaParticipacao = manterItemTrilhaParticipacao.ObterItemTrilhaParticipacaoPorID(int.Parse(Request["Id"].ToString()));
            }
            else
            {
                itemTrilhaParticipacao = new classes.ItemTrilhaParticipacao();
            }

            //Arquivo de Envio
            if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null && fupldArquivoEnvio.PostedFile.ContentLength > 0)
            {

                try
                {

                    string diretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    string nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
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

                    if (itemTrilhaParticipacao.FileServer == null)
                        itemTrilhaParticipacao.FileServer = new FileServer();

                    itemTrilhaParticipacao.FileServer.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                    itemTrilhaParticipacao.FileServer.NomeDoArquivoOriginal = fupldArquivoEnvio.FileName;
                    itemTrilhaParticipacao.FileServer.TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType;


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
                itemTrilhaParticipacao.FileServer = null;
            }



            //Texto Participacao
            itemTrilhaParticipacao.TextoParticipacao = txtTextoParticipacao.Text.Trim();

            // }

            //Item Trilha
            if (!string.IsNullOrWhiteSpace(this.ddlItemTrilha.SelectedItem.Value))
            {
                itemTrilhaParticipacao.ItemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(int.Parse(this.ddlItemTrilha.SelectedItem.Value));
            }

            //Usuário Trilha
            if (Request["Id"] != null)
                itemTrilhaParticipacao.UsuarioTrilha = manterItemTrilhaParticipacao.ObterUsuarioTrilha(int.Parse(hdfIdUsuarioTrilha.Value));
            else
                itemTrilhaParticipacao.UsuarioTrilha = manterItemTrilhaParticipacao.ObterUsuarioTrilha(int.Parse(this.ddlTrilhaNivel.SelectedItem.Value), int.Parse(ddlNomeAluno.SelectedItem.Value));

            return itemTrilhaParticipacao;
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
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<Trilha> ListaTrilhas = manterItemTrilhaParticipacao.ObterTrilhas();
            WebFormHelper.PreencherLista(ListaTrilhas, this.ddlTrilha, false, true);
        }

        private void PreencherComboTrilhasNivel(Trilha trilha)
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<classes.TrilhaNivel> ListaTrilhasNivel = manterItemTrilhaParticipacao.ObterTrilhasNivelPorTrilha(trilha);
            WebFormHelper.PreencherLista(ListaTrilhasNivel, this.ddlTrilhaNivel, false, true);
        }

        private void PreencherComboAlunos(int idTrilha, int idTrilhaNivel)
        {
            ManterMatriculaTrilha manterMatriculaTrilha = new ManterMatriculaTrilha();
            IList<Usuario> ListaUsuarios = manterMatriculaTrilha.ObterPorTrilhaTrilhaNivel(idTrilha, idTrilhaNivel);
            WebFormHelper.PreencherLista(ListaUsuarios, this.ddlNomeAluno, false, true);
        }

        private void PreencherComboTopicoTematico(int idTrilha, int idTrilhaNivel)
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<TrilhaTopicoTematico> ListaTopicoTematico = manterItemTrilhaParticipacao.ObterTopicosTematicosPorTrilhaNivel(idTrilha, idTrilhaNivel);
            WebFormHelper.PreencherLista(ListaTopicoTematico, this.ddlTopicoTematico, false, true);
        }

        private void PreencherComboItemTrilha(ViewTrilha filtro)
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<ItemTrilha> ListaItemTrilha = manterItemTrilhaParticipacao.ObterItemsTrilha(filtro);
            WebFormHelper.PreencherLista(ListaItemTrilha, this.ddlItemTrilha, false, true);
        }

        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Request["Id"] = null;
            Response.Redirect("ListarItemTrilhaParticipacao.aspx");
        }

        protected void ddlTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                Trilha trilha = new Trilha() { ID = int.Parse(ddlTrilha.SelectedItem.Value) };
                PreencherComboTrilhasNivel(trilha);
            }
        }

        protected void ddlTrilhaNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value)) &&
                (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value)))
            {

                int idTrilha = int.Parse(ddlTrilha.SelectedItem.Value);
                int idTrilhaNivel = int.Parse(ddlTrilhaNivel.SelectedItem.Value);

                PreencherComboAlunos(idTrilha, idTrilhaNivel);
                PreencherComboTopicoTematico(idTrilha, idTrilhaNivel);
            }

            else
            {
                ddlNomeAluno.Items.Clear();
            }

        }

        protected void ddlNomeAluno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value))
            {
                ViewTrilha viewTrilha = ObterObjetoViewTrilha();
                PreencherComboItemTrilha(viewTrilha);
            }
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

        protected void lkbArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                BaixarArquivo();
            }
            catch (FileNotFoundException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

    }
}