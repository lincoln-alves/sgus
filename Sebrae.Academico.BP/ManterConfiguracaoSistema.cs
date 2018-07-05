using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterConfiguracaoSistema : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMConfiguracaoSistema bmConfiguracaoSistema = null;

        #endregion

        #region "Construtor"

        public ManterConfiguracaoSistema()
            : base()
        {
            bmConfiguracaoSistema = new BMConfiguracaoSistema();
        }

        #endregion
        
        #region "Métodos Públicos"

        public void IncluirConfiguracaoSistema(ConfiguracaoSistema pConfiguracaoSistema)
        {
            bmConfiguracaoSistema.Salvar(pConfiguracaoSistema);
        }

        public void IncluirConfiguracaoSistema(IList<ConfiguracaoSistema> pListaConfiguracaoSistema)
        {
            pListaConfiguracaoSistema.ToList().ForEach(x => base.PreencherInformacoesDeAuditoria(x));
            bmConfiguracaoSistema.Salvar(pListaConfiguracaoSistema);
        }

        public void AlterarFileServer(ConfiguracaoSistema pConfiguracaoSistema)
        {
            bmConfiguracaoSistema.Salvar(pConfiguracaoSistema);
        }

        public ConfiguracaoSistema ObterConfiguracaoSistemaPorID(int pId)
        {
            return bmConfiguracaoSistema.ObterPorID(pId);
        }

        public IList<ConfiguracaoSistema> ObterTodasConfiguracoesSistema()
        {
            return bmConfiguracaoSistema.ObterTodos();
        }

        //public void ExcluirConfiguracaoSistema(int IdConfiguracaoSistema)
        //{
        //    try
        //    {
        //        FileServer fileServer = null;

        //        if (IdConfiguracaoSistema > 0)
        //        {
        //            fileServer = bmConfiguracaoSistema.ObterPorID(IdConfiguracaoSistema);
        //        }

        //        bmConfiguracaoSistema.Excluir(fileServer);
        //    }
        //    catch (AcademicoException ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public IList<ConfiguracaoSistema> ObterFileServerPorFiltro(ConfiguracaoSistema pConfiguracaoSistema)
        //{
        //    return bmConfiguracaoSistema.ObterPorFiltro(pConfiguracaoSistema);
        //}

        #endregion
    }
}
