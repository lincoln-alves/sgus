using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using System.Data.SqlClient;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcCertificadosPorCargoETema : ProcBase<DtoCertificadosPorCargoETema>
    {
        public IList<DtoCertificadosPorCargoETema> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_CERTIFICADOS_POR_CARGO_E_TURMA");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoCertificadosPorCargoETema>();
          
                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    lstResult.Add(ObterObjetoDTO(dr));
                }

                return lstResult;

            }
            catch
            {
                throw new AcademicoException("Ocorreu um erro na execução da Solicitação");
            }
            finally
            {
                base.FecharConexao();
            }


        }

        private DtoCertificadosPorCargoETema ObterObjetoDTO(System.Data.IDataReader dr)
        {

            var row = new DtoCertificadosPorCargoETema();

            row.NivelOcupacional = dr["NivelOcupacional"].ToString();
            row.SolucaoEducacional = dr["SolucaoEducacional"].ToString();
            row.Total = int.Parse(dr["Total"].ToString());

            return row;
        }


    }
}
