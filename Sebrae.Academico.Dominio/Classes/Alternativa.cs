namespace Sebrae.Academico.Dominio.Classes
{
    public class Alternativa : EntidadeBasica
    {
        public virtual int? CampoVinculadoOriginalId { get; set; }

        public virtual Campo Campo { get; set; }
        public virtual byte Ordem { get; set; }
        public virtual byte TipoCampo { get; set; }
        public virtual Campo CampoVinculado { get; set; }
    }
}
