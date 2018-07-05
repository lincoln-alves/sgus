using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioHorasCapacitacao : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.HorasCapacitacao; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<Perfil> ObterPerfilTodos()
        {
            using (BMPerfil perfilBM = new BMPerfil())
            {
                return perfilBM.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<Uf> ObterUFTodos()
        {
            using (BMUf ufBM = new BMUf())
            {
                return ufBM.ObterTodos().ToList();
            }
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (BMFormaAquisicao formaAquisicaoBM = new BMFormaAquisicao())
            {
                return formaAquisicaoBM.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodos()
        {
            using (var nivelBm = new BMNivelOcupacional())
            {
                return nivelBm.ObterTodos().OrderBy(n => n.Nome).ToList();
            }
        }

        public IList<StatusMatricula> ObterStatusMatriculaTodos()
        {
            return Enum.GetValues(typeof(enumStatusMatricula))
                .Cast<enumStatusMatricula>()
                .Select(enumStatus =>
                    new StatusMatricula() 
                    {
                        ID = (int)enumStatus, 
                        Nome = enumStatus.ToString() 
                    }).ToList();
        }

        public IQueryable<SolucaoEducacional> ObterSolucaoEducacionalPorFormaAquisicao(int idFormaAquisicao = 0, Uf uf = null)
        {
            using (var solEduBm = new BMSolucaoEducacional())
            {
                var retorno = solEduBm.ObterTodos();

                if (idFormaAquisicao > 0)
                    retorno = retorno.Where(s => s.FormaAquisicao.ID == idFormaAquisicao);

                if(uf != null)
                    solEduBm.FiltrarPermissaoVisualizacao(ref retorno, uf.ID);

                return retorno;
            }
        }
        
        public IList<DTORelatorioHorasCapacitacao> ConsultarRelatorio(int? idPerfil, int? idUf, int? idNivelOcupacional, int? idFormaAquisicao, int? idSolucaoEducacional, int? idStatusMatricula, DateTime? dataInicial, DateTime? dataFinal, IEnumerable<int> pUfResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterCapacitacao()).ConsultarRelatorioCapacitacao(idPerfil, idUf, idNivelOcupacional, idFormaAquisicao,
                idSolucaoEducacional, idStatusMatricula, dataInicial, dataFinal, pUfResponsavel);
        }
    }
}
