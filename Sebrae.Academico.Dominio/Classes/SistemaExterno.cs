using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SistemaExterno : EntidadeBasica
    {
        public virtual string LinkSistemaExterno { get; set; }
        public virtual bool? Publico { get; set; }
        public virtual bool? EnglishTown { get; set; }
        public virtual bool? MesmaJanela { get; set; }
        public virtual IList<SistemaExternoPermissao> ListaPermissao { get; set; }
        public virtual IList<SistemaExternoPermissao> ListaUsuariosPermitidos { get; set; }

        public SistemaExterno()
        {
            this.ListaPermissao = new List<SistemaExternoPermissao>();
            this.ListaUsuariosPermitidos = new List<SistemaExternoPermissao>();
        }

        #region "Relacionamentos"

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.SistemaExterno.ID == this.ID);

            if (!ufEstaNaLista)
            {
                SistemaExternoPermissao sistemaExternoPermissao = new SistemaExternoPermissao() { Uf = uf, SistemaExterno = this };
                this.ListaPermissao.Add(sistemaExternoPermissao);
            }

        }

        public virtual void RemoverUf(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.SistemaExterno.ID == this.ID);

            if (ufEstaNaLista)
            {
                var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null &&
                                                                   x.Uf.ID == uf.ID && x.SistemaExterno.ID == this.ID);
                this.ListaPermissao.Remove(ufASerExcluido);
            }
        }

        #endregion

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {

            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.SistemaExterno.ID == this.ID);

            if (!perfilEstaNaLista)
            {
                SistemaExternoPermissao sistemaExternoPermissao = new SistemaExternoPermissao() { Perfil = perfil, SistemaExterno = this };
                this.ListaPermissao.Add(sistemaExternoPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {

            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.SistemaExterno.ID == this.ID);

              if (perfilEstaNaLista)
              {
                  var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null && 
                                                                                    x.Perfil.ID == perfil.ID && x.SistemaExterno.ID == this.ID);
                  this.ListaPermissao.Remove(programaPermissaoASerExcluido);
              }
    
        }


        #endregion

        #region "Nivel Ocupacional"

        /// <summary>
        /// Atualiza a lista de NiveisOcupacionais do Programa.
        /// </summary>
        /// <param name="nivelOcupacional"></param>
        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivelOcupacional)
        {

            bool nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null
                                                                  && x.NivelOcupacional.ID == nivelOcupacional.ID && x.SistemaExterno.ID == this.ID);

            if (!nivelOcupacionalEstaNaLista)
            {
                SistemaExternoPermissao sistemaExternoPermissao = new SistemaExternoPermissao() { NivelOcupacional = nivelOcupacional, SistemaExterno = this };
                this.ListaPermissao.Add(sistemaExternoPermissao);
            }

        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            bool nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null
                                                                  && x.NivelOcupacional.ID == nivelOcupacional.ID && x.SistemaExterno.ID == this.ID);

            if (nivelOcupacionalEstaNaLista)
            {
                var nivelOcupacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.NivelOcupacional != null &&
                                                                                 x.NivelOcupacional.ID == nivelOcupacional.ID 
                                                                                 && x.SistemaExterno.ID == this.ID);
                this.ListaPermissao.Remove(nivelOcupacionalASerExcluido);
            }

        }

        #endregion
        
        #endregion

    }
}
