using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using System.IO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class CadastrarHistoricoExtraCurricular : BusinessProcessServicesBase
    {
        private BMHistoricoExtraCurricular _historicoExtraCurricularBm;

        public void RemoverHistoricoExtraCurricular(DTOHistoricoExtraCurricular pHistoricoExtraCurricular, AuthenticationRequest autenticacao)
        {
            try
            {
                if (pHistoricoExtraCurricular.ID == 0) throw new AcademicoException("Atividade Extra Curricular Inválida.");
                var bmHistoricoExtraCurricular = new BMHistoricoExtraCurricular();
                var historico = bmHistoricoExtraCurricular.ObterPorID(pHistoricoExtraCurricular.ID);
                if (historico != null) bmHistoricoExtraCurricular.Excluir(historico);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public void InserirHistoricoExtraCurricular(DTOHistoricoExtraCurricular pHistoricoExtraCurricular, AuthenticationRequest autenticacao)
        {
            try
            {
                // Caso seja cadastro e o arquivo não esteja informado, retorna o erro de validação.
                if ((pHistoricoExtraCurricular.ID == 0 &&
                     string.IsNullOrEmpty(pHistoricoExtraCurricular.CaminhoArquivoParticipacao)))
                    throw new AcademicoException("O arquivo anexo é obrigatório");

                /* 
                 * TODO: Atualmente o usuário do IIS não está permitindo a verficação do arquivo no file server, 
                 * investigar o porque do file exists não estar funcionando mesmo com o arquivo estando correto no servidor.

                Verifica inclusão do arquivo. 
                string caminho = string.Concat(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro, @"\", pHistoricoExtraCurricular.CaminhoArquivoParticipacao);

                if (pHistoricoExtraCurricular.ID == 0 && !File.Exists(caminho))
                {
                    throw new AcademicoException(caminho);
                }
                */

                var historico = pHistoricoExtraCurricular.ID > 0
                    ? new BMHistoricoExtraCurricular().ObterPorID(pHistoricoExtraCurricular.ID)
                    : new HistoricoExtraCurricular();

                historico.TextoAtividade = pHistoricoExtraCurricular.TextoAtividade;
                historico.Usuario = new BMUsuario().ObterPorId(pHistoricoExtraCurricular.IdUsuario);
                historico.CargaHoraria = pHistoricoExtraCurricular.CargaHoraria;
                historico.DataFimAtividade = Convert.ToDateTime(pHistoricoExtraCurricular.DataFimAtividade);
                historico.DataInicioAtividade = Convert.ToDateTime(pHistoricoExtraCurricular.DataInicioAtividade);
                historico.SolucaoEducacionalExtraCurricular = pHistoricoExtraCurricular.NomeSolucaoExtraCurricular;
                historico.Auditoria = new Auditoria(autenticacao.Login);
                historico.Instituicao = pHistoricoExtraCurricular.NomeInstituicao;
                if (pHistoricoExtraCurricular.IdFornecedor.HasValue && pHistoricoExtraCurricular.IdFornecedor.Value != 0)
                {
                    var manterFornecedor = new ManterFornecedor();
                    var fornecedor = manterFornecedor.ObterFornecedorPorID(pHistoricoExtraCurricular.IdFornecedor.Value);
                    historico.Fornecedor = fornecedor;
                }
                else
                {
                    historico.Fornecedor = null;
                }
                if (!string.IsNullOrEmpty(pHistoricoExtraCurricular.CaminhoArquivoParticipacao))
                {
                    var fs = new FileServer
                    {
                        NomeDoArquivoNoServidor = pHistoricoExtraCurricular.CaminhoArquivoParticipacao,
                        Auditoria = new Auditoria(autenticacao.Login),
                        TipoArquivo = pHistoricoExtraCurricular.TipoArquivoComprovacao,
                        NomeDoArquivoOriginal = pHistoricoExtraCurricular.NomeArquivoComprovacao,
                        MediaServer = true
                    };

                    new BMFileServer().Salvar(fs);
                    historico.FileServer = fs;
                }

                new BMHistoricoExtraCurricular().Salvar(historico);
            }
            catch (Exception ex)
            {
                // Caso a exceção seja violação da chave que não permite atividades com mesmo nome,
                // do mesmo usuário e com a mesma data de início, retorna mensagem de erro amigável.
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2627)
                {
                    throw new AcademicoException(
                        "Você já possui uma atividade extracurricular com este mesmo nome e com a mesma data de início. Altere os dados abaixo ou vá na listagem do seu histórico acadêmico e altere a atividade existente.");
                }

                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public IList<DTOFormaAquisicao> ObterListaFormaAquisicao()
        {
            IList<DTOFormaAquisicao> lstResult = null;

            try
            {
                lstResult = new List<DTOFormaAquisicao>();

                foreach (FormaAquisicao fa in new BMFormaAquisicao().ObterTodos())
                {
                    DTOFormaAquisicao faDTO = new DTOFormaAquisicao();
                    CommonHelper.SincronizarDominioParaDTO(fa, faDTO);
                    lstResult.Add(faDTO);
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return lstResult;
        }
    }
}
