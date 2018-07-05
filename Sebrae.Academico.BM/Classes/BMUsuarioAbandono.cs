using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Web;


namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioAbandono : BusinessManagerBase
    {
        private RepositorioBase<UsuarioAbandono> repositorio;


        public BMUsuarioAbandono()
        {
            repositorio = new RepositorioBase<UsuarioAbandono>();
        }

        public void Salvar(UsuarioAbandono pUsuarioAbandono)
        {
            ValidaUsuarioAbandonoInformado(pUsuarioAbandono);
            repositorio.Salvar(pUsuarioAbandono);
        }

        private void ValidaUsuarioAbandonoInformado(UsuarioAbandono pUsuarioAbandono)
        {
            ValidarInstancia(pUsuarioAbandono);

            if (pUsuarioAbandono.Usuario == null)
                throw new Exception("Usuário não informado. Campo Obrigatório!");

            if (pUsuarioAbandono.DataInicioAbandono == new DateTime(1, 1, 1))
                throw new Exception("Data de Inicio não informada. Campo Obrigatório!");

            if (pUsuarioAbandono.DataFimAbandono == new DateTime(1, 1, 1))
                throw new Exception("Data de Fim não informada. Campo Obrigatório!");

            if (pUsuarioAbandono.DataFimAbandono < pUsuarioAbandono.DataInicioAbandono)
                throw new Exception("Data de Fim menor que a Data de Inicio. Verifique os dados informados.");

        }


        public UsuarioAbandono ObterPorUsuario(Usuario pUsuario)
        {
            //return repositorio.GetByProperty("Usuario", pUsuario).FirstOrDefault();
            var query = repositorio.session.Query<UsuarioAbandono>();
            return query.FirstOrDefault(x => x.Usuario.ID == pUsuario.ID);
        }

        public bool ValidarAbandonoAtivo(int pUsuario)
        {
            //IList<UsuarioAbandono> lstAb = repositorio.GetByProperty("Usuario", new BMUsuario().ObterPorId(pUsuario));

            //IList<UsuarioAbandono> lstAb = repositorio.GetByProperty("Usuario", new BMUsuario().ObterPorId(pUsuario));

            var query = repositorio.session.Query<UsuarioAbandono>();
            IList<UsuarioAbandono> lstAb = query.Where(x => x.Usuario.ID == pUsuario).ToList<UsuarioAbandono>();

            return lstAb.Where(x => x.DataInicioAbandono.Date >= DateTime.Now.Date &&
                                     x.DataFimAbandono.Date <= DateTime.Now.Date &&
                                     x.Desconsiderado.Equals(false)).Count() > 0;

        }
    }
}
