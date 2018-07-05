using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BP
{
    public class ManterOferta : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMOferta bmOferta = null;

        #endregion

        #region "Construtor"

        public ManterOferta()
            : base()
        {
            bmOferta = new BMOferta();
        }

        #endregion

        public void IncluirOferta(Oferta pOferta)
        {
            try
            {
                pOferta.NomeSalvo = (pOferta.SolucaoEducacional.CategoriaConteudo.Sigla + ".SE" + pOferta.SolucaoEducacional.Sequencia + ".OF" + pOferta.Sequencia);
                this.PreencherInformacoesDeAuditoria(pOferta);
                bmOferta.Salvar(pOferta);

                AtualizarNodeIdDrupal(pOferta);

                //Notificar o Usuário
                this.NotificarUsuariosQueDesejamReceberNotificacaoOferta(pOferta);

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarOferta(Oferta pOferta)
        {
            try
            {
                PreencherInformacoesDeAuditoria(pOferta);

                bmOferta.Salvar(pOferta);

                AtualizarNodeIdDrupal(pOferta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarNodeIdDrupal(Oferta oferta, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            if (oferta.SolucaoEducacional.FormaAquisicao == null || !oferta.SolucaoEducacional.FormaAquisicao.EnviarPortal)
                return;

            if (oferta.InscricaoOnline != null && oferta.IdNodePortal != null && !oferta.InscricaoOnline.Value)
            {
                DrupalUtil.RemoverNodeDrupalRest(oferta.IdNodePortal.Value);
                return;
            }

            var id = SalvaNodeDrupalRest(oferta, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);

            if (!id.HasValue)
                return;

            oferta.IdNodePortal = id.Value;

            bmOferta.Salvar(oferta, false);
        }

        public int? SalvaNodeDrupalRest(Oferta oferta, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var postParameters = DrupalUtil.InitPostParameters(oferta.ID, oferta.Nome, null, "oferta");

            postParameters.Add("data[field_data_inicio_inscricao]", oferta.DataInicioInscricoes.HasValue ? DataUtil.ToUnix(oferta.DataInicioInscricoes.Value).ToString() : "");
            postParameters.Add("data[field_data_fim_inscricao]", oferta.DataFimInscricoes.HasValue ? DataUtil.ToUnix(oferta.DataFimInscricoes.Value).ToString() : "");
            postParameters.Add("data[field_carga_horaria]", oferta.CargaHoraria.ToString());
            postParameters.Add("data[field_solucao_sgus_id]", (oferta.SolucaoEducacional != null ? oferta.SolucaoEducacional.ID : 0).ToString());

            /*1 - Cursos Online; 2 - Cursos Presenciais; 3 - Cursos Mistos; 4 - Trilhas; 5 - Programas*/
            var tipoDeSolucao = 1;
            if (oferta.SolucaoEducacional != null)
            {
                switch (oferta.SolucaoEducacional.FormaAquisicao.ID)
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
            }

            postParameters.Add("data[field_tipo_de_solucao]", tipoDeSolucao.ToString());

            DrupalUtil.PermissoesUf(oferta.ListaPermissao.Where(p => p.Uf != null).Select(x => x.Uf.ID).ToList(), ref postParameters);
            DrupalUtil.PermissoesPerfil(oferta.ListaPermissao.Where(p => p.Perfil != null).Select(x => x.Perfil.ID).ToList(), ref postParameters);
            DrupalUtil.PermissoesNivelOcupacional(oferta.ListaPermissao.Where(p => p.NivelOcupacional != null).Select(x => x.NivelOcupacional.ID).ToList(), ref postParameters);
            try
            {
                return DrupalUtil.SalvaNodeDrupalRest(postParameters, true, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int? ObterProximoCodigoSequencial(SolucaoEducacional solucao)
        {
            return bmOferta.ObterProximoCodigoSequencial(solucao);
        }

        public bool AlterouSolucaoEducacional(int idOferta, SolucaoEducacional novaSolucao)
        {
            return bmOferta.AlterouSolucaoEducacional(idOferta, novaSolucao);
        }

        private void PreencherInformacoesDeAuditoria(Oferta pOferta)
        {
            base.PreencherInformacoesDeAuditoria(pOferta);
            pOferta.ListaPermissao.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        private void NotificarUsuariosQueDesejamReceberNotificacaoOferta(Oferta oferta)
        {
            if (oferta.TipoOferta.Equals(enumTipoOferta.Exclusiva)) return;
            IList<Usuario> ListaUsuariosQueDesejamReceberNotificacaoOferta = new BMUsuario().ObterUsuariosQueDesejamReceberNotificacaoOferta();
            if (ListaUsuariosQueDesejamReceberNotificacaoOferta != null && ListaUsuariosQueDesejamReceberNotificacaoOferta.Count > 0)
            {
                BMNotificacao notificacaoBM = new BMNotificacao();

                Notificacao notificacao = null;

                foreach (Usuario u in ListaUsuariosQueDesejamReceberNotificacaoOferta)
                {
                    notificacao = new Notificacao()
                    {
                        DataGeracao = DateTime.Now,
                        DataNotificacao = DateTime.Now,
                        Usuario = new BMUsuario().ObterPorId(u.ID),
                        TextoNotificacao = "Há vaga para o curso de seu interesse",
                    };

                    if (oferta.SolucaoEducacional.IdNode > 0)
                    {
                        notificacao.Link = string.Concat(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal).Registro,
                                                         string.Format("/Inscricoes/Inscricao/{0}", oferta.SolucaoEducacional.IdNode));
                    }
                    else
                    {
                        notificacao.Link = string.Concat(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal).Registro, "/Inscricoes/");
                    }

                }
            }
        }

        public void ExcluirOferta(int IdOferta)
        {
            if (IdOferta == 0) return;
            try
            {
                var oferta = bmOferta.ObterPorId(IdOferta);
                if (oferta == null) return;
                if (oferta.IdNodePortal.HasValue)
                {
                    DrupalUtil.RemoverNodeDrupalRest(oferta.IdNodePortal.Value);
                }
                bmOferta.ExcluirOferta(oferta);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<Oferta> ObterOfertaPorFiltro(string nome, string idChaveExterna, int idSolucaoEducacional)
        {
            return bmOferta.ObterPorFiltro(nome, idChaveExterna, idSolucaoEducacional);
        }

        public IQueryable<Oferta> ObterOfertasPorGestor(int idSolucaoEducacional)
        {
            // Caso a SE informada seja 0, retorna todas as ofertas de todas as SEs.
            return new BMSolucaoEducacional().ObterTodosPorGestor().Where(s => idSolucaoEducacional == 0 || s.ID == idSolucaoEducacional).SelectMany(s => s.ListaOferta);
        }

        public IQueryable<Oferta> ObterTodasIQueryable()
        {
            return bmOferta.ObterTodasIQueryable();
        }
        public IQueryable<Oferta> ObterTodasOfertas()
        {
            return bmOferta.ObterTodos();
        }

        public Oferta ObterOfertaPorID(int pId)
        {
            return bmOferta.ObterPorId(pId);
        }

        public Oferta ObterOfertaVigente(int solucaoEducacionalId)
        {
            return bmOferta.ObterTodos().FirstOrDefault(x => x.SolucaoEducacional.ID == solucaoEducacionalId && x.DataInicioInscricoes <= DateTime.Now && x.DataFimInscricoes >= DateTime.Now);
        }

        public IQueryable<Oferta> ObterOfertaPorSolucaoEducacional(int solucaoEducacionalId)
        {
            return bmOferta.ObterTodos().Where(x => x.SolucaoEducacional.ID == solucaoEducacionalId);
        }
        public IQueryable<Oferta> ObterOfertaPorSolucaoEducacional(SolucaoEducacional solucaoEducacional)
        {
            try
            {
                if (solucaoEducacional == null || solucaoEducacional.ID < 0)
                    throw new AcademicoException("Informe a Solução Educacional");

                return bmOferta.ObterOfertaPorSolucaoEducacional(solucaoEducacional);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        public void EnviarEmailParaAlunosDaOferta(IList<MatriculaOferta> matriculas, bool pendentes)
        {

            var templateMensagemEmailOfertaExclusiva = TemplateUtil.ObterInformacoes(enumTemplate.EnvioEmailParaInscritoOferta);
            if (pendentes)
                templateMensagemEmailOfertaExclusiva = TemplateUtil.ObterInformacoes(enumTemplate.EnvioEmailParaPendenteDeConfirmacao);

            foreach (var matriculaOferta in matriculas)
            {
                try
                {
                    var assuntoDoEmail = templateMensagemEmailOfertaExclusiva.Assunto;

                    var corpoEmail = templateMensagemEmailOfertaExclusiva.TextoTemplate;

                    var emailDoDestinatario = matriculaOferta.Usuario.Email;


                    if (matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito || matriculaOferta.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)
                    {
                        assuntoDoEmail = assuntoDoEmail.Replace("#NOME_CURSO", matriculaOferta.NomeSolucaoEducacional);
                        corpoEmail = CorpoEmail(corpoEmail, matriculaOferta);

                        EmailUtil.Instancia.EnviarEmail(emailDoDestinatario.Trim(),
                                                            assuntoDoEmail, corpoEmail);
                    }
                }
                catch
                {
                    //Se der erro no envio de E-mail, pega o próximo aluno e tenta enviar o e-mail novamente
                    continue;
                    throw;
                }
            }

        }

        public string CorpoEmail(string corpoEmail, MatriculaOferta matriculaOferta)
        {
            corpoEmail = corpoEmail.Replace("#NOME_CURSO", matriculaOferta.NomeSolucaoEducacional);
            corpoEmail = corpoEmail.Replace("#NOME_ALUNO", (string.IsNullOrEmpty(matriculaOferta.Usuario.NomeExibicao) ? matriculaOferta.Usuario.Nome : matriculaOferta.Usuario.NomeExibicao));
            corpoEmail = corpoEmail.Replace("#DATA_INSCRICAO", matriculaOferta.DataSolicitacao.ToShortDateString());
            corpoEmail = corpoEmail.Replace("#DATA_TERMINO", matriculaOferta.DataConclusao);

            string lnk = string.Format("<a href=\"http://{0}\">{0}</a>", string.IsNullOrEmpty(matriculaOferta.LinkAcesso) ? "www.uc.sebrae.com.br" : matriculaOferta.LinkAcesso.Replace("http://", ""));

            corpoEmail = corpoEmail.Replace("#LINK_CONFIRMAR_INSCRICAO", lnk);

            int iRemove = corpoEmail.IndexOf("@#SEINSCRITOINI");
            int fRemove = (corpoEmail.IndexOf("@#SEINSCRITOFIM") - corpoEmail.IndexOf("@#SEINSCRITOINI"));

            if (matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
            {
                iRemove = corpoEmail.IndexOf("@#SEPENDENTEINI");
                fRemove = (corpoEmail.IndexOf("@#SEPENDENTEFIM") - corpoEmail.IndexOf("@#SEPENDENTEINI"));
            }

            if (iRemove < 0)
                iRemove = 0;

            if (fRemove < 0)
                fRemove = 0;

            corpoEmail = corpoEmail.Remove(iRemove, fRemove);

            corpoEmail = corpoEmail.Replace("@#SEINSCRITOINI", "");
            corpoEmail = corpoEmail.Replace("@#SEINSCRITOFIM", "");
            corpoEmail = corpoEmail.Replace("@#SEPENDENTEINI", "");
            corpoEmail = corpoEmail.Replace("@#SEPENDENTEFIM", "");

            return corpoEmail;
        }

        public bool SincronizarOfertaComMoodle(Oferta oferta, SolucaoEducacional solucaoEducacional)
        {
            try
            {
                // Verificar dados nas tabelas 'int_sgus_moodle_ofertas' e 'int_sgus_moodle_cursos'
                var bmSgusMoodleOferta = new BMSgusMoodleOferta();
                var bmSgusMoodleCurso = new BMSgusMoodleCurso();

                int IDCategoriaMoodle = int.Parse(oferta.SolucaoEducacional.IDChaveExterna);
                int IDCursoMoodle = oferta.CodigoMoodle.Value;

                var cursoMoodle = bmSgusMoodleCurso.ObterPorCategoria(IDCategoriaMoodle);

                if (cursoMoodle == null)
                {
                    cursoMoodle = new SgusMoodleCurso()
                    {
                        Nome = oferta.SolucaoEducacional.Nome,
                        CodigoCategoria = IDCategoriaMoodle,
                        CodigoCurso = IDCursoMoodle,
                        DataCriacao = DateTime.Now,
                        DataAtualizacao = DateTime.Now
                    };
                }
                else
                {
                    cursoMoodle.Nome = oferta.SolucaoEducacional.Nome;
                    cursoMoodle.CodigoCurso = IDCursoMoodle;
                    cursoMoodle.DataAtualizacao = DateTime.Now;
                }

                bmSgusMoodleCurso.Cadastrar(cursoMoodle);


                var ofertaMoodle = bmSgusMoodleOferta.ObterPorCodigoCurso(IDCursoMoodle);

                if (ofertaMoodle == null)
                {
                    ofertaMoodle = new SgusMoodleOferta()
                    {
                        Nome = oferta.Nome,
                        CodigoCategoria = IDCategoriaMoodle,
                        CodigoCurso = IDCursoMoodle,
                        DataCriacao = DateTime.Now,
                        Desabilitado = 1
                    };
                }
                else
                {
                    ofertaMoodle.Nome = oferta.Nome;
                    ofertaMoodle.CodigoCategoria = IDCategoriaMoodle;
                    ofertaMoodle.CodigoCurso = IDCursoMoodle;
                    ofertaMoodle.DataCriacao = DateTime.Now;
                }

                bmSgusMoodleOferta.Cadastrar(ofertaMoodle);

                int? IDChaveExternaOferta = bmSgusMoodleOferta.ObterPorCodigoCurso(IDCursoMoodle).ID;

                if (IDChaveExternaOferta.HasValue && IDChaveExternaOferta.Value > 0)
                {
                    oferta.IDChaveExterna = IDChaveExternaOferta.ToString();
                    AlterarOferta(oferta);
                }

                // Remover o ID da Chave Externa de todas as ofertas anteriores
                foreach (var ofertaAnterior in solucaoEducacional.ListaOferta.Where(o => o.DataInicioInscricoes < oferta.DataInicioInscricoes))
                {
                    ofertaAnterior.IDChaveExterna = null;
                    AlterarOferta(ofertaAnterior);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public void AtualizarPermissoesSolucaoVinculada(SolucaoEducacional solucao)
        {
            IList<OfertaPermissao> ofertaPermissao = new List<OfertaPermissao>();
            foreach (var item in solucao.ListaPermissao)
            {
                ofertaPermissao.Add(new OfertaPermissao()
                {
                    NivelOcupacional = item.NivelOcupacional,
                    Perfil = item.Perfil,
                    QuantidadeVagasPorEstado = item.QuantidadeVagasPorEstado,
                    Uf = item.Uf
                });
            }

            foreach (var oferta in solucao.ListaOferta)
            {
                foreach (var permissao in ofertaPermissao)
                {
                    permissao.Oferta = oferta;
                }

                oferta.ListaPermissao = ofertaPermissao;

                try
                {
                    bmOferta.Salvar(oferta);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}