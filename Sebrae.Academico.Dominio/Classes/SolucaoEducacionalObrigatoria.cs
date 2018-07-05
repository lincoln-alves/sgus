using System.Collections.Generic;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalObrigatoria : EntidadeBasicaPorId
    {
        public SolucaoEducacionalObrigatoria()
        {
            this.SolucaoEducacional = new SolucaoEducacional();
            this.NivelOcupacional = new NivelOcupacional();
        }

        public SolucaoEducacionalObrigatoria(string cpf)
        {
            Auditoria = new Auditoria(cpf);
            SolucaoEducacional = new SolucaoEducacional();
            NivelOcupacional = new NivelOcupacional();
        }

        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
    }
}
