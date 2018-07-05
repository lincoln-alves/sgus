namespace Sebrae.Academico.Dominio.Classes
{
    public class PosicaoDadoCertificadoCertame
    {
        public virtual int ID { get; set; }
        public virtual int Ano { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual string Dado { get; set; }
    }
}