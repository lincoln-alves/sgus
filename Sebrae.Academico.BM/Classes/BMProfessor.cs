using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;


namespace Sebrae.Academico.BM.Classes
{
    public class BMProfessor : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<Professor> repositorio;

        public BMProfessor()
        {
            repositorio = new RepositorioBase<Professor>();
        }

        public void ValidarProfessor(Professor pProfessor)
        {

            this.ValidarInstancia(pProfessor);

            if (string.IsNullOrWhiteSpace(pProfessor.Nome))
                throw new AcademicoException("Nome não Informado. Campo Obrigatório");

        }

        public void Salvar(Professor pProfessor)
        {
            ValidarProfessor(pProfessor);
            //pProfessor.DataCadastro = DateTime.Now;
            repositorio.Salvar(pProfessor);
        }

        public IList<Professor> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public Professor ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }



        public IList<Professor> ObterPorFiltros(Professor professor)
        {
            var query = repositorio.session.Query<Professor>();

            if (professor != null)
            {

                if (!string.IsNullOrWhiteSpace(professor.Nome))
                    query = query.Where(x => x.Nome.ToUpper().Contains(professor.Nome.ToUpper()));

                if (!string.IsNullOrWhiteSpace(professor.Cpf))
                    query = query.Where(x => x.Cpf.Equals(professor.Cpf));

            }

            return query.ToList();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Excluir(Professor Professor)
        {
            if (this.ValidarDependencias(Professor))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Professor.");

            repositorio.Excluir(Professor);

        }

        protected override bool ValidarDependencias(object pProfessor)
        {
            Professor formaAquisicao = (Professor)pProfessor;

            return (formaAquisicao.ListaTurma != null && formaAquisicao.ListaTurma.Count > 0);
        }
        
    }
}
