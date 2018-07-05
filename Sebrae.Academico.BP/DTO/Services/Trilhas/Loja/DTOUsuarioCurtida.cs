using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.Loja
{
    public class DTOUsuarioCurtida
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Unidade { get; set; }
        public string Foto { get; set; }
        public enumTipoCurtida TipoCurtida { get; set; }
    }
}
