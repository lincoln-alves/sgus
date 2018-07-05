using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Professor: EntidadeBasica
    {
        public virtual string Cpf { get; set; }
        public virtual DateTime? DataNascimento { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual string Curriculo { get; set; }
        public virtual string Observacoes { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string TelefoneCelular { get; set; }
        public virtual string RG { get; set; }
        public virtual string TipoDocumentoRG { get; set; }
        public virtual DateTime? ExpedicaoRG { get; set; }
        public virtual string Naturalidade { get; set; }
        public virtual string Nacionalidade { get; set; }
        public virtual string EstadoCivil { get; set; }
        public virtual string NomePai { get; set; }
        public virtual string NomeMae { get; set; }
        public virtual string Email { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual string CEP { get; set; }

        public virtual DateTime? DataExpedicao { get; set; }
        public virtual IList<Turma> ListaTurma { get; set; }

    }
}
