using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.Services
{
    public class NotificarAlunosQuestionarioEficacia : BusinessProcessServicesBase
    {
        public RetornoWebService NotificarQuestionarioEficacia()
        {
            var retorno = new RetornoWebService();

            try
            {
                EnviarQuestionarioEficacia();
            }
            catch (Exception)
            {
                throw;
            }

            return retorno;
        }

        private void EnviarQuestionarioEficacia()
        {
            // Recupera lista de usuários que devem receber as notificações
            var questionarios = new ManterQuestionarioAssociacao().ObterTodosEficaciaDia();

            var mail = new ManterEmail();

            foreach (var questionario in questionarios)
            {
                foreach (var matricula in questionario.Turma.ListaMatriculas)
                {
                    var mensagem = ObterMensagem(questionario.Turma, matricula.MatriculaOferta.Usuario);
                    var assunto = "Questionário de eficácia";
                    mail.EnviarEmail(matricula.MatriculaOferta.Usuario.Email, assunto, mensagem, throwException: false);
                }
            }
        }

        private string ObterMensagem(Turma turma, Usuario usuario)
        {
            // Obtendo template 
            var template = new ManterTemplate().ObterTemplatePorID((int)enumTemplate.QuestionarioEficacia);
            var confSistema = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoPortal30);

            var sid = usuario.SID_Usuario;
            var link = "";

            var linkRedirect = string.Format("minhasinscricoes/questionario/eficacia/turma/{0}", turma.ID);

            link = !string.IsNullOrEmpty(sid) ? string.Format("{0}networkLoginAuto/{1}/{2}",
                confSistema.Registro,
                System.Web.HttpContext.Current.Server.UrlEncode(Base64Encode(InfraEstrutura.Core.Helper.CriptografiaHelper.Criptografar(sid)).ToString()),
                Base64Encode(linkRedirect)) :  confSistema.Registro + linkRedirect;

            var texto = template.TextoTemplate;

            texto = texto.Replace("#ALUNO", usuario.Nome);
            texto = texto.Replace("#SOLUCAOEDUCACIONAL", turma.NomeSolucaoEducacional);
            texto = texto.Replace("#LINK", link);

            return texto;
        }

        //Codifica para BAse 64
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
