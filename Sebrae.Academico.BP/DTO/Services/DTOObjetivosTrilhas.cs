using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOTrilhaObjetivos
    {
        public DTOUsuarioObjetivo Usuario { get; set; }
        public List<DTOObjetivosTrilhas> Objetivos { get; set; }
        public int IdTrilha { get; set; }
        public string NomeTrilha { get; set; }
        public int IdTrilhaNivel { get; set; }
        public string NomeTrilhaNivel { get; set; }
        public bool Status { get; set; }
        public string Msg { get; set; }
    }

    public class DTOObjetivosTrilhas
    {
        public int IdObjetivo { get; set; }
        public string ChaveExterna { get; set; }
        public string NomeObjetivo { get; set; }
        public bool StatusObjetivo { get; set; }

        public string StatusObjetivoFormatado()
        {
            return StatusObjetivo ? "Concluído" : "Não concluído";
        }

        public List<DTOSolucoesObrigatorias> SolucoesObrigatorias { get; set; }
    }

    public class DTOSolucoesObrigatorias
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
    }

    public class DTOUsuarioObjetivo
    {
        public string Nome { get; set; }
        public string UF { get; set; }
        public string NomeUF { get; set; }
        public string Cpf { get; set; }
    }
}