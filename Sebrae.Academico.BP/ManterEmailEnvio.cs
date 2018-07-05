using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterEmailEnvio : BusinessProcessBase, IDisposable
    {
        private BMEmailEnvio bmEmailEnvio;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterEmailEnvio()
            : base()
        {
            bmEmailEnvio = new BMEmailEnvio();
        }

        public void IncluirEmailEnvio(EmailEnvio EmailEnvio)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(EmailEnvio);
                bmEmailEnvio.Salvar(EmailEnvio);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarEmailEnvio(EmailEnvio EmailEnvio, bool resetarEmailsNaoEnviados = false)
        {
            this.PreencherInformacoesDeAuditoria(EmailEnvio);

            // Retira e-mails não enviados da lista de envio
            if (resetarEmailsNaoEnviados)
            {
                RemoverEmailsGerados(EmailEnvio);

                // Coloca somente os itens enviados na lista
                EmailEnvio.ListaEmailsGerados = new BMEmail().ObterPorEmailEnvio(EmailEnvio.ID, true).ToList();
            }

            bmEmailEnvio.Salvar(EmailEnvio);
        }

        public EmailEnvio RemoverEmailsGerados(EmailEnvio email)
        {
            var bmEmail = new BMEmail();

            var emailsRemover = bmEmail.ObterPorEmailEnvio(email.ID, false);

            // Remove os itens já removidos
            bmEmailEnvio.ExcluirTodos(emailsRemover);

            return email;
        }

        public IList<EmailEnvio> ObterTodasTurma()
        {
            return bmEmailEnvio.ObterTodos();
        }

        public EmailEnvio ObterEmailEnvioPorID(int pId)
        {
            return bmEmailEnvio.ObterPorID(pId);
        }

        public IList<EmailEnvio> ObterEmailEnvioPorFiltro(EmailEnvio EmailEnvio)
        {
            return bmEmailEnvio.ObterPorFiltro(EmailEnvio);
        }

        public void ExcluirEmailEnvio(int idNotificacao)
        {
            try
            {
                EmailEnvio notificacao = null;

                if (idNotificacao > 0)
                {
                    notificacao = bmEmailEnvio.ObterPorID(idNotificacao);
                }

                bmEmailEnvio.Excluir(notificacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void LimparSessao()
        {
            bmEmailEnvio.LimparSessao();
        }

        public IEnumerable<Email> ObterEmailsParaEnvio(EmailEnvio emailEnvio, bool ignorarEnviados = false)
        {
            var bmEmail = new BMEmail();
            var list = bmEmail.ObterPorEmailEnvio(emailEnvio.ID, null);

            list = list.Where(x => x.Usuario != null && x.Usuario.Situacao.ToLower() == "ativo");

            if (!ignorarEnviados)
                list = list.Where(x => x.Enviado == false);

            if (emailEnvio.ListaPermissao.Where(x => x.Usuario != null).Any())
            {
                var usuarios = emailEnvio.ListaPermissao.Where(x => x.Usuario != null).Select(y => y.Usuario.ID).ToList();
                list = list.Where(x => usuarios.Contains(x.Usuario.ID));
            }

            if (emailEnvio.ListaPermissao.Where(x => x.Perfil != null).Any())
            {
                var perfis = emailEnvio.ListaPermissao.Where(x => x.Perfil != null).Select(y => y.Perfil.ID).ToList();
                list = list.Where(x => x.Usuario.ListaPerfil.Any(y => perfis.Contains(y.Perfil.ID)));
            }

            if (emailEnvio.ListaPermissao.Where(x => x.NivelOcupacional != null).Any())
            {
                var niveiscupacionais = emailEnvio.ListaPermissao.Where(x => x.NivelOcupacional != null).Select(y => y.NivelOcupacional.ID).ToList();
                list = list.Where(x => niveiscupacionais.Contains(x.Usuario.NivelOcupacional.ID));
            }

            if (emailEnvio.ListaPermissao.Where(x => x.Uf != null).Any())
            {
                var ufs = emailEnvio.ListaPermissao.Where(x => x.Uf != null).Select(y => y.Uf.ID).ToList();
                list = list.Where(x => ufs.Contains(x.Usuario.UF.ID));
            }

            return list;
        }

        public IQueryable<Usuario> ObterUsuariosParaEnvio(EmailEnvio emailEnvio)
        {
            var bmUsuario = new BMUsuario();
            var list = bmUsuario.ObterTodosIQueryable().Where(x => x.Situacao.ToLower() == "ativo");

            var userIds = new List<int>();
            var perfisIds = new List<int>();
            var niveisIds = new List<int>();
            var ufsIds = new List<int>();
            var turmasIds = new List<int>();

            if (emailEnvio.ListaPermissao.Any(x => x.Usuario != null))
            {
                userIds.AddRange(emailEnvio.ListaPermissao.Where(x => x.Usuario != null).Select(x => x.ID).ToList());
            }

            if (emailEnvio.ListaPermissao.Any(x => x.Perfil != null))
            {
                perfisIds.AddRange(
                    emailEnvio.ListaPermissao.Where(x => x.Perfil != null).Select(y => y.Perfil.ID).ToList());
            }

            if (emailEnvio.ListaPermissao.Any(x => x.NivelOcupacional != null))
            {
                niveisIds.AddRange(
                    emailEnvio.ListaPermissao.Where(x => x.NivelOcupacional != null)
                        .Select(y => y.NivelOcupacional.ID)
                        .ToList());
            }

            if (emailEnvio.ListaPermissao.Any(x => x.Uf != null))
            {
                ufsIds.AddRange(emailEnvio.ListaPermissao.Where(x => x.Uf != null).Select(y => y.Uf.ID).ToList());
            }

            if (emailEnvio.ListaPermissao.Any(x => x.Turma != null))
            {
                turmasIds.AddRange(emailEnvio.ListaPermissao.Where(x => x.Turma != null).Select(y => y.Turma.ID).ToList());
            }

            return list.Where(u =>
                userIds.Contains(u.ID) ||
                u.ListaPerfil.Any(p => perfisIds.Contains(p.Perfil.ID)) ||
                niveisIds.Contains(u.NivelOcupacional.ID) ||
                ufsIds.Contains(u.UF.ID) ||
                u.ListaMatriculaOferta.Any(mo => mo.MatriculaTurma.Any(mt => turmasIds.Contains(mt.Turma.ID)))
                );
        }

        public void Dispose()
        {
            bmEmailEnvio.Dispose();
        }
    }
}
