using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MatriculaOfertaMap : ClassMap<MatriculaOferta>
    {
        public MatriculaOfertaMap()
        {
            Table("TB_MatriculaOferta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MatriculaOferta");
            

            //References(x => x.StatusMatricula).Column("ID_StatusMatricula");

            Map(x => x.StatusMatricula, "ID_StatusMatricula").CustomType<enumStatusMatricula>();
            Map(x => x.DataSolicitacao).Column("DT_Solicitacao").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.DataGeracaoCertificado).Column("DT_GeracaoCertificado");
            Map(x => x.CDCertificado).Column("CD_Certificado");
            Map(x => x.DataStatusMatricula).Column("DT_StatusMatricula");
            Map(x => x.LinkAcesso).Column("LK_Acesso");
            Map(x => x.LinkCertificado).Column("LK_Certificado");
            Map(x => x.FornecedorNotificado).Column("IN_FornecedorNotificado");
            Map(x => x.CertificadoEmitidoPorGestor).Column("ID_CertificadoEmitidoPorGestor");
            
            References(x => x.Oferta).Column("ID_Oferta");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.UF).Column("ID_UF");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");

            HasMany(x => x.MatriculaTurma).KeyColumn("ID_MatriculaOferta").AsBag()
                   .Inverse().Cascade.AllDeleteOrphan();

            HasMany(x => x.QuestoesRespostas).KeyColumn("ID_MatriculaOferta");

            HasMany(x => x.ListaItemTrilhaParticipacao)
                .KeyColumn("ID_MatriculaOferta")
                .LazyLoad()
                .AsBag()
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}
