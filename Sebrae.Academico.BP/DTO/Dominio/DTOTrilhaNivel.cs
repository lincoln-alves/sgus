using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOTrilhaNivel
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }
        public int DiasPrazo { get; set; }
        public string Descricao { get; set; }
        public bool UsuarioPossuiMatricula { get; set; }
        public string PrerequisitoNaoCumprido { get; set; }
        public int TipoNivel { get; set; }
        public byte Ordem { get; set; }
        public DTOTermoAceite TermoAceite { get; set; }

        public List<DTOTrilhaTopicoTematico> TopicosTematicos { get; set; }
    }
}
