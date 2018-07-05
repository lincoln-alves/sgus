using System;
using System.Web.UI;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System.Web;

namespace Sebrae.Academico.WebForms.Cadastros.MediaServer
{
    public partial class EditarMediaServer : Page
    {
        private FileServer _contentMediaServer;
        private ManterFileServer manterFileServer = new ManterFileServer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    var idFileServer = int.Parse(Request["Id"]);
                    _contentMediaServer = manterFileServer.ObterFileServerPorID(idFileServer);
                    PreencherCampos(_contentMediaServer);
                }
            }
        }

        private void PreencherCampos(FileServer fileServer)
        {
            if (fileServer != null)
            {
                imgFile.ImageUrl = ResolveUrl("~/MediaServer.ashx?Identificador=" + fileServer.ID);

                string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                txtUrlReduzida.Text = string.Format("{0}/ms/{1}", baseUrl, fileServer.ID);

                string caminhoArquivoAshx = string.Format(@"MediaServer.ashx?Identificador={1}", HttpContext.Current.Request.Url, fileServer.ID);
                string urlCompleta = HttpContext.Current.Request.Url.ToString();
                urlCompleta = urlCompleta.Replace(HttpContext.Current.Request.RawUrl, string.Concat("/", caminhoArquivoAshx));
                txtUrlCompleta.Text = urlCompleta;

                txtNomeDoArquivoOriginal.Text = fileServer.NomeDoArquivoOriginal;

                dvNomeArquivo.Visible = true;
                dvUrlCompleta.Visible = true;
                dvUrReduzida.Visible = true;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                _contentMediaServer = null;
                _contentMediaServer = ObterObjetoFileServer();

                if (Request["Id"] == null)
                {
                    manterFileServer.IncluirFileServer(_contentMediaServer);
                }
                else
                {
                    manterFileServer.AlterarFileServer(_contentMediaServer);
                }

                //Session.Remove("FileServerEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, string.Format("Dados Gravados com Sucesso! Código gerado: {0}", _contentMediaServer.ID.ToString()), "ListarMediaServer.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        private FileServer ObterObjetoFileServer()
        {
            FileServer mediaServer = null;

            if (Request["Id"] != null)
            {
                mediaServer = new ManterFileServer().ObterFileServerPorID(int.Parse(Request["Id"].ToString()));

                mediaServer.NomeDoArquivoOriginal = txtNomeDoArquivoOriginal.Text;
            }
            else
            {
                mediaServer = new FileServer();               
            }

            if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null &&
               fupldArquivoEnvio.PostedFile.ContentLength > 0)
            {
                try
                {
                    var caminhoDiretorioUpload =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    var nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
                    var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                        nomeAleatorioDoArquivoParaUploadCriptografado);

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

                    mediaServer.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                    mediaServer.NomeDoArquivoOriginal = !String.IsNullOrWhiteSpace(txtNomeDoArquivoOriginal.Text) ? txtNomeDoArquivoOriginal.Text : fupldArquivoEnvio.FileName;
                    mediaServer.TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType;

                    mediaServer.Uf = mediaServer.Uf ?? new ManterUf().ObterUfPorID(
                                                     new ManterUsuario().ObterUsuarioLogado().UF.ID);
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
                if (Request["Id"] == null) 
                    throw new AcademicoException("Nenhum arquivo informado.");                
                
            }

            mediaServer.MediaServer = true;
            return mediaServer;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session.Remove("FileServerEdit");
            Response.Redirect("ListarMediaServer.aspx");
        }
    }
}