using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    [Serializable]
    public class DTOTrilha : DTOEntidadeBasica
    {
        public virtual string Imagem { get; set; }
        public virtual string NomeExtendido { get; set; }
    }
}