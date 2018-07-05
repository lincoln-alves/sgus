using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class GroupMoodle
    {

        public virtual Int64 ID { get; set; }
        public virtual Int64 CourseID { get; set; }
        public virtual String IdNumber { get; set; }
        public virtual String Name { get; set; }
      
    }
}
