using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcParticipantesPorUnidade : ProcBase<DtoParticipantesPorUnidade>
    {
        public IList<DtoParticipantesPorUnidade> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_PARTICIPANTES_POR_UNIDADE_NACIONAL");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoParticipantesPorUnidade>();

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

        private DtoParticipantesPorUnidade ObterObjetoDTO(System.Data.IDataReader dr)
        {

            var row = new DtoParticipantesPorUnidade();

            row.Unidade = dr["Unidade"].ToString();
            row.Participantes = int.Parse(dr["Participantes"].ToString());

            return row;
        }
    }
}
