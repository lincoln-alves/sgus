using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Hierarquia
    {
        public virtual string CodPessoa { get; set; }

        public virtual string LoginUsuario { get; set; }

        public virtual string Email { get; set; }

        public virtual string Nome { get; set; }

        public virtual string CodEspacoOcupacional { get; set; }

        public virtual string EspacoOcupacional { get; set; }

        public virtual string CodUnidade { get; set; }

        public virtual string Unidade { get; set; }

        public virtual string LoginChefe { get; set; }

        public virtual string LoginDiretorUnidade { get; set; }

        public virtual string TipoGerencia { get; set; }

        public virtual string CargoFuncionario { get; set; }

        public virtual string CodigoFuncionario { get; set; }

        public virtual string CodVen { get; set; }

    }
}
