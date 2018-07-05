using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOEtapa
    {
        public DTOEtapa()
        {
            ListaCampos = new List<DTOCampo>();
            Analistas = new List<DTOUsuario>();
            AnalistasPorNucleo = new List<DTOEtapaPermissaoNucleo>();
        }
        public virtual int ID { get; set; }
        public virtual DTOProcessoInfo Processo { get; set; }
        public virtual string Nome { get; set; }
        public virtual int Status { get; set; }
        public virtual bool RequerAprovacao { get; set; }
        public virtual int IDEtapaRetorno { get; set; }
        public virtual List<DTOCampo> ListaCampos { get; set; }
        public virtual int ID_RespostaEtapa { get; set; }
        public virtual List<DTOEtapa> ListaEtapasRespostasInativas { get; set; }
        public virtual string DataPreenchimento { get; set; }
        public virtual DateTime? DataPreenchimentoDateTime { get; set; }
        public virtual string LinkAnexo { get; set; }
        public virtual string NomeAnexo { get; set; }
        public virtual string NomeFinalizacaoBotao { get; set; }
        public virtual string NomeReprovacaoBotao { get; set; }
        public virtual int Ordem { get; set; }
        public virtual bool LiberarCancelamento { get; set; }

        public virtual bool PodeSerReprovada { get; set; }

        public virtual bool AnalisavelPorNucleoUC { get; set; }
        public virtual List<DTOEtapaPermissaoNucleo> AnalistasPorNucleo { get; set; }

        // Propriedades da etapa resposta mas emprestadas aqui para facilitar o uso do WS
        public virtual DTOSituacaoProcesso Situacao { get; set; }
        public virtual DTOUsuario Analista { get; set; }
        public virtual DTOUsuario Assessor { get; set; }
        public virtual List<DTOUsuario> Analistas { get; set; }

        public virtual List<string> Responsaveis { get; set; }

        public virtual DTOEtapaEncaminhamentoUsuario EtapaEncaminhamentoUsuario { get; set; }

        public virtual string ObterAnalistas
        {
            get
            {
                var retorno = string.Empty;

                if (Analista != null && !string.IsNullOrWhiteSpace(Analista.Nome))
                {
                    retorno = UppercaseFirst(Analista.Nome);
                }
                else
                {
                    var ctAnalistas = 0;

                    if (Analistas != null && Analistas.Count > 0  || Responsaveis != null)
                    {
                        ctAnalistas = Analistas != null && Analistas.Count > 0 ? Analistas.Count() : Responsaveis.Count();
                    }

                    if (Analistas != null && Analistas.Count > 0)
                    {
                        foreach (var analista in Analistas)
                        {
                            retorno += UppercaseFirst(analista.Nome);

                            if (Analistas.Count() > 1)
                            {
                                var indice = Analistas.IndexOf(analista);

                                retorno += indice == (ctAnalistas - 2) ? " ou " : indice != ctAnalistas - 1 ? ", " : "";
                            }
                        }
                    }
                    else if (Responsaveis != null && Responsaveis.Count() > 0)
                    {
                        var indice = 0;
                        foreach (var responsavel in Responsaveis)
                        {
                            retorno += UppercaseFirst(responsavel);
                            retorno += indice == (ctAnalistas - 2) ? " ou " : "";
                            indice++;
                        }
                    }
                }

                return "<strong>" + Situacao.Nome + "</strong>" + (!string.IsNullOrWhiteSpace(retorno) ? " por " + retorno : "");
            }
        }

        public string NomeBotaoAjuste { get; set; }

        //private static string UppercaseFirst(string s)
        static string UppercaseFirst(string value)
        {
            value = value.ToLower();

            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
    }
}
