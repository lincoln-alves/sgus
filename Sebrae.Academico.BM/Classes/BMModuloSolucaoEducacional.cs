using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMModuloSolucaoEducacional : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<ModuloSolucaoEducacional> repositorio;

        #endregion

        #region "Construtor"

        public BMModuloSolucaoEducacional()
        {
            repositorio = new RepositorioBase<ModuloSolucaoEducacional>();
        }

        #endregion

        public void CadastrarLista(List<ModuloSolucaoEducacional> listaModuloSolucaoEducacional, int idModulo)
        {
            foreach (var moduloSolucaoEducacional in listaModuloSolucaoEducacional)
            {
                moduloSolucaoEducacional.Modulo.ID = idModulo;
                if (moduloSolucaoEducacional.ID == 0)
                {
                    this.Salvar(moduloSolucaoEducacional);
                }
            }

            this.MergeModuloSolucaoEducacional(listaModuloSolucaoEducacional, idModulo);
        }

        public IList<ModuloSolucaoEducacional> ObterPorModulo(int idModulo)
        {
            var query = repositorio.session.Query<ModuloSolucaoEducacional>();
            return query.Where(x => x.Modulo.ID == idModulo).ToList();
        }

        private void MergeModuloSolucaoEducacional(List<ModuloSolucaoEducacional> listaModuloSolucaoEducacional, int idModulo)
        {
            var moduloSE = this.ObterPorModulo(idModulo);// new BMModulo().ObterPorId(idModulo);
            foreach (var moduloSolucaoEducacional in moduloSE)
            {
                if (moduloSolucaoEducacional.ID > 0)
                {
                    if (!listaModuloSolucaoEducacional.Any(x => x.ID == moduloSolucaoEducacional.ID))
                    {
                        this.Excluir(moduloSolucaoEducacional);
                    }
                }
            }
        }

        public void Excluir(ModuloSolucaoEducacional pModuloSolucaoEducacional)
        {
            repositorio.Excluir(pModuloSolucaoEducacional);
        }

        public void Salvar(ModuloSolucaoEducacional pModuloSolucaoEducacional)
        {
            repositorio.Salvar(pModuloSolucaoEducacional);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }


        public ModuloSolucaoEducacional ProgramaPossuiSolucao(int idPrograma, int idSolucao)
        {
            var query = repositorio.session.Query<ModuloSolucaoEducacional>();            

            query = query.Where(x => x.Modulo.Capacitacao.Programa.ID == idPrograma);
            query = query.Where(x => x.SolucaoEducacional.ID == idSolucao);       

            return query.FirstOrDefault<ModuloSolucaoEducacional>();
        }

        public ModuloSolucaoEducacional CapacitacaoPossuiSolucao(int idCapacitacao, int idSolucao)
        {
            var query = repositorio.session.Query<ModuloSolucaoEducacional>();

            query = query.Where(x => x.Modulo.Capacitacao.ID == idCapacitacao);
            query = query.Where(x => x.SolucaoEducacional.ID == idSolucao);

            return query.FirstOrDefault();
        }

        public IList<ModuloSolucaoEducacional> ObterSolucoesPorModulo(int idModulo)
        {
            var query = repositorio.session.Query<ModuloSolucaoEducacional>();
            query = query.Where(x => x.Modulo.ID == idModulo).Select(y => new ModuloSolucaoEducacional() { ID = y.SolucaoEducacional.ID, Nome = y.SolucaoEducacional.Nome });
            return query.ToList();
        }

    }
}
