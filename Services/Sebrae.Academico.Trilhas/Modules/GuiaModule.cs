using System.Linq;
using Nancy.Security;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class GuiaModule : GenericModule
    {
        public GuiaModule() : base("guia")
        {
            this.RequiresAuthentication();

            Get["/possuifaq"] = p =>
            {
                return new DtoResponse(new ManterTrilhaFaq().ObterTodosAssuntoIQueryable().Any(x => x.ItensFaq.Any()));
            };

            // Obtêm dados de um tutorial específico ou do primeiro da sua categoria quando passado id como 0
            Get["/tutorial/{categoriaId:int}/{tutorialId:int}"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ObterTutorialPorCategoria(p.categoriaId, p.tutorialId, AcessoAtual));
            };

            Get["/faq"] = p =>
            {
                return new DtoResponse((new TrilhaServices().ObterListaFaq()));
            };

            Get["/faq/{faqId:int}"] = p =>
            {
                return new DtoResponse((new TrilhaServices().ObterFaqPorId(p.faqId)));
            };

            Get["/tutorial/acesso/{categoria_id:int}"] = p =>
            {
                return new DtoResponse((new TrilhaServices().ObterAcessoTutorial(AcessoAtual.Matricula, p.categoria_id)));
            };

            Get["/tutorial/leitura/{categoria_id:int}"] = p =>
            {                
                return new DtoResponse(new TrilhaServices().MarcarLidoAcessoTutorial(AcessoAtual.Matricula, p.categoria_id));
            };
        }
    }
}