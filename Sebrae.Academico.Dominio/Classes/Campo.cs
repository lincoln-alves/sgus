using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Campo : EntidadeBasica
    {
        public Campo()
        {
            ListaAlternativas = new List<Alternativa>();
            ListaMetaValues = new List<CampoMetaValue>();
            ListaCamposVinculados = new List<Campo>();
            CamposVinculadosOriginaisIDs = new List<int>();
            ListaCampoPorcentagem = new List<CampoPorcentagem>();
        }

        public virtual int OriginalID { get; set; }
        public virtual List<int> CamposVinculadosOriginaisIDs { get; set; }

        public virtual Etapa Etapa { get; set; }
        public virtual CampoResposta Resposta { get; set; }
        public virtual byte Ordem { get; set; }
        public virtual int Tamanho { get; set; }
        public virtual byte TipoDado { get; set; }
        public virtual byte TipoCampo { get; set; }
        public virtual bool PermiteNulo { get; set; }
        public virtual bool SomenteNumero { get; set; }
        public virtual bool SomenteLetra { get; set; }
        public virtual int Largura { get; set; }
        public virtual bool ExibirImpressao { get; set; }
        public virtual bool ExibirAjudaImpressao { get; set; }
        public virtual bool CampoDivisor { get; set; }
        public virtual string Ajuda { get; set; }
        public virtual Questionario Questionario { get; set; }
        public virtual OrcamentoReembolso OrcamentoReembolso { get; set; }
        public virtual IList<Alternativa> ListaAlternativas { get; set; }
        public virtual IList<CampoMetaValue> ListaMetaValues { get; set; }
        public virtual IList<Campo> ListaCamposVinculados { get; set; }
        public virtual IList<CampoPorcentagem> ListaCampoPorcentagem { get; set; }

        public virtual void AdicionarCampo(Campo campo)
        {
            ListaCamposVinculados.Add(campo);
        }

        public virtual void RemoverCampo(Campo campo)
        {
            ListaCamposVinculados.Remove(campo);
        }

        public virtual void RemoverTodosCamposVinculados()
        {
            ListaCamposVinculados = new List<Campo>();
        }
    }
}
