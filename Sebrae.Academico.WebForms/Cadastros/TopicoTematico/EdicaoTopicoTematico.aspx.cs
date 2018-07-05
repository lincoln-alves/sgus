using System;
using System.Web.UI;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using System.IO;
using System.Web;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.TopicoTematico
{
    public partial class EdicaoTopicoTematico : System.Web.UI.Page
    {

        private TrilhaTopicoTematico trilhaTopicoTematicoEdicao = null;
        private ManterTrilhaTopicoTematico manterTrilhaTopicoTematico = new ManterTrilhaTopicoTematico();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    int idTrilhaTopicoTematico = int.Parse(Request["Id"].ToString());
                    trilhaTopicoTematicoEdicao = manterTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorID(idTrilhaTopicoTematico);
                    PreencherCampos(trilhaTopicoTematicoEdicao);
                }
            }
        }

        private void PreencherCampos(TrilhaTopicoTematico trilhaTopicoTematico)
        {
            if (trilhaTopicoTematico != null)
            {
                txtNome.Text = trilhaTopicoTematico.Nome;
                txtDescTextoEnvio.Text = trilhaTopicoTematico.DescricaoTextoEnvio;
                txtArqEnvio.Text = trilhaTopicoTematico.DescricaoArquivoEnvio;
                txtQtdMinima.Text = trilhaTopicoTematico.QtdMinimaPontosAtivFormativa.ToString();

                if (!string.IsNullOrWhiteSpace(trilhaTopicoTematico.NomeExibicao))
                {
                    txtNomeExibicao.Text = trilhaTopicoTematico.NomeExibicao;
                }

                //Arquivo de Envio 
                if (trilhaTopicoTematicoEdicao.FileServer != null)
                {
                    if (!string.IsNullOrWhiteSpace(trilhaTopicoTematicoEdicao.FileServer.NomeDoArquivoOriginal))
                    {
                        //lkbArquivo.Text = string.Concat("Abrir arquivo ", trilhaTopicoTematicoEdicao.FileServer.NomeDoArquivoOriginal);
                        lkbArquivo.Visible = true;
                    }
                    else
                    {
                        lkbArquivo.Visible = false;
                    }
                }

                ViewState.Add("iditemTrilhaTopicoTematico", trilhaTopicoTematico.ID);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {

                trilhaTopicoTematicoEdicao = new TrilhaTopicoTematico();

                if (Request["Id"] == null)
                {
                    manterTrilhaTopicoTematico = new ManterTrilhaTopicoTematico();
                    trilhaTopicoTematicoEdicao = ObterObjetoTrilhaTopicoTematico();
                    manterTrilhaTopicoTematico.IncluirTrilhaTopicoTematico(trilhaTopicoTematicoEdicao);
                }
                else
                {
                    trilhaTopicoTematicoEdicao = ObterObjetoTrilhaTopicoTematico();
                    manterTrilhaTopicoTematico.AlterarTrilhaTopicoTematico(trilhaTopicoTematicoEdicao);
                }

                Session.Remove("TrilhaTopicoTematicoEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTopicoTematico.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private TrilhaTopicoTematico ObterObjetoTrilhaTopicoTematico()
        {

            if (Request["Id"] != null)
            {
                trilhaTopicoTematicoEdicao = manterTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorID(int.Parse(Request["Id"].ToString()));
            }
            else
            {
                trilhaTopicoTematicoEdicao = new TrilhaTopicoTematico();
            }

            trilhaTopicoTematicoEdicao.Nome = txtNome.Text.Trim();
            trilhaTopicoTematicoEdicao.DescricaoTextoEnvio = txtDescTextoEnvio.Text.Trim();
            trilhaTopicoTematicoEdicao.DescricaoArquivoEnvio = txtArqEnvio.Text.Trim();
            trilhaTopicoTematicoEdicao.NomeExibicao = txtNomeExibicao.Text.Trim();
            
            if (!string.IsNullOrWhiteSpace(this.txtQtdMinima.Text))
            {
                int qtdMinima = 0;
                if (!int.TryParse(this.txtQtdMinima.Text.Trim(), out qtdMinima))
                    throw new AcademicoException("Valor Inválido para o Campo Quantidade Mínima.");
                else
                    trilhaTopicoTematicoEdicao.QtdMinimaPontosAtivFormativa = qtdMinima;
            }

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
                    if (trilhaTopicoTematicoEdicao.FileServer == null)
                        trilhaTopicoTematicoEdicao.FileServer = new FileServer();

                    trilhaTopicoTematicoEdicao.FileServer.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                    trilhaTopicoTematicoEdicao.FileServer.NomeDoArquivoOriginal = fupldArquivoEnvio.FileName;
                    trilhaTopicoTematicoEdicao.FileServer.TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType;

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

            return trilhaTopicoTematicoEdicao;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("TrilhaTopicoTematicoEdit");
            Response.Redirect("ListarTopicoTematico.aspx");
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

        private void BaixarArquivo()
        {
        
           
            //Obtém o Id do Item Trilha participação do viewstate
            int iditemTrilhaTopicoTematico = ViewState["iditemTrilhaTopicoTematico"] == null ? 0 : (int)ViewState["iditemTrilhaTopicoTematico"];
            

            if (iditemTrilhaTopicoTematico > 0)
            {

                TrilhaTopicoTematico itemTrilhaTopicoTematico = manterTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorID(iditemTrilhaTopicoTematico);

                if (itemTrilhaTopicoTematico != null)
                {
                    string caminhoFisicoDoDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    string caminhoLogicoDoArquivo =  string.Concat(caminhoFisicoDoDiretorioDeUpload, @"\", itemTrilhaTopicoTematico.FileServer.NomeDoArquivoNoServidor);
                    
                    if (!File.Exists(caminhoLogicoDoArquivo))
                        throw new FileNotFoundException("Arquivo não encontrado no servidor!");


                    Response.ContentType = itemTrilhaTopicoTematico.FileServer.TipoArquivo;
                    Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", itemTrilhaTopicoTematico.FileServer.NomeDoArquivoOriginal));
                    HttpContext.Current.Response.TransmitFile(caminhoLogicoDoArquivo);
                    Response.End();
                }

            }

        }



        public int iditemTrilhaTopicoTematico { get; set; }
    }
}