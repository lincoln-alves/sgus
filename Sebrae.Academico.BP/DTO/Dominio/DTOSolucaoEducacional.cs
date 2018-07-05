using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOSolucaoEducacional : DTOEntidadeBasica
    {
        //public virtual List<DTOItemTrilha> ListaItemTrilha { get; set; }

        public virtual DTOFormaAquisicao FormaAquisicao { get; set; }
        public virtual DTOFornecedor Fornecedor { get; set; }
        public virtual DTOCategoriaConteudo CategoriaConteudo { get; set; }
        public virtual DTOCertificadoTemplate CertificadoTemplate { get; set; }
        public virtual string Ementa { get; set; }
        public virtual int? QTCargaHoraria { get; set; }
        public virtual DateTime? DTCadastro { get; set; }
        public virtual int? QTSemanas { get; set; }
        public virtual int? QTDiasPresencial { get; set; }
        public virtual string TemMaterial { get; set; }
        public virtual string Autor { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual string Apresentacao { get; set; }
        public virtual string Objetivo { get; set; }
        public virtual string NivelMBA { get; set; }
        public virtual string Situacao { get; set; }
        public virtual int? IdNode { get; set; }
        public virtual string Link { get; set; }

    }
}
