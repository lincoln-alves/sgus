using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Turma : EntidadeBasica, ICloneable
    {
        public Turma()
        {
            ListaMatriculas = new List<MatriculaTurma>();
            ListaQuestionarioAssociacao = new List<QuestionarioAssociacao>();
            //ListaProfessores = new List<TurmaProfessor>();
        }

        public virtual string NomeSalvo { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual string Local { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFinal { get; set; }
        public virtual string TipoTutoria { get; set; }
        public virtual bool InAberta { get; set; }
        public virtual decimal? NotaMinima { get; set; }
        public virtual IList<MatriculaTurma> ListaMatriculas { get; set; }
        public virtual IList<QuestionarioAssociacao> ListaQuestionarioAssociacao { get; set; }
        public virtual IEnumerable<QuestionarioParticipacao> ListaQuestionarioParticipacao { get; set; }
        //public virtual IList<TurmaProfessor> ListaProfessores { get; set; }
        public virtual int QuantidadeMaximaInscricoes { get; set; }
        public virtual int? AcessoWifi {get;set;}
        public virtual int? Sequencia { get; set; }
        public virtual bool AcessoAposConclusao { get; set; }
        public virtual Usuario Responsavel { get; set; }
        public virtual Usuario ConsultorEducacional { get; set; }
        public virtual enumStatusTurma? Status { get; set; }
        public virtual IList<JustificativaStatus> JustificativasStatus { get; set; }
        public virtual IList<Informe> Informes { get; set; }
        public virtual IList<Usuario> Professores { get; set; }
        public virtual IList<Avaliacao> Avaliacoes { get; set; }
        public virtual bool InAvaliacaoAprendizagem { get; set; }

        public virtual bool PodeVisualizarAvaliacao(Usuario usuarioLogado)
        {
            var avaliacao = Avaliacoes.FirstOrDefault();

            return usuarioLogado.IsGestor()
                ? !(avaliacao == null || avaliacao.Status != enumStatusAvaliacao.AguardandoGestor)
                : usuarioLogado.IsConsultorEducacional();
        }

        #region "Propriedades que não serão mapeadas"


        public virtual int QuantidadeAlunosMatriculadosNaTurma
        {
            get { return ListaMatriculas.Count(x => x.MatriculaOferta != null && !x.MatriculaOferta.IsCancelado()); }
        }

        public virtual string NomeOferta
        {
            get
            {
                string nomeOferta = string.Empty;

                if (this.Oferta != null && this.Oferta.ID > 0)
                {
                    nomeOferta = this.Oferta.Nome;
                }

                return nomeOferta;
            }
        }

        public virtual string NomeSolucaoEducacional
        {
            get
            {
                string nomeSolucaoEducacional = string.Empty;

                if (this.Oferta != null && this.Oferta.ID > 0)
                {
                    nomeSolucaoEducacional = this.Oferta.NomeSolucaoEducacional;
                }

                return nomeSolucaoEducacional;
            }
        }

        public virtual string Codigo
        {
            get
            {
                var id = Sequencia != null ? Sequencia.Value : ID;

                if (Oferta != null && Oferta.Codigo != "-")
                    return string.Format("{0}.T{1}",
                        Oferta.Codigo,
                        id);

                return "N/D";
            }
        }

        public virtual string DescricaoSequencial
        {
            get
            {
                var id = Sequencia != null ? Sequencia.Value : ID;

                if (Oferta != null && Oferta.DescricaoSequencial != "-")
                    return string.Format("{0} - {1}.T{2}",
                        Nome,
                        Oferta.DescricaoSequencial.Replace("*", ""),
                        id);

                return "-";
            }
        }

        public virtual bool PermitirLogResponsavel { get; set; }

        public virtual bool PermitirLogConsultor { get; set; }

        public virtual Usuario LogResponsavel { get; set; }

        public virtual Usuario LogConsultorEducacional { get; set; }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public override bool Equals(object obj)
        {
            Turma objeto = obj as Turma;
            return objeto == null ? false : this.ID.Equals(objeto.ID);
        }

        public virtual string ObterJustificativa()
        {
            return JustificativasStatus != null && JustificativasStatus.Any()
                ? JustificativasStatus.FirstOrDefault().Descricao
                : "";
        }

        // Adiciona/altera um texto de justificativa.
        public virtual void AdicionarJustificativa(string texto)
        {
            var justificativa = JustificativasStatus.FirstOrDefault() ?? new JustificativaStatus(this);

            justificativa.Descricao = texto;

            // Limpar os Status existentes.
            JustificativasStatus.Clear();
            JustificativasStatus.Add(justificativa);
        }

        public virtual bool IsVigente()
        {
            return DataInicio != null && DataInicio >= DateTime.Now && DataFinal != null && DateTime.Now <= DataFinal;
        }
    }
}
