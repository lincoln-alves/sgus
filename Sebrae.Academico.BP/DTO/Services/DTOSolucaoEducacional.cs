using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOSolucaoEducacional: DTOEntidadeBasica
    {
        public virtual int IdFormaAquisicao { get; set; }
        public virtual int IdCategoriaSolucaoEducacional { get; set; }
        //public virtual int? IdCertificadoTemplate{ get; set; }
        public virtual string Ementa { get; set; }
        public virtual bool? TemMaterial { get; set; }
        public virtual string Autor { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual string Apresentacao { get; set; }
        public virtual string Objetivo { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
