using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioCargo : BusinessProcessBase
    {
        private readonly BMUsuarioCargo _bmUsuarioCargo;

        public ManterUsuarioCargo()
        {
            _bmUsuarioCargo = new BMUsuarioCargo();
        }

        public IQueryable<UsuarioCargo> ObterPorUfTipoCargo(Uf uf, EnumTipoCargo tipoCargo)
        {
            return _bmUsuarioCargo.ObterPorUfTipoCargo(uf, tipoCargo);
        }

        public UsuarioCargo ObterPorCargo(Cargo cargo)
        {
            return _bmUsuarioCargo.ObterPorCargo(cargo);
        }

        public IQueryable<UsuarioCargo> ObterTodos()
        {
            return _bmUsuarioCargo.ObterTodos();
        }

        public void Salvar(Usuario usuario, Cargo cargo, bool mover = false)
        {
            _bmUsuarioCargo.Salvar(usuario, cargo, mover);
        }

        public void Salvar(UsuarioCargo usuarioCargo)
        {
            _bmUsuarioCargo.Salvar(usuarioCargo);
        }

        public void Remover(int usuarioCargoId)
        {
            _bmUsuarioCargo.Remover(usuarioCargoId);
        }

        public void Remover(UsuarioCargo usuarioCargo)
        {
            _bmUsuarioCargo.Remover(usuarioCargo);
        }

        public UsuarioCargo ObterPorId(int cargoId)
        {
            return _bmUsuarioCargo.ObterPorId(cargoId);
        }
    }
}