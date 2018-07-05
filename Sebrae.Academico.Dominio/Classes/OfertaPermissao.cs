using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.Dominio.Classes
{
    public class OfertaPermissao : EntidadeBasica
    {
        public OfertaPermissao()
        {
            Subareas = new List<Subarea>();
        }

        public virtual Oferta Oferta { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual int QuantidadeVagasPorEstado { get; set; }

        public virtual IList<Subarea> Subareas { get; set; }

        public virtual void AdicionarPermissaoSubarea(Subarea subArea)
        {
            if (!Subareas.Any(x => x.ID == subArea.ID))
            {
                Subareas.Add(subArea);
            }
        }

        public virtual void RemoverPermissaoSubarea(Subarea subArea)
        {
            if (Subareas.Any(x => x.ID == subArea.ID))
            {
                Subareas.Remove(Subareas.FirstOrDefault(x => x.ID == subArea.ID));
            }
        }
    }
}
