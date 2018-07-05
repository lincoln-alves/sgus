using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Programa : EntidadeBasica
    {

        public virtual bool Ativo { get; set; }
        public virtual string Apresentacao { get; set; }
        public virtual IList<ProgramaSolucaoEducacional> ListaSolucaoEducacional { get; set; }
        public virtual IList<MatriculaPrograma> ListaMatriculaPrograma { get; set; }
        public virtual IList<ProgramaPermissao> ListaPermissao { get; set; }
        public virtual IList<ProgramaTag> ListaTag { get; set; }
        public virtual IList<ViewSolucaoEducacionalPermissao> ListaUsuariosPermitidos { get; set; }
        public virtual IList<Capacitacao> ListaCapacitacao { get; set; }
        public virtual IList<ProgramaAreaTematica> ListaAreasTematicas { get; set; }
        public virtual int? IdNodePortal { get; set; }
        public virtual int Sequencia { get; set; }
        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public Programa()
        {
            this.ListaPermissao = new List<ProgramaPermissao>();
            this.ListaTag = new List<ProgramaTag>();
            this.ListaUsuariosPermitidos = new List<ViewSolucaoEducacionalPermissao>();
            this.ListaCapacitacao = new List<Capacitacao>();
            this.ListaAreasTematicas = new List<ProgramaAreaTematica>();
            this.ListaSolucaoEducacional = new List<ProgramaSolucaoEducacional>();
        }

        public virtual string DescricaoSequencial
        {
            get
            {
                if (Sequencia > 0)
                    return string.Format("{0}.PR", Sequencia);

                return "-";
            }
        }

        #region "Relacionamentos"

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.Programa.ID == this.ID);

            if (!ufEstaNaLista)
            {
                var programaPermissao = new ProgramaPermissao() { Uf = uf, Programa = this };
                this.ListaPermissao.Add(programaPermissao);
            }
        }

        public virtual void RemoverUf(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.Programa.ID == this.ID);

            if (ufEstaNaLista)
            {
                var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null &&
                                                                   x.Uf.ID == uf.ID && x.Programa.ID == this.ID);
                this.ListaPermissao.Remove(ufASerExcluido);
            }
        }

        #endregion

        #region "Tag"

        public virtual void AdicionarTag(Tag tag)
        {
            bool tagEstaNaLista = ListaTag.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.Programa.ID == this.ID);

            if (!tagEstaNaLista)
            {
                ProgramaTag programa = new ProgramaTag()
                {
                    Tag = tag,
                    Programa = this,
                    Auditoria = new Auditoria(null)
                };
                this.ListaTag.Add(programa);
            }
        }

        public virtual void RemoverTag(Tag tag)
        {

            bool tagEstaNaLista = ListaTag.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.Programa.ID == this.ID);

            if (tagEstaNaLista)
            {
                var tagASerExcluido = ListaTag.FirstOrDefault(x => x.Tag != null &&
                                                             x.Tag.ID == tag.ID && x.Programa.ID == this.ID);
                this.ListaTag.Remove(tagASerExcluido);
            }

        }


        #endregion

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.Programa.ID == this.ID);

            if (!perfilEstaNaLista)
            {
                ProgramaPermissao programaPermissao = new ProgramaPermissao()
                {
                    Perfil = perfil,
                    Programa = this,
                    Auditoria = new Auditoria(null)
                };
                this.ListaPermissao.Add(programaPermissao);
            }

        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.Programa.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var perfilASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                         x.Perfil.ID == perfil.ID && x.Programa.ID == this.ID);
                this.ListaPermissao.Remove(perfilASerExcluido);
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
                                                                  && x.NivelOcupacional.ID == nivelOcupacional.ID && x.Programa.ID == this.ID);

            if (!nivelOcupacionalEstaNaLista)
            {
                ProgramaPermissao programaPermissao = new ProgramaPermissao()
                {
                    NivelOcupacional = nivelOcupacional,
                    Programa = this,
                    //DataAlteracao = DateTime.Now };
                    Auditoria = new Auditoria(null)
                };
                this.ListaPermissao.Add(programaPermissao);
            }

        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            bool nivelOcupacionalEstaNaLista = ListaPermissao.Any(x => x.NivelOcupacional != null
                                                                  && x.NivelOcupacional.ID == nivelOcupacional.ID && x.Programa.ID == this.ID);

            if (nivelOcupacionalEstaNaLista)
            {
                var nivelOcupacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.NivelOcupacional != null &&
                                                                                 x.NivelOcupacional.ID == nivelOcupacional.ID
                                                                                 && x.Programa.ID == this.ID);

                this.ListaPermissao.Remove(nivelOcupacionalASerExcluido);
            }

        }

        #endregion

        #endregion

    }
}
