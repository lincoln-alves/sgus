using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterEmail : BusinessProcessBase
    {
        private BMEmail bmEmail;
        private BMUsuario usuarioBM;

        public ManterEmail()
            : base()
        {
            bmEmail = new BMEmail();
            listaConfiguracaoSistema = ConfiguracaoSistemaUtil.ObterTodasAsInformacoesDoTemplate();
        }
        #region "Propriedades"

        public static int PortaSMTP
        {
            get
            {
                return int.Parse(ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPPorta).Registro);
            }
        }

        public static string ServidorSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPServer).Registro;
            }
        }

        public static string UsuarioSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPUsuario).Registro;
            }
        }

        public static string SenhaSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPSenha).Registro;
            }
        }


        public static bool UsarSSLSMTP
        {
            get
            {
                return bool.Parse(ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPUsarSSL).Registro);
            }
        }

        public static string RemetenteSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPRemetente).Registro;
            }
        }
        public static string NomeRemetenteSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPRemetenteNome).Registro;
            }
        }

        public static string CodificacaoSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPCodificacao).Registro;
            }
        }

        public static string EmailErroSistema
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.EmailErroSistema).Registro;
            }
        }

        #endregion

        private static IList<ConfiguracaoSistema> ListaConfiguracaoSistema
        {
            get
            {
                return listaConfiguracaoSistema;
            }
        }

        private static IList<ConfiguracaoSistema> listaConfiguracaoSistema = null;

        // private ManterEmail()
        //{
        // listaConfiguracaoSistema = ConfiguracaoSistemaFacade.ObterTodasAsInformacoesDoTemplate();
        //}


        #region Singleton

        public static ManterEmail Instancia
        {
            get
            {

                if (Singleton.Instance == null)
                    return new ManterEmail();
                else
                    return Singleton.Instance;

            }
        }

        private class Singleton
        {
            static Singleton() { }
            internal static readonly ManterEmail Instance = new ManterEmail();
        }


        #endregion

        public void EnviarEmail(string emailDestinatario, string assunto, string mensagem,
            List<KeyValuePair<string, string>> linkedResources = null, bool throwException = true)
        {
            try
            {

                var sc = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Host = ServidorSMTP,
                    Port = PortaSMTP,
                    EnableSsl = UsarSSLSMTP
                };

                if (!string.IsNullOrEmpty(SenhaSMTP))
                    sc.Credentials = new NetworkCredential(UsuarioSMTP, SenhaSMTP);

                CommonHelper.EnviarEmail(emailDestinatario, assunto, mensagem, RemetenteSMTP, NomeRemetenteSMTP, sc,
                    linkedResources);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;
            }
        }

        public void FormatarEmailEnvioFormulario(EmailEnvio emailEnvioEdicao, int[] perfisSelecionados,
            int[] niveisOcupacionais, int[] ufs, int[] status, Turma turma, List<Usuario> usuarios)
        {
            var listUserIds = new List<int>();

            if (usuarios.Any())
            {
                listUserIds.AddRange(usuarios.Select(user => user.ID));
            }

            // Se tiver turma selecionada
            var selectedClass = emailEnvioEdicao.ListaPermissao.FirstOrDefault(x => x.Turma != null);
            var hashTags = new List<Tuple<Dictionary<string, string>, int>>();

            var matrTurma = selectedClass != null
                ? new ManterTurma().ObterTurmaPorID(selectedClass.Turma.ID).ListaMatriculas
                : null;

            if (matrTurma != null && matrTurma.Any())
            {
                // Caso não tenha nenhum status selecionado pega todos da turma, do contrário pega somente aqueles que possuam o status buscado
                var matriculasTurma =
                    matrTurma.Where(
                        x =>
                            (!status.Any() ||
                             status.Contains((int)x.MatriculaOferta.StatusMatricula)));

                if (matrTurma.Any())
                {
                    foreach (var mt in matriculasTurma)
                    {
                        var lnk = string.Format("<a href=\"http://{0}\">{0}</a>",
                            string.IsNullOrEmpty(mt.MatriculaOferta.LinkAcesso)
                                ? "www.uc.sebrae.com.br"
                                : mt.MatriculaOferta.LinkAcesso.Replace("http://", ""));

                        var hashTag = new Dictionary<string, string>
                        {
                            {"#NOMETURMA", mt.Turma.Nome},
                            {"#NOMEOFERTA", mt.Turma.Oferta.Nome},
                            {"#NOME_CURSO", mt.Turma.Oferta.SolucaoEducacional.Nome},
                            {"#LINK_CONFIRMAR_INSCRICAO", lnk},
                            {"#DATA_INSCRICAO", mt.MatriculaOferta.DataSolicitacao.ToString("dd/MM/yyyy")},
                            {"#DATA_TERMINO", mt.DataLimite.ToString("dd/MM/yyyy")}
                        };

                        var hashUsuario = new Tuple<Dictionary<string, string>, int>(hashTag,
                            mt.MatriculaOferta.Usuario.ID);
                        hashTags.Add(hashUsuario);
                    }
                }
                else
                {
                    throw new AcademicoException(
                        "Nenhum usuário inscrito na turma selecionado. Favor selecionar outra.");
                }
            }

            var usuariosSelecionados = listUserIds.ToArray();

            var manter = new ManterEmail();

            manter.PublicarEmail(emailEnvioEdicao,
                emailEnvioEdicao.Assunto,
                emailEnvioEdicao.Texto,
                hashTags,
                ufs,
                status,
                niveisOcupacionais,
                perfisSelecionados,
                turma,
                usuariosSelecionados);
        }

        public void PublicarEmail(EmailEnvio emailEnvio, string assunto, string texto,
            List<Tuple<Dictionary<string, string>, int>> hashTags, int[] ufs, int[] statusSelecionados,
            int[] niveisOcupacionais, int[] perfis, Turma turma = null, int[] usuarios = null)
        {
            var lstUf = new List<Uf>();
            var ufBm = new BMUf();

            if (ufs != null)
            {
                lstUf.AddRange(ufs.Select(uf => ufBm.ObterPorId(uf)));
            }

            var statusBm = new BMStatusMatricula();
            var lstStatus = statusSelecionados
                .Where(status => statusBm.ObterPorId(status) != null)
                .Select(status => statusBm.ObterPorId(status)).ToList();

            var lstNivel = new List<NivelOcupacional>();
            var nivelBm = new BMNivelOcupacional();
            if (niveisOcupacionais != null)
            {
                lstNivel.AddRange(niveisOcupacionais.Select(nivel => nivelBm.ObterPorID(nivel)));
            }

            var lstPerfil = new List<Perfil>();
            var perfilBm = new BMPerfil();

            if (perfis != null)
            {
                lstPerfil.AddRange(perfis.Select(perfil => perfilBm.ObterPorId(perfil)));
            }

            var lstUsuariosIds = new List<int>();
            usuarioBM = new BMUsuario();

            // Caso tenha algum filtro selecionado recupera os usuários filtrados
            if (ufs.Any() || lstStatus.Any() || lstNivel.Any() || lstPerfil.Any() || turma != null)
            {
                //var usuariosFiltrados = usuarioBM.ObterUsuariosParaEnvioEmail(lstUf, lstStatus, lstNivel, lstPerfil, turma);

                var statusIds = lstStatus.Select(x => x.ID).ToList();
                var niveisIds = lstNivel.Select(x => x.ID).ToList();
                var perfisIds = lstPerfil.Select(x => x.ID).ToList();

                var usuariosFiltradosIds = new ManterUsuario().ObterTodosIQueryable()
                    .Join(new BMUf().ObterTodosIQueryable(), u => u.UF.ID, uf => uf.ID,
                        (usuario, uf) => new {usuario, uf})
                    .Join(new ManterUsuarioPerfil().ObterTodosIQueryable(), join => join.usuario.ID,
                        usuarioPerfil => usuarioPerfil.Usuario.ID, (lastJoin, usuarioPerfil) =>
                            new
                            {
                                lastJoin.usuario,
                                lastJoin.uf,
                                usuarioPerfil
                            })
                    .Join(new ManterMatriculaOferta().ObterTodosIQueryable(), join => join.usuario.ID,
                        matricula => matricula.Usuario.ID,
                        (lastJoin, matriculaOferta) =>
                            new
                            {
                                lastJoin.usuario,
                                lastJoin.uf,
                                lastJoin.usuarioPerfil,
                                matriculaOferta
                            })
                    .Join(new ManterMatriculaTurma().ObterTodosIQueryable(), join => join.matriculaOferta.ID,
                        matricula => matricula.MatriculaOferta.ID,
                        (lastJoin, matriculaTurma) =>
                            new
                            {
                                lastJoin.usuario,
                                lastJoin.uf,
                                lastJoin.usuarioPerfil,
                                lastJoin.matriculaOferta,
                                matriculaTurma
                            })
                    .Where(x =>
                        (!ufs.Any() || ufs.Contains(x.uf.ID))
                        && (!statusIds.Any() || statusIds.Contains((int) x.matriculaOferta.StatusMatricula))
                        && (!niveisIds.Any() || niveisIds.Contains(x.usuario.NivelOcupacional.ID))
                        && (!perfisIds.Any() || perfisIds.Contains(x.usuarioPerfil.Perfil.ID))
                        && (turma == null || x.matriculaTurma.Turma.ID == turma.ID))
                    .Select(x => new
                    {
                        x.usuario.ID
                    })
                    .Distinct()
                    .ToList()
                    .Select(x => x.ID)
                    .ToList();

                lstUsuariosIds.AddRange(usuariosFiltradosIds);
            }

            // Adiciona usuários avulsos nos filtros gerais
            if (usuarios != null && usuarios.Any())
            {
                var lstUsuarioSelecionados = usuarios.Select(usuario => usuarioBM.ObterPorId(usuario)).Where(x => !lstUsuariosIds.Contains(x.ID));

                lstUsuariosIds.AddRange(lstUsuarioSelecionados.Where(u => u.Ativo).Select(x => x.ID));
            }

            var cpfLogado = usuarioBM.ObterUsuarioLogado().CPF;

            GerarEmailBaseEmail(lstUsuariosIds, assunto, texto, hashTags, cpfLogado, emailEnvio);
        }

        private void GerarEmailBaseEmail(IList<int> usuariosIds, string assunto, string texto,
            List<Tuple<Dictionary<string, string>, int>> hashTags, string cpfLogado, EmailEnvio emailEnvio)
        {
            var count = 0;
            var endPortal =
                ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.EnderecoPortal)?
                    .Registro;
            var endSgus =
                ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.EnderecoSGUS)?
                    .Registro;

            foreach (var u in usuariosIds)
            {
                if (emailEnvio.ListaEmailsGerados.Any(x => x.Usuario.ID == u))
                    continue;

                var usuario = usuarioBM.ObterPorId(u);

                var textoEmailUsuario = texto.Replace("#NOME_ALUNO", usuario.Nome)
                    .Replace("#CPF", usuario.CPF)
                    .Replace("#UF", usuario.UF.Nome)
                    .Replace("#EMAIL", usuario.Email)
                    .Replace("#SENHA", CriptografiaHelper.Decriptografar(usuario.Senha))
                    .Replace("#URL_PORTAL", endPortal)
                    .Replace("#URL_SGUS", endSgus);

                if (hashTags != null)
                {
                    textoEmailUsuario = hashTags.Where(x => x.Item2 == usuario.ID)
                        .Aggregate(textoEmailUsuario,
                            (current1, registro) =>
                                registro.Item1.Aggregate(current1,
                                    (current, item) => current.Replace(item.Key, item.Value)));
                }

                var email = new Email
                {
                    DataGeracao = DateTime.Now,
                    Assunto = assunto,
                    TextoEmail = textoEmailUsuario,
                    Enviado = false,
                    Usuario = usuario,
                    EmailEnvio = emailEnvio,
                    Auditoria = new Auditoria(cpfLogado)
                };

                count++;
                bmEmail.SalvarSemCommit(email);

                if (count < 300)
                    continue;

                bmEmail.Commit();
                count = 0;
            }

            bmEmail.Commit();
        }

        public IQueryable<Email> ObterPorFiltro(Usuario usuario, DateTime? dataDeEnvio, int idEmailEnvio)
        {
            var query = bmEmail.ObterTodosIqueryable();

            query = query.Where(x => x.EmailEnvio.ID == idEmailEnvio);

            if (usuario == null) return query;

            if (!string.IsNullOrEmpty(usuario.Nome))
                query = query.Where(x => x.Usuario.Nome.Contains(usuario.Nome));

            if (!string.IsNullOrEmpty(usuario.CPF))
                query = query.Where(x => x.Usuario.CPF == usuario.CPF);

            if (!string.IsNullOrEmpty(usuario.Email))
                query = query.Where(x => x.Usuario.Email == usuario.Email);

            if (dataDeEnvio.HasValue)
            {
                query = query.Where(x => x.DataEnvio.HasValue && x.DataEnvio.Value.Date == dataDeEnvio.Value.Date);
            }

            return query;
        }

        public IQueryable<Email> ObterPorEmailEnvio(EmailEnvio email)
        {
            return bmEmail.ObterTodosIqueryable().Where(x => x.EmailEnvio.ID == email.ID);
        }
    }
}