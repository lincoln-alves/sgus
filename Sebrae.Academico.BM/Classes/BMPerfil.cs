using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPerfil : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<Perfil> repositorio;

        public BMPerfil()
        {
            repositorio = new RepositorioBase<Perfil>();
        }

        public Perfil ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public IList<Perfil> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public void Salvar(Perfil pPerfil)
        {
            ValidarPerfilInformado(pPerfil);
            repositorio.Salvar(pPerfil);
        }

        private void ValidarPerfilInformado(Perfil pPerfil)
        {
            this.ValidarInstancia(pPerfil);

            if (string.IsNullOrWhiteSpace(pPerfil.Nome))
                throw new Exception("Nome Não Informado. Campo Obrigatório!");

            this.VerificarExistenciaDePerfil(pPerfil);
        }

        private void VerificarExistenciaDePerfil(Perfil pPerfil)
        {
            Perfil perfil = this.ObterPorNome(pPerfil.Nome.Trim());

            if (perfil != null)
            {
                if (pPerfil.ID != perfil.ID)
                {
                    throw new AcademicoException(string.Format("O Perfil '{0}' já está cadastrado", pPerfil.Nome.Trim()));
                }
            }
        }

        public void Excluir(Perfil perfil)
        {
            repositorio.Excluir(ObterPorID(perfil.ID));
        }

        private Perfil ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<Perfil> ObterPorFiltro(Perfil pPerfil)
        {
            var query = repositorio.session.Query<Perfil>();

            if (pPerfil != null)
            {
                if (!string.IsNullOrWhiteSpace(pPerfil.Nome))
                    query = query.Where(x => x.Nome.Contains(pPerfil.Nome));
            }

            return query.ToList<Perfil>();
        }

        public Perfil ObterPorNome(string pNome)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Perfil>();
            return query.FirstOrDefault(x => x.Nome == pNome);
        }

        public void Dispose()
        {
            if (repositorio != null)
            {
                GC.SuppressFinalize(repositorio);
            }
            GC.Collect();
        }
    }
}
