using Sebrae.Academico.BP.DTO.Dominio;
using System;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOMatriculaSE
    {
        // Dados Aluno
        public virtual string Nome { get; set; }
        public virtual string NivelOcupacional { get; set; }
        public virtual string UF { get; set; }
        public virtual string Cpf { get; set; }
        public virtual string Senha { get; set; }
        public virtual DateTime? DataNascimento { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Cep { get; set; }
        
        //Dados Oferta
        public virtual string StatusMatricula { get; set; }
        public virtual DateTime? DataStatusMatricula { get; set; }
        public virtual DateTime DataSolicitacao { get; set; }
        public virtual string LinkAcesso { get; set; }
        public virtual string LinkCertificado { get; set; }
        public virtual int IdMatriculaOferta { get; set; }

        //Dados Turma
        public virtual int IDTurma { get; set; }
        public virtual string IDChaveExternaTurma { get; set; }
        public virtual string NomeTurma { get; set; }
        public virtual DateTime? DataMatriculaTurma { get; set; }
        public virtual DateTime? DataLimite { get; set; }
        public virtual double? Nota1 { get; set; }
        public virtual double? Nota2 { get; set; }
        public virtual double? NotaOnline { get; set; }
        public virtual double? MediaFinal { get; set; }
        public virtual decimal? ValorNotaMinima { get; set; }
        public virtual int AcessoAposConclusao { get; set; }       
    }
}
