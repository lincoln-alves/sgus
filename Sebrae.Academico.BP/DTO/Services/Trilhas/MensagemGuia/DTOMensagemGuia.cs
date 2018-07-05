using Sebrae.Academico.BP.DTO.Services.Trilhas.MensagemGuia;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.Solucao
{
    public class DTOMensagemGuia
    {
        public DTOMensagemGuia(int id, string texto)
        {
            Id = id;
            Message = texto;
        }

        public DTOMensagemGuia(int id, List<DTOTutorial> tutoriais)
        {
            Id = id;
            Tutoriais = tutoriais;
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public List<DTOTutorial> Tutoriais { get; set; }

    }
}
