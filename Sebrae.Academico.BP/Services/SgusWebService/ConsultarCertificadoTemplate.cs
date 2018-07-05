using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarCertificadoTemplate : BusinessProcessServicesBase
    {

        public CertificadoTemplate ObterCertificado(int pIdMatriculaOferta, int pIdUsuarioTrilha)
        {

            CertificadoTemplate certificadoTemplate = null;

            try
            {
                if (pIdMatriculaOferta > 0)
                {

                    MatriculaOferta mo = new BMMatriculaOferta().ObterPorID(pIdMatriculaOferta);

                    if (!(mo != null && mo.Oferta.CertificadoTemplate != null && mo.Oferta.CertificadoTemplate.ID > 0))
                        return null;

                    if (mo.StatusMatricula == enumStatusMatricula.Concluido)
                    {
                        if (string.IsNullOrEmpty(mo.CDCertificado))
                        {
                            mo.CDCertificado = InfraEstrutura.Core.Helper.WebFormHelper.ObterStringAleatoria();
                            mo.DataGeracaoCertificado = DateTime.Now;
                            new BMMatriculaOferta().Salvar(mo);
                        }

                        return new BMCertificadoTemplate().ObterPorID(mo.Oferta.CertificadoTemplate.ID);
                    }
                    else
                        return certificadoTemplate;

                }

                else if (pIdUsuarioTrilha > 0)
                {
                    UsuarioTrilha ut = new BMUsuarioTrilha().ObterPorId(pIdUsuarioTrilha);

                    if (!(ut != null && ut.TrilhaNivel.CertificadoTemplate != null && ut.TrilhaNivel.CertificadoTemplate.ID > 0))
                        return null;

                    if (ut.StatusMatricula == enumStatusMatricula.Aprovado)
                    {
                        if (string.IsNullOrEmpty(ut.CDCertificado))
                        {
                            ut.CDCertificado = InfraEstrutura.Core.Helper.WebFormHelper.ObterStringAleatoria();
                            ut.DataGeracaoCertificado = DateTime.Now;
                            new BMUsuarioTrilha().Salvar(ut);
                        }

                        return new BMCertificadoTemplate().ObterPorID(ut.TrilhaNivel.CertificadoTemplate.ID);
                    }
                    else
                        return certificadoTemplate;
                }

                else
                    return certificadoTemplate;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return certificadoTemplate;
        }


        public IList<UsuarioTrilha> ObterListaUsuarioTrilha(Usuario usuario, TrilhaNivel trilhanivel)
        {

            IList<UsuarioTrilha> listaUsuarioTrilha = null;

            try
            {
                var lstResult = new BMUsuarioTrilha().ObterPorFiltro(new UsuarioTrilha()
                {
                    Usuario = usuario == null ? null : new BMUsuario().ObterPorId(usuario.ID),
                    TrilhaNivel = trilhanivel == null ? null : new BMTrilhaNivel().ObterPorID(trilhanivel.ID)
                });

                listaUsuarioTrilha = lstResult.Where(x => x.StatusMatricula == enumStatusMatricula.Aprovado &&
                                       string.IsNullOrWhiteSpace(x.CDCertificado)).ToList();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaUsuarioTrilha;
        }

        public IList<DTOCertificadoTemplate> ObterTodos()
        {
            IList<DTOCertificadoTemplate> lstResut = null;
            try
            {
                BMCertificadoTemplate certificadoTemplateBM = new BMCertificadoTemplate();

                lstResut = new List<DTOCertificadoTemplate>();

                foreach (CertificadoTemplate certificado in certificadoTemplateBM.ObterTodos())
                {
                    DTOCertificadoTemplate dto = new DTOCertificadoTemplate();
                    dto.Nome = certificado.Nome;
                    dto.ID = certificado.ID;
                    lstResut.Add(dto);
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return lstResut;
        }
    }
}
