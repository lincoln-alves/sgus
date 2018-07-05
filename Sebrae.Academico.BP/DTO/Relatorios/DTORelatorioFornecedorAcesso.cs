using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioFornecedorAcesso
    {
        public string Nome { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        public int QtdAcessos { get; set; }
    }
}
