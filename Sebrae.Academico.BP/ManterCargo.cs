using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterCargo : BusinessProcessBase
    {
        private readonly BMCargo _bmCargo;

        public ManterCargo()
        {
            _bmCargo = new BMCargo();
        }

        public IQueryable<Cargo> ObterTodos()
        {
            return _bmCargo.ObterTodos();
        }

        public IQueryable<Cargo> ObterDiretoria(int idUf)
        {
            return ObterTodos().Where(x => x.TipoCargo == EnumTipoCargo.Diretoria && x.Uf.ID == idUf).OrderBy(x => x.Ordem);
        }

        public Cargo ObterPorId(int idCargo)
        {
            return _bmCargo.ObterPorId(idCargo);
        }

        public void Salvar(Cargo cargo)
        {
            _bmCargo.Salvar(cargo);
        }

        public void FazerMerge(Cargo model)
        {
            _bmCargo.FazerMerge(model);
        }

        public void Evict(Cargo cargo)
        {
            _bmCargo.Evict(cargo);
        }

        public void Dispose()
        {
            _bmCargo.Dispose();
        }
    }
}