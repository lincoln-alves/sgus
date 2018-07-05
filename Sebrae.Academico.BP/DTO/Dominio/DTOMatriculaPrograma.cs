using System;
using Sebrae.Academico.Dominio.Enumeracao;
namespace Sebrae.Academico.BP.DTO.Dominio
{

       [Serializable]
    public class DTOMatriculaPrograma : DTOEntidadeBasicaPorId
    {
           public virtual DTOPrograma Programa { get; set; }
           public virtual DTOUsuario Usuario { get; set; }
           public virtual DTONivelOcupacional NivelOcupacional { get; set; }
           public virtual DTOUf UF { get; set; }
           public virtual enumStatusMatricula StatusMatricula { get; set; }
    }


}
