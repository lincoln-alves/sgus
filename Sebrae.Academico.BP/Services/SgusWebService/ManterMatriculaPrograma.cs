using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services.ListaProgramas;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterMatriculaPrograma: BusinessProcessServicesBase
    {
        private BMPrograma programaBM;
        private BMMatriculaPrograma matriculaProgramaBM;
        

        public List<DTOPrograma> ListarProgramas()
        {
            programaBM = new BMPrograma();

            IList<DTOPrograma> lstResult = new List<DTOPrograma>();

            foreach (Programa pr in programaBM.ObterTodos().ToList()) 
            {
                DTOPrograma prdto = new DTOPrograma();
                CommonHelper.SincronizarDominioParaDTO(pr, prdto);
                lstResult.Add(prdto);
            }

            return lstResult.ToList();
        }

        public void RegistrarMatricula(int idPrograma, int idUsuario, AuthenticationRequest autenticacao)
        {
            matriculaProgramaBM = new BMMatriculaPrograma();
            Usuario usuario = (new BMUsuario()).ObterPorId(idUsuario);

            MatriculaPrograma matriculaPrograma = new MatriculaPrograma() 
            {
                Programa = (new BMPrograma()).ObterPorId(idPrograma),
                Usuario = usuario,
                NivelOcupacional = (new BMNivelOcupacional()).ObterPorID(usuario.NivelOcupacional.ID),
                UF = (new BMUf()).ObterPorId(usuario.UF.ID),
                Auditoria = new Auditoria(autenticacao.Login),
                StatusMatricula = enumStatusMatricula.Inscrito,
                ID = 0,
                DataInicio = DateTime.Now
            };

            matriculaProgramaBM.RegistrarMatricula(matriculaPrograma);
        }





        public List<DTOMatriculaPrograma> ConsultaStatusMatricula(int idPrograma, int idUsuario)
        {

            MatriculaPrograma matriculaFiltro = new MatriculaPrograma()
            {
                Programa = (idPrograma == 0 ? null : new Programa() { ID = idPrograma }),
                Usuario = (idUsuario == 0 ? null : new Usuario() { ID = idUsuario })
            };

            IList<DTOMatriculaPrograma> lstResult = new List<DTOMatriculaPrograma>();

             matriculaProgramaBM = new BMMatriculaPrograma();

             foreach (MatriculaPrograma mp in matriculaProgramaBM.ObterPorFiltros(matriculaFiltro))
             {
                 DTOMatriculaPrograma mpdto = new DTOMatriculaPrograma();
                 CommonHelper.SincronizarDominioParaDTO(mp, mpdto);
                 lstResult.Add(mpdto);
             }

             return lstResult.ToList();
        }

        public List<DTOListaProgramaPrograma> ListarProgramasDisponiveis(int pIdUsuario)
        {
            programaBM = new BMPrograma();

            IList<DTOListaProgramaPrograma> lstResult = new List<DTOListaProgramaPrograma>();

            IList<ProgramaSolucaoEducacional> lstP;

            if (pIdUsuario > 0) 
            {
                lstP = programaBM.ObterPorUsuario(pIdUsuario);
            }
            else 
            {
                lstP = programaBM.ObterPrograSolucaoEducacional();
            }

           
            foreach (var pr in lstP)
            {
                DTOListaProgramaPrograma pDTO = new DTOListaProgramaPrograma()
                {
                    CodigoPrograma = pr.Programa.ID.ToString(),
                    NomePrograma = pr.Programa.Nome
                };


                IList<MatriculaPrograma> lstMtp = pIdUsuario == 0 ? pr.Programa.ListaMatriculaPrograma : pr.Programa.ListaMatriculaPrograma.Where(x => x.Usuario.ID == pIdUsuario).ToList();

                foreach (MatriculaPrograma p in lstMtp)
                {
                    DTOListaProgramaMatriculaPrograma mpDTO = new DTOListaProgramaMatriculaPrograma()
                    {
                        StatusMatricula = p.StatusMatricula.ToString()
                    };

                    pDTO.ListaMatriculaPrograma.Add(mpDTO);
                }

                DTOListaProgramaSolucaoEducacional seDTO = new DTOListaProgramaSolucaoEducacional()
                {
                    CodigoSolucaoEducacional = pr.SolucaoEducacional.ID.ToString(),
                    NomeSolucaoEducacional = pr.SolucaoEducacional.Nome,
                };

                if (pIdUsuario > 0)
                {

                    IList<MatriculaOferta> lstMo = pr.SolucaoEducacional.ListaOferta.Select(x => x.ListaMatriculaOferta).FirstOrDefault();


                    lstMo = lstMo.Where(x => x.Usuario.ID.Equals(pIdUsuario)).ToList();

                    foreach (MatriculaOferta mo in lstMo)
                    {
                        MatriculaTurma mt = mo.Oferta.ListaTurma.Where(x => x.Oferta.ID == mo.Oferta.ID)
                                                      .Select(x => x.ListaMatriculas).FirstOrDefault()
                                                      .Where(x => x.MatriculaOferta.Usuario.ID == mo.Usuario.ID &&
                                                      !(x.MatriculaOferta.StatusMatricula.Equals(enumStatusMatricula.CanceladoAdm) &&
                                                      x.MatriculaOferta.StatusMatricula.Equals(enumStatusMatricula.CanceladoAluno)))
                                                      .OrderByDescending(x => x.DataMatricula)
                                                      .FirstOrDefault();

                        DTOListaProgramaSolucaoEducacionalMatricula mtDTO = new DTOListaProgramaSolucaoEducacionalMatricula();

                        if (mt != null)
                        {

                                mtDTO.DataSolicitacao = mo.DataSolicitacao;
                                mtDTO.StatusMatricula = mt.MatriculaOferta.StatusMatricula.ToString();
                          
                        }

                        else 
                        {
                            mtDTO.DataSolicitacao = mo.DataSolicitacao;
                            mtDTO.StatusMatricula = mo.StatusMatricula.ToString();
                        }

                        seDTO.ListaSolucaoEducacionalMatricula.Add(mtDTO);
                        
                    }

                }

                foreach (SolucaoEducacionalTags tg in pr.SolucaoEducacional.ListaTags)
                {
                    DTOListaProgramaSolucaoEducacionalTags tgDTO = new DTOListaProgramaSolucaoEducacionalTags()
                    {
                        Codigo = tg.Tag.ID,
                        Nome = tg.Tag.Nome
                    };

                    seDTO.ListaTags.Add(tgDTO);
                }

                pDTO.ListaSolucaoEducacional.Add(seDTO);
                
                lstResult.Add(pDTO);
            }


            return lstResult.ToList();
        }
    }
}
