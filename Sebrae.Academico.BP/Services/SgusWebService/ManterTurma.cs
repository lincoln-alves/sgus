using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.ExternosWebService
{
    public class ManterTurma : BusinessProcessServicesBase
    {


        public void ManterCadastroTurma(DTOManterTurma pTurma, AuthenticationProviderRequest pAutenticacao)
        {

            try
            {
                ValidarCamposObrigatoriosTurma(pTurma);

                this.RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login }).FirstOrDefault(),
                                               "CadastrarTurma");


                using (BMTurma turmaBM = new BMTurma())
                {
                    Turma turma = turmaBM.ObterTurmaPorFornecedoreOferta(pAutenticacao.Login, pTurma.IDChaveExternaTurma, pTurma.IDChaveExternaOferta);

                    if (turma == null)
                    {
                        turma = new Turma();
                        turma.IDChaveExterna = pTurma.IDChaveExternaTurma;
                        turma.Oferta = (new BMOferta()).ObterOfertaPorFornecedor(pAutenticacao.Login, pTurma.IDChaveExternaOferta);
                        if (turma.Oferta == null)
                            throw new AcademicoException("Oferta não encontrada na base de dados");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(pTurma.IDChaveExternaTurmaNova))
                        {
                            turma.IDChaveExterna = pTurma.IDChaveExternaTurmaNova;
                        }
                    }

                    turma.Local = pTurma.Local;
                    turma.Nome = pTurma.NomedaTurma;
                    turma.TipoTutoria = pTurma.TipoTutoria;
                    turma.DataInicio = pTurma.DataInicio;
                    turma.DataFinal = pTurma.DataFinal;
                    turma.InAberta = true;
                    
                    if (!string.IsNullOrEmpty(pTurma.NomeProfessor))
                        turma.Professor = (new BMProfessor()).ObterPorFiltros(new Professor() { Nome = pTurma.NomeProfessor }).FirstOrDefault();
                    else
                        turma.Professor = null;

                    if (pTurma.IDQuestionarioPre > 0)
                    {
                        AdicionarQuestionarioATurma(turma, pTurma.IDQuestionarioPre, enumTipoQuestionarioAssociacao.Pre);
                    }

                    if (pTurma.IDQuestionarioPos > 0)
                    {
                        AdicionarQuestionarioATurma(turma, pTurma.IDQuestionarioPos, enumTipoQuestionarioAssociacao.Pos);
                    }

                    if (pTurma.IDQuestionarioProva > 0)
                    {
                        AdicionarQuestionarioATurma(turma, pTurma.IDQuestionarioProva, enumTipoQuestionarioAssociacao.Prova);
                    }
                    
                    turma.Auditoria = new Auditoria(pAutenticacao.Login);

                    turmaBM.Salvar(turma);
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

        }

        private void AdicionarQuestionarioATurma(Turma turma, int idQuestionario, enumTipoQuestionarioAssociacao tipoQuestionario)
        {
            if (idQuestionario > 0)
                this.TratarQuestionario(turma, idQuestionario, false, tipoQuestionario);
            else
                TratarRemocao(turma, false, tipoQuestionario);
        }

        private void TratarQuestionario(Turma turma, int idQuestionario, bool evolutivo, enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao)
        {
            Questionario questionario = new ManterQuestionario().ObterQuestionarioPorID(idQuestionario);
            QuestionarioAssociacao questionarioAssociacaoEditar = turma.ListaQuestionarioAssociacao.FirstOrDefault(x => x.Turma != null && x.Turma.ID == turma.ID && x.Evolutivo == evolutivo && x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao);

            if (questionarioAssociacaoEditar == null)
            {
                QuestionarioAssociacao questionarioAssociacaoAdicionar = new QuestionarioAssociacao();
                questionarioAssociacaoAdicionar.TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID((int)tipoQuestionarioAssociacao);
                questionarioAssociacaoAdicionar.Evolutivo = false;
                questionarioAssociacaoAdicionar.Turma = turma;
                questionarioAssociacaoAdicionar.Questionario = questionario;
                questionarioAssociacaoAdicionar.Obrigatorio = true;
                turma.ListaQuestionarioAssociacao.Add(questionarioAssociacaoAdicionar);
            }
            else
            {
                questionarioAssociacaoEditar.Questionario = questionario;

            }
        }

        private void TratarRemocao(Turma turma, bool evolutivo, enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao)
        {
            QuestionarioAssociacao questionarioAssociacaoRemover = turma.ListaQuestionarioAssociacao.Where(x => x.Turma != null && x.Turma.ID == turma.ID && x.Evolutivo == evolutivo && x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao).FirstOrDefault();
            if (questionarioAssociacaoRemover != null)
                turma.ListaQuestionarioAssociacao.Remove(questionarioAssociacaoRemover);
        }


        public DTOManterTurma CounsultaTurma(string idChaveExternaTurma, string idChaveExternaOferta, AuthenticationProviderRequest pAutenticacao)
        {
            DTOManterTurma result = null;

            try
            {
                this.RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login }).FirstOrDefault(),
                                                "CounsultaTurma");

                using (BMTurma turmaBM = new BMTurma())
                {

                    Turma turma = turmaBM.ObterTurmaPorFornecedoreOferta(pAutenticacao.Login, idChaveExternaTurma, idChaveExternaOferta);

                    if (turma == null)
                        throw new AcademicoException("Turma não encontrada");

                    result = new DTOManterTurma()
                    {
                        DataFinal = turma.DataFinal,
                        DataInicio = turma.DataInicio,
                        IDChaveExternaTurma = turma.IDChaveExterna,
                        Local = turma.Local,
                        NomedaTurma = turma.Nome,
                        TipoTutoria = turma.TipoTutoria,
                        NomeProfessor = turma.Professor != null ? turma.Professor.Nome : string.Empty,
                        CPFProfessor = turma.Professor != null ? turma.Professor.Cpf : string.Empty,
                        EmailProfessor = turma.Professor != null ? turma.Professor.Email : string.Empty,
                        QuantideInscritos = turma.ListaMatriculas.Count,
                        IDChaveExternaOferta = turma.Oferta.IDChaveExterna
                    };
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return result;
        }
        private static void ValidarCamposObrigatoriosTurma(DTOManterTurma manterTurma)
        {
            if (string.IsNullOrWhiteSpace(manterTurma.IDChaveExternaTurma) && string.IsNullOrWhiteSpace(manterTurma.IDChaveExternaTurmaNova))
                throw new AcademicoException("A chave externa da turma é obrigatória");
            if (string.IsNullOrWhiteSpace(manterTurma.NomedaTurma))
                throw new AcademicoException("O nome da turma é obrigatório");
            if (string.IsNullOrWhiteSpace(manterTurma.TipoTutoria))
                throw new AcademicoException("O tipo da tutoria é obrigatório");
        }
    }

}
