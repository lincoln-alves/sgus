namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioCapacitados
    {
        public int IdUsuario { get; set; }

        public string SituacaoUsuario { get; set; }
        public string UF { get; set; }  
        public string NivelOcupacional { get; set; }
        public string SolucaoEducacional { get; set; }

        /// <summary>
        /// Deve ser preenchido com o total de usuários agrupados de acordo com os campos exibidos
        /// </summary>
        public int TotalCapacitados { get; set; }

        public string UFResponsavel { get; set; }

    }
}
