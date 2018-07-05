using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterMissaoMedalha
    {
        private readonly BMMissaoMedalha _bmMissaoMedalha;

        public ManterMissaoMedalha()
        {
            _bmMissaoMedalha = new BMMissaoMedalha();
        }

        public MissaoMedalha ObterPorId(int idMissaoMedalha)
        {
            return _bmMissaoMedalha.ObterPorId(idMissaoMedalha);
        }

        public MissaoMedalha ObterPorUsuarioTrilhaMissao(int idUsuarioTrilha, int idMissao)
        {
            return _bmMissaoMedalha.ObterPorUsuarioTrilhaMissao(idUsuarioTrilha, idMissao);
        }

        public int ObterQuantidadePorUsuarioTrilha(int idUsuarioTrilha)
        {
            return _bmMissaoMedalha.ObterQuantidadePorUsuarioTrilha(idUsuarioTrilha);
        }

        
        public void Salvar(MissaoMedalha missaoMedalha)
        {
            _bmMissaoMedalha.salvar(missaoMedalha);
        }

        public void registrarMedalha(MissaoMedalha missaoMedalha)
        {
            MissaoMedalha _missaoMedalhaBD = _bmMissaoMedalha
                .ObterPorUsuarioTrilhaMissao(missaoMedalha.UsuarioTrilha.ID, missaoMedalha.Missao.ID);

            if(_missaoMedalhaBD != null)
            {
                if(_missaoMedalhaBD.Medalhas < missaoMedalha.Medalhas)
                {
                    _missaoMedalhaBD.Medalhas = missaoMedalha.Medalhas;
                    _bmMissaoMedalha.salvar(_missaoMedalhaBD);
                }
            }else{
                _bmMissaoMedalha.salvar(missaoMedalha);
            }
        }
    }
}
