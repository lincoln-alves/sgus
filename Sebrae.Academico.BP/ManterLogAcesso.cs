using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterLogAcesso : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMLogAcessosPaginas bmLogAcesso = null;

        #endregion

        #region "Construtor"

        public ManterLogAcesso()
            : base()
        {
            bmLogAcesso = new BMLogAcessosPaginas();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<LogAcesso> ObterUltimosAcessoDosUsuario(int idUsuario)
        {
            try
            {
                return bmLogAcesso.ObterUltimosAcessosDosUsuario(idUsuario,20);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IList<DTORelatorioAcesso> ConsultarRelatorioAcesso(int? pIdUsuario, int? pIdUf, int? pIdNivelOcupacional, int? pIdPerfil, DateTime? pDataInicial, DateTime? pDataFinal)
        {

            var lstParam = new Dictionary<string, object>{
                {"p_Usuario", pIdUsuario},
                { "p_Perfil", pIdPerfil },
                { "p_UF", pIdUf },
                { "p_Nivel_Ocupacional", pIdNivelOcupacional },
                { "p_Data_Inicial", pDataInicial },
                { "p_Data_Final", pDataFinal }
            };

            return bmLogAcesso.ExecutarProcedure<DTORelatorioAcesso>("SP_REL_ACESSOS", lstParam);
        }

        #endregion
    }
}

