namespace Sebrae.Academico.Dominio.Classes
{
    public class EtapaVersao : EntidadeBasicaPorId
    {
        public virtual int Versao { get; set; }
        public virtual byte Ordem { get; set; }
        public virtual Etapa Etapa { get; set; }
    }
}
