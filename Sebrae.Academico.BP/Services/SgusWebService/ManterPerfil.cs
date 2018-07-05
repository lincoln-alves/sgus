using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterPerfil : BusinessProcessServicesBase
    {
        private BMPerfil perfilBM;


        public void IncluirPerfil(DTO.Dominio.DTOPerfil pPerfil, string UsuarioAlteracao)
        {

            perfilBM = new BMPerfil();
            perfilBM.Salvar(new Perfil()
            {
                Nome = pPerfil.Nome
            });
        }

        public void ExcluirPerfil(DTO.Dominio.DTOPerfil pPerfil, string p)
        {
            perfilBM = new BMPerfil();
            perfilBM.Excluir(new Perfil() { ID = pPerfil.ID });
        }
    }
}
