using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterCertificadoCertameService : BusinessProcessServicesBase
    {
        public List<DtoCertificadoCertame> ObterCertamesUsuario(string cpf)
        {
            var usuario = new ManterUsuario().ObterPorCPF(cpf);

            if (usuario == null)
                return null;

            if(usuario.ListaUsuarioCertificadoCertame.All(x => x.Status != enumStatusUsuarioCertificadoCertame.Aprovado))
                return null;

            return
                usuario.ListaUsuarioCertificadoCertame.Where(x => x.Status == enumStatusUsuarioCertificadoCertame.Aprovado).OrderBy(x => x.CertificadoCertame.Ano)
                    .ThenBy(x => x.CertificadoCertame.NomeCertificado)
                    .Select(x => new DtoCertificadoCertame
                    {
                        Id = x.CertificadoCertame.ID,
                        Ano = x.CertificadoCertame.Ano,
                        Nome =
                            $"{x.CertificadoCertame.Ano} - {x.CertificadoCertame.NomeCertificado}",
                        CertificadoId = x.ID
                    }).ToList();
        }

        public bool PossuiCertificadosCertamesUsuario(string cpf)
        {
            var usuario = new ManterUsuario().ObterPorCPF(cpf);

            if (usuario == null)
                return false;

            return usuario.ListaUsuarioCertificadoCertame.Any(x => x.Status == enumStatusUsuarioCertificadoCertame.Aprovado);
        }

        public List<DtoCertificadoCertame> ObterCertificadosCertameHistoricoAcademico(string login)
        {
            var usuario = new ManterUsuario().ObterPorCPF(login);

            var listaCertificado = new ManterUsuarioCertificadoCertame()
                .ObterCertamesPorUsuario(usuario)
                .Where(x => x.Status == enumStatusUsuarioCertificadoCertame.Aprovado)
                .Select(x => new DtoCertificadoCertame
                {
                    Id = x.ID,
                    CertificadoId = x.CertificadoCertame.ID,
                    Nome = x.CertificadoCertame.NomeCertificado,
                    Ano = x.CertificadoCertame.Ano
                }).ToList();

            return listaCertificado;
        }

        public byte[] ConsultarCertificadoCertame(int certificadoId, string login)
        {
            var result = CertificadoTemplateUtil.ConsultarCertificadoCertame(certificadoId, login);

            if (result != null)
            {
                var usuario = new ManterUsuario().ObterPorCPF(login);
                var manterUsuarioCertificado = new ManterUsuarioCertificadoCertame();
                var usuarioCertificado = manterUsuarioCertificado
                    .ObterCertamesPorUsuario(usuario)
                    .Where(x => x.Status == enumStatusUsuarioCertificadoCertame.Aprovado)
                    .FirstOrDefault(x => x.CertificadoCertame.ID == certificadoId);

                if (usuarioCertificado != null)
                {
                    usuarioCertificado.DataDownload = DateTime.Now;
                    manterUsuarioCertificado.Salvar(usuarioCertificado);
                }
            }

            return result;
        }

        public byte[] ConsultarBoletimCertame(int certificadoId, string login)
        {
            var horaAtual = DateTime.Now;

            var result = CertificadoTemplateUtil.ConsultarBoletimCertame(certificadoId, login, horaAtual);

            if (result != null)
            {
                var usuario = new ManterUsuario().ObterPorCPF(login);
                var manterUsuarioCertificado = new ManterUsuarioCertificadoCertame();
                var usuarioCertificado = manterUsuarioCertificado
                    .ObterCertamesPorUsuario(usuario)
                    .Where(x => x.Status == enumStatusUsuarioCertificadoCertame.Aprovado)
                    .FirstOrDefault(x => x.CertificadoCertame.ID == certificadoId);
                
                if (usuarioCertificado != null)
                {
                    usuarioCertificado.DataDownloadBoletim = horaAtual;
                    manterUsuarioCertificado.Salvar(usuarioCertificado);
                }
            }

            return result;
        }
    }
}