using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    [Serializable]
    public class StatusMatricula : EntidadeBasica
    {
        public virtual bool Especifico { get; set; }
        public virtual IList<CategoriaConteudo> ListaCategoriaConteudo { get; set; }



        /// <summary>
        /// Gambiarra para ocultar os status que alguns perfis não podem ver. Precisa ser parametrizado pelo banco,
        /// e permitir a alteração através de uma tela.
        /// </summary>
        /// <param name="usuarioLogado">Usuário logado que poderá ou não visualizar o Status.</param>
        /// <returns></returns>
        public virtual bool PermiteVisualizacao(Usuario usuarioLogado)
        {
            if (usuarioLogado.IsConsultorEducacional())
            {
                return ID != (int)enumStatusMatricula.CanceladoAdm &&
                       ID != (int)enumStatusMatricula.CanceladoGestor &&
                       ID != (int)enumStatusMatricula.CanceladoAluno &&
                       ID != (int)enumStatusMatricula.CanceladoTurma;
            }

            return true;
        }
    }
}
