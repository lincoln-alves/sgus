using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sebrae.Academico.BP;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Summary description for FileServer
    /// </summary>
    public class ExibirFileServer : IHttpHandler
    {
        private FileServer contentMediaServer = null;
        private ManterFileServer manterFileServer = new ManterFileServer();

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
                context.Response.Clear();
                FileServer fs = new FileServer();
                fs.NomeDoArquivoNoServidor = Identificador;
                fs.MediaServer = false;
                contentMediaServer = manterFileServer.ObterFileServerPorFiltro(fs).FirstOrDefault();

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

                            // Removendo caracteres especiais que travam no chrome.
                            contentMediaServer.NomeDoArquivoOriginal =
                                RemoverCaracterEspecial(contentMediaServer.NomeDoArquivoOriginal);
                            //context.Response.AddHeader("Content-Disposition", "attachment; filename=" + contentMediaServer.NomeDoArquivoOriginal);
                            context.Response.ContentType = contentMediaServer.TipoArquivo;
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

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static string RemoverCaracterEspecial(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}