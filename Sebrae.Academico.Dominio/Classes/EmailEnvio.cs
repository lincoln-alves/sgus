using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EmailEnvio : EntidadeBasicaPorId
    {
        public virtual string Texto { get; set; }
        public virtual string Assunto { get; set; }
        public virtual bool Processado { get; set; }
        public virtual DateTime DataGeracao { get; set; }

        public virtual IList<EmailEnvioPermissao> ListaPermissao { get; set; }
        public virtual IList<Email> ListaEmailsGerados { get; set; }
        //public virtual IList<DTOSolucaoEducacionalPermissao> ListaUsuariosPermitidos { get; set; }

        public virtual string ProcessadoFormatado { get { return (Processado ? "Sim" : "Não"); } }
        public virtual string QuantidadeFormatado { get { return (this.ListaEmailsGerados != null ? this.ListaEmailsGerados.Count.ToString() : "0"); } }
        public virtual string QuantidadeEnviadoFormatado { get { return (this.ListaEmailsGerados != null ? this.ListaEmailsGerados.Count(x => x.Enviado == true).ToString() : "0"); } }

        public virtual Uf Uf { get; set; }

        public EmailEnvio()
        {
            this.ListaPermissao = new List<EmailEnvioPermissao>();
            this.ListaEmailsGerados = new List<Email>();
            //this.ListaUsuariosPermitidos = new List<DTOSolucaoEducacionalPermissao>();
        }

        #region "Nivel Ocupacional"

        /// <summary>
        /// Atualiza a lista de NiveisOcupacionais do Programa.
        /// </summary>
        /// <param name="nivelOcupacional"></param>
        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivelOcupacional, Usuario usuario)
        {
            IList<NivelOcupacional> ListaNivelOcupacional = ListaPermissao.Where(x => x.NivelOcupacional != null).Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();
            EmailEnvioPermissao EmailEnvioPermissao = new EmailEnvioPermissao() { NivelOcupacional = nivelOcupacional, EmailEnvio = this };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                EmailEnvioPermissao.Auditoria.UsuarioAuditoria = usuario.CPF;
                this.ListaPermissao.Add(EmailEnvioPermissao);
            }
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            IList<NivelOcupacional> ListaNivelOcupacional = ListaPermissao.Where(x => x.NivelOcupacional != null).Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();

            if (ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }

        #endregion

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.EmailEnvio.ID == this.ID);

            if (!perfilEstaNaLista)
            {
                EmailEnvioPermissao EmailEnvioPermissao = new EmailEnvioPermissao() { Perfil = perfil, EmailEnvio = this };
                this.ListaPermissao.Add(EmailEnvioPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.EmailEnvio.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var solucaoEducacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                   x.Perfil.ID == perfil.ID && x.EmailEnvio.ID == this.ID);
                this.ListaPermissao.Remove(solucaoEducacionalASerExcluido);
            }

        }


        #endregion

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf, Usuario usuario)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.EmailEnvio.ID == this.ID);

            if (!ufEstaNaLista)
            {
                EmailEnvioPermissao EmailEnvioPermissao = new EmailEnvioPermissao() { Uf = uf, EmailEnvio = this };
                EmailEnvioPermissao.Auditoria.UsuarioAuditoria = usuario.CPF;
                this.ListaPermissao.Add(EmailEnvioPermissao);
            }
        }

        public virtual void RemoverUfs(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.EmailEnvio.ID == this.ID);

            if (ufEstaNaLista)
            {
                var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null &&
                                                                   x.Uf.ID == uf.ID && x.EmailEnvio.ID == this.ID);
                this.ListaPermissao.Remove(ufASerExcluido);
            }
        }

        #endregion

        #region "Permissão"

        public virtual void AdicionarPermissao(EmailEnvioPermissao permissao)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Perfil.ID == permissao.ID && x.EmailEnvio.ID == this.ID);

            if (!ufEstaNaLista)
            {
                //TODO -> Retestar este ponto
                EmailEnvioPermissao EmailEnvioPermissao = new EmailEnvioPermissao() { Perfil = permissao.Perfil, EmailEnvio = this };
                this.ListaPermissao.Add(EmailEnvioPermissao);
            }

        }

        public virtual void RemoverPermissao(EmailEnvioPermissao permissao)
        {

            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == permissao.ID && x.EmailEnvio.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                  x.Perfil.ID == permissao.ID && x.EmailEnvio.ID == this.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }

        }


        #endregion

        #region "Turma"

        public virtual void removerUsuarios()
        {
            IList<EmailEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Usuario != null && x.EmailEnvio.ID == this.ID).ToList();

            if (notificacoesEnvioPermissaoExcluir.Count() > 0)
            {

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }

            }
        }

        public virtual void removerTurmas()
        {
            bool turmaEstaNaLista = ListaPermissao.Any(x => x.Turma != null && x.EmailEnvio.ID == this.ID);

            if (turmaEstaNaLista)
            {
                IList<EmailEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Turma != null && x.EmailEnvio.ID == this.ID).ToList();

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }
            }
        }

        public virtual void removerTurmasExceto(Turma tur)
        {
            bool turmaEstaNaLista = ListaPermissao.Any(x => x.Turma != null && x.EmailEnvio.ID == this.ID);

            if (turmaEstaNaLista)
            {
                IList<EmailEnvioPermissao> notificacoesEnvioPermissaoExcluir = ListaPermissao.Where(x => x.Turma != null && x.Turma.ID != tur.ID && x.EmailEnvio.ID == this.ID).ToList();

                foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
                {
                    this.ListaPermissao.Remove(notExcluir);
                }
            }
        }

        #endregion

        #region "Usuario"

        public virtual void adicionarTurma(Turma tur, Usuario usuario)
        {
            bool turmaEstaNaLista = ListaPermissao.Any(x => x.Turma != null && x.Turma.ID == tur.ID && x.EmailEnvio.ID == this.ID);

            if (!turmaEstaNaLista)
            {
                //TODO -> Retestar este ponto
                EmailEnvioPermissao EmailEnvioPermissao = new EmailEnvioPermissao() { Turma = tur, EmailEnvio = this };
                EmailEnvioPermissao.Auditoria.UsuarioAuditoria = usuario.CPF;
                this.ListaPermissao.Add(EmailEnvioPermissao);
            }
        }

        public virtual void adicionarUsuario(Usuario user, Usuario usuario)
        {
            bool usuarioEstaNaLista = ListaPermissao.Any(x => x.Usuario != null && x.Usuario.ID == user.ID && x.EmailEnvio.ID == this.ID);

            if (!usuarioEstaNaLista)
            {
                //TODO -> Retestar este ponto
                EmailEnvioPermissao EmailEnvioPermissao = new EmailEnvioPermissao() { Usuario = user, EmailEnvio = this };
                EmailEnvioPermissao.Auditoria.UsuarioAuditoria = usuario.CPF;
                this.ListaPermissao.Add(EmailEnvioPermissao);
            }
        }

        #endregion

        public virtual void removerStatus(EmailEnvio emailEnvioEdicao)
        {
            IList<EmailEnvioPermissao> notificacoesEnvioPermissaoExcluir = emailEnvioEdicao.ListaPermissao.Where(x => x.Status != null).ToList();

            foreach (var notExcluir in notificacoesEnvioPermissaoExcluir)
            {
                this.ListaPermissao.Remove(notExcluir);
            }

        }

        public virtual void adicionarStatus(IEnumerable<StatusMatricula> listaStatusMatricula, Usuario usuario)
        {

            List<EmailEnvioPermissao> listaInserir = new List<EmailEnvioPermissao>();
            foreach (var status in listaStatusMatricula)
            {
                EmailEnvioPermissao emailEnvioPermissao = new EmailEnvioPermissao() { Status = status, EmailEnvio = this };
                emailEnvioPermissao.Auditoria.UsuarioAuditoria = usuario.CPF;
                this.ListaPermissao.Add(emailEnvioPermissao);
            }
        }
    }
}
