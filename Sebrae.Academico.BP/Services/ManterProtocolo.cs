using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.Protocolo;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterProtocolo : BusinessProcessServicesBase, IDisposable
    {
        private BMProtocolo bmProtocolo;

        public ManterProtocolo()
        {
            bmProtocolo = new BMProtocolo();
        }

        public void SalvarSemCommit(Protocolo protocolo)
        {
            bmProtocolo.SalvarSemCommit(protocolo);
        }

        public List<DTOProtocolo> AcompanharProtocolo(int numeroProtocolo)
        {
            var listaProtocolos = bmProtocolo.ObterTodosPorNumero(numeroProtocolo).ToList();
            List<DTOProtocolo> dtoProtocolos = new List<DTOProtocolo>();

            foreach (var protocolo in listaProtocolos)
            {
                var dtoProtocolo = new DTOProtocolo
                {
                    ID = protocolo.ID,
                    Numero = protocolo.Numero,
                    DataEnvio = protocolo.DataEnvio.ToString("dd/MM/yyyy HH:mm:ss"),
                    DataRecebimento = protocolo.DataRecebimento != null ? protocolo.DataRecebimento.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                    Descricao = protocolo.Descricao,
                    NomeRemetente = protocolo.Remetente != null ? protocolo.Remetente.Nome : "",
                    NomeDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.Nome : "",
                    AssinaturaRecebimento = protocolo.UsuarioAssinatura != null ? protocolo.UsuarioAssinatura.Nome : "",
                    ID_UsuarioDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.ID : 0,
                    ID_UsuarioRemetente = protocolo.Remetente != null ? protocolo.Remetente.ID : 0,
                    Anexos = protocolo.Anexos.ToList().Select(a => new DTOFileServer
                    {
                        IdFileServer = a.FileServer.ID,
                        FileServerLink = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS, null).Registro + string.Format(@"/ExibirFileServer.ashx?Identificador={1}", HttpContext.Current.Request.Url, a.FileServer.NomeDoArquivoNoServidor),
                        NomeDoArquivoOriginal = a.FileServer.NomeDoArquivoOriginal,
                        Usuario = a.Usuario.Nome,
                        DataEnvio = a.FileServer.DataAlteracao?.ToString("dd/MM/yyyy")
                    }).ToList(),
                    Despacho = protocolo.Despacho,
                    DespachoReencaminhamento = protocolo.DespachoReencaminhamento,
                    ProtocoloPai = protocolo.ProtocoloPai?.ID,
                    Arquivado = protocolo.Arquivado
                };

                dtoProtocolos.Add(dtoProtocolo);
            }

            return dtoProtocolos.OrderByDescending(x => x.Numero).ToList();
        }

        public IList<DTOProtocolo> ObterTodos(string protocolo = null, string discriminacao = null, string dtaEnvio = null, string dtaRecebimento = null, string remetente = null, string destinatario = null)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("P_NumProtocolo", protocolo);
            parametros.Add("P_TxtDiscriminacao", discriminacao);
            parametros.Add("P_DtaEnvio", dtaEnvio);
            parametros.Add("P_DtaRecebimento", dtaRecebimento);
            parametros.Add("P_NomRemetente", remetente);
            parametros.Add("P_NomDestinatario", destinatario);

            return SQLUtil.ExecutarProcedure<DTOProtocolo>("SP_Protocolo", parametros).OrderByDescending(x => x.Numero).ToList();
        }

        public IList<DTOProtocolo> ListarHistorico(string cpf, string protocolo = null, string discriminacao = null, string dtaEnvio = null, string dtaRecebimento = null, string remetente = null, string destinatario = null, string statusId = null, int todos = 0)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("P_CpfUsuario", cpf);
            parametros.Add("P_NumProtocolo", protocolo);
            parametros.Add("P_TxtDiscriminacao", discriminacao);
            parametros.Add("P_DtaEnvio", dtaEnvio);
            parametros.Add("P_DtaRecebimento", dtaRecebimento);
            parametros.Add("P_NomRemetente", remetente);
            parametros.Add("P_NomDestinatario", destinatario);
            parametros.Add("P_StatusID", statusId);

            if (todos > 0) parametros.Add("P_Todos", 1);

            return SQLUtil.ExecutarProcedure<DTOProtocolo>("SP_ProtocoloHistorico", parametros).OrderByDescending(x => x.Numero).ToList();
        }

        public IList<DTOProtocolo> ListarProtocoloEnviado(string cpf, string protocolo = null, string discriminacao = null, string dtaEnvio = null, string dtaRecebimento = null, string remetente = null, string destinatario = null)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("P_CpfUsuario", cpf);
            parametros.Add("P_NumProtocolo", protocolo);
            parametros.Add("P_TxtDiscriminacao", discriminacao);
            parametros.Add("P_DtaEnvio", dtaEnvio);
            parametros.Add("P_DtaRecebimento", dtaRecebimento);
            parametros.Add("P_NomRemetente", remetente);
            parametros.Add("P_NomDestinatario", destinatario);

            return SQLUtil.ExecutarProcedure<DTOProtocolo>("SP_Protocolo", parametros).OrderByDescending(x => x.Numero).ToList();
        }

        public IList<DTOProtocolo> ListarProtocoloAnalisar(string cpf, string protocolo = null, string discriminacao = null, string dtaEnvio = null, string dtaRecebimento = null, string remetente = null, string destinatario = null)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("P_CpfUsuario", cpf);
            parametros.Add("P_NumProtocolo", protocolo);
            parametros.Add("P_TxtDiscriminacao", discriminacao);
            parametros.Add("P_DtaEnvio", dtaEnvio);
            parametros.Add("P_DtaRecebimento", dtaRecebimento);
            parametros.Add("P_NomRemetente", remetente);
            parametros.Add("P_NomDestinatario", destinatario);
            parametros.Add("P_Historico", 0);

            return SQLUtil.ExecutarProcedure<DTOProtocolo>("SP_Protocolo", parametros).OrderByDescending(x => x.Numero).ToList();
        }

        public DTOProtocolo ObterPorId(int idProtocolo)
        {
            var protocolo = bmProtocolo.ObterPorId(idProtocolo);

            return new DTOProtocolo()
            {
                ID = protocolo.ID,
                Numero = protocolo.Numero,
                DataEnvio = protocolo.DataEnvio.ToString("dd/MM/yyyy HH:mm:ss"),
                DataRecebimento = protocolo.DataRecebimento != null ? protocolo.DataRecebimento.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                Descricao = protocolo.Descricao,
                NomeRemetente = protocolo.Remetente != null ? protocolo.Remetente.Nome : "",
                NomeDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.Nome : "",
                AssinaturaRecebimento = protocolo.UsuarioAssinatura != null ? protocolo.UsuarioAssinatura.Nome : "",
                ID_UsuarioDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.ID : 0,
                ID_UsuarioRemetente = protocolo.Remetente != null ? protocolo.Remetente.ID : 0,
                ProtocoloPai = protocolo.ProtocoloPai.ID
            };
        }

        public Protocolo ObterProtocoloPorId(int idProtocolo)
        {
            return bmProtocolo.ObterPorId(idProtocolo);
        }

        public DTOProtocolo ObterPorNumero(int numero)
        {
            try
            {
                var protocolo = bmProtocolo.ObterPorNumero(numero);
                if (protocolo == null) throw new AcademicoException("protocolo nao encontrado");

                //string downloadFileServer = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + string.Format(@"/ExibirFileServer.ashx?Identificador={1}", HttpContext.Current.Request.Url, protocolo.FileServer.NomeDoArquivoNoServidor);

                return new DTOProtocolo()
                {
                    ID = protocolo.ID,
                    Numero = protocolo.Numero,
                    DataEnvio = protocolo.DataEnvio.ToString("dd/MM/yyyy HH:mm:ss"),
                    DataRecebimento =
                        protocolo.DataRecebimento != null ? protocolo.DataRecebimento.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                    Descricao = protocolo.Descricao,
                    NomeRemetente = protocolo.Remetente != null ? protocolo.Remetente.Nome : "",
                    NomeDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.Nome : "",
                    AssinaturaRecebimento = protocolo.UsuarioAssinatura != null ? protocolo.UsuarioAssinatura.Nome : "",
                    ID_UsuarioDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.ID : 0,
                    ID_UsuarioRemetente = protocolo.Remetente != null ? protocolo.Remetente.ID : 0,
                    //IdFileServer = protocolo.FileServer.ID,
                    //NomeFileServer = protocolo.FileServer.NomeDoArquivoOriginal,
                    //DownloadFileServer = downloadFileServer,
                    Despacho = protocolo.Despacho
                };
            }
            catch
            {
                throw new AcademicoException("Protocolo de número: " + numero + " não foi encontrado");
            }
        }

        /// <summary>
        /// Verifica se já existe protocolo como número informado
        /// </summary>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool VerificarNumero(int numero)
        {
            var protocolo = bmProtocolo.ObterPorNumero(numero);
            if (protocolo == null) return false;

            return true;
        }

        public DTOProtocolo ObterPorDescricao(string descricao)
        {
            var protocolo = bmProtocolo.ObterPorDescricao(descricao);

            return PreencherDTOProtocolo(protocolo);
        }

        public DTOProtocolo ObterPorRemetente(string remetente)
        {
            var protocolo = bmProtocolo.ObterPorRemetente(remetente);

            return PreencherDTOProtocolo(protocolo);
        }

        public DTOProtocolo ObterPorDestinatario(string destinatario)
        {
            var protocolo = bmProtocolo.ObterPorDestinatario(destinatario);

            return PreencherDTOProtocolo(protocolo);
        }

        /*public DTOProtocolo ObterPorDataEnvio(string dataEnvio)
        {
            DateTime dateTime = Convert.ToDateTime(dataEnvio);

            var protocolo = bmProtocolo.ObterPorDataEnvio(dateTime);

            return PreencherDTOProtocolo(protocolo);
        }

        public DTOProtocolo ObterPorDataRecebimento(string dataRecebimento)
        {
            var protocolo = bmProtocolo.ObterPorDataRecebimento(dataRecebimento);

            return PreencherDTOProtocolo(protocolo);
        }

        public DTOProtocolo ObterPorUnidade(string unidade)
        {
            var protocolo = bmProtocolo.ObterPorUnidade(unidade);

            return PreencherDTOProtocolo(protocolo);
        }*/

        private DTOProtocolo PreencherDTOProtocolo(Protocolo protocolo)
        {
            //string downloadFileServer = "";
            //if (protocolo.FileServer != null)
            //    downloadFileServer = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + string.Format(@"/ExibirFileServer.ashx?Identificador={1}", HttpContext.Current.Request.Url, protocolo.FileServer.NomeDoArquivoNoServidor);

            return new DTOProtocolo()
            {
                ID = protocolo.ID,
                Numero = protocolo.Numero,
                DataEnvio = protocolo.DataEnvio.ToString("dd/MM/yyyy HH:mm:ss"),
                DataRecebimento =
                    protocolo.DataRecebimento != null ? protocolo.DataRecebimento.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                Descricao = protocolo.Descricao,
                NomeRemetente = protocolo.Remetente != null ? protocolo.Remetente.Nome : "",
                NomeDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.Nome : "",
                AssinaturaRecebimento = protocolo.UsuarioAssinatura != null ? protocolo.UsuarioAssinatura.Nome : "",
                ID_UsuarioDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.ID : 0,
                ID_UsuarioRemetente = protocolo.Remetente != null ? protocolo.Remetente.ID : 0,
                //IdFileServer = protocolo.FileServer.ID,
                //NomeFileServer = protocolo.FileServer.NomeDoArquivoOriginal,
                //DownloadFileServer = downloadFileServer
            };
        }

        public List<DTOProtocolo> ObterPorFilto(string filtro, string cpf)
        {
            List<DTOProtocolo> protocolos = new List<DTOProtocolo>();

            #region Consulta por numero de protocolo
            try
            {
                int numero;
                if (int.TryParse(filtro, out numero))
                {
                    protocolos.Add(ObterPorNumero(numero));
                }
            }
            catch { }
            #endregion

            #region Consulta por descrição.
            try
            {
                protocolos.Add(ObterPorDescricao(filtro));
            }
            catch { }
            #endregion

            #region Consulta por remetente.
            try
            {
                protocolos.Add(ObterPorRemetente(filtro));
            }
            catch { }
            #endregion

            #region Consulta por Destinatário.
            try
            {
                protocolos.Add(ObterPorDestinatario(filtro));
            }
            catch (Exception) { }
            #endregion

            /*#region Consulta por Data de Envio.
            try
            {
                protocolos.Add(ObterPorDataEnvio(filtro));
            }
            catch (Exception e) { }
            #endregion

            #region Consulta por Data de Recebimento.
            try
            {
                protocolos.Add(ObterPorDataRecebimento(filtro));
            }
            catch (Exception e) { }
            #endregion

            #region Consulta por Data de Unidade.
            try
            {
                protocolos.Add(ObterPorDataRecebimento(filtro));
            }
            catch (Exception e) { }
            #endregion*/

            var usuario = new BMUsuario().ObterPorCPF(cpf);

            protocolos = protocolos.Where(x => x.ID_UsuarioRemetente == usuario.ID || x.ID_UsuarioDestinatario == usuario.ID).ToList();

            if (protocolos.Count == 0)
            {
                throw new AcademicoException("Protocolo não encontrado.");
            }

            return protocolos;
        }

        public List<DTOProtocolo> ObterPendentes(string cpf)
        {
            List<DTOProtocolo> protocolos = new List<DTOProtocolo>();

            var pendentes = (List<Protocolo>)bmProtocolo.ObterPendentes(cpf);

            pendentes.ForEach(protocolo => protocolos.Add(new DTOProtocolo()
            {
                ID = protocolo.ID,
                Numero = protocolo.Numero,
                DataEnvio = protocolo.DataEnvio.ToString("dd/MM/yyyy HH:mm:ss"),
                DataRecebimento = protocolo.DataRecebimento != null ? protocolo.DataRecebimento.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                Descricao = protocolo.Descricao,
                NomeRemetente = protocolo.Remetente != null ? protocolo.Remetente.Nome : "",
                NomeDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.Nome : "",
                AssinaturaRecebimento = protocolo.UsuarioAssinatura != null ? protocolo.UsuarioAssinatura.Nome : "",
                ID_UsuarioRemetente = protocolo.Remetente != null ? protocolo.Remetente.ID : 0,
                ID_UsuarioDestinatario = protocolo.Destinatario != null ? protocolo.Destinatario.ID : 0
            }));

            return protocolos;
        }

        /// <summary>
        /// Obtem usuários que podem ser usados como destinatário de um protocolo.
        /// </summary>
        /// <returns></returns>
        public List<Usuario> ObterTodosUsuariosDestinatarios()
        {
            var usuarios = new BMUsuario().ObterPorFiltros(new Usuario()
            {
                // Nacional
                UF = new Uf() { ID = 1 },
                Situacao = "ativo"
            });

            return usuarios.ToList();
        }

        public int GerarNumeroProtocolo()
        {
            try
            {
                string ano = DateTime.Now.Year.ToString();
                int numero;

                var protocoloBanco = new ManterProtocolo().ObterUltimoProtocolo();

                if (protocoloBanco == null || HouveMundancaDeAno(protocoloBanco))
                {
                    numero = 1;
                }
                else
                {
                    var numeroAntigo = protocoloBanco.Numero.ToString().Substring(4, (protocoloBanco.Numero.ToString().Count() - 4));
                    numero = int.Parse(numeroAntigo) + 1;
                }

                return int.Parse(ano.ToString() + numero.ToString("D5"));

            }
            catch
            {
                throw new Exception("Não foi possível gerar o número do protocolo");
            }
        }

        private static bool HouveMundancaDeAno(Protocolo protocoloBanco)
        {
            return int.Parse(protocoloBanco.Numero.ToString().Substring(0, 4)) < DateTime.Now.Year;
        }

        public Protocolo ObterUltimoProtocolo()
        {
            var query = bmProtocolo.ObterTodosIqueryable();
            return query.OrderByDescending(x => x.DataEnvio).ThenByDescending(x => x.Numero).FirstOrDefault();
        }

        public void SalvarProtocolo(Protocolo protocolo)
        {
            bmProtocolo.Salvar(protocolo);
        }

        /// <summary>
        /// Método usado para notificar o destinatário caso aconteça o envio de um protocolo.
        /// </summary>
        /// <param name="remetente"></param>
        /// <param name="destinatario"></param>
        public void NotificarDestinatario(Usuario remetente, Usuario destinatario, Protocolo protocolo)
        {
            if (destinatario != null)
            {
                if (string.IsNullOrEmpty(destinatario.Email))
                    throw new AcademicoException("Email não cadastrado");

                Template email = TemplateUtil.ObterInformacoes(enumTemplate.NotificacaoEnvioProtocolo);
                var documentos = protocolo.Anexos.Select(x => x.FileServer.NomeDoArquivoOriginal);
                var nomesDocumentos = string.Join(", ", documentos);

                var endereco = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal30, null).Registro;

                email.TextoTemplate = email.TextoTemplate
                    .Replace("#NUMEROPROTOCOLO", protocolo.Numero.ToString())
                    .Replace("#DOCUMENTO ", nomesDocumentos)
                    .Replace("#DESCRIMINACAO", protocolo.Descricao)
                    .Replace("#LINKACESSO", endereco + "protocolos/detalhes/" + protocolo.Numero.ToString());

                EmailUtil.Instancia.EnviarEmail(destinatario.Email, email.Assunto, email.TextoTemplate);
            }
        }

        /// <summary>
        /// Método usado para notificar o remetente do recebimento e assinatura de um protocolo.
        /// </summary>
        /// <param name="remetente"></param>
        /// <param name="destinatario"></param>
        public void NotificarRemetente(Usuario remetente, Usuario destinatario, long numero)
        {
            if (remetente != null)
            {
                if (string.IsNullOrEmpty(destinatario.Email))
                    throw new AcademicoException("Email não cadastrado");

                Template email = TemplateUtil.ObterInformacoes(enumTemplate.NotificacaoEnvioProtocolo);

                var htmlNewLine = "</p>" + Environment.NewLine;
                int quebra = 0;

                if (email != null && !string.IsNullOrEmpty(email.TextoTemplate))
                {
                    quebra = email.TextoTemplate.IndexOf(htmlNewLine, StringComparison.Ordinal) + htmlNewLine.Length;
                }

                string assuntoDoEmail = "Assinatura de protocolo realizada";
                string corpoDoEmail = string.Format("O protocolo número {0} foi assinado pelo usuário {1}", numero, destinatario.Nome);

                if (quebra > 0)
                {
                    if (email != null && !string.IsNullOrEmpty(email.TextoTemplate))
                    {
                        assuntoDoEmail = email.Assunto;
                        corpoDoEmail = email.TextoTemplate;
                    }
                }

                corpoDoEmail = corpoDoEmail.Replace("#REMETENTE", remetente.Nome)
                                               .Replace("#DESTINATARIO", destinatario.CPF)
                                               .Replace("#NUMEROPROTOCOLO", destinatario.CPF)
                                               .Replace("#DATAHORA", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));

                //Envia e-mail para o usuário 
                EmailUtil.Instancia.EnviarEmail(destinatario.Email,
                                                    assuntoDoEmail,
                                                    corpoDoEmail);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ArquivarProtocolo(int numeroProtocolo)
        {
            bmProtocolo.ObterTodosPorNumero(numeroProtocolo)
                .ToList()
                .ForEach(protocolo =>
                {
                    protocolo.Arquivado = true;
                    bmProtocolo.Salvar(protocolo);
                });
        }
    }
}
