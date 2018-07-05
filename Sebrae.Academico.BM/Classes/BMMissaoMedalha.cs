using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMissaoMedalha : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<MissaoMedalha> _repositorio;

        public BMMissaoMedalha()
        {
            _repositorio = new RepositorioBase<MissaoMedalha>();
        }
        public MissaoMedalha ObterPorId(int pId)
        {
            return _repositorio.session.Query<MissaoMedalha>().FirstOrDefault(x => x.ID == pId);
        }
        public MissaoMedalha ObterPorUsuarioTrilhaMissao(int idUsuarioTrilha, int idMissao)
        {
            return _repositorio.session.Query<MissaoMedalha>()
                .Where(x => x.Missao.ID == idMissao && x.UsuarioTrilha.ID == idUsuarioTrilha)
                .FirstOrDefault();
        }
        public int ObterQuantidadePorUsuarioTrilha(int idUsuarioTrilha)
        {
            return _repositorio.session.Query<MissaoMedalha>()
                .Where(x => x.UsuarioTrilha.ID == idUsuarioTrilha)
                .Count();
        }        
        public void salvar(MissaoMedalha model)
        {
            if(model.ID > 0)
            {
                _repositorio.FazerMerge(model);
            }
            else
            {
                _repositorio.Salvar(model);
            }
        }
        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public void Evict(Cargo cargo)
        {
            _repositorio.Evict(cargo);
        }
    }
}
