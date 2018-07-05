using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUf : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<Uf> repositorio;

        #endregion

        #region "Construtor"


        public BMUf()
        {
            repositorio = new RepositorioBase<Uf>();
        }

        #endregion

        public Uf ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public IList<Uf> BuscarporNome(Uf pStatus)
        {
            //return repositorio.GetByProperty("Nome", pStatus.Nome);
            var query = repositorio.session.Query<Uf>();
            return query.Where(x => x.Nome == pStatus.Nome).ToList<Uf>();
        }

        public void Salvar(Uf pUf)
        {
            repositorio.Salvar(pUf);
        }

        public IList<Uf> ObterTodos()
        {
            var query = repositorio.session.Query<Uf>();
            return query.ToList();
        }

        public void Excluir(Uf pTrilha)
        {
            repositorio.Excluir(pTrilha);
        }

        public IList<Uf> ObterPorNome(string pNome)
        {
            return repositorio.LikeByProperty("Nome", pNome);
        }

        public Uf ObterPorSigla(string uf)
        {
            //return repositorio.GetByProperty("Sigla", uf).FirstOrDefault();
            var query = repositorio.session.Query<Uf>();
            return query.FirstOrDefault(x => x.Sigla.ToLower() == uf.Trim().ToLower());
        }

        public void Nacionalizar(int idUf)
        {
            var uf = ObterPorId(idUf);

            var bmNacionalizacaoUf = new BMNacionalizacaoUf();

            if (uf.IsNacionalizado())
            {
                var nacionalizacao = (uf.ListaNacionalizacaoUf.FirstOrDefault());

                uf.ListaNacionalizacaoUf = new List<NacionalizacaoUf>();

                bmNacionalizacaoUf.Excluir(nacionalizacao);
            }
            else
            {
                var nacionalizacao = new NacionalizacaoUf
                {
                    Uf = uf,
                    DataAlteracao = DateTime.Now,
                    UsuarioAlteracao = new BMUsuario().ObterUsuarioLogado().CPF
                };

                uf.ListaNacionalizacaoUf.Add(nacionalizacao);

                bmNacionalizacaoUf.Salvar(nacionalizacao);

            }

            Salvar(uf);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<Uf> ObterTodosIQueryable()
        {
            return repositorio.session.Query<Uf>().AsQueryable();
        }
    }
}
