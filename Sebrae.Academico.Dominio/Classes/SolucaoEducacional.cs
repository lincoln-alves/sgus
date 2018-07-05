using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacional : EntidadeBasica
    {
        public SolucaoEducacional()
        {
            this.ListaProgramaSolucaoEducacional = new List<ProgramaSolucaoEducacional>();
            this.ListaPermissao = new List<SolucaoEducacionalPermissao>();
            this.ListaTags = new List<SolucaoEducacionalTags>();
            //this.ListaUsuariosPermitidos = new List<ViewSolucaoEducacionalPermissao>();
            this.ListaUsuariosPermitidos = new List<DTOSolucaoEducacionalPermissao>();
            this.ListaSolucaoEducacionalObrigatoria = new List<SolucaoEducacionalObrigatoria>();
            this.ListaPreRequisito = new List<SolucaoEducacionalPreRequisito>();
            this.ListaAreasTematicas = new List<SolucaoEducacionalAreaTematica>();
            this.ListProdutosSebrae = new List<SolucaoEducacionalProdutoSebrae>();
            this.ListUnidadesDemandates = new List<SolucaoEducacionalUnidadeDemantes>();
        }
        public virtual IList<SolucaoEducacionalAreaTematica> ListaAreasTematicas { get; set; }
        public virtual IList<SolucaoEducacionalProdutoSebrae> ListProdutosSebrae { get; set; }
        public virtual IList<SolucaoEducacionalUnidadeDemantes> ListUnidadesDemandates { get; set; }
        public virtual FormaAquisicao FormaAquisicao { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
        public virtual FileServer Imagem { get; set; }
        public virtual TermoAceite TermoAceite { get; set; }
        public virtual string Ementa { get; set; }
        public virtual DateTime? Inicio { get; set; }
        public virtual DateTime? Fim { get; set; }
        public virtual DateTime? DataCadastro { get; set; }
        public virtual bool? TemMaterial { get; set; }
        public virtual string Autor { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual string Apresentacao { get; set; }
        public virtual string Objetivo { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual bool TeraOfertasContinuas { get; set; }
        public virtual int? IdNode { get; set; }
        public virtual int? IdNodePortal { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual int? Sequencia { get; set; }
        public virtual IList<SolucaoEducacionalPermissao> ListaPermissao { get; set; }
        public virtual IList<DTOSolucaoEducacionalPermissao> ListaUsuariosPermitidos { get; set; }
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
        public virtual IEnumerable<Oferta> ListaOferta { get; set; }
        public virtual IList<ProgramaSolucaoEducacional> ListaProgramaSolucaoEducacional { get; set; }
        public virtual IList<SolucaoEducacionalTags> ListaTags { get; set; }
        public virtual IList<SolucaoEducacionalObrigatoria> ListaSolucaoEducacionalObrigatoria { get; set; }
        public virtual IList<SolucaoEducacionalPreRequisito> ListaPreRequisito { get; set; }


        public virtual Usuario UsuarioCriacao { get; set; }

        public virtual bool IntegracaoComSAS { get; set; }

        // UF da qual o usuário gestor era no momento de criação da SE, só preenchida para gestores
        public virtual Uf UFGestor { get; set; }

        public virtual string DescricaoSequencial
        {
            get
            {
                var id = Sequencia != null ? Sequencia.Value : ID;

                if (CategoriaConteudo != null &&
                    !string.IsNullOrWhiteSpace(CategoriaConteudo.ObterSigla()))
                    return string.Format("{0}.SE{1}", CategoriaConteudo.Sigla, id);

                return "N/D";
            }
        }

        /// <summary>
        /// ID de evento utilizado para sincronia com o sistema de credenciamento
        /// </summary>
        public virtual int? IDEvento { get; set; }

        #region "Tag"

        public virtual void AdicionarTag(Tag tag)
        {
            bool tagEstaNaLista = ListaTags.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.SolucaoEducacional.ID == this.ID);

            if (!tagEstaNaLista)
            {
                SolucaoEducacionalTags solucaoEducacional = new SolucaoEducacionalTags()
                {
                    Tag = tag,
                    SolucaoEducacional = this,
                    Auditoria = new Auditoria(null)
                };
                this.ListaTags.Add(solucaoEducacional);
            }
        }

        public virtual void RemoverTag(Tag tag)
        {

            bool tagEstaNaLista = ListaTags.Any(x => x.Tag != null && x.Tag.ID == tag.ID && x.SolucaoEducacional.ID == this.ID);

            if (tagEstaNaLista)
            {
                var tagASerExcluido = ListaTags.FirstOrDefault(x => x.Tag != null &&
                                                             x.Tag.ID == tag.ID && x.SolucaoEducacional.ID == this.ID);
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
            SolucaoEducacionalPermissao solucaoEducacionalPermissao = new SolucaoEducacionalPermissao() { NivelOcupacional = nivelOcupacional, SolucaoEducacional = this };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                this.ListaPermissao.Add(solucaoEducacionalPermissao);
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
            var perfilEstaNaLista =
                ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID);

            if (!perfilEstaNaLista)
            {
                var solucaoEducacionalPermissao = new SolucaoEducacionalPermissao
                {
                    Perfil = perfil,
                    SolucaoEducacional = this
                };

                ListaPermissao.Add(solucaoEducacionalPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            var perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == perfil.ID);

            if (perfilEstaNaLista)
            {
                var solucaoEducacionalASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null &&
                                                                                        x.Perfil.ID == perfil.ID);
                ListaPermissao.Remove(solucaoEducacionalASerExcluido);
            }
        }


        #endregion

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf)
        {
            AdicionarUfs(uf, 0);
        }

        public virtual void AdicionarUfs(Uf uf, int vagas)
        {
            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.SolucaoEducacional.ID == this.ID);

            if (!ufEstaNaLista)
            {
                SolucaoEducacionalPermissao solucaoEducacionalPermissao = new SolucaoEducacionalPermissao() { Uf = uf, SolucaoEducacional = this, QuantidadeVagasPorEstado = vagas };
                this.ListaPermissao.Add(solucaoEducacionalPermissao);
            }
            else
            {
                if (vagas > 0)
                {
                    RemoverUfs(uf);
                    AdicionarUfs(uf, vagas);
                }
            }
        }

        public virtual void RemoverUfs(Uf uf)
        {
            var ufEstaNaLista =
                ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == uf.ID && x.SolucaoEducacional.ID == ID);

            if (ufEstaNaLista)
            {
                var ufASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null &&
                                                                        x.Uf.ID == uf.ID);
                ListaPermissao.Remove(ufASerExcluido);
            }
        }

        #endregion

        #region "Permissão"

        public virtual void AdicionarPermissao(SolucaoEducacionalPermissao permissao)
        {
            //bool exists = this.ListaPermissao.Where(x => x.ID != permissao.ID).Count() == 0;

            ////Antes de adicionar, verifica se já existe na lista
            //if (!exists)
            //{
            //    this.ListaPermissao.Add(permissao);
            //}

            bool ufEstaNaLista = ListaPermissao.Any(x => x.Uf != null && x.Perfil.ID == permissao.ID && x.SolucaoEducacional.ID == this.ID);

            if (!ufEstaNaLista)
            {
                //TODO -> Retestar este ponto
                SolucaoEducacionalPermissao solucaoEducacionalPermissao = new SolucaoEducacionalPermissao() { Perfil = permissao.Perfil, SolucaoEducacional = this };
                this.ListaPermissao.Add(solucaoEducacionalPermissao);
            }

        }

        public virtual void RemoverPermissao(SolucaoEducacionalPermissao permissao)
        {
            //bool exists = this.ListaPermissao.Where(x => x.ID != permissao.ID).Count() == 0;
            //if (exists)
            //{
            //    var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.ID != null && x.ID == permissao.ID);
            //    this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            //}

            bool perfilEstaNaLista = ListaPermissao.Any(x => x.Perfil != null && x.Perfil.ID == permissao.ID && x.SolucaoEducacional.ID == ID);

            if (perfilEstaNaLista)
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Perfil != null && x.Perfil.ID == permissao.ID && x.SolucaoEducacional.ID == ID);

                ListaPermissao.Remove(programaPermissaoASerExcluido);
            }

        }

        public virtual bool PermiteVisualizacaoUf(int ufId)
        {
            return UFGestor != null && UFGestor.ID == ufId;
        }

        #endregion

        public virtual bool IsAcontecendo()
        {
            return Fim.HasValue && Inicio < DateTime.Now.Date && Fim.Value > DateTime.Now.Date;
        }

        public virtual bool UsuarioPossuiPermissaoMatricula(Usuario usuario)
        {
            var listaPerfil = usuario.ListaPerfil.Select(x => x.Perfil.ID).ToList();

            var permissaoPerfil = ListaPermissao.Any(x => x.Perfil != null && listaPerfil.Contains(x.Perfil.ID));

            var permissaoUf = ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == usuario.UF.ID);

            var permissaoNivel = ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == usuario.NivelOcupacional.ID);

            return permissaoPerfil && permissaoUf && permissaoNivel;
        }
    }
}

