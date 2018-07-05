using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.DTO.Filtros;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Classes
{
    public class BMViewConfiguracaoPagamentoPublicoAlvo : BusinessManagerBase
    {

        private RepositorioBase<ViewConfiguracaoPagamentoPublicoAlvo> repositorio;

        public BMViewConfiguracaoPagamentoPublicoAlvo()
            : base()
        {
            repositorio = new RepositorioBase<ViewConfiguracaoPagamentoPublicoAlvo>();
        }


        public IList<ViewConfiguracaoPagamentoPublicoAlvo> ObterInformacoesDePagamento(ConfiguracaoPagamentoDTOFiltro filtro)
        {
            IList<ViewConfiguracaoPagamentoPublicoAlvo> ListaConfiguracaoPagamentoPublicoAlvo = null;

            var query = repositorio.session.Query<ViewConfiguracaoPagamentoPublicoAlvo>();
            query = query.Fetch(x => x.Usuario);

            if (filtro.IdConfiguracaoPagamento == 0)
            {
                throw new AcademicoException("A configuração do pagamento é obrigatória");
            }

            query = query.Where(x => x.ConfiguracaoPagamento.ID == filtro.IdConfiguracaoPagamento);
            if (filtro.IdNivelOcupacional > 0)
            {
                query = query.Where(x => x.Usuario.NivelOcupacional.ID == filtro.IdNivelOcupacional);
            }

            if (filtro.IdPerfil > 0)
            {
                query = query.Where(x => x.Usuario.ListaPerfil.Any(y => y.Perfil.ID == filtro.IdPerfil));
            }

            if (filtro.IdUF > 0)
            {
                query = query.Where(x => x.Usuario.UF.ID == filtro.IdUF);
            }

            //Nome do Usuário
            if (!string.IsNullOrWhiteSpace(filtro.NomeUsuario))
            {
                query = query.Where(x => x.Usuario != null && x.Usuario.Nome != string.Empty &&
                                    x.Usuario.Nome.ToUpper().Contains(filtro.NomeUsuario.ToUpper()));
            }

            ListaConfiguracaoPagamentoPublicoAlvo = query.ToList<ViewConfiguracaoPagamentoPublicoAlvo>();

            return ListaConfiguracaoPagamentoPublicoAlvo;


        }
    }
}
