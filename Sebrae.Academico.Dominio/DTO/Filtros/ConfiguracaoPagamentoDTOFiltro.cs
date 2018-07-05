
namespace Sebrae.Academico.Dominio.DTO.Filtros
{
    /// <summary>
    /// Classe de DTO para filtros.
    /// </summary>
    public class ConfiguracaoPagamentoDTOFiltro
    {
        public virtual int IdUsuario { get; set; }
        public virtual int IdConfiguracaoPagamento { get; set; }
        public virtual int IdNivelOcupacional { get; set; }
        public virtual int IdUF { get; set; }
        public virtual int IdPerfil { get; set; }

        public virtual string NomeUsuario { get; set; }
    }
}
