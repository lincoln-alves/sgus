using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Relatorios.DashBoard
{
    public class DTOMatriculasPorUF
    {
        public enumUF Uf { get; set; }
        public string Estado { get; set; }
        public Decimal Porcentagem { get; set; }
        public int QuantidadePorUf { get; set; }
        public int QuantidadeTotalInscritos { get; set; }
    }
}
