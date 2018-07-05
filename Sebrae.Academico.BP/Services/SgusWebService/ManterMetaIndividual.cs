using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.ExternosWebService
{
    public class ManterMetaIndividual: BusinessProcessServicesBase
    {
        public void IncluirMetaIndividual(int ID_usuario, string ID_ChaveExterna, string Descricao, string Nome, DateTime DataValidade, string[] ListaSolucaoEducacional = null, string FornecedorAlteracao = null)
        {
            try
            {

                this.RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = FornecedorAlteracao }).FirstOrDefault(),
                                              "IncluirMetaIndividual");

                using (BMMetaIndividual metaIndividualBM = new BMMetaIndividual())
                {

                    MetaIndividual mi = new MetaIndividual()
                    {
                        DataValidade = DataValidade,
                        Nome = Nome,
                        IDChaveExterna = ID_ChaveExterna,
                        Auditoria = new Auditoria(FornecedorAlteracao),
                        Usuario = new BMUsuario().ObterPorId(ID_usuario),
                        ListaItensMetaIndividual = new List<ItemMetaIndividual>()
                    };

                    Fornecedor f = new BMFornecedor().ObterPorFiltro(new Fornecedor() { Login = FornecedorAlteracao }).FirstOrDefault();

                    foreach (string str in ListaSolucaoEducacional)
                    {

                        SolucaoEducacional se = new BMSolucaoEducacional().ObterPorFiltro(new SolucaoEducacional() { Nome = str, Fornecedor = f }).FirstOrDefault();

                        if (se != null)
                        {
                            ItemMetaIndividual it = new ItemMetaIndividual()
                            {
                                Auditoria = new Auditoria(FornecedorAlteracao),
                                MetaIndividual = mi,
                                SolucaoEducacional = se,
                            };


                            mi.ListaItensMetaIndividual.Add(it);
                        }
                    }

                    metaIndividualBM.Salvar(mi);
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
          
        }
    }
}
