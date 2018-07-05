using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using System.Collections;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogAcessoTrilha: BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        RepositorioBase<LogAcessoTrilha> repositorio;

        #endregion

        #region "Construtor"

        public BMLogAcessoTrilha()
        {
            repositorio = new RepositorioBase<LogAcessoTrilha>();
        }

        #endregion

        #region "Métodos Públicos"
        
        public void Salvar(LogAcessoTrilha PLogUsuarioTrilha)
        {
            repositorio.Salvar(PLogUsuarioTrilha);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
