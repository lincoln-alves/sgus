
//namespace Sebrae.Academico.Dominio.Classes
//{
//    public class IndiceImportancia : EntidadeBasicaPorId
//    {
//        public virtual Usuario Usuario { get; set; }
//        public virtual TrilhaNivel TrilhaNivel { get; set; }
//        public virtual TrilhaTopicoTematico TrilhaTopicoTematico { get; set; }
//        public virtual Objetivo Objetivo { get; set; }
//        public virtual bool Pre { get; set; }
//        public virtual decimal ValorImportancia { get; set; }

//        public override bool Equals(object obj)
//        {
//            IndiceImportancia objeto = obj as IndiceImportancia;
//            return objeto == null ? false : Usuario.Equals(objeto.Usuario)
//                && TrilhaNivel.Equals(objeto.TrilhaNivel)
//                && TrilhaTopicoTematico.Equals(objeto.TrilhaTopicoTematico)
//                && Objetivo.Equals(objeto.Objetivo)
//                && Objetivo.Equals(objeto.Pre);
//        }

//        public override int GetHashCode()
//        {
//            return Usuario.ID.GetHashCode() + TrilhaNivel.ID.GetHashCode()
//                + TrilhaTopicoTematico.ID.GetHashCode() + Objetivo.GetHashCode();
//        }

//        public IndiceImportancia()
//        {
//            Usuario = new Usuario();
//            TrilhaNivel = new TrilhaNivel();
//        }

//    }

//}
