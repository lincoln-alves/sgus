using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioParticipacaoTrilha : BusinessProcessBaseRelatorio
    {

        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { return enumRelatorio.SolucaoEducacionalOferta; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList ExecutarRelatorioParticipacaoTrilha(int pIdTrilha, int pIdTrilhaNivel) 
        {
            IDictionary<string,object> lstParametro = new Dictionary<string,object>();
            lstParametro.Add("IdTrilha",pIdTrilha);
            lstParametro.Add("IdTrilhaNivel",pIdTrilhaNivel);
            //TODO: [IMPORANTE] -> 27/09/2013 - Refatorar 
            //IList lstRetorno =  repositorioRelatio.ExecuteProcedure("sp_Participacao_Trilha", lstParametro);


            //IList lstRetornoObj = new List<object>();

            IList<object> lstRetorno = null;
            IList lstRetornoObj = new List<object>();

            foreach (object[] obj in lstRetorno)
            {

                //objresult.Nome = obj[0];
                //objresult.CPF = obj[1];
                //objresult.Nivel_Ocupacional = obj[2];
                //objresult.NM_UF = obj[3];
                //objresult.Email = obj[4];
                //objresult.qtd_topicos = obj[5];
                //objresult.qtd_topicos_executados = obj[6];
                //objresult.DT_Limite = obj[7];

                lstRetornoObj.Add(new {

                    Nome = obj[0],
                    CPF = obj[1],
                    Nivel_Ocupacional = obj[2],
                    NM_UF = obj[3],
                    Email = obj[4],
                    Trilha = obj[5],
                    Trilha_Nivel = obj[6],
                    qtd_topicos = obj[7],
                    qtd_topicos_executados = obj[8],
                    DT_Limite = obj[9]
                });
            }



            return lstRetornoObj;

        }

        public bool VerificarQuantidadeDeObjetivosConcluidos(int idTrilhaNivel, int idTrilhaTopicoTematico, int idUsuarioTrilha)
        {
            this.RegistrarLogExecucao();

            var result = (new ManterTrilha()).ConsultarParticipacaotrilhas(idTrilhaNivel, idTrilhaTopicoTematico, idUsuarioTrilha);
            if (result.Count <= 0) return false;
            var qtdObjetivos = result[0].QuantidadeObjetivosUnicos;
            var qtdObjetivosRealizados = result[0].QuantidadeObjetivosRealizados;
            
            return qtdObjetivosRealizados >= qtdObjetivos;
        }

    }
    public class DTORelatorioParticipacaoTrilha
    {
        public int ID { get; set; }
        public int QuantidadeObjetivosRealizados { get; set; }
        public int QuantidadeObjetivosUnicos { get; set; }

    }
}
