using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMModuloPreRequisito : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<ModuloPreRequisito> repositorio;

        #endregion

        #region "Construtor"

        public BMModuloPreRequisito()
        {
            repositorio = new RepositorioBase<ModuloPreRequisito>();
        }

        #endregion

        public void Salvar(ModuloPreRequisito pModuloPreRequisito)
        {
            repositorio.Salvar(pModuloPreRequisito);

        }

        public void CadastrarLista(List<ModuloPreRequisito> listaModuloPreRequisito, int idModulo)
        {
            if (listaModuloPreRequisito.Count() > 0)
            {
                foreach (var moduloPreRequisito in listaModuloPreRequisito)
                {
                    moduloPreRequisito.ModuloFilho.ID = idModulo;
                    if (moduloPreRequisito.ID == 0)
                    {
                        this.Salvar(moduloPreRequisito);
                    }
                    else
                    {
                        this.MergeModuloPreRequisito(listaModuloPreRequisito, idModulo);
                    }
                }
            }
            else
                this.MergeModuloPreRequisito(listaModuloPreRequisito, idModulo);
        }

        private void MergeModuloPreRequisito(List<ModuloPreRequisito> listaModuloPreRequisito, int idModulo)
        {
            var recuperarModulosPreRequisito = this.ObterPorModulo(idModulo);// new BMModulo().ObterPorId(idModulo);
            foreach (var moduloPreRequisito in recuperarModulosPreRequisito)
            {
                if (moduloPreRequisito.ID > 0)
                {
                    if (!listaModuloPreRequisito.Any(x => x.ModuloPai.ID == moduloPreRequisito.ModuloPai.ID))
                    {
                        this.Excluir(moduloPreRequisito);
                    }
                }
            }
        }

        public IList<ModuloPreRequisito> ObterPorModulo(int idModulo)
        {
            var query = repositorio.session.Query<ModuloPreRequisito>();
            return query.Where(x => x.ModuloFilho.ID == idModulo).ToList();
        }

        public void Excluir(ModuloPreRequisito pModuloPreRequisito)
        {
            repositorio.Excluir(pModuloPreRequisito);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public Modulo ListPreRequisitosPorModulo(ModuloSolucaoEducacional modSol, int IdUsuario){

            Modulo modReq = new Modulo();

            var query = repositorio.session.Query<ModuloPreRequisito>(); 

            query = query.Where(x => x.ModuloFilho == modSol.Modulo);

            IList<ModuloPreRequisito> modPreReqs = query.ToList();

            if (modPreReqs.Count() > 0)
            {
                BMMatriculaOferta matOfer = new BMMatriculaOferta();

                foreach(ModuloPreRequisito modPreReq in modPreReqs){
                    
                    int totalSol = modPreReq.ModuloPai.ListaSolucaoEducacional.Count();

                    if (modPreReq.ModuloPai.ListaSolucaoEducacional.Count() > 0) { 

                        foreach (ModuloSolucaoEducacional modPreReqSol in modPreReq.ModuloPai.ListaSolucaoEducacional)
                        {
                            if (!matOfer.AprovacaoPorUsuarioESolucaoEducacional(IdUsuario, modPreReqSol.SolucaoEducacional.ID))
                            {                            
                                return modPreReq.ModuloPai;
                            }                        

                        }

                    }
                                        
                }
            }
            return modReq;
        }
    }
}
