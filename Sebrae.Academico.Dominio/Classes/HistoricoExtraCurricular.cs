using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class HistoricoExtraCurricular: EntidadeBasicaPorId
    {
        public virtual string SolucaoEducacionalExtraCurricular { get; set; }
        public virtual string TextoAtividade { get; set; }
        public virtual string Instituicao { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual FileServer FileServer { get; set; }
        public virtual FormaAquisicao FormaAquisicao { get; set; }
        public virtual DateTime? DataInicioAtividade { get; set; }
        public virtual DateTime? DataFimAtividade { get; set; }
        public virtual Usuario Usuario { get; set; }
        
        public virtual short? CargaHoraria { get; set; }
    }
}
