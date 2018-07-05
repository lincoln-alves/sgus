using System.Linq;
using Nancy.ModelBinding;
using Nancy.Security;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Questionario;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class QuestionarioModule : GenericModule
    {
        public QuestionarioModule() : base ("Questionario")
        {
            this.RequiresAuthentication();

            // Obter dados do questionário.
            // nivel: Id do nível (mapa)
            // tipo: 1 - pré, 2 - pós, 3 - prova.
            Get["/{tipo:int}/{superAdmin:bool}"] =
            Get["solucao/{itemTrilhaId:int}/{superAdmin:bool}"] = p =>
            {
                bool superAdmin = p.superAdmin != null && p.superAdmin;

                if (superAdmin)
                {
                    var usuario = new ManterUsuario().ObterUsuarioPorID(AcessoAtual.Usuario.ID);

                    var podeSerSuperAdmin = usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.AdministradorPortal);

                    if (podeSerSuperAdmin == false)
                        throw new ResponseException(enumResponseStatusCode.PermissaoNegadaSuperAcesso);
                }

                if(!superAdmin)
                    VerificarBloqueio();

                var trilhaService = new TrilhaServices();

                var tipoQuestionarioAssociacao = trilhaService.ObterTipoQuestionario(AcessoAtual, p.tipo, p.itemTrilhaId, superAdmin);

                return new DtoResponse(trilhaService.ObterQuestionario(AcessoAtual, tipoQuestionarioAssociacao, p.itemTrilhaId, superAdmin));
            };

            Post["/"] = 
            Post["/{superAdmin:bool}"] = p =>
            {
                VerificarBloqueio();
                // Fazer o bind dos valores do POST para um objeto fortemente tipado.
                DTOInformarRespostaQuestionario respostasViewModel = this.Bind();

                bool superAdmin = p.superAdmin != null && p.superAdmin;
                
                return new DtoResponse(new TrilhaServices().InformarRespostasQuestionario(AcessoAtual, respostasViewModel, superAdmin));
            };

            Get["/participacao/{itemTrilha:int}"] = p =>
            {                
                int itemTrilha = (int)p.itemTrilha;
                return new DtoResponse(new TrilhaServices().ObterQuestionarioParticipacao(AcessoAtual, itemTrilha));
            };
        }
    }
}