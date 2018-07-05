using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterCampo : BusinessProcessBase
    {

        private BMCampo bmCampo = null;

        public ManterCampo()
            : base()
        {
            bmCampo = new BMCampo();
        }

        public void Incluir(Campo model)
        {
            try
            {
                ValidarCampo(model);
                this.PreencherInformacoesDeAuditoria(model);
                model.Ordem = (byte)(bmCampo.ObterUltimaOrdemDaEtapa(model.Etapa.ID) + 1);
                bmCampo.Salvar(model);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void Excluir(Campo campo)
        {
            try
            {
                bmCampo.Excluir(campo);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<Campo> ObterTodas()
        {
            return bmCampo.ObterTodos();
        }

        public IList<Campo> ObterPorFiltro(Campo filtro)
        {
            return bmCampo.ObterPorFiltro(filtro);
        }

        private void ValidarCampo(Campo campo)
        {
            if (String.IsNullOrEmpty(campo.Nome)) throw new AcademicoException("Nome é obrigatório");
            if (campo.Tamanho == 0 && campo.TipoCampo != (byte)enumTipoCampo.Field) throw new AcademicoException("Tamanho é obrigatório");
        }

        public void Alterar(Campo model)
        {
            try
            {
                ValidarCampo(model);
                this.PreencherInformacoesDeAuditoria(model);
                bmCampo.Salvar(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(Campo model)
        {
            //base.PreencherInformacoesDeAuditoria(pCapacitacao);
            //pCapacitacao.ListaModulos.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public Campo ObterPorID(int IdModel)
        {
            return bmCampo.ObterPorId(IdModel);
        }

        public IList<Campo> ObterPorEtapa(int IdEtapa)
        {
            return bmCampo.ObterPorEtapaId(IdEtapa);
        }

        public void DuplicarObjeto(int IdModel)
        {
            bmCampo.DuplicarObjeto(IdModel);
        }

        public void AdicionarCamposVinculados(List<Campo> campos, Campo campoEdicao)
        {

            foreach (Campo campo in campos)
            {
                if (campoEdicao.ListaCamposVinculados.ToList().All(x => x.ID != campo.ID))
                {
                    campoEdicao.AdicionarCampo(campo);
                }
            }

            //bmCampo.Salvar(campoEdicao);
        }

        public void RemoverCamposVinculados(List<Campo> campos, Campo campoEdicao)
        {
            foreach (Campo campo in campos)
            {
                campoEdicao.RemoverCampo(campo);
            }

            //bmCampo.Salvar(campoEdicao);
        }

        public void RemoverTodosCamposVinculados(Campo campoEdicao)
        {
            campoEdicao.RemoverTodosCamposVinculados();
        }

        public decimal ObterTotalSomatorio(Campo campo, int processoResposta, int idUsuario = 0)
        {
            decimal total = 0;

            if (campo.ListaCamposVinculados.Any())
            {
                Usuario usuario = new ManterUsuario().ObterUsuarioPorID(idUsuario);
                CultureInfo culture;
                NumberStyles style;
                culture = CultureInfo.CreateSpecificCulture("pt-BR");
                style = NumberStyles.Number;
                foreach (Campo item in campo.ListaCamposVinculados)
                {
                    CampoResposta campoResposta = new BMCampoResposta().ObterPorCampoProcessoResposta(item.ID, processoResposta);

                    string resposta = campoResposta != null ? campoResposta.Resposta : string.Empty;

                    decimal conversao = 0;
                    if (decimal.TryParse(resposta, style, culture, out conversao))
                    {
                        total += conversao;
                    }
                }
            }
            return total;
        }

        public decimal ObterTotalSubtracao(Campo campo, int processoResposta, int idUsuario = 0)
        {
            decimal total = 0;

            if (campo.ListaCamposVinculados.Any())
            {
                Usuario usuario = new ManterUsuario().ObterUsuarioPorID(idUsuario);
                CultureInfo culture;
                NumberStyles style;
                culture = CultureInfo.CreateSpecificCulture("pt-BR");
                style = NumberStyles.Number;
                foreach (Campo item in campo.ListaCamposVinculados)
                {
                    CampoResposta campoResposta = new BMCampoResposta().ObterPorCampoProcessoResposta(item.ID, processoResposta);

                    string resposta = campoResposta != null ? campoResposta.Resposta : string.Empty;

                    decimal conversao = 0;
                    if (decimal.TryParse(resposta, style, culture, out conversao))
                    {
                        if (total == 0)
                        {
                            total = conversao;
                            continue;
                        }

                        total -= conversao;
                    }
                }
            }
            return total;
        }

        public decimal ObterTotalMultiplicacao(Campo campo, int processoResposta, int idUsuario = 0)
        {
            decimal total = 0;

            if (campo.ListaCamposVinculados.Any())
            {
                Usuario usuario = new ManterUsuario().ObterUsuarioPorID(idUsuario);
                CultureInfo culture;
                NumberStyles style;
                culture = CultureInfo.CreateSpecificCulture("pt-BR");
                style = NumberStyles.Number;
                foreach (Campo item in campo.ListaCamposVinculados)
                {
                    CampoResposta campoResposta = new BMCampoResposta().ObterPorCampoProcessoResposta(item.ID, processoResposta);

                    string resposta = campoResposta != null ? campoResposta.Resposta : string.Empty;

                    decimal conversao = 0;
                    if (decimal.TryParse(resposta, style, culture, out conversao))
                    {
                        if (total == 0)
                        {
                            total = conversao;
                            continue;
                        }

                        total *= conversao;
                    }
                }
            }
            return total;
        }

        public List<int> ObterIdsCamposVinculados(Campo campo)
        {
            var ids = new List<int>();

            foreach (Campo item in campo.ListaCamposVinculados)
            {
                ids.Add(item.ID);
            }

            return ids;
        }

        public IQueryable<Campo>ObterTodos()
        {
            return bmCampo.ObterTodosIQueryable();
        }

        public IQueryable<Campo> ObterTodosIQueryable()
        {
            return bmCampo.ObterTodosIQueryable();
        }
    }
}
