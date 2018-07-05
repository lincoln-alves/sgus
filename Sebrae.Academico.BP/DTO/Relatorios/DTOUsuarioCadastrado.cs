using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOUsuarioCadastrado
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public string SolucaoEducacional { get; set; }
        public string Oferta { get; set; }
        public int CargaHoraria { get; set; }
        public bool TemMaterial { get; set; }
        public string FormaAquisicao { get; set; }
        public string Turma { get; set; }
        public string TipoTutoria { get; set; }
        public string Professor { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public string StatusMatricula { get; set; }
        public string Fornecedor { get; set; }
    }

    public class DTOUsuarioPorFiltro
    {
        public string UF { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }

    }
}
