using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using System;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterAreaTematica : BusinessProcessBase
    {
        #region "Atributos Privados"

        private readonly BMAreaTematica _bmAreaTematica = null;

        #endregion

        #region "Construtor"

        public ManterAreaTematica() : base()
        {
            _bmAreaTematica = new BMAreaTematica();
        }

        #endregion

        #region "Métodos Públicos"
        public AreaTematica ObterPorId(int pId)
        {
            try
            {
                return _bmAreaTematica.ObterPorId(pId);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
                return null;
            }
        }

        public void ExcluirAreaTematica(int id)
        {
            try
            {
                if (id == 0) return;
                var areaTematica = _bmAreaTematica.ObterPorId(id);
                if (areaTematica == null) return;
                if (areaTematica.IdNodePortal.HasValue)
                {
                    DrupalUtil.RemoverNodeDrupalRest(areaTematica.IdNodePortal.Value);
                }
                _bmAreaTematica.Excluir(areaTematica);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public IList<AreaTematica> ObterTodos()
        {
            return _bmAreaTematica.ObterTodos();
        }

        public IList<AreaTematica> ObterPorFiltro(AreaTematica obj)
        {
            return _bmAreaTematica.ObterPorFiltro(obj);
        }

        public void AtualizarAreaTematica(AreaTematica obj)
        {
            _bmAreaTematica.Salvar(obj);

            AtualizarNodeIdDrupal(obj);
        }

        public void AtualizarNodeIdDrupal(AreaTematica obj)
        {
            var id = SalvaNodeDrupalRest(obj);
            if (!id.HasValue) return;
            obj.IdNodePortal = id.Value;
            _bmAreaTematica.Salvar(obj);
        }

        public int? SalvaNodeDrupalRest(AreaTematica obj)
        {
            var postParameters = DrupalUtil.InitPostParameters(obj.ID, obj.Nome, obj.Apresentacao, "temas");

            postParameters.Add("data[field_icon]", obj.Icone);

            DrupalUtil.PermissoesUf(obj.ListaPermissao.Where(p => p.Uf != null).Select(x => x.Uf.ID).ToList(), ref postParameters);
            DrupalUtil.PermissoesPerfil(obj.ListaPermissao.Where(p => p.Perfil != null).Select(x => x.Perfil.ID).ToList(), ref postParameters);
            DrupalUtil.PermissoesNivelOcupacional(obj.ListaPermissao.Where(p => p.NivelOcupacional != null).Select(x => x.NivelOcupacional.ID).ToList(), ref postParameters);
            try
            {
                return DrupalUtil.SalvaNodeDrupalRest(postParameters);
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}

