using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOEtapaInfo
    {
        public DTOEtapaInfo()
        {
            ListaCampos = new List<DTOCampo>();
            Analista = new DTOUsuario();
        }

        public virtual int ID { get; set; }
        public virtual int IDRespostaEtapa { get; set; }
        public virtual string Nome { get; set; }
        public virtual int Status { get; set; }
        public virtual bool RequerAprovacao { get; set; }
        public virtual int IDEtapaRetorno { get; set; }
        public virtual List<DTOCampo> ListaCampos { get; set; }
        public virtual DTOUsuario Analista { get; set; }
        public virtual DTOUsuario Assessor { get; set; }
        public virtual string DataPreenchimento { get; set; }
        public virtual string NomeAprovador { get; set; }
        public virtual string LinkAnexo { get; set; }
        public virtual DTOSituacaoProcesso Situacao { get; set; }


    }
}
