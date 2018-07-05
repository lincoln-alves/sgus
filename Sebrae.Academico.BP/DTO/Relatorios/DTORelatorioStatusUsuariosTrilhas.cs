using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioStatusUsuariosTrilhas
    {
        public string Trilha { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string NivelTrilha { get; set; }
        public string NivelOcupacional { get; set; }
        public string UF { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string StatusMatricula { get; set; }
        public DateTime DataInicio { get; set; }
        public string DataFim { get; set; }
        public DateTime DataLimite { get; set; }
        public DateTime? DataAlteracaoStatus { get; set; }
        public int SolucoesAutoindicativas { get; set; }
        public int SolucoesRealizadas { get; set; }
        public int SprintsRealizados { get; set; }
        public int TotalMoedasOuro { get; set; }
        public int TotalMoedasPrata { get; set; }
        public string NotaProvaFinal { get; set; }
    }
}