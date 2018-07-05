using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services.GestorUC
{
    public class DTOGestorUC
    {
        public virtual int IdOferta { get; set; }
        public virtual string NomeOferta { get; set; }
        public virtual DateTime? DataInicioInscricoes { get; set; }
        public virtual DateTime? DataFimInscricoes { get; set; }
        public virtual int QtdVagasDisponiveis { get; set; }
        public virtual bool PermiteAlterarStatusPeloGestor { get; set; }

        public virtual List<DTOOferta> ListaOfertas { get; set; }
        public virtual List<DTOGestorUCUsuario> ListaMatriculados { get; set; }
        
        public DTOGestorUC()
        {
            this.ListaMatriculados = new List<DTOGestorUCUsuario>();
            this.ListaOfertas = new List<DTOOferta>();
        }
    }
}
