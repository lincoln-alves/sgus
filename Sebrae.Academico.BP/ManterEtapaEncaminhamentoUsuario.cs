using System;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.BM.Classes;
using System.Linq;
using System.Collections.Generic;

namespace Sebrae.Academico.BP
{
    public class ManterEtapaEncaminhamentoUsuario : BusinessProcessBase
    {
        private readonly BMEtapaEncaminhamentoUsuario _bmEtapaEncaminhamentoUsuario;

        public ManterEtapaEncaminhamentoUsuario()
        {
            _bmEtapaEncaminhamentoUsuario = new BMEtapaEncaminhamentoUsuario();
        }

        public IQueryable<EtapaEncaminhamentoUsuario> ObterTodosIQueryable()
        {
            return _bmEtapaEncaminhamentoUsuario.ObterTodosIQueryable();
        }

        public void Salvar(EtapaEncaminhamentoUsuario EtapaEncaminhamentoUsuario)
        {
            try
            {
                _bmEtapaEncaminhamentoUsuario.Salvar(EtapaEncaminhamentoUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
