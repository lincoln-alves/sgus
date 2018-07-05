using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP
{
    public class ManterEtapaRespostaPermissao : BusinessProcessBase, IDisposable
    {
        private BMEtapaRespostaPermissao bm;

        public ManterEtapaRespostaPermissao()
        {
            bm = new BMEtapaRespostaPermissao();
        }

        public IList<EtapaRespostaPermissao> ObterTodos()
        {
            return bm.ObterTodos();
        }

        public EtapaRespostaPermissao ObterPorId(int id)
        {
            return bm.ObterPorId(id);
        }

        public IList<Usuario> ObterTodosUsuarios(EtapaResposta etapa)
        {
            return etapa.PermissoesNucleoEtapaResposta.Select(x => x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario).ToList();
        }

        public void Salvar(EtapaRespostaPermissao permissao)
        {
            bm.Salvar(permissao);
        }

        public void Salvar(IList<EtapaRespostaPermissao> permissoes)
        {
            bm.Salvar(permissoes);
        }

        public void Excluir(EtapaRespostaPermissao permissao)
        {
            bm.Excluir(permissao);
        }

        public void ExcluirTodosFkEtapaPermissaoNucleo(int fk)
        {
            bm.ExcluirTodosFkEtapaPermissaoNucleo(fk);
        }

        public void Dispose()
        {
            bm.Dispose();
        }

        internal void IncluirPermissoes(EtapaResposta etapaResposta, IEnumerable<int> idsPermissoes)
        {
            List<EtapaRespostaPermissao> permissoes = new List<EtapaRespostaPermissao>();

            foreach (var id in idsPermissoes)
            {
                EtapaRespostaPermissao permissao = new EtapaRespostaPermissao
                {
                    EtapaResposta = new EtapaResposta { ID = etapaResposta.ID },
                    EtapaPermissaoNucleo = new EtapaPermissaoNucleo { ID = id }
                };

                permissoes.Add(permissao);
            }

            Salvar(permissoes);
        }
    }
}
