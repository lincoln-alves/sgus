using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class MensagemGuiaMap : ClassMap<MensagemGuia>
    {
        public MensagemGuiaMap()
        {
            Table("TB_MensagemGuia");
            LazyLoad();

            Id(x => x.ID).Column("ID_MensagemGuia").CustomType<enumMomento>();
            Map(x => x.Tipo).Column("VL_Tipo").CustomType<enumTipoMensagemGuia>();
            Map(x => x.Texto).Column("TX_Texto");
            Map(x => x.HashTags).Column("TX_HashTags");
            HasMany(x => x.ListaUsuarioTrilhaMensagemGuia).KeyColumn("ID_MensagemGuia").AsBag().Inverse().Cascade.None();
            HasManyToMany(x => x.Tutoriais).Table("TB_TrilhaTutorialGuia").ParentKeyColumn("ID_MensagemGuia").ChildKeyColumn("ID_TrilhaTutorial").Not.LazyLoad();
        }
    }
}