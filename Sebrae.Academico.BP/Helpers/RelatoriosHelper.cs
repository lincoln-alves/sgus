using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Helpers
{
    public static class RelatoriosHelper
    {
        private static Thread _thread;

        /// <summary>
        /// Executar uma consulta de relatório em threading e enviar para a tela de Solicitações de relatórios via FileServer.
        /// Este método é um wrap do método de datatable com menos parâmetros.
        /// </summary>
        /// <param name="dt">Objeto de dados do relatório</param>
        /// <param name="saida">Tipo de saída do relatório</param>
        /// <param name="items">Itens a serem exibidos pelo ReportViewer</param>
        /// <param name="nomeRelatorio">Nome do namespace do arquivo rpt do relatório</param>
        /// <param name="nomeAmigavel">Nome amigável do relatório</param>
        /// <param name="quantidadeRegistro">Quantidade de registros do relatório</param>
        /// <param name="totalizador">Objeto com totalizador</param>
        public static void ExecutarThreadSolicitacaoRelatorio(object dt, enumTipoSaidaRelatorio saida,
            ListItemCollection items, string nomeRelatorio, string nomeAmigavel, int quantidadeRegistro,
            object totalizador = null)
        {
            // Obter o caminho do arquivo com um nome aleatório.
            var caminhoDiretorioUpload =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

            var nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();

            var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                nomeAleatorioDoArquivoParaUploadCriptografado);

            ExecutarThreadSolicitacaoRelatorio(
                dt,
                saida,
                items,
                nomeRelatorio,
                nomeAmigavel,
                diretorioDeUploadComArquivo,
                nomeAleatorioDoArquivoParaUploadCriptografado,
                new ManterUsuario().ObterUsuarioLogado(),
                new ManterSolicitacaoRelatorio(),
                new ManterFileServer(),
                quantidadeRegistro,
                totalizador);

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Seu relatório está sendo gerado e deve aparecer no menu de solicitações de relatórios em breve. O menu fica em \"SOLICITAÇÕES DE RELATÓRIOS > Histórico de solicitações\".");
        }

        /// <summary>
        /// Executar uma consulta de relatório em threading e enviar para a tela de Solicitações de relatórios via FileServer.
        /// Este método é um wrap do método para requisição em URL com menos parâmetros.
        /// </summary>
        /// <param name="requestUrl">URl da requisição</param>
        /// <param name="saida">Tipo de saída do relatório</param>
        /// <param name="nomeRelatorio">Nome técnico do relatório</param>
        /// <param name="nomeAmigavel">Nome amigável do relatório</param>
        /// <param name="quantidadeRegistro">Quantidade de registros do relatório</param>
        public static void ExecutarThreadSolicitacaoRelatorioRequisicao(string requestUrl, enumTipoSaidaRelatorio saida,
            string nomeRelatorio, string nomeAmigavel, int quantidadeRegistro)
        {
            // Obter o caminho do arquivo com um nome aleatório.
            var caminhoDiretorioUpload =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

            var nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();

            var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                nomeAleatorioDoArquivoParaUploadCriptografado);

            // Ler requisição a partir de uma URL informada.
            var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);

            ExecutarThreadSolicitacaoRelatorioRequisicao(
                webRequest,
                saida,
                nomeRelatorio,
                nomeAmigavel,
                diretorioDeUploadComArquivo,
                nomeAleatorioDoArquivoParaUploadCriptografado,
                new ManterUsuario().ObterUsuarioLogado(),
                new ManterSolicitacaoRelatorio(),
                new ManterFileServer(),
                quantidadeRegistro);

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Seu relatório está sendo gerado e deve aparecer no menu de solicitações de relatórios em breve. O menu fica em \"SOLICITAÇÕES DE RELATÓRIOS > Histórico de solicitações\".");
        }

        private static void ExecutarThreadSolicitacaoRelatorio(object dt, enumTipoSaidaRelatorio saida,
            ListItemCollection items, string nomeRelatorio, string nomeAmigavel, string diretorioDeUploadComArquivo,
            string nomeAleatorioDoArquivoParaUploadCriptografado, Usuario usuario,
            ManterSolicitacaoRelatorio manterSolicitacao, ManterFileServer manterFileServer, int quantidadeRegistro,
            object totalizador = null)
        {
            _thread = new Thread(() =>
            {
                var solicitacao = new SolicitacaoRelatorio
                {
                    DataSolicitacao = DateTime.Now,
                    Nome = nomeRelatorio,
                    NomeAmigavel = nomeAmigavel,
                    Saida = saida.ToString(),
                    Usuario = usuario,
                    QuantidadeRegistros = quantidadeRegistro
                };

                // Salvar pedido inicial, para consulta do status da solicitação do relatório.
                manterSolicitacao.Salvar(solicitacao);

                try
                {
                    dynamic list = dt;
                    dt = DataUtil.ToDataTable(list);
                    var rv = WebFormHelper.GerarRelatorio(nomeRelatorio, dt, items, totalizador);

                    // Obtém o arquivo. Super lento em relatórios grandes. Por isso está em uma Thread dããã, senhor óbvio
                    var arquivoBytes = rv.LocalReport.Render(saida.ToString());

                    // Escrever o arquivo na pasta.
                    File.WriteAllBytes(diretorioDeUploadComArquivo, arquivoBytes);

                    FinalizarSolicitacaoRelatorio(nomeAmigavel, nomeAleatorioDoArquivoParaUploadCriptografado,
                        manterSolicitacao, manterFileServer, solicitacao);

                    Thread.CurrentThread.Abort();
                }
                catch (ThreadAbortException)
                {
                    // ignored
                }
                catch (Exception ex)
                {
                    // Informa que houve falha na geração do relatório.
                    solicitacao.Falha = true;
                    solicitacao.Descricao = ex.ToString();
                    manterSolicitacao.Salvar(solicitacao);

                    _thread.Abort();
                }
            })
            {
                IsBackground = true
            };

            // Let the chaos COMMENCE!
            _thread.Start();
        }

        private static void ExecutarThreadSolicitacaoRelatorioRequisicao(HttpWebRequest webRequest,
            enumTipoSaidaRelatorio saida, string nomeRelatorio, string nomeAmigavel, string diretorioDeUploadComArquivo,
            string nomeAleatorioDoArquivoParaUploadCriptografado, Usuario usuario,
            ManterSolicitacaoRelatorio manterSolicitacao, ManterFileServer manterFileServer, int quantidadeRegistro)
        {
            _thread = new Thread(() =>
            {
                var solicitacao = new SolicitacaoRelatorio
                {
                    DataSolicitacao = DateTime.Now,
                    Nome = nomeRelatorio,
                    NomeAmigavel = nomeAmigavel,
                    Saida = saida.ToString(),
                    Usuario = usuario,
                    QuantidadeRegistros = quantidadeRegistro
                };

                // Salvar pedido inicial, para consulta do status da solicitação do relatório.
                manterSolicitacao.Salvar(solicitacao);

                try
                {
                    var myReq = webRequest;

                    var myResp = myReq.GetResponse();

                    using (var stream = myResp.GetResponseStream())
                    {
                        if (stream == null)
                            throw new Exception(
                                "Stream nulo não pode ser utilizado para ler o relatório. Tente novamente.");

                        using (var ms = new MemoryStream())
                        {
                            int count;

                            do
                            {
                                var buf = new byte[1024];
                                count = stream.Read(buf, 0, 1024);
                                ms.Write(buf, 0, count);
                            } while (stream.CanRead && count > 0);

                            // Escrever o arquivo na pasta. É aqui que a mágica acontece.
                            File.WriteAllBytes(diretorioDeUploadComArquivo, ms.ToArray());
                        }
                    }

                    FinalizarSolicitacaoRelatorio(nomeAmigavel, nomeAleatorioDoArquivoParaUploadCriptografado,
                        manterSolicitacao, manterFileServer, solicitacao);

                    Thread.CurrentThread.Abort();

                }
                catch (ThreadAbortException)
                {
                    // ignored
                }
                catch (Exception ex)
                {
                    // Informa que houve falha na geração do relatório.
                    solicitacao.Descricao = "Erro ao escrever " + diretorioDeUploadComArquivo + " - Mensagem de erro: " + ex;
                    solicitacao.Falha = true;
                    manterSolicitacao.Salvar(solicitacao);

                    _thread.Abort();
                }
            })
            {
                IsBackground = true,
                Name = nomeRelatorio + Guid.NewGuid()
            };

            // Let the chaos COMMENCE!
            _thread.Start();
        }

        private static void FinalizarSolicitacaoRelatorio(string nomeAmigavel,
            string nomeAleatorioDoArquivoParaUploadCriptografado, ManterSolicitacaoRelatorio manterSolicitacao,
            ManterFileServer manterFileServer, SolicitacaoRelatorio solicitacao)
        {
            // Obtém o MIME da Request.
            var mime = "";
            var extensao = "";

            switch (solicitacao.ObterSaidaEnum())
            {
                case enumTipoSaidaRelatorio.PDF:
                    mime = "application/pdf";
                    extensao = "pdf";
                    break;
                case enumTipoSaidaRelatorio.WORD:
                    mime = "application/msword";
                    extensao = "doc";
                    break;

                case enumTipoSaidaRelatorio.EXCEL:
                    mime = "application/vnd.ms-excel";
                    extensao = "xls";
                    break;
            }

            var nomeOriginalArquivo =
                nomeAmigavel
                + "_"
                + solicitacao.DataSolicitacao.ToShortDateString()
                + "_"
                + solicitacao.DataSolicitacao.Hour.ToString().PadLeft(2, '0')
                + solicitacao.DataSolicitacao.Minute.ToString().PadLeft(2, '0')
                + "."
                + extensao;

            nomeOriginalArquivo = nomeOriginalArquivo.Replace(" ", "_");

            // Prepara o cadastro do arquivo na tabela do Media Server.
            var fileServer = new FileServer
            {
                NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado,
                NomeDoArquivoOriginal = nomeOriginalArquivo,
                TipoArquivo = mime,
                MediaServer = true
            };

            // Salva no FileServer.
            manterFileServer.IncluirFileServer(fileServer);
            solicitacao.Arquivo = fileServer;
            solicitacao.DataGeracao = DateTime.Now;

            // Salvar novamente, com o arquivo.
            manterSolicitacao.Salvar(solicitacao);
        }
    }
}