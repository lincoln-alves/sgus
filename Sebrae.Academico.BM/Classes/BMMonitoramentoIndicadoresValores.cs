using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMonitoramentoIndicadoresValores : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<MonitoramentoIndicadoresValores> repositorio;

        public BMMonitoramentoIndicadoresValores()
        {
            repositorio = new RepositorioBase<MonitoramentoIndicadoresValores>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(MonitoramentoIndicadoresValores monitoramentoIndicadorValor)
        {
            repositorio.Salvar(monitoramentoIndicadorValor);
        }

        public void Salvar(IList<MonitoramentoIndicadoresValores> listaMonitoramentoIndicadorValores)
        {
            repositorio.Salvar(listaMonitoramentoIndicadorValores);
        }

        public IList<MonitoramentoIndicadoresValores> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public MonitoramentoIndicadoresValores ObterPorID(int pId)
        {

            MonitoramentoIndicadoresValores monitoramentoIndicadorValor = null;
            var query = repositorio.session.Query<MonitoramentoIndicadoresValores>();
            monitoramentoIndicadorValor = query.FirstOrDefault(x => x.ID == pId);
            return monitoramentoIndicadorValor;
        }

        public MonitoramentoIndicadoresValores ObterPorIndicadorAno(int idMonitoramentoIndicadores, string indicador)
        {

            MonitoramentoIndicadoresValores monitoramentoIndicadoresValores = null;
            var query = repositorio.session.Query<MonitoramentoIndicadoresValores>();
            monitoramentoIndicadoresValores = query.FirstOrDefault(x => x.Registro == indicador && x.MonitoramentoIndicador.ID == idMonitoramentoIndicadores);
            return monitoramentoIndicadoresValores;
        }

    }
}