using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class PontoSebrae
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NomeExibicao { get; set; }
        public virtual int? QtMinimaPontos { get; set; }
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual IList<Missao> ListaMissoes { get; set; }
        public virtual IList<PontoSebraeParticipacao> ListaPontoSebraeParticipacao { get; set; }

        public virtual bool Ativo { get; set; }
        //public virtual IList<LogLider> ListaLogLider { get; set; }

        public virtual KeyValuePair<UsuarioTrilha, TimeSpan>? ObterLider()
        {
            var timesSpans = ListaPontoSebraeParticipacao.Where(x => x.UltimaParticipacao != null)
                .Select(x => new
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
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual IQueryable<FormaAquisicao> ObterFormasAquisicao()
        {
            return ListaMissoes.SelectMany(x => x.ListaItemTrilha).Where(x => x.Usuario == null)
                .Select(x => x.FormaAquisicao)
                .Distinct()
                .OrderBy(x => x.Nome)
                .AsQueryable();
        }

        public virtual IQueryable<ItemTrilha> ObterItensTrilha()
        {
            return ListaMissoes.SelectMany(x => x.ListaItemTrilha).AsQueryable();
        }

        public virtual bool PossuiLider()
        {
            return ListaPontoSebraeParticipacao.Any(x => x.UltimaParticipacao != null);
        }

    }
}