using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using moodle = Sebrae.Academico.BP.moodleIntegracao;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP
{
    public class ManterSolucaoEducacional : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMSolucaoEducacional bmSolucaoEducacional = null;

        #endregion

        #region "Construtor da Classe"

        /// <summary>
        /// Construtor da classe. Este construtor chama o construtor da classe base (BusinessProcessBase)
        /// </summary>
        public ManterSolucaoEducacional()
            : base()
        {
            bmSolucaoEducacional = new BMSolucaoEducacional();
        }

        #endregion

        #region "Métodos Pertinentes a Integração com o Drupal"

        private string PrepararEntradaDeDadosParaOServicoDrupal(SolucaoEducacional pSolucaoEducacional)
        {
            IList<SolucaoEducacionalPermissao> ListaSolucaoEducacionalPermissao = pSolucaoEducacional.ListaPermissao;
            IList<SolucaoEducacionalTags> ListaSolucaoEducacionalTags = pSolucaoEducacional.ListaTags;

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
            foreach (SolucaoEducacionalPermissao solucaoEducacionalPermissao in ListaSolucaoEducacionalPermissao)
            {
                if (solucaoEducacionalPermissao.Uf != null && solucaoEducacionalPermissao.Uf.ID > 0)
                {
                    sbTaxonomiaListaUF.Append(string.Concat(solucaoEducacionalPermissao.Uf.ID, ","));
                    temUf = true;
                }
                else if (solucaoEducacionalPermissao.Perfil != null && solucaoEducacionalPermissao.Perfil.ID > 0)
                {
                    sbTaxonomiaListaPerfil.Append(string.Concat(solucaoEducacionalPermissao.Perfil.ID, ","));
                    temPerfil = true;
                }
                else if (solucaoEducacionalPermissao.NivelOcupacional != null &&
                         solucaoEducacionalPermissao.NivelOcupacional.ID > 0)
                {
                    sbTaxonomiaListaNivelOcupacional.Append(
                        string.Concat(solucaoEducacionalPermissao.NivelOcupacional.ID, ","));
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

                //Adiciona a lista de ufs, à lista de taxonomias (stringBuilder sbTodasTaxonomias)
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
                    taxonomiaListaNivelOcupacional =
                        taxonomiaListaNivelOcupacional.Remove(taxonomiaListaNivelOcupacional.Length - 1, 1);
                }

                //Adiciona a lista de perfis, à lista de taxonomias
                sbTodasTaxonomias.Append(taxonomiaListaNivelOcupacional);
            }

            /* Percorre a Lista de Tags para acrescentar estas tags à lista de  
               taxonomias (stringBuilder sbTodasTaxonomias) */
            foreach (SolucaoEducacionalTags solucaoEducacionalTags in ListaSolucaoEducacionalTags)
            {
                if (solucaoEducacionalTags.Tag != null && solucaoEducacionalTags.Tag.ID > 0)
                {
                    sbTaxonomiaListaTag.Append(string.Concat(solucaoEducacionalTags.Tag.ID, ","));
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


            //Adiciona o id da categoria da solução educacional, à lista de todas as taxonomias (stringBuilder sbTodasTaxonomias)
            //sbTodasTaxonomias.Append(string.Concat(";ListaIdCategoriaSolucaoEducacional,"
            //                         , pSolucaoEducacional.CategoriaConteudo.ID));

            sbTodasTaxonomias.Append(string.Concat(";ListaFormaAquisicao,", pSolucaoEducacional.FormaAquisicao.ID));

            return sbTodasTaxonomias.ToString();

        }

        #endregion

        #region "Métodos Gerais"

        public void AtualizarNodeIdDrupal(SolucaoEducacional solucao, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            if (solucao.FormaAquisicao == null || !solucao.FormaAquisicao.EnviarPortal)
                return;

            var id = SalvaNodeDrupalRest(solucao, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);

            if (!id.HasValue)
                return;

            solucao.IdNodePortal = id.Value;
            bmSolucaoEducacional.Salvar(solucao, false);
        }

        /// <summary>
        /// Cadastra (inclui) uma solução educacional.
        /// </summary>
        /// <param name="pSolucaoEducacional">Informações de uma solução educacional</param>
        /// <param name="url_base">URL para criação do NODE ID</param>
        public void IncluirSolucaoEducacional(SolucaoEducacional pSolucaoEducacional, string url_base)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pSolucaoEducacional);
                this.bmSolucaoEducacional.Salvar(pSolucaoEducacional);

                AtualizarNodeIdDrupal(pSolucaoEducacional);
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

        private void PreencherInformacoesDeAuditoria(SolucaoEducacional pSolucaoEducacional)
        {
            base.PreencherInformacoesDeAuditoria(pSolucaoEducacional);
            pSolucaoEducacional.ListaPermissao.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
            pSolucaoEducacional.ListaTags.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        /// <summary>
        /// Alterar (faz update) nas informações de uma solução educacional.
        /// </summary>
        /// <param name="pSolucaoEducacional">Informações de uma solução educacional</param>
        /// <param name="urlBase">URL para criação do NODE ID</param>
        public void AlterarSolucaoEducacional(SolucaoEducacional pSolucaoEducacional, string urlBase)
        {
            var manterOferta = new ManterOferta();

            try
            {
                base.PreencherInformacoesDeAuditoria(pSolucaoEducacional);

                bmSolucaoEducacional.Salvar(pSolucaoEducacional);
                AtualizarNodeIdDrupal(pSolucaoEducacional);

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
            finally
            {
                Task.Run(() => manterOferta.AtualizarPermissoesSolucaoVinculada(pSolucaoEducacional));
            }
        }

        public bool AlterouCategoria(int solucaoEducacionalId, CategoriaConteudo novaCategoria)
        {
            return bmSolucaoEducacional.AlterouCategoria(solucaoEducacionalId, novaCategoria);
        }

        public int? ObterProximoCodigoSequencial(CategoriaConteudo categoria)
        {
            return bmSolucaoEducacional.ObterProximoCodigoSequencial(categoria);
        }


        /// <summary>
        /// Obtém do Repositório (Banco de dados) todas as Soluções Educacionais Cadastradas.
        /// </summary>
        /// <returns>Lista com todas as Soluções educacionais cadastradas</returns>
        public IQueryable<SolucaoEducacional> ObterTodosSolucaoEducacional(bool ordenarPorNome = true)
        {
            return bmSolucaoEducacional.ObterTodos(ordenarPorNome: ordenarPorNome);
        }

        /// <summary>
        /// Obtém uma solução educacional pelo seu ID.
        /// </summary>
        /// <param name="pId">Id da Solução Educacional que seja deseja obter do Repositório</param>
        /// <returns>Objeto da classe SolucaoEducacional, do ID informado</returns>
        public SolucaoEducacional ObterSolucaoEducacionalPorId(int pId)
        {
            return bmSolucaoEducacional.ObterPorId(pId);
        }

        /// <summary>
        /// Exclui uma Solução Educacional.
        /// </summary>
        public void ExcluirSolucaoEducacional(int idSolucaoEducacional)
        {
            if (idSolucaoEducacional == 0) return;
            var solucaoEducacional = bmSolucaoEducacional.ObterPorId(idSolucaoEducacional);
            if (solucaoEducacional == null) return;

            if (solucaoEducacional.IdNodePortal.HasValue)
            {
                DrupalUtil.RemoverNodeDrupalRest(solucaoEducacional.IdNodePortal.Value);
            }

            bmSolucaoEducacional.Excluir(solucaoEducacional);
        }

        public IList<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int pIdFormaAquisicao)
        {
            return bmSolucaoEducacional.ObterPorFormaAquisicao(pIdFormaAquisicao);
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicaoUf(int idFormaAquisicao = 0,
            Uf uf = null)
        {
            using (bmSolucaoEducacional)
            {
                return bmSolucaoEducacional.ObterPorFormaAquisicaoUf(idFormaAquisicao, uf);
            }
        }

        public IList<SolucaoEducacional> ObterSolucaoEducacionalPorFiltro(SolucaoEducacional pSolucaoEducacional)
        {
            return bmSolucaoEducacional.ObterPorFiltro(pSolucaoEducacional).ToList();
        }

        public IList<SolucaoEducacional> ObterPorFiltroPesquisa(SolucaoEducacional pSolucaoEducacional, bool? ativo,
            List<int> usuarioPermissoes)
        {
            return bmSolucaoEducacional.ObterPorFiltroPesquisa(pSolucaoEducacional, ativo, usuarioPermissoes);
        }

        public IList<SolucaoEducacional> ObterListaSolucaoEducacionalPorCategoria(int idCategoriaSolucaoEducacional)
        {
            return bmSolucaoEducacional.ObterListaSolucaoEducacionalPorCategoria(idCategoriaSolucaoEducacional);
        }

        public IList<SolucaoEducacional> ObterListaSolucaoEducacionalPorCategoria(IEnumerable<int> categoriaConteudo)
        {
            return bmSolucaoEducacional.ObterListaSolucaoEducacionalPorCategoria(categoriaConteudo);
        }

        public IQueryable<SolucaoEducacional> ObterListaSolucaoEducacionalPorCategoriaGestor(
            IEnumerable<int> categoriaConteudo)
        {
            return bmSolucaoEducacional.ObterListaSolucaoEducacionalPorCategoriaGestor(categoriaConteudo);
        }

        #endregion

        public IList<DTOTop5CursoOnline> ObterCursosOnline(DateTime dataInicio, DateTime dataFim, int idUf)
        {

            var parametros = new Dictionary<string, object>();

            parametros.Add("DataInicio", dataInicio);
            parametros.Add("DataFim", dataFim);

            if (idUf != 0)
            {
                parametros.Add("IdUf", idUf);
            }

            return bmSolucaoEducacional.ExecutarProcedure<DTOTop5CursoOnline>("DASHBOARD_REL_Top5CursosOnline",
                parametros);
        }

        public IList<DTOTop5CursoPresencial> ObterCursoPresencial(DateTime dataInicio, DateTime dataFim, int idUf)
        {
            var parametros = new Dictionary<string, object>();

            parametros.Add("DataInicio", dataInicio);
            parametros.Add("DataFim", dataFim);

            if (idUf != 0)
            {
                parametros.Add("IdUf", idUf);
            }

            return bmSolucaoEducacional.ExecutarProcedure<DTOTop5CursoPresencial>("DASHBOARD_Top5CursosPresenciais",
                parametros);
        }

        public IList<DTOSituacaoCursos> ConsultarSituacoes(int? IdUf, int? Ano)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            if (Ano.HasValue)
                lstParam.Add("ANO", Ano);
            if (IdUf.HasValue)
                lstParam.Add("ID_UF", IdUf);

            return bmSolucaoEducacional.ExecutarProcedure<DTOSituacaoCursos>("SP_SITUACAO_CURSOS", lstParam);
        }

        public IList<DTOSolucaoEducacionalFormaAquisicao> ConsultarSolucaEducacionalFormaAquisicao(List<int> formasDeAquisicao, List<int> categorias, List<int> pIdUfResponsavel)
        {
            var lstParameter = new Dictionary<string, object>
            {
                {
                    "P_FormasAquisicao", string.Join(",", formasDeAquisicao)
                },
                {
                    "P_Categorias", string.Join(",", categorias)
                }
            };

            if (pIdUfResponsavel != null && pIdUfResponsavel.Any())
                lstParameter.Add("P_UFResponsavel", string.Join(", ", pIdUfResponsavel));
            else
                lstParameter.Add("P_UFResponsavel", DBNull.Value);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTOSolucaoEducacionalFormaAquisicao>(
                    "SP_REL_SOLUCAO_EDUCACIONAL_FORMA_AQUISICAO", lstParameter);
        }

        public IList<DTOUnificadoSolucaoEducacional> ConsultarSolucaoEducacionalUnificado(
            IEnumerable<int> pFormasDeAquisicao, IEnumerable<int> pTiposDeOferta, IEnumerable<int> pProgramas,
            IEnumerable<int> pCategorias, IEnumerable<int> pPublicoAlvo, IEnumerable<int> pNiveisOcupacionais,
            IEnumerable<int> pPerfis, IEnumerable<int> pUf, IEnumerable<int> pUfResponsavel)
        {
            IDictionary<string, object> lstParams = new Dictionary<string, object>();

            if (pFormasDeAquisicao != null && pFormasDeAquisicao.Any())
                lstParams.Add("P_FormasDeAquisicao", string.Join(", ", pFormasDeAquisicao));
            else
                lstParams.Add("P_FormasDeAquisicao", DBNull.Value);


            if (pTiposDeOferta != null && pTiposDeOferta.Any())
                lstParams.Add("P_TiposOferta", string.Join(", ", pTiposDeOferta));
            else
                lstParams.Add("P_TiposOferta", DBNull.Value);


            if (pProgramas != null && pProgramas.Any())
                lstParams.Add("P_Programas", string.Join(", ", pProgramas));
            else
                lstParams.Add("P_Programas", DBNull.Value);


            if (pCategorias != null && pCategorias.Any())
                lstParams.Add("P_Categorias", string.Join(", ", pCategorias));
            else
                lstParams.Add("P_Categorias", DBNull.Value);


            if (pPublicoAlvo != null && pPublicoAlvo.Any())
                lstParams.Add("P_PublicosAlvo", string.Join(", ", pPublicoAlvo));
            else
                lstParams.Add("P_PublicosAlvo", DBNull.Value);


            if (pNiveisOcupacionais != null && pNiveisOcupacionais.Any())
                lstParams.Add("P_NiveisOcupacionais", string.Join(", ", pNiveisOcupacionais));
            else
                lstParams.Add("P_NiveisOcupacionais", DBNull.Value);


            if (pPerfis != null && pPerfis.Any())
                lstParams.Add("P_Perfis", string.Join(", ", pPerfis));
            else
                lstParams.Add("P_Perfis", DBNull.Value);

            if (pUf != null && pUf.Any())
                lstParams.Add("P_UFS", string.Join(", ", pUf));
            else
                lstParams.Add("P_UFS", DBNull.Value);

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParams.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParams.Add("P_UFResponsavel", DBNull.Value);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTOUnificadoSolucaoEducacional>(
                    "SP_REL_SOLUCAO_EDUCACIONAL_UNIFICADO", lstParams);
        }

        public IList<DTOSolucaoEducacionalPublicoAlvo> ConsultarSolucaEducacionalPublicoAlvo(List<int> publicosAlvo, List<int> categorias, IEnumerable<int> pUfResponsavel)
        {
            var lstParameter = new Dictionary<string, object>
            {
                {
                    "P_PublicosAlvo", string.Join(",", publicosAlvo)
                },
                {
                    "P_Categorias", string.Join(",", categorias)
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParameter.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParameter.Add("P_UFResponsavel", DBNull.Value);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTOSolucaoEducacionalPublicoAlvo>(
                    "SP_REL_SOLUCAO_EDUCACIONAL_PUBLICO_ALVO", lstParameter);
        }

        public IList<DTORelatorioSolucaoEducacionalPrograma> ObterSolucaoEducacionalPorPrograma(int pIdPrograma,
            int pIdUf, IEnumerable<int> pUfResponsavel)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();
            lstParam.Add("P_ID_Programa", pIdPrograma);
            lstParam.Add("P_ID_UF", pIdUf);

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTORelatorioSolucaoEducacionalPrograma>(
                    "SP_REL_SOLUCAO_EDUCACIONAL_PROGRAMA ", lstParam);
        }

        public IList<DTOSolucaoEducacionalOferta> ConsultarSolucaoEducacionalOferta(int? pIdFormaAquisicao,
            int? pIdTipoOferta, int? pIdSolucaoEducacional, int? pIdUf, IEnumerable<int> pUfResponsavel)
        {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "p_FormaAquisicao", pIdFormaAquisicao
                },
                {
                    "p_TipoOferta", pIdTipoOferta
                },
                {
                    "p_SolucaoEducacional", pIdSolucaoEducacional
                },
                {
                    "p_ID_UF", pIdUf
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTOSolucaoEducacionalOferta>(
                    "SP_REL_SOLUCAO_EDUCACIONAL_OFERTA", lstParam);
        }

        public IList<DTORelatorioSEOfertaMatricula> ConsultarSeOfertaMatricula(int? pIdFormaAquisicao,
            int? pIdSolucaoEducacional, int? pIdTipoOferta, DateTime? dtIni, DateTime? dtFim, List<int> pIdUfResponsavel)
        {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "P_FORMA_AQUISICAO", pIdFormaAquisicao
                },
                {
                    "P_SOLUCAO_EDUCACIONAL", pIdSolucaoEducacional
                },
                {
                    "P_TIPO_OFERTA", pIdTipoOferta
                },
                {
                    "P_DATA_INICIO", dtIni
                },
                {
                    "P_DATA_FIM", dtFim
                }
            };

            if (pIdUfResponsavel != null && pIdUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pIdUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return bmSolucaoEducacional.ExecutarProcedure<DTORelatorioSEOfertaMatricula>("SP_REL_SE_OFERTA_MATRICULA",
                lstParam);
        }

        public IList<DTORelatorioMetaDesenvolvimentoSE> ConsultarMetaDesenvolvimentoSe(int? pIdNivelOcupacional,
            int? pIdUf, int? pIdCategoriaConteudo)
        {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "P_Nivel_Ocupacional", pIdNivelOcupacional
                },
                {
                    "P_UF", pIdUf
                },
                {
                    "P_CATEGORIA_SE", pIdCategoriaConteudo
                }
            };


            return
                bmSolucaoEducacional.ExecutarProcedure<DTORelatorioMetaDesenvolvimentoSE>(
                    "SP_REL_META_DESENVOLVIMENTO_SE", lstParam);
        }

        public IList<DTORelatorioMatriculaSolucaoEducacional> ConsultarMatriculaSolucaoEducacional(int? pFormaAquisicao,
            int? pSolucaoEducacional, int? pSituacaoMatricula, List<int> pIdUf, DateTime? pDataInicial, DateTime? pDataFinal,
            string pCategorias, List<int> pIdUfResponsavel)
        {
            var lstParam = new Dictionary<string, object>
            {
                {
                    "p_FormaAquisicao", pFormaAquisicao
                },
                {
                    "p_SolucaoEducacional", pSolucaoEducacional
                },
                {
                    "p_StatusMatricula", pSituacaoMatricula
                },
                {
                    "p_Data_Inicial", pDataInicial
                },
                {
                    "p_Data_Final", pDataFinal
                },
                {
                    "p_Categorias", pCategorias
                },
                {
                    "p_UF_Gestor", 0
                }
            };

            if (pIdUf != null && pIdUf.Any())
                lstParam.Add("P_UF", string.Join(", ", pIdUf));
            else
                lstParam.Add("P_UF", DBNull.Value);

            if (pIdUfResponsavel != null && pIdUfResponsavel.Any())
                lstParam.Add("P_UFResponsavel", string.Join(", ", pIdUfResponsavel));
            else
                lstParam.Add("P_UFResponsavel", DBNull.Value);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTORelatorioMatriculaSolucaoEducacional>(
                    "SP_REL_MATRICULA_SOLUCAO_EDUCACIONAL", lstParam);
        }

        public void EnviarDadosOferta(int idOferta)
        {
            var matriculaFiltro = new MatriculaOferta { Oferta = new Oferta { ID = idOferta } };

            var listaProcessar = new BMMatriculaOferta().ObterPorFiltro(matriculaFiltro).ToList();

            foreach (var item in listaProcessar)
            {
                var soapCliente = new moodle.IntegracaoSoapClient();

                var firstOrDefault = item.MatriculaTurma.FirstOrDefault();

                if (firstOrDefault != null)
                    soapCliente.MatricularAluno(item.Usuario.Nome, item.Usuario.CPF, item.Usuario.Email,
                        item.Usuario.Cidade, item.Oferta.CodigoMoodle.ToString(), firstOrDefault.Turma.IDChaveExterna);
            }
        }

        public IList<DTOCursosMaiorNumeroConcluintes> ConsultarConcluintes(int? IdUf, int? Ano)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            if (Ano.HasValue)
                lstParam.Add("ANO", Ano);
            if (IdUf.HasValue)
                lstParam.Add("ID_UF", IdUf);

            return
                bmSolucaoEducacional.ExecutarProcedure<DTOCursosMaiorNumeroConcluintes>(
                    "SP_CURSOS_MAIOR_NUMERO_CONCLUINTES", lstParam);
        }

        public IList<DTORelatorioCapacitados> ConsultarRelatorioCapacitados(int? idPerfil, int? idUf,
            int? idNivelOcupacional,
            int? idFormaAquisicao, int? idSolucaoEducacional, int? idStatusMatricula, DateTime? dataInicial,
            DateTime? dataFinal, string situacaoUsuario, IEnumerable<int> pUfResponsavel)
        {

            var parametros = new Dictionary<string, object>
            {
                {
                    "p_Perfil", idPerfil
                },
                {
                    "p_UF", idUf
                },
                {
                    "p_NivelOcupacional", idNivelOcupacional
                },
                {
                    "p_FormaAquisicao", idFormaAquisicao
                },
                {
                    "p_SolucaoEducacional", idSolucaoEducacional
                },
                {
                    "p_StatusMatricula", idStatusMatricula
                },
                {
                    "p_DataInicial", dataInicial
                },
                {
                    "p_DataFinal", dataFinal
                },
                {
                    "p_SituacaoUsuario", situacaoUsuario
                }
            };

            if (pUfResponsavel != null && pUfResponsavel.Any())
                parametros.Add("P_UFResponsavel", string.Join(", ", pUfResponsavel));
            else
                parametros.Add("P_UFResponsavel", DBNull.Value);

            return bmSolucaoEducacional.ExecutarProcedure<DTORelatorioCapacitados>("SP_REL_CAPACITADOS", parametros);
        }

        /// <summary>
        /// Obtém do Repositório (Banco de dados) as Soluções Educacionais disponíveis e ativas levando em considerando o perfil de gestão        
        /// </summary>
        /// <returns>Lista com todas as Soluções educacionais cadastradas</returns>
        public IQueryable<SolucaoEducacional> ObterTodosPorGestor(bool obterInativos = false)
        {
            return obterInativos ? bmSolucaoEducacional.ObterTodosPorGestor(true) : bmSolucaoEducacional.ObterTodosPorGestor(null);
        }

        public IQueryable<SolucaoEducacional> ObterTodosIQueryable()
        {
            return bmSolucaoEducacional.ObterTodos(false, false);
        }

        /// <summary>
        /// Verifica se uma solução educacional pode ter o mesmo nome, dentro de uma determinada UF, nacionalizada ou não,
        /// ou uma UF que permite SEs com o mesmo nome.
        /// </summary>
        /// <param name="solucao">Solução Educacional</param>
        public void VerificarConsistenciaUk(SolucaoEducacional solucao)
        {
            new BMSolucaoEducacional().VerificarConsistenciaUk(solucao);
        }

        public int? SalvaNodeDrupalRest(SolucaoEducacional registro, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var postParameters = DrupalUtil.InitPostParameters(registro.ID, registro.Nome, registro.Apresentacao,
                "solucao", registro.Ativo);

            /*1 - Cursos Online; 2 - Cursos Presenciais; 3 - Cursos Mistos; 4 - Trilhas; 5 - Programas*/
            int tipoDeSolucao;

            switch (registro.FormaAquisicao.ID)
            {
                // Curso presencial.
                case 22:
                    tipoDeSolucao = 2;
                    break;
                // Curso misto.
                case 40:
                    tipoDeSolucao = 3;
                    break;

                // Jogo online, Jogo presencial e Jogo misto.
                case 43:
                case 44:
                case 45:
                case 113:
                    tipoDeSolucao = 6;
                    break;

                default:
                    tipoDeSolucao = 1;
                    break;
            }

            postParameters.Add("data[field_tipo_de_solucao]", tipoDeSolucao.ToString());

            DrupalUtil.PermissoesAreaTematica(registro.ListaAreasTematicas.Select(x => x.AreaTematica.ID).ToList(),
                ref postParameters);

            DrupalUtil.PermissoesUf(registro.ListaPermissao.Where(p => p.Uf != null).Select(x => x.Uf.ID).ToList(),
                ref postParameters);

            DrupalUtil.PermissoesPerfil(
                registro.ListaPermissao.Where(p => p.Perfil != null).Select(x => x.Perfil.ID).ToList(),
                ref postParameters);

            DrupalUtil.PermissoesNivelOcupacional(
                registro.ListaPermissao.Where(p => p.NivelOcupacional != null)
                    .Select(x => x.NivelOcupacional.ID)
                    .ToList(), ref postParameters);

            try
            {
                return DrupalUtil.SalvaNodeDrupalRest(postParameters, true, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
            }
            catch (Exception)
            {
                throw new AlertException("Erro na sincronização. Tente salvar novamente.");
            }
        }

        public IQueryable<SolucaoEducacional> ObterPreRequisitosUf()
        {
            var usuario = new BMUsuario().ObterUsuarioLogado();

            var retorno = ObterTodosSolucaoEducacional();

            if (usuario.IsGestor())
                bmSolucaoEducacional.FiltrarPermissaoVisualizacao(ref retorno, usuario.UF.ID);

            return retorno;
        }

        public void ValidarPreRequisitosDaMatricula(MatriculaOferta matriculaOferta, int idTurma = 0)
        {
            bmSolucaoEducacional.ValidarPreRequisitosDaMatricula(matriculaOferta, idTurma);
        }

        public void FiltrarPermissaoVisualizacao(ref IQueryable<SolucaoEducacional> se, int ufId)
        {
            bmSolucaoEducacional.FiltrarPermissaoVisualizacao(ref se, ufId);
        }

        public IEnumerable<SolucaoEducacionalObrigatoria> ObterObrigatoriosPorSolucaoEducacionalNiveisOcupacionais(
            IList<int> solucoesSelecionadas = null,
            IList<int> niveisOcupacionaisSelecionados = null)
        {
            var solucoesObrigatorias = ObterObrigatorios();

            if (solucoesSelecionadas != null && solucoesSelecionadas.Any())
                solucoesObrigatorias =
                    solucoesObrigatorias.Where(x => solucoesSelecionadas.Contains(x.SolucaoEducacional.ID));

            if (niveisOcupacionaisSelecionados != null && niveisOcupacionaisSelecionados.Any())
                solucoesObrigatorias =
                    solucoesObrigatorias.Where(
                        x => niveisOcupacionaisSelecionados.Contains(x.NivelOcupacional.ID));

            // Verificar UF do Gestor.
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsGestor())
                solucoesObrigatorias =
                    solucoesObrigatorias.Where(x => x.SolucaoEducacional.UFGestor.ID == usuarioLogado.UF.ID);

            return solucoesObrigatorias;
        }

        public IEnumerable<SolucaoEducacionalObrigatoria> ObterObrigatorios(NivelOcupacional nivelOcupacional = null)
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var sesObrigatorias = bmSolucaoEducacional.ObterObrigatorios();

            // Filtra pelas UF do gestor logado.
            if (usuarioLogado.IsGestor())
                sesObrigatorias =
                    sesObrigatorias.Where(
                        x =>
                            x.SolucaoEducacional.UFGestor != null &&
                            x.SolucaoEducacional.UFGestor.ID == usuarioLogado.UF.ID);

            if (nivelOcupacional != null)
                sesObrigatorias =
                    sesObrigatorias.Where(
                        x => x.NivelOcupacional != null && x.NivelOcupacional.ID == nivelOcupacional.ID);

            return sesObrigatorias;
        }

        public IQueryable<SolucaoEducacional> ObterComQuestionario(int? idQuestionario = null)
        {
            return bmSolucaoEducacional.ObterComQuestionario(idQuestionario);
        }
    }
}
