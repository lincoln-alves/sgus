using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioMetaDesenvolvimentoSE : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.MetaDeDesenvolvimentoVersusSolucaoEducacional; }
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodos()
        {
            using (BMNivelOcupacional niBM = new BMNivelOcupacional())
            {
                return niBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<DTORelatorioMetaDesenvolvimentoSE> ConsultarMetaDesenvolvimentoSe(int? pIdNivelOcupacional, int? pIdUf, int? pIdCategoriaConteudo)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarMetaDesenvolvimentoSe(pIdNivelOcupacional, pIdUf, pIdCategoriaConteudo);

        }

        public IList<Uf> ObterUfTodos(Uf ufSelecionada = null)
        {
            using (var ufBm = new BMUf())
            {
                return ufSelecionada != null ?
                    ufBm.ObterTodos().Where(x => x.ID == ufSelecionada.ID).OrderBy(x => x.Nome).ToList() :
                    ufBm.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<CategoriaConteudo> ObterCategoriaConteudoTodos(Uf ufSelecionada = null)
        {
            using (var cseBm = new BMCategoriaConteudo())
            {
                var retorno = cseBm.ObterTodos();

                if (ufSelecionada != null)
                    retorno =
                        retorno.Where(
                            x =>
                                x.ListaSolucaoEducacional.Any(s => s.UFGestor != null && s.UFGestor.ID == ufSelecionada.ID));

                return retorno.ToList();
            }
        }


        public void Dispose()
        {
            GC.Collect();
        }
    }
}
