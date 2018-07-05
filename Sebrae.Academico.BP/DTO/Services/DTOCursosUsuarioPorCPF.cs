using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOCursosUsuarioPorCPF
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public List<DTOSolucaoEducacionalCursosUsuarioPorCPF> SolucoesEducacionaisMoodle { get; set; }
        public List<DTOTrilhasUsuarioPorCPF> TrilhasMoodle { get; set; }
        
        public DTOCursosUsuarioPorCPF()
        {
            SolucoesEducacionaisMoodle = new List<DTOSolucaoEducacionalCursosUsuarioPorCPF>();
            TrilhasMoodle = new List<DTOTrilhasUsuarioPorCPF>();
        }
    }

    public class DTOSolucaoEducacionalCursosUsuarioPorCPF
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string IDChaveExterna { get; set; }
        public List<DTOOfertaCursosUsuarioPorCPF> Oferta { get; set; }
        public DTOSolucaoEducacionalCursosUsuarioPorCPF()
        {
            this.Oferta = new List<DTOOfertaCursosUsuarioPorCPF>();
        }
    }

    public class DTOOfertaCursosUsuarioPorCPF
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string ID_ChaveExterna { get; set; }
        public int? CodigoMoodle { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataInicioInscricoes { get; set; }
        public DateTime? DataFimInscricoes { get; set; }
        public DTOTurmaCursosUsuarioPorCPF Turma { get; set; }
        public string StatusMatricula { get; set; }
        public int IdStatusMatricula { get; set; }

        public DTOOfertaCursosUsuarioPorCPF()
        {
            Turma = new DTOTurmaCursosUsuarioPorCPF();
        }
    }

    public class DTOTurmaCursosUsuarioPorCPF
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string IDChaveExternaTurma { get; set; }
    }


    public class DTOTrilhasUsuarioPorCPF
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string IDChaveExterna { get; set; }
    }
}
