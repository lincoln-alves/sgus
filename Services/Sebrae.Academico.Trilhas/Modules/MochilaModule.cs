using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class MochilaModule : GenericModule
    {
        public MochilaModule() : base("Mochila")
        {
            this.RequiresAuthentication();

            // Obter dados da mochila, recebe o ID do Usuário, não o ID da matrícula do usuário em Trilhas
            Get["/{userId:int}"] = p =>
            {
                VerificarBloqueio();

                var trilhaServices = new TrilhaServices();

                var mensagensGuia = p.userId > 0 ? null : trilhaServices.ObterMensagensGuiaMochila(AcessoAtual.Matricula, AcessoAtual.Nivel);

                // Retornar os dados da mochila
                return
                    new DtoResponse(
                        trilhaServices.ObterDadosMochila(p.userId > 0 ? p.userId : AcessoAtual.Usuario.ID,
                            AcessoAtual), mensagensGuia);
            };

            Get["/extrato"] = p => new DtoResponse(new TrilhaServices().ObterExtrato(AcessoAtual, AcessoAtual.Usuario.ID, AcessoAtual.Nivel.ID));
        }
    }
}