using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOHistoricoProcessos
    {
        public int IdProcessoResposta { get; set; }

        public string Processo { get; set; }
        public string UsuarioDemandante { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataAlteracao { get; set; }
        private string _etapaConcluida;
        public string EtapaConcluida
        {
            get
            {
                if (Status == enumStatusProcessoResposta.Cancelado)
                    return "Demanda Cancelada";

                return _etapaConcluida;
            }
            set
            {
                _etapaConcluida = value;
            }
        }
        public enumStatusProcessoResposta? Status { get; set; }
    }
}
