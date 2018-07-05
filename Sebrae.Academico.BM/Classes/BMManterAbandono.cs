using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMManterAbandono : BusinessManagerBase
    {
        #region "Atributos Privados"

        private RepositorioBase<UsuarioAbandono> repositorio;

        #endregion

        #region "Construtor"

        public BMManterAbandono()
        {
            repositorio = new RepositorioBase<UsuarioAbandono>();
        }

        #endregion

        #region "Métodos Públicos"

        public UsuarioAbandono ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        /// <summary>
        /// Obtém informações sobre os abandonos do usuário.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public IList<UsuarioAbandono> ObterPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<UsuarioAbandono>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            return query.ToList<UsuarioAbandono>();
        }

        public void Salvar(UsuarioAbandono pUsuarioAbandono)
        {
            repositorio.Salvar(pUsuarioAbandono);
        }

        public UsuarioAbandono VerificarExistenciaDeAbandonoValido(Usuario usuario)
        {

            var query = repositorio.session.Query<UsuarioAbandono>();
            return query.Where(x => x.Usuario.ID == usuario.ID && x.DataFimAbandono.Date > DateTime.Today.Date && !x.Desconsiderado).OrderByDescending(x => x.DataFimAbandono).FirstOrDefault() ;

        }

        #endregion
    }
}
