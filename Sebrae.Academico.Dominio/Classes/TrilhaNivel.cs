using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaNivel : EntidadeBasica, ICloneable
    {
        public virtual int QuantidadeDiasPrazo { get; set; }
        //public virtual int QuantidadeDiasNovaTentativaProva { get; set; }
        //public virtual int? QuantidadeMaximaTentativasProva { get; set; }
        //public virtual int? QuantidadeDiasProrrogacaoProva { get; set; }
        public virtual TrilhaNivel PreRequisito { get; set; }
        public virtual Trilha Trilha { get; set; }
        public virtual Usuario Monitor { get; set; }
        public virtual IList<UsuarioTrilha> ListaUsuarioTrilha { get; set; }
        [Obsolete("A relação agora é feita através da missão -> ponto sebrae -> trilha nivel")]
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }

        public virtual enumTrilhaMapa Mapa { get; set; }

        /// <summary>
        /// Lista de Filhos do Nível da trilha
        /// </summary>
        public virtual IList<TrilhaNivel> ListaPreRequisito { get; set; }

        public virtual IList<QuestionarioAssociacao> ListaQuestionarioAssociacao { get; set; }
        public virtual IList<QuestionarioParticipacao> ListaQuestionarioParticipacao { get; set; }
        public virtual IList<DTOTrilhaNivelPermissao> ListaUsuariosPermitidos { get; set; }
        public virtual IList<PontoSebrae> ListaPontoSebrae { get; set; }

        public virtual TermoAceite TermoAceite { get; set; }

        public virtual string TextoTermoAceite { get; set; }

        public virtual decimal? NotaMinima { get; set; }

        public virtual string PorcentagensTrofeus { get; set; }

        public virtual IList<TrilhaNivelPermissao> ListaPermissao { get; set; }

        public virtual bool? AceitaNovasMatriculas { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual byte ValorOrdem { get; set; }
        public virtual byte? PrazoMonitorDiasUteis { get; set; }
        public virtual bool AvisarMonitor { get; set; }
        public virtual int LimiteCancelamento { get; set; }

        /// <summary>
        /// QUANTIDADE DE MOEDAS DE OURO PARA PROVA FINAL, quantidade de moedas que o Trilheiro deve obter para liberação da prova final de um nivel de trilha
        /// </summary>
        public virtual int? QuantidadeMoedasProvaFinal { get; set; }

        public virtual int? QuantidadeMoedasPorCurtida { get; set; }
        public virtual int? QuantidadeMoedasPorDescurtida { get; set; }

        #region "Atributos Lógicos"

        /// <summary>
        /// Id Lógico utilizado no cadastro de Trilha Nível
        /// </summary>
        public virtual int IdLogico { get; set; }

        /// <summary>
        /// Atributo lógico utilizado para indicar se o registro foi alterado ou se é um novo registro.
        /// </summary>
        public virtual enumStatusRegistro StatusRegistro { get; set; }

        public virtual bool RemoverPeloIdLogico { get; set; }

        public virtual bool MarcadoComoFilho { get; set; }

        /// <summary>
        /// Câmbio de Moedas, quantidade de moedas de prata que equivalem a uma moeda de ouro
        /// </summary>
        public virtual int? ValorPrataPorOuro { get; set; }

        #endregion


        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public TrilhaNivel()
        {
            this.ListaQuestionarioAssociacao = new List<QuestionarioAssociacao>();
            this.ListaQuestionarioParticipacao = new List<QuestionarioParticipacao>();
            this.ListaPermissao = new List<TrilhaNivelPermissao>();
            this.StatusRegistro = enumStatusRegistro.Novo;
            this.ListaUsuariosPermitidos = new List<DTOTrilhaNivelPermissao>();
            this.ListaUsuarioTrilha = new List<UsuarioTrilha>();
            this.ListaItemTrilha = new List<ItemTrilha>();
            this.ListaPreRequisito = new List<TrilhaNivel>();
        }

        public virtual IQueryable<UsuarioTrilha> ListaUsuarioTrilhaDoNivel
        {
            get
            {
                return ListaUsuarioTrilha.Where(ut => ut.TrilhaNivel.ID == ID && ut.NovasTrilhas == true).AsQueryable();
            }
        }

        public virtual bool PossuiInscritos
        {
            get
            {
                return ListaUsuarioTrilha.Any(x => x.TrilhaNivel.ID == this.ID && x.StatusMatricula == enumStatusMatricula.Inscrito);
            }
        }

        //public virtual int ObterTotalMoedas()
        //{
        //    return
        //        ListaPontoSebrae.SelectMany(x => x.ListaMissoes)
        //            .SelectMany(x => x.ListaItemTrilha)
        //            .Where(x => x.Usuario == null)
        //            .Sum(x => x.Moedas ?? 0);
        //}

        #region "Uf"

        public virtual void AdicionarUfs(Uf uf)
        {
            IList<Uf> ListaUfs =
                ListaPermissao.Where(x => x.Uf != null)
                    .Select(x => new Uf() { ID = x.Uf.ID, Nome = x.Uf.Nome })
                    .ToList<Uf>();
            TrilhaNivelPermissao trilhaNivelPermissao = new TrilhaNivelPermissao() { Uf = uf, TrilhaNivel = this };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaUfs.Where(x => x.ID == uf.ID).Any())
            {
                this.ListaPermissao.Add(trilhaNivelPermissao);
            }
        }

        public virtual void RemoverUf(Uf uf)
        {
            IList<Uf> ListaUfs =
                ListaPermissao.Where(x => x.Uf != null)
                    .Select(x => new Uf() { ID = x.Uf.ID, Nome = x.Uf.Nome })
                    .ToList<Uf>();

            if (ListaUfs.Where(x => x.ID == uf.ID).Any())
            {
                var programaPermissaoASerExcluido = ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == uf.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }





        public virtual void AdicionarUfs(List<Uf> ufs)
        {
            var tmp = ListaPermissao.ToList();
            List<TrilhaNivelPermissao> permissoes = new List<TrilhaNivelPermissao>();
            ufs.ForEach(uf =>
            {
                permissoes.Add(new TrilhaNivelPermissao()
                {
                    TrilhaNivel = this,
                    Uf = uf
                });
            });
            tmp.AddRange(permissoes);
            ListaPermissao = tmp;
        }

        public virtual void RemoverUfs(List<Uf> ufs)
        {
            var tmp = ListaPermissao.ToList();
            tmp.RemoveAll(x => x.Uf != null && !ufs.Select(u => u.ID).Contains(x.Uf.ID));
            ListaPermissao = tmp;
        }





        #endregion

        #region "Perfil"

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            IList<Perfil> ListaPerfils =
                this.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil() { ID = x.Perfil.ID, Nome = x.Perfil.Nome })
                    .ToList<Perfil>();
            TrilhaNivelPermissao trilhaNivelPermissao = new TrilhaNivelPermissao() { Perfil = perfil, TrilhaNivel = this };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaPerfils.Where(x => x.ID == perfil.ID).Any())
            {
                this.ListaPermissao.Add(trilhaNivelPermissao);
            }
        }

        public virtual void RemoverPerfil(Perfil perfil)
        {
            IList<Perfil> ListaPerfil =
                ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil() { ID = x.Perfil.ID, Nome = x.Perfil.Nome })
                    .ToList<Perfil>();


            if (ListaPerfil.Where(x => x.ID == perfil.ID).Any())
            {
                var programaPermissaoASerExcluido =
                    ListaPermissao.FirstOrDefault(x => x.Perfil != null && x.Perfil.ID == perfil.ID);
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
            IList<NivelOcupacional> ListaNivelOcupacional =
                ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome })
                    .ToList<NivelOcupacional>();
            TrilhaNivelPermissao trilhaNivelPermissao = new TrilhaNivelPermissao()
            {
                NivelOcupacional = nivelOcupacional,
                TrilhaNivel = this
            };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                this.ListaPermissao.Add(trilhaNivelPermissao);
            }
        }

        public virtual void RemoverNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            IList<NivelOcupacional> ListaNivelOcupacional =
                ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome })
                    .ToList<NivelOcupacional>();

            if (ListaNivelOcupacional.Where(x => x.ID == nivelOcupacional.ID).Any())
            {
                var programaPermissaoASerExcluido =
                    ListaPermissao.FirstOrDefault(
                        x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID);
                this.ListaPermissao.Remove(programaPermissaoASerExcluido);
            }
        }



        #endregion

        public virtual CertificadoTemplate CertificadoTemplate { get; set; }

        public override bool Equals(object obj)
        {
            TrilhaNivel objeto = obj as TrilhaNivel;

            if (objeto.RemoverPeloIdLogico)
            {
                return objeto == null ? false : this.IdLogico.Equals(objeto.IdLogico);
            }
            else
            {
                return objeto == null ? false : this.ID.Equals(objeto.ID);
            }
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public virtual bool UsuarioPossuiPermissao(Usuario usuario)
        {
            var perfis = usuario.ObterIdsPerfis();

            return ListaPermissao.Any(
                p => p.NivelOcupacional != null && p.NivelOcupacional.ID == usuario.NivelOcupacional.ID) &&
                   ListaPermissao.Any(p => p.Perfil != null && perfis.Contains(p.Perfil.ID)) &&
                   ListaPermissao.Any(p => p.Uf != null && p.Uf.ID == usuario.UF.ID);
        }

        /// <summary>
        /// Verifica se o usuário possui matrícula num nível de trilha. É um wraper.
        /// </summary>
        /// <param name="usuario">Objeto do usuário</param>
        /// <returns></returns>
        public virtual bool UsuarioPossuiMatricula(Usuario usuario)
        {
            return UsuarioPossuiMatricula(usuario.ID);
        }

        /// <summary>
        /// Verifica se o usuário possui matrícula num nível de trilha.
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <returns></returns>
        public virtual bool UsuarioPossuiMatricula(int usuarioId)
        {
            return ListaUsuarioTrilha.Any(ut => ut.Usuario.ID == usuarioId && ut.NovasTrilhas == true && ut.StatusMatricula != enumStatusMatricula.CanceladoAluno);
        }

        /// <summary>
        /// Verifica se o usuário cumpriu o nível de pré-requisito, caso exista cadastro.
        /// </summary>
        /// <param name="usuario">Usuário a ser verificado no pré-requisito.</param>
        /// <returns>Se não existir pré-requisito, volta true. Se existir pré-requisito e o usuário não tiver aprovação nele, volta false.</returns>
        public virtual string ObterPrerequisitoNaoCumprido(Usuario usuario)
        {
            var possuiPrerequisito = PreRequisito != null &&
                                     !PreRequisito.ListaUsuarioTrilha.Any(
                                         ut =>
                                             ut.StatusMatricula == enumStatusMatricula.Aprovado &&
                                             ut.Usuario.ID == usuario.ID);

            return possuiPrerequisito ? PreRequisito.Nome : null;
        }

        /// <summary>
        /// 
        /// Obter uma quantidade N de participantes da trilha, aleatoriamente.
        /// </summary>
        /// <param name="usuarioTrilhaIdLogado">ID do usuário logado, que não deverá visualizar seu PIN na tela.</param>
        /// <param name="qntMax">Quantidade máxima de participantes.</param>
        /// <returns></returns>
        public virtual List<int> ObterParticipantes(int usuarioTrilhaIdLogado, int? qntMax = 50)
        {
            // Obter somente os ids das matrículas que possuam participações nesse nível.
            var ids =
                ListaUsuarioTrilha
                    .Select(x => new {x.ID, x.NovasTrilhas, x.StatusMatricula})
                    .Where(x => x.NovasTrilhas == true && x.StatusMatricula == enumStatusMatricula.Inscrito &&
                                x.ID != usuarioTrilhaIdLogado)
                    .Select(x => x.ID)
                    .ToList();

            var selecionados = new List<int>();

            var randi = new Random();

            // Loopar pelos ids e ir removendo aleatoriamente de acordo com a quantidade de ids restantes.
            // O loop só acontece enquanto houverem ids ou alcançar o limite máximo (qntMax)
            for (var i = 1; i <= qntMax && ids.Any(); i++)
            {
                var valor = ids[randi.Next(ids.Count())];

                selecionados.Add(valor);

                // Remover o id selecionado a cada loop para não repetir.
                ids.Remove(valor);
            }

            return selecionados;
        }

        public virtual dynamic ObterLideres(UsuarioTrilha matricula, string enderecoSgus)
        {
            var matriculaLideres = ListaPontoSebrae.Select(x => x.ObterLiderDetalhado(enderecoSgus));

            return matriculaLideres.Where(x => x.Id == matricula.Usuario.ID).ToList();
        }

        public virtual IQueryable<Missao> ObterMissoes()
        {
            return ListaPontoSebrae.SelectMany(x => x.ListaMissoes).Distinct().AsQueryable();
        }

        public virtual IEnumerable<PontoSebrae> ObterPontosSebraeAtivos()
        {
            return ListaPontoSebrae.Where(x => x.Ativo);
        }

        public virtual IQueryable<ItemTrilha> ObterItensTrilha()
        {
            return ListaPontoSebrae.SelectMany(x => x.ListaMissoes).Distinct().AsQueryable().SelectMany(x => x.ListaItemTrilha).Distinct();
        }
    }
}