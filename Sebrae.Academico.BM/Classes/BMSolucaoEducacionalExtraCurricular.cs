using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSolucaoEducacionalExtraCurricular : BusinessManagerBase
    {
        private RepositorioBase<SolucaoEducacionalExtraCurricular> repositorio;

        public BMSolucaoEducacionalExtraCurricular()
        {
            repositorio = new RepositorioBase<SolucaoEducacionalExtraCurricular>();
        }

        public SolucaoEducacionalExtraCurricular ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public SolucaoEducacionalExtraCurricular ObterPorNome(SolucaoEducacionalExtraCurricular pSolucao)
        {
            //SolucaoEducacionalExtraCurricular se = repositorio.GetByProperty("Nome", pSolucao.Nome).FirstOrDefault();

            var query = repositorio.session.Query<SolucaoEducacionalExtraCurricular>();
            SolucaoEducacionalExtraCurricular se = query.FirstOrDefault(x => x.Nome == pSolucao.Nome);

            if (se == null)
            {
                se = new SolucaoEducacionalExtraCurricular()
                {
                    FormaAquisicao = pSolucao.FormaAquisicao,
                    Nome = pSolucao.Nome,
                    Usuario = pSolucao.Usuario,
                    Auditoria = new Auditoria(null)
                };

                Salvar(se);
            };


            return se;
        }

        private void Salvar(SolucaoEducacionalExtraCurricular se)
        {
            repositorio.Salvar(se);
        }
    }
}
