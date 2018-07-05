using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterNivelOcupacional : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMNivelOcupacional bmNivelOcupacional = null;

        #endregion

        #region "Construtor"

        public ManterNivelOcupacional()
            : base()
        {
            bmNivelOcupacional = new BMNivelOcupacional();
        }

        #endregion
        
        #region "Métodos Públicos"

        public void IncluirNivelOcupacional(NivelOcupacional pNivelOcupacional)
        {
            bmNivelOcupacional.ValidarNivelOcupacionalInformado(pNivelOcupacional);

            //pNivelOcupacional.DataAlteracao = DateTime.Now;

            bmNivelOcupacional.Salvar(pNivelOcupacional);
        }

        public void AlterarNivelOcupacional(NivelOcupacional pNivelOcupacional)
        {
            //pNivelOcupacional.DataAlteracao = DateTime.Now;
            bmNivelOcupacional.Salvar(pNivelOcupacional);
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalPorNome(string pNome)
        {
            if (string.IsNullOrWhiteSpace(pNome)) throw new AcademicoException("Nome. Campo Obrigatório");
            return bmNivelOcupacional.ObterPorNome(pNome);
        }

        public void ExcluirNivelOcupacional(int IdNivelOcupacional)
        {
            try
            {
                NivelOcupacional nivelOcupacional = null;

                if (IdNivelOcupacional > 0)
                {
                    nivelOcupacional = bmNivelOcupacional.ObterPorID(IdNivelOcupacional);
                }

                bmNivelOcupacional.Excluir(nivelOcupacional);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<NivelOcupacional> ObterTodosNivelOcupacional()
        {
            return bmNivelOcupacional.ObterTodos();
        }

        public NivelOcupacional ObterNivelOcupacionalPorID(int pId)
        {
            return bmNivelOcupacional.ObterPorID(pId);
        }

        public IEnumerable<NivelOcupacional> ObterObrigatorios()
        {
            return bmNivelOcupacional.ObterObrigatorios();
        }

        #endregion
    }
}
