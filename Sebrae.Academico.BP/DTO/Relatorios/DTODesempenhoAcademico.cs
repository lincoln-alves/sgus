using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTODesempenhoAcademico
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string NivelOcupacional { get; set; }
        public string UF { get; set; }
        public string Programa { get; set; }
        public string Trilha { get; set; }
        public string CategoriaPai { get; set; }
        public string Categoria2 { get; set; }
        public string Categoria3 { get; set; }
        public string Categoria { get; set; }
        public string SolucaoEducacional { get; set; }
        public int ID_SolucaoEducacional { get; set; }
        public int ID_Oferta { get; set; }
        public int ID_OfertaCodigoMoodle { get; set; }
        public int ID_Usuario { get; set; }
        public int ID_Turma { get; set; }
        public string FormaAquisicao { get; set; }
        public int CargaHoraria { get; set; }
        public string TipoTutoria { get; set; }
        public string Turma { get; set; }
        public string TemMaterial { get; set; }
        public string StatusMatricula { get; set; }
        public decimal Nota1 { get; set; }
        public decimal Nota2 { get; set; }
        public decimal NotaProvaOnline { get; set; }
        public decimal MediaFinal { get; set; }
        public DateTime? DataInicioInscricoesOferta { get; set; }
        public DateTime? DataFimInscricoesOferta { get; set; }
        public DateTime? DataMatricula { get; set; }
        public DateTime? DataTermino { get; set; }
        public DateTime? DataFinalTurma { get; set; }
        public DateTime? DataInicioTurma { get; set; }
        public DateTime? DataFimTurma { get; set; }
        public string Fornecedor { get; set; }
        public DateTime? DataGeracaoCertificado { get; set; }
        public string DataGeracaoCertificadoString { get; set; }
        public string Oferta { get; set; }
        public string Observacao { get; set; }
        public string Unidade { get; set; }
        public string Cidade { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string Sexo { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int SolucaoEducacionalQuantidadeDeAcessos { get; set; }
        public string Feedback { get; set; }
        public string UFResponsavel { get; set; }
        public string ResponsavelPelaTurma { get; set; }

        public DateTime? DataTerminoFormatado
        {
            get
            {
                // Nas Turmas já finalizadas, para os alunos que estão com a "Data de Conclusão do Aluno" em branco, incluir a "DATA FINAL DA TURMA".
                if (DataFinalTurma < DateTime.Now && this.DataTermino == null)
                {
                    return this.DataFinalTurma;
                }
                // Para os Alunos que possuem: "Data de Conclusão do Aluno", maior que a "DATA FINAL DA TURMA", Alterar a "Data de Conclusão do Aluno" para a "DATA FINAL DA TURMA".
                else if (this.DataTermino > this.DataFinalTurma)
                {
                    return this.DataFinalTurma;
                }

                return this.DataTermino;
            }
        }

        public string DataInicioInscricoesOfertaString
        {
            get
            {
                return DataInicioInscricoesOferta.HasValue ? DataInicioInscricoesOferta.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string DataFimInscricoesOfertaString
        {
            get
            {
                return DataFimInscricoesOferta.HasValue ? DataFimInscricoesOferta.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string DataMatriculaString
        {
            get
            {
                return DataMatricula.HasValue ? DataMatricula.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string DataTerminoString
        {
            get
            {
                return DataTermino.HasValue ? DataTermino.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string DataAdmissaoString
        {
            get
            {
                return DataAdmissao.HasValue ? DataAdmissao.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string DataInicioTurmaString
        {
            get
            {
                return DataInicioTurma.HasValue ? DataInicioTurma.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string DataFimTurmaString
        {
            get
            {
                return DataFimTurma.HasValue ? DataFimTurma.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
            }
        }

        public string Cargos { get; set; }
    }
}
