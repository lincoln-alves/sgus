using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.ExternosWebService
{
    public class ManterOferta : BusinessProcessServicesBase
    {
        private BMOferta ofertaBM;

        public void ManterOfertaFornecedor(DTOManterOferta dtoOferta, AuthenticationProviderRequest pAutenticacao)
        {
            Fornecedor fornecedor = new BMFornecedor().ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login }).FirstOrDefault();
            try
            {
                this.RegistrarLogExecucaoFornecedor(fornecedor, "ManterOferta");

                using (ofertaBM = new BMOferta())
                {
                    ValidarCamposObrigatoriosOferta(dtoOferta);

                    SolucaoEducacional solucaoEducacional = new BMSolucaoEducacional().ObterSolucaoEducacionalPorFornecedor(pAutenticacao.Login, dtoOferta.IDChaveExternaSolucaoEducacional);
                    if (solucaoEducacional == null)
                    {
                        throw new AcademicoException("Não foi possível encontrar a Solução Educacional");
                    }
                    Oferta oferta = ofertaBM.ObterOfertaPorFornecedor(null, fornecedor.Login, dtoOferta.IDChaveExternaOferta, dtoOferta.IDChaveExternaSolucaoEducacional);

                    if (oferta == null)
                    {
                        oferta = new Oferta();
                        oferta.IDChaveExterna = dtoOferta.IDChaveExternaOferta;
                        oferta.SolucaoEducacional = solucaoEducacional;
                        try
                        {
                            oferta.TipoOferta = (enumTipoOferta)Enum.Parse(typeof(enumTipoOferta), dtoOferta.TipoOferta.ToString());
                        }
                        catch
                        {
                            throw new AcademicoException("Não foi possível encontrar o Tipo da Oferta");
                        }
                        oferta.ListaMatriculaOferta = new List<MatriculaOferta>();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dtoOferta.IDChaveExternaOfertaNova))
                        {
                            oferta.IDChaveExterna = dtoOferta.IDChaveExternaOfertaNova;
                        }
                    }
                    
                    oferta.DataFimInscricoes = dtoOferta.DataFimInscricoes;
                    oferta.DataInicioInscricoes = dtoOferta.DataInicioInscricoes;
                    oferta.FiladeEspera = dtoOferta.FiladeEspera;
                    oferta.InscricaoOnline = dtoOferta.InscricaoOnline;
                    oferta.Nome = dtoOferta.NomedaOferta;

                    if (dtoOferta.IdCertificado > 0)
                    {
                        oferta.CertificadoTemplate = new BMCertificadoTemplate().ObterPorID(dtoOferta.IdCertificado);
                    }

                    int codigoMoodle = 0;

                    if (int.TryParse(dtoOferta.CodigoMoodle, out codigoMoodle))
                        oferta.CodigoMoodle = codigoMoodle;

                    if (dtoOferta.QTDiasPrazo.HasValue)
                        oferta.DiasPrazo = dtoOferta.QTDiasPrazo.Value;
                    else
                        oferta.DiasPrazo = 30;

                    oferta.QuantidadeMaximaInscricoes = dtoOferta.QuantidadeMaximaInscricoes;
                    oferta.Auditoria = new Auditoria(pAutenticacao.Login);
                    oferta.CargaHoraria = dtoOferta.QTCargaHoraria;

                    if (dtoOferta.ListaMatriculaAlunoOferta != null && dtoOferta.ListaMatriculaAlunoOferta.Count > 0)
                    {
                        var manterItemTrilhaParticipacao = new BP.ManterItemTrilhaParticipacao(); 

                        foreach (var dtoMatriculaSE in dtoOferta.ListaMatriculaAlunoOferta)
                        {
                            if (string.IsNullOrWhiteSpace(dtoMatriculaSE.Cpf))
                            {
                                throw new AcademicoException("O CPF do aluno é obrigatório");
                            }
                            if (string.IsNullOrWhiteSpace(dtoMatriculaSE.StatusMatricula))
                            {
                                throw new AcademicoException("O status da matrícula do aluno é obrigatório");
                            }

                            enumStatusMatricula statusMatricula;
                            try
                            {
                                statusMatricula = (enumStatusMatricula)Enum.Parse(typeof(enumStatusMatricula), dtoMatriculaSE.StatusMatricula);
                            }
                            catch
                            {
                                throw new AcademicoException("Status da matrícula inválido");
                            }

                            if (statusMatricula == enumStatusMatricula.CanceladoAdm || statusMatricula == enumStatusMatricula.CanceladoAluno)
                                continue;

                            MatriculaOferta matriculaOferta = null;
                            Usuario usuario = null;

                            if (dtoMatriculaSE.IdMatriculaOferta > 0)
                            {
                                matriculaOferta = oferta.ListaMatriculaOferta.Where(x => x.ID == dtoMatriculaSE.IdMatriculaOferta).FirstOrDefault();
                                usuario = matriculaOferta.Usuario;
                            }
                            else
                            {
                                matriculaOferta = new MatriculaOferta();
                                usuario = (new BMUsuario()).ObterPorCPF(dtoMatriculaSE.Cpf);
                                if (usuario == null)
                                {
                                    throw new AcademicoException("Não foi possível encontrar o aluno");
                                }
                                matriculaOferta.Oferta = oferta;
                                matriculaOferta.Usuario = usuario;
                                matriculaOferta.UF = usuario.UF;
                                matriculaOferta.NivelOcupacional = usuario.NivelOcupacional;
                            }

                            if (matriculaOferta.ID == 0)
                            {
                                //Criar Matricula Oferta
                                matriculaOferta.StatusMatricula = statusMatricula;
                                matriculaOferta.Auditoria = new Auditoria(pAutenticacao.Login);
                                matriculaOferta.DataSolicitacao = dtoMatriculaSE.DataSolicitacao;
                                matriculaOferta.DataStatusMatricula = dtoMatriculaSE.DataStatusMatricula;
                                matriculaOferta.LinkAcesso = dtoMatriculaSE.LinkAcesso;
                                matriculaOferta.LinkCertificado = dtoMatriculaSE.LinkCertificado;

                                if (!string.IsNullOrWhiteSpace(dtoMatriculaSE.IDChaveExternaTurma))
                                {
                                    //Criar matricula Turma
                                    Turma turma = new BMTurma().ObterTurmaPorFornecedor(fornecedor.Login, dtoMatriculaSE.IDChaveExternaTurma);
                                    if (turma != null)
                                    {
                                        MatriculaTurma matriculaTurma = new MatriculaTurma()
                                        {
                                            Turma = turma,
                                            MatriculaOferta = matriculaOferta,
                                            DataMatricula = dtoMatriculaSE.DataMatriculaTurma.Value,
                                            DataLimite = dtoMatriculaSE.DataLimite.Value,
                                            Nota1 = dtoMatriculaSE.Nota1,
                                            Nota2 = dtoMatriculaSE.Nota2,
                                            MediaFinal = dtoMatriculaSE.MediaFinal,
                                            ValorNotaProvaOnline = dtoMatriculaSE.NotaOnline,
                                            Auditoria = new Auditoria(pAutenticacao.Login),
                                        };
                                        if (matriculaOferta.MatriculaTurma == null)
                                            matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();

                                        matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                                    }
                                }
                            }
                            else
                            {
                                //Alterar Dados da Matricula Oferta
                                bool terminou = false;
                                if (matriculaOferta.StatusMatricula != statusMatricula && (statusMatricula == enumStatusMatricula.Aprovado || statusMatricula == enumStatusMatricula.Concluido))
                                {
                                    terminou = true;
                                }
                                matriculaOferta.StatusMatricula = statusMatricula;
                                matriculaOferta.DataStatusMatricula = dtoMatriculaSE.DataStatusMatricula;
                                matriculaOferta.LinkAcesso = dtoMatriculaSE.LinkAcesso;
                                matriculaOferta.LinkCertificado = dtoMatriculaSE.LinkCertificado;
                                matriculaOferta.Auditoria = new Auditoria(pAutenticacao.Login);
                                //matriculaOferta.DataSolicitacao = dtoMatriculaSE.DataSolicitacao;

                                if (!string.IsNullOrWhiteSpace(dtoMatriculaSE.IDChaveExternaTurma))
                                {
                                    if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                                    {
                                        matriculaOferta.MatriculaTurma[0].DataMatricula = dtoMatriculaSE.DataMatriculaTurma.Value;
                                        matriculaOferta.MatriculaTurma[0].DataLimite = dtoMatriculaSE.DataLimite.Value;
                                        matriculaOferta.MatriculaTurma[0].Nota1 = dtoMatriculaSE.Nota1;
                                        matriculaOferta.MatriculaTurma[0].Nota2 = dtoMatriculaSE.Nota2;
                                        matriculaOferta.MatriculaTurma[0].MediaFinal = dtoMatriculaSE.MediaFinal;
                                        matriculaOferta.MatriculaTurma[0].ValorNotaProvaOnline = dtoMatriculaSE.NotaOnline;

                                        if (terminou)
                                        {
                                            matriculaOferta.MatriculaTurma[0].DataTermino = DateTime.Now;
                                            manterItemTrilhaParticipacao.AtualizarStatusParticipacoesTrilhas(matriculaOferta);
                                        }

                                        matriculaOferta.MatriculaTurma[0].Auditoria = new Auditoria(pAutenticacao.Login);
                                    }
                                    else
                                    {
                                        Turma turma = new BMTurma().ObterTurmaPorFornecedor(fornecedor.Login, dtoMatriculaSE.IDChaveExternaTurma);
                                        if (turma != null)
                                        {
                                            MatriculaTurma matriculaTurma = new MatriculaTurma()
                                            {
                                                Turma = turma,
                                                MatriculaOferta = matriculaOferta,
                                                DataMatricula = dtoMatriculaSE.DataMatriculaTurma.Value,
                                                DataLimite = dtoMatriculaSE.DataLimite.Value,
                                                Nota1 = dtoMatriculaSE.Nota1,
                                                Nota2 = dtoMatriculaSE.Nota2,
                                                MediaFinal = dtoMatriculaSE.MediaFinal,
                                                ValorNotaProvaOnline = dtoMatriculaSE.NotaOnline,
                                                Auditoria = new Auditoria(pAutenticacao.Login)
                                            };

                                            if (matriculaOferta.MatriculaTurma == null)
                                                matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();

                                            matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                                        }
                                    }
                                }

                            }
                            if (matriculaOferta.ID == 0)
                                oferta.ListaMatriculaOferta.Add(matriculaOferta);

                        }
                    }

                    if(oferta.ID != 0)
                        ofertaBM.EvictOferta(oferta);

                    ofertaBM.Salvar(oferta);

                }
            }
            catch (AcademicoException ex)
            {
                throw new AcademicoException(ex.Message);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private static void ValidarCamposObrigatoriosOferta(DTOManterOferta pOferta)
        {
            if (string.IsNullOrWhiteSpace(pOferta.IDChaveExternaOferta) && string.IsNullOrWhiteSpace(pOferta.IDChaveExternaOfertaNova))
                throw new AcademicoException("A chave externa da oferta é obrigatória");
            if (string.IsNullOrWhiteSpace(pOferta.IDChaveExternaSolucaoEducacional))
                throw new AcademicoException("A solução educacional é obrigatória");
            if (string.IsNullOrWhiteSpace(pOferta.TipoOferta))
                throw new AcademicoException("O tipo da oferta é obrigatório");
            if (string.IsNullOrWhiteSpace(pOferta.NomedaOferta))
                throw new AcademicoException("O nome da oferta é obrigatório");
            if (!(pOferta.QTCargaHoraria > 0))
                throw new AcademicoException("A carga horária é obrigatória");
            if (!(pOferta.QuantidadeMaximaInscricoes > 0))
                throw new AcademicoException("A quantidade máxima de inscrições é obrigatória");

        }

        public bool ManterMatriculaTurma(int idMatriculaOferta, int idTurma, double mediaFinal, AuthenticationProviderRequest autenticacao)
        {
            try
            {
                RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = autenticacao.Login }).FirstOrDefault(), "ManterMatriculaTurma");

                var bmMatriculaTurma = new BMMatriculaTurma();
                var bmMatriculaOferta = new BMMatriculaOferta();
                var bmTurma = new BMTurma();

                MatriculaTurma matriculaTurma = new MatriculaTurma();
                matriculaTurma.MatriculaOferta = new MatriculaOferta();
                matriculaTurma.Turma = new Turma();
                matriculaTurma.MatriculaOferta.ID = idMatriculaOferta;
                matriculaTurma.Turma.ID = idTurma;

                matriculaTurma = bmMatriculaTurma.ObterPorFiltro(matriculaTurma).FirstOrDefault();

                if (matriculaTurma != null)
                {
                    MatriculaOferta matriculaOferta = bmMatriculaOferta.ObterPorID(idMatriculaOferta);
                    Turma turma = bmTurma.ObterPorID(idTurma);
                    
                    matriculaTurma.MediaFinal = matriculaOferta.StatusMatricula == enumStatusMatricula.Abandono
                        ? null
                        : (double?)mediaFinal;

                    if (matriculaTurma.MediaFinal == 0)
                    {
                        matriculaTurma.MediaFinal = null;
                    }

                    if (!matriculaTurma.DataTermino.HasValue)
                        matriculaTurma.DataTermino = DateTime.Now;

                    decimal notaMinima = turma.NotaMinima.HasValue ? turma.NotaMinima.Value : 7;
                    decimal notaFinal = matriculaTurma.MediaFinal.HasValue ? Convert.ToDecimal(matriculaTurma.MediaFinal.Value) : decimal.Zero;

                    if (notaFinal >= notaMinima)
                    {
                        matriculaOferta.StatusMatricula = enumStatusMatricula.Aprovado;
                    }
                    else
                    {
                        // Se já tiver terminada a turma vamos reprovar o aluno e ele não obteve nota para ser aprovado vamos reprovar ele
                        if (matriculaTurma.DataLimite < DateTime.Now)
                        {
                            // Se não obteve nenhuma nota é abandono
                            if (notaFinal == 0)
                            {
                                matriculaOferta.StatusMatricula = enumStatusMatricula.Abandono;
                            }
                            // Do contrário é deve ser reprovado
                            else
                            {
                                matriculaOferta.StatusMatricula = enumStatusMatricula.Reprovado;
                            }
                        }
                    }

                    matriculaTurma.Auditoria.UsuarioAuditoria = matriculaOferta.Auditoria.UsuarioAuditoria = autenticacao.Login;
                    matriculaOferta.Auditoria.DataAuditoria = matriculaTurma.Auditoria.DataAuditoria = DateTime.Now;

                    bmMatriculaTurma.Salvar(matriculaTurma);
                    bmMatriculaOferta.Salvar(matriculaOferta);

                    //CASO A OFERTA FOR CONTINUA, VERIFICA E ATUALIZA O ITEM TRILHA DO USUARIO
                    new BP.ManterItemTrilhaParticipacao().vinculaMatriculaOferta(matriculaOferta);

                    new BP.ManterItemTrilhaParticipacao().AtualizarStatusParticipacoesTrilhas(matriculaOferta);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public DTOManterOferta ConsultaOferta(string idsOfertas, string idChaveExternaOferta, string idChaveExternaSolucaoEducacional, AuthenticationProviderRequest pAutenticacao)
        {
            DTOManterOferta ofResult = null;
            try
            {
                this.RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login }).FirstOrDefault(), "ConsultaOferta");
                using (ofertaBM = new BMOferta())
                {
                    IList<int> lsIdsOfertas = null;
                    if (!string.IsNullOrEmpty(idsOfertas))
                    {
                        lsIdsOfertas = idsOfertas.Split(',').Select(Int32.Parse).ToList();
                    }
                    var oferta = ofertaBM.ObterOfertaPorFornecedor(lsIdsOfertas, pAutenticacao.Login, idChaveExternaOferta, idChaveExternaSolucaoEducacional);
                    if (oferta == null) throw new AcademicoException("Oferta não encontrada");
                    var existeProximaOferta = false;
                    // #1855 - Caso o fornecedor seja Moodle e caso a oferta esteja vencida,
                    // remover o ID da Chave Externa e continuar buscando a oferta atual.
                    if (oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                    {
                        if (lsIdsOfertas == null) lsIdsOfertas = new List<int>();
                        lsIdsOfertas.Add(oferta.ID);

                        if (oferta.ListaMatriculaOferta.Any(p => p.StatusMatricula != enumStatusMatricula.Inscrito))
                        {
                            oferta.IDChaveExterna = null;
                            ofertaBM.Salvar(oferta);
                        }
                        
                        var proximaOferta = ofertaBM.ObterOfertaPorFornecedor(lsIdsOfertas, pAutenticacao.Login,
                            idChaveExternaOferta, idChaveExternaSolucaoEducacional);

                        existeProximaOferta = proximaOferta != null && proximaOferta.ID != oferta.ID;
                    }

                    ofResult = new DTOManterOferta
                    {
                        IDChaveExternaOferta = oferta.IDChaveExterna,
                        DataFimInscricoes = oferta.DataFimInscricoes,
                        DataInicioInscricoes = oferta.DataInicioInscricoes,
                        FiladeEspera = oferta.FiladeEspera,
                        TipoOferta = oferta.TipoOferta.ToString(),
                        IDChaveExternaSolucaoEducacional = oferta.SolucaoEducacional.IDChaveExterna,
                        ListaMatriculaAlunoOferta = new List<DTOMatriculaSE>(),
                        ListaTurmasOferta = new List<DTOManterTurma>(),
                        NomedaOferta = oferta.Nome,
                        QuantidadeMaximaInscricoes = oferta.QuantidadeMaximaInscricoes,
                        QTCargaHoraria = oferta.CargaHoraria,
                        InscricaoOnline = oferta.InscricaoOnline,
                        QTDiasPrazo = oferta.DiasPrazo,
                        CodigoMoodle = oferta.CodigoMoodle.ToString(),
                        ProximaOferta = existeProximaOferta,
                        IDOferta = oferta.ID
                    };

                    foreach (var mo in oferta.ListaMatriculaOferta)
                    {
                        var dtoMatriculaSE = new DTOMatriculaSE
                        {
                            IdMatriculaOferta = mo.ID,
                            Nome = mo.Usuario.Nome,
                            NivelOcupacional = mo.Usuario.NivelOcupacional.Nome,
                            UF = mo.Usuario.UF.Nome,
                            Cpf = mo.Usuario.CPF,
                            DataNascimento = mo.Usuario.DataNascimento,
                            Email = mo.Usuario.Email,
                            Telefone = mo.Usuario.TelefoneExibicao,
                            Endereco = mo.Usuario.Endereco,
                            Cidade = mo.Usuario.Cidade,
                            Estado = mo.Usuario.Estado,
                            Cep = mo.Usuario.Cep,
                            StatusMatricula = mo.StatusMatricula.ToString(),
                            DataSolicitacao = mo.DataSolicitacao,
                            DataStatusMatricula = mo.DataStatusMatricula,
                            LinkAcesso = mo.LinkAcesso,
                            LinkCertificado = mo.LinkCertificado
                        };

                        if (mo.MatriculaTurma != null && mo.MatriculaTurma.Count > 0)
                        {
                            var matriculaTurma = mo.MatriculaTurma.First();
                            dtoMatriculaSE.IDTurma = matriculaTurma.Turma.ID;
                            dtoMatriculaSE.IDChaveExternaTurma = matriculaTurma.Turma.IDChaveExterna;
                            dtoMatriculaSE.NomeTurma = matriculaTurma.Turma.Nome;
                            dtoMatriculaSE.DataMatriculaTurma = matriculaTurma.DataMatricula;
                            dtoMatriculaSE.DataLimite = matriculaTurma.DataLimite;
                            dtoMatriculaSE.Nota1 = matriculaTurma.Nota1;
                            dtoMatriculaSE.Nota2 = matriculaTurma.Nota2;
                            dtoMatriculaSE.MediaFinal = matriculaTurma.MediaFinal;
                            dtoMatriculaSE.NotaOnline = matriculaTurma.ValorNotaProvaOnline;
                            dtoMatriculaSE.DataSolicitacao = mo.DataSolicitacao;
                            dtoMatriculaSE.ValorNotaMinima = matriculaTurma.Turma.NotaMinima;
                            dtoMatriculaSE.AcessoAposConclusao = matriculaTurma.Turma.AcessoAposConclusao ? 1 : 0;
                        }
                        ofResult.ListaMatriculaAlunoOferta.Add(dtoMatriculaSE);
                    }
                    //Listando as turmas vinculadas à Oferta.
                    using(var bmTurma = new BMTurma())
                    {
                        ofResult.ListaTurmasOferta = new BMTurma()
                          .ObterTodos()
                          .Where(turma => turma.Oferta.ID == oferta.ID)
                          .ToList()
                          .Select(MapearListaParaDtoManterTurma())
                          .ToList();
                    }

                }
            }
            catch (AcademicoException ex)
            {
                throw new AcademicoException(ex.Message);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return ofResult;
        }

        private static Func<Turma, DTOManterTurma> MapearListaParaDtoManterTurma()
        {
            return turma => new DTOManterTurma
            {
                DataFinal = turma.DataFinal,
                DataInicio = turma.DataInicio,
                IDChaveExternaTurma = turma.IDChaveExterna,
                Local = turma.Local,
                NomedaTurma = turma.Nome,
                TipoTutoria = turma.TipoTutoria,
                QuantideInscritos = CountInscritosPeloIdTurma(turma),
                ListaTurmaProfessor = ObterListaDeProfessores(turma)

            };
        }

        private static List<DTOManterTurmaProfessor> ObterListaDeProfessores(Turma turma)
        {
            using(var bmTurma = new BMTurmaProfessor())
            {
                return bmTurma
                   .ObterPorFiltro(new TurmaProfessor { Turma = turma })
                   .Select(tp => new DTOManterTurmaProfessor
                   {
                       IDTurmaProfessor = tp.ID,
                       NomeProfessor = tp.Professor.Nome,
                       EmailProfessor = tp.Professor.Email,
                       CPFProfessor = tp.Professor.CPF
                   })
                   .ToList();
            }
        }

        private static int CountInscritosPeloIdTurma(Turma turma)
        {
            return new BMMatriculaTurma()
                .ObterTodosIQueryable()
                .Count(x =>  x.Turma.ID == turma.ID);
        }

        public DTOConsultarOfertasPorPeriodo ConsultarOfertasPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var dados = new DTOConsultarOfertasPorPeriodo();
            var ofertas = new BMOferta().ConsultarOfertasPorPeriodo(dataInicio, dataFim);

            foreach (var item in ofertas)
            {
                var dtoOferta = new DTODadosOferta
                {
                    ID = item.ID,
                    Nome = item.Nome
                };

                var turma = item.ListaTurma.OrderByDescending(x => x.AcessoWifi).FirstOrDefault();

                switch (turma.AcessoWifi.Value)
                {
                    case 1:
                        dtoOferta.AcessoWifi = "Asa Norte";
                        break;
                    case 2:
                        dtoOferta.AcessoWifi = "Asa Sul";
                        break;
                    case 3:
                        dtoOferta.AcessoWifi = "Asa Norte e Asa Sul";
                        break;
                }

                if (!string.IsNullOrEmpty(item.EmailResponsavel))
                {
                    dtoOferta.Responsavel.Email = item.EmailResponsavel;

                    var buscarResponsavelPorEmail = new BMUsuario().ObterPorEmail(item.EmailResponsavel);

                    if (buscarResponsavelPorEmail != null)
                    {
                        dtoOferta.Responsavel.CPF = buscarResponsavelPorEmail.CPF;
                        dtoOferta.Responsavel.Email = buscarResponsavelPorEmail.Email;
                        dtoOferta.Responsavel.Nome = buscarResponsavelPorEmail.Nome;
                        dtoOferta.Responsavel.UF = buscarResponsavelPorEmail.UF.Sigla;
                    }
                }

                dtoOferta.ListaDadosMatriculados.AddRange(
                    item.ListaMatriculaOferta.Select(
                        x =>
                            new DTODadosMatriculados
                            {
                                CPF = x.Usuario.CPF,
                                DataNascimento = x.Usuario.DataNascimento,
                                Email = x.Usuario.Email,
                                Matricula = x.Usuario.Matricula,
                                Nome = x.Usuario.Nome,
                                UF = x.Usuario.UF.Sigla
                            }));

                dados.ListaDadosOferta.Add(dtoOferta);

            }

            return dados;
        }
    }
}
