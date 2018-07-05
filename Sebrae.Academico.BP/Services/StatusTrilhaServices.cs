//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System;
//using Sebrae.Academico.BM.Classes;
//using Sebrae.Academico.BM.Views;
//using Sebrae.Academico.BP.DTO.Services.StatusTrilha;
//using Sebrae.Academico.Dominio.Classes;
//using Sebrae.Academico.Dominio.Enumeracao;
//using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
//using Sebrae.Academico.Util.Classes;
//using Sebrae.Academico.Dominio.DTO;
//using Sebrae.Academico.BP.Relatorios;
//using Sebrae.Academico.BP.DTO.Services;
//using Sebrae.Academico.BP.DTO.Dominio;


//namespace Sebrae.Academico.BP.Services
//{
//    public class StatusTrilhaServices : BusinessProcessServicesBase
//    {
//        //private BMItemTrilha itemTrilha;
//        private BMViewTrilha viewTrilha;
//        //private BMUsuarioTrilha usuarioTrilha;

//        public RetornoStatusTrilha ConsultarStatusTrilha(int Id_Trilha, int Id_Usuario, int Id_TrilhaNivel, bool superAcesso = true)
//        {
//            RetornoStatusTrilha resultado = new RetornoStatusTrilha();

//            viewTrilha = new BMViewTrilha();

//            var bmUsuario = new BMUsuario();

//            Usuario usuario_web_service = bmUsuario.ObterPorId(Id_Usuario);

//            ViewTrilha viewTrilhaFiltro = new ViewTrilha()
//            {
//                TrilhaOrigem = Id_Trilha == 0 ? null : new BMTrilha().ObterPorId(Id_Trilha),
//                TrilhaNivelOrigem = Id_TrilhaNivel == 0 ? null : new BMTrilhaNivel().ObterPorID(Id_TrilhaNivel),
//                UsuarioOrigem = usuario_web_service
//            };

//            var listaMatriculaUsuario = (new BMUsuarioTrilha()).ObterPorFiltro(new UsuarioTrilha()
//            {
//                Usuario = new Usuario { ID = Id_Usuario }
//            }).Where(x =>
//                    x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
//                    x.StatusMatricula != enumStatusMatricula.CanceladoAluno && 
//                    x.StatusMatricula != enumStatusMatricula.Abandono &&
//                    x.StatusMatricula != enumStatusMatricula.NaoAprovado)
//            .OrderByDescending(x => x.DataInicio).ToList();

//            // Verifica limite de conclusão da trilha por usuário.
//            // retirar status aprovado neste caso
//            if (listaMatriculaUsuario.Any())
//            {
//                VerificarLimiteDeConlusaoTrilha(listaMatriculaUsuario.Where(p => p.StatusMatricula != enumStatusMatricula.Aprovado).ToList());
//            }

//            IList<DTOTrilhaNivelPermissao> ListaTrilhaNivelPermissao = null;

//            if (Id_Usuario > 0)
//            {
//                ListaTrilhaNivelPermissao = new BMTrilhaNivel().ObterListaDePermissoes(Id_Usuario, superAcesso);
//            }

//            // Caso tenha permissão de acesso a pelo menos um nível de trilha
//            if (ListaTrilhaNivelPermissao != null && ListaTrilhaNivelPermissao.Any())
//            {

//                List<int> niveisPermitidos = new List<int>();

//                foreach (var TrilhaNivelPermissao in ListaTrilhaNivelPermissao)
//                {
//                    if (niveisPermitidos == null || niveisPermitidos.IndexOf(TrilhaNivelPermissao.TrilhaNivel.ID) == -1)
//                    {
//                        niveisPermitidos.Add(TrilhaNivelPermissao.TrilhaNivel.ID);
//                    }
//                }

//                IList<ViewTrilha> lstView = viewTrilha.ObterViewTrilhaPorFiltro(viewTrilhaFiltro).Where(e => niveisPermitidos.Contains(e.TrilhaNivelOrigem.ID)).OrderBy(x => x.TrilhaOrigem.ID).OrderBy(x => x.TrilhaNivelOrigem.ID).OrderBy(x => x.TopicoTematico.ID).ToList();

//                ConsultarMeusCursos consultarMeusCursos = new ConsultarMeusCursos();

//                // Por padrão estamos usando sempre como fornecedor de Base de apoio para Trilha o Moodle
//                Fornecedor fornecedorMoodle = new BMFornecedor().ObterPorID(2);

//                foreach (Trilha vt in lstView.Select(e => e.TrilhaOrigem).Distinct())
//                {
//                    TrilhaDTO dtoTrilha = new TrilhaDTO();
//                    dtoTrilha.Nome = vt.Nome;
//                    dtoTrilha.NomeExtendido = vt.NomeEstendido;
//                    dtoTrilha.Descricao = vt.Descricao;
//                    //dtoTrilha.Imagem = vt.Imagem;
//                    dtoTrilha.Id = vt.ID;

//                    if (vt.ID_CodigoMoodle != null)
//                    {
//                        dtoTrilha.LinkComunidade = consultarMeusCursos.ConsultarLinkAcessoFornecedor(fornecedorMoodle, usuario_web_service, vt.ID_CodigoMoodle.ToString());
//                    }

//                    dtoTrilha.EmailTutor = vt.EmailTutor;

//                    var lstviewTn = lstView.Where(x => x.TrilhaOrigem.ID == vt.ID).Select(x => x.TrilhaNivelOrigem).Distinct().OrderBy(x => x.ValorOrdem).ToList();

//                    #region Trilha Nivel

//                    foreach (TrilhaNivel trilhaNivel in lstviewTn)
//                    {
//                        UsuarioTrilha usuarioTrilha = null;

//                        if (ListaTrilhaNivelPermissao != null)
//                        {
//                            //Verifica se o usuário possui acesso a esta trilha
//                            if (!ListaTrilhaNivelPermissao.Any(x => x.TrilhaNivel.ID == trilhaNivel.ID))
//                            {
//                                if (!(listaMatriculaUsuario != null && listaMatriculaUsuario.Any(x => x.TrilhaNivel.ID == trilhaNivel.ID)))
//                                {
//                                    continue;
//                                }
//                            }
//                        }

//                        TrilhaNivelDTO dtoTrilhaNivel = new TrilhaNivelDTO();
//                        dtoTrilhaNivel.ID = trilhaNivel.ID;
//                        dtoTrilhaNivel.Nome = trilhaNivel.Nome;
//                        dtoTrilhaNivel.Ordem = trilhaNivel.ValorOrdem;

//                        bool existeCertificado = (trilhaNivel.CertificadoTemplate != null);

//                        if (trilhaNivel.PreRequisito != null)
//                        {
//                            dtoTrilhaNivel.NomePreReq = trilhaNivel.PreRequisito.Nome;
//                            dtoTrilhaNivel.IdPreReq = trilhaNivel.PreRequisito.ID;
//                            dtoTrilhaNivel.HabilitadoMatricula = listaMatriculaUsuario.Any(x => x.TrilhaNivel.ID == trilhaNivel.PreRequisito.ID && x.StatusMatricula == enumStatusMatricula.Aprovado);
//                        }
//                        else
//                        {
//                            dtoTrilhaNivel.HabilitadoMatricula = true;
//                        }

//                        if (trilhaNivel.AceitaNovasMatriculas.HasValue && !trilhaNivel.AceitaNovasMatriculas.Value)
//                        {
//                            dtoTrilhaNivel.HabilitadoMatricula = false;
//                        }

//                        var listaItemTrilha = trilhaNivel.ListaItemTrilha.Where(p => p.Ativo == true).ToList();

//                        dtoTrilhaNivel.DiasPrazo = trilhaNivel.QuantidadeDiasPrazo;
//                        dtoTrilhaNivel.NotaMinima = trilhaNivel.NotaMinima;
//                        dtoTrilhaNivel.TermoAceite = trilhaNivel.TextoTermoAceite;
//                        dtoTrilhaNivel.QtdEstrelasPossiveis = lstView.Where(x => x.TrilhaOrigem.ID == vt.ID && x.TrilhaNivelOrigem.ID == trilhaNivel.ID && x.UsuarioOrigem == null).Select(x => x.TopicoTematico).Distinct().Count() * 2;
//                        dtoTrilhaNivel.QtdEstrelas = 0;
//                        dtoTrilhaNivel.CargaHoraria = trilhaNivel.CargaHoraria;
//                        dtoTrilhaNivel.QuantidadeSolucoesUC = listaItemTrilha.Count(x => x.Usuario == null);
//                        dtoTrilhaNivel.QuantidadeSolucoesAI = listaItemTrilha.Count(x => x.Usuario != null);
//                        dtoTrilhaNivel.Descricao = trilhaNivel.Descricao;
//                        dtoTrilhaNivel.HabilitaCancelamento = 0;

//                        if (listaMatriculaUsuario != null && listaMatriculaUsuario.Any(x => x.TrilhaNivel.ID == trilhaNivel.ID))
//                        {
//                            usuarioTrilha = listaMatriculaUsuario.FirstOrDefault(x => x.TrilhaNivel.ID == trilhaNivel.ID);


//                            dtoTrilhaNivel.IdUsuarioTrilha = usuarioTrilha.ID;
//                            dtoTrilhaNivel.UsuarioTrilha = usuarioTrilha.ID;
//                            dtoTrilhaNivel.StatusNivel = usuarioTrilha.StatusMatricula.ToString().ToUpper();
//                            dtoTrilhaNivel.HabilitadoAcesso = !usuarioTrilha.AcessoBloqueado;
//                            dtoTrilhaNivel.HabilitadoMatricula = false;
//                            dtoTrilhaNivel.NotaObtida = usuarioTrilha.NotaProva;
//                            dtoTrilhaNivel.DataLimite = usuarioTrilha.DataLimite.ToString("dd/MM/yyyy");
//                            dtoTrilhaNivel.NovaProvaLiberada = usuarioTrilha.NovaProvaLiberada;

//                            if (usuarioTrilha.NovaProvaLiberada && usuarioTrilha.DataLiberacaoNovaProva.HasValue)
//                                dtoTrilhaNivel.DataLiberacaoNovaProva = usuarioTrilha.DataLiberacaoNovaProva;

//                            dtoTrilhaNivel.QuantidadeSolucoesUCConcluidas = listaItemTrilha.Count(x => x.Usuario == null && x.ListaItemTrilhaParticipacao != null && x.ListaItemTrilhaParticipacao.Any(y => y.UsuarioTrilha.ID == dtoTrilhaNivel.UsuarioTrilha && y.Autorizado.HasValue && y.Autorizado.Value == true && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro));
//                            dtoTrilhaNivel.QuantidadeSolucoesAIConcluidas = listaItemTrilha.Count(x => x.Usuario != null && x.ListaItemTrilhaParticipacao != null && x.ListaItemTrilhaParticipacao.Any(y => y.UsuarioTrilha.ID == dtoTrilhaNivel.UsuarioTrilha && y.Autorizado.HasValue && y.Autorizado.Value == true && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro));

//                            // Verifica se o Aluno pode realizar o cancelamento da matrícula
//                            if (usuarioTrilha.StatusMatricula.Equals(enumStatusMatricula.Inscrito))
//                            {
//                                try
//                                {
//                                    int diasCancelamento = int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro.ToString());
//                                    if (usuarioTrilha.DataInicio.Date.AddDays(diasCancelamento) >= DateTime.Now)
//                                    {
//                                        dtoTrilhaNivel.HabilitaCancelamento = 1;
//                                    }
//                                    else
//                                    {
//                                        dtoTrilhaNivel.HabilitaCancelamento = 0;
//                                    }
//                                }
//                                catch
//                                {
//                                    dtoTrilhaNivel.HabilitaCancelamento = 0;
//                                }
//                            }
//                        }


//                        if (trilhaNivel.ListaQuestionarioAssociacao.Count > 0)
//                        {
//                            TratarQuestionariosTrilha(Id_Usuario, trilhaNivel, dtoTrilhaNivel);
//                        }

//                        #endregion

//                        #region Tópicos temáticos

//                        IList<TrilhaTopicoTematico> lstviewTP = lstView.Where(x => x.TrilhaOrigem.ID == vt.ID && x.TrilhaNivelOrigem.ID == trilhaNivel.ID && x.UsuarioOrigem == null).Select(x => x.TopicoTematico).Distinct().ToList();
//                        if (usuarioTrilha != null)
//                        {
//                            foreach (TrilhaTopicoTematico tp in lstviewTP)
//                            {
//                                IList<IndiceImportancia> ListaIndice = new BMIndiceImportancia().ObterPorFiltro(new IndiceImportancia() { Usuario = usuarioTrilha.Usuario, TrilhaNivel = trilhaNivel, TrilhaTopicoTematico = tp, Pre = true });
//                                if (ListaIndice.Count > 0)
//                                {
//                                    if (ListaIndice.Count == 1)
//                                        tp.ValorImportancia = ListaIndice.FirstOrDefault().ValorImportancia;
//                                    else
//                                        tp.ValorImportancia = ListaIndice.Average(x => x.ValorImportancia);
//                                }
//                            }

//                            // Monta exibição dos Tópicos temáticos do nivel da trilha baseado no seu valor de importância
//                            lstviewTP = lstviewTP.OrderByDescending(y => y.ValorImportancia).ToList();
//                        }

//                        foreach (TrilhaTopicoTematico tp in lstviewTP)
//                        {
//                            TrilhaTopicoTematicoDTO dtoTrilhaTopicoTematico = new TrilhaTopicoTematicoDTO();

//                            if (usuarioTrilha != null)
//                            {
//                                List<TrilhaAtividadeFormativaParticipacao> listaAtv = null;
//                                listaAtv = (new BMTrilhaAtividadeFormativaParticipacao()).ObterTrilhaAtividadeFormativaParticipacaoPorFiltro(new TrilhaAtividadeFormativaParticipacao()
//                                {
//                                    UsuarioTrilha = usuarioTrilha,
//                                    TrilhaTopicoTematico = new TrilhaTopicoTematico() { ID = tp.ID }
//                                }).ToList();

//                                if (listaAtv != null && listaAtv.Count > 0)
//                                {
//                                    foreach (var participacao in listaAtv)
//                                    {
//                                        AtividadeFormativaDTO registroDTO = new AtividadeFormativaDTO();
//                                        if (participacao.FileServer != null && participacao.FileServer.ID > 0)
//                                        {
//                                            registroDTO.ArquivoParticipacao = participacao.FileServer.NomeDoArquivoOriginal;
//                                            registroDTO.NomeCaminhoArquivoParticipacao = participacao.FileServer.NomeDoArquivoNoServidor;
//                                            registroDTO.TipoArquivoParticipacao = participacao.FileServer.TipoArquivo;
//                                            registroDTO.LinkAnexo = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + participacao.FileServer.NomeDoArquivoNoServidor;
//                                        }
//                                        registroDTO.ID = participacao.ID;
//                                        registroDTO.TextoParticipacao = participacao.TextoParticipacao;
//                                        registroDTO.DataEnvio = participacao.DataEnvio.ToString("dd/MM/yyyy H:mm");
//                                        registroDTO.TipoParticipacao = (int)participacao.TipoParticipacao;
//                                        registroDTO.Status = FormatarStatusTrilha(participacao.Autorizado);
//                                        if (participacao.Visualizado.HasValue)
//                                        {
//                                            registroDTO.Visualizado = participacao.Visualizado.Value;
//                                        }
//                                        if (participacao.Monitor != null)
//                                        {
//                                            registroDTO.Monitor = participacao.Monitor.Nome;
//                                            registroDTO.AutorParticipacao = registroDTO.Monitor;
//                                            if (string.IsNullOrEmpty(registroDTO.TextoParticipacao) && participacao.Autorizado.HasValue && participacao.Autorizado.Value)
//                                            {
//                                                registroDTO.TextoParticipacao = "Aprovado pelo monitor";
//                                            }
//                                        }
//                                        else
//                                        {
//                                            registroDTO.Visualizado = true; //Se for do usuário já visualizou
//                                            registroDTO.AutorParticipacao = participacao.UsuarioTrilha.Usuario.Nome;
//                                        }

//                                        dtoTrilhaTopicoTematico.ListaAtividadeFormativa.Add(registroDTO);
//                                    }
//                                    ControlarVisibilidadeObjetosTopicoTematico(dtoTrilhaTopicoTematico);
//                                }

//                            }

//                            dtoTrilhaTopicoTematico.ID = tp.ID;
//                            dtoTrilhaTopicoTematico.Nome = tp.NomeExibicao;
//                            dtoTrilhaTopicoTematico.AtividadeFormativaLabelTexto = tp.DescricaoTextoEnvio;
//                            dtoTrilhaTopicoTematico.AtividadeFormativaLabelArquivo = tp.DescricaoArquivoEnvio;

//                            if (tp.FileServer != null)
//                                dtoTrilhaTopicoTematico.AtividadeFormativaArquivoInstrucoes = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + tp.FileServer.NomeDoArquivoNoServidor;

//                            IList<ItemTrilha> lstviewIT = lstView.Where(x => x.TrilhaOrigem.ID == vt.ID && x.TrilhaNivelOrigem.ID == trilhaNivel.ID && x.TopicoTematico.ID == tp.ID)
//                                .Select(x => x.ItemTrilha).Distinct().Where(p => p.Ativo == true).ToList();

//                            if (lstviewIT != null && lstviewIT.Count() > 0)
//                            {
//                                double QtdItens = lstviewIT.Count();
//                                double QtdItensUC = lstviewIT.Where(x => x.Usuario == null).Count();
//                                double QtdItensAI = lstviewIT.Where(x => x.Usuario != null).Count();
//                                double QtdItensUCConcluidos = 0;
//                                double QtdItensAIConcluidos = 0;

//                                //int qtdItensAIConcluidosRecalculado = 0;

//                                if (usuarioTrilha != null)
//                                {
//                                    QtdItensUCConcluidos = lstviewIT.Where(x => x.Usuario == null && x.ListaItemTrilhaParticipacao.Any(y => y.UsuarioTrilha.ID == usuarioTrilha.ID && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Autorizado.HasValue && y.Autorizado == true)).Count();
//                                    QtdItensAIConcluidos = lstviewIT.Where(x => x.Usuario != null && x.ListaItemTrilhaParticipacao.Any(y => y.UsuarioTrilha.ID == usuarioTrilha.ID && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Autorizado.HasValue && y.Autorizado == true)).Count();

//                                    //List<ItemTrilhaParticipacao> listaItemTrilhaParticipacao = new BMItemTrilhaParticipacao().ObterItemTrilhaParticipacaoPorUsuario(usuarioTrilha.ID).ToList();
//                                    //if (listaItemTrilhaParticipacao != null && listaItemTrilhaParticipacao.Count() > 0)
//                                    //{
//                                    //    qtdItensAIConcluidosRecalculado = listaItemTrilhaParticipacao.Count(f => f.ItemTrilha.TrilhaNivel.ID == trilhaNivel.ID
//                                    //                                                                            && f.UsuarioTrilha.ID == usuarioTrilha.ID
//                                    //                                                                            && f.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro
//                                    //                                                                            && f.Autorizado.HasValue
//                                    //                                                                            && f.Autorizado.Value == true
//                                    //                                                                            && f.ItemTrilha.TrilhaTopicoTematico.ID == tp.ID);


//                                    //    QtdItensAIConcluidos = (qtdItensAIConcluidosRecalculado - (QtdItensUCConcluidos > 0 ? QtdItensUCConcluidos : 0));
//                                    //}
//                                }

//                                dtoTrilhaTopicoTematico.PercentualConclusaoAI = QtdItensAIConcluidos == 0 ? 0 : QtdItensAIConcluidos / QtdItensUC;
//                                dtoTrilhaTopicoTematico.PercentualConclusaoUC = QtdItensUCConcluidos == 0 ? 0 : QtdItensUCConcluidos / QtdItensUC;

//                                dtoTrilhaTopicoTematico.TemEstrelaAI = QtdItensAIConcluidos == QtdItensUC;
//                                dtoTrilhaTopicoTematico.TemEstrelaUC = QtdItensUCConcluidos == QtdItensUC;

//                                if (dtoTrilhaTopicoTematico.TemEstrelaAI)
//                                    dtoTrilhaNivel.QtdEstrelas++;
//                                if (dtoTrilhaTopicoTematico.TemEstrelaUC)
//                                    dtoTrilhaNivel.QtdEstrelas++;

//                                if (QtdItensAI < QtdItensUC)
//                                    dtoTrilhaTopicoTematico.HabilitaCadastroSolucaoAI = true;

//                                dtoTrilhaTopicoTematico.HabilitaSolucaoUC = lstviewIT.Any(x => x.Usuario == null);
//                                dtoTrilhaTopicoTematico.HabilitaSolucaoAI = lstviewIT.Any(x => x.Usuario != null);
//                            }

//                            foreach (ItemTrilha it in lstviewIT)
//                            {
//                                if (it.Ativo != null && !(bool)it.Ativo)
//                                {
//                                    continue;
//                                }

//                                ItemTrilhaDTO dtoItemTrilha = preencherObjetoItemTrilha(it, usuarioTrilha);
//                                if (it.Usuario == null)
//                                {
//                                    if (!dtoTrilhaTopicoTematico.ListaItemTrilhaUC.Any(p => p.ID == dtoItemTrilha.ID)) dtoTrilhaTopicoTematico.ListaItemTrilhaUC.Add(dtoItemTrilha);
//                                }
//                                else if (!dtoTrilhaTopicoTematico.ListaItemTrilhaAI.Any(p => p.ID == dtoItemTrilha.ID))
//                                {
//                                    dtoTrilhaTopicoTematico.ListaItemTrilhaAI.Add(dtoItemTrilha);
//                                }

//                            }
//                            if (usuarioTrilha != null)
//                            {
//                                ItemTrilhaParticipacao itTrilhaParticipacao = new ItemTrilhaParticipacao();
//                                itTrilhaParticipacao.UsuarioTrilha = usuarioTrilha;
//                                IList<ItemTrilhaParticipacao> lstItemTrilhaParticipacao = new BMItemTrilhaParticipacao().ObterItemTrilhaParticipacaoPorFiltro(itTrilhaParticipacao).Where(x => x.ItemTrilha.Usuario != null && x.ItemTrilha.Usuario.ID != usuarioTrilha.Usuario.ID).ToList();
//                                if (lstItemTrilhaParticipacao != null)
//                                {
//                                    foreach (ItemTrilhaParticipacao itTrilhaPart in lstItemTrilhaParticipacao)
//                                    {
//                                        ItemTrilhaDTO dtoItemTrilhaParticipaSugerida = preencherObjetoItemTrilha(itTrilhaPart.ItemTrilha, usuarioTrilha);
//                                        if (dtoTrilhaTopicoTematico.ID == itTrilhaPart.ItemTrilha.TrilhaTopicoTematico.ID)
//                                        {
//                                            if (!dtoTrilhaTopicoTematico.ListaItemTrilhaAI.Any(p => p.ID == dtoItemTrilhaParticipaSugerida.ID))
//                                            {
//                                                dtoTrilhaTopicoTematico.ListaItemTrilhaAI.Add(dtoItemTrilhaParticipaSugerida);
//                                            }
//                                        }
//                                    }
//                                }
//                            }

//                            //dtoTrilhaTopicoTematico.HabilitaCadastroAtividadeFormativa = HabilitaAtividadeFormativa(tp, new BMUsuario().ObterPorId(Id_Usuario));
//                            if (dtoTrilhaTopicoTematico.ListaItemTrilhaUC.Any(x => x.ListaItemTrilhaParticipacao.Any(y => y.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Status == "A")))
//                            {
//                                if (usuarioTrilha != null)
//                                {
//                                    var relatorioPartTrilha = new RelatorioParticipacaoTrilha();
//                                    //var realizouPeloMenosUmObjetivoDeCada = relatorioPartTrilha.verificarQuantidadeDeObjetivosConcluidos(dtoTrilhaNivel.ID, dtoTrilhaTopicoTematico.ID, usuarioTrilha.ID);

//                                    var result = dtoTrilhaTopicoTematico.ListaItemTrilhaUC.GroupBy(p => p.IDObjetivo, (key, g) => new
//                                    {
//                                        id = key,
//                                        totalAprovados = g.Count(a => a.StatusParticipacao == "A"),
//                                        habilitarEnviarSprint = (g.Count(s => s.SolucaoObrigatoria) == g.Count(s => s.SolucaoObrigatoria && s.Aprovado))
//                                    });
//                                    foreach (var item in result)
//                                    {
//                                        dtoTrilhaTopicoTematico.HabilitaCadastroAtividadeFormativa = item.totalAprovados >= 1 && item.habilitarEnviarSprint;
//                                    }
//                                }
//                            }
//                            dtoTrilhaTopicoTematico.QuantidadeNotificacoesAITopico = dtoTrilhaTopicoTematico.ListaItemTrilhaAI.Sum(x => x.QuantidadeNotificacoesAIItem);
//                            dtoTrilhaTopicoTematico.QuantidadeNotificacoesUCTopico = dtoTrilhaTopicoTematico.ListaItemTrilhaUC.Sum(x => x.QuantidadeNotificacoesUCItem);

//                            dtoTrilhaNivel.ListaTopicoTematico.Add(dtoTrilhaTopicoTematico);
//                        }

//                        #endregion

//                        // CALCULAR PERCENTUAL DE CONCLUSAO E VERIFICAR SE O USUARIO PODE FAZER A PROVA
//                        CalcularPercentualConclusaoTrilha(dtoTrilhaNivel);

//                        // VERIFICAR BOTOES
//                        VerificaStatusBotoes(dtoTrilhaNivel, existeCertificado);

//                        // VERIFICAR TROFEU
//                        if (dtoTrilhaNivel.StatusNivel == enumStatusMatricula.Aprovado.ToString().ToUpper())
//                        {
//                            dtoTrilhaNivel.NivelTrofeu = 1;
//                            int QtdNiveisCadastrados = dtoTrilhaNivel.ListaTopicoTematico.Count;
//                            int QtdNiveisComCemPorcento = 0;
//                            foreach (var item in dtoTrilhaNivel.ListaTopicoTematico)
//                            {
//                                if (item.ListaItemTrilhaUC.Where(x => x.ListaItemTrilhaParticipacao != null).Count() == item.ListaItemTrilhaUC.Count() &&
//                                    item.ListaItemTrilhaAI.Where(x => x.ListaItemTrilhaParticipacao != null).Count() == item.ListaItemTrilhaAI.Count() &&
//                                    item.ListaItemTrilhaUC.Count() == item.ListaItemTrilhaAI.Count())
//                                {
//                                    QtdNiveisComCemPorcento++;
//                                }
//                            }
//                            if (QtdNiveisComCemPorcento > 0)
//                            {
//                                if (QtdNiveisComCemPorcento == QtdNiveisCadastrados)
//                                {
//                                    dtoTrilhaNivel.NivelTrofeu = 3;
//                                }
//                                else
//                                {
//                                    dtoTrilhaNivel.NivelTrofeu = 2;
//                                }
//                            }
//                        }
//                        if (dtoTrilhaNivel.ListaTopicoTematico.Count > 0)
//                            dtoTrilha.ListaTrilhaNivel.Add(dtoTrilhaNivel);
//                    }
//                    if (dtoTrilha.ListaTrilhaNivel.Count > 0)
//                        resultado.ListaTrilhas.Add(dtoTrilha);
//                }
//            }

//            resultado.Retorno.Erro = 0;
//            return resultado;


//        }

//        public RetornoStatusTrilha ConsultarTrilhas(int Id_Usuario)
//        {
//            var resultado = new RetornoStatusTrilha();

//            viewTrilha = new BMViewTrilha();

//            var usuario = (new BMUsuario()).ObterPorId(Id_Usuario);

//            var viewTrilhaFiltro = new ViewTrilha()
//            {
//                UsuarioOrigem = usuario
//            };

//            IList<DTOTrilhaNivelPermissao> listaTrilhaNivelPermissao = null;

//            if (Id_Usuario > 0)
//            {
//                listaTrilhaNivelPermissao = new BMTrilhaNivel().ObterListaDePermissoes(Id_Usuario);
//            }

//            // Caso tenha permissão de acesso a pelo menos um nível de trilha
//            if (listaTrilhaNivelPermissao == null || !listaTrilhaNivelPermissao.Any()) return resultado;

//            var niveisPermitidos = new List<int>();

//            foreach (var trilhaNivelPermissao in listaTrilhaNivelPermissao.Where(trilhaNivelPermissao => niveisPermitidos.IndexOf(trilhaNivelPermissao.TrilhaNivel.ID) == -1))
//            {
//                niveisPermitidos.Add(trilhaNivelPermissao.TrilhaNivel.ID);
//            }

//            IList<ViewTrilha> lstView = viewTrilha.ObterViewTrilhaPorFiltro(viewTrilhaFiltro)
//                    .Where(e => niveisPermitidos.Contains(e.TrilhaNivelOrigem.ID))
//                    .OrderBy(x => x.TrilhaOrigem.ID)
//                    .ThenBy(x => x.TrilhaNivelOrigem.ID)
//                    .ThenBy(x => x.TopicoTematico.ID)
//                    .ToList();

//            resultado.ListaTrilhas = lstView.Select(e => e.TrilhaOrigem).Distinct().Select(t => new TrilhaDTO
//            {
//                Nome = t.Nome,
//                NomeExtendido = t.NomeEstendido,
//                Descricao = t.Descricao,
//                Id = t.ID
//            }).ToList();


//            resultado.Retorno.Erro = 0;
//            return resultado;
//        }

//        private static void ControlarVisibilidadeObjetosTopicoTematico(TrilhaTopicoTematicoDTO registro)
//        {
//            if (registro.ListaAtividadeFormativa.Any())
//            {
//                if (registro.ListaAtividadeFormativa.Any(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro))
//                {
//                    registro.TemParticipacao = true;
//                    var participacaoMaisRecente = registro.ListaAtividadeFormativa.OrderByDescending(x => x.ID).FirstOrDefault(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro);
//                    if (participacaoMaisRecente.Status == "A")
//                    {
//                        //Participacao aprovada
//                        registro.ParticipacaoAprovada = true;
//                        registro.StatusParticipacao = "A";
//                        // Se está aprovada não precisa preencher nada
//                        registro.HabilitaFormularioParticipacao = false;
//                        registro.HabilitaBotaoEnviarMensagem = false;
//                    }
//                    else if (participacaoMaisRecente.Status == "N")
//                    {
//                        //Participacao reprovada
//                        registro.HabilitaBotaoEnviarParticipacao = true;
//                        registro.HabilitaBotaoEnviarMensagem = true;
//                        registro.HabilitaFormularioParticipacao = true;
//                        registro.StatusParticipacao = "N";
//                    }
//                    else if (participacaoMaisRecente.Status == "P")
//                    {
//                        //Participacao pendente
//                        registro.HabilitaBotaoEnviarMensagem = false;
//                        registro.HabilitaFormularioParticipacao = false;
//                        registro.StatusParticipacao = "P";
//                    }
//                }
//            }
//            else
//            {
//                registro.HabilitaBotaoEnviarParticipacao = true;
//                registro.HabilitaFormularioParticipacao = true;
//            }
//        }


//        private static void ControlarVisibilidadeObjetosItemTrilha(ItemTrilhaDTO registro)
//        {
//            if (registro.ListaItemTrilhaParticipacao.Any())
//            {
//                if (registro.ListaItemTrilhaParticipacao.Any(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro))
//                {
//                    registro.TemParticipacao = true;
//                    var participacaoMaisRecente = registro.ListaItemTrilhaParticipacao.OrderByDescending(x => x.ID).FirstOrDefault(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro);
//                    if (participacaoMaisRecente.Status == "A")
//                    {
//                        //Participacao aprovada
//                        registro.ParticipacaoAprovada = true;
//                        registro.StatusParticipacao = "A";
//                    }
//                    else if (participacaoMaisRecente.Status == "N")
//                    {
//                        //Participacao reprovada
//                        registro.HabilitaBotaoEnviarParticipacao = true;
//                        registro.HabilitaBotaoEnviarMensagem = true;
//                        registro.HabilitaFormularioParticipacao = true;
//                        registro.StatusParticipacao = "N";
//                    }
//                    else if (participacaoMaisRecente.Status == "P")
//                    {
//                        //Participacao pendente
//                        registro.HabilitaBotaoEnviarMensagem = false;
//                        registro.HabilitaFormularioParticipacao = false;
//                        registro.StatusParticipacao = "P";
//                    }
//                }
//            }
//            else
//            {
//                registro.HabilitaBotaoEnviarParticipacao = true;
//                registro.HabilitaFormularioParticipacao = true;
//            }
//        }
//        private string FormatarStatusTrilha(bool? status)
//        {
//            if (status.HasValue)
//            {
//                if (status.Value)
//                {
//                    return "A";
//                }
//                else
//                {
//                    return "N";
//                }
//            }
//            else
//            {
//                return "P";
//            }
//        }

//        private static void TratarQuestionariosTrilha(int Id_Usuario, TrilhaNivel tn, TrilhaNivelDTO dtoTrilhaNivel)
//        {
//            //Questionario POS
//            QuestionarioAssociacao QuestionarioPos = tn.ListaQuestionarioAssociacao.FirstOrDefault(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos && !x.Evolutivo);
//            if (QuestionarioPos != null)
//            {
//                dtoTrilhaNivel.QuestionarioPos.Existe = true;
//                dtoTrilhaNivel.QuestionarioPos.Obrigatorio = QuestionarioPos.Obrigatorio;
//                dtoTrilhaNivel.QuestionarioPos.Tipo = QuestionarioPos.TipoQuestionarioAssociacao.Nome;
//                bool encontrou = tn.ListaQuestionarioParticipacao.Any(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos && x.DataParticipacao != null && x.Usuario.ID == Id_Usuario && !x.Evolutivo);
//                dtoTrilhaNivel.QuestionarioPos.Preenchido = encontrou;
//            }
//            //Questionario PRE
//            QuestionarioAssociacao QuestionarioPre = tn.ListaQuestionarioAssociacao.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre && !x.Evolutivo).FirstOrDefault();
//            if (QuestionarioPre != null)
//            {
//                dtoTrilhaNivel.QuestionarioPre.Existe = true;
//                dtoTrilhaNivel.QuestionarioPre.Obrigatorio = QuestionarioPre.Obrigatorio;
//                dtoTrilhaNivel.QuestionarioPre.Tipo = QuestionarioPre.TipoQuestionarioAssociacao.Nome;
//                bool encontrou = tn.ListaQuestionarioParticipacao.Any(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre && x.DataParticipacao != null && x.Usuario.ID == Id_Usuario && !x.Evolutivo);
//                dtoTrilhaNivel.QuestionarioPre.Preenchido = encontrou;
//            }
//            //Questionario Evolutivo Pre
//            QuestionarioAssociacao QuestionarioEvolutivoPre = tn.ListaQuestionarioAssociacao.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre && x.Evolutivo).FirstOrDefault();
//            if (QuestionarioEvolutivoPre != null)
//            {
//                dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe = true;
//                dtoTrilhaNivel.QuestionarioEvolutivoPre.Obrigatorio = true;
//                dtoTrilhaNivel.QuestionarioEvolutivoPre.Tipo = QuestionarioEvolutivoPre.TipoQuestionarioAssociacao.Nome;
//                bool encontrou = tn.ListaQuestionarioParticipacao.Any(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre && x.DataParticipacao != null && x.Usuario.ID == Id_Usuario && x.Evolutivo);
//                dtoTrilhaNivel.QuestionarioEvolutivoPre.Preenchido = encontrou;
//            }
//            //Questionario Evolutivo Pos
//            QuestionarioAssociacao QuestionarioEvolutivoPos = tn.ListaQuestionarioAssociacao.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos && x.Evolutivo).FirstOrDefault();
//            if (QuestionarioEvolutivoPos != null)
//            {
//                dtoTrilhaNivel.QuestionarioEvolutivoPos.Existe = true;
//                dtoTrilhaNivel.QuestionarioEvolutivoPos.Obrigatorio = true;
//                dtoTrilhaNivel.QuestionarioEvolutivoPos.Tipo = QuestionarioEvolutivoPos.TipoQuestionarioAssociacao.Nome;
//                bool encontrou = tn.ListaQuestionarioParticipacao.Any(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos && x.DataParticipacao != null && x.Usuario.ID == Id_Usuario && x.Evolutivo);
//                dtoTrilhaNivel.QuestionarioEvolutivoPos.Preenchido = encontrou;
//            }
//        }

//        private static void CalcularPercentualConclusaoTrilha(TrilhaNivelDTO dtoTrilhaNivel)
//        {
//            try
//            {
//                int QtdTopicosLiberadosProva = 0; decimal PercentualCadaTopico = dtoTrilhaNivel.ListaTopicoTematico.Count > 0 ? 100 / dtoTrilhaNivel.ListaTopicoTematico.Count : 0;
//                dtoTrilhaNivel.QuantidadeTopicos = dtoTrilhaNivel.ListaTopicoTematico.Count;
//                dtoTrilhaNivel.QuantidadeTopicosConcluidos = 0;

//                foreach (var topicoTematico in dtoTrilhaNivel.ListaTopicoTematico)
//                {
//                    if (topicoTematico.ListaAtividadeFormativa.Any(x => x.Status == "A" && x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro))
//                    {
//                        QtdTopicosLiberadosProva++;
//                        dtoTrilhaNivel.PercentualConclusao += PercentualCadaTopico;
//                        dtoTrilhaNivel.QuantidadeTopicosConcluidos++;
//                    }
//                    else
//                    {
//                        decimal PercentualCadaItemTrilha = topicoTematico.ListaItemTrilhaUC.Count > 0 ? PercentualCadaTopico / topicoTematico.ListaItemTrilhaUC.Count : 0;
//                        foreach (var ItemTrilha in topicoTematico.ListaItemTrilhaUC)
//                        {
//                            if (ItemTrilha.ListaItemTrilhaParticipacao.Any(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && x.Status == "A"))
//                            {
//                                dtoTrilhaNivel.PercentualConclusao += PercentualCadaItemTrilha;
//                            }
//                        }
//                    }
//                }
//                dtoTrilhaNivel.PercentualConclusao = decimal.Round(dtoTrilhaNivel.PercentualConclusao);
//                if (QtdTopicosLiberadosProva == dtoTrilhaNivel.ListaTopicoTematico.Count())
//                {
//                    dtoTrilhaNivel.HabilitaProvaTrilha = true;
//                }
//            }
//            catch
//            {
//            }
//        }

//        private ItemTrilhaDTO preencherObjetoItemTrilha(ItemTrilha objItemTrilha, UsuarioTrilha usuarioTrilha)
//        {
//            ItemTrilhaDTO dtoItemTrilha = new ItemTrilhaDTO();
//            dtoItemTrilha.FormaAquisicao = objItemTrilha.FormaAquisicao.Nome;
//            dtoItemTrilha.FormaAquisicaoImagem = objItemTrilha.FormaAquisicao.Imagem;
//            dtoItemTrilha.ID = objItemTrilha.ID;
//            dtoItemTrilha.Nome = objItemTrilha.Nome;
//            dtoItemTrilha.Aprovado = objItemTrilha.Aprovado == enumStatusSolucaoEducacionalSugerida.Aprovado;
//            dtoItemTrilha.AprovadoTexto = objItemTrilha.AprovadoStatus;
//            dtoItemTrilha.QuantidadePontosParticipacao = objItemTrilha.QuantidadePontosParticipacao;
//            dtoItemTrilha.StatusAprovacao = objItemTrilha.AprovadoStatus;
//            if (objItemTrilha.Objetivo != null)
//            {
//                dtoItemTrilha.Objetivo = objItemTrilha.Objetivo.Nome;
//                dtoItemTrilha.IDObjetivo = objItemTrilha.Objetivo.ID;
//            }

//            if (objItemTrilha.FileServer != null && objItemTrilha.FileServer.ID > 0)
//            {
//                dtoItemTrilha.CaminhoAnexo = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + "/ExibirFileServer.ashx?Identificador=" + objItemTrilha.FileServer.NomeDoArquivoNoServidor;
//            }
//            if (objItemTrilha.SolucaoEducacional != null)
//            {
//                dtoItemTrilha.SolucaoEducacional = new SolucaoEducacionalDTO
//                {
//                    Nome = objItemTrilha.SolucaoEducacional.Nome,
//                    NodeId = objItemTrilha.SolucaoEducacional.IdNode ?? 0,
//                    IdNodePortal = objItemTrilha.SolucaoEducacional.IdNodePortal ?? 0,
//                    SolucaoObrigatoria = objItemTrilha.SolucaoObrigatoria ?? false
//                };

//                if (objItemTrilha.SolucaoEducacional.ListaOferta.Any())
//                {
//                    dtoItemTrilha.CargaHoraria = objItemTrilha.SolucaoEducacional.ListaOferta.OrderByDescending(x => x.DataInicioInscricoes).FirstOrDefault().CargaHoraria.ToString();
//                }
//                else
//                {
//                    dtoItemTrilha.CargaHoraria = "N/D";
//                }
//            }
//            dtoItemTrilha.Local = objItemTrilha.Local;
//            dtoItemTrilha.LinkConteudo = objItemTrilha.LinkConteudo;
//            dtoItemTrilha.ReferenciaBibliografica = objItemTrilha.ReferenciaBibliografica;

//            if (usuarioTrilha != null)
//            {
//                List<ItemTrilhaParticipacao> listaItp = objItemTrilha.ListaItemTrilhaParticipacao.Where(s => s.UsuarioTrilha.ID == usuarioTrilha.ID).ToList();

//                if (listaItp != null)
//                {
//                    foreach (var itp in listaItp)
//                    {
//                        ItemTrilhaParticipacaoDTO registroDTO = new ItemTrilhaParticipacaoDTO();

//                        registroDTO.ID = itp.ID;
//                        registroDTO.DataEnvio = itp.DataEnvio.ToString("dd/MM/yyyy H:mm");
//                        registroDTO.TextoParticipacao = itp.TextoParticipacao;
//                        registroDTO.Orientacao = itp.Orientacao;
//                        registroDTO.TipoParticipacao = (int)itp.TipoParticipacao;

//                        if (itp.FileServer != null)
//                        {
//                            registroDTO.ArquivoParticipacao = itp.FileServer.NomeDoArquivoOriginal;
//                            registroDTO.NomeArquivoParticipacao = itp.FileServer.NomeDoArquivoNoServidor;
//                            registroDTO.TipoArquivoParticipacao = itp.FileServer.TipoArquivo;
//                            registroDTO.LinkAnexo = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + itp.FileServer.NomeDoArquivoNoServidor;
//                        }

//                        registroDTO.Status = FormatarStatusTrilha(itp.Autorizado);

//                        if (itp.Visualizado.HasValue)
//                        {
//                            registroDTO.Visualizado = itp.Visualizado.Value;
//                        }

//                        if (itp.Monitor != null)
//                        {
//                            registroDTO.Monitor = itp.Monitor.Nome;
//                            registroDTO.AutorParticipacao = registroDTO.Monitor;
//                            if (string.IsNullOrEmpty(registroDTO.TextoParticipacao) && itp.Autorizado.HasValue && itp.Autorizado.Value)
//                            {
//                                registroDTO.TextoParticipacao = "Aprovado pelo monitor";
//                            }
//                        }
//                        else
//                        {
//                            registroDTO.Visualizado = true; //Se for do próprio usuário já visualizou
//                            registroDTO.AutorParticipacao = itp.UsuarioTrilha.Usuario.Nome;
//                        }

//                        dtoItemTrilha.ListaItemTrilhaParticipacao.Add(registroDTO);
//                    }
//                    if (objItemTrilha.Usuario == null && dtoItemTrilha.ListaItemTrilhaParticipacao.Any(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.InteracaoMonitor && x.Visualizado == false))
//                        dtoItemTrilha.QuantidadeNotificacoesUCItem = 1;
//                    if (objItemTrilha.Usuario != null && dtoItemTrilha.ListaItemTrilhaParticipacao.Any(x => x.TipoParticipacao == (int)enumTipoParticipacaoTrilha.InteracaoMonitor && x.Visualizado == false))
//                        dtoItemTrilha.QuantidadeNotificacoesAIItem = 1;

//                    ControlarVisibilidadeObjetosItemTrilha(dtoItemTrilha);
//                }

//            }
//            return dtoItemTrilha;
//        }

//        private static void VerificaStatusBotoes(TrilhaNivelDTO dtoTrilhaNivel, bool existeCertificado)
//        {
//            try
//            {
//                dtoTrilhaNivel.StatusBtnCredenciamento = "B";
//                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "B";
//                dtoTrilhaNivel.StatusBtnBandeirada = "B";
//                dtoTrilhaNivel.StatusBtnDepoimento = "B";
//                dtoTrilhaNivel.StatusBtnCertificado = "B";
//                dtoTrilhaNivel.StatusBtnHistoricoPercurso = "B";

//                if (dtoTrilhaNivel.StatusNivel != null)
//                {
//                    if (dtoTrilhaNivel.StatusNivel.Equals(enumStatusMatricula.Inscrito.ToString().ToUpper()))
//                    {
//                        dtoTrilhaNivel.StatusBtnCredenciamento = "A";
//                        dtoTrilhaNivel.StatusBtnHistoricoPercurso = "AC";
//                        if (dtoTrilhaNivel.HabilitaProvaTrilha)
//                            dtoTrilhaNivel.StatusBtnBandeirada = "AC";

//                        if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe && dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Preenchido && dtoTrilhaNivel.QuestionarioPre.Preenchido)
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                            else
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "AC";
//                        }
//                        else if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe && !dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Preenchido)
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                            else
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "AC";
//                        }
//                        else if (!dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe && dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioPre.Preenchido)
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                            else
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "AC";
//                        }
//                        else
//                        {
//                            dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "B";
//                        }

//                    }
//                    if (dtoTrilhaNivel.StatusNivel.Equals(enumStatusMatricula.Aprovado.ToString().ToUpper()))
//                    {
//                        dtoTrilhaNivel.StatusBtnHistoricoPercurso = "AC";

//                        dtoTrilhaNivel.StatusBtnBandeirada = "A";

//                        if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe || dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                        }
//                        if (dtoTrilhaNivel.QuestionarioEvolutivoPos.Existe && dtoTrilhaNivel.QuestionarioPos.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioEvolutivoPos.Preenchido && dtoTrilhaNivel.QuestionarioPos.Preenchido)
//                            {
//                                if (existeCertificado)
//                                    dtoTrilhaNivel.StatusBtnCertificado = "AC";

//                                dtoTrilhaNivel.StatusBtnDepoimento = "A";
//                            }
//                            else
//                            {
//                                dtoTrilhaNivel.StatusBtnDepoimento = "AC";
//                            }
//                        }
//                        else if (dtoTrilhaNivel.QuestionarioEvolutivoPos.Existe && !dtoTrilhaNivel.QuestionarioPos.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioEvolutivoPos.Preenchido)
//                            {
//                                if (existeCertificado)
//                                    dtoTrilhaNivel.StatusBtnCertificado = "AC";

//                                dtoTrilhaNivel.StatusBtnDepoimento = "A";
//                            }
//                            else
//                            {
//                                dtoTrilhaNivel.StatusBtnDepoimento = "AC";
//                            }
//                        }
//                        else if (!dtoTrilhaNivel.QuestionarioEvolutivoPos.Existe && dtoTrilhaNivel.QuestionarioPos.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioPos.Preenchido)
//                            {
//                                if (existeCertificado)
//                                    dtoTrilhaNivel.StatusBtnCertificado = "AC";

//                                dtoTrilhaNivel.StatusBtnDepoimento = "A";
//                            }
//                            else
//                            {
//                                dtoTrilhaNivel.StatusBtnDepoimento = "AC";
//                            }
//                        }
//                        else
//                        {
//                            if (existeCertificado)
//                                dtoTrilhaNivel.StatusBtnCertificado = "AC";
//                        }
//                    }
//                    if (dtoTrilhaNivel.StatusNivel.Equals(enumStatusMatricula.NaoAprovado.ToString().ToUpper()))
//                    {
//                        dtoTrilhaNivel.StatusBtnCredenciamento = "A";
//                        dtoTrilhaNivel.StatusBtnHistoricoPercurso = "AC";

//                        // Se já puder realizar a nova prova ou se precisar clicar para requisitar uma nova
//                        if ((dtoTrilhaNivel.HabilitaProvaTrilha && dtoTrilhaNivel.DataLiberacaoNovaProva.HasValue && dtoTrilhaNivel.DataLiberacaoNovaProva.Value.Date <= DateTime.Today) || !dtoTrilhaNivel.NovaProvaLiberada)
//                        {
//                            dtoTrilhaNivel.StatusBtnBandeirada = "AC";
//                        }

//                        if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe && dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Preenchido && dtoTrilhaNivel.QuestionarioPre.Preenchido)
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                            else
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "AC";
//                        }
//                        else if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe && !dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioEvolutivoPre.Preenchido)
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                            else
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "AC";
//                        }
//                        else if (!dtoTrilhaNivel.QuestionarioEvolutivoPre.Existe && dtoTrilhaNivel.QuestionarioPre.Existe)
//                        {
//                            if (dtoTrilhaNivel.QuestionarioPre.Preenchido)
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "A";
//                            else
//                                dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "AC";
//                        }
//                        else
//                        {
//                            dtoTrilhaNivel.StatusBtnDiagnosticoEntrada = "B";
//                        }
//                    }
//                }
//                if (dtoTrilhaNivel.HabilitadoMatricula)
//                {
//                    dtoTrilhaNivel.StatusBtnCredenciamento = "AC";
//                }
//            }
//            catch
//            {
//            }
//        }

//        private bool HabilitaAtividadeFormativa(TrilhaTopicoTematico TrilhaTopicoTematico, Usuario Usuario)
//        {
//            if (TrilhaTopicoTematico.ListaItemTrilha.Where(x => x.ListaItemTrilhaParticipacao != null && x.ListaItemTrilhaParticipacao.Count() > 0 && x.ListaItemTrilhaParticipacao.Where(y => y.UsuarioTrilha.Usuario.ID == Usuario.ID).FirstOrDefault() != null).FirstOrDefault() == null)
//                return false;
//            else
//            {

//                int QuantidadeDePontos = TrilhaTopicoTematico.ListaItemTrilha.Where(x => x.TrilhaTopicoTematico.ID == TrilhaTopicoTematico.ID
//                                                        && x.Usuario == null
//                                                        && x.ListaItemTrilhaParticipacao != null
//                                                        && x.ListaItemTrilhaParticipacao.Count > 0
//                                                        && x.ListaItemTrilhaParticipacao.Where(y => y.UsuarioTrilha.Usuario.ID == Usuario.ID && y.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro && y.Autorizado.HasValue && y.Autorizado.Value == true).Count() > 0)
//                                              .Sum(x => x.QuantidadePontosParticipacao);

//                if (QuantidadeDePontos >= TrilhaTopicoTematico.QtdMinimaPontosAtivFormativa)
//                {
//                    if (TrilhaTopicoTematico.TrilhaAtividadeFormativaParticipacao != null)
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//                return false;
//            }

//        }

//        public List<DTOUsuarioTrilha> ConsultarAusenciaTrilhas()
//        {
//            var relatorioUsuarioTrilha = new RelatorioUsuarioTrilha();
//            var dataLimite = DateTime.Now.AddDays(-14);
//            IList<UsuarioTrilha> lstUsuarioTrilha = relatorioUsuarioTrilha.ObterUsuariosAusentes(dataLimite);
//            this.VerificarLimiteDeConlusaoTrilha(lstUsuarioTrilha);
//            List<DTOUsuarioTrilha> lstResult = new List<DTOUsuarioTrilha>();

//            foreach (var usuarioTrilha in lstUsuarioTrilha)
//            {
//                // Caso o usuário tenha ultrapassado a data limite de finalização não envia e-mail para o usuário.
//                if (usuarioTrilha.DataLimite < DateTime.Now)
//                {
//                    continue;
//                }

//                DTOUsuarioTrilha dtoUsuarioTrilha = new DTOUsuarioTrilha();
//                dtoUsuarioTrilha.ID = usuarioTrilha.ID;
//                dtoUsuarioTrilha.DataLimite = usuarioTrilha.DataLimite;
//                dtoUsuarioTrilha.Usuario.ID = usuarioTrilha.Usuario.ID;
//                dtoUsuarioTrilha.Usuario.Nome = usuarioTrilha.Usuario.Nome;
//                dtoUsuarioTrilha.Usuario.Email = usuarioTrilha.Usuario.Email;
//                dtoUsuarioTrilha.Trilha.Nome = usuarioTrilha.TrilhaNivel.Trilha.Nome;
//                dtoUsuarioTrilha.Trilha.ID = usuarioTrilha.TrilhaNivel.Trilha.ID;
//                dtoUsuarioTrilha.TrilhaNivel.ID = usuarioTrilha.TrilhaNivel.ID;
//                dtoUsuarioTrilha.TrilhaNivel.Nome = usuarioTrilha.TrilhaNivel.Nome;
//                dtoUsuarioTrilha.StatusMatricula.Nome = usuarioTrilha.StatusMatriculaFormatado;
//                dtoUsuarioTrilha.StatusMatricula.ID = Convert.ToInt32(usuarioTrilha.StatusMatricula);
//                lstResult.Add(dtoUsuarioTrilha);
//            }


//            return lstResult;
//        }

//        /// <summary>
//        /// Verifica limite de conclusão de trilha baseado na data limite.
//        /// </summary>
//        /// <param name="usuarios">Lista de UsuarioTrilha.</param>
//        /// <param name="reprovar">Caso true, abandona os usuários que não passarem na validação.</param>
//        public void VerificarLimiteDeConlusaoTrilha(IList<UsuarioTrilha> usuarios, bool reprovar = false)
//        {
//            var bmUsuarioTrilha = new BMUsuarioTrilha();
//            // Caso o usuário tenha ultrapassado a data limite de finalização da trilha muda seus status para abandono.
//            foreach (var usuarioTrilha in usuarios.Where(usuarioTrilha => usuarioTrilha.DataLimite.Date < DateTime.Now.Date))
//            {
//                usuarioTrilha.StatusMatricula = reprovar ? enumStatusMatricula.NaoAprovado : enumStatusMatricula.Abandono;
//                bmUsuarioTrilha.Salvar(usuarioTrilha);
//            }
//        }

//        public void RegistrarVisualizacaoNotificacaoAtividadeFormativa(int idTopicoTematico, int idUsuarioTrilha)
//        {
//            BMTrilhaAtividadeFormativaParticipacao bm = new BMTrilhaAtividadeFormativaParticipacao();
//            List<TrilhaAtividadeFormativaParticipacao> listaAtividadeParticipacao = bm.ObterParticipacoesUsuarioTrilha(idTopicoTematico, idUsuarioTrilha);
//            if (listaAtividadeParticipacao != null && listaAtividadeParticipacao.Any(x => x.Visualizado == false && x.TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor))
//            {
//                foreach (var item in listaAtividadeParticipacao.Where(x => x.Visualizado == false && x.TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor))
//                {
//                    item.Visualizado = true;
//                    bm.Salvar(item);
//                }
//            }
//        }

//        public void RegistrarVisualizacaoNotificacaoItemTrilhaParticipacao(int idItemTrilha, int idUsuarioTrilha)
//        {
//            BMItemTrilhaParticipacao bm = new BMItemTrilhaParticipacao();
//            List<ItemTrilhaParticipacao> listaItemParticipacao = bm.ObterParticipacoesUsuarioTrilha(idItemTrilha, idUsuarioTrilha);
//            if (listaItemParticipacao != null && listaItemParticipacao.Any(x => x.Visualizado == false && x.TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor))
//            {
//                foreach (var item in listaItemParticipacao.Where(x => x.Visualizado == false && x.TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor))
//                {
//                    item.Visualizado = true;
//                    bm.Salvar(item);
//                }
//            }
//        }

//    }
//}
