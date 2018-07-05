using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterObjetivo : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMObjetivo bmObjetivo;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterObjetivo()
            : base()
        {
            bmObjetivo = new BMObjetivo();
        }


        #endregion

        #region "Métodos Públicos"

        public void IncluirObjetivo(Objetivo pObjetivo)
        {
            base.PreencherInformacoesDeAuditoria(pObjetivo);
            bmObjetivo.Salvar(pObjetivo);
        }

        public void AlterarObjetivo(Objetivo pObjetivo)
        {
            base.PreencherInformacoesDeAuditoria(pObjetivo);
            bmObjetivo.Salvar(pObjetivo);
        }

        public IList<Objetivo> ObterTodosObjetivos()
        {
            return bmObjetivo.ObterTodos();
        }

        public Objetivo ObterObjetivoPorID(int pId)
        {
            return bmObjetivo.ObterPorID(pId);
        }

        public void ExcluirObjetivo(int idObjetivo)
        {
            try
            {
                Objetivo objetivo = null;

                if (idObjetivo > 0)
                {
                    objetivo = bmObjetivo.ObterPorID(idObjetivo);
                }

                bmObjetivo.Excluir(objetivo);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<Objetivo> ObterObjetivoPorFiltro(Objetivo pObjetivo)
        {
            return bmObjetivo.ObterPorFiltro(pObjetivo);
        }

        public Objetivo ObterPorTexto(string textoObjetivo)
        {
            if (string.IsNullOrWhiteSpace(textoObjetivo))
            {
                throw new AcademicoException("Informe o texto do Objetivo");
            }

            return bmObjetivo.ObterPorTexto(textoObjetivo);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public bool ChaveExternaUnica(string chaveExterna, int currentId)
        {
            var objetivo = bmObjetivo.ObterPorChaveExterna(chaveExterna, currentId);
            if(objetivo == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public IQueryable<Objetivo> ObterTodosIQueryable()
        {
            return bmObjetivo.ObterTodosIQueryable();
        }
    }
}
