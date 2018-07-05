using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CertificadoTemplate : EntidadeBasica
    {
        public virtual string TextoDoCertificado { get; set; }
        public virtual string Imagem { get; set; }
        public virtual IList<TrilhaNivel> ListaTrilhaNivel { get; set; }
        public virtual IList<Oferta> ListaOferta { get; set; }
        public virtual IList<Oferta> ListaOfertaProfessor { get; set; }
        public virtual string Imagem2 { get; set; }
        public virtual string TextoCertificado2 { get; set; }
        public virtual bool? CertificadoTrilhas { get; set; }

        public virtual IList<CategoriaConteudo> ListaCategoriaConteudo { get; set; }

        public virtual bool Professor { get; set; }
        public virtual Uf UF { get; set; }
        public virtual bool Ativo { get; set; }


        public virtual string ObterCodigoCertificadoTutor(int idOferta, int? idTurma, int idUsuario)
        {
            var codigo = "cr" + ID + (idTurma.HasValue ? "tu" + idTurma.Value : "") + "pr" + idUsuario;

            return codigo.ToLower();
        }
    }
}
