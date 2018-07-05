using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services.Questionario
{
    public class DTOQuestionario
    {
        public virtual int ID { get; set; }
        public virtual int Tipo { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual string NomeQuestionario { get; set; }
        
    }
}
