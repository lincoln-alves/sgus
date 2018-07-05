using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Web;
using Sebrae.Academico.Dominio.Enumeracao;
using System.IO;
using Sebrae.Academico.Util.Classes;
using System.Transactions;
using System.Text;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioPagamento : BusinessProcessBase
    {
        private BMUsuarioPagamento bmUsuarioPagamento = null;

        public ManterUsuarioPagamento()
            : base()
        {
            bmUsuarioPagamento = new BMUsuarioPagamento();
        }

        /// <summary>
        /// Obtém informações dos pagamentos de um usuário.
        /// </summary>
        /// <param name="pUsuario">Id do Usuário</param>
        /// <returns>Lista contendo Informações dos pagamentos de um usuário</returns>
        public IList<UsuarioPagamento> ObterInformacoesDePagamentoDoUsuario(int pIdUsuario)
        {
            if (pIdUsuario < 0) throw new AcademicoException("Usuário. Campo Obrigatório");

            return bmUsuarioPagamento.ObterInformacoesDePagamentoDoUsuario(pIdUsuario);
        }

        public UsuarioPagamento ObterInformacoesDePagamentoPorIDUsuario(int pIdUsuario)
        {
            if (pIdUsuario < 0) throw new AcademicoException("Usuário. Campo Obrigatório");

            return bmUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioPorIdUsuario(pIdUsuario);
        }

        public UsuarioPagamento ObterInformacoesDePagamentoPorID(int pIdUsuarioPagamento)
        {
            if (pIdUsuarioPagamento < 0) throw new AcademicoException("Registro Inválido");

            return bmUsuarioPagamento.ObterInformacoesDePagamentoPorID(pIdUsuarioPagamento);
        }

        public IList<UsuarioPagamento> ObterInformacoesDePagamentoPorUsuario(int pIdUsuario)
        {
            if (pIdUsuario < 0) throw new AcademicoException("Usuário. Campo Obrigatório");

            return bmUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioPorUsuario(pIdUsuario);
        }

        public UsuarioPagamento ObterInformacoesDePagamentoDoUsuarioNossoNumero(string refTranNossoNumero)
        {
            if (string.IsNullOrWhiteSpace(refTranNossoNumero)) throw new AcademicoException("refTranNossoNumero é obrigatório");
            return bmUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioPorNossoNumero(refTranNossoNumero);
        }

        public Int64 ObterInformacoesDePagamentoDoUltimoUsuarioPorNossoNumero()
        {
            return bmUsuarioPagamento.ObterInformacoesDoUltimoPagamentoDoUsuarioPorNossoNumero();
        }

        public UsuarioPagamento ObterInformacoesCalculadasDeConfiguracaoPagamento(int idConfiguracaoPagamento)
        {
            ConfiguracaoPagamento configuracaoPagamento = new BMConfiguracaoPagamento().ObterPorID(idConfiguracaoPagamento);
            UsuarioPagamento usuarioPagamento = bmUsuarioPagamento.ObterInformacoesCalculadasDeConfiguracaoPagamento(configuracaoPagamento);
            return usuarioPagamento;
        }

        public void SalvarInformacoesDePagamento(UsuarioPagamento usuarioPagamento)
        {
            try
            {
                bmUsuarioPagamento.Salvar(usuarioPagamento);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        

        public void BaixarPagamento(UsuarioPagamento usuarioPagamento)
        {
            try
            {

                UsuarioPagamento usuarioPagamentoEncontrado = bmUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioPorNossoNumero(usuarioPagamento.NossoNumero);

                //Verifica se o pagamento já foi realizado
                //if(usuarioPagamento.PagamentoEfetuado)

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void IncluirUsuarioPagamento(UsuarioPagamento usuarioPagamento)
        {
            try
            {
                bmUsuarioPagamento.Salvar(usuarioPagamento);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void IncluirPagamentoDoUsuario(Usuario usuario, UsuarioPagamento usuarioPagamento)
        {

            BMUsuario bmUsuario = null;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {

                bmUsuario = new BMUsuario();

                //using (var transaction = bmUsuario.repositorio.ObterTransacao())
                //{

                //Atualiza os dados do usuario
                bmUsuario.Salvar(usuario);

                //Insere um novo pagamento para o usuário
                bmUsuarioPagamento.Salvar(usuarioPagamento);

                //    transaction.Commit();

                //}

                scope.Complete();
            }
        }

        
        public void ProcessarArquivoDeDebitoDoBancoDoBrasil(HttpPostedFile arquivoEnviado)
        {
            try
            {

                bmUsuarioPagamento = new BMUsuarioPagamento();
                
                string linhacompleta, identificacao, datalote, convenio = null;
                decimal valorrecebido = 0;
                string datadocredito, refTranNossoNumero = null;

                //foreach (string linha in linhas)
                // {

                StringBuilder sbPagamentosNaoEncontrados = new StringBuilder();
                sbPagamentosNaoEncontrados.Append("O(s) seguinte(s) Pagamento(s) não foi(oram) encontrado(s)");
                sbPagamentosNaoEncontrados.AppendLine("<br />");
                sbPagamentosNaoEncontrados.AppendLine("<br />");
                bool arquivoVazio = true;
                bool temPagamentoNaoEncontrado = false;

                using (StreamReader sr = new StreamReader(arquivoEnviado.InputStream))
                {

                    while ((linhacompleta = sr.ReadLine()) != null)
                    {
                        identificacao = linhacompleta.Substring(0, 1);

                        if (identificacao.ToUpper().Equals("A")) //registro header
                        {
                            datalote = linhacompleta.Substring(65, 8);
                        }
                        //else if (identificacao.ToUpper().Equals("Z")) //registro footer
                        //{
                        //    throw new AcademicoException("Arquivo Inválido.");
                        //}
                        else //registro detalhe
                        {
                            arquivoVazio = false;
                            convenio = linhacompleta.Substring(64, 7);
                            if (convenio == "3072072")
                            {
                                valorrecebido = decimal.Parse(linhacompleta.Substring(81, 12));
                                datadocredito = linhacompleta.Substring(29, 8);
                                datadocredito = datadocredito.Substring(6, 2) + "/" + datadocredito.Substring(4, 2) + "/" + datadocredito.Substring(0, 4);
                                refTranNossoNumero = linhacompleta.Substring(64, 17);

                                if (refTranNossoNumero.Substring(0, 7) == "3072072")
                                {
                                    ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                                    UsuarioPagamento usuarioPagamento = this.ObterInformacoesDePagamentoDoUsuarioNossoNumero(refTranNossoNumero);

                                    //Se não encontrou o pagamento pelo nosso número, concatena
                                    if (usuarioPagamento == null)
                                    {
                                        sbPagamentosNaoEncontrados.AppendLine(string.Concat(refTranNossoNumero, "<br />"));
                                        temPagamentoNaoEncontrado = true;
                                        continue;
                                    }

                                    usuarioPagamento.DataPagamento = DateTime.Parse(datadocredito);
                                    usuarioPagamento.ValorPagamento = valorrecebido;
                                    usuarioPagamento.PagamentoConfirmado = true;
                                    usuarioPagamento.Auditoria = new Auditoria();

                                    if (valorrecebido > 0)
                                    {
                                        //Inserir aqui a gravação de um registro na tabela TB_UsuarioPagamento campos IN_Pago com 1 e DT_Pagamento
                                        usuarioPagamento.DataPagamento = DateTime.Parse(datadocredito);
                                        usuarioPagamento.ValorPagamento = valorrecebido / 10;
                                        usuarioPagamento.PagamentoConfirmado = true;
                                        usuarioPagamento.Auditoria = new Auditoria("Processo de retorno");

                                        manterUsuarioPagamento.SalvarInformacoesDePagamento(usuarioPagamento);
                                    }
                                }
                            }
                        }
                    }
                }

                //}fim foreach
                if (arquivoVazio)
                {
                    throw new AcademicoException("Arquivo Inválido.");
                }

                if (temPagamentoNaoEncontrado)
                {
                    throw new AcademicoException(sbPagamentosNaoEncontrados.ToString());
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void ProcessarArquivoDeDebitoDoBancoDoBrasilCBR643(HttpPostedFile arquivoEnviado)
        {

            try
            {
                bmUsuarioPagamento = new BMUsuarioPagamento();
                
                
                string linhacompleta, identificacao, datalote, convenio = null;
                decimal valorrecebido = 0;
                string liquidacao = null;
                string datadocredito, refTranNossoNumero = null;


                StringBuilder sbPagamentosNaoEncontrados = new StringBuilder();
                sbPagamentosNaoEncontrados.Append("O(s) seguinte(s) Pagamento(s) não foi(oram) encontrado(s)");
                sbPagamentosNaoEncontrados.AppendLine("<br />");
                sbPagamentosNaoEncontrados.AppendLine("<br />");
                bool arquivoVazio = true;
                bool temPagamentoNaoEncontrado = false;

                using (StreamReader sr = new StreamReader(arquivoEnviado.InputStream))
                {

                    while ((linhacompleta = sr.ReadLine()) != null)
                    {

                        //linhacompleta = linha;
                        identificacao = linhacompleta.Substring(0, 1);

                        if (int.Parse(identificacao) == 0) //registro header
                        {
                            datalote = linhacompleta.Substring(94, 6);
                        }
                        //else if (int.Parse(identificacao) == 9) //registro footer
                        //{
                        //    //TODO: -> Rever este ponto, pois podemos ter um arquivo começando com o valor 9.
                        //    throw new AcademicoException("Arquivo Inválido.");
                        //}
                        else
                        {
                            arquivoVazio = false;
                            convenio = linhacompleta.Substring(31, 7);
                            if (convenio == "3072072")
                            {
                                valorrecebido = decimal.Parse(linhacompleta.Substring(253, 13));
                                datadocredito = linhacompleta.Substring(175, 6);
                                datadocredito = datadocredito.Substring(0, 2) + "/" + datadocredito.Substring(2, 2) + "/20" + datadocredito.Substring(4, 2);
                                liquidacao = linhacompleta.Substring(108, 2);
                                refTranNossoNumero = linhacompleta.Substring(63, 17);

                                if (liquidacao == "03" || liquidacao == "13")
                                {
                                    //TODO: Verificar esta regra no legado.
                                    //Houve problema com a liquidação - 03 = Comando recusado (Motivo indicado na posição 087/088), 13 = Abatimento Cancelado
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(refTranNossoNumero))
                                    {
                                        ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                                        UsuarioPagamento usuarioPagamento = manterUsuarioPagamento.ObterInformacoesDePagamentoDoUsuarioNossoNumero(refTranNossoNumero);

                                        //Se não encontrou o pagamento pelo nosso número, concatena
                                        if (usuarioPagamento == null)
                                        {
                                            sbPagamentosNaoEncontrados.AppendLine(string.Concat(refTranNossoNumero, "<br />"));
                                            temPagamentoNaoEncontrado = true;
                                            continue;
                                        }

                                        if (valorrecebido > 0)
                                        {
                                            usuarioPagamento.DataPagamento = DateTime.Parse(datadocredito);
                                            usuarioPagamento.ValorPagamento = valorrecebido / 10;
                                            usuarioPagamento.PagamentoConfirmado = true;
                                            usuarioPagamento.Auditoria = new Auditoria("Processo de retorno");
                                            manterUsuarioPagamento.SalvarInformacoesDePagamento(usuarioPagamento);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                //}Fim do ForEach

                if (arquivoVazio)
                {
                    throw new AcademicoException("Arquivo Inválido.");
                }

                if (temPagamentoNaoEncontrado)
                {
                    throw new AcademicoException(sbPagamentosNaoEncontrados.ToString());
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }





        }

        public IList<UsuarioPagamento> ObterInformacoesDePagamentoPorConfiguracaoPagamento(int idConfiguracaoPagamento)
        {

            try
            {
                return new BMUsuarioPagamento().ObterInformacoesDePagamentoPorConfiguracaoPagamento(idConfiguracaoPagamento);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

    }
}
