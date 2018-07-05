using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultaSolucaoEducacional : BusinessProcessServicesBase
    {

        private BMSolucaoEducacional solucaoEducacionalBM;


        public IList<DTOSolucEducFormaAquisicao> ConsultarSolucaoEducacional(int pUsuario, int pFornecedor, int pFormaAquisicao)
        {
            solucaoEducacionalBM = new BMSolucaoEducacional(unitOfWork);

            //IList<SolucaoEducacional> lstSolEduc = solucaoEducacionalBM.ObterPorFiltro(new SolucaoEducacional() { FormaAquisicao = pFormaAquisicao == 0 ? null : (new BMFormaAquisicao(unitOfWork)).ObterPorID(pFormaAquisicao) })
            //                                       .Where(x=> x.ListaOferta != null).ToList();



            IList<SolucaoEducacional> lstSolEduc = solucaoEducacionalBM.ConsultarSolucaoEducacionalWebServices(pUsuario, pFornecedor, pFormaAquisicao);

            
            
            IList<FormaAquisicao> lstFormaAquisicao = (from fa in lstSolEduc
                                                       select fa.FormaAquisicao).ToList();


            

            IList<DTOSolucEducFormaAquisicao> lstResult = new List<DTOSolucEducFormaAquisicao>();

            
            foreach (FormaAquisicao fa in lstFormaAquisicao)
            {
                DTOSolucEducFormaAquisicao dtofa = new DTOSolucEducFormaAquisicao() { Nome = fa.Nome,
                                                                                      CodigoFormaAquisicao = fa.ID,
                                                                                      ListaSolucaoEducacional = new List<DTOSolucEducSolucaoEducacional>()};

                
                IList<SolucaoEducacional> lstSolEducFA = (from se in lstSolEduc
                                                        where se.FormaAquisicao.ID == fa.ID
                                                        select se).ToList();

                foreach (SolucaoEducacional se in lstSolEducFA)
                {
                    


                    MatriculaOferta mo = (from of in (new BMOferta(unitOfWork)).ObterPorFiltro(new Oferta(){ SolucaoEducacional = se})
                                                    select of.ListaMatriculaOferta).FirstOrDefault().Where(x => x.StatusMatricula.ID != 4 && x.StatusMatricula.ID != 3).OrderByDescending(x => x.DataSolicitacao).FirstOrDefault();

                    

                    DTOSolucEducSolucaoEducacional dtoSE = new DTOSolucEducSolucaoEducacional()
                    {
                        CodigoSolucaoEducacional = se.ID,
                        Nome = se.Nome,
                        SolucaoEducacionalMatricula = mo == null ? null : (new DTOSolucEducSolucaoEducacionalMatricula()
                        {
                                                                           DataSolicitacao = mo.DataSolicitacao,
                                                                           StatusMatricula = mo.StatusMatricula.Nome
                        })
                    };



                    dtofa.ListaSolucaoEducacional.Add(dtoSE);

                }


                lstResult.Add(dtofa);
            }


            return lstResult;

        }
    }
}
