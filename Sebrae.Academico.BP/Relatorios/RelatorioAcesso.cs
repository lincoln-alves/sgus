using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioAcesso : BusinessProcessBaseRelatorio, IDisposable
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.AcessoDeUsuarios; }
        }

        public IList<DTORelatorioAcesso> ConsultarRelatorioAcesso(int? pIdUsuario, int? pIdUf, int? pIdNivelOcupacional, int? pIdPerfil, DateTime? pDataInicial, DateTime? pDataFinal)
        {

            RegistrarLogExecucao();

            return (new ManterLogAcesso()).ConsultarRelatorioAcesso(pIdUsuario,pIdUf,pIdNivelOcupacional,pIdPerfil,pDataInicial,pDataFinal);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
