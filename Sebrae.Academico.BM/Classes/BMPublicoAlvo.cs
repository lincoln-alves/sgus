using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPublicoAlvo : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<PublicoAlvo> repositorio;

        public BMPublicoAlvo()
        {
            repositorio = new RepositorioBase<PublicoAlvo>();
        }

        public void Salvar(PublicoAlvo publicoAlvo, bool verificarUf = false)
        {
			var query = repositorio.session.Query<PublicoAlvo>().Where(x =>
						x.Nome.Trim() == publicoAlvo.Nome.Trim() && x.ID != publicoAlvo.ID);
							
			if(verificarUf)
				query = query.Where(x => publicoAlvo.UF != null && x.UF.ID == publicoAlvo.UF.ID);
			
            if (query.Any())
			{
                throw new AcademicoException("Já existe no banco de dados um registro com esse nome"
											+ (verificarUf ? " para sua UF" : "") + ".");
            }

            repositorio.Salvar(publicoAlvo);
        }

        public IList<PublicoAlvo> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public PublicoAlvo ObterPorID(int id)
        {
            return repositorio.ObterPorID(id);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Excluir(int idPublicoAlvo)
        {
            repositorio.Excluir(ObterPorID(idPublicoAlvo));
        }

        public IList<PublicoAlvo> ObterPorFiltro(PublicoAlvo pPublicoAlvo)
        {
            var query = repositorio.session.Query<PublicoAlvo>();

            if (pPublicoAlvo != null)
            {
                if (!string.IsNullOrWhiteSpace(pPublicoAlvo.Nome))
                    query = query.Where(x => x.Nome.ToUpper().Contains(pPublicoAlvo.Nome.ToUpper()));
            }

            return query.ToList();
        }
    }
}
