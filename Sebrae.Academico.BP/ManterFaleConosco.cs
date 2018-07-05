using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterFaleConosco : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMFaleConosco bmFaleConosco = null;

        #endregion

        #region "Construtor"

        public ManterFaleConosco()
            : base()
        {
            bmFaleConosco = new BMFaleConosco();
        }

        #endregion
        
        #region "Métodos Públicos"

        public LogFaleConosco ObterPorID(int pId)
        {
            return bmFaleConosco.ObterPorID(pId);
        }

        /// <summary>
        /// Obtém as mensagens enviadas por um usuário.
        /// </summary>
        /// <param name="pCPF">CPF do usuário que enviou a mensagem</param>
        /// <returns>Lista de mensagens do usuário</returns>
        public IList<LogFaleConosco> ListarPorCPF(string pCPF)
        {
            return bmFaleConosco.ListarPorCPF(pCPF);
        }

        #endregion
    }
}
