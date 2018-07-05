using System;
using System.Collections;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatatorioIndividual
    {
        public RelatatorioIndividual()
        {

        }

        public System.Collections.IList ExecutarRelatoriosIndividuais(int pId_Usuario)
        {
            IDictionary<string, object> lstParametro = new Dictionary<string, object>();
            lstParametro.Add("pID_Usuario", pId_Usuario);

            //TODO: [IMPORANTE] -> 27/09/2013 - Refatorar 
            IList lstRetorno = new List<object>();
            //IList lstRetorno = repositorioRelatio.ExecuteProcedure("sp_relatorio_individual", lstParametro);
            
            IList lstRetornoObj = new List<object>();

            foreach (object[] obj in lstRetorno)
            {
                DateTime data = DateTime.Now;

                lstRetornoObj.Add(new { 
                
                NM_Trilha = obj[0],
                NM_StatusMatricula = obj[1],
                DT_Inicio = ( string.IsNullOrWhiteSpace(obj[2].ToString())? "" : DateTime.Parse(obj[2].ToString()).ToShortDateString()),
                NM_TrilhaNivel = obj[3],
                NM_TrilhaTopicoTematico = obj[4],
                IN_AtividadeFormativa = obj[5],
                QT_Ponto = obj[6]
                
                
                });

            }


            return lstRetornoObj;
        }
    }
}
