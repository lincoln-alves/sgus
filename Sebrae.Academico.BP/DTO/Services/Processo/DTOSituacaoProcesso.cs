using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOSituacaoProcesso
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }

        public DTOSituacaoProcesso ObterSituacao(int statusResposta, EtapaResposta etapa)
        {
            StringBuilder situacaoNome = new StringBuilder();


            if (etapa != null)
            {
                if (statusResposta == (int)enumStatusEtapaResposta.Aguardando && etapa.PermissoesNucleoEtapaResposta != null &&
                    etapa.PermissoesNucleoEtapaResposta.Count > 0)
                {
                    situacaoNome.Append("Aguardando avaliação pelo Núcleo");
                }
                else
                {
                    situacaoNome.Append(EnumExtensions.GetDescription(((enumStatusEtapaResposta)statusResposta)));
                }
            }

            return new DTOSituacaoProcesso()
            {
                ID = statusResposta,
                Nome = situacaoNome != null ? situacaoNome.ToString() : ""
            };
        }
    }
}
