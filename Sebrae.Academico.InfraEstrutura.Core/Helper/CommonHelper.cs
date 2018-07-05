using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MySql.Data.MySqlClient;
using NHibernate.Collection;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;
using NHibernate.Collection.Generic;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class CommonHelper
    {


        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["cnxSebraeAcademico"].ConnectionString; } }

        public static string MysqlConnectionString { get { return ConfigurationManager.ConnectionStrings["cnxMoodle"].ConnectionString; } }

        public static string GerarInformacoesDoArquivoParaBase64(FileUpload fileUpload)
        {
            string tipoDoArquivo = fileUpload.PostedFile.ContentType;
            string informacaoParaGravacao = string.Format("data:{0};base64", tipoDoArquivo);
            return informacaoParaGravacao;
        }

        public static string FormataHora(string valor, bool inicio)
        {
            if (string.IsNullOrEmpty(valor)) return valor;
            var dados = valor.Split(':');
            var h = int.Parse(dados[0]);
            var m = int.Parse(dados[1]);
            var s = int.Parse(dados[2]);
            if (h > 23) h = 23;
            if (m > 59) m = 59;
            if (s > 59) s = 59;
            return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
        }

        public static string FormataDataHora(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return valor;
            var dt = valor.Split(' ');
            dt[1] = FormataHora(dt[1].Replace("_", "0"), true);
            return dt[0] + " " + dt[1];
        }

        public static DateTime? TratarData(string dataInformada, string campo)
        {
            var data = DateTime.MinValue;

            if (!string.IsNullOrWhiteSpace(dataInformada))
            {
                if (!DateTime.TryParse(dataInformada.Trim(), out data))
                    throw new AcademicoException(string.Format("Valor inválido para o Campo {0}", campo));
            }

            return !data.Equals(DateTime.MinValue) ? data : (DateTime?)null;

        }

        public static DateTime TratarDataObrigatoria(string dataInformada, string campo)
        {
            DateTime data = DateTime.MinValue;


            if (!string.IsNullOrWhiteSpace(dataInformada))
            {
                if (!DateTime.TryParse(dataInformada.Trim(), out data))
                    throw new AcademicoException(string.Format("Valor Inválido para o Campo {0}", campo));

            }
            else
            {
                throw new AcademicoException(string.Format("Valor Inválido para o Campo {0}", campo));
            }

            return data;

        }

        private static byte[] ConverterStreamEmArrayDeBytes(Stream stream)
        {
            var imagem = stream;
            byte[] imagemEmArrayDeBytes = null;

            using (var memoryStream = new MemoryStream())
            {
                imagem.CopyTo(memoryStream);
                imagemEmArrayDeBytes = memoryStream.ToArray();
            }

            return imagemEmArrayDeBytes;
        }

        public static string ObterBase64String(Stream stream)
        {
            byte[] imagemEmArrayDeBytes = ConverterStreamEmArrayDeBytes(stream);
            string base64String = ConverterArrayDeBytesEmBase64String(imagemEmArrayDeBytes);
            return base64String;
        }

        private static string ConverterArrayDeBytesEmBase64String(byte[] ImagemEmArrayDeBytes)
        {
            // Convert byte[] to Base64 String
            return Convert.ToBase64String(ImagemEmArrayDeBytes);
        }

        public static string ObterTipoDoArquivo(string base64string)
        {
            //Ex: data:image/png;base64,iVBO
            string[] tipoDoArquivo = base64string.Split(new char[] { ':', ';' });
            string tipoArquivo = tipoDoArquivo[1];
            return tipoArquivo;
        }

        public static string ObterLinkParaArquivoDeImagem(string caminhoServidor, int idFileServer)
        {
            return string.Format(String.Concat(caminhoServidor, "/MediaServer.ashx?Identificador={0}"), idFileServer);
        }

        public static string ObterLinkParaArquivoDeImagem(string caminhoServidor, string nomedoArquivo)
        {
            return string.Format(String.Concat(caminhoServidor, "/ExibirFileServer.ashx?Identificador={0}"), nomedoArquivo);
        }

        public static MemoryStream ObterMemoryStream(string base64string, bool urlSchemeSynthax = true)
        {
            if (urlSchemeSynthax)
            {
                base64string = base64string.Substring(base64string.IndexOf(",") + 1);
            }

            //Transforma a imagem (que está no formato Base64) em um array de bytes.
            byte[] fileBytes = Convert.FromBase64String(base64string);

            MemoryStream ms = new MemoryStream(fileBytes, 0, fileBytes.Length);
            ms.Write(fileBytes, 0, fileBytes.Length);

            return ms;

        }

        public static void SincronizarDominioParaDTO(object pDominioOrigem, object pDTODestino)
        {
            foreach (PropertyInfo po in pDominioOrigem.GetType().GetProperties())
            {

                if (pDTODestino.GetType().GetProperty(po.Name) == null)
                    continue;


                if (po.PropertyType.Name.IndexOf("IList") == -1)
                    try
                    {
                        pDTODestino.GetType().GetProperty(po.Name).SetValue(pDTODestino, po.GetValue(pDominioOrigem, null), null);
                    }
                    catch
                    {
                        var objDTO = pDTODestino.GetType().GetProperty(po.Name).PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);

                        SincronizarDominioParaDTO(po.GetValue(pDominioOrigem, null), objDTO);

                        pDTODestino.GetType().GetProperty(po.Name).SetValue(pDTODestino, objDTO, null);

                    }
                else
                {

                    IList list1;

                    object obj = po.GetValue(pDominioOrigem, null);

                    list1 = (IList)obj;

                    if (list1 == null || list1.Count == 0)
                        continue;


                    var dtoType = pDTODestino.GetType().GetProperty(po.Name).PropertyType;

                    IList list2 = pDTODestino.GetType().GetProperty(po.Name).PropertyType.GetGenericTypeDefinition()
                                  .MakeGenericType(dtoType.GetGenericArguments())
                                  .GetConstructor(Type.EmptyTypes)
                                  .Invoke(null) as IList;

                    foreach (object obj1 in list1)
                    {
                        var dtoType2 = list2.GetType();
                        var obj2 = dtoType2.GetGenericArguments()[0].GetConstructor(Type.EmptyTypes).Invoke(null);

                        SincronizarDominioParaDTO(obj1, obj2);

                        list2.Add(obj2);

                    }

                    pDTODestino.GetType().GetProperty(po.Name).SetValue(pDTODestino, list2, null);
                }
            }
        }



        public static FileStream ObterArquivo(string pNomeArquivo)
        {
            return File.Open(HttpContext.Current.Server.MapPath("~") + pNomeArquivo, FileMode.Open);
        }

        public static void EnviarEmail(string destinatario, string assunto, string mensagem,
            string emailRemetente, string nomeRemetente, SmtpClient sc, List<KeyValuePair<string, string>> linkedResources = null)
        {

            if (sc == null)
                throw new Exception("Nenhum servidor de E-mail Foi encontrado. Redefina as configurações de e-mail.");

            var m = new MailMessage
            {
                Body = mensagem,
                Subject = assunto,
                From =
                    new MailAddress(emailRemetente,
                        string.IsNullOrEmpty(nomeRemetente) ? emailRemetente : nomeRemetente)
            };

            if (linkedResources != null && linkedResources.Any())
            {
                var htmlView = AlternateView.CreateAlternateViewFromString(mensagem, System.Text.Encoding.UTF8, "text/html");

                foreach (var linkedResource in linkedResources)
                {
                    var theEmailImage = new LinkedResource(linkedResource.Value) { ContentId = linkedResource.Key };

                    //Add the Image to the Alternate view
                    htmlView.LinkedResources.Add(theEmailImage);
                }


                m.AlternateViews.Add(htmlView);
            }


            var emails = destinatario.Split(',');

            foreach (var email in emails)
                m.To.Add(new MailAddress(email.Trim()));

            m.IsBodyHtml = true;
            sc.Send(m);
        }

        public static byte[] ConverterStringToByteArray(string pStr)
        {
            byte[] bytes = new byte[pStr.Length * sizeof(char)];
            Buffer.BlockCopy(pStr.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string ObterStatusMatriculaPorExtenso(enumStatusMatricula statusMatricula)
        {

            string statusMatriculaPorExtenso = string.Empty;

            switch (statusMatricula)
            {
                case enumStatusMatricula.Inscrito:
                    statusMatriculaPorExtenso = "Inscrito";
                    break;
                case enumStatusMatricula.CanceladoAluno:
                    statusMatriculaPorExtenso = "Cancelado Aluno";
                    break;
                case enumStatusMatricula.CanceladoAdm:
                    statusMatriculaPorExtenso = "Cancelado Atendimento";
                    break;
                case enumStatusMatricula.Abandono:
                    statusMatriculaPorExtenso = "Abandono";
                    break;
                case enumStatusMatricula.PendenteConfirmacaoAluno:
                    statusMatriculaPorExtenso = "Pendente de Confirmação Aluno";
                    break;
                case enumStatusMatricula.Concluido:
                    statusMatriculaPorExtenso = "Concluído";
                    break;
                case enumStatusMatricula.FilaEspera:
                    statusMatriculaPorExtenso = "Fila de Espera";
                    break;
                case enumStatusMatricula.Aprovado:
                    statusMatriculaPorExtenso = "Aprovado";
                    break;
                case enumStatusMatricula.Reprovado:
                    statusMatriculaPorExtenso = "Reprovado";
                    break;
            }

            return statusMatriculaPorExtenso;

        }



        public static byte[] GerarRelatorioParaWebServices(string pCaminhoArquivoRLDC, object dt)
        {
            ReportViewer rv = new ReportViewer();
            rv.LocalReport.ReportPath = pCaminhoArquivoRLDC;
            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            rv.LocalReport.Refresh();

            return rv.LocalReport.Render("PDF");
        }

        public static void EnviarArquivoParaRepositorio(string caminhoDiretorioUpload, MemoryStream memoryStream, string nomeArquivoNoServidor)
        {
            var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                nomeArquivoNoServidor);

            File.WriteAllBytes(diretorioDeUploadComArquivo, memoryStream.ToArray());
        }

        public static decimal CalcularPercentualDaProva(QuestionarioParticipacao qp)
        {
            decimal totalPontosPossiveis = 0;
            decimal totalPontosConseguidos = 0;
            foreach (ItemQuestionarioParticipacao iqp in qp.ListaItemQuestionarioParticipacao)
            {
                totalPontosPossiveis += iqp.ValorQuestao == null ? 0 : iqp.ValorQuestao.Value;
                totalPontosConseguidos += iqp.ValorAtribuido == null ? 0 : iqp.ValorAtribuido.Value;
            }

            decimal percObtido = totalPontosPossiveis == 0 ? 0 : totalPontosConseguidos / totalPontosPossiveis * 10;

            return percObtido;
        }

        public static string FormatarTextoRecuperacaoSenhaSemConfirmacao(string nomeUsuario, string email, string senha, string urlServidor,
                                                                         string templateMensagemRecuperacaoDeSenhaSemConfirmacao)
        {
            string textoEmailFormatado = string.Empty;

            textoEmailFormatado = templateMensagemRecuperacaoDeSenhaSemConfirmacao.Replace("#ALUNO#", nomeUsuario)
                                                                                  .Replace("#Email#", email)
                                                                                  .Replace("#SENHA#", senha)
                                                                                  .Replace("#URLSERVIDOR#", urlServidor)
                                                                                  .Replace("#DATAHORA#", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm"));



            return textoEmailFormatado;
        }

        public static string FormatarTextoRecuperacaoSenhaComConfirmacao(SolicitacaoSenha solicitacaoSenha, string urlServidor,
                                                                         string templateMensagemRecuperacaoDeSenha)
        {
            string textoEmailFormatado = string.Empty;

            textoEmailFormatado = templateMensagemRecuperacaoDeSenha.Replace("#ALUNO#", solicitacaoSenha.Usuario.Nome)
                                                                    .Replace("#TOKEN#", solicitacaoSenha.Token)
                                                                    .Replace("#URLSERVIDOR#", urlServidor)
                                                                    .Replace("#DATAHORA#", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm"));


            return textoEmailFormatado;
        }



        public static void GerarArquivoParaDowload(byte[] arradeBytes, string tipoSaida = "PDF")
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            //switch (rblTipoSaida.SelectedValue)

            //Se não informar o tipo de saída, o padrão será um pdf.

            switch (tipoSaida)
            {
                case "PDF":
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    break;
                case "WORD":
                    HttpContext.Current.Response.ContentType = "application/msword";
                    break;

                case "EXCEL":
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    break;
            }
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement; filename=\"teste.pdf\"");
            HttpContext.Current.Response.AddHeader("Content-Length", arradeBytes.Length.ToString());
            HttpContext.Current.Response.OutputStream.Write(arradeBytes, 0, arradeBytes.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static FileServer ObterObjetoFileServer(MemoryStream memoryStream)
        {
            var fileServer = new FileServer
            {
                NomeDoArquivoNoServidor = WebFormHelper.ObterStringAleatoria(),
                MediaServer = true
            };


            return fileServer;
        }

        public static void ExecutarProcedureMysql(string name, IDictionary<string, object> lstParams)
        {
            var cnx = new MySqlConnection(MysqlConnectionString);

            using (MySqlConnection conn = new MySqlConnection(MysqlConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(name, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Se abre la conexión
                    conn.Open();
                    //verificamos si se mando la lista de parámetros
                    if (lstParams.Count > 0)
                    {
                        //existen parámetros los recorremos y agregamos.
                        foreach (KeyValuePair<string, object> pars in lstParams)
                        {
                            cmd.Parameters.Add(new MySqlParameter(pars.Key, pars.Value));
                            cmd.Parameters[pars.Key].Direction = ParameterDirection.Input;
                        }
                    }
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }
    }
}
