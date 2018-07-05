using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CategoriaConteudo : EntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
        public virtual CategoriaConteudo CategoriaConteudoPai { get; set; }
        public virtual TermoAceiteCategoriaConteudo TermoAceiteCategoriaCounteudo { get; set; }
        public virtual int? IdNode { get; set; }
        public virtual string Apresentacao { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Sigla { get; set; }
        public virtual bool PossuiFiltroCategorias { get; set; }
        public virtual Uf UF { get; set; }
        public virtual MetaFm MetaFm { get; set; }

        public virtual IList<CategoriaConteudoUF> ListaCategoriaConteudoUF { get; set; }
        public virtual IList<CategoriaConteudo> ListaCategoriaConteudoFilhos { get; set; }
        public virtual IList<SolucaoEducacional> ListaSolucaoEducacional { get; set; }
        public virtual IList<Trilha> ListaTrilha { get; set; }
        public virtual IList<CategoriaConteudoPermissao> ListaPermissao { get; set; }
        public virtual IList<CategoriaConteudoTags> ListaTags { get; set; }
        public virtual IList<StatusMatricula> ListaStatusMatricula { get; set; }

        public virtual IEnumerable<Usuario> ListaUsuario { get; set; }

        public virtual bool LiberarInscricao { get; set; }

        public virtual bool? PossuiStatus { get; set; }

        public virtual bool? PossuiAreas { get; set; }


        public CategoriaConteudo()
        {
            ListaCategoriaConteudoFilhos = new List<CategoriaConteudo>();
            ListaSolucaoEducacional = new List<SolucaoEducacional>();
            ListaTrilha = new List<Trilha>();
            ListaPermissao = new List<CategoriaConteudoPermissao>();
            ListaTags = new List<CategoriaConteudoTags>();
            ListaCategoriaConteudoUF = new List<CategoriaConteudoUF>();
        }


        #region "Tag"

        public virtual void AdicionarTag(Tag tag)
        {
            bool tagEstaNaLista = ListaTags.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.CategoriaConteudo.ID == this.ID);

            if (!tagEstaNaLista)
            {
                CategoriaConteudoTags categoria = new CategoriaConteudoTags()
                {
                    Tag = tag,
                    CategoriaConteudo = this,
                    Auditoria = new Auditoria(null)
                    //    DataAlteracao = DateTime.Now 
                };
                this.ListaTags.Add(categoria);
            }
        }

        public virtual void RemoverTag(Tag tag)
        {

            bool tagEstaNaLista = ListaTags.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.CategoriaConteudo.ID == this.ID);

            if (tagEstaNaLista)
            {
                var tagASerExcluido = ListaTags.FirstOrDefault(x => x.Tag != null &&
                                                             x.Tag.ID == tag.ID && x.CategoriaConteudo.ID == this.ID);
                this.ListaTags.Remove(tagASerExcluido);
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
            IList<NivelOcupacional> ListaNivelOcupacional = ListaPermissao.Where(x => x.NivelOcupacional != null).Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();
            CategoriaConteudoPermissao CategoriaConteudoPermissao = new CategoriaConteudoPermissao() { NivelOcupacional = nivelOcupacional, CategoriaConteudo = this };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                this.ListaPermissao.Add(CategoriaConteudoPermissao);
            }
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            IList<NivelOcupacional> ListaNivelOcupacional = ListaPermissao.Where(x => x.NivelOcupacional != null).Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();

            if (ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }

        #endregion

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.CategoriaConteudo.ID == this.ID);

            if (!perfilEstaNaLista)
            {
                CategoriaConteudoPermissao CategoriaConteudoPermissao = new CategoriaConteudoPermissao() { Perfil = perfil, CategoriaConteudo = this };
                this.ListaPermissao.Add(CategoriaConteudoPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID && x.CategoriaConteudo.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var solucaoEducacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                   x.Perfil.ID == perfil.ID && x.CategoriaConteudo.ID == this.ID);
                this.ListaPermissao.Remove(solucaoEducacionalASerExcluido);
            }

        }


        #endregion

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.CategoriaConteudo.ID == this.ID);

            if (!ufEstaNaLista)
            {
                CategoriaConteudoPermissao CategoriaConteudoPermissao = new CategoriaConteudoPermissao() { Uf = uf, CategoriaConteudo = this };
                this.ListaPermissao.Add(CategoriaConteudoPermissao);
            }
        }

        public virtual void RemoverUfs(Uf uf)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.CategoriaConteudo.ID == this.ID);

            if (ufEstaNaLista)
            {
                var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null &&
                                                                   x.Uf.ID == uf.ID && x.CategoriaConteudo.ID == this.ID);
                this.ListaPermissao.Remove(ufASerExcluido);
            }
        }

        #endregion

        #region Uf do contexto gestor

        #endregion

        #region "Permissão"

        public virtual void AdicionarPermissao(CategoriaConteudoPermissao permissao)
        {

            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Perfil.ID == permissao.ID && x.CategoriaConteudo.ID == this.ID);

            if (!ufEstaNaLista)
            {
                //TODO -> Retestar este ponto
                CategoriaConteudoPermissao CategoriaConteudoPermissao = new CategoriaConteudoPermissao() { Perfil = permissao.Perfil, CategoriaConteudo = this };
                this.ListaPermissao.Add(CategoriaConteudoPermissao);
            }

        }

        public virtual void RemoverPermissao(CategoriaConteudoPermissao permissao)
        {
            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == permissao.ID && x.CategoriaConteudo.ID == this.ID);

            if (perfilEstaNaLista)
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                  x.Perfil.ID == permissao.ID && x.CategoriaConteudo.ID == this.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }

        }


        #endregion

        public virtual int ObterQuantidadeInscricoes(int ano)
        {
            return
                ObterFilhos().Sum(
                    filho =>
                        filho.ListaSolucaoEducacional.SelectMany(s => s.ListaOferta)
                            .SelectMany(o => o.ListaMatriculaOferta)
                            .Count(mo => mo.DataStatusMatricula.HasValue && mo.DataStatusMatricula.Value.Year == ano));
        }

        public virtual IEnumerable<CategoriaConteudo> ObterFilhos(bool incluirEste = true)
        {
            var retorno = new List<CategoriaConteudo>();

            if (incluirEste)
                retorno.Add(this);

            foreach (var filho in ListaCategoriaConteudoFilhos)
            {
                retorno.Add(filho);

                retorno.AddRange(filho.ObterFilhos(false));
            }

            return retorno;
        }

        public virtual IEnumerable<CategoriaConteudo> ObterPais(bool incluirEste = true)
        {
            var retorno = new List<CategoriaConteudo>();

            if (incluirEste)
                retorno.Add(this);

            if (CategoriaConteudoPai != null)
            {
                retorno.Add(CategoriaConteudoPai);
                retorno.AddRange(CategoriaConteudoPai.ObterPais(false));
            }

            return retorno;
        }

        public virtual string ObterSigla()
        {
            if (CategoriaConteudoPai != null)
            {
                return CategoriaConteudoPai.Sigla;
            }

            return Sigla;
        }

        public virtual void AdicionarSigla(string siglaCadastro)
        {
            if (CategoriaConteudoPai == null)
            {
                Sigla = siglaCadastro;
            }
        }

        /// <summary>
        /// Verifica se essa Categoria possui permissão de exibir Status ou, caso não possua, verifica se algum pai da hierarquia possui permissão de exibir.
        /// </summary>
        /// <returns></returns>
        public virtual bool PossuiGerenciamentoStatus()
        {
            return PossuiStatus == null
                ? CategoriaConteudoPai != null && CategoriaConteudoPai.PossuiGerenciamentoStatus()
                : PossuiStatus == true;
        }

        /// <summary>
        /// Verifica se essa Categoria possui permissão de exibir Áreas/Subáreas ou, caso não possua, verifica se algum pai da hierarquia possui permissão de exibir.
        /// </summary>
        /// <returns></returns>
        public virtual bool PossuiGerenciamentoAreas()
        {
            return PossuiAreas == null
                ? CategoriaConteudoPai != null && CategoriaConteudoPai.PossuiGerenciamentoAreas()
                : PossuiAreas == true;
        }
    }
}
