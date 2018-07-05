using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioCertificadoConhecimento : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio { get; }

        private ManterUsuarioCertificadoCertame manter;

        public RelatorioUsuarioCertificadoConhecimento()
        {
            manter = new ManterUsuarioCertificadoCertame();
        }

        public IQueryable<UsuarioCertificadoCertame> ObterCertamesUsuario()
        {
            return manter.ObterTodosIQueryable();
        }

        public IEnumerable<DTORelatorioUsuarioCertificadoCertame> ObterCertamesPorUsuario(Usuario usuario)
        {
            return manter.ObterTodos().Where(x => x.Usuario.ID == usuario.ID).OrderByDescending(x => x.CertificadoCertame.Ano).Select(x => new DTORelatorioUsuarioCertificadoCertame()
            {
                Nome = x.Usuario.Nome,
                CPF = x.Usuario.CPF,
                UF = x.Usuario.UF.Sigla,
                Unidade = x.Usuario.Unidade,
                Ano = x.CertificadoCertame.Ano,
                TemaCertificacao = x.CertificadoCertame.NomeCertificado,
                DataDownload = x.DataDownload != null ? x.DataDownload.Value.ToString("dd/MM/yyyy") : ""
            });
        }

        public void Dispose()
        {
            manter.Dispose();
        }
    }
}
