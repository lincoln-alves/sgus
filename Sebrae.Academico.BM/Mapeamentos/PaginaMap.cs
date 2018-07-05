using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class PaginaMap : ClassMap<Pagina>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public PaginaMap()
        {
            Table("TB_Pagina");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Pagina");
            Map(x => x.Nome).Column("TX_Nome");
            Map(x => x.Descricao).Column("TX_Descricao").CustomSqlType("TEXT").Length(int.MaxValue);
            Map(x => x.CaminhoRelativo).Column("TX_CaminhoRelativo");
            Map(x => x.DescricaoAdministrador).Column("TX_DescricaoAdministrador").CustomSqlType("TEXT").Length(int.MaxValue);
            Map(x => x.DescricaoGestor).Column("TX_DescricaoGestor").CustomSqlType("TEXT").Length(int.MaxValue);

            Map(x => x.IconeMenu).Column("TX_IconeMenu");
            Map(x => x.IconePaginas).Column("TX_IconePaginas");
            Map(x => x.Estilo).Column("TX_Estilo");

            Map(x => x.PaginaInicial).Column("IN_PaginaInicial").Nullable();

            Map(x => x.TodosPerfis).Column("IN_TodosPerfis");
            Map(x => x.TodasUfs).Column("IN_TodasUfs");
            Map(x => x.TodosNiveisOcupacionais).Column("IN_TodosNiveisOcupacionais");

            Map(x => x.ConsiderarNacionalizacaoUf).Column("IN_ConsiderarNacionalizacaoUf");

            Map(x => x.Left).Column("QT_Left");
            Map(x => x.Right).Column("QT_Right");

            Map(x => x.Titulo).Column("TX_Titulo");
            Map(x => x.ChaveVerificadora).Column("TX_ChaveVerificadora");

            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            HasManyToMany(a => a.Perfis)
                .Table("TB_PaginaPerfil")
                .ParentKeyColumn("ID_Pagina")
                .ChildKeyColumn("ID_Perfil");

            HasManyToMany(a => a.Ufs)
                .Table("TB_PaginaUf")
                .ParentKeyColumn("ID_Pagina")
                .ChildKeyColumn("ID_Uf");

            HasManyToMany(a => a.NiveisOcupacionais)
                .Table("TB_PaginaNivelOcupacional")
                .ParentKeyColumn("ID_Pagina")
                .ChildKeyColumn("ID_NivelOcupacional");
        }
    }
}
