using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;
using System.Text;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.wsDrupalServices;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterTrilha : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTrilha bmTrilha;

        #endregion

        #region "Construtor"


        public ManterTrilha()
            : base()
        {
            bmTrilha = new BMTrilha();
        }

        #endregion

        #region "Métodos Privados"

        private void PreencherInformacoesDeAuditoria(Trilha pTrilha)
        {
            base.PreencherInformacoesDeAuditoria(pTrilha);
            pTrilha.ListaPermissao.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
            pTrilha.ListaTag.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
            pTrilha.ListaTrilhaNivel.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
            //pTrilha.ListaTrilhaNivel.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x.PreRequisito ));

            foreach (TrilhaNivel trilhaNivel in pTrilha.ListaTrilhaNivel)
            {
                this.PreencherInformacoesDeAuditoria(trilhaNivel);
            }

            pTrilha.ListaTrilhaNivel.ToList().ForEach(x => x.ListaQuestionarioAssociacao.ToList()
                .ForEach(y => this.PreencherInformacoesDeAuditoria(y)));
        }

        #endregion

        #region "Métodos Públicos"

        public IList<DTORelatorioStatusUsuariosTrilhas> ObterStatusTrilhas(IEnumerable<int> statusMatricula, int? TrilhaId = null, int? UsuarioId = null,
            int? NivelTrilha = null, int? NivelOcupacionalId = null, int? UFId = null,
            DateTime? DataInicio = null, DateTime? DataFim = null, DateTime? DataLimite = null)
        {
            var query = new ManterUsuarioTrilha().ObterTodosIQueryable();
            var queryItemTrilha = new ManterItemTrilha().ObterTodosIQueryable();

            query = UsuarioId.HasValue && UsuarioId.Value != 0 ? query.Where(x => x.Usuario.ID == UsuarioId.Value) : query;
            query = NivelTrilha.HasValue && NivelTrilha.Value != 0 ? query.Where(x => x.TrilhaNivel.ID == NivelTrilha.Value) : query;
            query = TrilhaId.HasValue && TrilhaId.Value != 0 ? query.Where(x => x.TrilhaNivel.Trilha.ID == TrilhaId.Value) : query;
            query = NivelOcupacionalId.HasValue && NivelOcupacionalId.Value != 0 ? query.Where(x => x.NivelOcupacional.ID == NivelOcupacionalId.Value) : query;
            query = UFId.HasValue && UFId.Value != 0 ? query.Where(x => x.Uf.ID == UFId.Value) : query;
            query = statusMatricula.Any() ? query.Where(x => statusMatricula.Contains((int)x.StatusMatricula)) : query;
            query = DataInicio.HasValue ? query.Where(x => x.DataInicio >= DataInicio.Value) : query;
            query = DataFim.HasValue ? query.Where(x => x.DataFim <= DataFim.Value) : query;
            query = DataLimite.HasValue ? query.Where(x => x.DataLimite <= DataLimite.Value) : query;

            return query.ToList().Select(x => new DTORelatorioStatusUsuariosTrilhas
            {
                DataInicio = x.DataInicio,
                DataFim = x.DataFim?.ToString("dd/MM/yyyy") ?? "--",
                StatusMatricula = x.StatusMatricula.GetDescription(),
                DataLimite = x.DataLimite,
                NivelTrilha = x.TrilhaNivel.Nome,
                UF = x.Uf.Nome,
                NivelOcupacional = x.NivelOcupacional.Nome,
                Trilha = x.TrilhaNivel.Trilha.Nome,
                EmailUsuario = x.Usuario.Email,
                NomeUsuario = x.Usuario.Nome,
                TotalMoedasOuro = x.ObterSomaMoedas(enumTipoMoeda.Ouro),
                TotalMoedasPrata = x.ObterSomaMoedas(enumTipoMoeda.Prata),
                CPF = x.Usuario.CPF,
                Email = x.Usuario.Email,
                NotaProvaFinal = x.NotaProva?.ToString() ?? "--",
                DataAlteracaoStatus = x.DataAlteracaoStatus,
                SprintsRealizados = new ManterPontoSebrae().ObterTodosIqueryable().SelectMany(z => z.ListaPontoSebraeParticipacao).Where(y => y.UsuarioTrilha.ID == x.ID && y.UltimaParticipacao != null).Count(),
                SolucoesAutoindicativas = queryItemTrilha.Where(y => y.Missao.PontoSebrae.TrilhaNivel.ID == x.TrilhaNivel.ID && y.Ativo == true).Select(y => y.Usuario.ID).Count(i => i == x.Usuario.ID),
                SolucoesRealizadas = x.ListaItemTrilhaParticipacao.Count(i => i.Autorizado.HasValue && i.Autorizado.Value)
            }).ToList();
        }

        public IList<DTORelatorioParticipacaoTrilha> ConsultarParticipacaotrilhas(int idTrilhaNivel,
            int idTrilhaTopicoTematico, int idUsuarioTrilha)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("p_ID_TrilhaNivel", idTrilhaNivel);
            lstParam.Add("p_ID_TrilhaTopicoTematico", idTrilhaTopicoTematico);
            lstParam.Add("p_ID_UsuarioTrilha", idUsuarioTrilha);
            return bmTrilha.ExecutarProcedure<DTORelatorioParticipacaoTrilha>("SP_VERIFICA_PARTICIPACAO_ITEM_TRILHA",
                lstParam);
        }

        public IList<DTOMonitoramentoTrilhas> ConsultarMonitoramentoTrilhas(int idTrilha, int idNivelTrilha,
            int idUsuarioMonitor, int tipoParticipacao, DateTime dataInicial, DateTime dataFinal)
        {
            dataInicial = dataInicial + (new TimeSpan(0, 0, 0));
            dataFinal = dataFinal + (new TimeSpan(23, 59, 59));

            IDictionary<string, object> lstParams = new Dictionary<string, object>();
            lstParams.Add("pIdTrilha", idTrilha == 0 ? (int?)null : idTrilha);
            lstParams.Add("pIdNivelTrilha", idNivelTrilha == 0 ? (int?)null : idNivelTrilha);
            lstParams.Add("pIdUsuarioMonitor", idUsuarioMonitor == 0 ? (int?)null : idUsuarioMonitor);
            lstParams.Add("pTipoParticipacao", tipoParticipacao);
            lstParams.Add("pDataInicial", dataInicial);
            lstParams.Add("pDataFim", dataFinal);

            return bmTrilha.ExecutarProcedure<DTOMonitoramentoTrilhas>("SP_REL_Monitor_Trilhas", lstParams);
        }

        public IList<DTOMonitorTrilhas> ObterMonitores(int idTrilha, int idNivelTrilha)
        {
            IDictionary<string, object> lstParams = new Dictionary<string, object>();
            lstParams.Add("pIdTrilha", idTrilha == 0 ? (int?)null : idTrilha);
            lstParams.Add("pIdNivelTrilha", idNivelTrilha == 0 ? (int?)null : idNivelTrilha);

            return bmTrilha.ExecutarProcedure<DTOMonitorTrilhas>("SP_REL_Monitor_Trilhas_Monitores", lstParams);
        }

        public void AtualizarNodeIdDrupal(Trilha trilha, ManterTrilha manterTr = null, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null, bool statusTrilhaNivel = false)
        {
            var id = SalvaNodeDrupalRest(trilha, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, statusTrilhaNivel);

            if (!id.HasValue)
                return;

            trilha.IdNodePortal = id;


            (manterTr ?? new ManterTrilha()).Salvar(trilha);
        }

        public void IncluirTrilha(Trilha pTrilha, string url_base)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pTrilha);
                bmTrilha.Salvar(pTrilha);

                AtualizarNodeIdDrupal(pTrilha);

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

        public void AlterarTrilha(Trilha pTrilha, string url_base)
        {
            try
            {
                PreencherInformacoesDeAuditoria(pTrilha);

                // Atualizar os níveis antes de salvar o paizão.
                var niveisExistentes = new ManterTrilhaNivel().ObterPorTrilha(pTrilha);

                foreach (var nivel in pTrilha.ListaTrilhaNivel)
                {
                    if (nivel.ID == 0 && niveisExistentes.Any(x => x.Nome == nivel.Nome))
                    {
                        throw new AcademicoException(string.Format("O nível {0} já existe", nivel.Nome));
                    }
                }

                foreach (var nivelExistente in niveisExistentes)
                {
                    if (!pTrilha.ListaTrilhaNivel.Select(x => x.ID).Contains(nivelExistente.ID))
                    {
                        new ManterTrilhaNivel().ExcluirTrilhaNivel(nivelExistente.ID);
                    }
                }

                // Salvar o paizão.
                bmTrilha.FazerMergeAoSalvar(pTrilha);

                // Atualizar o objeto
                pTrilha = ObterTrilhaPorId(pTrilha.ID);

                AtualizarNodeIdDrupal(pTrilha);
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

        public IList<Trilha> ObterTodasTrilhas()
        {
            return bmTrilha.ObterTrilhas();
        }

        public IQueryable<Trilha> ObterTodasTrilhasIQueryable()
        {
            return bmTrilha.ObterTrilhasIQueryable();
        }

        public void ExcluirTrilha(int IdTrilha)
        {
            if (IdTrilha == 0) return;
            try
            {
                var trilha = bmTrilha.ObterPorId(IdTrilha);
                if (trilha == null) return;

                if (trilha.IdNodePortal.HasValue)
                    DrupalUtil.RemoverNodeDrupalRest(trilha.IdNodePortal.Value);

                bmTrilha.Excluir(trilha);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

        }

        public Trilha ObterTrilhaPorId(int pIdTrilha)
        {
            return bmTrilha.ObterPorId(pIdTrilha);
        }

        public IList<Trilha> ObterTodasTrilhasComParticipacao()
        {
            var ls = bmTrilha.ObterTrilhasIQueryable();

            ls =
                ls.Where(
                    p => p.ListaTrilhaNivel.Any(a => a.ListaItemTrilha.Any(b => b.ListaItemTrilhaParticipacao.Any())));

            return ls.ToList();
        }

        public IList<Trilha> ObterTrilhaPorNome(string pNome)
        {
            return bmTrilha.ObterTrilhaPorNome(pNome);
        }

        public IList<Trilha> ObterTrilhaPorFiltro(Trilha pTrilha)
        {
            return bmTrilha.ObterPorFiltro(pTrilha);
        }

        public void Evict(Trilha trilha)
        {
            bmTrilha.Evict(trilha);
        }

        #endregion

        #region "Métodos Pertinentes a Integração com o Drupal"

        public int? SalvaNodeDrupalRest(Trilha trilha, BMConfiguracaoSistema bmConfiguracaoSistema, BMLogSincronia bmLogSincronia, Usuario usuarioLogado, bool statusNivelTrilha = false)
        {
            var postParameters = DrupalUtil.InitPostParameters(trilha.ID, trilha.Nome, trilha.Descricao, "solucao", statusNivelTrilha);

            postParameters.Add("data[field_tipo_de_solucao]", "4");

            DrupalUtil.PermissoesAreaTematica(
                trilha.ListaAreasTematicas.Where(p => p.AreaTematica != null).Select(x => x.AreaTematica.ID).ToList(),
                ref postParameters);

            var permissoesUfs = trilha.ObterUfsDasPermissoes();
            var permissoesPerfis = trilha.ObterPerfisDasPermissoes();
            var permissoesNiveisOcupacionais = trilha.ObterNiveisOcupacionaisDasPermissoes();

            DrupalUtil.PermissoesUf(permissoesUfs.Select(x => x.ID).ToList(), ref postParameters);
            DrupalUtil.PermissoesPerfil(permissoesPerfis.Select(x => x.ID).ToList(),
                ref postParameters);
            DrupalUtil.PermissoesNivelOcupacional(permissoesNiveisOcupacionais.Select(x => x.ID).ToList(),
                ref postParameters);

            try
            {
                return DrupalUtil.SalvaNodeDrupalRest(postParameters, true, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AtualizaNodeDrupal(Trilha registro, string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return;
                if (!(registro.IdNode > 0))
                {
                    return;
                }

                var sbTodasTaxonomias = this.PrepararEntradaDeDadosParaOServicoDrupal(registro);

                var idNodePai = 0;

                if (registro.CategoriaConteudo != null && registro.CategoriaConteudo.ID > 0 &&
                    !registro.CategoriaConteudo.IdNode.HasValue)
                    throw new AlertException(
                        "Dados salvaos no banco de dados. Não foi possível enviar os dados para o Portal. Favor publicar a categoria pai primeiro");

                if (registro.CategoriaConteudo != null && registro.CategoriaConteudo.IdNode.HasValue)
                    idNodePai = registro.CategoriaConteudo.IdNode.Value;

                var ativo = true;

                var linkParaImagem = string.Empty;

                //if (registro.Imagem != null && registro..ID > 0)
                //{
                //    ConfiguracaoSistema caminhoParaDiretorioDeUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);
                //    linkParaImagem = CommonHelper.ObterLinkParaArquivoDeImagem(caminhoParaDiretorioDeUpload.Registro, registro.Imagem.ID);
                //}
                /*cursos_soap_updateRequest c = new cursos_soap_updateRequest(
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioSOAPPortal).Registro,
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaSOAPPortal).Registro,
                        string.Concat("/trilhas/trilha/", registro.ID.ToString()), 
                        registro.Nome, 
                        registro.Apresentacao, 
                        sbTodasTaxonomias, 
                        ativo.ToString(),
                        registro.IdNode.ToString(), 
                        idNodePai.ToString(), 
                        linkParaImagem,
                        registro.CargaHoraria);

                soap_server_port_type s = new soap_server_port_typeClient();
                cursos_soap_updateResponse cursosSoapCreateResponse = s.cursos_soap_update(c);
                */
                var postParameters = new Dictionary<string, string>
                {
                    {
                        "login",
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioSOAPPortal).Registro
                    },
                    {
                        "senha", ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaSOAPPortal).Registro
                    },
                    {"id_solucao_educacional", string.Concat("/trilhas/trilha/", registro.ID.ToString())},
                    {"titulo", registro.Nome},
                    //{"corpo", registro.Apresentacao},
                    {"lista_taxs", sbTodasTaxonomias},
                    {"status", ativo.ToString()},
                    {"node_id", registro.IdNode.ToString()},
                    {"parent_id", idNodePai.ToString()},
                    {"imagem", linkParaImagem}
                };
                var result = JsonUtil.GetJson<DTOJsonResultNodeId>(url, "POST", postParameters);
                var nodeRetorno = int.Parse(result.status);

                if (nodeRetorno == registro.IdNode) return;
                var trilha = ObterTrilhaPorId(registro.ID);
                trilha.IdNode = nodeRetorno;
                bmTrilha.FazerMergeAoSalvar(trilha);
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

        private string PrepararEntradaDeDadosParaOServicoDrupal(Trilha registro)
        {
            IList<TrilhaPermissao> ListaSolucaoEducacionalPermissao = registro.ListaPermissao;
            IList<TrilhaTag> ListaSolucaoEducacionalTags = registro.ListaTag;

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
            foreach (TrilhaPermissao itemPermissao in ListaSolucaoEducacionalPermissao)
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
                    taxonomiaListaNivelOcupacional =
                        taxonomiaListaNivelOcupacional.Remove(taxonomiaListaNivelOcupacional.Length - 1, 1);
                }

                //Adiciona a lista de perfis, à lista de taxonomias
                sbTodasTaxonomias.Append(taxonomiaListaNivelOcupacional);
            }

            /* Percorre a Lista de Tags para acrescentar estas tags à lista de  
               taxonomias (stringBuilder sbTodasTaxonomias) */
            foreach (TrilhaTag itemTag in ListaSolucaoEducacionalTags)
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

        public void Salvar(Trilha trilha)
        {
            bmTrilha.FazerMergeAoSalvar(trilha);
        }

        #endregion
    }
}