
namespace Sebrae.Academico.Dominio.Classes
{
    public class ConfiguracaoPagamentoPublicoAlvo: EntidadeBasicaPorId
    {
        public virtual ConfiguracaoPagamento ConfiguracaoPagamento { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Uf UF { get; set; }
        public virtual Perfil Perfil { get; set; }
        
    }
}
