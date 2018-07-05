﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{

    
    public sealed class ProcTotalMatriculasNoAnoPorFormaAquisicao : ProcBase<DTOMatriculaOfertaNoAno>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculaOfertaNoAno.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOMatriculaOfertaNoAno> ObterTotalMatriculasNoAnoPorFormaAquisicao(int? ano, int? idUf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_TOTAL_MATRICULAS_NO_ANO_POR_FORMA_AQUISICAO");

                //Passa o id do usuário para o parâmetro da procedure                
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));
                if (idUf > 0) { this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UF", idUf)); }
                 
                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOMatriculaOfertaNoAno> lstResult = new List<DTOMatriculaOfertaNoAno>();
                DTOMatriculaOfertaNoAno matriculaOfertaNoAno = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    matriculaOfertaNoAno = ObterObjetoDTO(dr);
                    lstResult.Add(matriculaOfertaNoAno);
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

        #endregion

        private DTOMatriculaOfertaNoAno ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOMatriculaOfertaNoAno matriculaOfertaNoAno = new DTOMatriculaOfertaNoAno();

            matriculaOfertaNoAno.NomeFormaAquisicao = dr["NM_FormaAquisicao"].ToString();
            matriculaOfertaNoAno.QuantidadeMatriculados = int.Parse(dr["QTD"].ToString());

            return matriculaOfertaNoAno;
        }


    }
}
