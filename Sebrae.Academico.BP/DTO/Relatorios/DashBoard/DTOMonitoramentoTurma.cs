using System;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.DTO.Relatorios.DashBoard
{
    public class DTOMonitoramentoTurma
    {
        public int ID_Turma { get; set; }
        public string NomeTurma { get; set; }
        public string SolucaoEducacional { get; set; }
        public int? Status { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public string StatusFormatado
        {
            get
            {
                return Status != null && Status > 0 ? ((enumStatusTurma)Status).GetDescription() : "Sem status";
            }
        }

        public long TotalMatriculados { get; set; }
        public long TotalMatriculadosAno { get; set; }

        public long TotalTurmas { get; set; }
        public long TotalTurmasAno { get; set; }

        public long TotalTurmasComStatus { get; set; }

        public long TotalMatriculasComStatus { get; set; }
    }
}