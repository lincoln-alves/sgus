using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterPontoSebrae : BusinessProcessBase
    {
        private readonly BMPontoSebrae _bmPontoSebrae;


        public ManterPontoSebrae()
        {
            _bmPontoSebrae = new BMPontoSebrae();
        }

        public void IncluirPontoSebrae(PontoSebrae pontoSebrae)
        {
            _bmPontoSebrae.ValidarTrilhaTopicoTematicoInformado(pontoSebrae);

            var pontosSebrae = ObterPontoSebraePorNome(pontoSebrae.Nome);

            if (pontosSebrae != null && pontosSebrae.Any())
                throw new AcademicoException("Este Ponto Sebrae Já está Cadastrado!");

            _bmPontoSebrae.Salvar(pontoSebrae);
        }

        public void AlterarPontoSebrae(PontoSebrae pontoSebrae)
        {
            _bmPontoSebrae.ValidarTrilhaTopicoTematicoInformado(pontoSebrae);

            _bmPontoSebrae.Salvar(pontoSebrae);
        }
        
        public IQueryable<PontoSebrae> ObterPontoSebraePorNome(string pNome)
        {
            if (string.IsNullOrWhiteSpace(pNome))
                throw new AcademicoException("Nome. Campo Obrigatório");

            return _bmPontoSebrae.ObterPorNome(pNome);
        }

        public void ExcluirPontoSebrae(int pontoSebraeId)
        {
            try
            {
                PontoSebrae pontoSebrae = null;

                if (pontoSebraeId > 0)
                {
                    pontoSebrae = _bmPontoSebrae.ObterPorId(pontoSebraeId);
                }

                _bmPontoSebrae.Excluir(pontoSebrae);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IQueryable<PontoSebrae> ObterTodos()
        {
            return _bmPontoSebrae.ObterTodos();
        }

        public IQueryable<PontoSebrae>ObterTodosIqueryable()
        {
            return _bmPontoSebrae.ObterTodosIqueryable();
        }

        public PontoSebrae ObterPorId(int pontoSebraeId)
        {
            return _bmPontoSebrae.ObterPorId(pontoSebraeId);
        }

        public IQueryable<PontoSebrae> ObterPontoSebraePorFiltro(PontoSebrae pontoSebrae)
        {
            return _bmPontoSebrae.ObterPorFiltro(pontoSebrae);
        }

        public IQueryable<PontoSebrae> ObterPorTrilhaNivel(TrilhaNivel trilhaNivel)
        {
            return ObterTodos().Where(x => x.TrilhaNivel.ID == trilhaNivel.ID);
        }

        public IQueryable<PontoSebrae> ObterPorTrilhaNivel(int trilhaNivel)
        {
            return ObterTodos().Where(x => x.TrilhaNivel.ID == trilhaNivel);
        }

        public IQueryable<PontoSebrae> ObterPorTrilhaNivelAtivos(TrilhaNivel trilhaNivel)
        {
            return ObterTodos().Where(x => x.TrilhaNivel.ID == trilhaNivel.ID && x.Ativo);
        }

        public IQueryable<PontoSebrae> ObterPorTrilha(Trilha trilha)
        {
            return ObterTodos().Where(x => x.TrilhaNivel.Trilha.ID == trilha.ID);
        }

        public IQueryable<PontoSebrae> ObterPorTrilha(int id)
        {
            return ObterTodosIqueryable().Where(x => x.TrilhaNivel.Trilha.ID == id);
        }

        public IQueryable<PontoSebrae> ObterAtivos()
        {
            return ObterTodos().Where(x => x.Ativo);
        }
    }
}
