using System;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Services
{
    public class NotificarAlunosAprovadosTurmaQuestionarioPosService : BusinessProcessServicesBase
    {
        public RetornoWebService NotificarAlunosAprovadosTurmaQuestionarioPos()
        {
            var retorno = new RetornoWebService();

            var matriculasTurma = new ManterMatriculaTurma().ObterDeHoje();
            var mail = new ManterEmail();
            var notificacao = new ManterNotificacao();
            var confSistema = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoPortal30Dev);


            foreach (var matriculaTurma in matriculasTurma)
            {
                var turma = matriculaTurma.Turma.Nome;
                var idTurma = matriculaTurma.Turma.ID;
                var email = matriculaTurma.MatriculaOferta.Usuario.Email;
                var usuario = matriculaTurma.MatriculaOferta.Usuario.Nome;
                var idUsuario = matriculaTurma.MatriculaOferta.Usuario.ID;
                var sidUsuario = matriculaTurma.MatriculaOferta.Usuario.SID_Usuario;
                
                var linkRedirect = string.Format("minhasinscricoes/questionario/{0}/turma/{1}", (int)enumTipoQuestionarioAssociacao.Pos, idTurma);
                var link = string.Format("{0}networkLoginAuto/{1}/{2}",
                    confSistema.Registro,
                    System.Web.HttpContext.Current.Server.UrlEncode(Base64Encode(InfraEstrutura.Core.Helper.CriptografiaHelper.Criptografar(sidUsuario))), 
                    Base64Encode(linkRedirect));

                var mensagem = ObterCorpo(usuario, turma, link);
                var assunto = "Questionário de avaliação do curso: " + turma;
                mail.EnviarEmail(email, assunto, mensagem);

                notificacao.PublicarNotificacao(linkRedirect, mensagem, idUsuario);
            }

            return retorno;
        }

        private const string CorpoEmail = "<p>Prezado, {0}<br />Você foi aprovado na {1} e deverá responder o questionário de avaliação para emissão do seu certificado. Clique do link abaixo e responda as perguntas. Após o envio do mesmo. O  certificado ficará disponível em seu histórico acadêmico no portal UC.</p><p>Link para acesso ao questionário: <a href='{2}'>Clique aqui</a></p><p>Atenciosamente<br />Universidade Corporativa Sebrae</p>";

        private string ObterCorpo(string nome, string turma, string link)
        {
            return string.Format(CorpoEmail, nome, turma, link);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}