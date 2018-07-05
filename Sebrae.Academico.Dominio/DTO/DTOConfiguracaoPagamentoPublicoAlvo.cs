using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações de Configuração de Pagamento
    /// </summary>
    public class DTOConfiguracaoPagamentoPublicoAlvo
    {
        public virtual int NumeroLinha { get; set; }
        public virtual ConfiguracaoPagamento ConfiguracaoPagamento { get; set; }
        public virtual int IdUsuario { get; set; }
        
        #region "Informações do Usuário da configuração Pagamento"

        public virtual string EnderecoUsuario { get; set; }
        public virtual string CidadeDoUsuario { get; set; }
        public virtual string CepDoUsuario { get; set; }
        public virtual string NomeUsuario { get; set; }
        
        public virtual int IDUfDoUsuario { get; set; }
        public virtual string SiglaUfDoUsuario { get; set; }
        public virtual string UfDoUsuarioPorExtenso { get; set; }


        public virtual int IDNivelOcupacional { get; set; }
        public virtual string NomeNivelOcupacional { get; set; }

        #endregion


    }
}
