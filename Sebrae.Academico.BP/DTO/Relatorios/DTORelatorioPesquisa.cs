using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioPesquisa
    {
        public string Enunciado { get; set; }
        public string TopicosAvaliados { get; set; }
        public string Moda { get; set; }
        public string Media { get; set; }
        public string DP { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string MediaFinal { get; set; }
        public string Categoria { get; set; }
        public string FormaAquisicao { get; set; }
        public string SolucaoEducacional { get; set; }
        public string Oferta { get; set; }
        public string Turma { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string QntAlunosMatriculadosTurma { get; set; }
        public string QntAlunosRespostaQuestionario { get; set; }
        public string QntAlunosChegaramFinalCurso { get; set; }
        public string PctAlunosRespostaQuestionarioVsTotalAlunos { get; set; }
        public string PctAlunosChegaramFinalCurso { get; set; }
        public string QtdTutoresSolucaoEducacional { get; set; }
    }



    public class DTORelQuestionarioComItemPesquisa
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public int ID_CategoriaConteudo { get; set; }
    }

    public class DTORelatorioQuestionarioEnunciado
    {
        public DTORelatorioQuestionarioEnunciado()
        {
            QuestoesRelacionadas = new List<DTORelatorioQuestionarioQuestao>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public List<DTORelatorioQuestionarioQuestao> QuestoesRelacionadas { get; set; }
    }

    public class DTORelatorioQuestionarioQuestao
    {
        public DTORelatorioQuestionarioQuestao()
        {
            AvaliaProfessor = false;
            ItensQuestionarioParticipacaoIds = new List<int>();
        }

        public int Id { get; set; }
        public int IdEnunciado { get; set; }
        public string Nome { get; set; }
        public bool AvaliaProfessor { get; set; }
        public int? IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        /// <summary>
        /// IDs dos ItensQuestionarioParticipacao vinculados.
        /// </summary>
        public List<int> ItensQuestionarioParticipacaoIds { get; set; }
    }

    public class DTORelatorioQuestionarioParticipacao
    {
        public DTORelatorioQuestionarioParticipacao()
        {
            Respostas = new List<DTORelatorioQuestionarioResposta>();
        }

        public int IdQuestionarioResposta { get; set; }
        public string Questionario { get; set; }
        public string Curso { get; set; }
        public string Nome { get; set; }
        public DateTime? Data { get; set; }
        public List<DTORelatorioQuestionarioResposta> Respostas { get; set; }
        
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }

        public string Oferta { get; set; }

        public string Turma { get; set; }
    }

    public class DTORelatorioQuestionarioTutor
    {
        public DTORelatorioQuestionarioTutor()
        {
            Respostas = new List<DTORelatorioQuestionarioResposta>();
        }
        public string Tutor { get; set; }
        public List<DTORelatorioQuestionarioResposta> Respostas { get; set; }
    }

    public class DTORelatoriQuestaoRespostas
    {
        public DTORelatoriQuestaoRespostas()
        {
            Respostas = new List<DTORelatorioQuestionarioResposta>();
        }

        public DTORelatorioQuestionarioQuestao Questao { get; set; }
        public List<DTORelatorioQuestionarioResposta> Respostas { get; set; }
    }

    public class DTORelatorioQuestionarioResposta
    {
        public DTORelatorioQuestionarioResposta()
        {
            Questao = new DTORelatorioQuestionarioQuestao();
        }
        
        public int? Nota { get; set; }
        public string NotaTexto { get; set; }
        public int? IdProfessor { get; set; }
        public DTORelatorioQuestionarioQuestao Questao { get; set; }
    }

    public class Respondente
    {
        public int IdQuestionarioParticipacao { get; set; }
        public int ID_Usuario { get; set; }
        public int ID_Questionario { get; set; }
        public int ID_Turma { get; set; }
        public int ID_UF { get; set; }
        public int ID_TipoQuestionarioAssociacao { get; set; }
        public DateTime? DT_Participacao { get; set; }
    }

    public class DTORelatorioQuestionarioRespondente
    {
        public List<DTORelatorioQuestionarioEnunciado> Enunciados { get; set; }
        public List<DTORelatorioQuestionarioQuestao> Questoes { get; set; }

        public IQueryable<QuestionarioParticipacao> Consulta { get; set; }
        public int TotalQuestoes { get; set; }
        public int TotalRespostas { get; set; }
    }

    public class DTORelatorioQuestionarioEstatistico
    {
        public int QtdeItens { get; set; }
        public string Nome { get; set; }
        public double Media { get; set; }
        public double DP { get; set; }
        public int Moda { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Principal { get; set; }
        public double MediaFinal { get; set; }
    }

    public class DTOFiltroRelatorioQuestionario
    {
        // Esse campo é obrigatório, então não é nulável.
        public int IdQuestionario { get; set; }

        public List<int> IdsCategorias { get; set; }
        public int? IdSolucaoEducacional { get; set; }
        public int? IdOferta { get; set; }
        public int? IdTurma { get; set; }
        public List<int> IdsUf { get; set; }
        public List<int> IdsNivelOcupacional { get; set; }
        public int? IdProcesso { get; set; }
        public List<int> IdsStatusMatricula { get; set; }
        public int? IdTipoQuestionario { get; set; }
        public bool IsRelatorioTutor { get; set; }
        public int? IdProfessor { get; set; }


        public bool PossuiDados()
        {
            return IdQuestionario != 0 || IdsCategorias != null || (IdsCategorias != null && IdsCategorias.Any()) ||
                   IdSolucaoEducacional.HasValue || IdOferta.HasValue || IdTurma.HasValue || IdsUf.Any() ||
                   IdsNivelOcupacional.Any() || IdProcesso.HasValue || IdsStatusMatricula.Any();
        }
    }
}
