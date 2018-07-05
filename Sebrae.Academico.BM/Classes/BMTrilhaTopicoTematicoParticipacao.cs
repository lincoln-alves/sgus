using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaTopicoTematicoParticipacao : BusinessManagerBase
    {
        private RepositorioBase<PontoSebraeParticipacao> repositorio;


        public BMTrilhaTopicoTematicoParticipacao()
        {
            repositorio = new RepositorioBase<PontoSebraeParticipacao>();
        }

        public IQueryable<PontoSebraeParticipacao> ObterTodos()
        {
            return repositorio.ObterTodos() as IQueryable<PontoSebraeParticipacao>;
        }

        public PontoSebraeParticipacao ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public PontoSebraeParticipacao ObterPorTopicoTematico(int id)
        {
            var query = repositorio.session.Query<PontoSebraeParticipacao>();

            return query.FirstOrDefault(x => x.PontoSebrae.ID == id);
        }

        public PontoSebraeParticipacao ObterPorUsuarioPontoSebrae(UsuarioTrilha usuarioTrilha, PontoSebrae pontoSebrae)
        {
            var query = repositorio.session.Query<PontoSebraeParticipacao>();

            return query.FirstOrDefault(x => x.UsuarioTrilha == usuarioTrilha && x.PontoSebrae.ID == pontoSebrae.ID);
        }

        public void Salvar(PontoSebraeParticipacao model)
        {
            //Caso seja unico, descomentar a linha baixo e implementar
            //a verificacao por nome do programa.
            if (model.ID == 0)
            {
                if (this.ObterPorId(model.ID) != null)
                {
                    throw new AcademicoException("Já existe um registro.");
                }
            }

            repositorio.Salvar(model);
        }

        public void Excluir(PontoSebraeParticipacao model)
        {
            repositorio.Excluir(model);
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
