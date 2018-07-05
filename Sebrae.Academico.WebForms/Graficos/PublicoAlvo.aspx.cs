using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.BP;
using System.Globalization;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class PublicoAlvo : System.Web.UI.Page
    {
        int height = 180;
        int width = 625;
        int xInicial = 15;
        int yInicial = 15;
        int larguraBox = 110;
        int alturaBox = 95;
        int alturaBoxInterno = 40;
        int distancia = 12;
        List<SolidBrush> Cores;

        protected void Page_Load(object sender, EventArgs e)
        {
            CarregarCores();
            MontarGrafico();
        }

        private void MontarGrafico()
        {
            int xAtual = xInicial;
            int yAtual = yInicial;

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, width, height));

            Font font12 = new Font("Calibri", 12);
            Font font14 = new Font("Calibri", 14);
            Brush bruBranco = Brushes.White;
            Brush bruPreto = Brushes.Black;

            //Box 1
            List<DTOPublicoAlvo> lista = new ManterDashBoard().ObterTotalPublicoAlvo().ToList();
            int contador = 0;
            int total = 0;
            foreach (var registro in lista)
            {
                MontarBox(xAtual, yAtual, g, font12, font14, bruBranco, bruPreto, registro.Publico, registro.Quantidade.ToString("0,0", CultureInfo.CreateSpecificCulture("pt-BR")), contador);
                total += registro.Quantidade;
                xAtual += larguraBox + distancia;
                contador++;
            }

            Rectangle boxTotal = new Rectangle(xInicial, yInicial + alturaBox + distancia, ((larguraBox + distancia) * contador)-distancia, 30);
            g.FillRectangle(Cores[Cores.Count - 1], boxTotal);
            using (var sf = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            })
            {
                g.DrawString("Total: " + total.ToString("0,0", CultureInfo.CreateSpecificCulture("pt-BR")), font14, bruBranco, boxTotal, sf);
            }

            Response.Clear();
            Response.ContentType = "image/jpeg";

            Encoder quality = Encoder.Quality;
            var ratio = new EncoderParameter(quality, 85L);
            var codecParams = new EncoderParameters(1);
            codecParams.Param[0] = ratio;
            bmp.Save(Response.OutputStream, GetEncoder(ImageFormat.Jpeg), codecParams);

            bmp.Save(Response.OutputStream, ImageFormat.Bmp);
            g.Dispose();
            bmp.Dispose();
            Response.End();
        }

        private void MontarBox(int xAtual, int yAtual, Graphics g, Font font1, Font font2, Brush bruBranco, Brush bruPreto, String textobox, String valorbox, int cor)
        {
            Rectangle boxSuperior = new Rectangle(xAtual, yAtual, larguraBox, alturaBoxInterno);
            g.FillRectangle(Cores[cor * 2], boxSuperior);
            Rectangle boxInferior = new Rectangle(xAtual, yAtual + alturaBoxInterno, larguraBox, alturaBox - alturaBoxInterno);
            g.FillRectangle(Cores[(cor * 2) + 1], boxInferior);
            using (var sf = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            })
            {
                g.DrawString(textobox, font1, bruBranco, boxSuperior, sf);
                g.DrawString(valorbox, font2, bruPreto, boxInferior, sf);
            }
        }



        protected void CarregarCores()
        {
            Cores = new List<SolidBrush>();
            Cores.Add(new SolidBrush(Color.FromArgb(255, 192, 80, 77))); //Vermelho Escuro
            Cores.Add(new SolidBrush(Color.FromArgb(255, 234, 213, 213))); //Vermelho Claro

            Cores.Add(new SolidBrush(Color.FromArgb(255, 155, 187, 89))); //Verde Escuro
            Cores.Add(new SolidBrush(Color.FromArgb(255, 225, 233, 214))); //Verde Claro

            Cores.Add(new SolidBrush(Color.FromArgb(255, 128, 100, 162))); //Roxo Escuro
            Cores.Add(new SolidBrush(Color.FromArgb(255, 220, 215, 227))); //Roxo Claro

            Cores.Add(new SolidBrush(Color.FromArgb(255, 75, 172, 198))); //Azul/Verde Escuro
            Cores.Add(new SolidBrush(Color.FromArgb(255, 213, 230, 236))); //Azul/Verde Claro

            Cores.Add(new SolidBrush(Color.FromArgb(255, 247, 150, 70))); //Laranja Escuro
            Cores.Add(new SolidBrush(Color.FromArgb(255, 252, 224, 213))); //Laranja Claro

            Cores.Add(new SolidBrush(Color.FromArgb(255, 79, 129, 189))); //Azul
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;
            return null;
        }
    }
}