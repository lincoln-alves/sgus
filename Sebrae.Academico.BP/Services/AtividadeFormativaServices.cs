using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System.IO;
using Sebrae.Academico.Extensions;

namespace Sebrae.Academico.BP.Services
{
    public class AtividadeFormativaServices : BusinessProcessServicesBase
    {

        public AtividadeFormativaServices()
            : base()
        {

        }


        public void CadastrarAtividadeFormativa(int idUsuarioTrilha, int idTopicoTematico, string textoParticipacao,
                                                                    string nomeOriginalArquivo, string tipoArquivo, string nomeArquivoServidor,
                                                                    int tipoParticipacao,
                                                                    AuthenticationRequest autenticacao)
        {

            BMTrilhaAtividadeFormativaParticipacao bm = new BMTrilhaAtividadeFormativaParticipacao();
            List<TrilhaAtividadeFormativaParticipacao> ListaParticipacao = new List<TrilhaAtividadeFormativaParticipacao>();
            if (idTopicoTematico > 0 && idUsuarioTrilha > 0)
            {
                ListaParticipacao = bm.ObterParticipacoesUsuarioTrilha(idTopicoTematico, idUsuarioTrilha);
                if (ListaParticipacao.Count > 0)
                {
                    if (ListaParticipacao.Any(x => x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && x.Autorizado.HasValue && x.Autorizado.Value) && tipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro)
                    {
                        throw new AcademicoException("Sua atividade já está aprovada!");
                    }
                    if (ListaParticipacao.Any(x => x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && !x.Autorizado.HasValue) && tipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro)
                    {
                        throw new AcademicoException("Sua atividade ainda não foi avaliada!");
                    }
                }

                TrilhaAtividadeFormativaParticipacao novoItem = new TrilhaAtividadeFormativaParticipacao();
                novoItem.TrilhaTopicoTematico = (new BMTrilhaTopicoTematico()).ObterPorID(idTopicoTematico);
                novoItem.TextoParticipacao = textoParticipacao;
                novoItem.DataEnvio = DateTime.Now;
                novoItem.UsuarioTrilha = new BMUsuarioTrilha().ObterPorId(idUsuarioTrilha);
                novoItem.DataPrazoAvaliacao = DateTime.Now.CalcularPrazo((int)novoItem.UsuarioTrilha.TrilhaNivel.PrazoMonitorDiasUteis);
                novoItem.Visualizado = false;
                novoItem.TipoParticipacao = (enumTipoParticipacaoTrilha)Enum.Parse(typeof(enumTipoParticipacaoTrilha), tipoParticipacao.ToString());
                novoItem.Auditoria = new Auditoria(autenticacao.Login);

                FileServer fileServer = new FileServer();

                // Se não for do tipo base64 está sendo feito o upload por meio do site
                if (!IsBase64(nomeArquivoServidor)) { 
                    
                    if (string.IsNullOrEmpty(tipoArquivo) || string.IsNullOrEmpty(nomeArquivoServidor) || string.IsNullOrEmpty(nomeOriginalArquivo))
                    {
                        fileServer = null;
                    }
                    else
                    {
                        fileServer.NomeDoArquivoOriginal = nomeOriginalArquivo;
                        fileServer.TipoArquivo = tipoArquivo;
                        fileServer.NomeDoArquivoNoServidor = nomeArquivoServidor;

                    }
                }
                // Se for do tipo base64 são requests feitos pelo jogo
                else
                {
                    fileServer = salvaArquivoBase64Sprint(nomeArquivoServidor, nomeOriginalArquivo, tipoArquivo, autenticacao.Login);
                }

                novoItem.FileServer = fileServer;
                bm.Salvar(novoItem);
            }
        }

        private FileServer salvaArquivoBase64Sprint(string imagem, string nomeOriginalArquivo, string tipoArquivo, string login)
        {
            FileServer fileServer = new FileServer();

            MemoryStream memoryStream = CommonHelper.ObterMemoryStream(imagem, false);

            fileServer = CommonHelper.ObterObjetoFileServer(memoryStream);

            //Define o tipo de arquivo (/Quebra a string para obter o tipo do arquivo. Ex: bmp, jpeg, etc...)
            fileServer.TipoArquivo = tipoArquivo;
            fileServer.MediaServer = false;
            fileServer.NomeDoArquivoOriginal = nomeOriginalArquivo;
            fileServer.Auditoria = new Auditoria(login);            

            //Salva a imagem no disco
            ConfiguracaoSistema caminhoParaDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload);

            try
            {               
                string caminhoCompletoComNomeDoArquivo = string.Concat(caminhoParaDiretorioDeUpload.Registro, "\\", fileServer.NomeDoArquivoNoServidor);

                //Salva a imagem no disco
                using (FileStream file = new FileStream(caminhoCompletoComNomeDoArquivo, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(file);
                    file.Close();
                };

            }
            catch (IOException ex)
            {
                throw ex;
            }

            return fileServer;
        }

        private static bool IsBase64(string base64String)
        {
            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                // Handle the exception
            }
            return false;
        }

        public static DateTime CalcularPrazoMonitor(DateTime dataBase, byte? diasUteis)
        {
            if (!diasUteis.HasValue)
                return DataUtil.CalcularPrazo(dataBase, 3); //Prazo pradrão

            return DataUtil.CalcularPrazo(dataBase, diasUteis.Value);
        }

        public IList<TrilhaAtividadeFormativaParticipacao> BuscarListaAtividadeFormativaParticipacao(int pUsuarioTrilha, int pTopicoTematico)
        {

            TrilhaAtividadeFormativaParticipacao atividade = new TrilhaAtividadeFormativaParticipacao()
            {
                UsuarioTrilha = new BMUsuarioTrilha().ObterPorId(pUsuarioTrilha),
                TrilhaTopicoTematico = new BMTrilhaTopicoTematico().ObterPorID(pTopicoTematico)
            };

            if (atividade.UsuarioTrilha == null)
                throw new Exception("Usuário Trilha não identificado.");

            if (atividade.TrilhaTopicoTematico == null)
                throw new Exception("Tópico Temático não identificado.");

            return new BMTrilhaAtividadeFormativaParticipacao().ObterTrilhaAtividadeFormativaParticipacaoPorFiltro(atividade);


        }
    }
}
