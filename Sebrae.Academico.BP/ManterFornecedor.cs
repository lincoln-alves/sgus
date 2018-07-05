using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterFornecedor : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMFornecedor bmFornecedor = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterFornecedor()
            : base()
        {
            bmFornecedor = new BMFornecedor();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<DTORelatorioFornecedorAcesso> ConsultarForneceorAcesso(string pNome) {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("P_Nome", pNome);

            return bmFornecedor.ExecutarProcedure<DTORelatorioFornecedorAcesso>("SP_REL_FORNECEDOR_ACESSO", lstParam);
        }

        public void IncluirFornecedor(Fornecedor pFornecedor)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pFornecedor);
                bmFornecedor.Salvar(pFornecedor);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public void IncluirUfs(Fornecedor fornecedor, List<int> ufs) {
            var ls = fornecedor.ListaFornecedorUF.Where(p => !ufs.Contains(p.UF.ID)).ToList();
            foreach(var item in ls)
            {
                fornecedor.RemoverUf(item);
            }

            foreach (var idUf in ufs)
            {
                if (fornecedor.ListaFornecedorUF.Any(f => f.UF.ID == idUf)) continue;
                var fornecedorUf = new FornecedorUF() { UF = new BMUf().ObterPorId(idUf), Fornecedor = fornecedor };
                fornecedor.AdicionarUf(fornecedorUf);
            }
        }

        public void AlterarFornecedor(Fornecedor pFornecedor)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pFornecedor);
                bmFornecedor.Salvar(pFornecedor);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public IList<Fornecedor> ObterTodosFornecedores()
        {
            IList<Fornecedor> listaFornecedores = null;

            try
            {
                listaFornecedores = bmFornecedor.ObterTodos();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaFornecedores;
        }

        public IQueryable<Fornecedor> ObterTodosIQueryable()
        {
            return bmFornecedor.ObterTodosIQueryable();
        }

        public IList<Fornecedor> ObterFornecedoresUf(Uf uf)
        {
            return bmFornecedor.ObterTodos().Where(f => f.ListaFornecedorUF.Any(u => u.UF.ID == uf.ID)).ToList();
        }


        public Fornecedor ObterFornecedorPorID(int pId)
        {
            Fornecedor fornecedor = null;

            try
            {
                fornecedor = bmFornecedor.ObterPorID(pId);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return fornecedor;
        }

        public void ExcluirFornecedor(int IdFornecedor)
        {
            try
            {
                Fornecedor fornecedor = null;

                if (IdFornecedor > 0)
                {
                    fornecedor = bmFornecedor.ObterPorID(IdFornecedor);
                }

                bmFornecedor.Excluir(fornecedor);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public IList<Fornecedor> ObterFornecedorPorFiltro(Fornecedor pFornecedor)
        {

            IList<Fornecedor> listaFornecedores = null;

            try
            {
                listaFornecedores = bmFornecedor.ObterPorFiltro(pFornecedor);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaFornecedores;
        }

        public List<DTOFornecedor> ObterTodosFornecedoresParaHistoricoAcademico()
        {
            try
            {
                var listaFornecedores = bmFornecedor.ObterTodosIQueryable().Where(p => p.ApresentarComoFornecedorNoPortal).ToList();

                return listaFornecedores
                    .Select(
                        p =>
                            new DTOFornecedor
                            {
                                ID = p.ID,
                                Nome = p.Nome,
                                NomeInstituicaoApresentacao = p.NomeApresentacao ?? ""
                            })
                    .ToList();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return null;
        }

        public Fornecedor ObterFornecedorSistema(enumFornecedor fornecedor)
        {
            try
            {
                return bmFornecedor.ObterFornecedorSistema(fornecedor);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
