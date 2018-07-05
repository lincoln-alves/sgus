//using System.Collections.Generic;
//using System.Linq;
//using NHibernate.Linq;
//using Sebrae.Academico.Dominio.Classes;
//using Sebrae.Academico.InfraEstrutura.Core.Helper;
//using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
//using System;


//namespace Sebrae.Academico.BM.Classes
//{
//    public class BMIndiceImportancia : BusinessManagerBase
//    {

//        #region "Atributos Privados"

//        private RepositorioBase<IndiceImportancia> repositorio = null;

//        #endregion

//        #region "Construtor"

//        public BMIndiceImportancia()
//        {
//            repositorio = new RepositorioBase<IndiceImportancia>();
//        }

//        #endregion

//        #region "Métodos Privados"


//        #endregion

//        #region "Métodos Públicos"

//        public void Salvar(IndiceImportancia pIndiceImportancia)
//        {
//            //ValidarItemTrilhaInformado(pIndiceImportancia);

//            ////Se Id =0, significa insert.
//            //if (pIndiceImportancia.ID == 0)
//            //{
//            //    pIndiceImportancia.DataCriacao = DateTime.Now;
//            //}
//            repositorio.Salvar(pIndiceImportancia);
//        }

//        public IList<IndiceImportancia> ObterTodos()
//        {
//            return repositorio.ObterTodos().ToList<IndiceImportancia>();
//        }

//        public IndiceImportancia ObterPorID(int idUsuario, int idTrilhaNivel, int idTrilhaTopicoTematico, int idObjetivo, bool inPre)
//        {
//            var query = repositorio.session.Query<IndiceImportancia>();

//            query = query.Where(x => x.Usuario.ID == idUsuario);
//            query = query.Where(x => x.TrilhaNivel.ID == idTrilhaNivel);
//            query = query.Where(x => x.TrilhaTopicoTematico.ID == idTrilhaTopicoTematico);
//            query = query.Where(x => x.Objetivo.ID == idObjetivo);
//            query = query.Where(x => x.Pre == inPre);
//            return query.FirstOrDefault();
//        }

//        public IList<IndiceImportancia> ObterPorFiltro(IndiceImportancia indiceImportancia)
//        {
//            ValidarInstancia(indiceImportancia);
//            var query = repositorio.session.Query<IndiceImportancia>();
//            if (indiceImportancia.Usuario != null && indiceImportancia.Usuario.ID > 0)
//                query = query.Where(x => x.Usuario.ID == indiceImportancia.Usuario.ID);
            
//            if (indiceImportancia.TrilhaNivel != null && indiceImportancia.TrilhaNivel.ID > 0)
//                query = query.Where(x => x.TrilhaNivel.ID == indiceImportancia.TrilhaNivel.ID);

//            if (indiceImportancia.TrilhaTopicoTematico != null && indiceImportancia.TrilhaTopicoTematico.ID > 0)
//                query = query.Where(x => x.TrilhaTopicoTematico.ID == indiceImportancia.TrilhaTopicoTematico.ID);

//            if (indiceImportancia.Objetivo != null && indiceImportancia.Objetivo.ID > 0)
//                query = query.Where(x => x.Objetivo.ID == indiceImportancia.Objetivo.ID);
            
//            query = query.Where(x => x.Pre == indiceImportancia.Pre);

//            return query.ToList();
//        }

//        public void Excluir(IndiceImportancia pIndiceImportancia)
//        {

//            this.ValidarInstancia(pIndiceImportancia);

//            if (this.ValidarDependencias(pIndiceImportancia))
//                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Indice de Importância.");

//            repositorio.Excluir(pIndiceImportancia);
//        }

//        public void Dispose()
//        {
//            GC.SuppressFinalize(repositorio);
//            GC.Collect();
//        }

//        #endregion

//        #region "Métodos Protected"

//        //protected override bool ValidarDependencias(object pItemTrilha)
//        //{
//        //    IndiceImportancia indiceImportancia = (IndiceImportancia)pItemTrilha;
//        //    return (indiceImportancia.ListaItemTrilhaParticipacao != null);
//        //}

//        #endregion

//    }
//}
