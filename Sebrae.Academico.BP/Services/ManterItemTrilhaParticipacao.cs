using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Extensions;
using System.IO;

namespace Sebrae.Academico.BP.Services
{
    public class ManterItemTrilhaParticipacao : BusinessProcessServicesBase
    {
        private BMItemTrilhaParticipacao itemTrilhaParticipacaoBM;
        private BMFileServer fileServerBM;

        public dynamic CadastrarItemTrilhaParticipacao(int idUsuarioTrilha, int idItemTrilha, string textoParticipacao, string nomeOriginalArquivo, string imagem, int tipoParticipacao, string cpfAuditoria)
        {
            itemTrilhaParticipacaoBM = new BMItemTrilhaParticipacao();
            fileServerBM = new BMFileServer();

            var usuarioTrilha = new BMUsuarioTrilha().ObterPorId(idUsuarioTrilha);

            var isParticipacaoTrilheiro = tipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro;

            if (isParticipacaoTrilheiro && DateTime.Now > usuarioTrilha.DataLimite)
            {
                throw new AcademicoException("O prazo para envio desta atividade já passou!");
            }

            var itemTrilhaParticipacao = new ItemTrilhaParticipacao();
            var itemTrilha = new BMItemTrilha().ObterPorID(idItemTrilha);
            if (idItemTrilha > 0 && idUsuarioTrilha > 0)
            {
                var listaItemTrilhaParticipacao = itemTrilhaParticipacaoBM.ObterParticipacoesUsuarioTrilha(
                    idItemTrilha, idUsuarioTrilha);

                if (listaItemTrilhaParticipacao.Count > 0)
                {
                    if (
                        listaItemTrilhaParticipacao.Any(
                            x =>
                                x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro &&
                                x.Autorizado.HasValue && x.Autorizado.Value) && isParticipacaoTrilheiro)
                    {
                        throw new AcademicoException("Sua atividade já está aprovada!");
                    }

                    if (
                        listaItemTrilhaParticipacao.Any(
                            x =>
                                x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro &&
                                !x.Autorizado.HasValue) && isParticipacaoTrilheiro)
                    {
                        throw new AcademicoException("Sua atividade ainda não foi avaliada!");
                    }
                }

                itemTrilhaParticipacao.ItemTrilha = itemTrilha;
                itemTrilhaParticipacao.TextoParticipacao = textoParticipacao;
                itemTrilhaParticipacao.DataEnvio = DateTime.Now;
                itemTrilhaParticipacao.UsuarioTrilha = usuarioTrilha;
                itemTrilhaParticipacao.Visualizado = false;
                itemTrilhaParticipacao.TipoParticipacao =
                    (enumTipoParticipacaoTrilha)
                        Enum.Parse(typeof(enumTipoParticipacaoTrilha), tipoParticipacao.ToString());
                itemTrilhaParticipacao.Auditoria = new Auditoria(cpfAuditoria);
                if (itemTrilha.Missao.PontoSebrae.TrilhaNivel.PrazoMonitorDiasUteis != null)
                    itemTrilhaParticipacao.DataPrazoAvaliacao =
                        DateTime.Now.CalcularPrazo((int) itemTrilha.Missao.PontoSebrae.TrilhaNivel.PrazoMonitorDiasUteis);

                FileServer fileServer;

                if (string.IsNullOrEmpty(nomeOriginalArquivo))
                {
                    fileServer = null;
                }
                else
                {
                    var memoryStream = CommonHelper.ObterMemoryStream(imagem);
                    fileServer = CommonHelper.ObterObjetoFileServer(memoryStream);
                    
                    var caminhoDiretorioUpload =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

                    // Escrever o arquivo na pasta.
                    CommonHelper.EnviarArquivoParaRepositorio(caminhoDiretorioUpload, memoryStream, fileServer.NomeDoArquivoNoServidor);

                    //Define o tipo de arquivo (/Quebra a string para obter o tipo do arquivo. Ex: bmp, jpeg, etc...)
                    fileServer.TipoArquivo = CommonHelper.ObterTipoDoArquivo(imagem);
                    fileServer.MediaServer = true;
                    fileServer.NomeDoArquivoOriginal = nomeOriginalArquivo;
                    fileServer.Auditoria = new Auditoria(cpfAuditoria);
                    fileServerBM.Salvar(fileServer);
                }

                itemTrilhaParticipacao.FileServer = fileServer;
                itemTrilhaParticipacaoBM.Salvar(itemTrilhaParticipacao);

                // Verifica se o usuário já tem participação no item trilha
                InformarParticipacaoLoja(itemTrilhaParticipacao);
            }

            var status = itemTrilha.ObterStatusParticipacoesItemTrilha(usuarioTrilha);

            return new
            {
                Status = status != null ? (int?)status.Value : null
            };
        }

        private static void EnviarArquivoParaRepositorio(MemoryStream memoryStream, string nomeArquivoNoServidor)
        {
            var caminhoDiretorioUpload =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

            var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                nomeArquivoNoServidor);

            File.WriteAllBytes(diretorioDeUploadComArquivo, memoryStream.ToArray());
        }

        public bool UsuarioAprovadoObjetivo(UsuarioTrilha usuarioTrilha, int idObjetivo)
        {
            var lista = new BMItemTrilha().ObterItemTrilhasPorNivelTrilhaObjetivo(idObjetivo, usuarioTrilha.TrilhaNivel.ID);

            // Se tiver soluções obrigatórias verifica se o usuário foi aprovado em todas as soluções obrigatórias
            if (lista.Any(x => x.SolucaoEducacional.ID != 0 && x.SolucaoObrigatoria == true))
            {
                return lista.Any(x => x.SolucaoEducacional.ID != 0 && x.SolucaoObrigatoria == true &&
                                      x.ListaItemTrilhaParticipacao.All(y => y.UsuarioTrilha.ID == usuarioTrilha.ID && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Autorizado == true));
            }
            // Do contrário verifica se o usuário realizou alguma solução desse objetivo e foi aprovado
            else
            {
                return lista.Any(x => x.ListaItemTrilhaParticipacao.Any(y => y.UsuarioTrilha.ID == usuarioTrilha.ID && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Autorizado == true));
            }
        }

        public List<DTOSolucoesObrigatorias> obtemSolucoesJogo(UsuarioTrilha usuarioTrilha, int idObjetivo)
        {
            List<DTOSolucoesObrigatorias> lista = new List<DTOSolucoesObrigatorias>();

            var query = new BMItemTrilha().ObterItemTrilhasPorNivelTrilhaObjetivo(idObjetivo, usuarioTrilha.TrilhaNivel.ID);
            var solucoesObrigatorias = query.Where(x => x.SolucaoEducacional.ID != 0 && x.SolucaoObrigatoria == true).ToList();

            // Caso tenha alguma solução obrigatória retorna essas
            if (solucoesObrigatorias.Count() > 0)
            {
                foreach (var item in solucoesObrigatorias)
                {
                    lista = insereItemParticipacaoJogo(lista, item, usuarioTrilha);
                }
            }
            else
            {
                // Da preferência para Soluções Educacionais, dessa forma funciona caso não tenha soluções educacionais o objetivo
                var item = query.Where(x => x.SolucaoObrigatoria == false).OrderByDescending(x => x.SolucaoEducacional).FirstOrDefault();

                lista = insereItemParticipacaoJogo(lista, item, usuarioTrilha);

            }
            return lista;
        }

        private List<DTOSolucoesObrigatorias> insereItemParticipacaoJogo(List<DTOSolucoesObrigatorias> lista, ItemTrilha item, UsuarioTrilha usuarioTrilha)
        {
            lista.Add(new DTOSolucoesObrigatorias()
            {
                ID = item.ID,
                Nome = item.Nome,
                Status = item.ListaItemTrilhaParticipacao.Any(y => y.ItemTrilha.ID == item.ID && y.UsuarioTrilha.ID == usuarioTrilha.ID && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Autorizado.HasValue && y.Autorizado == true)
            });
            return lista;
        }

        public static DateTime CalcularPrazoMonitor(DateTime dataBase, byte? diasUteis)
        {
            if (!diasUteis.HasValue)
                return DataUtil.CalcularPrazo(dataBase, 3); //Prazo pradrão

            return DataUtil.CalcularPrazo(dataBase, diasUteis.Value);
        }

        public void InformarParticipacaoLoja(ItemTrilhaParticipacao itemTrilhaParticipacao)
        {
            new ManterTrilhaTopicoTematicoParticipacao().IncluirPrimeiraParticipacao(itemTrilhaParticipacao);
        }
     
        public static void Salvar(ItemTrilhaParticipacao itemTrilhaParticipacao)
        {
            new BMItemTrilhaParticipacao().Salvar(itemTrilhaParticipacao);
        }
    }
}
