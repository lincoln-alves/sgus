using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUnificadoSolucaoEducacional : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.UnificadoSolucaoEducacional; }
        }

        public IList<FormaAquisicao> ObterFormasDeAquisicao()
        {
            var bmFormaAquisicao = new BMFormaAquisicao();
            return bmFormaAquisicao.ObterTodos();
        }

        public IList<TipoOferta> ObterTiposOferta()
        {
            var bmTipoOferta = new BMTipoOferta();
            return bmTipoOferta.ObterTodos();
        }

        public IQueryable<Programa> ObterProgramas()
        {
            return new BMPrograma().ObterTodos();
        }

        public IQueryable<CategoriaConteudo> ObterCategorias()
        {
            return new BMCategoriaConteudo().ObterTodos();
        }

        public IList<PublicoAlvo> ObterPublicosAlvo()
        {
            var bmPublicoAlvo = new BMPublicoAlvo();
            return bmPublicoAlvo.ObterTodos();
        }

        public IList<NivelOcupacional> ObterNiveisOcupacionais()
        {
            var bmNivelOcupacional = new BMNivelOcupacional();
            return bmNivelOcupacional.ObterTodos();
        }

        public IList<Perfil> ObterPerfis()
        {
            var bmPerfil = new BMPerfil();
            return bmPerfil.ObterTodos();
        }

        public IList<Uf> ObterUFS()
        {
            var bmUF = new BMUf();
            return bmUF.ObterTodos();
        }

        public IList<DTOUnificadoSolucaoEducacional> ConsultarSolucaoEducacional(IEnumerable<int> pFormasDeAquisicao,
            IEnumerable<int> pTiposDeOferta, IEnumerable<int> pProgramas, IEnumerable<int> pCategorias,
            IEnumerable<int> pPublicoAlvo, IEnumerable<int> pNiveisOcupacionais, IEnumerable<int> pPerfis,
            IEnumerable<int> pUf, IEnumerable<int> pUfResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarSolucaoEducacionalUnificado(pFormasDeAquisicao,
                pTiposDeOferta, pProgramas, pCategorias, pPublicoAlvo, pNiveisOcupacionais, pPerfis, pUf, pUfResponsavel);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}