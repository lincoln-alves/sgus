using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterTermoAceite : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTermoAceite bmTermoAceite;

        #endregion

        #region "Construtor"

        public ManterTermoAceite()
        {
            bmTermoAceite = new BMTermoAceite();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<TermoAceite> ObterTodosTermoAceite()
        {
            return bmTermoAceite.ObterTodos();
        }

        public TermoAceite ObterTermoAceitePorID(int pId)
        {
            return bmTermoAceite.ObterPorID(pId);
        }

        public IEnumerable<TermoAceite> ObterListaPorCategoriaConteudo(int idCategoriaConteudo, int idTermoAceite, Usuario usuarioLogado)
        {
            var termos = bmTermoAceite.ObterListaPorCategoriaConteudo(idCategoriaConteudo, idTermoAceite);

            // Busca os termos de aceite da UF do criador ou pelo ID, caso seja edição.
            if (usuarioLogado.IsGestor())
                termos =
                    termos.Where(
                        x =>
                            (idTermoAceite == 0 || x.ID == idTermoAceite) ||
                            (x.Usuario != null && (x.Usuario.UF.ID == usuarioLogado.UF.ID)));

            return termos;
        }

        public DTOTermoAceite ObterPorSolucacaoEducacional(int idSe)
        {
            return bmTermoAceite.ObterPorSolucacaoEducacional(idSe);
        }

        public IEnumerable<TermoAceite> ObterPorNome(string nome, bool parcial = true, bool caseSensitive = false)
        {
            return bmTermoAceite.ObterPorNome(nome, parcial, caseSensitive);
        }

        #endregion

        public void Salvar(TermoAceite termoAceite)
        {
            bmTermoAceite.Salvar(termoAceite);
        }

        public void Excluir(int idTermoAceite)
        {
            bmTermoAceite.Excluir(idTermoAceite);
        }
    }
}