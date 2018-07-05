using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioMatriculadoSEFormaAquisicao : BusinessProcessBaseRelatorio, IDisposable
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.UsuarioMatriculadoEmSolucoesEducacionaisPorFormaDeAquisicao; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (var formaAquisicaoBM = new BMFormaAquisicao())
            {
                return formaAquisicaoBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<DTORelatorioUsuarioMatriculadoSEFormaAquisicao>
            ConsultarRelatorioUsuarioMatriculadoSeFormaAquisicao(List<int> idsFormaAquisicao, int? pStatusMatricula,
                DateTime? pDataInicioTurma, DateTime? pDataFimTurma, int? idUf = null, IEnumerable<int> pUfResponsavel = null)
        {
            RegistrarLogExecucao();

            return (new ManterFormaAquisicao()).ConsultarRelatorioUsuarioMatriculadoSeFormaAquisicao(idsFormaAquisicao,
                pStatusMatricula, pDataInicioTurma, pDataFimTurma, idUf, pUfResponsavel);
        }
        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

    }
}
