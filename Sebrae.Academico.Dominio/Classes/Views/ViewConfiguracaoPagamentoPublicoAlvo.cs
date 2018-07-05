
namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewConfiguracaoPagamentoPublicoAlvo
    {
        public virtual int NumeroLinha { get; set; }
        public virtual ConfiguracaoPagamento ConfiguracaoPagamento { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
