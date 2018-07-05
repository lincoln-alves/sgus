using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioHistoricoAcademico : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.HistoricoAcademico; }
        }

        public IList<DTOHistoricoAcademicoDadosGerais> ConsultarHistoricoAcademicoDadosGerais(int pIdUsuario)
        {

            using (BMUsuario userBm = new BMUsuario())
            {
                IList<DTOHistoricoAcademicoDadosGerais> lstResut = new List<DTOHistoricoAcademicoDadosGerais>();
                Usuario u = userBm.ObterPorId(pIdUsuario);

                var dg = new DTOHistoricoAcademicoDadosGerais()
                {
                    Nome = u.Nome,
                    CPF = u.CPF,
                    NivelOcupacional = u.NivelOcupacional.Nome,
                    UF = u.UF.Nome
                };

                lstResut.Add(dg);

                return lstResut;
            }

        }

        public IList<DTOHistoricoAcademicoCursos> ConsultaHistoricoAcademicoCursos(int pIdUsuario)
        {
            BMMatriculaOferta moBM = new BMMatriculaOferta();

            List<MatriculaOferta> lista = new List<MatriculaOferta>();
            lista = moBM.ObterPorUsuario(pIdUsuario).Where(x => x.StatusMatricula == enumStatusMatricula.Concluido || x.StatusMatricula == enumStatusMatricula.Aprovado).ToList();
            
            IList<DTOHistoricoAcademicoCursos> retorno = new List<DTOHistoricoAcademicoCursos>();

            foreach (var registro in lista)
            {
                DTOHistoricoAcademicoCursos itemAdicionar = new DTOHistoricoAcademicoCursos();
                itemAdicionar.NomeCurso = registro.Oferta.SolucaoEducacional.Nome;
                if (registro.MatriculaTurma != null && registro.MatriculaTurma.Count > 0 && registro.MatriculaTurma.FirstOrDefault().DataTermino.HasValue)
                    itemAdicionar.DataConclusao = registro.MatriculaTurma.FirstOrDefault().DataTermino.Value;
                itemAdicionar.FormaAquisicao = registro.Oferta.SolucaoEducacional.FormaAquisicao.Nome;
                itemAdicionar.CargaHoraria = registro.Oferta.CargaHoraria;
                itemAdicionar.Fornecedor = registro.Oferta.SolucaoEducacional.Fornecedor.Nome;
                if (registro.MatriculaTurma != null && registro.MatriculaTurma.Count > 0 && registro.MatriculaTurma.FirstOrDefault().MediaFinal.HasValue)
                    itemAdicionar.MediaFinal = registro.MatriculaTurma.FirstOrDefault().MediaFinal.Value;
                retorno.Add(itemAdicionar);

            }
            return retorno;
            

        }

        public IList<DTOHistoricoAcademicoSGTC> ConsultaHistoricoAcademicoSGTC(int pIdUsuario)
        {

            using (BMHistoricoSGTC histSGTCBM = new BMHistoricoSGTC())
            {
                return (from h in histSGTCBM.ObterPorUsuario(pIdUsuario)
                        select new DTOHistoricoAcademicoSGTC()
                        {
                            DataConclusao = h.DataConclusao,
                            NomeCurso = h.NomeSolucaoEducacional,
                            CargaHoraria = 0 //TODO: Colocado aqui para testar. Aguardando o Marcelo para saber se realmente este atributo irá exibi
                        }).ToList();
            }

        }

        public void RegistrarLog()
        {
            this.RegistrarLogExecucao();
        }

        public IList<DTOHistoricoAcademicoTrilha> ConsultarHistoricoAcademicoTrilha(int pIdUsuario)
        {
            using (BMUsuario uBM = new BMUsuario())
            {
                using (BMUsuario userTrilhaBM = new BMUsuario())
                {

                    return (from u in uBM.ObterPorId(pIdUsuario).ListaMatriculasNaTrilha

                            select new DTOHistoricoAcademicoTrilha()
                                {
                                    NomeTrilha = u.TrilhaNivel.Trilha.Nome,
                                    NivelTrilha = u.TrilhaNivel.Nome,
                                    DataConclusao = u.DataGeracaoCertificado,
                                    CargaHoraria = u.TrilhaNivel.CargaHoraria

                                }
                            ).ToList();
                }
            }
        }

        public IList<DTOHistoricoAcademicoPrograma> ConsultarHistoricoAcademicoPrograma(int pIdUsuario)
        {
            using (BMUsuario usuarioBM = new BMUsuario())
            {

                return (from p in usuarioBM.ObterPorId(pIdUsuario).ListaMatriculaPrograma
                        select new DTOHistoricoAcademicoPrograma()
                            {
                                NomePrograma = p.Programa.Nome,
                                DataConclusao = p.DataFim,
                                CargaHoraria = 0 //TODO: Colocado aqui para testar. Aguardando o Marcelo para saber se realmente este atributo irá exibir
                            }
                        ).ToList();
            }

        }

        public IList<DTOHistoricoAcademicoExtracurricular> ConsultarHistoricoAcademicoExtracurricular(int pIdUsuario)
        {
            using (BMUsuario uBM = new BMUsuario())
            {
                return (from u in uBM.ObterPorId(pIdUsuario).ListaHistoricoExtraCurricular
                        select new DTOHistoricoAcademicoExtracurricular()
                        {
                            CargaHoraria = u.CargaHoraria,
                            DataConclusao = u.DataFimAtividade,
                            Instituicao = u.Instituicao,
                            NomeCurso = u.SolucaoEducacionalExtraCurricular
                        }).ToList();
            }
        }


        public void Dispose()
        {
            GC.Collect();
        }


    }
}
