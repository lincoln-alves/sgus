using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Views;
using Sebrae.Academico.BP.DTO.Services.ConsultarSolucaoEducacionalAutoIndicativa;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Services
{
    public class ViewTrilhaServices: BusinessProcessServicesBase
    {
        private BMViewTrilha _viewTrilhaBm;

        public IList<ItemTrilhaAutoIndicativaDTO> BuscarSolucoesEducacionaisAutoIndicativas(int pIdUsuario, int pIdTrilhaNivel, int pIdTopicoTematico)
        {
            _viewTrilhaBm = new BMViewTrilha();
            var viewTrilha = new ViewTrilha();

            if (pIdTrilhaNivel != 0)
                viewTrilha.TrilhaNivelOrigem = new TrilhaNivel() { ID = pIdTrilhaNivel };

            if (pIdTopicoTematico != 0)
                viewTrilha.TopicoTematico = new TrilhaTopicoTematico() { ID = pIdTopicoTematico };

            IList<ViewTrilha> lstViewTrilha = _viewTrilhaBm.ObterViewTrilhaPorFiltro(viewTrilha).ToList<ViewTrilha>();
            IList<ItemTrilha> lstItemTrilhaPendente = lstViewTrilha.Where(p => p.Aprovado == enumStatusSolucaoEducacionalSugerida.Pendente && p.UsuarioOrigem != null && p.UsuarioOrigem.ID == pIdUsuario).Select(x => x.ItemTrilha).Distinct().ToList();
            IList<ItemTrilha> lstItemTrilha = lstViewTrilha.Where(p => p.Aprovado == enumStatusSolucaoEducacionalSugerida.Aprovado && p.UsuarioOrigem != null).Select(x => x.ItemTrilha).Distinct().ToList();
             
            IList<ItemTrilhaAutoIndicativaDTO> listaItemResult = new List<ItemTrilhaAutoIndicativaDTO>();
            if (lstItemTrilhaPendente.Any()) {
                foreach (var registro in lstItemTrilhaPendente.Select(item => PreencheItemTrilhaAutoIndicativaDto(item, pIdUsuario))) {
                    listaItemResult.Add(registro);
                }
            }
            if (lstItemTrilha.Any()) {
                foreach (var registro in lstItemTrilha.Select(item => PreencheItemTrilhaAutoIndicativaDto(item,pIdUsuario))) {
                    listaItemResult.Add(registro);
                }
            }
            return listaItemResult;
        }

        private ItemTrilhaAutoIndicativaDTO PreencheItemTrilhaAutoIndicativaDto(ItemTrilha item, int pIdUsuario){
            var registro = new ItemTrilhaAutoIndicativaDTO{
                FormaAquisicao = item.FormaAquisicao.Nome,
                ID = item.ID,
                Nome = item.Nome,
                NomeIndicador = item.Usuario.Nome,
                IDIndicador = item.Usuario.ID,
                UF = item.Usuario.UF.Nome,
                UFSigla = item.Usuario.UF.Sigla,
                EmailIndicador = item.Usuario.Email,
                DataIndicacao = item.DataCriacao == null ? String.Empty : item.DataCriacao.Value.ToString("dd/MM/yyyy"),
                Local = item.Local,
                ReferenciaBibliografica = item.ReferenciaBibliografica,
                LinkAcesso = item.LinkConteudo,
                StatusAprovacao = item.AprovadoStatus
            };

            var usuarioTrilha = new BMUsuarioTrilha().ObterPorUsuario(pIdUsuario).FirstOrDefault(x => x.TrilhaNivel.ID == item.Missao.PontoSebrae.TrilhaNivel.ID);
            registro.Participante = usuarioTrilha != null && item.ListaItemTrilhaParticipacao.Any(x => x.UsuarioTrilha.ID == usuarioTrilha.ID);

            if (item.Objetivo == null) return registro;
            registro.Objetivo = item.Objetivo.Nome;
            registro.idObjetivo = item.Objetivo.ID;
            return registro;
        }
    }
}
