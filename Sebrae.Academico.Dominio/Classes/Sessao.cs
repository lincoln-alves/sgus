using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Dominio.Classes
{
    /// <summary>
    /// Sessão gerenciada pela aplicação
    /// </summary>
    public class Sessao : EntidadeBasicaPorId
    {
        public virtual string Valor { get; set; }

        public virtual int Hash { get; set; }

        public Sessao()
        {
        }

        public Sessao(string Valor)
        {
            this.Valor = Valor;
            Hash = this.Hash <= 0 ? (new Random().Next(0, Math.Abs(this.Valor.GetHashCode()))) : this.Hash;
        }
    }
}
