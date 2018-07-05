using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterProfessor : BusinessProcessBase
    {
        private BMProfessor bmProfessor = null;
        
        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterProfessor()
            : base()
        {
            bmProfessor = new BMProfessor();
        }

        public IList<DTORelatorioDadosProfessor> ConsultarDadosProfessor(string pNome, string pCpf, int? idUf) {
            var lstParam = new Dictionary<string, object> {
                { "p_Nome", pNome },
                { "p_CPF", pCpf },
                { "p_IdUf", idUf }
            };

            return bmProfessor.ExecutarProcedure<DTORelatorioDadosProfessor>("SP_REL_DADOS_PROFESSOR", lstParam);
        }

        public void IncluirProfessor(Professor pProfessor)
        {
            bmProfessor.Salvar(pProfessor);
        }

        public void AlterarProfessor(Professor pProfessor)
        {
            bmProfessor.Salvar(pProfessor);
        }

        public IList<Professor> ObterTodosProfessores()
        {
            return bmProfessor.ObterTodos();
        }

        public Professor ObterProfessorPorId(int pIdProfessor)
        {
            return bmProfessor.ObterPorId(pIdProfessor);
        }

        public IList<Professor> ObterProfessorPorFiltro(Professor pProfessor)
        {
            return bmProfessor.ObterPorFiltros(pProfessor);
        }

        public void ExcluirProfessor(int idProfessor)
        {
            try
            {
                Professor professor = null;

                if (idProfessor > 0)
                {
                    professor = bmProfessor.ObterPorId(idProfessor);
                }

                bmProfessor.Excluir(professor);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

    }
}
