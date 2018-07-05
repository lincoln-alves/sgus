using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOFiltroDesempenhoAcademico
    {
        private DTOFiltroDesempenhoAcademico()
        {

        }
        public DTOFiltroDesempenhoAcademico(string nome, string cPF, IEnumerable<int> nivelOcupacional, IEnumerable<int> uf, IEnumerable<int> publicosAlvo, DateTime? dataInicial, DateTime? dataFinal, DateTime? dataInicialTermino, DateTime? dataFinalTermino, DateTime? dataInicioTurma, DateTime? dataFinalTurma, IEnumerable<int> statusMatricula, int solucaoEducacional, int oferta, int turma, IEnumerable<int> categorias, IEnumerable<int> formasAquisicao, IEnumerable<int> ufResponsavel)
        {
            Nome = nome;
            CPF = cPF;
            NivelOcupacional = nivelOcupacional;
            Uf = uf;
            PublicosAlvo = publicosAlvo;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            DataInicialTermino = dataInicialTermino;
            DataFinalTermino = dataFinalTermino;
            DataInicioTurma = dataInicioTurma;
            DataFinalTurma = dataFinalTurma;
            StatusMatricula = statusMatricula;
            SolucaoEducacional = solucaoEducacional;
            Oferta = oferta;
            Turma = turma;
            Categorias = categorias;
            FormasAquisicao = formasAquisicao;
            UfResponsavel = ufResponsavel;
        }

        public string Nome { get; }
        public string CPF { get; }
        public IEnumerable<int> NivelOcupacional { get; }
        public IEnumerable<int> Uf { get; }
        public IEnumerable<int> PublicosAlvo { get; }
        public DateTime? DataInicial { get; }
        public DateTime? DataFinal { get; }
        public DateTime? DataInicialTermino { get; }
        public DateTime? DataFinalTermino { get; }
        public DateTime? DataInicioTurma { get; }
        public DateTime? DataFinalTurma { get; }
        public IEnumerable<int> StatusMatricula { get; }
        public int SolucaoEducacional { get; }
        public int Oferta { get; }
        public int Turma { get; }
        public IEnumerable<int> Categorias { get; }
        public IEnumerable<int> FormasAquisicao { get; }
        public IEnumerable<int> UfResponsavel { get; }

        public IDictionary<string, object> GetParams ()
        {

            IDictionary<string, object> lstParams = new Dictionary<string, object>();
            lstParams.Add("P_Nome", Nome);
            lstParams.Add("P_CPF", CPF);
            lstParams.Add("P_SolucaoEducacional", SolucaoEducacional);
            lstParams.Add("P_Oferta", Oferta);
            lstParams.Add("P_Turma", Turma);

            if (Uf != null && Uf.Any())
                lstParams.Add("P_UF", string.Join(", ", Uf));
            else
                lstParams.Add("P_UF", DBNull.Value);

            if (NivelOcupacional != null && NivelOcupacional.Any())
                lstParams.Add("P_Nivel_Ocupacional", string.Join(", ", NivelOcupacional));
            else
                lstParams.Add("P_Nivel_Ocupacional", DBNull.Value);

            if (StatusMatricula != null && StatusMatricula.Any())
                lstParams.Add("P_StatusMatricula", string.Join(", ", StatusMatricula));
            else
                lstParams.Add("P_StatusMatricula", DBNull.Value);

            if (PublicosAlvo != null && PublicosAlvo.Any())
                lstParams.Add("P_PublicoAlvo", string.Join(", ", PublicosAlvo));
            else
                lstParams.Add("P_PublicoAlvo", DBNull.Value);

            if (DataInicial.HasValue)
                lstParams.Add("P_Data_Inicial_Matricula", DataInicial.Value);
            else
                lstParams.Add("P_Data_Inicial_Matricula", DBNull.Value);

            if (DataFinal.HasValue)
                lstParams.Add("P_Data_Final_Matricula", DataFinal.Value);
            else
                lstParams.Add("P_Data_Final_Matricula", DBNull.Value);

            if (Categorias != null && Categorias.Any())
                lstParams.Add("P_Categorias", string.Join(", ", Categorias));
            else
                lstParams.Add("P_Categorias", DBNull.Value);

            if (DataInicialTermino.HasValue)
                lstParams.Add("P_Data_Inicial_Termino", DataInicialTermino.Value);
            else
                lstParams.Add("P_Data_Inicial_Termino", DBNull.Value);

            if (DataFinalTermino.HasValue)
                lstParams.Add("P_Data_Final_Termino", DataFinalTermino.Value);
            else
                lstParams.Add("P_Data_Final_Termino", DBNull.Value);

            if (DataInicioTurma.HasValue)
                lstParams.Add("P_Data_Inicio_Turma", DataInicioTurma.Value);
            else
                lstParams.Add("P_Data_Inicio_Turma", DBNull.Value);

            if (DataFinalTurma.HasValue)
                lstParams.Add("P_Data_Final_Turma", DataFinalTurma.Value);
            else
                lstParams.Add("P_Data_Final_Turma", DBNull.Value);

            if (FormasAquisicao != null && FormasAquisicao.Any())
                lstParams.Add("P_FormasAquisicao", string.Join(", ", FormasAquisicao));

            if (UfResponsavel != null && UfResponsavel.Any())
                lstParams.Add("P_UFResponsavel", string.Join(", ", UfResponsavel));
            else
                lstParams.Add("P_UFResponsavel", DBNull.Value);

            return lstParams;
        }
    }
}
