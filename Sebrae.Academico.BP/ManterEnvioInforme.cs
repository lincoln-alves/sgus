using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterEnvioInforme : BusinessProcessBase
    {
        private readonly BMEnvioInforme _bmInforme;

        public ManterEnvioInforme()
        {
            _bmInforme = new BMEnvioInforme();
        }
        
        public IQueryable<EnvioInforme> ObterPorInforme(int informeId)
        {
            return _bmInforme.ObterPorInforme(informeId);
        }

        public IEnumerable<EnvioInforme> ObterTodos()
        {
            return _bmInforme.ObterTodos();
        }

        public EnvioInforme ObterPorId(int id)
        {
            return _bmInforme.ObterPorId(id);
        }

        public void Excluir(int id)
        {
            _bmInforme.Excluir(ObterPorId(id));
        }

        public void Salvar(EnvioInforme envioInforme)
        {
            _bmInforme.Salvar(envioInforme);
        }

        public List<string> ObterDestinatarios(EnvioInforme envio)
        {
            if (envio.Usuario != null)
                return new List<string> { envio.Usuario.Email };

            var perfis = envio.Perfis.Select(x => x.ID).ToList();
            var niveis = envio.NiveisOcupacionais.Select(x => x.ID).ToList();
            var ufs = envio.Ufs.Select(x => x.ID).ToList();

            var usuarios = new ManterUsuario().ObterTodos();

            if (perfis.Any())
                usuarios = usuarios.Where(u => u.ListaPerfil.Any(up => perfis.Contains(up.ID)));

            if (niveis.Any())
                usuarios = usuarios.Where(u => niveis.Contains(u.NivelOcupacional.ID));

            if (ufs.Any())
                usuarios = usuarios.Where(u => ufs.Contains(u.UF.ID));

            return usuarios.Select(u => u.Email).Distinct().ToList();
        }
    }
}