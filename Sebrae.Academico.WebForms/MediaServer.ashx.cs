using System;
using System.Web;
using Sebrae.Academico.BP;
using System.IO;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Web.Routing;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Summary description for MediaServer
    /// </summary>
    public class MediaServer : IHttpHandler
    {
        private FileServer contentMediaServer = null;
        private ManterFileServer manterFileServer = new ManterFileServer();

        public void ProcessRequest(HttpContext context)
        {
            int Identificador = 0;
            RequestContext requestContext = context.Request.RequestContext;

            // Utilizado para obter id de requests por querystring e por sistema de rotas
            if(!int.TryParse(requestContext.RouteData.Values["Id"] as string, out Identificador))
            {
                if (context.Request.QueryString["Identificador"] == null || !int.TryParse(context.Request.QueryString["Identificador"].ToString(), out Identificador))
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Acesso negado!");
                    return;
                }
            }

            if (Identificador > 0)
            {
                context.Response.Clear();
                contentMediaServer = manterFileServer.ObterFileServerPorID(Identificador);

                if (contentMediaServer != null)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (FileStream file = new FileStream(string.Concat(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro, @"\", contentMediaServer.NomeDoArquivoNoServidor), FileMode.Open, FileAccess.Read))
                            {
                                byte[] bytes = new byte[file.Length];
                                file.Read(bytes, 0, (int)file.Length);
                                ms.Write(bytes, 0, (int)file.Length);
                            }
                            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + contentMediaServer.NomeDoArquivoOriginal);
                            context.Response.ContentType = contentMediaServer.TipoArquivo;
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