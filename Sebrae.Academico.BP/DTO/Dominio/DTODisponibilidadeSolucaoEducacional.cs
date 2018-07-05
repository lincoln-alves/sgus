using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTODisponibilidadeSolucaoEducacional
    {
        public DTODisponibilidadeSolucaoEducacional()
        {
            OfertasDisponiveis = new List<DTOOferta>();
        }

        public int? IdOferta { get; set; }
        public int? IdSolucaoEducacional { get; set; }
        public DateTime? DataInicioOferta { get; set; }
        public DateTime? DataFimOferta { get; set; }
        public DateTime? DataInicioInscricoes { get; set; }
        public DateTime? DataFimInscricoes { get; set; }
        public bool? PermiteFilaEspera { get; set; }
        public string CargaHoraria { get; set; }
        public string Prazo { get; set; }
        public int CodigoDisponibilidade { get; set; }
        public string TextoDisponibilidade { get; set; }
        public bool InscricaoOnline { get; set; }

        public string NomeTermoAceite { get; set; }
        public string TextoTermoAceite { get; set; }
        public string TextoPoliticaConsequencia { get; set; }
        public string TextoInformacaoAdicional { get; set; }

        public int IdTipoOferta { get; set; }
        public int? IdTurma { get; set; }

        public List<DTOOferta> OfertasDisponiveis { get; set; }
    }

    public class DTOSolucoes
    {
        public List<DTOCurso> Cursos { get; set; }

        public DTOSolucoes()
        {
            Cursos = new List<DTOCurso>();
        }
    }

    public class DTOCurso
    {
        public virtual int IDSolucaoEducacional { get; set; }
        public virtual List<DTOOfertas> Ofertas { get; set; }

        public DTOCurso()
        {
            Ofertas = new List<DTOOfertas>();
        }
    }

    public class DTOOfertas
    {
        public virtual int IDOferta { get; set; }
        public virtual List<DTOOfertaTurma> ListaTurma { get; set; }
        public virtual int CargaHoraria { get; set; }
        public virtual bool TrancadoPorNaoPagamento { get; set; }
        public virtual string Nome { get; set; }
        public virtual bool PermiteAlterarStatusPeloGestor { get; set; }

        public DTOOfertas()
        {
            this.ListaTurma = new List<DTOOfertaTurma>();
        }
    }

    public class DTODisponibilidadeSolucaoEducacionalPorUsuarioT
    {
        public DTODisponibilidadeSolucaoEducacionalPorUsuarioT()
        {
            DTODisponibilidadeSolucaoEducacional = new List<DTODisponibilidadeSolucaoEducacional>();
        }

        public int QtdeSolucoes { get; set; }
        public int PaginaAtual { get; set; }
        public int QtdePaginas { get; set; }
        public virtual List<DTODisponibilidadeSolucaoEducacional> DTODisponibilidadeSolucaoEducacional { get; set; }
    }

    public class DTODisponibilidadeSolucaoEducacionalPorUsuario
    {
        public DTODisponibilidadeSolucaoEducacionalPorUsuario()
        {
            DTODisponibilidadeSolucaoEducacional = new List<DTOSolucaoEducacionalPorUsuario>();
        }

        public virtual List<DTOSolucaoEducacionalPorUsuario> DTODisponibilidadeSolucaoEducacional { get; set; }
    }

    public class DTOSolucaoEducacionalPorUsuario
    {
        public virtual int? IdOferta { get; set; }
        public virtual int? IdSolucaoEducacional { get; set; }
        public virtual DateTime? DataInicioOferta { get; set; }
        public virtual DateTime? DataFimOferta { get; set; }
        public virtual DateTime? DataInicioInscricoes { get; set; }
        public virtual DateTime? DataFimInscricoes { get; set; }
        public virtual Boolean? PermiteFilaEspera { get; set; }
        public virtual string CargaHoraria { get; set; }
        public virtual string Prazo { get; set; }
        public virtual int CodigoDisponibilidade { get; set; }
        public virtual string TextoDisponibilidade { get; set; }
    }

    public class DTOOfertaTurma
    {
        public virtual int IdTurma { get; set; }
        public virtual string Nome { get; set; }
    }

    public class DTOCursosPorCategoria
    {
        public virtual DTOCategoriaConteudo Categoria { get; set; }
        public virtual List<DTOSolucaoEducacional> ListaCursos { get; set; }
    }

}
