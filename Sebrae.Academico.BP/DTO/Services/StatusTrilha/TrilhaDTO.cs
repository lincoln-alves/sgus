using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class TrilhaDTO
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NomeExtendido { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Imagem { get; set; }
        public List<TrilhaNivelDTO> ListaTrilhaNivel { get; set; }
        public virtual string CorChamadaFundo { get; set; }
        public virtual string CorChamadaTexto { get; set; }
        public virtual string TextoChamada { get; set; }
        public virtual string EmailTutor { get; set; }
        public virtual string LinkComunidade { get; set; }
        public virtual string LinkPDFDepoimento { get; set; }

        public TrilhaDTO()
        {
            ListaTrilhaNivel = new List<TrilhaNivelDTO>();
        }
    }
}
