using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMHistoricoSGTC : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<HistoricoSGTC> repositorio;

        #endregion

        #region "Construtor"

        public BMHistoricoSGTC()
        {
            repositorio = new RepositorioBase<HistoricoSGTC>();
        }

        #endregion
        
        #region "Métodos Privados"

        private void ValidarHistoricoSGTC(HistoricoSGTC pHistoricoSGTC)
        {
            ValidarInstancia(pHistoricoSGTC);

            //Usuário
            if (pHistoricoSGTC.Usuario != null || (pHistoricoSGTC.Usuario.ID <= 0)) throw new AcademicoException("Usuário. Campo Obrigatório");

            //Solução Educacional
            if (string.IsNullOrWhiteSpace(pHistoricoSGTC.NomeSolucaoEducacional)) throw new AcademicoException("Nome da Solução Educacional. Campo Obrigatório");

            //Chave Externa
            if (pHistoricoSGTC.IDChaveExterna <= 0) throw new AcademicoException("ID da Chave Externa. Campo Obrigatório");

            //Data de Conclusão
            if (pHistoricoSGTC.DataConclusao.Equals(DateTime.MinValue)) throw new AcademicoException("Data de Conclusão. Campo Obrigatório");

            this.VerificarExistenciaDeHistoricoSGTC(pHistoricoSGTC);
        }

        private void VerificarExistenciaDeHistoricoSGTC(HistoricoSGTC pHistoricoSGTC)
        {
            var historicoSGTC = ObterHistoricoSGTC(pHistoricoSGTC);

            if (historicoSGTC != null)
            {
                if (pHistoricoSGTC.ID != historicoSGTC.ID)
                {
                    throw new AcademicoException("Já existe um histórico extracurricular cadastrado");
                }
            }
        }

        #endregion

        #region "Métodos Públicos"

        public HistoricoSGTC ObterHistoricoSGTC(HistoricoSGTC pHistoricoSGTC)
        {
            var query = repositorio.session.Query<HistoricoSGTC>();

            /* Obtém um registro de Histórico SGTC através dos atributos: 
               ID_Usuario, IdChaveExterna
               (Existe uma Unique key para estas duas colunas na tabela TB_HistoricoSGTC)  */
            HistoricoSGTC historicoExtraCurricular = query.FirstOrDefault(x => pHistoricoSGTC.Usuario != null && x.Usuario.ID == pHistoricoSGTC.Usuario.ID &&
                                                                               pHistoricoSGTC.IDChaveExterna == pHistoricoSGTC.IDChaveExterna);

            return historicoExtraCurricular;
        }

        public IList<HistoricoSGTC> ObterPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<HistoricoSGTC>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            return query.ToList<HistoricoSGTC>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(HistoricoSGTC pHistoricoSGTC)
        {
            ValidarHistoricoSGTC(pHistoricoSGTC);
            repositorio.Salvar(pHistoricoSGTC);
        }

        #endregion
    }
}
