using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.BM.AutoMapper;
using AutoMapper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCampoMeta : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<CampoMeta> repositorio;

        #endregion

        #region "Construtor"

        public BMCampoMeta()
        {
            repositorio = new RepositorioBase<CampoMeta>();
        }

        #endregion

        public IList<CampoMeta> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoMeta>();
            return query.ToList<CampoMeta>();
        }

        public CampoMeta ObterPorId(int id)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoMeta>();
            return query.Where(x => x.ID == id).FirstOrDefault();
        }

        public IList<CampoMeta> ObterPorCampo(Campo campo)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoMeta>();

            IList<CampoMeta> result;
            if (campo.ID != 0)
            {

                var queryCM = repositorio.session.Query<CampoMetaValue>();

                query.ToList().ForEach(x => x.ListaMetaValues = queryCM.Where(m => m.Campo.ID == campo.ID && x.ID == m.CampoMeta.ID).ToList());

                result = query
                    .Where(
                        x =>
                            (x.CampoTipo == campo.TipoCampo && x.CampoTipoDado == null) ||
                            (x.CampoTipo == campo.TipoCampo && x.CampoTipoDado == campo.TipoDado))
                    .ToList();

                // Se não tem ID não tenta pegar os valores específicos ao campo
            }
            else
            {
                var consulta = from x in query
                    where (x.CampoTipo == campo.TipoCampo && x.CampoTipoDado == null) ||
                          (x.CampoTipo == campo.TipoCampo && x.CampoTipoDado == campo.TipoDado)
                    select
                        new CampoMeta
                        {
                            ID = x.ID,
                            MetaKey = x.MetaKey,
                            MetaNome = x.MetaNome,
                            MetaDescription = x.MetaDescription
                        };

                result = consulta.ToList();
            }
            return result;
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(CampoMeta pCampoMeta)
        {
            repositorio.Salvar(pCampoMeta);
        }

    }

}
