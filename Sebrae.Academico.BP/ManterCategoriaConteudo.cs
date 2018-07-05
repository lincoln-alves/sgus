using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Text;
using Sebrae.Academico.BP.wsDrupalServices;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterCategoriaConteudo : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMCategoriaConteudo bmCategoriaConteudo = null;

        #endregion

        #region "Construtor"

        public ManterCategoriaConteudo()
            : base()
        {
            bmCategoriaConteudo = new BMCategoriaConteudo();
        }

        #endregion

        #region "Métodos Públicos"

        public void AtualizarSiglaSubCategorias(CategoriaConteudo pCategoriaConteudo) {
            if (pCategoriaConteudo.CategoriaConteudoPai != null) return;
            if (!pCategoriaConteudo.ListaCategoriaConteudoFilhos.Any()) return;
            foreach (var item in pCategoriaConteudo.ListaCategoriaConteudoFilhos) {
                item.Sigla = pCategoriaConteudo.Sigla;

                AlterarCategoriaConteudo(item);
            }
        }

        public void IncluirCategoriaConteudo(CategoriaConteudo pCategoriaConteudo)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pCategoriaConteudo);
                bmCategoriaConteudo.Incluir(pCategoriaConteudo);

                AtualizarSiglaSubCategorias(pCategoriaConteudo);
            }
            catch (AlertException ex)
            {
                throw ex;
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

        public void AlterarCategoriaConteudo(CategoriaConteudo pCategoriaConteudo)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pCategoriaConteudo);
                bmCategoriaConteudo.Alterar(pCategoriaConteudo);
                
                AtualizarSiglaSubCategorias(pCategoriaConteudo);
            }
            catch (AlertException ex)
            {
                throw ex;
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

        public CategoriaConteudo ObterCategoriaConteudoPorID(int pId)
        {
            CategoriaConteudo CategoriaConteudo = null;

            try
            {
                CategoriaConteudo = bmCategoriaConteudo.ObterPorID(pId);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return CategoriaConteudo;
        }

        public IQueryable<CategoriaConteudo> ObterTodasCategoriasConteudo()
        {
            try
            {
                return bmCategoriaConteudo.ObterTodos();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
                return null;
            }
        }

        public IList<CategoriaConteudo> ObterTodasCategoriasConteudoPorUF(Uf uf)
        {
            IList<CategoriaConteudo> listaCategoriaConteudo = null;

            try
            {
                listaCategoriaConteudo = bmCategoriaConteudo.ObterTodosPorUf(uf);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaCategoriaConteudo;
        }

        public IList<CategoriaConteudo> ObterTodasCategoriasPais(int idCategoriaFilha)
        {
            var retorno = new List<CategoriaConteudo>();

            var categoriaFilha = ObterCategoriaConteudoPorID(idCategoriaFilha);

            retorno.Add(categoriaFilha);

            if (categoriaFilha.CategoriaConteudoPai != null)
            {
                retorno.AddRange(ObterTodasCategoriasPais(categoriaFilha.CategoriaConteudoPai.ID));
            }

            return retorno;
        }

        public void ExcluirCategoriaConteudo(int IdCategoriaConteudo)
        {

            try
            {
                CategoriaConteudo CategoriaConteudo = null;

                if (IdCategoriaConteudo > 0)
                {
                    CategoriaConteudo = bmCategoriaConteudo.ObterPorID(IdCategoriaConteudo);
                }

                bmCategoriaConteudo.Excluir(CategoriaConteudo);
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

        public IList<CategoriaConteudo> ObterCategoriaConteudoPorFiltro(CategoriaConteudo pCategoriaConteudo)
        {
            IList<CategoriaConteudo> listaCategoriaConteudo = null;

            try
            {
                listaCategoriaConteudo = bmCategoriaConteudo.ObterPorFiltro(pCategoriaConteudo);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaCategoriaConteudo;
        }

        public bool IsFormacaoDeFormadores(CategoriaConteudo categoria)
        {
            int id;

            if (int.TryParse(new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.IdFormacaoDeFormadores).Registro, out id))
            {
                return ObterCategoriaConteudoPorID(id).ObterFilhos().Select(x => x.ID).Contains(categoria.ID);
            }

            throw new AcademicoException("ID da Formação de Formadores não informada nas configurações do sistema");
        }

        #endregion

        #region "Métodos Pertinentes a Integração com o Drupal"

        public void CriaNodeDrupal(CategoriaConteudo registro, string url)
        {
            try
            {
                if (registro.IdNode > 0)
                {
                    return;
                }

                string sbTodasTaxonomias = this.PrepararEntradaDeDadosParaOServicoDrupal(registro);

                bool ativo = true;

                int idNodePai = 0;
                if (registro.CategoriaConteudoPai != null && registro.CategoriaConteudoPai.ID > 0 && !registro.CategoriaConteudoPai.IdNode.HasValue)
                    throw new AlertException("Dados salvaos no banco de dados. Não foi possível enviar os dados para o Portal. Favor publicar a categoria pai primeiro");

                if (registro.CategoriaConteudoPai != null && registro.CategoriaConteudoPai.IdNode.HasValue)
                    idNodePai = registro.CategoriaConteudoPai.IdNode.Value;

                string linkParaImagem = string.Empty;

                //if (registro.Imagem != null && registro..ID > 0)
                //{
                //    ConfiguracaoSistema caminhoParaDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);
                //    linkParaImagem = CommonHelper.ObterLinkParaArquivoDeImagem(caminhoParaDiretorioDeUpload.Registro, registro.Imagem.ID);
                //}
                var postParameters = new Dictionary<string, string>{
                    { "login", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioSOAPPortal).Registro },
                    { "senha", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaSOAPPortal).Registro },
                    {"id_solucao_educacional", string.Empty},
                    {"titulo", registro.Nome},
                    {"corpo", registro.Apresentacao},
                    {"lista_taxs", sbTodasTaxonomias},
                    {"status", ativo.ToString()},
                    {"parent_id", idNodePai.ToString()},
                    {"imagem", linkParaImagem}
                };
                var result = JsonUtil.GetJson<DTOJsonResultNodeId>(url, "POST", postParameters);
                /*cursos_soap_createRequest c = new cursos_soap_createRequest(
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioSOAPPortal).Registro , 
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaSOAPPortal).Registro,
                        string.Empty, 
                        registro.Nome, 
                        registro.Apresentacao, 
                        sbTodasTaxonomias, 
                        ativo.ToString(), 
                        idNodePai.ToString(), 
                        linkParaImagem,
                        registro.CargaHoraria);

                soap_server_port_type s = new soap_server_port_typeClient();
                cursos_soap_createResponse cursosSoapCreateResponse = s.cursos_soap_create(c);

                */
                CategoriaConteudo categoriaConteudo = new BMCategoriaConteudo().ObterPorID(registro.ID);
                categoriaConteudo.IdNode = int.Parse(result.status);

                new BMCategoriaConteudo().Alterar(categoriaConteudo);

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new AlertException("Dados salvos no banco de dados. Erro ao sincronizar com o Portal");
            }
        }


        public void AtualizaNodeDrupal(CategoriaConteudo registro, string url)
        {
            try
            {

                if (!(registro.IdNode > 0))
                {
                    return;
                }

                string sbTodasTaxonomias = this.PrepararEntradaDeDadosParaOServicoDrupal(registro);

                int idNodePai = 0;

                if (registro.CategoriaConteudoPai != null && registro.CategoriaConteudoPai.ID > 0 && !registro.CategoriaConteudoPai.IdNode.HasValue)
                    throw new AlertException("Dados salvaos no banco de dados. Não foi possível enviar os dados para o Portal. Favor publicar a categoria pai primeiro");

                if (registro.CategoriaConteudoPai != null && registro.CategoriaConteudoPai.IdNode.HasValue)
                    idNodePai = registro.CategoriaConteudoPai.IdNode.Value;

                bool ativo = true;

                string linkParaImagem = string.Empty;

                //if (registro.Imagem != null && registro..ID > 0)
                //{
                //    ConfiguracaoSistema caminhoParaDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);
                //    linkParaImagem = CommonHelper.ObterLinkParaArquivoDeImagem(caminhoParaDiretorioDeUpload.Registro, registro.Imagem.ID);
                //}
                var postParameters = new Dictionary<string, string>{
                    { "login", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioSOAPPortal).Registro },
                    { "senha", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaSOAPPortal).Registro },
                    {"id_solucao_educacional", string.Empty},
                    {"titulo", registro.Nome},
                    {"corpo", registro.Apresentacao},
                    {"lista_taxs", sbTodasTaxonomias},
                    {"status", ativo.ToString()},
                    {"node_id",registro.IdNode.ToString()},
                    {"parent_id", idNodePai.ToString()},
                    {"imagem", linkParaImagem}
                };
                var result = JsonUtil.GetJson<DTOJsonResultNodeId>(url, "POST", postParameters);
                /*cursos_soap_updateRequest c = new cursos_soap_updateRequest(
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioSOAPPortal).Registro , 
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaSOAPPortal).Registro, 
                        string.Empty, 
                        registro.Nome, 
                        registro.Apresentacao, 
                        sbTodasTaxonomias, 
                        ativo.ToString(), 
                        registro.IdNode.ToString(), 
                        idNodePai.ToString(), 
                        linkParaImagem,
                        registro.CargaHoraria);

                soap_server_port_type s = new soap_server_port_typeClient();
                cursos_soap_updateResponse cursosSoapCreateResponse = s.cursos_soap_update(c);*/

                int nodeRetorno = int.Parse(result.status);

                if (nodeRetorno != registro.IdNode)
                {
                    CategoriaConteudo categoriaConteudo = new BMCategoriaConteudo().ObterPorID(registro.ID);
                    categoriaConteudo.IdNode = int.Parse(result.status);
                    new BMCategoriaConteudo().Alterar(categoriaConteudo);
                }

            }
            catch (AlertException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new AlertException("Dados salvos no banco de dados. Erro ao sincronizar com o Portal");
            }
        }

        private string PrepararEntradaDeDadosParaOServicoDrupal(CategoriaConteudo registro)
        {
            IList<CategoriaConteudoPermissao> ListaSolucaoEducacionalPermissao = registro.ListaPermissao;
            IList<CategoriaConteudoTags> ListaSolucaoEducacionalTags = registro.ListaTags;

            StringBuilder sbTodasTaxonomias = new StringBuilder();
            StringBuilder sbTaxonomiaListaUF = new StringBuilder();
            StringBuilder sbTaxonomiaListaPerfil = new StringBuilder();
            StringBuilder sbTaxonomiaListaNivelOcupacional = new StringBuilder();
            StringBuilder sbTaxonomiaListaTag = new StringBuilder();

            sbTaxonomiaListaUF.Append("ListaUF,");
            sbTaxonomiaListaPerfil.Append(";ListaPerfil,");
            sbTaxonomiaListaNivelOcupacional.Append(";ListaNivelOcupacional,");
            sbTaxonomiaListaTag.Append(";ListaTag,");

            bool temUf = false;
            bool temPerfil = false;
            bool temTag = false;
            bool temNivelOcupacional = false;

            /* Percorre a Lista de Soluções Educacionais para acrescentar estas Soluções Educacionais à lista de  
               taxonomias */
            foreach (CategoriaConteudoPermissao itemPermissao in ListaSolucaoEducacionalPermissao)
            {
                if (itemPermissao.Uf != null && itemPermissao.Uf.ID > 0)
                {
                    sbTaxonomiaListaUF.Append(string.Concat(itemPermissao.Uf.ID, ","));
                    temUf = true;
                }
                else if (itemPermissao.Perfil != null && itemPermissao.Perfil.ID > 0)
                {
                    sbTaxonomiaListaPerfil.Append(string.Concat(itemPermissao.Perfil.ID, ","));
                    temPerfil = true;
                }
                else if (itemPermissao.NivelOcupacional != null && itemPermissao.NivelOcupacional.ID > 0)
                {
                    sbTaxonomiaListaNivelOcupacional.Append(string.Concat(itemPermissao.NivelOcupacional.ID, ","));
                    temNivelOcupacional = true;
                }
            }

            if (temUf)
            {
                string taxonomiaListaUF = sbTaxonomiaListaUF.ToString();

                if (taxonomiaListaUF.EndsWith(","))
                {
                    taxonomiaListaUF = taxonomiaListaUF.Remove(taxonomiaListaUF.Length - 1, 1);
                }

                sbTodasTaxonomias.Append(taxonomiaListaUF);
            }

            if (temPerfil)
            {
                string taxonomiaListaPerfil = sbTaxonomiaListaPerfil.ToString();

                if (taxonomiaListaPerfil.EndsWith(","))
                {
                    taxonomiaListaPerfil = taxonomiaListaPerfil.Remove(taxonomiaListaPerfil.Length - 1, 1);
                }

                //Adiciona a lista de perfis, à lista de taxonomias
                sbTodasTaxonomias.Append(taxonomiaListaPerfil);
            }

            if (temNivelOcupacional)
            {
                string taxonomiaListaNivelOcupacional = sbTaxonomiaListaNivelOcupacional.ToString();

                if (taxonomiaListaNivelOcupacional.EndsWith(","))
                {
                    taxonomiaListaNivelOcupacional = taxonomiaListaNivelOcupacional.Remove(taxonomiaListaNivelOcupacional.Length - 1, 1);
                }

                //Adiciona a lista de perfis, à lista de taxonomias
                sbTodasTaxonomias.Append(taxonomiaListaNivelOcupacional);
            }

            /* Percorre a Lista de Tags para acrescentar estas tags à lista de  
               taxonomias (stringBuilder sbTodasTaxonomias) */
            foreach (CategoriaConteudoTags itemTag in ListaSolucaoEducacionalTags)
            {
                if (itemTag.Tag != null && itemTag.Tag.ID > 0)
                {
                    sbTaxonomiaListaTag.Append(string.Concat(itemTag.Tag.ID, ","));
                    temTag = true;
                }
            }

            if (temTag)
            {
                string taxonomiaListaTags = sbTaxonomiaListaTag.ToString();

                if (taxonomiaListaTags.EndsWith(","))
                {
                    taxonomiaListaTags = taxonomiaListaTags.Remove(taxonomiaListaTags.Length - 1, 1);
                }

                //Adiciona a lista de perfis, à lista de taxonomias
                sbTodasTaxonomias.Append(taxonomiaListaTags);
            }

            return sbTodasTaxonomias.ToString();

        }
        #endregion

        public IList<CategoriaConteudo> ObterTodasCategoriasFilhas(int idCategoriaPai)
        {
            var retorno = new List<CategoriaConteudo>();

            var categoriaPai = ObterCategoriaConteudoPorID(idCategoriaPai);

            retorno.Add(categoriaPai);

            foreach (var filha in categoriaPai.ListaCategoriaConteudoFilhos)
                retorno.AddRange(ObterTodasCategoriasFilhas(filha.ID));

            return retorno;
        }
    }
}
