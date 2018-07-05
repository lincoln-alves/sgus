namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOLoja
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool PossuiLider { get; set; }
        public int MoedasTotais { get; set; }
        public int MoedasConquistadas { get; set; }
        public string UltimoAcesso { get; set; }
        public bool PossuiItemTrilhaParticipacao { get; set; }

    }
}
