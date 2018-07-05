using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Loja;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterItemTrilha : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMItemTrilha bmItemTrilha = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterItemTrilha()
            : base()
        {
            bmItemTrilha = new BMItemTrilha();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirItemTrilha(ItemTrilha pItemTrilha)
        {
            bmItemTrilha.Salvar(pItemTrilha);
        }

        public void AlterarItemTrilha(ItemTrilha pItemTrilha)
        {
            PreencherInformacoesDeAuditoria(pItemTrilha);

            if (pItemTrilha.FileServer != null)
            {
                PreencherInformacoesDeAuditoria(pItemTrilha.FileServer);
            }

            bmItemTrilha.Salvar(pItemTrilha);
        }

        public IQueryable<ItemTrilha> ObterTodosItemTrilha()
        {
            return bmItemTrilha.ObterTodos();
        }

        public IQueryable<ItemTrilha> ObterTodosIQueryable()
        {
            return bmItemTrilha.ObterTodosIQueryable();
        }

        public IEnumerable<SolucaoEducacional> ObterItemTrilhaSolucaoPorTipo(enumTipoSolucao tipo)
        {
            var query = bmItemTrilha.ObterTodosIQueryable();

            query = tipo == enumTipoSolucao.SolucaoSebrae ? query.Where(x => x.Usuario == null) : query.Where(x => x.Usuario != null);

            var solucoes = query.Where(x => x.SolucaoEducacional != null).Select(x => x.SolucaoEducacional).ToList();
            var groupSolucoes = solucoes.GroupBy(x => x.ID);

            return groupSolucoes.Select(x => x.FirstOrDefault());
        }

        public ItemTrilha ObterItemTrilhaPorID(int pId)
        {
            return bmItemTrilha.ObterPorID(pId);
        }

        public void ExcluirItemTrilha(int IdItemTrilha)
        {
            try
            {
                ItemTrilha itemTrilha = null;
                if (IdItemTrilha > 0)
                {
                    itemTrilha = bmItemTrilha.ObterPorID(IdItemTrilha);
                }
                bmItemTrilha.Excluir(itemTrilha);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IQueryable<ItemTrilha> ObterItemTrilhaPorFiltro(ItemTrilha pItemTrilha)
        {
            return bmItemTrilha.ObterPorFiltro(pItemTrilha);
        }

        #endregion

        public void ExcluirSolucaoEducacionalAutoIndicativa(int idItemTrilha, Usuario usuarioLogado)
        {
            if (usuarioLogado == null) throw new AcademicoException("Acesso Restrito.");
            ExcluirSolucaoEducacionalAutoIndicativa(idItemTrilha, usuarioLogado.CPF, usuarioLogado);
        }

        public void ExcluirSolucaoEducacionalAutoIndicativa(int idItemTrilha, string cpfSolicitante,
            Usuario usuarioLogado = null)
        {
            if (usuarioLogado == null)
            {
                var manterUsuario = new ManterUsuario();
                usuarioLogado = manterUsuario.ObterPorCPF(cpfSolicitante);
                if (usuarioLogado == null) throw new AcademicoException("Acesso Restrito.");
            }
            var itemTrilha = ObterItemTrilhaPorID(idItemTrilha);
            if (itemTrilha == null)
            {
                throw new AcademicoException("Acesso Restrito.");
            }
            if (itemTrilha.Usuario.ID != usuarioLogado.ID)
            {
                throw new AcademicoException("Acesso Restrito.");
            }
            var bmItemTrilhaParticipacao = new BMItemTrilhaParticipacao();
            var lista =
                bmItemTrilhaParticipacao.ObterItemTrilhaParticipacaoPorFiltro(new ItemTrilhaParticipacao
                {
                    ItemTrilha = itemTrilha
                });
            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    bmItemTrilhaParticipacao.Excluir(item);
                }
            }
            var perfilPodeExcluir = usuarioLogado.IsMonitorTrilha() || usuarioLogado.IsAdministradorTrilha() ||
                                    usuarioLogado.IsAdministrador();
            if (!perfilPodeExcluir)
            {
                bool usuarioAssociado = bmItemTrilhaParticipacao.ItemAutoIndicativo(idItemTrilha, cpfSolicitante);
                if (!usuarioAssociado)
                {
                    throw new AcademicoException(
                        "Você não pode excluir itens da trilha de outro usuário ou sugerido pela UC.");
                }
            }
            this.ExcluirItemTrilha(idItemTrilha);

        }

        //Trilha Game
        public dynamic CadastrarSolucaoTrilheiro(UsuarioTrilha usuarioTrilha, DTOSolucaoTrilheiro dtoSolucaoTrilheiro, UsuarioTrilha matricula)
        {
            FileServer fileServer = null;

            if (!string.IsNullOrEmpty(dtoSolucaoTrilheiro.Arquivo))
            {
                //Fazer upload do arquivo
                var memoryStream = CommonHelper.ObterMemoryStream(dtoSolucaoTrilheiro.Arquivo);

                fileServer = CommonHelper.ObterObjetoFileServer(memoryStream);

                var caminhoDiretorioUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

                // Escrever o arquivo na pasta.
                CommonHelper.EnviarArquivoParaRepositorio(caminhoDiretorioUpload, memoryStream, fileServer.NomeDoArquivoNoServidor);

                fileServer.TipoArquivo = CommonHelper.ObterTipoDoArquivo(dtoSolucaoTrilheiro.Arquivo);
                fileServer.MediaServer = true;
                fileServer.NomeDoArquivoOriginal = dtoSolucaoTrilheiro.NomeDoArquivoOriginal;
                fileServer.Auditoria = new Auditoria(matricula.Usuario.CPF);
                new BMFileServer().Salvar(fileServer);
            }

            //item Trilha
            var item = new ItemTrilha
            {
                ID = 0,
                DataCriacao = DateTime.Now,
                Nome = dtoSolucaoTrilheiro.Nome,
                Local = dtoSolucaoTrilheiro.Orientacao,
                LinkConteudo = dtoSolucaoTrilheiro.LinkConteudo,
                ReferenciaBibliografica = dtoSolucaoTrilheiro.ReferenciaBibliografica,
                QuantidadePontosParticipacao = 1,
                SolucaoEducacional = null,
                Missao = new ManterMissao().ObterPorID(dtoSolucaoTrilheiro.MissaoId),
                Usuario = usuarioTrilha.Usuario,
                FormaAquisicao = new ManterFormaAquisicao().ObterFormaAquisicaoPorID(dtoSolucaoTrilheiro.IdTipo),
                Ativo = true,
                Aprovado = enumStatusSolucaoEducacionalSugerida.Aprovado,
                FileServer = fileServer,
                CargaHoraria = dtoSolucaoTrilheiro.GetCargaHoriaEmMinutos()
            };

            bmItemTrilha.Salvar(item);

            // Retorna os dados para re-inserção na tela.
            return new DtoTrilhaSolucaoSebrae
            {
                Id = item.ID,
                Nome = item.Nome,
                FormaAquisicaoId = item.FormaAquisicao.ID,
                FormaAquisicao = item.FormaAquisicao.Nome,
                Orientacao = !string.IsNullOrWhiteSpace(item.Local) ? item.Local : "Sem Orientação"
            };
        }
    }
}