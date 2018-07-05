using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioMatriculadoPrograma : BusinessProcessBaseRelatorio, IDisposable
    {

        public void Dispose()
        {
            GC.Collect();
        }

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.UsuarioMatriculadoPorPrograma; }
        }

        public IList<Programa> ObterProgramaTodos()
        {
            using (BMPrograma progBM = new BMPrograma())
            {
                return progBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<DTORelatorioUsuarioMatriculadoPrograma> ConsultarUsuarioMatriculaPrograma(int pIdPrograma, int pStatusMatricula)
        {
            this.RegistrarLogExecucao();

            return (new ManterPrograma()).ConsultarUsuarioMatriculaPrograma(pIdPrograma, pStatusMatricula);
        }

    }
}
