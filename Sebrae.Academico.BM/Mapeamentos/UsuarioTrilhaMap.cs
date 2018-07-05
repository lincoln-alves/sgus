using System;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class UsuarioTrilhaMap : ClassMap<UsuarioTrilha>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public UsuarioTrilhaMap()
        {
            Table("TB_USUARIOTRILHA");
            LazyLoad();

            Id(x => x.ID, "ID_USUARIOTRILHA").GeneratedBy.Identity();
            References(x => x.NivelOcupacional).Column("ID_NIVELOCUPACIONAL").Cascade.None();
            References(x => x.Usuario).Column("ID_USUARIO").Cascade.None();
            References(x => x.TrilhaNivel).Column("ID_TRILHANIVEL").Cascade.None().LazyLoad();
            References(x => x.Uf).Column("ID_UF").Cascade.None();
            Map(x => x.DataInicio).Column("DT_INICIO");
            Map(x => x.DataLimite).Column("DT_LIMITE");
            Map(x => x.DataFim).Column("DT_FIM");
            Map(x => x.NotaProva).Column("NU_NOTAPROVA");
            Map(x => x.StatusMatricula).Column("ID_StatusMatricula").CustomType<enumStatusMatricula>().Not.Nullable();
            Map(x => x.CDCertificado).Column("CD_Certificado");
            Map(x => x.DataGeracaoCertificado).Column("DT_GeracaoCertificado");
            Map(x => x.QTEstrelas).Column("QT_Estrelas");
            Map(x => x.QTEstrelasPossiveis).Column("QT_EstrelasPossiveis");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.NovaProvaLiberada).Column("IN_LiberaProva");
            Map(x => x.DataLiberacaoNovaProva).Column("DT_LiberaProva");
            Map(x => x.AcessoBloqueado).Column("IN_BloqueiaAcesso");
            Map(x => x.Token).Column("SID_Token");
            Map(x => x.NovasTrilhas).Column("IN_NovasTrilhas");
            Map(x => x.TipoTrofeu).Column("TP_Trofeu").CustomType<enumTipoTrofeu>().Not.Nullable();
            Map(x => x.DataAlteracaoStatus).Column("DT_AlteracaoStatus");

            HasMany(x => x.ListaItemTrilhaParticipacao).KeyColumn("ID_USUARIOTRILHA").AsBag().Inverse().Cascade.All();
            HasMany(x => x.ListaTrilhaAtividadeFormativaParticipacao)
                .KeyColumn("ID_USUARIOTRILHA")
                .AsBag()
                .Inverse()
                .Cascade.All();

            HasMany(x => x.ListaUsuarioTrilhaMoedas).KeyColumn("ID_USUARIOTRILHA").AsBag().Inverse().Cascade.All();
            HasMany(x => x.ListaNotificacoes).KeyColumn("ID_USUARIOTRILHA").AsBag().Inverse().Cascade.All();
            HasMany(x => x.ListaVisualizacoesMensagemGuia)
                .KeyColumn("ID_UsuarioTrilha")
                .AsBag()
                .Inverse()
                .Cascade.All();

            HasMany(x => x.ListaLogLider).KeyColumn("ID_Aluno").AsBag().Inverse().Cascade.All();
        }
    }
}