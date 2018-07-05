using System;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Services
{
    public class NotificarAlunosAprovadosTurmaQuestionarioPosService : BusinessProcessServicesBase
    {
        //Intervalo de Tempo para busca de Registros
        public const int Intervalo = 10;

        // Notificar Alunos Aprovados no Questionario Pos e Manter o log de envio
        public RetornoWebService NotificarAlunosAprovadosTurmaQuestionarioPos()
        {
            var retorno = new RetornoWebService();

            try
            {
                InserirAlunosPrimeiroEnvio(enumStatusMatricula.Aprovado);
                NotificarAlunosQuestionarioAssociacaoEnvio();

                //Inativar Questionarios nao respondidos mais que 30 dias do primerio envio.
                new ManterQuestionarioAssociacaoEnvio().Inativar(Intervalo);

            }
            catch (Exception)
            {
                throw;
            }
            
            return retorno;
        }

        //Corpo de e-mail
        private const string CorpoEmail = "<p>Prezado, {0}<br />Você foi aprovado na {1} e deverá responder o questionário de avaliação para emissão do seu certificado. Clique do link abaixo e responda as perguntas. Após o envio do mesmo. O  certificado ficará disponível em seu histórico acadêmico no portal UC.</p><p>Link para acesso ao questionário: <a href='{2}'>Clique aqui</a></p><p>Atenciosamente<br />Universidade Corporativa Sebrae</p>";
        private string ObterCorpo(string nome, string turma, string link)
        {
            return string.Format(CorpoEmail, nome, turma, link);
        }

        //Codifica para BAse 64
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //Notifica os Alunos de Primeiro envio
        //Em seguida inserir no fluxo de Log de Envio de e-mail.
        public void InserirAlunosPrimeiroEnvio(enumStatusMatricula? statusMatricula = null)
        {
            var matriculasTurma = new ManterMatriculaTurma().ObterPorIntervalo(Intervalo, statusMatricula);
            var manterQuestionarioAssociacaoEnvio = new ManterQuestionarioAssociacaoEnvio();

            foreach (var matriculaTurma in matriculasTurma)
            {
                manterQuestionarioAssociacaoEnvio.Inserir(matriculaTurma);
            }
        }

        //Notifica os Alunos que já tiveram o primeiro envio e mantem a logica de 3 envio 1 a cada 7 dias e inativa o maior de 30 dias
        public void NotificarAlunosQuestionarioAssociacaoEnvio()
        {
            var mail = new ManterEmail();
            var notificacao = new ManterNotificacao();
            var confSistema = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoPortal30);

            var manterQuestionariosAssociacaoEnvio = new ManterQuestionarioAssociacaoEnvio();
            var questionariosAssociacaoEnvio = manterQuestionariosAssociacaoEnvio.ObterPorIntervalo(Intervalo);
            foreach (var questionarioAssociacaoEnvio in questionariosAssociacaoEnvio)
            {
                var dtoNotificarAluno = new DTONotificarAlunoQuestionarioPos()
                {
                    Usuario = questionarioAssociacaoEnvio.Usuario,
                    Turma = questionarioAssociacaoEnvio.QuestionarioAssociacao.Turma
                };

                NotificarAlunos(dtoNotificarAluno, mail, notificacao, confSistema);
                manterQuestionariosAssociacaoEnvio.Atualizar(questionarioAssociacaoEnvio);
            }
        }


        //Notificar Alunos
        public DTONotificarAlunoQuestionarioPos NotificarAlunos(DTONotificarAlunoQuestionarioPos dtoNotificarAluno, ManterEmail mail, ManterNotificacao notificacao, ConfiguracaoSistema confSistema)
        {
            var turma = dtoNotificarAluno.Turma.Nome;
            var idTurma = dtoNotificarAluno.Turma.ID;
            var email = dtoNotificarAluno.Usuario.Email;
            var usuario = dtoNotificarAluno.Usuario.Nome;
            var idUsuario = dtoNotificarAluno.Usuario.ID;
            var sidUsuario = dtoNotificarAluno.Usuario.SID_Usuario;

            var linkRedirect = string.Format("minhasinscricoes/questionario/pos/turma/{0}", idTurma);
            var link = string.Format("{0}networkLoginAuto/{1}/{2}",
                confSistema.Registro,
                System.Web.HttpContext.Current.Server.UrlEncode(Base64Encode(InfraEstrutura.Core.Helper.CriptografiaHelper.Criptografar(sidUsuario))),
                Base64Encode(linkRedirect));

            var mensagem = ObterCorpo(usuario, turma, link);
            var assunto = "Questionário de avaliação do curso: " + turma;
            mail.EnviarEmail(email, assunto, mensagem, throwException: false);

            //notificacao.PublicarNotificacao(linkRedirect, mensagem, idUsuario);

            return dtoNotificarAluno;
        }
    }

    public class DTONotificarAlunoQuestionarioPos
    {
        public Turma Turma { get; set; }
        public Usuario Usuario { get; set; }
    }

}