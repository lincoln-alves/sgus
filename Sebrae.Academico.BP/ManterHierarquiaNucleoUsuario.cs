using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterHierarquiaNucleoUsuario : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMHierarquiaNucleoUsuario bmHierarquiaNucleoUsuario = null;

        #endregion

        #region "Construtor"

        public ManterHierarquiaNucleoUsuario()
            : base()
        {
            bmHierarquiaNucleoUsuario = new BMHierarquiaNucleoUsuario();
        }

        #endregion

        public HierarquiaNucleoUsuario ObterPorId(int idHierarquiaNucleoUsuario)
        {
            return bmHierarquiaNucleoUsuario.ObterPorId(idHierarquiaNucleoUsuario);
        }

        public HierarquiaNucleoUsuario ObterPorUsuarioEHieraquiaNucleo(int idUsuario, int idHierarquiaNucleo)
        {
            return bmHierarquiaNucleoUsuario.ObterPorUsuarioEHieraquiaNucleo(idUsuario, idHierarquiaNucleo);
        }

        public IList<HierarquiaNucleoUsuario> ObterTodos()
        {
            return bmHierarquiaNucleoUsuario.ObterTodos().ToList();
        }
        public IQueryable<HierarquiaNucleoUsuario> ObterTodosIQueryable()
        {
            return bmHierarquiaNucleoUsuario.ObterTodos();
        }

        public IQueryable<HierarquiaNucleoUsuario> ObterUsuariosNucleo(int idUsuario)
        {
            return bmHierarquiaNucleoUsuario.ObterUsuariosNucleo(idUsuario);
        }

        public void Salvar(Usuario usuario, HierarquiaNucleo hierarquiaNucleo, Uf uf, bool isGestor = false)
        {
            var hierarquiaNucleoUsuario = new HierarquiaNucleoUsuario
            {
                Usuario = usuario,
                Uf = uf,
                HierarquiaNucleo = hierarquiaNucleo,
                IsGestor = isGestor
            };

            bmHierarquiaNucleoUsuario.Salvar(hierarquiaNucleoUsuario);
        }

        public void Remover(int idHierarquiaNucleoUsuario)
        {
            bmHierarquiaNucleoUsuario.Remover(idHierarquiaNucleoUsuario);
        }

        public void ExcluirTodosFkHieraquiaNucleoUsuario(int fk)
        {
            bmHierarquiaNucleoUsuario.ExcluirTodosFkHieraquiaNucleoUsuario(fk);
        }

        public void Excluir(int idHierarquiaNucleo)
        {
            bmHierarquiaNucleoUsuario.Excluir(idHierarquiaNucleo);
        }

        public void Dispose()
        {
            bmHierarquiaNucleoUsuario.Dispose();
        }
    }
}