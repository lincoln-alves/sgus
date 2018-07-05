﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalCategoriaConteudo : EntidadeBasica
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
    }
}
