using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolicitacaoRelatorio
    {
        public virtual int ID { get; set; }

        public virtual DateTime DataSolicitacao { get; set; }

        public virtual DateTime? DataGeracao { get; set; }

        public virtual string Nome { get; set; }

        public virtual string NomeAmigavel { get; set; }

        public virtual string Saida { get; set; }
        public virtual string Descricao { get; set; }

        public virtual FileServer Arquivo { get; set; }

        public virtual bool? Baixado { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual bool? Falha { get; set; }

        public virtual long? QuantidadeRegistros { get; set; }

        public virtual enumTipoSaidaRelatorio ObterSaidaEnum()
        {
            switch (Saida)
            {
                case "PDF":
                    return enumTipoSaidaRelatorio.PDF;
                case "WORD":
                    return enumTipoSaidaRelatorio.WORD;
                case "EXCEL":
                    return enumTipoSaidaRelatorio.EXCEL;
            }

            return enumTipoSaidaRelatorio.PDF;
        }
    }
}
