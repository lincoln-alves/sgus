using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterCertificadoService : BusinessProcessServicesBase
    {
        public bool ConsultarVeracidadeCertificado(string codigo, int idUsuario)
        {
            // Certificado do tutor.
            if (codigo.Length < 32 && codigo.StartsWith("cr"))
            {
                var verificarCertificado = new ManterCertificadoTemplate().VerificarCertificadoTutor(codigo);

                if (verificarCertificado.Valido)
                {
                    return true;
                }
            }

            var retorno = false;

            var usuario = new Usuario();

            if (idUsuario > 0)
            {
                usuario = new BMUsuario().ObterPorId(idUsuario);

                if (usuario == null)
                    throw new AcademicoException("Usuário não localizado na base");
            }

            if (string.IsNullOrEmpty(codigo))
                throw new AcademicoException("O código do certificado não foi preenchido");

            var bmMatriculaOferta = new BMMatriculaOferta();
            var buscaMatOferta = bmMatriculaOferta.ObterPorCodigoCertificado(codigo, usuario);
            if (buscaMatOferta != null)
            {
                retorno = true;
            }
            else
            {
                BMUsuarioTrilha BMUsuarioTrilha = new BMUsuarioTrilha();
                UsuarioTrilha buscaUsuarioTrilha = BMUsuarioTrilha.ObterPorCodigoCertificao(codigo, usuario);
                if (buscaUsuarioTrilha != null)
                {
                    retorno = true;
                }
            }

            return retorno;
        }
    }
}
