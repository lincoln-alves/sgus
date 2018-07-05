using System;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.Services
{
    public class NotificarLiberacaoNovaProvaTrilhaService : BusinessProcessServicesBase
    {

        // Notificar Alunos Aprovados no Questionario Pos e Manter o log de envio
        public RetornoWebService NotificarLiberacaoNovaProvaTrilha()
        {
            var retorno = new RetornoWebService();

            try
            {
                EnviarEmailNovaProva();
            }
            catch (Exception)
            {
                throw;
            }
            
            return retorno;
        }

        //Corpo
        private const string CorpoEmail = "<p>Prezado(a) {0},<br />A nova data para você refazer a prova final da trilha de aprendizagem {1} - Nível: {2}, Chegou!.</p><p>Acesse sua trilha e refaça sua prova!</p><p>Atenciosamente<br />Nina, Sua Guia de trilhas.</p>";
        private string ObterMensagem(UsuarioTrilha usuarioTrilha)
        {
            return string.Format(
                CorpoEmail, 
                usuarioTrilha.Usuario.Nome, 
                usuarioTrilha.TrilhaNivel.Trilha.Nome, 
                usuarioTrilha.TrilhaNivel.Nome
            );
        }

        //Enviar email informando que está liberado uma nova prova final 
        public void EnviarEmailNovaProva()
        {
            var mail = new ManterEmail();
            var confSistema = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoPortal30);
            var listaNovaProva = new ManterUsuarioTrilha().ObterTodosUsuarioTrilha()
                .Where(x => x.DataLimite != null && x.DataLiberacaoNovaProva != null && x.DataLiberacaoNovaProva.Value.Date == DateTime.Now.Date && x.Usuario.Email != null);

            foreach (var prova in listaNovaProva)
            {
                var mensagem = ObterMensagem(prova);
                var assunto = "Nova data da prova final " + prova.TrilhaNivel.Trilha.Nome + " - " + prova.TrilhaNivel.Nome;
                mail.EnviarEmail(prova.Usuario.Email, assunto, mensagem, throwException: false);
            }
        }
    }
}