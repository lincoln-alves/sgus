using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP.DTO.Relatorios.DashBoard
{
    public class DTOEtapaNucleo
    {
        public int IdEtapaResposta { get; set; }
        public int IdProcessoResposta { get; set; }
        public string NomeEtapa { get; set; }
        public string NomeProcessoResposta { get; set; }
        public DateTime? Prazo { get; set; }
        public List<string> Responsaveis { get; set; }
        public enumPrazoEncaminhamentoDemanda? Situacao { get; set; }
        public IEnumerable<DTOEtapaNucleo> Etapas { get; set; }
        public DTOUsuario Analista { get; set; }
        public List<DTOUsuario> Analistas { get; set; }
        public DateTime? DataPreenchimento { get; set; }
        public byte OrdemEtapa { get; set; }
        public string Demandante { get; set; }

        public string ObterStatusFormatado
        {
            get
            {
                var status = new EtapaResposta().ObterStatusEncaminhamento(Prazo);

                return status != null ? EnumExtensions.GetDescription(status) : "";
            }
        }

        public string ObterAnalistas
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
                    var ctAnalistas = Analistas != null && Analistas.Count > 0 ? Analistas.Count() : Responsaveis.Count();

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

                return (!string.IsNullOrWhiteSpace(retorno) ? retorno : "");
            }
        }

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
