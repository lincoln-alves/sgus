using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{

    public class MatriculaPrograma : EntidadeBasicaComStatus
    {
        public virtual Programa Programa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Uf UF { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }        

        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }

    }


}
