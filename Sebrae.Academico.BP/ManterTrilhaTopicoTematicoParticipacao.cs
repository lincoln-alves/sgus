using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterTrilhaTopicoTematicoParticipacao : BusinessProcessBase
    {
        private readonly BMTrilhaTopicoTematicoParticipacao _bmTrilhaTopicoTematicoParticipacao;

        public ManterTrilhaTopicoTematicoParticipacao()
        {
            _bmTrilhaTopicoTematicoParticipacao = new BMTrilhaTopicoTematicoParticipacao();
        }


        public void Salvar(PontoSebraeParticipacao model)
        {
            _bmTrilhaTopicoTematicoParticipacao.Salvar(model);
        }


        public void IncluirUltimaParticipacao(UsuarioTrilha usuario, ItemTrilha itemTrilha)
        {
            var participacao =
                _bmTrilhaTopicoTematicoParticipacao.ObterPorUsuarioPontoSebrae(usuario, itemTrilha.Missao.PontoSebrae);

            if (VerificarUltimaParticipacao(usuario, itemTrilha))
            {
                var ultimaParticipacao =
                    itemTrilha.ListaItemTrilhaParticipacao.LastOrDefault(
                        x => x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro
                             && x.UsuarioTrilha.ID == usuario.ID);

                if (ultimaParticipacao != null)
                {
                    participacao.UltimaParticipacao = ultimaParticipacao.DataEnvio;
                    _bmTrilhaTopicoTematicoParticipacao.Salvar(participacao);
                }
            }
        }

        public void IncluirPrimeiraParticipacao(ItemTrilhaParticipacao itemTrilhaParticipacao)
        {
            // Verifica se o usuário já tem alguma participação no tópico temático
            if (itemTrilhaParticipacao.ItemTrilha.Missao.PontoSebrae.ListaPontoSebraeParticipacao.Any(
                x => x.UsuarioTrilha.ID == itemTrilhaParticipacao.UsuarioTrilha.ID))
                return;

            // Caso não tenha cria a primeira participação do usuário
            var participacao = new PontoSebraeParticipacao
            {
                PrimeiraParticipacao = itemTrilhaParticipacao.DataEnvio,
                PontoSebrae = itemTrilhaParticipacao.ItemTrilha.Missao.PontoSebrae,
                UsuarioTrilha = itemTrilhaParticipacao.UsuarioTrilha
            };

            new ManterTrilhaTopicoTematicoParticipacao().Salvar(participacao);
        }

        // Caso o usuário tenha participado de todos os itens do tópico temático (loja)
        public bool VerificarUltimaParticipacao(UsuarioTrilha usuario, ItemTrilha itemTrilha)
        {
            return
                itemTrilha.Missao.PontoSebrae.ObterItensTrilha().Where(x => x.Usuario == null && x.Ativo == true)
                    .All(
                        x =>
                            x.Tipo != null &&
                            x.ObterStatusParticipacoesItemTrilha(usuario) == enumStatusParticipacaoItemTrilha.Aprovado);
        }
    }
}