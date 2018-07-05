using System;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTODadosPagamento
    {
        public virtual bool PossuePagamento { get; set; }
        public virtual string TermoDeUso { get; set; }
        //usuario.Cep = Regex.Replace(dtoUsuario.CEP.Trim(), @"\D", "").ToString();
        //usuario.Endereco = dtoUsuario.Logradouro;
        //usuario.Complemento = dtoUsuario.Complemento;
        //usuario.Bairro = dtoUsuario.Bairro;
        //usuario.Cidade = dtoUsuario.Cidade;
        //usuario.Estado = dtoUsuario.Estado;

        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }

}

