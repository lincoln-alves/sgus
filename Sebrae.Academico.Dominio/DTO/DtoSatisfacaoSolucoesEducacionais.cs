using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DtoSatisfacaoSolucoesEducacionais
    {
        public string SolucaoEducacional { get; set; }
        public int Concluintes { get; set; }
        public double? Satisfacao { get; set; }

        public string PorcentagemSatisfacao { get { return Satisfacao.HasValue ? Satisfacao.Value.ToString("P") : "-"; } }
    }
}
