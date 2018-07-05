using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMetaInstitucional : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<MetaInstitucional> repositorio;

        public BMMetaInstitucional()
        {
            repositorio = new RepositorioBase<MetaInstitucional>();
        }

        public IList<MetaInstitucional> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IList<MetaInstitucional> ObterMetasInstitucionaisValidas()
        {
            var query = repositorio.session.Query<MetaInstitucional>();
            query = query.Where(x => x.DataInicioCiclo.Date <= DateTime.Today && x.DataFimCiclo.Date >= DateTime.Today);
            return query.ToList();
        }
        public IList<MetaInstitucional> ObterPorFiltro(MetaInstitucional pMetaInstitucional)
        {
            var query = repositorio.session.Query<MetaInstitucional>();

            if (!string.IsNullOrWhiteSpace(pMetaInstitucional.Nome))
                query = query.Where(x => x.Nome.ToLower().Contains(pMetaInstitucional.Nome.ToLower()));

            if (pMetaInstitucional.DataInicioCiclo != DateTime.Parse("1-1-0001"))
                query = query.Where(x => x.DataInicioCiclo >= pMetaInstitucional.DataInicioCiclo);

            if (pMetaInstitucional.DataFimCiclo != DateTime.Parse("1-1-0001"))
                query = query.Where(x => x.DataFimCiclo <= pMetaInstitucional.DataFimCiclo);

            return query.ToList();
        }

        public void Salvar(MetaInstitucional pMetaInstitucional)
        {
            ValidarMetaInstitucionalInformada(pMetaInstitucional);
            repositorio.Salvar(pMetaInstitucional);
        }

        private void ValidarMetaInstitucionalInformada(MetaInstitucional pMetaInstitucional)
        {
            this.ValidarInstancia(pMetaInstitucional);

            if (string.IsNullOrWhiteSpace(pMetaInstitucional.Nome))
                throw new Exception("Nome não informado: Campo Obrigatório!");

            if (pMetaInstitucional.DataInicioCiclo.Date == new DateTime(1, 1, 1).Date)
                throw new Exception("Data de Inicio do Ciclo não informada: Campo Obrigatório!");

            if (pMetaInstitucional.DataFimCiclo.Date == new DateTime(1, 1, 1).Date)
                throw new Exception("Data de Fim do Ciclo não informada: Campo Obrigatório!");

            if (pMetaInstitucional.DataInicioCiclo > pMetaInstitucional.DataFimCiclo)
                throw new Exception("Data de Fim do Ciclo não pode ser maior que a Data de Inicio de Ciclo!");

        }

        public MetaInstitucional ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(MetaInstitucional metaInstitucional)
        {
            repositorio.Excluir(metaInstitucional);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
