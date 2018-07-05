using System;
namespace Sebrae.Academico.BP.DTO.Dominio
{

    public class DTOFornecedor : DTOEntidadeBasica
    {
        public virtual string NomeInstituicaoApresentacao { get; set; }
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual DateTime? DataUltimoAcesso { get; set; }
        public virtual int? QuantidadeAcessos { get; set; }
    }
}