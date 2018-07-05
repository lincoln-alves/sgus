using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOOferta : DTOEntidadeBasica
    {
        public DTOOferta()
        {

        }

        public DTOOferta(Oferta oferta)
        {
            ID = oferta.ID;
            Nome = oferta.Nome;
            TipoOferta = new DTOTipoOferta { ID = (int)oferta.TipoOferta };
            FiladeEspera = oferta.FiladeEspera;
            Prazo = oferta.DiasPrazo ?? 0;
            CargaHoraria = oferta.CargaHoraria;
            InscricaoOnline = oferta.InscricaoOnline == true;
            InformacaoAdicional = oferta.InformacaoAdicional;
            Link = oferta.Link;

            if (oferta.DataInicioInscricoes.HasValue)
                DataInicioInscricoes = oferta.DataInicioInscricoes.Value;

            if (oferta.DataFimInscricoes.HasValue)
                DataFimInscricoes = oferta.DataFimInscricoes.Value;

            if (oferta.ValorPrevisto.HasValue)
                ValorPrevisto = oferta.ValorPrevisto.Value;

            Turmas = oferta.ListaTurma.Where(x => x.Status != enumStatusTurma.Cancelada && x.InAberta).Select(x => new DTOTurma
            {
                ID = x.ID,
                Nome = x.Nome,
                DataInicio = x.DataInicio,
                DataFinal = x.DataFinal,
                Local = string.IsNullOrWhiteSpace(x.Local) ? null : x.Local,
                VagasDisponiveis = (x.QuantidadeMaximaInscricoes -  x.QuantidadeAlunosMatriculadosNaTurma)
            })
                .ToList();
        }

        public bool RequerPagamento { get; set; }

        public DTOTipoOferta TipoOferta { get; set; }
        public DTOSolucaoEducacional SolucaoEducacional { get; set; }
        public string ChaveExterna { get; set; }
        public bool FiladeEspera { get; set; }
        //public DateTime DataInicio { get; set; }
        //public DateTime DataFim { get; set; }
        public DateTime DataInicioInscricoes { get; set; }
        public DateTime DataFimInscricoes { get; set; }
        public bool InscricaoOnline { get; set; }
        public double ValorPrevisto { get; set; }
        public int Prazo { get; set; }
        public int CargaHoraria { get; set; }
        public string InformacaoAdicional { get; set; }

        public List<DTOTurma> Turmas { get; set; }
    }
}