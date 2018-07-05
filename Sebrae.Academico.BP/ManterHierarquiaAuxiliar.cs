using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterHierarquiaAuxiliar : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMHierarquiaAuxiliar bmHierarquiaAux = null;

        #endregion

        #region "Construtor"

        public ManterHierarquiaAuxiliar()
            : base()
        {
            bmHierarquiaAux = new BMHierarquiaAuxiliar();
        }

        #endregion

        public void ExcluirHierarquiaAuxiliar(int IdHierarquia)
        {

            try
            {
                HierarquiaAuxiliar oferta = null;

                if (IdHierarquia > 0)
                {
                    oferta = bmHierarquiaAux.ObterPorId(IdHierarquia);
                }

                bmHierarquiaAux.ExcluirHierarquiaAuxiliar(oferta);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public HierarquiaAuxiliar ObterPorId(int idHierarquiaAux)
        {
            return bmHierarquiaAux.ObterPorId(idHierarquiaAux);
        }

        public IQueryable<HierarquiaAuxiliar> ObterTodasHierquiaAuxiliares()
        {
            return bmHierarquiaAux.ObterTodos();
        }

        public IList<HierarquiaAuxiliar> ObterPorNomeDeUsuario(string nome)
        {
            return bmHierarquiaAux.ObterPorNomeDeUsuario(nome);
        }

        public void AlterarHierarquiaAuxiliar(HierarquiaAuxiliar pHierarquiaAuxiliar)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pHierarquiaAuxiliar);
                bmHierarquiaAux.Salvar(pHierarquiaAuxiliar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}