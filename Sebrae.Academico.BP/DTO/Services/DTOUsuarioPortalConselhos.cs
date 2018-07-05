using System;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOUsuarioPortalConselhos
    {
        public string Nome { get; set; }        
        public string Email { get; set; }
        public string UF { get; set; }
        public string GuidUsuario { get; set; }        
        public string MensagemLogin { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public string Imagem { get; set; }
        public string MsgErro { get; set; }
    }
}
