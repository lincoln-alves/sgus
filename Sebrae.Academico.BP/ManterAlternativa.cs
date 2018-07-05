using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterAlternativa : BusinessProcessBase
    {

        private BMAlternativa bmAlternativa = null;

        public ManterAlternativa()
            : base()
        {
            bmAlternativa = new BMAlternativa();
        }

        private void ValidarAlternativa(Alternativa model)
        {
            if (String.IsNullOrEmpty(model.Nome)) throw new AcademicoException("Nome é obrigatório");
        }

        public void Incluir(Alternativa model)
        {
            try
            {
                ValidarAlternativa(model);
                this.PreencherInformacoesDeAuditoria(model);
                bmAlternativa.Salvar(model);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarOrdemCampos(dynamic obj)
        {
            foreach (var item in obj)
            {
                Alternativa model = bmAlternativa.ObterPorId(Convert.ToInt16(item["id"]));
                model.Ordem = Convert.ToByte(item["ordem"]);
                bmAlternativa.Salvar(model);
            }
        }

        public void Excluir(int IdModel)
        {
            try
            {
                Alternativa campo = null;

                if (IdModel > 0)
                {
                    campo = bmAlternativa.ObterPorId(IdModel);
                    bmAlternativa.Excluir(campo);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<Alternativa> ObterTodas()
        {
            return bmAlternativa.ObterTodos();
        }

        public void Alterar(Alternativa model)
        {
            try
            {
                ValidarAlternativa(model);
                this.PreencherInformacoesDeAuditoria(model);
                bmAlternativa.Salvar(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(Alternativa model)
        {
            //base.PreencherInformacoesDeAuditoria(pCapacitacao);
            //pCapacitacao.ListaModulos.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public Alternativa ObterPorID(int IdModel)
        {
            return bmAlternativa.ObterPorId(IdModel);
        }

        public IList<Alternativa> ObterPorCampoId(int IdCampo)
        {
            IList<Alternativa> ListaCampo = null;

            try
            {
                ListaCampo = bmAlternativa.ObterPorCampoId(IdCampo);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

            return ListaCampo;
        }
    }
}
