using System;
namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalPermissao : EntidadeBasica
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Int32 QuantidadeVagasPorEstado { get; set; }
    }
}
