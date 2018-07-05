using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ConfiguracaoSistema: EntidadeBasicaPorId
    {
        public virtual string Registro { get; set; }
        public virtual string Descricao { get; set; }


        public ConfiguracaoSistema()
        {
        }
    }
}