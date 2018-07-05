using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcTotalizacoesCursosOferecidos : ProcBase<DtoCertificadosPorCargoETema>
    {
        public IList<DtoTotalizacoesCursosOferecidos> PegarDadosRelatorio()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_TOTALIZACOES_CURSOS_OFERECIDOS");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DtoTotalizacoesCursosOferecidos>();

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

        private DtoTotalizacoesCursosOferecidos ObterObjetoDTO(System.Data.IDataReader dr)
        {

            var row = new DtoTotalizacoesCursosOferecidos();

            row.Quantidade = int.Parse(dr["Quantidade"].ToString());
            row.Descricao = dr["Descricao"].ToString();
            
            return row;
        }




    }
}
