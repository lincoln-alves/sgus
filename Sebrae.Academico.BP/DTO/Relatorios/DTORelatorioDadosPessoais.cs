using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioDadosPessoais
    {
        public string Nome { get; set; }
        public string Nome2 { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public string Perfil { get; set; }
        public string Matricula { get; set; }
        public string Situacao { get; set; }
        public string Email { get; set; }
        public string Unidade { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string TelResidencial { get; set; }
        public string TelCelular { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Estado2 { get; set; }
        public string AreaConhecimento { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        public int QtdAcessos { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DT_Insercao { get; set; }

        public virtual string DataAdmissaoForm
        {
            get { return DataAdmissao != null ? DataAdmissao.ToString() : "--" ; }
        }

        public virtual string DataNascimentoForm
        {
            get { return DataNascimento != null ? DataNascimento.ToString() : "--"; }
        }

        public virtual string UltimoAcessoForm
        {
            get { return UltimoAcesso != null ? UltimoAcesso.ToString() : "--"; }
        }

        public virtual string DataAtualizacaoForm
        {
            get { return DataAtualizacao != null ? DataAtualizacao.ToString() : "--"; }
        }

        public virtual string DT_InsercaoForm
        {
            get { return DT_Insercao != null ? DT_Insercao.ToString() : "--"; }
        }

        public virtual string AreaConhecimentoForm
        {
            get { return "--"; }
        }

    }
}
