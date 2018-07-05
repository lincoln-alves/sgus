using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOMatriculasPorMes
    {
        public virtual string Mes { get; set; }
        public virtual int qtdCursoOnline { get; set; }
        public virtual int qtdCursoInCompany { get; set; }
    }
}
