using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterHistoricoExtraCurricular : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMHistoricoExtraCurricular bmHistoricoExtraCurricular = null;

        #endregion

        #region "Construtor"

        public ManterHistoricoExtraCurricular()
            : base()
        {
            bmHistoricoExtraCurricular = new BMHistoricoExtraCurricular();
        }

        #endregion
        
        #region "Métodos Públicos"

        public HistoricoExtraCurricular ObterPorID(int pId)
        {
            return bmHistoricoExtraCurricular.ObterPorID(pId);
        }

        /// <summary>
        /// Obtém as informações de Histórico Curricular de um usuário.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário</param>
        /// <returns>Lista com informações de Histórico Curricular de um usuário.</returns>
        public IList<HistoricoExtraCurricular> ObterPorUsuario(int idUsuario)
        {
            return bmHistoricoExtraCurricular.ObterPorUsuario(idUsuario);
        }

        #endregion

        public IList<DTORelatorioAtividadeExtraCurricular> ConsultarRelatorioAtividadeExtraCurricular(DateTime? dataTerIni, DateTime? dataTerFim, DateTime? dataCadIni, DateTime? dataCadFim, int cargaHoraria)
        {
            var lstParam = new Dictionary<string, object>{
                { "data_ter_ini", dataTerIni},
                { "data_ter_fim", dataTerFim},
                { "data_cad_ini", dataCadIni },
                { "data_cad_fim", dataCadFim },
                { "carga_horaria", cargaHoraria }
            };

            return bmHistoricoExtraCurricular.ExecutarProcedure<DTORelatorioAtividadeExtraCurricular>("SP_REL_ATIVIDADE_EXTRA_CURRICULAR", lstParam);
        }
    }
}