using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Summary description for ExibirThumbnail
    /// </summary>
    public class ExibirThumbnail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string Identificador;
            if (context.Request.QueryString["Identificador"] == null)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Acesso negado!");
                return;
            }

            Identificador = context.Request.QueryString["Identificador"].ToString();
            if (!string.IsNullOrEmpty(Identificador))
            {
                Type tipoArquivo;
                context.Response.Clear();
                //FileServer fs = new FileServer();
                //fs.NomeDoArquivoNoServidor = Identificador;
                //fs.MediaServer = false;
                //contentMediaServer = manterFileServer.ObterFileServerPorFiltro(fs).FirstOrDefault();

                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream file = new FileStream(string.Concat(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro, @"\",
                            Identificador), FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);
                            tipoArquivo = file.GetType();
                        }

                        // Removendo caracteres especiais que travam no chrome.
                        //contentMediaServer.NomeDoArquivoOriginal =
                        //    RemoverCaracterEspecial(contentMediaServer.NomeDoArquivoOriginal);

                        //context.Response.AddHeader("Content-Disposition", "attachment; filename=" + contentMediaServer.NomeDoArquivoOriginal);
                        context.Response.ContentType = tipoArquivo.Name;
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.BinaryWrite(ms.ToArray());
                        context.Response.Flush();
                    }
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Erro filetype 1 - " + ex.ToString());
                    return;
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}