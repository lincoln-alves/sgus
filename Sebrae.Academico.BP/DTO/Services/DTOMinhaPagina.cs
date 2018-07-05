using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services.HistoricoAcademico;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOMinhaPagina
    {
        public List<DTOItemMeusCursos> ListaMinhasInscricoes { get; set; }
        public List<DTOItemHistoricoAcademico> ListaHistoricoAcademico { get; set; }
        public List<DTOSistemaExterno> ListaSistemasExternos { get; set; }
        public DTOAgenda DadosAgenda { get; set; }

        public DTOMinhaPagina(){
            ListaMinhasInscricoes = new List<DTOItemMeusCursos>();
            ListaHistoricoAcademico = new List<DTOItemHistoricoAcademico>();
        }
    }

    public class DTOAgenda {
        public int AnoInicial { get; set; }
        public int AnoFinal { get; set; }
        public string DataInicioMes { get; set; }
        public string DataFimMes { get; set; }
        public string DataAtual { get; set; }
        public string DiaSemana { get; set; }
        public string DiaSemanaInicioMes { get; set; }
        public List<DTOEventoAgenda> Eventos { get; set; }
        public List<string> Erros { get; set; } 

        public DTOAgenda() {
            Eventos = new List<DTOEventoAgenda>();
            Erros = new List<string>();
        }

        public string FormatoSemana {
            get {
                return "SEG,TER,QUAR,QUI,SEX,SAB,DOM";
            }
        }
    }

    public class DTOEventoAgenda {
        public int IdSolucaoEducacional { get; set; }
        public int IdTurma { get; set; }
        public int IdOferta { get; set; }
        public int IdMatriculaOferta { get; set; }
        public string IdMatriculaTurma { get; set; }
        public string Data { get; set; }
        public string Nome { get; set; }
        public int TipoEvento { get; set; }
    }
}
