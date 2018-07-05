using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.GerarPagamento
{
    public class DTOGerarPagamento
    {
        public virtual int IDUsuario { get; set; }
        public virtual int IDConfiguracaoPagamento { get; set; }
        public virtual int IDFormaPagamento { get; set; }
        public virtual string CEP { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }

    }
}
