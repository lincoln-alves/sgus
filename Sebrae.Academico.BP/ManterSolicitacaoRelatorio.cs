using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterSolicitacaoRelatorio : BusinessProcessBase
    {

        private BMSolicitacaoRelatorio _bmSolicitacao;

        public ManterSolicitacaoRelatorio()
        {
            _bmSolicitacao = new BMSolicitacaoRelatorio();
        }

        public void Salvar(SolicitacaoRelatorio model)
        {
            try
            {
                _bmSolicitacao.Salvar(model);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void Excluir(int idModel)
        {
            var solicitacao = _bmSolicitacao.ObterPorId(idModel);
            _bmSolicitacao.Excluir(solicitacao);
        }

        public IEnumerable<SolicitacaoRelatorio> ObterTodas()
        {
            return _bmSolicitacao.ObterTodos();
        }

        public SolicitacaoRelatorio ObterPorID(int idModel)
        {
            return _bmSolicitacao.ObterPorId(idModel);
        }

        public IEnumerable<SolicitacaoRelatorio> ObterPorUsuario(int idUsuario)
        {
            return _bmSolicitacao.ObterPorUsuario(idUsuario);
        }

        public IEnumerable<SolicitacaoRelatorio> ObterDoUsuarioLogado()
        {
            return _bmSolicitacao.ObterPorUsuario(new ManterUsuario().ObterUsuarioLogado().ID);
        }
    }
}
