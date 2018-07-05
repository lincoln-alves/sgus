using System;
using System.Web;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucUpload : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If first time page is submitted and we have file in FileUpload control but not in session 
            // Store the values to SEssion Object 
            if (Session["FileUpload1"] == null && fupldImagemEnviada.HasFile)
            {
                Session["FileUpload1"] = fupldImagemEnviada;
                //Label1.Text = FileUpload1.FileName;
            }
            // Next time submit and Session has values but FileUpload is Blank 
            // Return the values from session to FileUpload 
            else if (Session["FileUpload1"] != null && (!fupldImagemEnviada.HasFile))
            {
                fupldImagemEnviada = (FileUpload)Session["FileUpload1"];
                //Label1.Text = FileUpload1.FileName;
            }
            // Now there could be another sictution when Session has File but user want to change the file 
            // In this case we have to change the file in session object 
            else if (fupldImagemEnviada.HasFile)
            {
                Session["FileUpload1"] = fupldImagemEnviada;
                //Label1.Text = FileUpload1.FileName;
            }
        }

        public void DefinirTextoParaoLabelDaImagem(string textoParaOLabelDaImagem)
        {
            if (!string.IsNullOrWhiteSpace(textoParaOLabelDaImagem))
            {
                this.lblImagem.Text = textoParaOLabelDaImagem;
            }
        }

        public bool ImagemCampoObrigatorio
        {
            //get
            //{
            //    if (ViewState["CampoObrigatorio"] != null)
            //        return (bool)ViewState["CampoObrigatorio"];
            //    else
            //        return false;
            //}

            set
            {
                ViewState["CampoObrigatorio"] = value;
                this.rfvImagem.Enabled = (bool)ViewState["CampoObrigatorio"];
            }
        }

        /// <summary>
        /// Imagem Enviada.
        /// </summary>
        public string Imagem
        {
            get
            {
                if (ViewState["ViewStateImagem"] != null)
                {
                    return (string)ViewState["ViewStateImagem"];
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ViewStateImagem"] = value;
            }

        }

        public void PrepararExibicaoDaImagemSalva(string pImagem)
        {

            //Imagem
            if (!string.IsNullOrWhiteSpace(pImagem))
            {
                imgImagem.Src = pImagem;
                imgImagem.Visible = true;
                this.Imagem = pImagem;
            }
            //else
            //{
            //    imgImagem.Visible = false;
            //}

            //imgImagem.Visible = true;
        }

        public string ObterImagemFormatada()
        {
            string imagemEnviada = null;

            //Imagem enviada
            if (this.ArquivoFoiEnviado)
            {

                if (fupldImagemEnviada != null && fupldImagemEnviada.PostedFile != null 
                    && fupldImagemEnviada.PostedFile.ContentLength > 0)
                {

                    var imagem = fupldImagemEnviada.PostedFile.InputStream;

                    string imagemConvertidaEmBase64String = CommonHelper.ObterBase64String(imagem);

                    string informacoesDoArquivoParaBase64 = CommonHelper.GerarInformacoesDoArquivoParaBase64(fupldImagemEnviada);

                    //certificadoTemplate.OBImagem = string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);
                    imagemEnviada = string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);

                    this.Imagem = imagemEnviada;

                }
                else
                {
                    imagemEnviada = this.Imagem;
                }
            }

            return imagemEnviada;

        }

        public bool ArquivoFoiEnviado
        {

            get
            {

                bool arquivoFoienviado = false;

                if (fupldImagemEnviada != null && fupldImagemEnviada.PostedFile != null
                   && fupldImagemEnviada.PostedFile.ContentLength > 0)
                {
                    arquivoFoienviado = true;
                }
                else
                {
                    /* Se imagem vazia, significa que o usuário não subiu uma imagem ou então
                        houve um postback e o controle fileupload não guardou a imagem enviada.
                        Apesar disso, verifica se a imagem foi persistida no user control de upload */
                    if (!string.IsNullOrWhiteSpace(this.Imagem))
                    {
                        arquivoFoienviado = true;
                    }
                }

                return arquivoFoienviado;
            }
        }

       

        //protected void cvImagem_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        //{
        //    if (this.ImagemCampoObrigatorio)
        //    {
        //        if (!this.ArquivoFoiEnviado)
        //        {
        //            this.cvImagem.ErrorMessage = "Imagem. Campo Obrigatório";
        //            args.IsValid = false;
        //        }
        //        else
        //        {
        //            args.IsValid = true;
        //        }
        //    }
        //}
    }
}