using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class HistoricoSGTC : EntidadeBasicaPorId
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string NomeSolucaoEducacional { get; set; }
        public virtual int IDChaveExterna { get; set; }
        public virtual DateTime DataConclusao { get; set; }
        public virtual string CDCertificado { get; set; }
    }

}
