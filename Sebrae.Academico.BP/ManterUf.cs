using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterUf : BusinessProcessBase
    {

        private BMUf bmUf = null;

        public ManterUf()
            : base()
        {
            bmUf = new BMUf();
        }
              
        public Uf ObterUfPorID(int pId)
        {
            return bmUf.ObterPorId(pId);
        }

        public Uf ObterUfPorSigla(string pSigla)
        {

            if (string.IsNullOrWhiteSpace(pSigla))
            {
                throw new AcademicoException("Sigla. Campo Obrigatório");
            }

            return bmUf.ObterPorSigla(pSigla);
        }

        public IList<Uf> ObterTodosUf()
        {
            return bmUf.ObterTodos();
        }

        public IQueryable<Uf> ObterTodosIQueryable()
        {
            return bmUf.ObterTodosIQueryable();
        }

        public IList<Uf> ObterDoUsuarioLogado()
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var listaUf = new List<Uf>();

            if (usuarioLogado.UF.ID != (int)enumUF.NA)
                listaUf.Add(usuarioLogado.UF);
            else
                listaUf = bmUf.ObterTodos().OrderBy(x => x.Nome).ToList();

            return listaUf;
        }

        public void Nacionalizar(int idUf)
        {
            bmUf.Nacionalizar(idUf);
        }
    }
}