using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOHistoricoExtraCurricular
    {
        public virtual int ID { get; set; }
        public virtual int IdUsuario { get; set; }
        public virtual int? IdFornecedor { get; set; }
        //public virtual DTOSolucaoEducacionalExtraCurricular SolucaoEducacionalExtraCurricular { get; set; }
        public virtual string NomeSolucaoExtraCurricular { get; set; }
        public virtual string NomeInstituicao { get; set; }
        public virtual string NomeArquivoComprovacao { get; set; }
        public virtual string NomeArquivoOriginal { get; set; }
        public virtual string TipoArquivoComprovacao { get; set; }
        public virtual string CaminhoArquivoParticipacao { get; set; }
        public virtual string TextoAtividade { get; set; }
        public virtual string DataInicioAtividade { get; set; }
        public virtual string DataFimAtividade { get; set; }
        public virtual short? CargaHoraria { get; set; }

        public DTOHistoricoExtraCurricular()
        {

        }
    }
}
