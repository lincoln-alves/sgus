using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
namespace Sebrae.Academico.Dominio.Classes
{
    public class Fornecedor : EntidadeBasica
    {
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual DateTime? DataUltimoAcesso { get; set; }
        public virtual int? QuantidadeAcessos { get; set; }
        public virtual IList<SolucaoEducacional> ListaSolucaoEducacional { get; set; }
        public virtual string LinkAcesso { get; set; }
        public virtual string TextoCriptografia { get; set; }
        public virtual string NomeApresentacao { get; set; }
        public virtual bool PermiteGestaoSGUS { get; set; }
        public virtual bool PermiteCriarOferta { get; set; }
        public virtual bool PermiteCriarTurma { get; set; }
        public virtual bool ApresentarComoFornecedorNoPortal { get; set; }
        public virtual IEnumerable<FornecedorUF> ListaFornecedorUF { get; protected internal set; }
        public virtual enumFornecedor? FornecedorSistema { get; set; }

        #region "Lista que não terá integridade referencial (chave estrangeira) - foreign key - fk"

        public virtual IList<LogAcessoFornecedor> ListaLogAcesoFornecedor { get; set; }

        #endregion

        public Fornecedor()
        {
            ListaFornecedorUF = new List<FornecedorUF>();
        }

        public virtual void RemoverUf(FornecedorUF fornecedorUf)
        {
            var lista = (IList<FornecedorUF>)ListaFornecedorUF;
            lista.Remove(fornecedorUf);
        }

        public virtual void AdicionarUf(FornecedorUF fornecedorUf)
        {
            var lista = (IList<FornecedorUF>)ListaFornecedorUF;
            lista.Add(fornecedorUf);
        }
    }
}