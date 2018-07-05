using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;


namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ProfessorMap: ClassMap<Professor>
    {
        public ProfessorMap()
        {
            Table("TB_Professor");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Professor");
            Map(x => x.Nome).Column("NM_Professor").Length(250);
            Map(x => x.Cpf).Column("CD_Cpf").Length(11);
            Map(x => x.DataNascimento).Column("DT_Nascimento");
            Map(x => x.DataCadastro).Column("DT_Cadastro").Not.Nullable();
            Map(x => x.DataDesativacao).Column("DT_Desativacao");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable();
            Map(x => x.Curriculo).Column("TX_Curriculo").Length(2147483647);
            Map(x => x.Observacoes).Column("TX_Observacoes").Length(2147483647);
            Map(x => x.Telefone).Column("NU_Telefone").Length(15);
            Map(x => x.TelefoneCelular).Column("NU_TelefoneCelular").Length(15);
            Map(x => x.RG).Column("VL_RG").Length(15);
            Map(x => x.TipoDocumentoRG).Column("NM_TipoDocumento").Length(15);
            Map(x => x.DataExpedicao).Column("DT_Expedicao");
            Map(x => x.Naturalidade).Column("NM_Naturalidade").Length(25);
            Map(x => x.Nacionalidade).Column("NM_Nacionalidade").Length(15);
            Map(x => x.EstadoCivil).Column("NM_EstadoCivil").Length(15);
            Map(x => x.NomePai).Column("NM_Pai").Length(250);
            Map(x => x.NomeMae).Column("NM_Mae").Length(250);
            Map(x => x.Email).Column("NM_Email").Length(150);
            Map(x => x.Endereco).Column("NM_Endereco").Length(150);
            Map(x => x.Bairro).Column("NM_Bairro").Length(50);
            Map(x => x.Cidade).Column("NM_Cidade").Length(50);
            Map(x => x.Estado).Column("NM_Estado").Length(2);
            Map(x => x.CEP).Column("VL_CEP").Length(9);

            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
            
            HasMany(x => x.ListaTurma).KeyColumn("ID_Professor");
        }
    }
}
