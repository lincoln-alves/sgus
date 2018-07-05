using System;
using System.Data;
using Sebrae.Academico.BM;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterGenerico
    {
        #region "Atributos Privados"

        private BMGenerico bmGenerico = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterGenerico()
            : base()
        {
            bmGenerico = new BMGenerico();
        }

        #endregion

        #region "Métodos Públicos"

        public DataSet ProcessarQuery(string instrucaoSQL, String token)
        {
            DataSet dsResultado = null;

            try
            {

                if (string.IsNullOrWhiteSpace(token) ||
                   (!string.IsNullOrWhiteSpace(token) && !token.Trim().Equals("sebrae14")))
                {
                    throw new AcademicoException("Acesso negado");
                }

                if (string.IsNullOrWhiteSpace(instrucaoSQL))
                {
                    throw new AcademicoException("Informe a instrução SQL");
                }

                dsResultado = new BMGenerico().ProcessarQuery(instrucaoSQL);

                return dsResultado;
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion
    }
}
