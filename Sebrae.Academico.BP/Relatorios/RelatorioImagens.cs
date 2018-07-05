using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioImagens : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.Imagens; }
        }
        public IList<DTORelatorioImagens> ConsultarRelatorioImagens()
        {
            return new List<DTORelatorioImagens>();
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
