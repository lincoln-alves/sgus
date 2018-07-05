using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioTag : BusinessManagerBase
    {
        private RepositorioBase<UsuarioTag> repositorio;

        public BMUsuarioTag()
        {
            repositorio = new RepositorioBase<UsuarioTag>();
        }

        public void Salvar(UsuarioTag usuarioTag)
        {
            ValidarUsuarioTagInformado(usuarioTag);
            repositorio.Salvar(usuarioTag);
        }

        private void ValidarUsuarioTagInformado(UsuarioTag usuarioTag)
        {
            ValidarInstancia(usuarioTag);

            if (usuarioTag.Usuario == null)
                throw new Exception("Usuário Não Infomado. Campo Obrigatório!");

        }

        public IList<UsuarioTag> ObterPorFiltro(UsuarioTag usuarioTag)
        {
            var query = repositorio.session.Query<UsuarioTag>();

            if (usuarioTag != null)
            {
                if (usuarioTag.Usuario != null)
                {
                    query = query.Where(x => x.Usuario.ID == usuarioTag.Usuario.ID);
                }
            }
            return query.ToList();

        }

        public void Excluir(UsuarioTag usuarioTag)
        {
            ValidaExclusaoTagInteresse(usuarioTag);
            repositorio.Excluir(repositorio.ObterPorID(usuarioTag.ID));
        }

        public void Excluir(int IdUsuario, int IdTag)
        {
            var query = repositorio.session.Query<UsuarioTag>();
            UsuarioTag usuarioTag = query.FirstOrDefault(x => x.Usuario.ID == IdUsuario && x.Tag.ID == IdTag);

            if (usuarioTag != null)
                repositorio.Excluir(usuarioTag);
            else
            {
                throw new AcademicoException("Registro não encontrado");
            }
        }

        private void ValidaExclusaoTagInteresse(UsuarioTag pUsuarioTag)
        {

            UsuarioTag validacao = ObterPorID(pUsuarioTag.ID);
            if (validacao == null)
                throw new Exception("Erro ao tentar excluir o registro. Registro informado não existe!");

            if (validacao.DataValidade != null &&
                validacao.DataValidade.Value.Date >= DateTime.Now.Date)
                throw new Exception("Erro ao tentar excluir o registro. Não é possível excluir um registro ainda pactuado.");

        }

        public UsuarioTag ObterPorID(int pIdUsuarioTag)
        {
            return repositorio.ObterPorID(pIdUsuarioTag);
        }

        
        public bool VerificarSeUsuarioPossuiTag(int IdUsuario, int IdTag)
        {
            var query = repositorio.session.Query<UsuarioTag>();
            bool usuarioPossuiTag = query.Any(x => x.Usuario.ID == IdUsuario && x.Tag.ID == IdTag);
            return usuarioPossuiTag;
        }
    }
}
