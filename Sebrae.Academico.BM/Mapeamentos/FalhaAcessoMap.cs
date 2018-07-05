using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class FalhaAcessoMap : ClassMap<FalhaAcesso>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public FalhaAcessoMap()
        {
            Table("LG_FalhaAcesso");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_FalhaAcesso");
            Map(x => x.Login).Column("TX_Login").Length(500);
            Map(x => x.Senha).Column("TX_Senha").Length(500);
            Map(x => x.IP).Column("TX_IP").Length(20);
            Map(x => x.DataAcesso).Column("DT_Acesso").Not.Nullable();
        }

    }
}