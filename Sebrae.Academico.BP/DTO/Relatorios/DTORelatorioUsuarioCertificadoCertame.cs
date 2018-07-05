using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioUsuarioCertificadoCertame
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string Unidade { get; set; }
        public int Ano { get; set; }
        public string TemaCertificacao { get; set; }
        public string Status { get; set; }
        public string NumeroInscricao { get; set; }
        public float? Nota { get; set; }

        public string CertificadoEmitido
        {
            get { return !string.IsNullOrEmpty(DataDownload) ? "Sim" : "Não"; }
        }

        public string DataDownload { get; set; }
    }
}
