﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Mapeamentos.Moodle
{
    public class SgusMoodleCursoMap : ClassMap<SgusMoodleCurso>
    {
        public SgusMoodleCursoMap()
        {
            Table("int_sgus_moodle_cursos");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id");  
            Map(x => x.CodigoCategoria).Column("codCat");
            Map(x => x.CodigoCurso).Column("codCurso");
            Map(x => x.Nome).Column("nomeCurso");
            Map(x => x.DataCriacao).Column("createTime");
            Map(x => x.DataAtualizacao).Column("updateTime");
            Map(x => x.Desabilitado).Column("disabled");
        }
    }
}
