using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Helpers
{
    public class LupaUsuarioHelper : BusinessProcessBase
    {

        private BMUsuario usuarioBM;
        private BMNivelOcupacional nivelOcupacionalBM;
        private BMUf ufBM;

        public LupaUsuarioHelper()
        {

        }

        public IList<NivelOcupacional> ObterListaNivelOcupacional()
        {
            nivelOcupacionalBM = new BMNivelOcupacional();
            return nivelOcupacionalBM.ObterTodos();
        }
        public IList<Uf> ObterListaUf()
        {
            ufBM = new BMUf();
            return ufBM.ObterTodos();
        }


        public IList<Usuario> ObterListaUsuarioPorFiltro(Usuario userFiltro, int qtdRegistros = 0)
        {
            usuarioBM = new BMUsuario();

            //Ao menos 1 parâmetro deve ser informado
            if (string.IsNullOrWhiteSpace(userFiltro.Nome) &&
                string.IsNullOrWhiteSpace(userFiltro.CPF) &&
                string.IsNullOrWhiteSpace(userFiltro.Nome) &&
                userFiltro.UF == null &&
                userFiltro.NivelOcupacional == null)
            {
                throw new AcademicoException("Informe ao menos 1 parâmetro");
            }
            
            IList<Usuario> ListaUsuarios = usuarioBM.ObterPorFiltros(userFiltro, qtdRegistros);

            if (ListaUsuarios != null && ListaUsuarios.Count() > qtdRegistros)
            {
                throw new AcademicoException("Foram encontrados mais de " + qtdRegistros + " resultados. Por Favor, refina a sua busca.");
            }

            if (ListaUsuarios != null && ListaUsuarios.Count == 0)
            {
                throw new AcademicoException("Nenhum registro foi encontrado!");
            }

            return ListaUsuarios;
        }
        
        public Usuario ObterUsuarioPorID(int pId)
        {
            return new BMUsuario().ObterPorId(pId);
        }
    }
}
