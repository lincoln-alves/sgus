using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EntidadeBasicaComStatus : EntidadeBasicaPorId
    {

        public virtual enumStatusMatricula StatusMatricula { get; set; }

        #region "Propriedades que não serão mapeadas"

        public virtual string StatusMatriculaFormatado
        {
            get
            {
                string statusMatriculaPorExtensao = string.Empty;

                switch (StatusMatricula)
                {
                    case enumStatusMatricula.Inscrito:
                        statusMatriculaPorExtensao = "Inscrito";
                        break;

                    case enumStatusMatricula.CanceladoAluno:
                        statusMatriculaPorExtensao = "Cancelado pelo Aluno";
                        break;

                    case enumStatusMatricula.CanceladoAdm:
                        statusMatriculaPorExtensao = "Cancelado pela Secretaria";
                        break;

                    case enumStatusMatricula.Abandono:
                        statusMatriculaPorExtensao = "Abandono";
                        break;

                    case enumStatusMatricula.PendenteConfirmacaoAluno:
                        statusMatriculaPorExtensao = "Pendente de confirmação pelo Aluno";
                        break;

                    case enumStatusMatricula.Concluido:
                        statusMatriculaPorExtensao = "Concluído";
                        break;

                    case enumStatusMatricula.FilaEspera:
                        statusMatriculaPorExtensao = "Fila de Espera";
                        break;

                    case enumStatusMatricula.Aprovado:
                        statusMatriculaPorExtensao = "Aprovado";
                        break;

                    case enumStatusMatricula.Reprovado:
                        statusMatriculaPorExtensao = "Reprovado";
                        break;

                    case enumStatusMatricula.AprovadoComoMultiplicador:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como multiplicador";
                        break;

                    case enumStatusMatricula.AprovadoComoMultiplicadorComAcompanhamento:
                        statusMatriculaPorExtensao = "Aprovado Como Multiplicador Com Acompanhamento";
                        break;

                    case enumStatusMatricula.AprovadoComoFacilitador:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como facilitador";
                        break;

                    case enumStatusMatricula.AprovadoComoFacilitadorComAcompanhamento:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como facilitador c/acompanhamento";
                        break;

                    case enumStatusMatricula.AprovadoComoConsultor:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como consultor";
                        break;

                    case enumStatusMatricula.AprovadoComoConsultorComAcompanhamento:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como consultor c/acompanhamento";
                        break;

                    case enumStatusMatricula.AprovadoComoModerador:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como moderador";
                        break;

                    case enumStatusMatricula.AprovadoComoModeradorComAcompanhamento:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como moderador c/acompanhamento";
                        break;

                    case enumStatusMatricula.AprovadoComoGestor:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como gestor";
                        break;

                    case enumStatusMatricula.AprovadoComoFacilitadorConsultor:
                        statusMatriculaPorExtensao = "Apto(a) para atuar como facilitador e consultor";
                        break;

                    case enumStatusMatricula.CanceladoGestor:
                        statusMatriculaPorExtensao = "Cancelado Pelo Gestor";
                        break;

                    case enumStatusMatricula.Ouvinte:
                        statusMatriculaPorExtensao = "Ouvinte";
                        break;
                }

                return statusMatriculaPorExtensao;
            }
        }

        #endregion
    }
}
