using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioStatusUsuariosTrilhas : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.StatusUsuariosTrilhas; }
        }

        public IList<Trilha> ObterTrilhasTodas()
        {
            using (BMTrilha bmTrilha = new BMTrilha())
            {
                return bmTrilha.ObterTrilhas();
            }
        }

        public IQueryable<TrilhaNivel> ObterNivelTrilha()
        {
            return new BMTrilhaNivel().ObterTodosIQueryable();
        }

        public IList<Uf> ObterUFTodas()
        {
            using (BMUf bmUf = new BMUf())
            {
                return bmUf.ObterTodos();
            }
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodas()
        {
            using (BMNivelOcupacional bmNivelOcupacional = new BMNivelOcupacional())
            {
                return bmNivelOcupacional.ObterTodos();
            }
        }

        public IList<StatusMatricula> ObterStatusMatriculaTodas()
        {
            using (BMStatusMatricula bmStatusMatricula = new BMStatusMatricula())
            {
                return bmStatusMatricula.ObterTodosIncluindoEspecificos();
            }
        }

        public IList<StatusMatricula> ObterStatusMatriculaTrilhas()
        {
            using (BMStatusMatricula bmStatusMatricula = new BMStatusMatricula())
            {
                return bmStatusMatricula.ObterStatusMatriculaDeTrilhas();
            }
        }

        public IList<DTORelatorioStatusUsuariosTrilhas> ObterStatusTrilhas(IEnumerable<int> statusMatricula, int? TrilhaId = null,
            int? UsuarioId = null, int? NivelTrilha = null, int? NivelOcupacionalId = null, int? UFId = null, DateTime? DataInicio = null, DateTime? DataFim = null, DateTime? DataLimite = null)
        {
            return new ManterTrilha().ObterStatusTrilhas(statusMatricula, TrilhaId, UsuarioId, NivelTrilha, NivelOcupacionalId, UFId, DataInicio, DataFim, DataLimite);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
