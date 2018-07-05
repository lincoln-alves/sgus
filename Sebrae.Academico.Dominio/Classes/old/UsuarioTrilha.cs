using System;

namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class UsuarioTrilha : EntidadeBasicaPorId
    {
        //Todo -> Trocar este Id pela entidade referente ao Usuario (SgusUsuario)
        public virtual int CodUsuario { get; set; }

        public virtual TrilhaNivel TrilhaNivel { get; set; }

        public virtual Trilha Trilha { get; set; }

        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataLimite { get; set; }
        public virtual DateTime? DataFim { get; set; }

        public virtual decimal NotaProva { get; set; }
        public virtual string Status { get; set; }

    }
}
