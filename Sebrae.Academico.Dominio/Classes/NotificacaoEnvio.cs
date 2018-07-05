using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class NotificacaoEnvio : EntidadeBasicaPorId
    {

        public virtual string Texto { get; set; }
        public virtual string Link { get; set; }

        // Uf nacionalizada da notificação
        public virtual Uf Uf { get; set; }

        public virtual IList<NotificacaoEnvioPermissao> ListaPermissao { get; set; }
        //public virtual IList<DTOSolucaoEducacionalPermissao> ListaUsuariosPermitidos { get; set; }
        public virtual IList<Notificacao> Notificacoes { get; set; }

        public NotificacaoEnvio()
        {
            this.ListaPermissao = new List<NotificacaoEnvioPermissao>();
            //this.ListaUsuariosPermitidos = new List<DTOSolucaoEducacionalPermissao>();
        }

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.NotificacaoEnvio.ID == this.ID);

            if (!perfilEstaNaLista)
            {
                NotificacaoEnvioPermissao notificacaoEnvioPermissao = new NotificacaoEnvioPermissao() { Perfil = perfil, NotificacaoEnvio = this };
                this.ListaPermissao.Add(notificacaoEnvioPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.NotificacaoEnvio.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var solucaoEducacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                   x.Perfil.ID == perfil.ID && x.NotificacaoEnvio.ID == this.ID);
                this.ListaPermissao.Remove(solucaoEducacionalASerExcluido);
            }

        }


        #endregion

        #region "Uf"

        //public virtual void AdicionarUfs(Uf uf)
        //{
        //    AdicionarUfs(uf, 0);
        //}

        public virtual void AdicionarUfs(Uf uf, Usuario usuario)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.NotificacaoEnvio.ID == this.ID);

            if (!ufEstaNaLista)
            {
                NotificacaoEnvioPermissao notificacaoEnvioPermissao = new NotificacaoEnvioPermissao() { Uf = uf, NotificacaoEnvio = this };
                notificacaoEnvioPermissao.Auditoria.UsuarioAuditoria = usuario.CPF;
                this.ListaPermissao.Add(notificacaoEnvioPermissao);
            }
        }

        public virtual void RemoverUfs(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.NotificacaoEnvio.ID == this.ID);

            if (ufEstaNaLista)
            {
                var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null &&
                                                                   x.Uf.ID == uf.ID && x.NotificacaoEnvio.ID == this.ID);
                this.ListaPermissao.Remove(ufASerExcluido);
            }
        }

        #endregion

        #region "Permissão"

        public virtual void AdicionarPermissao(NotificacaoEnvioPermissao permissao)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Perfil.ID == permissao.ID && x.NotificacaoEnvio.ID == this.ID);

            if (!ufEstaNaLista)
            {
                //TODO -> Retestar este ponto
                NotificacaoEnvioPermissao notificacaoEnvioPermissao = new NotificacaoEnvioPermissao() { Perfil = permissao.Perfil, NotificacaoEnvio = this };
                this.ListaPermissao.Add(notificacaoEnvioPermissao);
            }

        }

        public virtual void RemoverPermissao(NotificacaoEnvioPermissao permissao)
        {
            //bool exists = this.ListaPermissao.Where(x => x.ID != permissao.ID).Count() == 0;
            //if (exists)
            //{
            //    var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.ID != null && x.ID == permissao.ID);
            //    this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            //}

            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == permissao.ID && x.NotificacaoEnvio.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                  x.Perfil.ID == permissao.ID && x.NotificacaoEnvio.ID == this.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }

        }


        #endregion

        #region "Turma"

        public virtual void RemoverUsuarios()
        {
            IList<NotificacaoEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Usuario != null && x.NotificacaoEnvio.ID == this.ID).ToList();

            if (notificacoesEnvioPermissaoExcluir.Count() > 0)
            {

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }

            }
        }

        public virtual void RemoverTurmas()
        {
            bool turmaEstaNaLista = ListaPermissao.Any(x => x.Turma != null && x.NotificacaoEnvio.ID == this.ID);

            if (turmaEstaNaLista)
            {
                IList<NotificacaoEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Turma != null && x.NotificacaoEnvio.ID == this.ID).ToList();

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }
            }
        }

        public virtual void RemoverTurmasExceto(Turma tur)
        {
            bool turmaEstaNaLista = ListaPermissao.Any(x => x.Turma != null && x.NotificacaoEnvio.ID == this.ID);

            if (turmaEstaNaLista)
            {
                IList<NotificacaoEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Turma != null && x.Turma.ID != tur.ID && x.NotificacaoEnvio.ID == this.ID).ToList();

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }
            }
        }

        public virtual void RemoverStatus()
        {
            bool statusEstaNaLista = ListaPermissao.Any(x => x.Status != null && x.NotificacaoEnvio.ID == this.ID);

            if (statusEstaNaLista)
            {
                IList<NotificacaoEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Status != null && x.NotificacaoEnvio.ID == this.ID).ToList();

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }
            }
        }

        #endregion

        #region "Usuario"

        public virtual void AdicionarTurma(Turma tur, Usuario usuario)
        {
            var turmaEstaNaLista = ListaPermissao.Any(x => x.Turma != null && x.Turma.ID == tur.ID && x.NotificacaoEnvio.ID == this.ID);

            if (!turmaEstaNaLista)
            {
                //TODO -> Retestar este ponto
                var notificacaoEnvioPermissao = new NotificacaoEnvioPermissao
                {
                    Turma = tur,
                    NotificacaoEnvio = this,
                    Auditoria = { UsuarioAuditoria = usuario.CPF }
                };

                ListaPermissao.Add(notificacaoEnvioPermissao);
            }
        }

        public virtual void AdicionarStatus(IEnumerable<StatusMatricula> listaStatusMatricula, Usuario usuario)
        {
            foreach (var status in listaStatusMatricula)
            {
                var notificacaoEnvioPermissao = new NotificacaoEnvioPermissao
                {
                    Status = status,
                    NotificacaoEnvio = this,
                    Auditoria = { UsuarioAuditoria = usuario.CPF }
                };

                ListaPermissao.Add(notificacaoEnvioPermissao);
            }
        }

        public virtual void AdicionarUsuario(Usuario user, Usuario usuario)
        {
            var usuarioEstaNaLista = ListaPermissao.Any(x => x.Usuario != null && x.Usuario.ID == user.ID);

            if (!usuarioEstaNaLista)
            {
                //TODO -> Retestar este ponto
                var notificacaoEnvioPermissao = new NotificacaoEnvioPermissao
                {
                    Usuario = user,
                    NotificacaoEnvio = this,
                    Auditoria = { UsuarioAuditoria = usuario.CPF }
                };

                ListaPermissao.Add(notificacaoEnvioPermissao);
            }
        }

        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivel)
        {
            //Adicionar caso não esteja na ListaPermissao
            bool nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivel.ID && x.NotificacaoEnvio.ID == this.ID);

            if (!nivelOcupacionalEstaNaLista)
            {
                NotificacaoEnvioPermissao notificacaoEnvioPermissao = new NotificacaoEnvioPermissao() { NivelOcupacional = nivel, NotificacaoEnvio = this };
                this.ListaPermissao.Add(notificacaoEnvioPermissao);
            }
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivel)
        {
            // Remover da ListaPermissao caso o nivel exista lá
            bool nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivel.ID && x.NotificacaoEnvio.ID == this.ID);

            if (nivelOcupacionalEstaNaLista)
            {
                var notificacaoEnvioPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.NivelOcupacional != null &&
                                                                               x.NivelOcupacional.ID == nivel.ID && x.NotificacaoEnvio.ID == this.ID);
                this.ListaPermissao.Remove(notificacaoEnvioPermissaoASerExcluido);
            }
        }

        #endregion
    }
}