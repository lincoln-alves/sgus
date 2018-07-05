using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioTrilha : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { return enumRelatorio.SolucaoEducacionalOferta; }
        }

        public IList<DTORankingUsuarioTrilha> ConsultarRankingTrilhaUsuario(int pIdTrilha)
        {
            this.RegistrarLogExecucao();

            return (new ManterUsuarioTrilha()).ConsultarRankingTrilhaUsuario(pIdTrilha);
        }

        public IList<UsuarioTrilha> ObterUsuariosAusentes(DateTime dataLimite)
        {
            this.RegistrarLogExecucao();
            return (new ManterUsuarioTrilha()).ObterUsuariosAusentes(dataLimite);
        }


        public void Dispose()
        {
            GC.Collect();
        }

    }

    public class DTORankingUsuarioTrilha
    {
        public int ID_UsuarioTrilha { get; set; }
        public int ID_Usuario { get; set; }
        public int QT_Estrelas{ get; set; }
        public int QT_EstrelasPossiveis{ get; set; }
        public string Nome{ get; set; }
        public int UF{ get; set; }
        public string Email{ get; set; }
        public string CPF{ get; set; }
        public DateTime DataInicio{ get; set; }
        public DateTime? DataFim { get; set; }
        public int StatusMatricula { get; set; }
        public string Estado { get; set; }
        public string UF_nome { get; set; }
        public string UF_sigla { get; set; }
        public string Nivel { get; set; }
    }
}
