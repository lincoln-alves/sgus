using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarFormaAquisicao : BusinessProcessServicesBase
    {
        public IList<DTOFormaAquisicao> ListarFormaAquisicao()
        {
            IList<DTOFormaAquisicao> lstResut;

            try
            {
                var bmformaAquisicaoBm = new BMFormaAquisicao();

                lstResut =
                    bmformaAquisicaoBm.ObterPorTipo(enumTipoFormaAquisicao.Trilha).Select(fa => new DTOFormaAquisicao
                    {
                        ID = fa.ID,
                        Nome = fa.Nome,
                        Imagem = fa.Imagem,
                        CargaHoraria = fa.CargaHorariaFormatada,
                        PermiteAlterarCargaHoraria = fa.PermiteAlterarCargaHoraria == true
                    }).ToList();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
                throw ex;
            }

            return lstResut;
        }

    }
}
