using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Views;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterEmailService
    {
        public void NotificarComentarioPortal(int IDUsuario, int IDUsuarioComentario, string urlComentario, string comentario)
        {
            Usuario usuario;
            Usuario usuarioComentario;

            try
            {
                using (var manterUsuario = new ManterUsuario())
                {
                    usuario = manterUsuario.ObterUsuarioPorID(IDUsuario);
                    usuarioComentario = manterUsuario.ObterUsuarioPorID(IDUsuarioComentario);
                }

                var template = @"Prezado(a) {0}, <br/><br/>
                Seu comentário foi respondido por {1} com a mensagem: 
                {2} 
                <br/><br/>
                Clique aqui {3} e visualize a resposta. <br/><br/>
                Atenciosamente, <br/><br/>
                UNIVERSIDADE CORPORATIVA SEBRAE";

                template = string.Format(template, usuario.Nome, usuarioComentario.Nome, comentario, urlComentario);

                new ManterEmail().EnviarEmail(usuarioComentario.Email, "Novo Comentário", template);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void NotificarDemitidos()
        {
            var usuarios = new ViewUsuariosDemitidosServices().ObterUsuariosDemitidosHoje();
            if (usuarios.Count > 0)
            {
                var texto = ObterTemplateUsuariosDemitidos(usuarios);

                var usuarioNotificar = new ManterUsuario().ObterPorCPF("81771053100");
                new ManterEmail().EnviarEmail(usuarioNotificar.Email, "Notificação de usuários demitidos", texto);
            }
        }

        private string ObterTemplateUsuariosDemitidos(IList<ViewUsuariosDemitidos> demitidos)
        {
            StringBuilder texto = new StringBuilder();

            texto.Append("Usuários demitidos hoje " + DateTime.Now.Date.ToString("dd/MM/yyyy") + " <br />");
            texto.Append("<table><thead><th>Nome</th><th>CPF</th><th>Matricula</th><th>Data de Demissão</th></thead><tbody>");

            foreach (var usuario in demitidos)
            {
                texto.AppendLine("<tr><td>"+ usuario.Nome + "</td><td>" + usuario.CPF + "</td><td>" + usuario.Matricula + "</td><td>" 
                    + usuario.DataDemissao.ToString("dd/MM/yyyy") + "</td></tr>");
            }

            return texto.ToString();
        }
    }
}
