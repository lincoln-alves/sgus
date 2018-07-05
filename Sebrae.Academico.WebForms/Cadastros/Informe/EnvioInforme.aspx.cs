using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using AutoMapper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Informe
{
    public partial class EnvioInforme : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucPermissoes.PreencherListas();

                classes.Informe informe;

                int informeId;

                if (Request["Id"] == null ||
                    !int.TryParse(Request["Id"], out informeId) ||
                    (informe = new ManterInforme().ObterPorId(informeId)) == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Informe inválido.", "ListarInformes.aspx");
                }
                else
                {
                    if (!informe.Turmas.Any())
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao,
                            "Informe não possui turmas selecionadas. Selecione turmas pra configurar o envio do informe.",
                            "EdicaoInforme.aspx?Id=" + informe.ID);
                    else
                    {
                        PreencherDados(informe);
                    }
                }
            }
        }

        private void PreencherDados(classes.Informe informe)
        {
            var envio = ObterObjeto(false, informe);

            if (envio.Usuario != null)
                ucLupaUsuario.SelectedUser = envio.Usuario;

            if (envio.Perfis != null && envio.Perfis.Any())
                ucPermissoes.SelecionarPerfis(envio.Perfis);

            if (envio.NiveisOcupacionais != null && envio.NiveisOcupacionais.Any())
                ucPermissoes.SelecionarNiveisOcupacionais(envio.NiveisOcupacionais);

            if (envio.Ufs != null && envio.Ufs.Any())
                ucPermissoes.SelecionarUfs(envio.Ufs);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoInforme.aspx?Id=" + Request["Id"]);
        }

        /// <summary>
        /// Obtém ou cria um novo envio. Se existirem envios não enviados, os busca para preencher a tela ou para que sejam enviados.
        /// Se não existem envios enviados, cria um novo envio baseado no último envio que foi enviado.
        /// Caso nunca tenha havido um envio, traz um objeto novo obtendo os dados da tela.
        /// </summary>
        /// <param name="obterDadosDaTela">Caso true, obtém os dados do objeto a partir da tela. Caso false, obtém o objeto clone ou um novo objeto.</param>
        /// <param name="informe">Caso já existe um informe buscado, passá-lo como esse parâmetro para evitar mais uma consulta ao banco.</param>
        /// <returns></returns>
        private classes.EnvioInforme ObterObjeto(bool obterDadosDaTela, classes.Informe informe = null)
        {
            informe = informe ?? new ManterInforme().ObterPorId(int.Parse(Request["Id"]));

            classes.EnvioInforme envio;

            if (informe.Envios.Any(x => !x.Enviado()))
            {
                // Caso já tenha envios não enviados, busca os dados desse envio e preenche a tela.
                envio = informe.Envios.FirstOrDefault(x => !x.Enviado());
            }
            else
            {
                envio = new classes.EnvioInforme();

                // Busca o último envio, caso existente.
                var ultimoEnvio = informe.Envios.LastOrDefault();

                if (ultimoEnvio != null)
                    Mapper.Map(ultimoEnvio, envio);
            }

            // Vai que... né?
            if (envio == null)
                envio = new classes.EnvioInforme();


            // Caso esteja chamando para obter os dados da tela.
            if (obterDadosDaTela)
            {
                envio.Informe = new ManterInforme().ObterPorId(int.Parse(Request["Id"]));
                envio.Usuario = ucLupaUsuario.SelectedUser;

                // Buscar perfis; níveis ocupacionais e UFs das permissões.
                envio.Perfis = ucPermissoes.ObterPerfisSelecionados();
                envio.NiveisOcupacionais = ucPermissoes.ObterNiveisOcupacionaisSelecionados();
                envio.Ufs = ucPermissoes.ObterUfsSelecionadas();
            }

            return envio;
        }

        protected void btnSalvar_OnClick(object sender, EventArgs e)
        {
            try
            {
                var envio = SalvarEnvio();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados gravados com sucesso.", "EnvioInforme.aspx?Id=" + envio.Informe.ID);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private classes.EnvioInforme SalvarEnvio()
        {
            var envio = ObterObjeto(true);

            new ManterEnvioInforme().Salvar(envio);
            return envio;
        }

        protected void btnEnviar_OnClick(object sender, EventArgs e)
        {
            // Salvar caso esteja enviando sem salvar.
            var envio = SalvarEnvio();

            ExecutarThreadEnvioInforme(envio);
        }

        private void ExecutarThreadEnvioInforme(classes.EnvioInforme envio)
        {
            var destinatarios = new ManterEnvioInforme().ObterDestinatarios(envio);

            var assunto = envio.Informe.Numero + " " + envio.Informe.ObterMesAno();

            var manterEmail = new ManterEmail();
            var manterEnvioInforme = new ManterEnvioInforme();

            // Pode ser lento.
            var mensagem = new ManterInforme().ObterTemplateHTML(envio.Informe);

            var imagens = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Header", Server.MapPath("../../img/newsletter/header.jpg")),
                new KeyValuePair<string, string>("Footer", Server.MapPath("../../img/newsletter/footer.jpg")),
                new KeyValuePair<string, string>("RightArrow", Server.MapPath("../../img/newsletter/right-arrow.jpg")),
                new KeyValuePair<string, string>("PageFlip", Server.MapPath("../../img/newsletter/page-flip.jpg"))
            };

            var thread = new Thread(() =>
            {
                foreach (var email in destinatarios)
                {
                    try
                    {
                        manterEmail.EnviarEmail(email, assunto, mensagem, imagens);
                    }
                    catch (Exception)
                    {
                        // Ignored.
                    }
                }

                envio.DataEnvio = DateTime.Now;

                manterEnvioInforme.Salvar(envio);
            })
            {
                IsBackground = true
            };

            // Let the chaos COMMENCE!
            thread.Start();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "As mensagens estão sendo enviadas para " + destinatarios.Count() + " email(s) automaticamente. Este processo pode demorar dependendo da quantidade de receptores.");
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoInforme.aspx?Id=" + Request["Id"], true);
        }

        protected void btnRemoverUsuario_OnClick(object sender, EventArgs e)
        {
            ucLupaUsuario.SelectedUser = null;
        }
    }
}