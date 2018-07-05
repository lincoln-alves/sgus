using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSistemaExterno : BusinessManagerBase
    {
        private RepositorioBase<SistemaExterno> repositorio;


        public BMSistemaExterno()
        {
            repositorio = new RepositorioBase<SistemaExterno>();
        }

        public void Excluir(SistemaExterno pSistemaExterno)
        {
            repositorio.Excluir(pSistemaExterno);
        }

        private void ValidarSistemaExternoInformado(SistemaExterno pSistemaExterno)
        {
            
            if (string.IsNullOrWhiteSpace(pSistemaExterno.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");
        }

        public void Salvar(SistemaExterno pSistemaExterno)
        {
            ValidarSistemaExternoInformado(pSistemaExterno);
            repositorio.Salvar(pSistemaExterno);
        }

        //public void AlterarPrograma(Programa pPrograma)
        //{
        //    ValidarPrograma(pPrograma);
        //    pPrograma.DataAlteracao = DateTime.Now;
        //    repositorio.Salvar(pPrograma);
        //}

        public IList<SistemaExterno> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public SistemaExterno ObterPorId(int pId)
        {
            ////return repositorio.ObterPorID(pId);

            //var query = repositorio.session.Query<SistemaExterno>();
            //query = query.FetchMany(x => x.ListaPermissao).ThenFetch(x => x.Uf);
            //query = query.FetchMany(x => x.ListaPermissao).ThenFetch(x => x.NivelOcupacional);
            //query = query.FetchMany(x => x.ListaPermissao).ThenFetch(x => x.Perfil);

            //return query.FirstOrDefault(x => x.ID == pId);

            return repositorio.ObterPorID(pId);
        }

        public IList<Programa> ObterPorFiltro(Programa programa)
        {
            var query = repositorio.session.Query<Programa>(); //.Where(x => x.Ativo == programa.Ativo);

            if (programa.ID != 0)
                query = query.Where(x => x.ID == programa.ID);

            if (!string.IsNullOrWhiteSpace(programa.Nome))
                query.Where(x => x.Nome.Contains(programa.Nome));

            return query.ToList();


        }

        public IList<SistemaExterno> ObterSistemaExternoPorFiltro(SistemaExterno pSistemaExterno)
        {
            var query = repositorio.session.Query<SistemaExterno>();

            if (pSistemaExterno != null)
            {
                if (!string.IsNullOrWhiteSpace(pSistemaExterno.Nome))
                    query = query.Where(x => x.Nome.ToUpper().Contains(pSistemaExterno.Nome.ToUpper()));

                // Se for um sistema publico
                if (pSistemaExterno.Publico.HasValue)
                {
                    query = query.Where(x => x.Publico.Value);
                }
            }

            return query.ToList<SistemaExterno>();
        }

        /// <summary>
        /// Obtem todos os acessos de sistemas externos por usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public IQueryable<SistemaExterno> ObterTodosPorUsuario(Usuario usuario)
        {
            var query = repositorio.session.Query<SistemaExterno>();
            query.Where(x => x.ListaUsuariosPermitidos.Any(permissao => usuario.ListaPerfil.Any(u => u.ID == permissao.ID)));
            return query;
        } 
    }
}
