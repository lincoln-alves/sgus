using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    [Serializable]
    public class Trilha : EntidadeBasica, ICloneable
    {
        public virtual IList<TrilhaAreaTematica> ListaAreasTematicas { get; set; }
        public virtual IList<TrilhaPermissao> ListaPermissao { get; set; }
        public virtual IList<TrilhaTag> ListaTag { get; set; }
        public virtual IList<TrilhaNivel> ListaTrilhaNivel { get; set; }
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
        public virtual string NomeEstendido { get; set; }
        public virtual int? IdNode { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual int? ID_CodigoMoodle { get; set; }
        public virtual string EmailTutor { get; set; }
        public virtual int? IdNodePortal { get; set; }
        public virtual IList<TrilhaCategoriaConteudo> ListaCategoriaConteudo { get; set; }
        public virtual string Credito { get; set; }

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public Trilha()
        {
            this.ListaPermissao = new List<TrilhaPermissao>();
            this.ListaTag = new List<TrilhaTag>();
            this.ListaTrilhaNivel = new List<TrilhaNivel>();
            this.ListaCategoriaConteudo = new List<TrilhaCategoriaConteudo>();
            this.ListaAreasTematicas = new List<TrilhaAreaTematica>();
        }

        #region "Nivel Ocupacional"

        /// <summary>
        /// Atualiza a lista de Niveis Ocupacionais do Programa.
        /// </summary>
        /// <param name="nivelOcupacional"></param>
        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            IList<NivelOcupacional> ListaNivelOcupacional =
                ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() {ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome})
                    .ToList<NivelOcupacional>();
            TrilhaPermissao trilhaPermissao = new TrilhaPermissao() {NivelOcupacional = nivelOcupacional, Trilha = this};
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaNivelOcupacional.Any(x => x.ID == nivelOcupacional.ID))
            {
                this.ListaPermissao.Add(trilhaPermissao);
            }
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            IList<NivelOcupacional> ListaNivelOcupacional =
                ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() {ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome})
                    .ToList<NivelOcupacional>();

            if (ListaNivelOcupacional.Any(x => x.ID == nivelOcupacional.ID))
            {
                var programaPermissaoASerExcluido =
                    ListaPermissao.FirstOrDefault(
                        x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }

        #endregion

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf)
        {
            IList<Uf> ListaUfs =
                ListaPermissao.Where(x => x.Uf != null)
                    .Select(x => new Uf() {ID = x.Uf.ID, Nome = x.Uf.Nome})
                    .ToList<Uf>();
            TrilhaPermissao trilhaPermissao = new TrilhaPermissao() {Uf = uf, Trilha = this};
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaUfs.Any(x => x.ID == uf.ID))
            {
                this.ListaPermissao.Add(trilhaPermissao);
            }
        }

        public virtual void RemoverUf(Uf uf)
        {
            IList<Uf> ListaUfs =
                ListaPermissao.Where(x => x.Uf != null)
                    .Select(x => new Uf() {ID = x.Uf.ID, Nome = x.Uf.Nome})
                    .ToList<Uf>();

            if (ListaUfs.Any(x => x.ID == uf.ID))
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == uf.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }

        #endregion

        #region "Tag"

        public virtual void AdicionarTag(Tag tag)
        {
            bool tagEstaNaLista = ListaTag.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.Trilha.ID == this.ID);

            if (!tagEstaNaLista)
            {
                TrilhaTag trilhaTag = new TrilhaTag()
                {
                    Tag = tag,
                    Trilha = this,
                    Auditoria = new Auditoria(null)
                };
                this.ListaTag.Add(trilhaTag);
            }
        }

        public virtual void RemoverTag(Tag tag)
        {
            bool tagEstaNaLista = ListaTag.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.Trilha.ID == this.ID);

            if (tagEstaNaLista)
            {
                var tagASerExcluido = ListaTag.FirstOrDefault(x => x.Tag != null &&
                                                                   x.Tag.ID == tag.ID && x.Trilha.ID == this.ID);
                this.ListaTag.Remove(tagASerExcluido);
            }
        }


        #endregion

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            IList<Perfil> ListaPerfils =
                this.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil() {ID = x.Perfil.ID, Nome = x.Perfil.Nome})
                    .ToList<Perfil>();
            TrilhaPermissao trilhaPermissao = new TrilhaPermissao() {Perfil = perfil, Trilha = this};
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaPerfils.Any(x => x.ID == perfil.ID))
            {
                this.ListaPermissao.Add(trilhaPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            IList<Perfil> ListaPerfil =
                ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil() {ID = x.Perfil.ID, Nome = x.Perfil.Nome})
                    .ToList<Perfil>();
            if (ListaPerfil.Any(x => x.ID == perfil.ID))
            {
                var programaPermissaoASerExcluido =
                    ListaPermissao.FirstOrDefault(x => x.Perfil != null && x.Perfil.ID == perfil.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }


        #endregion

        public override bool Equals(object obj)
        {
            Trilha objeto = obj as Trilha;
            return objeto == null ? false : this.ID.Equals(objeto.ID);
        }

        public virtual bool UsuarioPossuiPermissao(Usuario usuario)
        {
            return ListaTrilhaNivel.Any(n => n.UsuarioPossuiPermissao(usuario));
        }

        #region "Atributos Lógicos"

        /// <summary>
        /// Id Lógico utilizado no cadastro de Trilha
        /// </summary>
        public virtual int IdLogico { get; set; }

        /// <summary>
        /// Atributo lógico utilizado para indicar se o registro foi alterado ou se é um novo registro.
        /// </summary>
        public virtual enumStatusRegistro StatusRegistro { get; set; }


        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        /// <summary>
        /// Obter todas as UFs distintas das permissões de todos os níveis dessa trilha.
        /// </summary>
        /// <returns></returns>
        public virtual List<Uf> ObterUfsDasPermissoes()
        {
            var idsUfs = new List<int>();

            foreach (var nivel in ListaTrilhaNivel)
            {
                idsUfs.AddRange(nivel.ListaPermissao.Where(p => p.Uf != null).Select(p => p.Uf.ID));
            }

            var retorno = idsUfs.Distinct().Select(x => new Uf {ID = x}).ToList();

            // Distingue as possíveis repetições.
            return retorno;
        }

        /// <summary>
        /// Obter todos os perfis distintos das permissões de todos os níveis dessa trilha.
        /// </summary>
        /// <returns></returns>
        public virtual List<Perfil> ObterPerfisDasPermissoes()
        {
            var perfisIds = new List<int>();

            foreach (var nivel in ListaTrilhaNivel)
            {
                perfisIds.AddRange(nivel.ListaPermissao.Where(p => p.Perfil != null).Select(p => p.Perfil.ID));
            }

            // Distingue as possíveis repetições.
            return perfisIds.Distinct().Select(x => new Perfil {ID = x}).ToList();
        }

        /// <summary>
        /// Obter todos os Níveis Ocupacionas distintos das permissões de todos os níveis dessa trilha.
        /// </summary>
        /// <returns></returns>
        public virtual List<NivelOcupacional> ObterNiveisOcupacionaisDasPermissoes()
        {
            var niveisOcupacionaisIds = new List<int>();

            foreach (var nivel in ListaTrilhaNivel)
            {
                niveisOcupacionaisIds.AddRange(
                    nivel.ListaPermissao.Where(p => p.NivelOcupacional != null).Select(p => p.NivelOcupacional.ID));
            }

            // Distingue as possíveis repetições.
            return niveisOcupacionaisIds.Distinct().Select(x => new NivelOcupacional {ID = x}).ToList();
        }
    }
}
