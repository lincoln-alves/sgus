using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Missao
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual PontoSebrae PontoSebrae { get; set; }
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }

        public virtual IEnumerable<FormaAquisicao> ObterFormasAquisicao()
        {
            return ListaItemTrilha.Select(it => it.FormaAquisicao).Distinct();
        }

        /// <summary>
        /// Obter a quantidade de itens trilha por forma de aquisição.
        /// </summary>
        /// <param name="formaAquisicaoId">ID do grupo da forma de aquisição</param>
        /// <returns></returns>
        public virtual int ContarItensPorGrupoFormaAquisicao(int formaAquisicaoId)
        {
            return ListaItemTrilha.AsEnumerable().Count(it => it.FormaAquisicao.ID == formaAquisicaoId);
        }

        public virtual bool UsuarioIniciou(UsuarioTrilha matricula)
        {
            try
            {
                var solucoesSebrae =
                    ListaItemTrilha.Where(
                        x => x.Usuario == null && x.Missao.PontoSebrae.TrilhaNivel.ID == matricula.TrilhaNivel.ID)
                        .AsQueryable();

                if (!solucoesSebrae.Any())
                    return false;


                var retorno = solucoesSebrae
                        .Where(x => x.ObterStatusParticipacoesItemTrilha(matricula) == enumStatusParticipacaoItemTrilha.EmAndamento || x.ObterStatusParticipacoesItemTrilha(matricula) == enumStatusParticipacaoItemTrilha.Aprovado)
                        .Count() > 0;
                

                return retorno;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool UsuarioConcluiu(UsuarioTrilha matricula)
        {
            try
            {
                var solucoesSebrae =
                    ListaItemTrilha.Where(
                        x => x.Usuario == null && x.Missao.PontoSebrae.TrilhaNivel.ID == matricula.TrilhaNivel.ID)
                        .AsQueryable();

                if (!solucoesSebrae.Any())
                    return false;

                var retorno = solucoesSebrae.All(
                        it =>
                            it.ObterStatusParticipacoesItemTrilha(matricula) == enumStatusParticipacaoItemTrilha.Aprovado);

                return retorno;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}