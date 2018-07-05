using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterTrilhaNivel : BusinessProcessBase, IDisposable
    {
        private readonly BMTrilhaNivel _bmTrilhaNivel;
        private readonly BMPontoSebrae _bmPontoSebrae;
        private readonly BMMissao _bmMissao;
        private readonly BMItemTrilha _bmItemTrilha;

        public ManterTrilhaNivel()
        {
            _bmTrilhaNivel = new BMTrilhaNivel();
            _bmPontoSebrae = new BMPontoSebrae();
            _bmMissao = new BMMissao();
            _bmItemTrilha = new BMItemTrilha();
        }

        public void IncluirTrilhaNivel(TrilhaNivel pTrilhaNivel)
        {
            try
            {
                _bmTrilhaNivel.Salvar(pTrilhaNivel);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarTrilhaNivel(TrilhaNivel pTrilhaNivel)
        {
            try
            {
                _bmTrilhaNivel.Salvar(pTrilhaNivel);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirTrilhaNivel(int IdTrilhaNivel)
        {
            try
            {
                TrilhaNivel trilhaNivel = null;

                if (IdTrilhaNivel > 0)
                {
                    trilhaNivel = _bmTrilhaNivel.ObterPorID(IdTrilhaNivel);
                }

                _bmTrilhaNivel.Excluir(trilhaNivel);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }


        public IList<TrilhaNivel> ObterPorTrilha(Trilha trilha)
        {
            if (trilha == null) throw new AcademicoException("Trilha. Campo Obrigatório");

            return _bmTrilhaNivel.ObterPorTrilha(trilha);
        }

        public IList<TrilhaNivel> ObterPorTrilha(int id)
        {
            return _bmTrilhaNivel.ObterPorTrilha(id);
        }


        public IList<TrilhaNivel> ObterTodosTrilhaNivel()
        {
            try
            {
                return _bmTrilhaNivel.ObterTodos();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IQueryable<TrilhaNivel> ObterTodosTrilhaNivelIQueryable()
        {
            return _bmTrilhaNivel.ObterTodosIQueryable();
        }

        public TrilhaNivel ObterTrilhaNivelPorID(int pId)
        {
            try
            {
                return _bmTrilhaNivel.ObterPorID(pId);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<TrilhaNivel> ObterTrilhaNivelPreRequisito()
        {
            try
            {
                return _bmTrilhaNivel.ObterTrilhaNivelPreRequisito();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<TrilhaNivel> ObterTrilhaNivelPorFiltro(TrilhaNivel pTrilhaNivel)
        {
            return _bmTrilhaNivel.ObterPorFiltro(pTrilhaNivel);
        }

        /// <summary>
        /// Verifica se o nível da trilha é Pré-Requisito, ou seja, se possui registros dependentes dele,
        /// na tabela tb_trilhanivel
        /// </summary>
        /// <returns></returns>
        public bool ONivelDaTrilhaEhPreRequisito(int idTrilhaNivel)
        {
            return new BMTrilhaNivel().ONivelDaTrilhaEhPreRequisito(idTrilhaNivel);
        }

        public void Salvar(TrilhaNivel nivel)
        {
            _bmTrilhaNivel.Salvar(nivel);
        }


        public IQueryable<TrilhaNivel> ObterPorTrilhaUsuario(int trilhaId, Usuario usuario)
        {
            var perfis = usuario.ObterIdsPerfis();

            return _bmTrilhaNivel.ObterTodosIQueryable()
                .Where(
                    x =>
                        x.Trilha.ID == trilhaId &&
                        x.AceitaNovasMatriculas == true &&

                        // Se o aluno estiver matriculado, só retorna o nível caso a matrícula do aluno não esteja Cancelada/Adm.
                        !x.ListaUsuarioTrilha.Any(
                            ut => ut.Usuario.ID == usuario.ID && ut.StatusMatricula == enumStatusMatricula.CanceladoAdm && ut.NovasTrilhas == true) &&

                        x.ListaPermissao.Any(
                            p => p.NivelOcupacional != null && p.NivelOcupacional.ID == usuario.NivelOcupacional.ID) &&
                        x.ListaPermissao.Any(p => p.Perfil != null && perfis.Contains(p.Perfil.ID)) &&
                        x.ListaPermissao.Any(p => p.Uf != null && p.Uf.ID == usuario.UF.ID)
                )

                .OrderBy(x => x.ValorOrdem);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public int ObterTotalMoedasSolucoesSebrae(int nivelId)
        {
            return ObterTodosTrilhaNivelIQueryable()
                       .Join(new ManterPontoSebrae().ObterTodosIqueryable(), n => n.ID, p => p.TrilhaNivel.ID,
                           (nivel, pontoSebrae) => new {nivel, pontoSebrae})
                       .Join(new ManterMissao().ObterTodosIQueryable(), p => p.pontoSebrae.ID, m => m.PontoSebrae.ID,
                           (lastJoin, missao) => new {lastJoin.nivel, missao})
                       .Join(new ManterItemTrilha().ObterTodosIQueryable(), m => m.missao.ID, it => it.Missao.ID,
                           (lastJoin, itemTrilha) => new {lastJoin.nivel, itemTrilha})
                       .Where(join =>
                           join.nivel.ID == nivelId && join.itemTrilha.Ativo == true &&
                           join.itemTrilha.Usuario == null && join.itemTrilha.Moedas != null &&
                           join.itemTrilha.Tipo != null)
                       .Sum(join => join.itemTrilha.Moedas) ?? 0;
        }
    }
}