using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioHistoricoAtividadeDadosBasicos
    {
        public string Trilha { get; set; }
        public string Nivel { get; set; }
        public string StatusMatricula { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public int IDTopicoTematico { get; set; }
        public string TopicoTematico { get; set; }
        public string NivelOcupacional { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataLimite { get; set; }
        public string StatusAtividadeFormativa { get; set; }

    }
}
