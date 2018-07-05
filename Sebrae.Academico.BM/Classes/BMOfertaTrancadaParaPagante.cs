using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMOfertaTrancadaParaPagante : BusinessManagerBase
    {
        private RepositorioBase<OfertaTrancadaParaPagante> repositorio = null;

        public BMOfertaTrancadaParaPagante()
        {
            repositorio = new RepositorioBase<OfertaTrancadaParaPagante>();
        }

        public List<OfertaTrancadaParaPagante> ObterPorFiltro(OfertaTrancadaParaPagante pOfertaTrancadaParaPagante)
        {
            var query = repositorio.session.Query<OfertaTrancadaParaPagante>();

            if (pOfertaTrancadaParaPagante.NivelOcupacional.ID >0)
                query = query.Where(x => x.NivelOcupacional.ID == pOfertaTrancadaParaPagante.NivelOcupacional.ID);

            if (pOfertaTrancadaParaPagante.Oferta.ID > 0)
                query = query.Where(x => x.Oferta.ID == pOfertaTrancadaParaPagante.Oferta.ID);

            return query.ToList();
        }

        public List<OfertaTrancadaParaPagante> ObterPorOferta(int idOferta)
        {
            var query = repositorio.session.Query<OfertaTrancadaParaPagante>();

            return query.Where(x=>x.Oferta.ID == idOferta).ToList();
        }

        public void Salvar(OfertaTrancadaParaPagante pOfertaTrancadaParaPagante)
        {
            repositorio.Salvar(pOfertaTrancadaParaPagante);
        }

        public void Excluir(OfertaTrancadaParaPagante pOfertaTrancadaParaPagante)
        {
            repositorio.Excluir(pOfertaTrancadaParaPagante);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
