using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterTrilhaTopicoTematico : BusinessProcessBase
    {
        private BMTrilhaTopicoTematico bmTrilhaTopicoTematico = null;


        public ManterTrilhaTopicoTematico()
            : base()
        {
            bmTrilhaTopicoTematico = new BMTrilhaTopicoTematico();
        }

        public void IncluirTrilhaTopicoTematico(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            bmTrilhaTopicoTematico.ValidarTrilhaTopicoTematicoInformado(pTrilhaTopicoTematico);

            IList<TrilhaTopicoTematico> trilhaTopico = ObterTrilhaTopicoTematicoPorNome(pTrilhaTopicoTematico.Nome);

            if (trilhaTopico != null && trilhaTopico.Count > 0) throw new AcademicoException("Este Tópico Temático Já está Cadastrado !");

            base.PreencherInformacoesDeAuditoria(pTrilhaTopicoTematico);
            bmTrilhaTopicoTematico.Salvar(pTrilhaTopicoTematico);
        }

        public void AlterarTrilhaTopicoTematico(TrilhaTopicoTematico pTrilhaTopicoTematico)
        {
            bmTrilhaTopicoTematico.ValidarTrilhaTopicoTematicoInformado(pTrilhaTopicoTematico);
            base.PreencherInformacoesDeAuditoria(pTrilhaTopicoTematico);
            bmTrilhaTopicoTematico.Salvar(pTrilhaTopicoTematico);
        }
        
        public IList<TrilhaTopicoTematico> ObterTrilhaTopicoTematicoPorNome(string pNome)
        {
            if (string.IsNullOrWhiteSpace(pNome)) throw new AcademicoException("Nome. Campo Obrigatório");
            return bmTrilhaTopicoTematico.ObterPorNome(pNome);
        }

        public void ExcluirTrilhaTopicoTematico(int IdTrilhaTopicoTematico)
        {
            try
            {
                TrilhaTopicoTematico trilhaTopicoTematico = null;

                if (IdTrilhaTopicoTematico > 0)
                {
                    trilhaTopicoTematico = bmTrilhaTopicoTematico.ObterPorID(IdTrilhaTopicoTematico);
                }

                bmTrilhaTopicoTematico.Excluir(trilhaTopicoTematico);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IQueryable<TrilhaTopicoTematico> ObterTodosTrilhaTopicoTematico()
        {
            return bmTrilhaTopicoTematico.ObterTodos();
        }

        public TrilhaTopicoTematico ObterTrilhaTopicoTematicoPorID(int pId)
        {
            return bmTrilhaTopicoTematico.ObterPorID(pId);
        }

        public IList<TrilhaTopicoTematico> ObterTrilhaTopicoTematicoPorFiltro(TrilhaTopicoTematico ptrilhaTopicoTematico)
        {
            return bmTrilhaTopicoTematico.ObterPorFiltro(ptrilhaTopicoTematico);
        }

        public TrilhaTopicoTematico ObterTrilhaTopicoTematicoPorObjetivoTrilhaNivel(string chaveExternaObjetivo, string token)
        {
            return bmTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorObjetivoTrilhaNivel(chaveExternaObjetivo, token);
        }
    }
}
