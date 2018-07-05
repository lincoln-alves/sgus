using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterNotificacaoEnvio : BusinessProcessBase
    {
        private BMNotificacaoEnvio bmNotificacaoEnvio;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterNotificacaoEnvio()
            : base()
        {
            bmNotificacaoEnvio = new BMNotificacaoEnvio();
        }

        public void IncluirNotificacaoEnvio(NotificacaoEnvio notificacaoEnvio)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(notificacaoEnvio);
                bmNotificacaoEnvio.Salvar(notificacaoEnvio);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarNotificacaoEnvio(NotificacaoEnvio notificacaoEnvio)
        {
            this.PreencherInformacoesDeAuditoria(notificacaoEnvio);
            bmNotificacaoEnvio.Salvar(notificacaoEnvio);
        }

        public IList<NotificacaoEnvio> ObterTodasTurma()
        {
            return bmNotificacaoEnvio.ObterTodos();
        }

        public NotificacaoEnvio ObterNotificacaoEnvioPorID(int pId)
        {
            return bmNotificacaoEnvio.ObterPorID(pId);
        }

        public IQueryable<NotificacaoEnvio> ObterTodosIQueryable()
        {
            return bmNotificacaoEnvio.ObterTodosIQueryable();
        }

        public IList<NotificacaoEnvio> ObterNotificacaoEnvioPorFiltro(NotificacaoEnvio notificacaoEnvio)
        {
            return bmNotificacaoEnvio.ObterPorFiltro(notificacaoEnvio);
        }

        public void ExcluirNotificacaoEnvio(int idNotificacao)
        {
            try
            {
                NotificacaoEnvio notificacao = null;

                if (idNotificacao > 0)
                {
                    notificacao = bmNotificacaoEnvio.ObterPorID(idNotificacao);
                }

                bmNotificacaoEnvio.Excluir(notificacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void EmiteNotificacao(NotificacaoEnvio notificacaoEnvio)
        {
            // Notifica os usuários
            var perfis = notificacaoEnvio.ListaPermissao.Where(x => x.Perfil != null).Select(x => x.Perfil).ToList();
            var niveis =
                notificacaoEnvio.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => x.NivelOcupacional)
                    .ToList();
            var ufs = notificacaoEnvio.ListaPermissao.Where(x => x.Uf != null).Select(x => x.Uf).ToList();
            var usuarios = notificacaoEnvio.ListaPermissao.Where(x => x.Usuario != null).Select(x => x.Usuario).ToList();

            // Se tiver turma selecionada
            var permissaoTurma = notificacaoEnvio.ListaPermissao.FirstOrDefault(x => x.Turma != null);

            if (permissaoTurma != null)
            {
                var matriculasTurma = permissaoTurma.Turma.ListaMatriculas;

                if (matriculasTurma.Any())
                {
                    var idsUsuarios = usuarios.Select(x => x.ID).ToList();

                    usuarios.AddRange(
                        matriculasTurma.Where(
                            x =>
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito &&

                                // Remover usuários que já estão na lista.
                                !idsUsuarios.Contains(x.MatriculaOferta.Usuario.ID))

                            // Selecionar apenas o ID, que é o que interessa.
                            .Select(mt => new Usuario {ID = mt.MatriculaOferta.Usuario.ID}));
                }
            }

            // Obter os usuários do restante das permissões apenas caso algum filtro de permissão tenha sido informado.
            if (ufs.Any() || niveis.Any() || perfis.Any())
            {
                var usuariosUfsNiveisPerfis = new BMUsuario().ObterPorUfsNiveisPerfis(ufs, niveis, perfis).ToList();

                if (usuariosUfsNiveisPerfis.Any())
                {
                    var idsUsuarios = usuarios.Select(x => x.ID).ToList();

                    // Adicionar somente os usuários que já não estão na lista.
                    usuarios.AddRange(usuariosUfsNiveisPerfis.Where(x => !idsUsuarios.Contains(x.ID)));
                }
            }

            new ManterNotificacao().PublicarNotificacao(notificacaoEnvio.Link,
                notificacaoEnvio.Texto,
                usuarios.AsQueryable());
        }

        public IEnumerable<Usuario> CompilarUsuarios(NotificacaoEnvio notificacaoEnvioEdicao)
        {
            var parametros = new Dictionary<string, object>();

            var ufs = string.Join(",",
                notificacaoEnvioEdicao.ListaPermissao.Where(x => x.Uf != null)
                    .Select(x => new {x.Uf.ID})
                    .Select(x => x.ID)
                    .Distinct()
                    .ToList());

            if (!string.IsNullOrWhiteSpace(ufs))
                parametros.Add("Ufs", ufs);

            var niveis = string.Join(",",
                notificacaoEnvioEdicao.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new {x.NivelOcupacional.ID})
                    .Select(x => x.ID)
                    .Distinct()
                    .ToList());

            if (!string.IsNullOrWhiteSpace(niveis))
                parametros.Add("NiveisOcupacionais", niveis);

            var perfis = string.Join(",",
                notificacaoEnvioEdicao.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new { x.Perfil.ID })
                    .Select(x => x.ID)
                    .Distinct()
                    .ToList());

            if (!string.IsNullOrWhiteSpace(perfis))
                parametros.Add("Perfis", perfis);

            var status = string.Join(",",
                notificacaoEnvioEdicao.ListaPermissao.Where(x => x.Status != null)
                    .Select(x => new { x.Status.ID })
                    .Select(x => x.ID)
                    .Distinct()
                    .ToList());

            if (!string.IsNullOrWhiteSpace(status))
                parametros.Add("Status", status);

            var permissaoTurma = notificacaoEnvioEdicao.ListaPermissao.FirstOrDefault(x => x.Turma != null);

            if (permissaoTurma != null)
                parametros.Add("Turma", permissaoTurma.Turma.ID);

            var usuarios = string.Join(",",
                notificacaoEnvioEdicao.ListaPermissao.Where(x => x.Usuario != null)
                    .Select(x => new { x.Usuario.ID })
                    .Select(x => x.ID)
                    .Distinct()
                    .ToList());

            if (!string.IsNullOrWhiteSpace(usuarios))
                parametros.Add("Usuarios", usuarios);

            var usuariosResult = bmNotificacaoEnvio.ExecutarProcedure<Usuario>("SP_COMPILAR_USUARIOS_NOTIFICACAO", parametros);

            return usuariosResult;
        }
    }
}
