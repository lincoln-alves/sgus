using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.SGC;
using Sebrae.Academico.BP.SGC;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucAreas : UserControl
    {
        public bool CarregarDados { get; set; }

        public Oferta OfertaSelecionada { get; set; }

        public void VerificarDados(Oferta ofertaSelecionada)
        {
            OfertaSelecionada = ofertaSelecionada;

            // Apenas carregar dados caso seja sinalizado.
            // Isso evita carregamento desnecessário em telas que não vão precisar desses dados.
            if (CarregarDados)
            {
                txtArea.Text = "";

                var manterArea = new ManterArea();

                // Mostrar todas as subareas.
                var areas = manterArea.ObterTodos();

                ViewState["_Areas"] = SerializarAreas(areas, ofertaSelecionada);
            }
        }

        public List<Subarea> ObterSubareasSelecionadas()
        {
            var dtoIdsSubAreasSelecionadas =
                new JavaScriptSerializer().Deserialize<List<DTOArea>>(txtJsonAreasSelecionadas.Value)
                    .SelectMany(x => x.subAreas).Where(x => x.selecionada == true).Select(x => x.id).ToList();

            var subAreas =
                new ManterSubarea().ObterTodos().Select(x => x.ID).ToList();

            return (from id in dtoIdsSubAreasSelecionadas
                    where subAreas.Contains(id)
                    select new Subarea { ID = id }).ToList();
        }

        private static string SerializarAreas(IEnumerable<Area> lista, Oferta oferta)
        {
            if (lista == null)
                return null;

            return
                new JavaScriptSerializer().Serialize(
                    lista.AsEnumerable().Select(
                        x =>
                            new
                            {
                                id = x.ID,
                                nome = x.Nome,
                                subAreas = x.Subareas.Select(s => new
                                {
                                    id = s.ID,
                                    nome = s.Nome,
                                    selecionada = oferta != null && oferta.ListaPermissao != null && oferta.ListaPermissao.Any(p => p.Subareas.Any(s2 => s2.ID == s.ID))
                                })
                            }));
        }
    }
}