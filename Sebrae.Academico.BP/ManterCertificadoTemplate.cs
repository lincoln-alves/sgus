using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Linq;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterCertificadoTemplate : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMCertificadoTemplate bmCertificadoTemplate = null;

        #endregion

        #region "Construtor"

        public ManterCertificadoTemplate()
            : base()
        {
            bmCertificadoTemplate = new BMCertificadoTemplate();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<CertificadoTemplate> ObterTodosAtivos()
        {
            return bmCertificadoTemplate.ObterTodosAtivos();
        }

        public void IncluirCertificadoTemplate(CertificadoTemplate certificadoTemplate)
        {
            try
            {
                PreencherInformacoesDeAuditoria(certificadoTemplate);

                if (NomeExiste(certificadoTemplate.Nome))
                {
                    throw new AcademicoException("Já existe um template de certificado com o nome informado.");
                }

                bmCertificadoTemplate.Salvar(certificadoTemplate);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public void AlterarCertificadoTemplate(CertificadoTemplate pCertificadoTemplate)
        {
            try
            {
                PreencherInformacoesDeAuditoria(pCertificadoTemplate);

                bmCertificadoTemplate.Salvar(pCertificadoTemplate);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public CertificadoTemplate ObterCertificadoTemplatePorID(int pId)
        {
            CertificadoTemplate certificadoTemplate = null;

            try
            {
                certificadoTemplate = bmCertificadoTemplate.ObterPorID(pId);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return certificadoTemplate;
        }

        public IList<CertificadoTemplate> ObterPorCategoriaConteudo(CategoriaConteudo categoriaConteudo)
        {
            var categorias = categoriaConteudo.ObterPais().Select(x => x.ID).ToList();

            return bmCertificadoTemplate.ObterPorCategoriaConteudo(categorias).ToList();
        }

        public IList<CertificadoTemplate> ObterTemplateAtivoPorCategoriaConteudo(CategoriaConteudo categoriaConteudo)
        {
            var categorias = categoriaConteudo.ObterPais().Select(x => x.ID).ToList();

            return bmCertificadoTemplate.ObterTemplateAtivoPorCategoriaConteudo(categorias).ToList();
        }

        public IQueryable<CertificadoTemplate> ObterTodosCertificadosSomenteIdNome()
        {
            return bmCertificadoTemplate.ObterTodosCertificadosSomenteIdNome();
        }

        public IEnumerable<CertificadoTemplate> ObterTodosCertificadoTemplate()
        {
            IEnumerable<CertificadoTemplate> listaCertificadoTemplate = null;

            try
            {
                listaCertificadoTemplate = bmCertificadoTemplate.ObterTodos();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaCertificadoTemplate;
        }

        public void ExcluirCertificadoTemplate(int idCertificadoTemplate)
        {
            try
            {
                CertificadoTemplate certificadoTemplate = null;

                if (idCertificadoTemplate <= 0) return;
                certificadoTemplate = bmCertificadoTemplate.ObterPorID(idCertificadoTemplate);
                bmCertificadoTemplate.Excluir(certificadoTemplate);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public IList<CertificadoTemplate> ObterCertificadoTemplatePorFiltro(CertificadoTemplate pCertificadoTemplate)
        {
            IList<CertificadoTemplate> listaCertificadoTemplate = null;

            try
            {
                listaCertificadoTemplate = bmCertificadoTemplate.ObterPorFiltro(pCertificadoTemplate);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaCertificadoTemplate;
        }

        public bool NomeExiste(string nomeCertificado)
        {
            return bmCertificadoTemplate.NomeExiste(nomeCertificado);
        }

        public CertificadoTemplate ObterCertificado(int pIdMatriculaOferta, int pIdUsuarioTrilha)
        {

            CertificadoTemplate certificadoTemplate = null;

            try
            {

                if (pIdMatriculaOferta > 0)
                {

                    MatriculaOferta mo = new BMMatriculaOferta().ObterPorID(pIdMatriculaOferta);

                    if (!(mo != null && mo.Oferta.CertificadoTemplate != null && mo.Oferta.CertificadoTemplate.ID > 0))
                        return certificadoTemplate;

                    if (mo.StatusMatricula == enumStatusMatricula.Concluido)
                    {
                        if (string.IsNullOrEmpty(mo.CDCertificado))
                        {
                            mo.CDCertificado = InfraEstrutura.Core.Helper.WebFormHelper.ObterStringAleatoria();
                            mo.DataGeracaoCertificado = DateTime.Now;
                            new BMMatriculaOferta().Salvar(mo);
                        }

                        certificadoTemplate = new BMCertificadoTemplate().ObterPorID(mo.Oferta.CertificadoTemplate.ID);
                    }
                    else
                        return null;

                }

                else if (pIdUsuarioTrilha > 0)
                {
                    UsuarioTrilha ut = new BMUsuarioTrilha().ObterPorId(pIdUsuarioTrilha);

                    if (!(ut != null && ut.TrilhaNivel.CertificadoTemplate != null && ut.TrilhaNivel.CertificadoTemplate.ID > 0))
                        return certificadoTemplate;

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
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return certificadoTemplate;
        }

        /// <summary>
        /// Verifica se o código do certificado do tutor é autêntico.
        /// </summary>
        /// <param name="codigo">Código do certificado</param>
        /// <returns></returns>
        public DTOValidarCertificadoTutor VerificarCertificadoTutor(string codigo)
        {
            var retorno = new DTOValidarCertificadoTutor();

            try
            {
                var inicioTurma = (codigo.IndexOf("tu", StringComparison.Ordinal)) + 2;

                var inicioProfessor = (codigo.IndexOf("pr", StringComparison.Ordinal)) + 2;

                var idTurma = int.Parse(codigo.Substring(inicioTurma, inicioProfessor - inicioTurma - 2));

                var idProfessor = int.Parse(codigo.Substring(inicioProfessor, codigo.Length - inicioProfessor));

                // Verifica se existe alguma turma vencida da oferta selecionada e que possui o professor selecionado.
                var isValido = new BMTurmaProfessor().ObterTodos()
                    .Any(tp => tp.Turma.ID == idTurma &&
                              tp.Turma.DataFinal.HasValue &&
                              tp.Turma.DataFinal.Value < DateTime.Now &&
                              tp.Professor.ID == idProfessor);

                retorno.Valido = isValido;
                retorno.IdTurma = idTurma;
                retorno.IdProfessor = idProfessor;
            }
            catch
            {
                retorno.Valido = false;
            }

            return retorno;
        }

        #endregion
    }
}
