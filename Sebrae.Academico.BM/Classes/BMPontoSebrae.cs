using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPontoSebrae : BusinessManagerBase
    {
        private readonly RepositorioBase<PontoSebrae> _repositorio;

        public BMPontoSebrae()
        {
            _repositorio = new RepositorioBase<PontoSebrae>();
        }

        public void Salvar(PontoSebrae pontoSebrae)
        {
            _repositorio.Salvar(pontoSebrae);
        }

        public IQueryable<PontoSebrae> ObterTodos()
        {
            return _repositorio.ObterTodos().OrderBy(x => x.Nome).AsQueryable();
        }

        public IQueryable<PontoSebrae>ObterTodosIqueryable()
        {
            return _repositorio.ObterTodosIQueryable();
        }

        public PontoSebrae ObterPorId(int id)
        {
            return _repositorio.ObterPorID(id);
        }

        public IQueryable<PontoSebrae> ObterPorNome(PontoSebrae pontoSebrae)
        {
            return ObterPorNome(pontoSebrae.Nome);
        }

        public IQueryable<PontoSebrae> ObterPorNome(string nome)
        {
            return _repositorio.session.Query<PontoSebrae>().Where(x => x.Nome == nome);
        }

        public PontoSebrae ObterPrimeiroPorNome(PontoSebrae pontoSebrae)
        {
            return ObterPrimeiroPorNome(pontoSebrae.Nome);
        }

        public PontoSebrae ObterPrimeiroPorNome(string nome)
        {
            return _repositorio.session.Query<PontoSebrae>().FirstOrDefault(x => x.Nome == nome);
        }


        public void Excluir(PontoSebrae pontoSebrae)
        {
            try
            {
                ValidarDependencias(pontoSebrae);
                _repositorio.Excluir(pontoSebrae);
            }
            catch (AcademicoException e)
            {
                WebFormHelper.ExibirMensagem(Dominio.Enumeracao.enumTipoMensagem.Erro, e.Message);
            }
            catch (Exception)
            {
            }
        }

        private void ValidarDependencias(PontoSebrae ponto)
        {
            if (ponto.ListaMissoes.Count > 0)
            {
                throw new AcademicoException("Não é possível excluir o registro, existem missões vincualdas ao ponto sebrae");
            }
        }

        /// <summary>
        /// Validação das informações de um Tópico Temático.
        /// </summary>
        /// <param name="pontoSebrae"></param>
        public void ValidarTrilhaTopicoTematicoInformado(PontoSebrae pontoSebrae)
        {
            ValidarInstancia(pontoSebrae);

            if (string.IsNullOrWhiteSpace(pontoSebrae.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            VerificarExistenciaPontoSebrae(pontoSebrae);
        }

        private void VerificarExistenciaPontoSebrae(PontoSebrae pontoSebrae)
        {
            var pontoSebraeCadastrado = ObterPrimeiroPorNome(pontoSebrae);

            if (pontoSebraeCadastrado != null)
            {
                if (pontoSebrae.ID != pontoSebraeCadastrado.ID)
                {
                    throw new AcademicoException(string.Format("O Ponto Sebrae '{0}' já está cadastrado",
                                                 pontoSebrae.Nome.Trim()));
                }
            }
        }

        public IQueryable<PontoSebrae> ObterPorFiltro(PontoSebrae pontoSebrae)
        {
            var query = _repositorio.session.Query<PontoSebrae>();

            if (pontoSebrae != null)
            {
                if (!string.IsNullOrWhiteSpace(pontoSebrae.Nome))
                    query = query.Where(x => x.Nome.Contains(pontoSebrae.Nome.ToUpper()));
            }

            if(pontoSebrae.TrilhaNivel != null)
            {
                query = query.Where(x => x.TrilhaNivel.ID == pontoSebrae.TrilhaNivel.ID);
            }

            return query;
        }

        public PontoSebrae ObterPorNomeExibicao(string nomeExibicao)
        {
            return
                _repositorio.session.Query<PontoSebrae>()
                    .FirstOrDefault(x => x.NomeExibicao != null && x.NomeExibicao.ToUpper() == nomeExibicao.ToUpper());
        }
    }
}