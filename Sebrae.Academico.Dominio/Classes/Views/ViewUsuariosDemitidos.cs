using System;

namespace Sebrae.Academico.Dominio.Classes.Views
{
    public class ViewUsuariosDemitidos
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string CPF { get; set; }
        public virtual string Matricula { get; set; }
        public virtual DateTime DataDemissao { get; set; }
    }
}
