using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcRelatorioUFPerfil : ProcBase<DtoRelatorioUFPerfil>
    {
        public IList<DtoRelatorioUFPerfil> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_REL_UF_PERFIL");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoRelatorioUFPerfil>();

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

        private DtoRelatorioUFPerfil ObterObjetoDTO(System.Data.IDataReader dr)
        {

            var row = new DtoRelatorioUFPerfil();

            row.NivelOcupacional = dr["NivelOcupacional"].ToString();
            row.UF = dr["UF"].ToString();
            row.Quantidade = int.Parse(dr["Concluintes"].ToString());

            return row;
        }
    }
}
