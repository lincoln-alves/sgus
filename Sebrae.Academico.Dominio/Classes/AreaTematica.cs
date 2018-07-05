using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class AreaTematica : EntidadeBasica
    {
        public virtual string Icone { get; set; }
        public virtual string Apresentacao { get; set; }
        public virtual IList<AreaTematicaPermissao> ListaPermissao { get; set; }
        public virtual int? IdNodePortal { get; set; }

        public AreaTematica(){
            ListaPermissao = new List<AreaTematicaPermissao>();
        }

        public virtual void AdicionarUfs(Uf uf){
            var ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.AreaTematica.ID == this.ID);

            if (ufEstaNaLista) return;
            var permissao = new AreaTematicaPermissao { Uf = uf, AreaTematica = this };
            this.ListaPermissao.Add(permissao);
        }

        public virtual void RemoverUf(Uf uf){
            var ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.AreaTematica.ID == this.ID);

            if (!ufEstaNaLista) return;
            var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == uf.ID && x.AreaTematica.ID == this.ID);
            this.ListaPermissao.Remove(ufASerExcluido);
        }

        public virtual void AdicionarPerfil(Perfil perfil){
            var perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.AreaTematica.ID == this.ID);

            if (perfilEstaNaLista) return;
            var permissao = new AreaTematicaPermissao{
                Perfil = perfil,
                AreaTematica = this,
                Auditoria = new Auditoria(null)
            };
            this.ListaPermissao.Add(permissao);
        }

        public virtual void RemoverPerfil(Perfil perfil){
            var perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.AreaTematica.ID == this.ID);

            if (!perfilEstaNaLista) return;
            var perfilASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                        x.Perfil.ID == perfil.ID && x.AreaTematica.ID == this.ID);
            this.ListaPermissao.Remove(perfilASerExcluido);
        }

        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivelOcupacional){
            var nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID && x.AreaTematica.ID == this.ID);

            if (nivelOcupacionalEstaNaLista) return;
            var permissao = new AreaTematicaPermissao{
                NivelOcupacional = nivelOcupacional,
                AreaTematica = this,
                Auditoria = new Auditoria(null)
            };
            this.ListaPermissao.Add(permissao);
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            var nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID && x.AreaTematica.ID == this.ID);

            if (!nivelOcupacionalEstaNaLista) return;
            var nivelOcupacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID && x.AreaTematica.ID == this.ID);

            this.ListaPermissao.Remove(nivelOcupacionalASerExcluido);
        }
    }
}
