using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOGrupoMoodle
    {
        public string idCourse { get; set; }
        public string idOffer { get; set; }
        public string idGroup { get; set; }
        public string nameCourse { get; set; }
        public string nameOffer { get; set; }
        public string nameGroup { get; set; }
        public string listUsers { get; set; }
        public string listTheachers { get; set; }
        public string newIdGroup { get; set; }
        public string fullSgus { get; set; }
    }
}
