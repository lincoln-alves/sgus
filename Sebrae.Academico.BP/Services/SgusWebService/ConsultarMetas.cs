using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services.ListaProgramas;
using Sebrae.Academico.BP.DTO.Services.MetasIndividuais;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarMetas : BusinessProcessServicesBase
    {
        private BMMetaInstitucional metaInstitucionalBM;


        public IList<DTOMetaInstitucional> ObterTodos()
        {
            IList<DTOMetaInstitucional> lstResult = null;

            try
            {
                metaInstitucionalBM = new BMMetaInstitucional();

                lstResult = new List<DTOMetaInstitucional>();

                foreach (MetaInstitucional mi in metaInstitucionalBM.ObterTodos())
                {
                    DTOMetaInstitucional midto = new DTOMetaInstitucional();
                    CommonHelper.SincronizarDominioParaDTO(mi, midto);
                    lstResult.Add(midto);
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return lstResult;

        }

        public IList<DTOListaMetaIndividualMeta> ConsultarMetasIndividuais(int pIdUsuario)
        {
            IList<MetaIndividual> lstMeta = null;
            IList<DTOListaMetaIndividualMeta> lstResult = null;

            try
            {
                lstMeta = new BMMetaIndividual().ObterPorFiltro(new MetaIndividual() { Usuario = new BMUsuario().ObterPorId(pIdUsuario) });

                lstResult = new List<DTOListaMetaIndividualMeta>();

                foreach (MetaIndividual mi in lstMeta)
                {
                    DTOListaMetaIndividualMeta miDTO = new DTOListaMetaIndividualMeta()
                    {
                        NomeMetaIndividual = mi.Nome,
                        ValidadeMetaIndividual = mi.DataValidade
                    };


                    foreach (ItemMetaIndividual imi in mi.ListaItensMetaIndividual)
                    {
                        DTOListaProgramaSolucaoEducacional solDTO = new DTOListaProgramaSolucaoEducacional()
                        {
                            CodigoSolucaoEducacional = imi.SolucaoEducacional.ID.ToString(),
                            NomeSolucaoEducacional = imi.SolucaoEducacional.Nome
                        };

                        foreach (var tg in imi.SolucaoEducacional.ListaTags)
                        {
                            DTOListaProgramaSolucaoEducacionalTags tgDTO = new DTOListaProgramaSolucaoEducacionalTags()
                            {
                                Codigo = tg.Tag.ID,
                                Nome = tg.Tag.Nome
                            };

                            solDTO.ListaTags.Add(tgDTO);

                        }

                        IList<MatriculaOferta> lstMo = imi.SolucaoEducacional.ListaOferta.Select(x => x.ListaMatriculaOferta).FirstOrDefault();


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

                            solDTO.ListaSolucaoEducacionalMatricula.Add(mtDTO);

                        }

                        miDTO.ListaItemMetaIndividual.Add(new DTOListaMetaIndividualItemMeta() { SolucaoEducacional = solDTO });
                    }

                    lstResult.Add(miDTO);

                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return lstResult;
        }
    }
}
