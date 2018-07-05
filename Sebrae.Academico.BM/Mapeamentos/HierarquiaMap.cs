using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class HierarquiaMap : ClassMap<Hierarquia>
    {
        public HierarquiaMap()
        {
            Table("Tb_Hierarquia");
            LazyLoad();
            Id(x => x.CodPessoa).GeneratedBy.Assigned();

            Map(x => x.CodPessoa).Column("CodPessoa");
            Map(x => x.LoginUsuario).Column("LoginUsuario");
            Map(x => x.Email).Column("Email");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.CodEspacoOcupacional).Column("CodEspacoOcupacional");
            Map(x => x.EspacoOcupacional).Column("EspacoOcupacional");
            Map(x => x.CodUnidade).Column("CodUnidade");
            Map(x => x.Unidade).Column("Unidade");
            Map(x => x.LoginChefe).Column("LoginChefe");
            Map(x => x.LoginDiretorUnidade).Column("LoginDiretorUnidade");
            Map(x => x.TipoGerencia).Column("TipoGerencia");
            Map(x => x.CargoFuncionario).Column("CargoFuncionario");
            Map(x => x.CodigoFuncionario).Column("CodigoFuncionario");
            Map(x => x.CodVen).Column("CodVen");
        }

    }
}
