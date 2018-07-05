using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioHistoricoAtividadeDiagnostico
    {
        public bool Pre { get; set; }
        public string TemaItem { get; set; }
        public string ObjetivoItem { get; set; }
        public string NotaPreItemI { get; set; }
        public string NotaPosItemI { get; set; }
        public string NotaPreItemD { get; set; }
        public string NotaPosItemD { get; set; }
    }
}
