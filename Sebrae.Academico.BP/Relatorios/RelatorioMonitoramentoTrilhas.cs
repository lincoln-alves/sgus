using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.BM.Classes.Moodle.Views;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioMonitoramentotrilhas : BusinessProcessBaseRelatorio, IDisposable
    {        
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.MonitoramentoTrilhas; }
        }
        
        public IList<DTOMonitorTrilhas> ObterMonitores(int idTrilha,int idNivelTrilha) {
            RegistrarLogExecucao();
            return (new ManterTrilha()).ObterMonitores(idTrilha, idNivelTrilha);
        }

        public IList<Trilha> ObterTrilhasComParticipacao() {
            var manterTrilha = new ManterTrilha();
            return manterTrilha.ObterTodasTrilhasComParticipacao();
        } 

        public IList<DTOMonitoramentoTrilhas> ConsultarMonitoramentoTrilhas(int idTrilha,int idNivelTrilha, int idUsuarioMonitor, int tipoParticipacao, DateTime dataInicial,DateTime dataFinal){
            RegistrarLogExecucao();

            return (new ManterTrilha()).ConsultarMonitoramentoTrilhas(idTrilha, idNivelTrilha, idUsuarioMonitor,
                tipoParticipacao, dataInicial, dataFinal);
        }

        public void Dispose(){
            GC.Collect();
        }
    }

}