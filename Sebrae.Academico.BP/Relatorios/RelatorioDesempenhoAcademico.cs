using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.BM.Classes.Moodle.Views;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioDesempenhoAcademico : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.DesempenhoAcademico; }
        }

        public IList<NivelOcupacional> GetNivelOcupacionalTodos()
        {
            using (var noBM = new BMNivelOcupacional())
            {
                return noBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<PublicoAlvo> ObterPublicoAlvoTodos()
        {
            using (var bmPublicoAlvo = new BMPublicoAlvo())
            {
                var query = bmPublicoAlvo.ObterTodos().AsQueryable();
                
                return query.OrderBy(x => x.Nome).ToList();
            }
        }

        public IQueryable<SolucaoEducacional> ObterSolucoesEducacionais(Uf uf = null)
        {
            using (var bmSe = new BMSolucaoEducacional())
            {
                var ses = bmSe.ObterTodos();

                if (uf != null)
                    bmSe.FiltrarPermissaoVisualizacao(ref ses, uf.ID);

                return ses;
            }
        }

        public IQueryable<Oferta> ObterOfertas(int idSolucaoEducacional, int idUF = 0)
        {
            using (var bmO = new BMOferta())
            {
                var retorno = bmO.ObterTodos();

                if (idSolucaoEducacional != 0)
                    retorno = retorno.Where(x => x.SolucaoEducacional.ID == idSolucaoEducacional);

                if (idUF > 0)
                    retorno =
                        retorno.Where(
                            x =>
                                x.ListaPermissao.All(c => c.Uf != null) ||
                                x.ListaPermissao.Any(c => c.Uf != null && c.Uf.ID == idUF));

                return retorno;
            }
        }

        public IQueryable<Turma> ObterTurmas(int idOferta, int idUf = 0)
        {
            using (var bmO = new BMTurma())
            {
                var retorno = bmO.ObterTodos();

                if (idOferta != 0)
                    retorno = retorno.Where(x => x.Oferta.ID == idOferta);

                if (idUf > 0)
                    retorno =
                        retorno.Where(
                            x =>
                                x.Oferta.ListaPermissao.All(c => c.Uf == null) ||
                                x.Oferta.ListaPermissao.Any(c => c.Uf != null && c.Uf.ID == idUf));

                return retorno;
            }
        }

        public IList<StatusMatricula> ObterStatusMatriculaTodos()
        {
            BMStatusMatricula bmStatusMatricula = new BMStatusMatricula();
            return bmStatusMatricula.ObterTodosIncluindoEspecificos().OrderBy(x => x.Nome).ToList();
        }

        public IList<DTODesempenhoAcademico> ConsultarDesempenhoAcademico(DTOFiltroDesempenhoAcademico dTOFiltroDesempenhoAcademico)
        {
            RegistrarLogExecucao();

            return new ManterMatricula().ConsultarDesempenhoAcademico(dTOFiltroDesempenhoAcademico.GetParams());
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IEnumerable<FormaAquisicao> ObterFormaDeAquisicaoTodos()
        {
            return new BMFormaAquisicao().ObterTodos().ToList();
        }
    }

}