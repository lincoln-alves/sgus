using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterPublicacaoSaberUsuario : BusinessProcessBase
    {
        private BMPublicacaoSaberUsuario publicacaoSaberUsuario = null;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterPublicacaoSaberUsuario()
            : base()
        {
            publicacaoSaberUsuario = new BMPublicacaoSaberUsuario();
        }


        public IList<PublicacaoSaberUsuario> ObterPorIdUsuario(int pIdUsuario)
        {
            return publicacaoSaberUsuario.ObterPorIdUsuario(pIdUsuario);
        }

       
    }
}

