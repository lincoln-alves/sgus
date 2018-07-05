using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcConcluintesPorPerfilETipo : ProcBase<DtoConcluintesPorPerfilETipo>
    {
        public IList<DtoConcluintesPorPerfilETipo> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_CONCLUINTES_POR_PERFIL_E_TIPO");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoConcluintesPorPerfilETipo>();

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

        private DtoConcluintesPorPerfilETipo ObterObjetoDTO(System.Data.IDataReader dr)
        {

            var row = new DtoConcluintesPorPerfilETipo();

            row.NivelOcupacional = dr["NivelOcupacional"].ToString();
            row.Concluintes = int.Parse(dr["Concluintes"].ToString());

            return row;
        }
    }
}
