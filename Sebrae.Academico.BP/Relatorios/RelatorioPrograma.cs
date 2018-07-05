using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioPrograma : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.RelacaoDeProgramas; }
        }

        public IQueryable<Programa> ObterProgramas()
        {
            return new BMPrograma().ObterTodos();
        }

        public DataTable ConsultarProgramas(int? id, int? idCapacitacao,int? idTurmaCapacitacao, int? idModulo)
        {
            RegistrarLogExecucao();
            return (new ManterPrograma()).ConsultarProgramasTable(id, idCapacitacao, idTurmaCapacitacao,idModulo);
        }

        public IList<DTORelatorioPrograma> ConsultarProgramas(string pNome)
        {
            this.RegistrarLogExecucao();

            return (new ManterPrograma()).ConsultarProgramas(pNome);

        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
