using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TurmaProfessor : EntidadeBasicaPorId
    {
        public virtual Turma Turma { get; set; }
        public virtual Usuario Professor { get; set; }

        public TurmaProfessor()
        {
            this.Turma = new Turma();
            this.Professor = new Usuario();
        }
    }
}
