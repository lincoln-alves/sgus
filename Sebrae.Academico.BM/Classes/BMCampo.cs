using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.BM.AutoMapper;
using AutoMapper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCampo : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Campo> repositorio;

        #endregion

        #region "Construtor"

        public BMCampo()
        {
            repositorio = new RepositorioBase<Campo>();
        }

        #endregion

        public IList<Campo> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Campo>();
            return query.ToList<Campo>();
        }

        public Campo ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Campo>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<Campo> ObterPorEtapaId(int id)
        {
            var query = repositorio.session.Query<Campo>();

            return query.Where(d => d.Etapa.ID == id).ToList();
        }

        public int ObterUltimaOrdemDaEtapa(int idEtapa)
        {
            var query = repositorio.session.Query<Campo>();

            Campo campo = query.Where(d => d.Etapa.ID == idEtapa).OrderByDescending(d => d.Ordem).FirstOrDefault();

            if (campo != null)
            {
                return campo.Ordem;
            }
            else
            {
                return 0;
            }
        }

        public IList<Campo> ObterPorFiltro(Campo modulo)
        {
            var query = repositorio.session.Query<Campo>();

            //if (!string.IsNullOrEmpty(modulo.Nome))
            //    query = query.Where(x => x.Nome.Contains(modulo.Nome));

            //if (modulo.Capacitacao.ID > 0)
            //{
            //    query = query.Where(x => x.Capacitacao.ID == modulo.Capacitacao.ID);
            //}
            //else if (modulo.Capacitacao.ID == 0 && modulo.Capacitacao.Programa.ID > 0)
            //{
            //    query = query.Where(x => x.Capacitacao.Programa.ID == modulo.Capacitacao.Programa.ID);
            //}

            return query.ToList<Campo>();
        }

        public void Salvar(Campo model)
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

        public void Excluir(Campo campo)
        {
            // Remover dependências do campo.
            using (repositorio.ObterTransacao())
            {
                try
                {
                    var respostas = new BMCampoResposta().ObterRespostasPorCampo(campo.ID);

                    var bmCampoResposta = new BMCampoResposta();

                    foreach (var resposta in respostas)
                    {
                        // Remover Alternativas Respostas
                        var bmAltRes = new BMAlternativaResposta();
                        var alternativasRespostas = bmAltRes.ObterPorCampoRespostaId(resposta.ID);

                        foreach (var alternativaResposta in alternativasRespostas)
                            bmAltRes.Excluir(alternativaResposta);

                        // Remover Resposta
                        bmCampoResposta.Excluir(resposta);
                    }

                    // Remover Alternativas
                    var bmAlt = new BMAlternativa();
                    var alternativas = bmAlt.ObterPorCampoId(campo.ID);

                    foreach (var alternativa in alternativas)
                        bmAlt.Excluir(alternativa);

                    // Finalmente, excluir o campo.
                    repositorio.Excluir(campo);

                    repositorio.Commit();
                }
                catch
                {
                    repositorio.RollbackTransaction();
                    throw;
                }
            }
        }

        private void ValidarModuloInformada(Campo model)
        {
            //throw new NotImplementedException();
        }

        public void DuplicarObjeto(int idCampo)
        {
            Campo campoOriginal = ObterPorId(idCampo);

            if (campoOriginal != null)
            {
                AutoMapperConfig.RegisterMappings();

                var campoNovo = new Campo();

                Mapper.Map(campoOriginal, campoNovo);
                campoNovo.ID = 0;
                campoNovo.Nome = campoNovo.Nome + " - Cópia";

                // Etapa copiada manualmente pois o AutoMapper trava por alguma referência circular a campo dentro da estrutura de Etapa (Refs #1751)
                campoNovo.Etapa = campoOriginal.Etapa;

                campoNovo.Resposta = null;
                if (campoNovo.ListaAlternativas != null)
                {
                    foreach (var alternativa in campoNovo.ListaAlternativas)
                    {
                        alternativa.ID = 0;
                    }
                }


                Salvar(campoNovo);
            }

        }

        public IQueryable<Campo> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void SalvarSemCommit(Campo campo)
        {
            repositorio.SalvarSemCommit(campo);
        }
    }
}
