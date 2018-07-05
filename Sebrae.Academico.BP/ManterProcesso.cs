using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterProcesso : BusinessProcessBase
    {

        private BMProcesso bmProcesso = null;

        public ManterProcesso()
            : base()
        {
            bmProcesso = new BMProcesso();
        }

        public void IncluirProcesso(Processo model)
        {
            try
            {
                ValidarProcesso(model);
                this.PreencherInformacoesDeAuditoria(model);
                bmProcesso.Salvar(model);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ValidarProcesso(Processo processo)
        {
            if (String.IsNullOrEmpty(processo.Nome)) throw new AcademicoException("Nome é obrigatório");
        }

        public void ExcluirProcesso(int IdModel)
        {

            try
            {
                Processo model = null;

                if (IdModel > 0)
                {
                    model = bmProcesso.ObterPorId(IdModel);
                    //model.Ativo = false;
                    bmProcesso.Excluir(model);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<Processo> ObterTodosProcessos()
        {
            return bmProcesso.ObterTodos();
        }

        public IQueryable<Processo> ObterTodosIQueryable()
        {
            return bmProcesso.ObterTodosIQueryable();
        }
        
        public IList<Processo> ObterPorFiltro(Processo filtro)
        {
            return bmProcesso.ObterPorFiltro(filtro);
        }

        public void AlterarOrdemEtapas(dynamic obj, IEnumerable<byte> ordemOriginal)
        {
            var novaOrdem = new Dictionary<int, byte>();

            BMEtapa bmEtapa = new BMEtapa();
            foreach (var item in obj)
            {
                novaOrdem.Add(Convert.ToInt16(item["id"]), (byte)item["ordemBanco"]);
            }

            if (novaOrdem.Select(x => x.Value).SequenceEqual(ordemOriginal)) return;

            VersionarOrdem(novaOrdem);
        }

        private void VersionarOrdem(Dictionary<int, byte> novaOrdem)
        {
            if (novaOrdem.Count > 0)
            {
                var etapas = new List<Etapa>();
                var versoes = new List<EtapaVersao>();

                Processo processo = new ManterEtapa().ObterPorID(novaOrdem.FirstOrDefault().Key).Processo;

                int? ultimaVersao = new ManterEtapaVersao().ObterVersaoAtualProcesso(processo);
                ultimaVersao++;

                using (var manter = new ManterEtapa())
                {
                    int ordem = 0;

                    foreach (var etapa in novaOrdem)
                    {
                        var novaEtapa = manter.ObterPorID(etapa.Key);
                        novaEtapa.Ordem = (byte)ordem++;
                        etapas.Add(novaEtapa);

                        versoes.Add(new EtapaVersao
                        {
                            Etapa = novaEtapa,
                            Ordem = novaEtapa.Ordem,
                            Versao = ultimaVersao.Value
                        });
                    }

                    manter.Salvar(etapas);
                }

                using (var manter = new ManterEtapaVersao())
                {
                    manter.Salvar(versoes);
                }
            }
        }

        public bool PossuiEtapas(int idProcesso)
        {
            return bmProcesso.PossuiEtapas(idProcesso);
        }

        public void AlterarProcesso(Processo model)
        {
            try
            {
                ValidarProcesso(model);
                this.PreencherInformacoesDeAuditoria(model);
                bmProcesso.Salvar(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(Processo processo)
        {
            //base.PreencherInformacoesDeAuditoria(pCapacitacao);
            //pCapacitacao.ListaModulos.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public Processo ObterPorID(int IdModel)
        {
            return bmProcesso.ObterPorId(IdModel);
        }

        public void DuplicarObjeto(int idProcesso)
        {
            bmProcesso.DuplicarObjeto(idProcesso);
        }
    }
}
