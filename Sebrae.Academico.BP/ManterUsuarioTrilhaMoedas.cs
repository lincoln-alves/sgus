using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioTrilhaMoedas : RepositorioBase<UsuarioTrilhaMoedas>
    {

        #region "Atributos Privados"

        private readonly BMUsuarioTrilhaMoedas _bmUsuarioTrilhaMoedas;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterUsuarioTrilhaMoedas()
        {
            _bmUsuarioTrilhaMoedas = new BMUsuarioTrilhaMoedas();
        }

        #endregion

        #region "Métodos Públicos"

        /// <summary>
        /// Inclui o histórico e a alteração no saldo das moedas do usuárioTrilha, itemTrilha se for solução sebrae e ou itemTrilhaCurtida se for uma curtida.
        /// </summary>
        /// <param name="usuarioTrilha"></param>
        /// <param name="itemTrilha"></param>
        /// <param name="itemTrilhaCurtida"></param>
        /// <param name="quantidadePrata"></param>
        /// <param name="quantidadeOuro"></param>
        /// <param name="tipoCurtidaAnterior"></param>
        public Dictionary<string, int> Incluir(UsuarioTrilha usuarioTrilha, ItemTrilha itemTrilha = null,
            ItemTrilhaCurtida itemTrilhaCurtida = null, int quantidadePrata = 0, int quantidadeOuro = 0,
            enumTipoCurtida tipoCurtidaAnterior = enumTipoCurtida.SemAcao)
        {
            VerificarRegrasNegocio(usuarioTrilha, itemTrilha, itemTrilhaCurtida, quantidadePrata, quantidadeOuro);

            var usuarioTrilhaMoedas = new UsuarioTrilhaMoedas()
            {
                TipoCurtida = itemTrilhaCurtida != null ? itemTrilhaCurtida.TipoCurtida : enumTipoCurtida.SemAcao,
                Curtida = itemTrilhaCurtida,
                ItemTrilha = itemTrilha,
                UsuarioTrilha = usuarioTrilha,
                MoedasDePrata = quantidadePrata,
                MoedasDeOuro = quantidadeOuro,
                DataCriacao = DateTime.Now,
                Auditoria = new Auditoria(usuarioTrilha.Usuario.CPF)
            };

            if (itemTrilhaCurtida != null)
                usuarioTrilhaMoedas.MoedasDePrata = CalcularCurtida(itemTrilhaCurtida, tipoCurtidaAnterior);
            
            if (!this.IsCreditoDuplicado(usuarioTrilhaMoedas))
                _bmUsuarioTrilhaMoedas.Salvar(usuarioTrilhaMoedas);
            
            return new Dictionary<string, int>()
            {
                { "Ouro", Obter(usuarioTrilha, enumTipoMoeda.Ouro) },
                { "Prata", Obter(usuarioTrilha, enumTipoMoeda.Prata) }
            };
        }

        /// <summary>
        /// Verifica se já ocorreu o crédito de moedas de ouro para o usuarioTrilha.
        /// </summary>
        /// <param name="usuarioTrilhaMoedas"></param>
        /// <returns></returns>
        private bool IsCreditoDuplicado(UsuarioTrilhaMoedas usuarioTrilhaMoedas)
        {
            // Moedas de prata podem ser inseridas 'n' vezes para um UsuarioTrilhaMoedas
            if (usuarioTrilhaMoedas?.Curtida != null)
                return false;

            // Moedas de ouro
            var rsMoedas = this.ObterTodos().FirstOrDefault(
                    x =>
                        x.UsuarioTrilha == usuarioTrilhaMoedas.UsuarioTrilha &&
                        x.ItemTrilha == usuarioTrilhaMoedas.ItemTrilha &&
                        x.MoedasDeOuro == usuarioTrilhaMoedas.MoedasDeOuro
                );

            var hasMoedasOuro = (rsMoedas != null);

            // Log de tentativa de cadastro duplicado para moedas de ouro
            if (hasMoedasOuro)
                LogErroCreditoDuplicado();
            
            return hasMoedasOuro;
        }

        /// <summary>
        /// Registra o log de crédito duplicado em moedas
        /// </summary>
        private static void LogErroCreditoDuplicado()
        {
            var stack = new StackTrace();
            var file = AppDomain.CurrentDomain.BaseDirectory + "/Log_moeda.txt";
            
            if (!File.Exists(file))
            {
                using (var streamWriter = File.CreateText(file))
                {
                    streamWriter.Write("\r\nCredito Duplicado em Moedas: ");
                }
            }

            using (var eStreamWriter = File.AppendText(file))
            {
                eStreamWriter.Write("\r\n");
                eStreamWriter.WriteLine("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                eStreamWriter.WriteLine(":");
                eStreamWriter.WriteLine(stack.ToString());
                eStreamWriter.WriteLine("\r\n");
            }

            var email = (new ErroUtil()).EmailDeDestinoDoErro;
            var assunto = "Erro - Crédito duplicado em moedas: " + DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString();
            var mensagem = stack.ToString();

            try
            {
                (new ManterEmail()).EnviarEmail(email, assunto, mensagem);
            }
            catch (SmtpException smtpException)
            {
                using (var eStreamWriter = File.AppendText(file))
                {
                    eStreamWriter.Write("\r\n");
                    eStreamWriter.Write("Erro no envio de email: ");
                    eStreamWriter.Write("\r\n");
                    eStreamWriter.WriteLine((new ErroUtil()).ObterMensagemDeErro(smtpException));

                }
            }
        }

        /// <summary>
        /// Faz as verificações de regra de negócio para inserção e alteração de moedas
        /// </summary>
        /// <param name="itemTrilha"></param>
        /// <param name="itemTrilhaCurtida"></param>
        /// <param name="quantidadePrata"></param>
        /// <param name="quantidadeOuro"></param>
        public void VerificarRegrasNegocio(UsuarioTrilha usuarioTrilha, ItemTrilha itemTrilha = null, ItemTrilhaCurtida itemTrilhaCurtida = null, int quantidadePrata = 0, int quantidadeOuro = 0)
        {
            if (itemTrilha != null && itemTrilhaCurtida != null)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Não é possível incluir variações ao mesmo tempo no histórico de moedas.");

            if (quantidadeOuro > 0 && quantidadePrata > 0)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Não é possível incluir moedas de prata e ouro ao mesmo tempo, a troca só é válida ao remover prata e adicionar ouro.");

            if (itemTrilha != null && quantidadePrata != 0)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Não é possível incluir moedas de prata ao completar uma solução sebrae.");

            if (itemTrilhaCurtida != null && quantidadeOuro != 0)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Não é possível incluir moedas de ouro ao curtir uma solução do trilheiro.");

            if (quantidadeOuro < 0)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Não é possível retirar moedas de ouro.");

            if (quantidadeOuro > 0 && quantidadePrata < 0 && Obter(usuarioTrilha, enumTipoMoeda.Prata) < 0)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Usuário não possuí moedas de prata suficientes para realizar a troca.");

        }

        /// <summary>
        /// Calcula o peso da curtida/descurtida/cancelamento que a ação atual gerou.
        /// </summary>
        /// <param name="itemTrilhaCurtida"></param>
        /// <param name="tipoCurtidaAnterior"></param>
        /// <returns></returns>
        public int CalcularCurtida(ItemTrilhaCurtida itemTrilhaCurtida, enumTipoCurtida tipoCurtidaAnterior = enumTipoCurtida.SemAcao)
        {
            if (itemTrilhaCurtida == null)
                return 0;

            int quantidadePrata = 0;

            switch (tipoCurtidaAnterior)
            {
                case enumTipoCurtida.SemAcao:
                    if (itemTrilhaCurtida.TipoCurtida == enumTipoCurtida.Curtiu)
                        quantidadePrata += itemTrilhaCurtida.ValorCurtida;
                    else if (itemTrilhaCurtida.TipoCurtida == enumTipoCurtida.Descurtiu)
                        quantidadePrata -= itemTrilhaCurtida.ValorDescurtida;
                    break;
                case enumTipoCurtida.Curtiu:
                    // Removo a curtida porque ele só pode ir para sem ação ou descurtida
                    quantidadePrata -= itemTrilhaCurtida.ValorCurtida;
                    // Caso ele vá para descurtida, removo mais o valor da descurtida
                    if (itemTrilhaCurtida.TipoCurtida == enumTipoCurtida.Descurtiu)
                        quantidadePrata -= itemTrilhaCurtida.ValorDescurtida;
                    break;
                case enumTipoCurtida.Descurtiu:
                    // Removo a descurtida porque ele só pode ir para sem ação ou curtida
                    quantidadePrata += itemTrilhaCurtida.ValorDescurtida;
                    // Caso ele vá para descurtida, removo mais o valor da descurtida
                    if (itemTrilhaCurtida.TipoCurtida == enumTipoCurtida.Curtiu)
                        quantidadePrata += itemTrilhaCurtida.ValorCurtida;
                    break;
            }

            return quantidadePrata;
        }

        /// <summary>
        /// Calcula as moedas pelo histórico e atualiza as mesmas no saldo do usuário.
        /// </summary>
        /// <param name="usuarioTrilha"></param>
        /// <returns></returns>
        public Dictionary<string, int> CalcularMoedas(UsuarioTrilha usuarioTrilha)
        {
            return new Dictionary<string, int>()
            {
                { "Ouro", Obter(usuarioTrilha, enumTipoMoeda.Ouro) },
                { "Prata", Obter(usuarioTrilha, enumTipoMoeda.Prata) }
            };
        }

        /// <summary>
        /// Obtem a soma das moedas do histórico pelo tipo de moeda.
        /// </summary>
        /// <param name="usuarioTrilha"></param>
        /// <param name="pTipoMoeda"></param>
        /// <returns></returns>
        public int Obter(UsuarioTrilha usuarioTrilha, enumTipoMoeda pTipoMoeda)
        {
            var query = _bmUsuarioTrilhaMoedas.ObterTodos(usuarioTrilha);

            if (!query.Any())
                return 0;

            switch (pTipoMoeda)
            {
                case enumTipoMoeda.Prata:
                    return query.Sum(x => x.MoedasDePrata);
                case enumTipoMoeda.Ouro:
                    return query.Sum(x => x.MoedasDeOuro);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Obtem a soma das moedas do histórico pela curtida.
        /// </summary>
        /// <param name="usuarioTrilha"></param>
        /// <param name="curtida"></param>
        /// <param name="pTipoMoeda"></param>
        /// <returns></returns>
        public int Obter(UsuarioTrilha usuarioTrilha, ItemTrilha itemTrilha)
        {
            var query = _bmUsuarioTrilhaMoedas.ObterTodos(usuarioTrilha).Where(x => x.Curtida.ItemTrilha.ID == itemTrilha.ID);

            if (!query.Any())
                return 0;
            
            return query.Sum(x => x.MoedasDePrata);
        }

        public IQueryable<UsuarioTrilhaMoedas> ObterTodos()
        {
            return _bmUsuarioTrilhaMoedas.ObterTodos();
        }

        /// <summary>
        /// Obtem todo o histórico do nivel de acordo com o usuario trilha
        /// </summary>
        /// <param name="pUsuarioTrilha"></param>
        /// <returns></returns>
        public IQueryable<UsuarioTrilhaMoedas> ObterTodos(UsuarioTrilha usuarioTrilha)
        {
            return _bmUsuarioTrilhaMoedas.ObterTodos(usuarioTrilha);
        }

        public UsuarioTrilhaMoedas Obter(int pId)
        {
            return _bmUsuarioTrilhaMoedas.Obter(pId);
        }

        public dynamic ObterExtrato(UsuarioTrilha usuarioTrilha)
        {
            // Moedas por Solução Sebrae
            var solucoesSebrae = ObterTodos(usuarioTrilha).Where(x => x.ItemTrilha != null).Select(x => new
            {
                Nome = x.ItemTrilha.Nome,
                Moedas = x.MoedasDeOuro
            }).Take(4).ToList();

            // Cambio de Moedas
            var cambioMoedas = ObterTodos(usuarioTrilha).Where(x => x.Curtida == null && x.ItemTrilha == null && x.MoedasDePrata < 0 && x.MoedasDeOuro > 0).Take(4).ToList().Select(x => new
            {
                QuantidadePrata = x.MoedasDePrata,
                QuantidadeOuro = x.MoedasDeOuro,
                DataCambio = x.DataCriacao
            }).ToList();
            
            // Moedas por Solução Trilheiro (curtidas)
            var solucoesTrilheiro = usuarioTrilha.TrilhaNivel.ListaItemTrilha.Where(x => x.Usuario != null && x.Usuario.ID == usuarioTrilha.Usuario.ID).Select(x => new
            {
                Nome = x.Nome,
                Curtidas = usuarioTrilha.ObterCurtidasPorTipo(x, enumTipoCurtida.Curtiu),
                Descurtidas = usuarioTrilha.ObterCurtidasPorTipo(x, enumTipoCurtida.Descurtiu),
                ExcluirSolucaoTrilheiro = usuarioTrilha.ObterCurtidasPorTipo(x, enumTipoCurtida.ExcluirSolucaoTrilheiro),
                Moedas = usuarioTrilha.ObterSomaMoedasPrata(x)
            }).OrderByDescending(y => y.Curtidas).Take(4).ToList();

            return new
            {
                SolucoesSebrae = solucoesSebrae,
                SolucoesTrilheiro = solucoesTrilheiro,
                CambioMoedas = cambioMoedas
            };
        }
        #endregion
    }
}