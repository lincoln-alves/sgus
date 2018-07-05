using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services.Questionario;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOCampo
    {
        public DTOCampo()
        {
            TipoCampo = new DTOTipoFormulario();
            ListaAlternativas = new List<DTOAlternativa>();
            ListaMetaValues = new List<DTOMetaValue>();
            CamposVinculadosDivisao = new List<DTOCampo>();
            ListaCampoPorcentagem = new List<DTOCampo>();
            Arquivos = new List<DTOCampoArquivo>();

        }

        public virtual int ID { get; set; }
        public virtual string Titulo { get; set; }
        //public virtual string Label { get; set; }
        public virtual DTOTipoFormulario TipoCampo { get; set; }
        public virtual List<DTOAlternativa> ListaAlternativas { get; set; }
        // Somente usado para o TipoCampo Arquivo
        public virtual DTOCampoArquivo Arquivo { get; set; }
        public virtual List<DTOCampoArquivo> Arquivos { get; set; }
        public virtual string Resposta { get; set; }
        public virtual bool PermiteNulo { get; set; }
        public virtual bool SomenteLetra { get; set; }
        public virtual bool SomenteNumero { get; set; }
        public virtual string TipoDado { get; set; }
        public virtual int Tamanho { get; set; }
        public virtual int Largura { get; set; }
        public virtual bool ExibirAjudaImpressao { get; set; }
        public virtual bool CampoDivisor { get; set; }
        public virtual DTOQuestionario Questionario { get; set; }
        public virtual bool Disabled { get; set; }

        public virtual List<DTOQuestionarioParticipacao> QuestionarioParticipacao { get; set; }

        public virtual string Ajuda { get; set; }

        public virtual decimal TotalSomatorio { get; set; }

        public virtual List<DTOMetaValue> ListaMetaValues { get; set; }

        public virtual List<int> CamposVinculados { get; set; }

        public virtual List<DTOCampo> CamposVinculadosDivisao { get; set; }
        public virtual List<DTOCampo> ListaCampoPorcentagem { get; set; }

        private string _ObterTexto { get; set; } 

        public string ObterTexto()
        {
            if(!string.IsNullOrWhiteSpace(_ObterTexto))
                return _ObterTexto;

            switch ((enumTipoCampo)TipoCampo.ID)
            {
                case enumTipoCampo.CheckBox:
                    if (ListaAlternativas.Any())
                    {
                        var resposta =
                            ListaAlternativas.Where(alternativa => alternativa.OpcaoRespondida)
                                .Aggregate(" - ", (current, alternativa) => current + (alternativa.Nome + ", "));

                        _ObterTexto = Titulo + ": " + resposta;

                        return _ObterTexto;
                    }

                    _ObterTexto = Titulo + ": Não respondido";

                    return _ObterTexto;
                default:
                    _ObterTexto = Titulo + ": " + Resposta;

                    return _ObterTexto;
            }
        }
    }
}