using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMHistoricoExtraCurricular : BusinessManagerBase
    {

        #region "Atributos Privados"

        private RepositorioBase<HistoricoExtraCurricular> repositorio;

        #endregion

        #region "Construtor"
        
        public BMHistoricoExtraCurricular()
        {
            repositorio = new RepositorioBase<HistoricoExtraCurricular>();
        }

        #endregion
        
        #region "Métodos Privados"

        private void ValidarHistoricoExtraCurricularEnviado(HistoricoExtraCurricular pHistoricoExtraCurricular)
        {
            ValidarInstancia(pHistoricoExtraCurricular);


            if (pHistoricoExtraCurricular.Usuario == null)
                throw new Exception("Usuário não Informado. Campo Obrigatório!");

            if (pHistoricoExtraCurricular.SolucaoEducacionalExtraCurricular == null)
                throw new Exception("Solução Educacional Extra Curricular não Informada. Campo Obrigatório!");

            this.VerificarExistenciaDeHistoricoEscolar(pHistoricoExtraCurricular);

        }

        private void VerificarExistenciaDeHistoricoEscolar(HistoricoExtraCurricular pHistoricoExtraCurricular)
        {
            HistoricoExtraCurricular historicoExtraCurricular = this.ObterHistoricoExtraCurricularPorChave(pHistoricoExtraCurricular);

            if (historicoExtraCurricular != null)
            {
                if (pHistoricoExtraCurricular.ID != 0 && pHistoricoExtraCurricular.ID != historicoExtraCurricular.ID)
                {
                    throw new AcademicoException("Já existe um histórico extracurricular cadastrado");
                }
            }
        }

        #endregion

        #region "Métodos Públicos"

        public void Excluir(HistoricoExtraCurricular obj)
        {
            repositorio.Excluir(obj);
        }

        public void Salvar(HistoricoExtraCurricular pHistoricoExtraCurricular)
        {
            ValidarHistoricoExtraCurricularEnviado(pHistoricoExtraCurricular);
            repositorio.Salvar(pHistoricoExtraCurricular);
        }

        public HistoricoExtraCurricular ObterHistoricoExtraCurricularPorChave(HistoricoExtraCurricular pHistoricoExtraCurricular)
        {
            var query = repositorio.session.Query<HistoricoExtraCurricular>();

            /* Obtém um registro de Históricio extracurricular através dos atributos: 
               ID_Usuario, NM_SolucaoEducacionalExtraCurricular e DT_InicioAtividade 
               (Existe uma Unique key para estas tres colunas na tabela TB_HistoricoExtraCurricular)  */

            HistoricoExtraCurricular historicoExtraCurricular = query.FirstOrDefault(x => pHistoricoExtraCurricular.Usuario != null && x.Usuario.ID == pHistoricoExtraCurricular.Usuario.ID &&
                                                                  x.SolucaoEducacionalExtraCurricular != null && x.SolucaoEducacionalExtraCurricular != string.Empty &&
                                                                  pHistoricoExtraCurricular.SolucaoEducacionalExtraCurricular != null && pHistoricoExtraCurricular.SolucaoEducacionalExtraCurricular != string.Empty &&
                                                                  x.SolucaoEducacionalExtraCurricular.Trim().ToLower() == pHistoricoExtraCurricular.SolucaoEducacionalExtraCurricular.Trim() &&
                                                                  x.DataInicioAtividade.HasValue && pHistoricoExtraCurricular.DataInicioAtividade.HasValue &&
                                                                  x.DataInicioAtividade.Value == pHistoricoExtraCurricular.DataInicioAtividade.Value);

            return historicoExtraCurricular;
        }

        /// <summary>
        /// Obtém as informações de Histórico Curricular de um usuário.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário</param>
        /// <returns>Lista com informações de Histórico Curricular de um usuário.</returns>
        public IList<HistoricoExtraCurricular> ObterPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<HistoricoExtraCurricular>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            return query.ToList<HistoricoExtraCurricular>();
        }

        public HistoricoExtraCurricular ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        #endregion

        

    }
}
