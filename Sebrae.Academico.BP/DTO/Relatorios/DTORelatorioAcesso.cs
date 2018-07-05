using System;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    /// <summary>
    /// Fonte de dados para o relatório de acessos.
    /// </summary>
    public class DTORelatorioAcesso
    {
        /// <summary>
        /// Nome do usuário que realizou o acesso.
        /// </summary>
        public string Nome { get; set; }
        public string Email { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public DateTime Acesso { get; set; }
        public string AcessoFormatado 
        {
            get
            {
                return this.Acesso.ToString("dd/MM");
            }
       
        }

        public int Quantidade { get; set; }
    }
}
