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
    public class BMMonitoramentoIndicadores : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<MonitoramentoIndicadores> repositorio;

        public BMMonitoramentoIndicadores()
        {
            repositorio = new RepositorioBase<MonitoramentoIndicadores>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(MonitoramentoIndicadores monitoramentoIndicador) {
            //demanda #3587
            var obter = ObterPorFiltro(monitoramentoIndicador).FirstOrDefault(p => p.ID != monitoramentoIndicador.ID);
            if (obter != null) {
                throw new AcademicoException("Já existe no banco de dados um registro com esse nome.");
            }
            //fim demanda #3587

            repositorio.Salvar(monitoramentoIndicador);
        }

        public void Salvar(IList<MonitoramentoIndicadores> listaMonitoramentoIndicadores)
        {
            //demanda #3587
            foreach (var item in listaMonitoramentoIndicadores) {
                var obter = ObterPorFiltro(item).FirstOrDefault(p => p.ID != item.ID);
                if (obter != null){
                    throw new AcademicoException("Já existe no banco de dados um registro com esse nome.");
                }
            }
            //fim demanda #3587

            repositorio.Salvar(listaMonitoramentoIndicadores);
        }

        public IList<MonitoramentoIndicadores> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(p=>p.Ano).ToList();
        }

        public MonitoramentoIndicadores ObterPorID(int pId)
        {

            MonitoramentoIndicadores monitoramentoIndicador = null;
            var query = repositorio.session.Query<MonitoramentoIndicadores>();
            monitoramentoIndicador = query.FirstOrDefault(x => x.ID == pId);
            return monitoramentoIndicador;
        }
        public IQueryable<MonitoramentoIndicadores> ObterPorFiltro(MonitoramentoIndicadores monitoramentoIndicador)
        {
            var query = repositorio.session.Query<MonitoramentoIndicadores>();
            return query.Where(x => x.Ano == monitoramentoIndicador.Ano);
        }

        public void Excluir(int id) {
            var monitoramentoIndicador = ObterPorID(id);
            if(monitoramentoIndicador == null)throw new AcademicoException("Registro não encontrado");
            repositorio.Excluir(monitoramentoIndicador);
        }

        public MonitoramentoIndicadores ObterPorAno(int ano = 0)
        {

            MonitoramentoIndicadores monitoramentoIndicador = null;
            var query = repositorio.session.Query<MonitoramentoIndicadores>();
            monitoramentoIndicador = query.FirstOrDefault(x => x.Ano == ano);
            return monitoramentoIndicador;
        }
    }
}