using System.Collections.Generic;
using System;

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class TrilhaNivelDTO
    {        
        public virtual int ID {get; set;}
        public virtual string Nome {get; set;}
        public virtual int Ordem { get; set; }
        public virtual int IdPreReq {get; set;}
        public virtual string NomePreReq {get; set;}
        public virtual int DiasPrazo {get; set;}
        public virtual string DataLimite { get; set; }
        public virtual string TermoAceite { get; set; }
        public virtual string Descricao { get; set; }
        public virtual decimal? NotaMinima {get; set;}
        public virtual decimal? NotaObtida { get; set; }
        public virtual string Creditos { get; set; }

        public virtual string LinkJogo { get; set; }

        public virtual int UsuarioTrilha { get; set; }
        public virtual int QtdEstrelasPossiveis { get; set; }
        public virtual int QtdEstrelas { get; set; }
        public virtual bool HabilitadoAcesso { get; set; }      // retorna true se o usuário pode clicar no nível

        public virtual bool HabilitadoMatricula { get; set; }   // retornar false se ele estiver com o status inscrito ou concluído.
                                                                // se não inscrito: retorna false se houver pré-requisito E se este não estiver como concluído
                                                                // demais retona true.
        public virtual string StatusNivel { get; set; } // Inscrito / Concluido / Vazio (Não cursou ainda)
        public virtual bool HabilitaProvaTrilha { get; set; }
        public virtual DateTime? DataLiberacaoNovaProva { get; set; }
        public virtual bool NovaProvaLiberada {get; set;}
        public virtual int CargaHoraria { get; set; }
        public virtual int QuantidadeSolucoesUC { get; set; }
        public virtual int QuantidadeSolucoesAI { get; set; }
        public virtual int QuantidadeSolucoesUCConcluidas { get; set; }
        public virtual int QuantidadeSolucoesAIConcluidas { get; set; }
        public virtual int QuantidadeTopicos { get; set; }
        public virtual int QuantidadeTopicosConcluidos { get; set; }

        //Status Botoes => B = Bloqueado / A = Ativo / AC = Ativo e Clicavel
        public virtual string StatusBtnCredenciamento { get; set; }
        public virtual string StatusBtnDiagnosticoEntrada { get; set; }
        public virtual string StatusBtnDepoimento { get; set; }
        public virtual string StatusBtnBandeirada { get; set; }
        public virtual string StatusBtnCertificado { get; set; }
        public virtual string StatusBtnHistoricoPercurso { get; set; }

        public virtual decimal PercentualConclusao { get; set; }
        public virtual int NivelTrofeu { get; set; }

        public virtual QuestionarioDTO QuestionarioPre { get; set; }
        public virtual QuestionarioDTO QuestionarioPos { get; set; }
        public virtual QuestionarioDTO QuestionarioEvolutivoPre { get; set; }
        public virtual QuestionarioDTO QuestionarioEvolutivoPos { get; set; }
        public virtual List<TrilhaTopicoTematicoDTO> ListaTopicoTematico { get; set; }

        public virtual int HabilitaCancelamento { get; set; }
        public virtual int IdUsuarioTrilha { get; set; }

        public TrilhaNivelDTO()
        {
            ListaTopicoTematico = new List<TrilhaTopicoTematicoDTO>();
            QuestionarioPre = new QuestionarioDTO();
            QuestionarioPos = new QuestionarioDTO();
            QuestionarioEvolutivoPre = new QuestionarioDTO();
            QuestionarioEvolutivoPos = new QuestionarioDTO();
            HabilitaProvaTrilha = false;
            HabilitadoAcesso = false;
            HabilitadoMatricula = false;
            PercentualConclusao = 0;
            NivelTrofeu = 0;
        }
    }
}
