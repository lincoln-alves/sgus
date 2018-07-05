using System.Collections.Generic;
using System;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaTopicoTematico : EntidadeBasica
    {
        public virtual string DescricaoTextoEnvio { get; set; }
        public virtual string DescricaoArquivoEnvio { get; set; }
        public virtual int QtdMinimaPontosAtivFormativa { get; set; }
        public virtual decimal? ValorImportancia { get; set; }
        public virtual string NomeExibicao { get; set; }

        public virtual FileServer FileServer { get; set; }
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
        public virtual TrilhaAtividadeFormativaParticipacao TrilhaAtividadeFormativaParticipacao { get; set; }

        public virtual IList<TrilhaAtividadeFormativaParticipacao> ListaTrilhaAtividadeFormativaParticipacao
        {
            get; set;
        }

        public virtual IEnumerable<Objetivo> ObterObjetivos()
        {
            return ListaItemTrilha.Select(x => x.Objetivo).Distinct();
        }

        public virtual bool PossuiLider()
        {
            return ListaTrilhaTopicoTematicoParticipacao.Any(x => x.UltimaParticipacao != null);
        }

        public virtual IList<PontoSebraeParticipacao> ListaTrilhaTopicoTematicoParticipacao { get; set; }

        /// <summary>
        /// Obter a matrícula do usuário líder do tópico temático, dentro do nível infornado.
        /// O líder é aquele que conclui todas as Soluções Sebrae em menos tempo.
        /// </summary>
        /// <returns></returns>
        public virtual KeyValuePair<UsuarioTrilha, TimeSpan>? ObterLider()
        {
            var timesSpans = ListaTrilhaTopicoTematicoParticipacao.Where(x => x.UltimaParticipacao != null)
                .Select(
                    x =>
                        new
                        {
                            Matricula = x.UsuarioTrilha,
                            Tempo = (x.UltimaParticipacao.Value - x.PrimeiraParticipacao)
                        });

            var lider = timesSpans.OrderBy(x => x.Tempo).FirstOrDefault();

            if (lider == null)
                return null;

            return new KeyValuePair<UsuarioTrilha, TimeSpan>(lider.Matricula, lider.Tempo);
        }

        /// <summary>
        /// Obter a matrícula do usuário líder do tópico temático, dentro do nível infornado.
        /// O líder é aquele que conclui todas as Soluções Sebrae em menos tempo.
        /// </summary>        
        /// <returns></returns>
        public virtual dynamic ObterLiderDetalhado(string enderecoSgus)
        {
            var lider = ObterLider();

            // Caso não tenha líder volta o objeto vario
            var obj = new
            {
                Id = 0,
                Nome = "",
                Imagem = "",
                TempoConclusao = new TimeSpan(),
                NomeTopicoTematico = ""
            };


            if (lider != null && lider.Value.Key.ID > 0)
            {
                obj = new
                {
                    Id = lider.Value.Key.Usuario.ID,
                    Nome = lider.Value.Key.Usuario.ObterPrimeirosNomes(),
                    Imagem = lider.Value.Key.Usuario.ObterLinkImagem(enderecoSgus),
                    TempoConclusao = lider.Value.Value,
                    NomeTopicoTematico = Nome
                };
            }

            return obj;
        }

        public virtual IQueryable<FormaAquisicao> ObterFormasAquisicao()
        {
            return
                ListaItemTrilha.Where(x => x.Usuario == null)
                    .Select(x => x.FormaAquisicao)
                    .Distinct()
                    .OrderBy(x => x.Nome)
                    .AsQueryable();
        }

        public virtual bool UsuarioConcluiu(UsuarioTrilha matricula)
        {
            return
                ListaItemTrilha.Where(x => x.PodeExibir() && x.Usuario == null && x.Missao.PontoSebrae.TrilhaNivel.ID == matricula.ID).All(
                    x => x.ObterStatusParticipacoesItemTrilha(matricula) == enumStatusParticipacaoItemTrilha.Aprovado);
        }
    }
}