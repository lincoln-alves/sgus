using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPrograma : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Programa> repositorio;


        public BMPrograma()
        {
            repositorio = new RepositorioBase<Programa>();
        }

        //protected override bool ValidarDependencias(object pParametro)
        //{
        //    NivelOcupacional nivelOcupacional = (NivelOcupacional)pParametro;
        //    return (nivelOcupacional.ListaUsuarios != null && nivelOcupacional.ListaUsuarios.Count > 0);
        //}

        protected override bool ValidarDependencias(object pPrograma)
        {
            Programa programa = (Programa)pPrograma;
            return ((programa.ListaMatriculaPrograma != null && programa.ListaMatriculaPrograma.Count > 0)); // ||
            //(programa.ListaPermissao != null && programa.ListaPermissao.Count > 0) ||
            //(programa.ListaTag != null && programa.ListaTag.Count > 0) ||
            //(programa.ListaSolucaoEducacional != null && programa.ListaSolucaoEducacional.Count > 0));
        }

        public void Excluir(Programa pPrograma)
        {
            if (this.ValidarDependencias(pPrograma))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Programa.");

            repositorio.Excluir(pPrograma);
        }

        public void ValidarProgramaInformado(Programa pPrograma)
        {

            this.ValidarInstancia(pPrograma);

            if (string.IsNullOrWhiteSpace(pPrograma.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            this.VerificarExistenciaDePrograma(pPrograma);
        }

        private void VerificarExistenciaDePrograma(Programa pPrograma)
        {
            Programa programa = this.ObterPorNome(pPrograma);

            if (programa != null)
            {
                if (pPrograma.ID != programa.ID)
                {
                    throw new AcademicoException(string.Format("O Programa '{0}' já está cadastrado",
                                                 pPrograma.Nome.Trim()));
                }
            }
        }

        public Programa ObterPorNome(Programa pPrograma)
        {
            //return repositorio.GetByProperty("Nome", pPrograma.Nome).FirstOrDefault();
            var query = repositorio.session.Query<Programa>();
            return query.FirstOrDefault(x => x.ID == pPrograma.ID);
        }

        public void Salvar(Programa pPrograma)
        {
            ValidarProgramaInformado(pPrograma);

            //Caso seja unico, descomentar a linha baixo e implementar
            //a verificacao por nome do programa.
            if (pPrograma.ID == 0)
            {
                if (this.ObterPorNome(pPrograma) != null)
                {
                    throw new AcademicoException("Já existe um registro de Programa com este nome.");
                }
            }

            repositorio.Salvar(pPrograma);

        }

        private IList<Usuario> BuscarporNome(Usuario ptrilha)
        {
            //return repositorio.GetByProperty("Nome", ptrilha.Nome);
            var query = repositorio.session.Query<Usuario>();
            return query.Where(x => x.Nome == ptrilha.Nome).ToList<Usuario>();
        }

        //public void AlterarPrograma(Programa pPrograma)
        //{
        //    ValidarPrograma(pPrograma);
        //    pPrograma.DataAlteracao = DateTime.Now;
        //    repositorio.Salvar(pPrograma);
        //}

        public IQueryable<Programa> ObterTodos()
        {
            return repositorio.session.Query<Programa>();
        }

        public Programa ObterPorId(int pId)
        {
            ////return repositorio.ObterPorID(pId);

            var query = repositorio.session.Query<Programa>();

            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IQueryable<Programa> ObterPorInscricoesAbertas()
        {
            return repositorio.session.Query<Programa>()
                .Where(
                    x =>
                        x.ListaCapacitacao.Any(
                            f =>
                                f.ListaTurmas.Any(t => t.DataInicio.HasValue && t.DataFim.HasValue && t.DataInicio <= DateTime.Today && t.DataFim >= DateTime.Today)) &&
                        x.Ativo);
        }

        public IList<Programa> ObterPorFiltro(Programa programa, bool isAtivo = false)
        {
            var query = repositorio.session.Query<Programa>(); //.Where(x => x.Ativo == programa.Ativo);

            if (programa.ID != 0)
                query = query.Where(x => x.ID == programa.ID);

            if (!string.IsNullOrWhiteSpace(programa.Nome))
                query = query.Where(x => x.Nome.Contains(programa.Nome));

            if (isAtivo)
            {
                query = query.Where(x => x.Ativo == programa.Ativo);
            }

            return query.ToList();
        }

        public IList<ProgramaSolucaoEducacional> ObterPorUsuario(int pIdUsuario)
        {
            var query = (from p in repositorio.session.Query<Programa>()

                         join ps in repositorio.session.Query<ProgramaSolucaoEducacional>() on
                         p.ID equals ps.Programa.ID

                         join pp in repositorio.session.Query<ViewProgramaPermissao>() on
                         p.ID equals pp.Programa.ID

                         where p.Ativo.Equals(true) &&
                         pp.Usuario.ID.Equals(pIdUsuario)

                         select ps).ToList();

            return query;

        }

        public IList<ProgramaSolucaoEducacional> ObterPrograSolucaoEducacional()
        {
            var query = repositorio.session.Query<ProgramaSolucaoEducacional>();

            query = query.Where(x => x.Programa.Ativo.Equals(true));
            return query.ToList();
        }

        public int ObterMaiorSequencia()
        {
            try
            {
                return repositorio.session.Query<Programa>().Max(x => x.Sequencia);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
