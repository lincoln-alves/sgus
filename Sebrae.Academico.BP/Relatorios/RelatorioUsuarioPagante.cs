using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioPagante : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.UsuarioPagante; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<DTORelatorioUsuarioPagante> ConsultarRelatorio(string nome, string cpf, int? idUf, int? idNivelOcupacional, int? tipo)
        {
            RegistrarLogExecucao();

            return (new ManterUsuario()).ConsultarRelatorioUsuariosPagantes(nome, cpf, idUf, idNivelOcupacional, tipo);
        }


    }
}
