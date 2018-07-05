using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Trilhas.Extensions;

namespace Sebrae.Academico.Trilhas.Modules
{
   
    public class CertificadoModule : GenericModule
    {
        public CertificadoModule() : base("Certificado")
        {
            this.RequiresAuthentication();

            // Obtêm dados de um tutorial específico ou do primeiro da sua categoria quando passado id como 0
            Get["/"] = p => new DtoResponse(new TrilhaServices().EmitirCertificado(AcessoAtual.Matricula, AcessoAtual.JwtToken));

            Get["/modelo"] = p => new DtoResponse((new TrilhaServices().EmitirCertificadoModelo(AcessoAtual.Matricula)));

            Get["/boletim"] = p => Response.FromByteArray(new TrilhaServices().GetBoletinInPdf(AcessoAtual.Usuario.ID), "application/pdf");
        }
    }
}