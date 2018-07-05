using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Services.Processo;

namespace Sebrae.Academico.BP
{
    public class ManterEtapa : BusinessProcessBase, IDisposable
    {

        private readonly BMEtapa _bmEtapa;

        public ManterEtapa()
        {
            _bmEtapa = new BMEtapa();
        }

        //public void Incluir(Etapa model, IEnumerable<EtapaPermissao> permissoes, IEnumerable<EtapaPermissaoNucleo> permissoesNucleo)
        //{
        //    try
        //    {
        //        ValidarEtapa(model);
        //        //this.PreencherInformacoesDeAuditoria(model);
        //        model.Ordem = bmEtapa.ObterUltimaPosicaoOrdem();

        //        model.Permissoes = permissoes != null ? permissoes.ToList() : null;
        //        model.PermissoesNucleo = permissoesNucleo != null ? permissoesNucleo.ToList() : null;

        //        if (bmEtapa.ObterPorProcessoId(model.Processo.ID).Count == 0)
        //        {
        //            model.PrimeiraEtapa = true;
        //        }
        //        else
        //        {
        //            model.PrimeiraEtapa = false;
        //        }

        //        bmEtapa.Salvar(model);

        //        AdicionarPermissoes(model, permissoes);

        //        if (permissoesNucleo != null && permissoesNucleo.Count() > 0)
        //            AtualizarPermissoesNucleo(model.ID, permissoesNucleo);
        //    }
        //    catch (AcademicoException ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Excluir(int id)
        {
            try
            {
                if (id <= 0)
                    return;

                var etapa = _bmEtapa.ObterPorId(id);

                var manterEtapaPermissao = new ManterEtapaPermissao();

                var permissoes = etapa.Permissoes.ToList();
                etapa.Permissoes.Clear();

                foreach (var permissao in permissoes)
                {
                    manterEtapaPermissao.Excluir(permissao);
                }

                _bmEtapa.Excluir(etapa);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<Etapa> ObterTodos()
        {
            return _bmEtapa.ObterTodos();
        }

        public IQueryable<Etapa> ObterTodosIQueryable()
        {
            return _bmEtapa.ObterTodosIQueryable();
        }

        public IList<Etapa> ObterPorFiltro(Etapa filtro)
        {
            return _bmEtapa.ObterPorFiltro(filtro);
        }

        private void ValidarEtapa(Etapa etapa)
        {
            if (string.IsNullOrEmpty(etapa.Nome))
                throw new AcademicoException("Nome é obrigatório");
        }

        public void Salvar(Etapa etapa, IEnumerable<EtapaPermissaoNucleo> permissoesNucleo)
        {
            try
            {
                ValidarEtapa(etapa);
                _bmEtapa.Salvar(etapa);

                //excluirPermissoesDeEtapa(model.ID);
                //adicionarPermissoes(model, permissoes);

                AtualizarPermissoesNucleo(etapa.ID, permissoesNucleo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Precisa de refatoração.
        /// </summary>
        /// <param name="idEtapa"></param>
        /// <param name="permissoesNucleo"></param>
        private void AtualizarPermissoesNucleo(int idEtapa, IEnumerable<EtapaPermissaoNucleo> permissoesNucleo)
        {
            var remover = new List<EtapaPermissaoNucleo>();
            var incluir = new List<EtapaPermissaoNucleo>();

            var etapa = new ManterEtapa().ObterPorID(idEtapa);

            // Valida a remoção de permissões
            if (etapa.PermissoesNucleo != null)
            {
                if (permissoesNucleo.Any())
                {
                    foreach (var permissao in etapa.PermissoesNucleo)
                    {

                        if (
                            permissoesNucleo.Any(
                                x =>
                                    x.HierarquiaNucleoUsuario.ID == permissao.HierarquiaNucleoUsuario.ID &&
                                    x.Etapa.ID == permissao.Etapa.ID))
                            continue;

                        remover.Add(permissao);
                    }
                }
                else
                {
                    remover.AddRange(etapa.PermissoesNucleo);
                }
            }

            // Valida inclusão de permissões
            if (etapa.PermissoesNucleo?.Count > 0)
            {
                foreach (var permissao in permissoesNucleo)
                {
                    if (
                        etapa.PermissoesNucleo.Any(
                            p =>
                                p.HierarquiaNucleoUsuario.ID == permissao.HierarquiaNucleoUsuario.ID &&
                                p.Etapa.ID == permissao.Etapa.ID))
                        continue;

                    incluir.Add(permissao);
                }
            }
            else
            {
                if(permissoesNucleo.Any())
                    incluir.AddRange(permissoesNucleo);
            }

            using (var manter = new ManterEtapaPermissaoNucleo())
            {
                try
                {
                    manter.ExcluirTodos(remover);
                }
                catch (Exception)
                {
                    throw new AcademicoException(
                        "Não foi possível excluir as permissões do núcleo, existem registros dependentes no banco");
                }
                manter.Salvar(incluir);
            }
        }

        internal void Salvar(List<Etapa> etapas)
        {
            _bmEtapa.Salvar(etapas);
        }

        //private void excluirPermissoesNucleo(int id)
        //{
        //    using (var manter = new ManterEtapaPermissaoNucleo())
        //    {
        //        manter.ExcluirTodosEtapa(id);
        //    }
        //}

        public void AlterarOrdemCampos(dynamic obj)
        {
            var bmCampo = new BMCampo();

            foreach (var item in obj)
            {
                Campo model = bmCampo.ObterPorId(Convert.ToInt16(item["id"]));
                model.Ordem = Convert.ToByte(item["ordem"]);
                bmCampo.Salvar(model);
            }
        }

        public Etapa ObterPorID(int IdModel)
        {
            return _bmEtapa.ObterPorId(IdModel);
        }

        //private void excluirPermissoesDeEtapa(int idEtapa)
        //{
        //    var bmPermissao = new BMPermissao();

        //    bmPermissao.ExcluirTodosDeEtapa(idEtapa);
        //}

        //private void AdicionarPermissoes(Etapa etapa, IEnumerable<EtapaPermissao> permissoes)
        //{
        //    using (var bmPermissao = new BMPermissao())
        //    {
        //        foreach (var item in permissoes)
        //        {
        //            item.Etapa = etapa;
        //            bmPermissao.Salvar(item);
        //        }
        //    }
        //}

        public void DuplicarObjeto(Etapa etapa, bool concatenarCopiaAoNome)
        {
            _bmEtapa.DuplicarObjeto(etapa, concatenarCopiaAoNome);
        }

        public void Dispose()
        {
            _bmEtapa.Dispose();
        }

        //public List<Usuario> 

        //internal IList<Usuario> ObterAnalistas(Etapa etapa, int idProcessoResposta)
        //{
        //    IList<Usuario> usuarios = new List<Usuario>();

        //    if (etapa != null) {
        //        var usuario = new BMProcessoResposta().ObterPorId(idProcessoResposta).Usuario;

        //        usuarios = new BMPermissao().ObterUsuariosDasPermissoesPorEtapa(etapa.ID, usuario, true, false, etapa.PodeSerAprovadoAssessor);
        //    }

        //    return usuarios.Distinct().ToList();
        //}

        public byte ObterUltimaPosicaoOrdem()
        {
            return _bmEtapa.ObterUltimaPosicaoOrdem();
        }

        public IList<Etapa> ObterPorProcessoId(int id)
        {
            return _bmEtapa.ObterPorProcessoId(id);
        }
    }
}