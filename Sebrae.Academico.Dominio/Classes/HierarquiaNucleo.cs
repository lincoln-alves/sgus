using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class HierarquiaNucleo : EntidadeBasica
    {
        public virtual Uf Uf { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual IList<HierarquiaNucleoUsuario> HierarquiaNucleoUsuarios { get; set; }

        public HierarquiaNucleo()
        {
            HierarquiaNucleoUsuarios = new List<HierarquiaNucleoUsuario>();
        }
        public virtual IEnumerable<HierarquiaNucleoUsuario> ObterGestores()
        {
            return HierarquiaNucleoUsuarios.Where(x => x.IsGestor == true);
        }

        public virtual IEnumerable<HierarquiaNucleoUsuario> ObterFuncionariosHierarquiaNucleo()
        {
            return HierarquiaNucleoUsuarios.Where(x => x.IsGestor == false);
        }

        private IEnumerable<HierarquiaNucleoUsuario> ObterUsuariosPorUf(Uf uf)
        {
            return HierarquiaNucleoUsuarios.Where(x => x.Uf.ID == uf.ID);
        }        

        public virtual IList<Usuario> ObterFuncionarios()
        {
            return HierarquiaNucleoUsuarios.Where(x => !x.IsGestor).Select(x => x.Usuario).ToList();
        }
    }
}
