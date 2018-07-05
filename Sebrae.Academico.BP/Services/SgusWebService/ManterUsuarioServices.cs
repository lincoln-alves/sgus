using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.DTO.Filtros;
using System.Configuration;
using Sebrae.Academico.BP.solTokenService;
using System.Text.RegularExpressions;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterUsuarioServices : BusinessProcessServicesBase
    {
        private BMUsuario usuarioBM = new BMUsuario();

        public DTOManterUsuario ConsultarPorId(int idUsurio)
        {
            Usuario usuario = new BMUsuario().ObterPorId(idUsurio);
            DTOManterUsuario Result = ObterDTOUsuario(usuario);
            return Result;
        }

        public void ManterUsuario(DTOManterUsuario us, AuthenticationRequest autenticacao)
        {
            bool novoUsuario = false;

            if (us == null)
            {
                return;
            }

            Usuario usuario = new BMUsuario().ObterPorId(us.ID);

            if (usuario != null)
            {
                this.AtualizarInformacoesDoUsuario(us, usuario);
            }
            else
            {
                //usuario = this.ObterObjetoUsuario(us);
                novoUsuario = true;
            };

            if (string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = CriptografiaHelper.Criptografar(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaPadrao).Registro);
            }

            usuario.Auditoria = new Auditoria(autenticacao.Login);

            usuarioBM.Salvar(usuario);

            if (novoUsuario)
                new ManterUsuario().EnviarEmailBoasVindas(usuario);

        }

        public void ManterUsuarioPortal(DTOUsuarioPortal us, AuthenticationRequest autenticacao)
        {
            bool novoUsuario = false;

            if (us == null)
            {
                return;
            }

            Usuario usuario = new BMUsuario().ObterPorId(us.ID);

            if (usuario != null)
            {
                this.AtualizarInformacoesDoUsuarioPortal(us, ref usuario);
            }
            else
            {
                //usuario = this.ObterObjetoUsuario(us);
                novoUsuario = true;
            };

            if (string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = CriptografiaHelper.Criptografar(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaPadrao).Registro);
            }

            usuario.Auditoria = new Auditoria(autenticacao.Login);

            usuarioBM.Salvar(usuario);

            if (novoUsuario)
                new ManterUsuario().EnviarEmailBoasVindas(usuario);

        }

        private Usuario ObterObjetoUsuario(DTOManterUsuario us)
        {
            return new Usuario()
            {

                AnoConclusao = us.AnoConclusao,
                Bairro = us.Bairro,
                //Bairro2 = us.Bairro2,
                Cep = us.Cep,
                //Cep2 = us.Cep2,
                Cidade = us.Cidade,
                //Cidade2 = us.Cidade2,
                CodigoCampoConhecimento = us.CodigoCampoConhecimento,
                Complemento = us.Complemento,
                //Complemento2 = us.Complemento2,
                CPF = us.CPF,
                DataAdmissao = us.DataAdmissao,
                DataExpedicaoIdentidade = us.DataExpedicaoIdentidade,
                DataNascimento = us.DataNascimento,
                Email = us.Email,
                Endereco = us.Endereco,
                //Endereco2 = us.Endereco2,
                Escolaridade = us.Escolaridade,
                Estado = us.Estado,
                //Estado2 = us.Estado2,
                EstadoCivil = us.EstadoCivil,
                Instituicao = us.Instituicao,
                LoginLms = us.LoginLms,
                //MaterialDidatico = us.MaterialDidatico,
                Matricula = us.Matricula,
                Nacionalidade = us.Nacionalidade,
                Naturalidade = us.Naturalidade,
                NivelOcupacional = new BMNivelOcupacional().ObterPorNome(us.ListaNivelOcupacional.FirstOrDefault().Nome).FirstOrDefault(),
                Nome = us.Nome,
                NomeMae = us.NomeMae,
                NomePai = us.NomePai,
                NumeroIdentidade = us.NumeroIdentidade,
                OrgaoEmissor = us.OrgaoEmissor,
                Pais = us.Pais,
                //Pais2 = us.Pais2,
                //Senha = GetMD5(us.Senha),
                Sexo = us.Sexo,
                Situacao = us.Situacao,
                TelCelular = us.TelCelular,
                TelResidencial = us.TelResidencial,
                TipoDocumento = us.TipoDocumento,
                TipoInstituicao = us.TipoInstituicao,
                UF = new BMUf().ObterPorNome(us.ListaUF.FirstOrDefault().Sigla).FirstOrDefault(),
                Unidade = us.Unidade,
                RamalExibicao = us.RamalExibicao,
                TelefoneExibicao = us.TelefoneExibicao,
                TipoTelefoneExibicao = us.TipoTelefoneExibicao,
                NomeExibicao = us.NomeExibicao,
                //Imagem = us.Imagem,
                MiniCurriculo = us.MiniCurriculo
            };
        }

        private void AtualizarInformacoesDoUsuario(DTOManterUsuario us, Usuario usuario)
        {
            //Campos Editaveis
            if (!string.IsNullOrEmpty(us.RamalExibicao))
                usuario.RamalExibicao = us.RamalExibicao;
            if (!string.IsNullOrEmpty(us.RamalExibicao))
                usuario.RamalExibicao = us.RamalExibicao;
            if (!string.IsNullOrEmpty(us.TelefoneExibicao))
                usuario.TelefoneExibicao = us.TelefoneExibicao;
            if (!string.IsNullOrEmpty(us.TipoTelefoneExibicao))
                usuario.TipoTelefoneExibicao = us.TipoTelefoneExibicao;
            if (!string.IsNullOrEmpty(us.NomeExibicao))
                usuario.NomeExibicao = us.NomeExibicao;
            if (!string.IsNullOrEmpty(us.MiniCurriculo))
                usuario.MiniCurriculo = us.MiniCurriculo;
            if (!string.IsNullOrEmpty(us.Senha))
                usuario.Senha = CriptografiaHelper.Criptografar(us.Senha);

            //DADOS ATUALIZACAO CADASTRAL
            int camposAtualizados = 0;
            if (usuario.NumeroIdentidade != us.NumeroIdentidade)
            {
                usuario.NumeroIdentidade = us.NumeroIdentidade;
                camposAtualizados++;
            }
            if (usuario.OrgaoEmissor != us.OrgaoEmissor)
            {
                usuario.OrgaoEmissor = us.OrgaoEmissor;
                camposAtualizados++;
            }
            if (usuario.DataExpedicaoIdentidade != us.DataExpedicaoIdentidade)
            {
                usuario.DataExpedicaoIdentidade = us.DataExpedicaoIdentidade;
                camposAtualizados++;
            }
            if (usuario.Sexo != us.Sexo)
            {
                usuario.Sexo = us.Sexo;
                camposAtualizados++;
            }
            if (usuario.Nacionalidade != us.Nacionalidade)
            {
                usuario.Nacionalidade = us.Nacionalidade;
                camposAtualizados++;
            }
            if (usuario.EstadoCivil != us.EstadoCivil)
            {
                usuario.EstadoCivil = us.EstadoCivil;
                camposAtualizados++;
            }
            if (usuario.NomePai != us.NomePai)
            {
                usuario.NomePai = us.NomePai;
                camposAtualizados++;
            }
            if (usuario.NomeMae != us.NomeMae)
            {
                usuario.NomeMae = us.NomeMae;
                camposAtualizados++;
            }
            if (usuario.Endereco != us.Endereco)
            {
                usuario.Endereco = us.Endereco;
                camposAtualizados++;
            }
            if (usuario.Complemento != us.Complemento)
            {
                usuario.Complemento = us.Complemento;
                camposAtualizados++;
            }
            if (usuario.Bairro != us.Bairro)
            {
                usuario.Bairro = us.Bairro;
                camposAtualizados++;
            }
            if (usuario.Cep != us.Cep)
            {
                usuario.Cep = us.Cep;
                camposAtualizados++;
            }
            if (usuario.Cidade != us.Cidade)
            {
                usuario.Cidade = us.Cidade;
                camposAtualizados++;
            }
            if (usuario.Estado != us.Estado)
            {
                usuario.Estado = us.Estado;
                camposAtualizados++;
            }
            if (usuario.Pais != us.Pais)
            {
                usuario.Pais = us.Pais;
                camposAtualizados++;
            }
            if (usuario.TelResidencial != us.TelResidencial)
            {
                usuario.TelResidencial = us.TelResidencial;
                camposAtualizados++;
            }
            if (usuario.TelCelular != us.TelCelular)
            {
                usuario.TelCelular = us.TelCelular;
                camposAtualizados++;
            }
            if (usuario.Naturalidade != us.Naturalidade)
            {
                usuario.Naturalidade = us.Naturalidade;
                camposAtualizados++;
            }
            if (usuario.Email != us.Email)
            {
                var jaExiste = new BMUsuario().ObterPorFiltros(new Usuario { Email = us.Email });
                // Caso não exista o e-mail na base
                if (jaExiste.Count() == 0)
                {
                    usuario.Email = us.Email;
                    camposAtualizados++;
                }
            }
            if (camposAtualizados > 0)
                usuario.DataAtualizacaoCadastralUsuario = DateTime.Now;

        }

        private void AtualizarInformacoesDoUsuarioPortal(DTOUsuarioPortal us, ref Usuario usuario)
        {
            //Campos Editaveis
            if (!string.IsNullOrEmpty(us.TelefoneExibicao))
                usuario.TelefoneExibicao = us.TelefoneExibicao;
            if (!string.IsNullOrEmpty(us.NomeExibicao))
                usuario.NomeExibicao = us.NomeExibicao;
            if (!string.IsNullOrEmpty(us.Senha))
                usuario.Senha = CriptografiaHelper.Criptografar(us.Senha);

            //DADOS ATUALIZACAO CADASTRAL
            int camposAtualizados = 0;
            if (usuario.Sexo != us.Sexo)
            {
                usuario.Sexo = us.Sexo;
                camposAtualizados++;
            }
            if (usuario.TelCelular != us.TelCelular)
            {
                usuario.TelCelular = us.TelCelular;
                camposAtualizados++;
            }
            if (usuario.Email != us.Email)
            {
                var jaExiste = new BMUsuario().ObterPorFiltros(new Usuario { Email = us.Email });
                // Caso não exista o e-mail na base
                if (jaExiste.Count() == 0)
                {
                    usuario.Email = us.Email;
                    camposAtualizados++;
                }
            }
            if (camposAtualizados > 0)
                usuario.DataAtualizacaoCadastralUsuario = DateTime.Now;

        }

        public DTOManterUsuario ConsultarUsuario(string cpf)
        {
            Usuario usuario = new BMUsuario().ObterPorCPF(cpf);
            List<Fornecedor> fornecedores = new List<Fornecedor>();
            fornecedores.AddRange(new BMFornecedor().ObterTodos());
            DTOManterUsuario Result = null;

            if (usuario != null)
            {
                Result = ObterDTOUsuario(usuario, fornecedores);
            }

            return Result;
        }

        public DTOManterUsuarioConexao ConsultarUsuarioConexao(string cpf)
        {
            Usuario usuario = new BMUsuario().ObterPorCPF(cpf);
            List<Fornecedor> fornecedores = new List<Fornecedor>();
            fornecedores.AddRange(new BMFornecedor().ObterTodos());
            DTOManterUsuarioConexao Result = null;

            if (usuario != null)
            {
                Result = ObterDTOUsuarioConexao(usuario, fornecedores);
            }

            return Result;
        }

        public DTOUsuarioPortalConselhos ConsultarUsuarioConselhos(string cpf)
        {
            var us = this.usuarioBM.ObterPorCPF(cpf);

            if (us == null)
            {
                return ObterDTOUsuarioPortalConselhosErrorMsg("Nenhum usuário com esse CPF foi encontrado");                
            }

            if(us.ListaPerfil.All(x => x.Perfil != enumPerfil.Conselheiro))
            {
                return ObterDTOUsuarioPortalConselhosErrorMsg("O usuário precisa ser do perfil Conselheiro para ser inserido no sistema.");                
            }

            return this.ObterDTOUsuarioPortalConselhos(us);
        }

        public DTOCursosUsuarioPorCPF ConsultarCursosUsuarioPorCPF(string cpf)
        {
            var retorno = new DTOCursosUsuarioPorCPF();
            DTOManterUsuario usuario = (new ManterUsuarioServices()).ConsultarUsuario(cpf);
            if (usuario != null)
            {
                retorno.ID = usuario.ID;
                retorno.Nome = usuario.Nome;
                retorno.CPF = usuario.CPF;
                retorno.Email = usuario.Email;

                IList<MatriculaOferta> meusCursos = new Sebrae.Academico.BM.Classes.BMMatriculaOferta()
                                                    .ObterPorUsuario(usuario.ID)
                                                    .Where(f => f.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae &&
                                                              f.Oferta.SolucaoEducacional.IDChaveExterna != null &&
                                                              f.Oferta.CodigoMoodle != null)
                                                    .OrderByDescending(x => x.DataSolicitacao)
                                                    .ToList();

                foreach (var s in meusCursos)
                {
                    if (retorno.SolucoesEducacionaisMoodle.Count(f => f.ID == s.Oferta.SolucaoEducacional.ID) == 0)
                    {
                        var solucao = new DTOSolucaoEducacionalCursosUsuarioPorCPF();

                        solucao.ID = s.Oferta.SolucaoEducacional.ID;
                        solucao.Nome = s.Oferta.SolucaoEducacional.Nome;
                        solucao.IDChaveExterna = s.Oferta.SolucaoEducacional.IDChaveExterna;

                        MatriculaOferta matriculaOferta;

                        MatriculaOferta cursoInscrito = meusCursos.Where(f => f.Oferta.SolucaoEducacional.ID == solucao.ID && f.StatusMatricula.Equals(enumStatusMatricula.Inscrito)).FirstOrDefault();

                        // Da preferência a matrícula que tenha o status inscrito
                        if (cursoInscrito != null)
                        {
                            matriculaOferta = cursoInscrito;
                        }
                        else
                        {
                            matriculaOferta = meusCursos.Where(f => f.Oferta.SolucaoEducacional.ID == solucao.ID).FirstOrDefault();
                        }

                        if (matriculaOferta == null)
                            continue;

                        var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

                        if (matriculaTurma == null)
                            continue;

                        var oferta = new DTOOfertaCursosUsuarioPorCPF
                        {
                            ID = matriculaOferta.Oferta.ID,
                            Nome = matriculaOferta.Oferta.Nome,
                            ID_ChaveExterna = matriculaOferta.Oferta.IDChaveExterna,
                            CodigoMoodle = matriculaOferta.Oferta.CodigoMoodle,
                            DataInicio = matriculaTurma.Turma.DataInicio,
                            DataFim = matriculaTurma.Turma.DataFinal,
                            DataInicioInscricoes = matriculaOferta.Oferta.DataInicioInscricoes,
                            DataFimInscricoes = matriculaOferta.Oferta.DataFimInscricoes,
                            StatusMatricula = matriculaOferta.StatusMatriculaFormatado,
                            IdStatusMatricula = (int) matriculaOferta.StatusMatricula
                        };

                        if (matriculaOferta.MatriculaTurma.Any())
                        {
                            oferta.Turma.ID = matriculaTurma.Turma.ID;
                            oferta.Turma.IDChaveExternaTurma = matriculaTurma.Turma.IDChaveExterna;
                        }

                        oferta.Turma.Nome = matriculaOferta.NomeTurma;


                        solucao.Oferta.Add(oferta);

                        retorno.SolucoesEducacionaisMoodle.Add(solucao);
                    }
                }

                // Pega as trilhas que o usuário esteja inscrito
                IList<Trilha> minhasTrilhas = new BMTrilha().ObterPorUsuario(usuario.ID).Where(x => x.ID_CodigoMoodle != null).ToList();

                if (minhasTrilhas.Any())
                {

                    foreach (Trilha Trilha in minhasTrilhas)
                    {

                        DTOTrilhasUsuarioPorCPF t = new DTOTrilhasUsuarioPorCPF();

                        t.ID = Trilha.ID;
                        t.Nome = Trilha.Nome;
                        t.IDChaveExterna = Trilha.ID_CodigoMoodle.ToString();

                        retorno.TrilhasMoodle.Add(t);

                    }

                }

            }

            return retorno;
        }

        public DTOManterUsuario ConsultarUsuario(Usuario usuario)
        {
            DTOManterUsuario Result = ObterDTOUsuario(usuario);
            return Result;
        }

        public DTOUsuarioPortal ConsultarUsuarioPortal(Usuario usuario)
        {
            return ObterDTOUsuarioPortal(usuario);
        }


        public DTOManterUsuarioConexao ConsultarUsuarioConexao(Usuario usuario)
        {
            DTOManterUsuarioConexao Result = ObterDTOUsuarioConexao(usuario, new List<Fornecedor>());
            return Result;
        }

        private DTOManterUsuario ObterDTOUsuario(Usuario us)
        {
            return ObterDTOUsuario(us, new List<Fornecedor>());
        }

        private DTOManterUsuario ObterDTOUsuario(Usuario us, List<Fornecedor> fornecedores)
        {
            string txCriptografiaMoodle = string.Empty;
            string txCriptografiaWebAula = string.Empty;
            if (fornecedores.Count() > 0)
            {
                var moodle = fornecedores.FirstOrDefault(f => f.ID == (int)enumFornecedor.MoodleSebrae);
                if (moodle != null)
                {
                    txCriptografiaMoodle = moodle.TextoCriptografia;
                }
            }

            DTOManterUsuario Result = new DTOManterUsuario()
            {
                ID = us.ID,
                AnoConclusao = us.AnoConclusao,
                Bairro = us.Bairro,
                Cep = us.Cep,
                Cidade = us.Cidade,
                CodigoCampoConhecimento = us.CodigoCampoConhecimento,
                Complemento = us.Complemento,
                CPF = us.CPF,
                DataAdmissao = us.DataAdmissao,
                DataAtualizacaoCarga = us.DataAtualizacaoCarga,
                DataExpedicaoIdentidade = us.DataExpedicaoIdentidade,
                DataNascimento = us.DataNascimento,
                Email = us.Email,
                Endereco = us.Endereco,
                Escolaridade = us.Escolaridade,
                Estado = us.Estado,
                EstadoCivil = us.EstadoCivil,
                Instituicao = us.Instituicao,
                LoginLms = us.LoginLms,
                //MaterialDidatico = us.MaterialDidatico,
                Matricula = us.Matricula,
                MiniCurriculo = !string.IsNullOrEmpty(us.MiniCurriculo) ? us.MiniCurriculo.Replace("\v", " ") : string.Empty,
                Nacionalidade = us.Nacionalidade,
                Naturalidade = us.Naturalidade,
                Nome = us.Nome,
                NomeMae = us.NomeMae,
                NomePai = us.NomePai,
                NumeroIdentidade = us.NumeroIdentidade,
                OrgaoEmissor = us.OrgaoEmissor,
                Pais = us.Pais,
                SenhaMD5 = string.IsNullOrEmpty(us.Senha) ? string.Empty : CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(us.Senha)),
                SMoodle = System.Web.HttpUtility.UrlEncode(CriptografiaHelper.Criptografar(CriptografiaHelper.Decriptografar(us.Senha), txCriptografiaMoodle)),
                Sexo = us.Sexo,
                Situacao = us.Situacao,
                TelCelular = us.TelCelular,
                TelResidencial = us.TelResidencial,
                TipoDocumento = us.TipoDocumento,
                TipoInstituicao = us.TipoInstituicao,
                Unidade = us.Unidade,
                NomeExibicao = us.NomeExibicao,
                TelefoneExibicao = us.TelefoneExibicao,
                TipoTelefoneExibicao = us.TipoTelefoneExibicao,
                RamalExibicao = us.RamalExibicao,
                DataUltimaAtualizacao = us.DataAlteracao,
                GuidUsuario = ObterSenhaGuid(us),
                IsGestor = us.IsGestor()
            };

            //Obtém o link para Imagem
            var manterConfigSistema = new ManterConfiguracaoSistema();
            var enderecoSgus = manterConfigSistema.ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoSGUS).Registro;
            var manterUsuario = new ManterUsuario();        
            Result.ImagemBase64 = manterUsuario.ObterUrlImagem(us.ID);

            string linkParaImagem = new ManterUsuario().ObterLinkParaImagem(us);
            Result.LinkImagem = linkParaImagem;

            Result.LinkConexao = new FornecedorServices().ConsultaURLAcesso((int)enumFornecedor.Xys, us);

            if (string.IsNullOrEmpty(Result.NumeroIdentidade) ||
                string.IsNullOrEmpty(Result.OrgaoEmissor) ||
                !Result.DataExpedicaoIdentidade.HasValue ||
                string.IsNullOrEmpty(Result.OrgaoEmissor) ||
                string.IsNullOrEmpty(Result.Sexo) ||
                string.IsNullOrEmpty(Result.Nacionalidade) ||
                string.IsNullOrEmpty(Result.EstadoCivil) ||
                string.IsNullOrEmpty(Result.NomeMae) ||
                string.IsNullOrEmpty(Result.Endereco) ||
                string.IsNullOrEmpty(Result.Complemento) ||
                string.IsNullOrEmpty(Result.Bairro) ||
                string.IsNullOrEmpty(Result.Cep) ||
                string.IsNullOrEmpty(Result.Cidade) ||
                string.IsNullOrEmpty(Result.Estado) ||
                string.IsNullOrEmpty(Result.Pais) ||
                string.IsNullOrEmpty(Result.TelResidencial) ||
                string.IsNullOrEmpty(Result.TelCelular) ||
                string.IsNullOrEmpty(Result.Naturalidade))
            {
                if (!us.DataAtualizacaoCadastralUsuario.HasValue)
                {
                    Result.PendenciaCadastral = true;
                }
                else
                {
                    Result.PendenciaCadastral = false;
                }
            }

            //this.ObterInformacoesDoFileServer(us, Result);

            DTOUf uf = new DTOUf();
            uf.Nome = us.UF.Nome;
            uf.ID = us.UF.ID;
            uf.Sigla = us.UF.Sigla;

            DTONivelOcupacional no = new DTONivelOcupacional();
            no.Nome = us.NivelOcupacional.Nome;
            no.ID = us.NivelOcupacional.ID;

            Result.ListaNivelOcupacional.Add(no);
            Result.ListaUF.Add(uf);

            Result.TemPagamentoAssociado = false;
            Result.TemPagamentoValido = false;

            //Pagamento - Não é mais feito dessa forma agora somente por TB_OfertaTrancadaParaPagante e TB_UsuarioPagamento
            /*ConfiguracaoPagamentoDTOFiltro filtro = new ConfiguracaoPagamentoDTOFiltro() { IdUsuario = us.ID };

            us.ListaConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamentoPorFiltro(filtro);

            if (us.ListaConfiguracaoPagamentoPublicoAlvo != null && us.ListaConfiguracaoPagamentoPublicoAlvo.Count > 0)
            {

                ConfiguracaoPagamento registro = us.ListaConfiguracaoPagamentoPublicoAlvo.Where(x => x.ConfiguracaoPagamento.DataInicioCompetencia.Date <= DateTime.Today &&
                                                                      x.ConfiguracaoPagamento.DataFimCompetencia.Date >= DateTime.Today &&
                                                                      x.ConfiguracaoPagamento.Ativo).Select(x => x.ConfiguracaoPagamento).FirstOrDefault();
                if (registro != null)
                {
                    Result.TemPagamentoAssociado = true;
                    Result.CodigoPagamentoAssociado = registro.ID.ToString();
                    Result.ValorPagamentoAssociado = registro.ValorAPagar;

                    if (us.ListaHistoricoPagamento != null && us.ListaHistoricoPagamento.Any(x => x.ConfiguracaoPagamento.ID == registro.ID))
                    {
                        UsuarioPagamento registroPagamento = us.ListaHistoricoPagamento.Where(x => x.ConfiguracaoPagamento.ID == registro.ID).OrderBy(x => x.ID).FirstOrDefault();
                        Result.TemPagamentoValido = true;
                        Result.DataInicioVigenciaPagamento = registroPagamento.DataInicioVigencia;
                        Result.DataFimVigenciaPagamento = registroPagamento.DataFimVigencia;
                        Result.DataPagamento = registroPagamento.DataPagamento;
                        Result.ValorPagamento = registroPagamento.ValorPagamento;
                        Result.DataInicioRenovacaoPagamento = registroPagamento.DataInicioRenovacao;
                        Result.DataMaximaInadimplenciaPagamento = registroPagamento.DataMaxInadimplencia;
                        Result.PagamentoEfetuado = registroPagamento.PagamentoEfetuado;
                        Result.FormaPagamento = (int)registroPagamento.FormaPagamento;
                    }
                    else
                    {
                        Result.TermoAdesaoPagamento = string.IsNullOrEmpty(registro.TextoTermoAdesao) ? string.Empty : registro.TextoTermoAdesao;
                    }
                }
            }*/

            IList<MetaInstitucional> ListaMetaInstitucional = new BMMetaInstitucional().ObterMetasInstitucionaisValidas();

            //Meta Institucional
            if (ListaMetaInstitucional != null && ListaMetaInstitucional.Count > 0)
            {
                foreach (MetaInstitucional metaInstitucional in ListaMetaInstitucional)
                {
                    DTOMetaInstitucional pDTOMetaInstitucional = new DTOMetaInstitucional()
                    {
                        ID = metaInstitucional.ID,
                        Nome = metaInstitucional.Nome,
                        DataFimCiclo = metaInstitucional.DataFimCiclo,
                        DataInicioCiclo = metaInstitucional.DataInicioCiclo
                    };

                    Result.ListaMetaInstitucional.Add(pDTOMetaInstitucional);
                }

            }

            //Meta Individual
            foreach (MetaIndividual metaIndividual in us.ListaMetaIndividual)
            {
                DTOMetaIndividual pDTOMetaIndividual = new DTOMetaIndividual()
                {
                    ID = metaIndividual.ID,
                    DataValidade = metaIndividual.DataValidade,
                    Nome = metaIndividual.Nome
                };

                Result.ListaMetaIndividual.Add(pDTOMetaIndividual);
            }

            //Perfil
            foreach (UsuarioPerfil p in us.ListaPerfil)
            {
                DTOPerfil pDTO = new DTOPerfil()
                {
                    ID = p.Perfil.ID,
                    Nome = p.Perfil.Nome
                };
                Result.ListaPerfil.Add(pDTO);
            }

            //Usuario Tag
            foreach (UsuarioTag usuarioTag in us.ListaTag)
            {
                DTOTag pDTOUsuarioTag = new DTOTag()
                {
                    ID = usuarioTag.Tag.ID,
                    Nome = usuarioTag.Tag.Nome
                };

                Result.ListaTag.Add(pDTOUsuarioTag);
            }

            us.ListaPermissaoSistemasExternos = new ManterUsuario().ObterListaPermissaoSistemasExternos(us.ID);

            // Substituindo placeholders na descrição - Utilizado no caso da biblioteca CENGAGE
            var dataurl = new
            {
                plate = us.ID,
                email = us.Email,
                user = us.Nome
            };
            var data_cengage = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dataurl)));

            string tokenSOL = string.Empty;
            foreach (DTOSistemaExternoPermissao registro in us.ListaPermissaoSistemasExternos)
            {
                DTOSistemaExterno pDTOSistemaExterno = new DTOSistemaExterno()
                {
                    ID = registro.SistemaExterno.ID,
                    Nome = registro.SistemaExterno.Nome,
                    LinkAcesso = registro.SistemaExterno.LinkSistemaExterno
                };

                pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#DATAURL", data_cengage);
                pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#CPF", us.CPF);
                pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#SENHAMD5 ", CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(us.Senha)));
                pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#TOKENSABER", CriptografiaHelper.ObterHashMD5(us.CPF + "fgv2012").ToLower());
                pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#TOKENPEARSON", CriptografiaHelper.ObterHashMD5(us.CPF + "UwmdFPxx4AT6kTqtKqISn6fwqjLrPz3F").ToLower());
                if (registro.SistemaExterno.LinkSistemaExterno.Contains("SOL"))
                {
                    if (string.IsNullOrEmpty(tokenSOL))
                    {
                        tokenSOL = RecuperarTokenSOL(us.CPF, us.Senha);
                    }
                    pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#TOKENSOL", tokenSOL);
                }
                Result.ListaSistemaExterno.Add(pDTOSistemaExterno);
            }
            return Result;
        }

        public DTOManterUsuario AutenticaUsuarioInformadoPorGuid(string guid, string login)
        {            
            string guidDescriptografado = CriptografiaHelper.Decriptografar(guid);

            var senhaCripto = guidDescriptografado.Substring(guidDescriptografado.IndexOf(login) + login.Length);

            var senhaDecripto = CriptografiaHelper.Decriptografar(senhaCripto);
            
            return AutenticaUsuarioInformado(login, senhaDecripto, GetIPAddress(), string.Empty);
        }

        /// <summary>
        /// Seta e retornar o DTOUsuarioPortalConselhos com a mensagem de erro
        /// </summary>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        private DTOUsuarioPortalConselhos ObterDTOUsuarioPortalConselhosErrorMsg(string ErrorMsg)
        {
            var usConselhos = new DTOUsuarioPortalConselhos()
            {
                MsgErro = ErrorMsg
            };
            return usConselhos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="us"></param>
        /// <returns></returns>
        private DTOUsuarioPortalConselhos ObterDTOUsuarioPortalConselhos(Usuario us)
        {
            var usConselhos = new DTOUsuarioPortalConselhos()
            {
                Nome = us.Nome,
                GuidUsuario = ObterSenhaGuid(us),
                Email = us.Email,
                UF = us.UF.Sigla,
                DataNascimento = us.DataNascimento,
                Endereco = us.Endereco,
                CEP = us.Cep,
                Cidade = us.Cidade,
                TelefoneResidencial = us.TelResidencial,
                TelefoneCelular = us.TelCelular,
                Imagem = new ManterUsuario().ObterUrlImagem(us.ID)
            };

            return usConselhos;
        }


        /// <summary>
        ///     Obtem um usuário mas simples somente com os dados de acordo com a necessidade
        ///     do portal UC 3.0
        /// </summary>
        /// <param name="us">Recebe um Usuario do SGUS como parâmetro</param>
        /// <returns></returns>
        private DTOUsuarioPortal ObterDTOUsuarioPortal(Usuario us)
        {
            // Dados básicos
            var usPortal = new DTOUsuarioPortal()
            {
                ID = us.ID,
                NomeExibicao = us.NomeExibicao,
                Sexo = us.Sexo,
                Email = us.Email,
                TelefoneExibicao = us.TelefoneExibicao,
                TelCelular = us.TelCelular,
                DataUltimaAtualizacao = us.DataAlteracao,
                GuidUsuario = ObterSenhaGuid(us)
            };

            // Obter o endereço do SGUS para obter o caminho para as fotos dos perfis no mapeamento.            
            string linkParaImagem = new ManterUsuario().ObterLinkParaImagem(us);
            usPortal.LinkImagem = linkParaImagem;


            // Filtros de permissão
            DTOUf uf = new DTOUf();
            uf.Nome = us.UF.Nome;
            uf.ID = us.UF.ID;
            uf.Sigla = us.UF.Sigla;

            DTONivelOcupacional no = new DTONivelOcupacional();
            no.Nome = us.NivelOcupacional.Nome;
            no.ID = us.NivelOcupacional.ID;

            usPortal.ListaNivelOcupacional.Add(no);
            usPortal.ListaUF.Add(uf);

            // Metas
            IList<MetaInstitucional> ListaMetaInstitucional = new BMMetaInstitucional().ObterMetasInstitucionaisValidas();

            //Meta Institucional
            if (ListaMetaInstitucional != null && ListaMetaInstitucional.Count > 0)
            {
                foreach (MetaInstitucional metaInstitucional in ListaMetaInstitucional)
                {
                    DTOMetaInstitucional pDTOMetaInstitucional = new DTOMetaInstitucional()
                    {
                        ID = metaInstitucional.ID,
                        Nome = metaInstitucional.Nome,
                        DataFimCiclo = metaInstitucional.DataFimCiclo,
                        DataInicioCiclo = metaInstitucional.DataInicioCiclo
                    };

                    usPortal.ListaMetaInstitucional.Add(pDTOMetaInstitucional);
                }

            }

            //Meta Individual
            foreach (MetaIndividual metaIndividual in us.ListaMetaIndividual)
            {
                DTOMetaIndividual pDTOMetaIndividual = new DTOMetaIndividual()
                {
                    ID = metaIndividual.ID,
                    DataValidade = metaIndividual.DataValidade,
                    Nome = metaIndividual.Nome
                };

                usPortal.ListaMetaIndividual.Add(pDTOMetaIndividual);
            }

            //Perfil
            foreach (UsuarioPerfil p in us.ListaPerfil)
            {
                DTOPerfil pDTO = new DTOPerfil()
                {
                    ID = p.Perfil.ID,
                    Nome = p.Perfil.Nome
                };
                usPortal.ListaPerfil.Add(pDTO);
            }

            return usPortal;

        }

        public string ObterSenhaGuid(Usuario us)
        {
            string retorno = "";

            if (!string.IsNullOrEmpty(us.Senha))
            {
                string criptografarDados = us.Nome + us.CPF + us.Senha;

                retorno = CriptografiaHelper.Criptografar(criptografarDados);
            }
            return retorno;
        }
        
        public DTODadosPagamento VerificarPagamento(int idUsuario)
        {
            var usuario = new ManterUsuario().ObterUsuarioPorID(idUsuario);

            var retorno = new DTODadosPagamento
            {
                PossuePagamento = ConsultarStatusMatriculaSolucaoEducacional.VerificarTrancadoParaPagante(usuario),
                CEP = usuario.Cep,
                Logradouro = usuario.Endereco,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado
            };

            if (!retorno.PossuePagamento)
            {
                var hoje = DateTime.Today;

                var dadosVigentes = new BMConfiguracaoPagamento().ObterTodosIQueryable().OrderByDescending(x => x.ID)
                    .FirstOrDefault(x => x.Ativo && x.DataInicioCompetencia <= hoje && x.DataFimCompetencia >= hoje);

                retorno.TermoDeUso = dadosVigentes?.TextoTermoAdesao;
            }

            return retorno;
        }

        private DTOManterUsuarioConexao ObterDTOUsuarioConexao(Usuario us, List<Fornecedor> fornecedores)
        {
            string txCriptografiaMoodle = string.Empty;
            string txCriptografiaWebAula = string.Empty;

            DTOManterUsuarioConexao Result = new DTOManterUsuarioConexao()
            {
                ID = us.ID,
                AnoConclusao = us.AnoConclusao,
                Bairro = us.Bairro,
                Cep = us.Cep,
                Cidade = us.Cidade,
                CodigoCampoConhecimento = us.CodigoCampoConhecimento,
                Complemento = us.Complemento,
                CPF = us.CPF,
                DataAdmissao = us.DataAdmissao,
                DataAtualizacaoCarga = us.DataAtualizacaoCarga,
                DataExpedicaoIdentidade = us.DataExpedicaoIdentidade,
                DataNascimento = us.DataNascimento,
                Email = us.Email,
                Endereco = us.Endereco,
                Escolaridade = us.Escolaridade,
                Estado = us.Estado,
                EstadoCivil = us.EstadoCivil,
                Instituicao = us.Instituicao,
                LoginLms = us.LoginLms,
                //MaterialDidatico = us.MaterialDidatico,
                Matricula = us.Matricula,
                MiniCurriculo = !string.IsNullOrEmpty(us.MiniCurriculo) ? us.MiniCurriculo.Replace("\v", " ") : string.Empty,
                Nacionalidade = us.Nacionalidade,
                Naturalidade = us.Naturalidade,
                Nome = us.Nome,
                NomeMae = us.NomeMae,
                NomePai = us.NomePai,
                NumeroIdentidade = us.NumeroIdentidade,
                OrgaoEmissor = us.OrgaoEmissor,
                Pais = us.Pais,
                SenhaMD5 = string.IsNullOrEmpty(us.Senha) ? string.Empty : CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(us.Senha)),
                SMoodle = string.IsNullOrEmpty(us.Senha) ? string.Empty : System.Web.HttpUtility.UrlEncode(CriptografiaHelper.Criptografar(CriptografiaHelper.Decriptografar(us.Senha), txCriptografiaMoodle)),
                Sexo = us.Sexo,
                Situacao = us.Situacao,
                TelCelular = us.TelCelular,
                TelResidencial = us.TelResidencial,
                TipoDocumento = us.TipoDocumento,
                TipoInstituicao = us.TipoInstituicao,
                Unidade = us.Unidade,
                NomeExibicao = us.NomeExibicao,
                TelefoneExibicao = us.TelefoneExibicao,
                TipoTelefoneExibicao = us.TipoTelefoneExibicao,
                RamalExibicao = us.RamalExibicao,
                DataUltimaAtualizacao = us.DataAlteracao,
                SID = (string.IsNullOrEmpty(us.SID_Usuario) ? "" : us.SID_Usuario)
            };

            //Obtém o link para Imagem
            string linkParaImagem = new ManterUsuario().ObterLinkParaImagem(us);
            Result.LinkImagem = linkParaImagem;
            Result.LinkConexao = new FornecedorServices().ConsultaURLAcesso((int)enumFornecedor.Xys, us);

            if (string.IsNullOrEmpty(Result.NumeroIdentidade) ||
                string.IsNullOrEmpty(Result.OrgaoEmissor) ||
                !Result.DataExpedicaoIdentidade.HasValue ||
                string.IsNullOrEmpty(Result.OrgaoEmissor) ||
                string.IsNullOrEmpty(Result.Sexo) ||
                string.IsNullOrEmpty(Result.Nacionalidade) ||
                string.IsNullOrEmpty(Result.EstadoCivil) ||
                string.IsNullOrEmpty(Result.NomeMae) ||
                string.IsNullOrEmpty(Result.Endereco) ||
                string.IsNullOrEmpty(Result.Complemento) ||
                string.IsNullOrEmpty(Result.Bairro) ||
                string.IsNullOrEmpty(Result.Cep) ||
                string.IsNullOrEmpty(Result.Cidade) ||
                string.IsNullOrEmpty(Result.Estado) ||
                string.IsNullOrEmpty(Result.Pais) ||
                string.IsNullOrEmpty(Result.TelResidencial) ||
                string.IsNullOrEmpty(Result.TelCelular) ||
                string.IsNullOrEmpty(Result.Naturalidade))
            {
                if (!us.DataAtualizacaoCadastralUsuario.HasValue)
                {
                    Result.PendenciaCadastral = true;
                }
                else
                {
                    Result.PendenciaCadastral = false;
                }
            }

            //this.ObterInformacoesDoFileServer(us, Result);

            DTOUf uf = new DTOUf();
            uf.Nome = us.UF.Nome;
            uf.ID = us.UF.ID;
            uf.Sigla = us.UF.Sigla;

            DTONivelOcupacional no = new DTONivelOcupacional();
            no.Nome = us.NivelOcupacional.Nome;
            no.ID = us.NivelOcupacional.ID;

            Result.ListaNivelOcupacional.Add(no);
            Result.ListaUF.Add(uf);

            Result.TemPagamentoAssociado = false;
            Result.TemPagamentoValido = false;

            //Pagamento - Não é mais feito dessa forma agora somente por TB_OfertaTrancadaParaPagante e TB_UsuarioPagamento
            /*ConfiguracaoPagamentoDTOFiltro filtro = new ConfiguracaoPagamentoDTOFiltro() { IdUsuario = us.ID };

            us.ListaConfiguracaoPagamentoPublicoAlvo = new ManterConfiguracaoPagamento().ObterInformacoesDePagamentoPorFiltro(filtro);

            if (us.ListaConfiguracaoPagamentoPublicoAlvo != null && us.ListaConfiguracaoPagamentoPublicoAlvo.Count > 0)
            {

                ConfiguracaoPagamento registro = us.ListaConfiguracaoPagamentoPublicoAlvo.Where(x => x.ConfiguracaoPagamento.DataInicioCompetencia.Date <= DateTime.Today &&
                                                                      x.ConfiguracaoPagamento.DataFimCompetencia.Date >= DateTime.Today &&
                                                                      x.ConfiguracaoPagamento.Ativo).Select(x => x.ConfiguracaoPagamento).FirstOrDefault();
                if (registro != null)
                {
                    Result.TemPagamentoAssociado = true;
                    Result.CodigoPagamentoAssociado = registro.ID.ToString();
                    Result.ValorPagamentoAssociado = registro.ValorAPagar;

                    if (us.ListaHistoricoPagamento != null && us.ListaHistoricoPagamento.Any(x => x.ConfiguracaoPagamento.ID == registro.ID))
                    {
                        UsuarioPagamento registroPagamento = us.ListaHistoricoPagamento.Where(x => x.ConfiguracaoPagamento.ID == registro.ID).OrderBy(x => x.ID).FirstOrDefault();
                        Result.TemPagamentoValido = true;
                        Result.DataInicioVigenciaPagamento = registroPagamento.DataInicioVigencia;
                        Result.DataFimVigenciaPagamento = registroPagamento.DataFimVigencia;
                        Result.DataPagamento = registroPagamento.DataPagamento;
                        Result.ValorPagamento = registroPagamento.ValorPagamento;
                        Result.DataInicioRenovacaoPagamento = registroPagamento.DataInicioRenovacao;
                        Result.DataMaximaInadimplenciaPagamento = registroPagamento.DataMaxInadimplencia;
                        Result.PagamentoEfetuado = registroPagamento.PagamentoEfetuado;
                        Result.FormaPagamento = (int)registroPagamento.FormaPagamento;
                    }
                    else
                    {
                        Result.TermoAdesaoPagamento = string.IsNullOrEmpty(registro.TextoTermoAdesao) ? string.Empty : registro.TextoTermoAdesao;
                    }
                }
            }*/

            IList<MetaInstitucional> ListaMetaInstitucional = new BMMetaInstitucional().ObterMetasInstitucionaisValidas();

            //Meta Institucional
            if (ListaMetaInstitucional != null && ListaMetaInstitucional.Count > 0)
            {
                foreach (MetaInstitucional metaInstitucional in ListaMetaInstitucional)
                {
                    DTOMetaInstitucional pDTOMetaInstitucional = new DTOMetaInstitucional()
                    {
                        ID = metaInstitucional.ID,
                        Nome = metaInstitucional.Nome,
                        DataFimCiclo = metaInstitucional.DataFimCiclo,
                        DataInicioCiclo = metaInstitucional.DataInicioCiclo
                    };

                    Result.ListaMetaInstitucional.Add(pDTOMetaInstitucional);
                }

            }

            //Meta Individual
            foreach (MetaIndividual metaIndividual in us.ListaMetaIndividual)
            {
                DTOMetaIndividual pDTOMetaIndividual = new DTOMetaIndividual()
                {
                    ID = metaIndividual.ID,
                    DataValidade = metaIndividual.DataValidade,
                    Nome = metaIndividual.Nome
                };

                Result.ListaMetaIndividual.Add(pDTOMetaIndividual);
            }

            //Perfil
            foreach (UsuarioPerfil p in us.ListaPerfil)
            {
                DTOPerfil pDTO = new DTOPerfil()
                {
                    ID = p.Perfil.ID,
                    Nome = p.Perfil.Nome
                };
                Result.ListaPerfil.Add(pDTO);
            }

            //Usuario Tag
            foreach (UsuarioTag usuarioTag in us.ListaTag)
            {
                DTOTag pDTOUsuarioTag = new DTOTag()
                {
                    ID = usuarioTag.Tag.ID,
                    Nome = usuarioTag.Tag.Nome
                };

                Result.ListaTag.Add(pDTOUsuarioTag);
            }

            us.ListaPermissaoSistemasExternos = new ManterUsuario().ObterListaPermissaoSistemasExternos(us.ID);


            if (!string.IsNullOrEmpty(us.Senha))
            {
                foreach (DTOSistemaExternoPermissao registro in us.ListaPermissaoSistemasExternos)
                {
                    DTOSistemaExterno pDTOSistemaExterno = new DTOSistemaExterno()
                    {
                        ID = registro.SistemaExterno.ID,
                        Nome = registro.SistemaExterno.Nome,
                        LinkAcesso = registro.SistemaExterno.LinkSistemaExterno
                    };

                    pDTOSistemaExterno = this.preparaLinkDeAcessoSistemaExterno(pDTOSistemaExterno, registro, us);

                    Result.ListaSistemaExterno.Add(pDTOSistemaExterno);
                }
            }
            return Result;
        }

        private DTOSistemaExterno preparaLinkDeAcessoSistemaExterno(DTOSistemaExterno pDTOSistemaExterno, DTOSistemaExternoPermissao registro, Usuario us)
        {
            string tokenSOL = string.Empty;

            // Substituindo placeholders na descrição - Utilizado no caso da biblioteca CENGAGE
            var dataurl = new
            {
                plate = us.ID,
                email = us.Email,
                user = us.Nome
            };
            var data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dataurl)));
            pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#DATAURL", data);

            pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#CPF", us.CPF);
            pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#SENHAMD5 ", CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(us.Senha)));
            pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#TOKENSABER", CriptografiaHelper.ObterHashMD5(us.CPF + "fgv2012").ToLower());
            pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#TOKENPEARSON", CriptografiaHelper.ObterHashMD5(us.CPF + "UwmdFPxx4AT6kTqtKqISn6fwqjLrPz3F").ToLower());
            if (registro.SistemaExterno.LinkSistemaExterno.Contains("SOL"))
            {
                if (string.IsNullOrEmpty(tokenSOL))
                {
                    tokenSOL = RecuperarTokenSOL(us.CPF, us.Senha);
                }
                pDTOSistemaExterno.LinkAcesso = pDTOSistemaExterno.LinkAcesso.Replace("#TOKENSOL", tokenSOL);
            }

            return pDTOSistemaExterno;
        }

        public DTOSistemaExterno recuperaSistemaExterno(int idSistemaExterno, int idUsuario)
        {

            Usuario us = new BMUsuario().ObterPorId(idUsuario);

            us.ListaPermissaoSistemasExternos = new ManterUsuario().ObterListaPermissaoSistemasExternos(us.ID).Where(x => x.SistemaExterno.ID.Equals(idSistemaExterno)).ToList();

            DTOSistemaExterno pDTOSistemaExterno = new DTOSistemaExterno();

            if (!string.IsNullOrEmpty(us.Senha))
            {
                foreach (DTOSistemaExternoPermissao registro in us.ListaPermissaoSistemasExternos)
                {
                    pDTOSistemaExterno.ID = registro.SistemaExterno.ID;
                    pDTOSistemaExterno.Nome = registro.SistemaExterno.Nome;
                    pDTOSistemaExterno.LinkAcesso = registro.SistemaExterno.LinkSistemaExterno;

                    pDTOSistemaExterno = this.preparaLinkDeAcessoSistemaExterno(pDTOSistemaExterno, registro, us);
                }
            }

            return pDTOSistemaExterno;

        }

        private string RecuperarTokenSOL(string cpf, string senha)
        {
            string token = string.Empty;

            try
            {
                TokenWSClient twsc = new TokenWSClient();
                string senhaDescriptografada = string.Empty;
                if (!string.IsNullOrEmpty(senha))
                    senhaDescriptografada = CriptografiaHelper.Decriptografar(senha);
                token = twsc.gerarToken(cpf, senhaDescriptografada);
            }
            catch (Exception)
            {

            }
            return token;
        }

        private void ObterInformacoesDoFileServer(Usuario us, DTOManterUsuario Result)
        {
            if (us.FileServer != null && us.FileServer.ID > 0)
            {
                //(ConfiguracaoSisma.UrlServidor)+\MediaServer.ashx?Identificador=(ID_FileServer)
                ConfiguracaoSistema configuracaoSistema = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS);
                Result.LinkImagem = CommonHelper.ObterLinkParaArquivoDeImagem(configuracaoSistema.Registro, us.FileServer.ID);
                //Result.LinkImagem = string.Format(String.Concat(configuracaoSistema.Registro, "/MediaServer.ashx?Identificador={0}"), us.FileServer.ID);
            }
        }

        public DTOManterUsuario AutenticaUsuarioInformado(string cpf, string Senha, string IPUsuario, string sessionID)
        {
            var manter = new ManterUsuario();

            Usuario us = manter.Login(cpf, Senha, false, false);

            if (us == null)
            {
                FalhaAcesso falhaAcesso = new FalhaAcesso(cpf, Senha, IPUsuario);
                BMFalhaAcesso bmFalhaAcesso = new BMFalhaAcesso();
                bmFalhaAcesso.Salvar(falhaAcesso);
                throw new Exception("Login ou Senha inválida. Verifique se foi digitado corretamente.");
            }

            return ConsultarUsuario(us);
        }


        public DTOUsuarioPortal AutenticaUsuarioPortal(string cpf, string Senha, string IPUsuario, string sessionID)
        {
            var manterUsuario = new ManterUsuario();

            Usuario us = manterUsuario.Login(cpf, Senha, false, false);

            if (us == null)
            {
                FalhaAcesso falhaAcesso = new FalhaAcesso(cpf, Senha, IPUsuario);
                BMFalhaAcesso bmFalhaAcesso = new BMFalhaAcesso();
                bmFalhaAcesso.Salvar(falhaAcesso);
                throw new Exception("Login ou Senha inválida. Verifique se foi digitado corretamente.");
            }

            return this.ObterDTOUsuarioPortal(us);
        }

        public DTOUsuarioPortalConselhos AutenticaUsuarioConselhos(string cpf, string Senha, string IPUsuario)
        {
            var manterUsuario = new ManterUsuario();

            Usuario us = manterUsuario.Login(cpf, Senha, false, false);

            // Somente coneselheiros podem entrar no sistema
            if (us == null || us.ListaPerfil.All(x => x.Perfil != enumPerfil.Conselheiro))
            {
                FalhaAcesso falhaAcesso = new FalhaAcesso(cpf, Senha, IPUsuario);
                BMFalhaAcesso bmFalhaAcesso = new BMFalhaAcesso();
                bmFalhaAcesso.Salvar(falhaAcesso);
                if(us == null) { 
                    throw new Exception("Login ou Senha inválida. Verifique se foi digitado corretamente.");
                }
                if (us.ListaPerfil.All(x => x.Perfil != enumPerfil.Conselheiro))
                {
                    throw new Exception("Somente Conselheiros podem acessar esse portal. Caso seja um conselheiro por favor entre em contato via Fale Conosco.");
                }


            }

            return this.ObterDTOUsuarioPortalConselhos(us);
        }


        public DTOManterUsuarioConexao AutenticaUsuarioInformadoConexao(string cpf, string Senha, string IPUsuario, string sessionID)
        {
            var manterUsuario = new ManterUsuario();

            Usuario us = manterUsuario.Login(cpf, Senha, false, false);

            if (us == null)
            {
                FalhaAcesso falhaAcesso = new FalhaAcesso(cpf, Senha, IPUsuario);
                BMFalhaAcesso bmFalhaAcesso = new BMFalhaAcesso();
                bmFalhaAcesso.Salvar(falhaAcesso);
                throw new Exception("Login ou Senha inválida. Verifique se foi digitado corretamente.");
            }
           

            return this.ConsultarUsuarioConexao(us);
        }


        public string CadastrarImagemUsuario(int idUsuario, string imagemBase64, AuthenticationRequest autenticacao)
        {
            return new ManterUsuario().CadastrarImagem(idUsuario, imagemBase64, autenticacao.Login);
        }


        public void GerarSenhaPortal(string cpf, enumTemplate template = enumTemplate.RecuperacaoSenhaSemConfirmacao)
        {
            usuarioBM = new BMUsuario();
            var usuario = usuarioBM.ObterPorCPF(cpf);

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Email))
                    throw new AcademicoException("Email não cadastrado");

                string senha;

                Template emailSenha = TemplateUtil.ObterInformacoes(template);

                var HtmlNewLine = "</p>"+Environment.NewLine;

                int quebra = emailSenha.TextoTemplate.IndexOf(HtmlNewLine) + HtmlNewLine.Length;
                string assuntoDoEmail = emailSenha.Assunto;
                string corpoDoEmail = string.Empty;
                if (quebra > 0)
                {
                    assuntoDoEmail = emailSenha.Assunto;
                    corpoDoEmail = emailSenha.TextoTemplate;
                }

                if (string.IsNullOrEmpty(usuario.Senha) || usuario.Senha.Equals("c2VicmFlMjAxNQ=="))
                {
                    //CRIAR SENHA
                    senha = WebFormHelper.ObterSenhaAleatoria();
                    usuario.Senha = CriptografiaHelper.Criptografar(senha);
                    usuario.Auditoria = new Auditoria("DESLOGADO");
                    usuarioBM.Salvar(usuario);
                }
                else
                {
                    senha = CriptografiaHelper.Decriptografar(usuario.Senha);
                }
                corpoDoEmail = corpoDoEmail.Replace("#NOME", usuario.Nome)
                                               .Replace("#SENHA", senha)
                                               .Replace("#CPF", usuario.CPF)
                                               .Replace("#DATAHORA", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm"));

                //Envia e-mail para o usuário 
                EmailUtil.Instancia.EnviarEmail(usuario.Email,
                                                    assuntoDoEmail,
                                                    corpoDoEmail);
            }
            else
            {
                throw new AcademicoException("CPF não localizado");
            }
        }

        public DTOManterUsuario ConsultarUsuarioPorSID(string sid)
        {
            Usuario usuario = new BMUsuario().ObterADPorSID(sid);
            DTOManterUsuario Result = ObterDTOUsuario(usuario);
            return Result;
        }

        public DTOManterUsuario ConsultarUsuarioPorToken(Guid token)
        {
            TokenAcesso tokenAcesso = new BMTokenAcesso().ObterTokenValido(token);
            Usuario usuario = tokenAcesso.Usuario;
            DTOManterUsuario Result = ObterDTOUsuario(usuario);
            return Result;
        }

        public DTOManterUsuario ConsultarUsuarioPorTokenMD5(string token)
        {
            TokenAcesso tokenAcesso = new BMTokenAcesso().ObterTokenValidoMd5TokenAcesso(token);
            Usuario usuario = tokenAcesso.Usuario;
            DTOManterUsuario Result = ObterDTOUsuario(usuario);
            return Result;
        }

        public object DTOConfiguracaoUsuarioPublicoAlvo { get; set; }

        public string GerarTokenPorSID(string sid, AuthenticationProviderRequest loginFornecedor)
        {
            BMTokenAcesso bmTokenAcesso = new BMTokenAcesso();

            Usuario usuario = new BMUsuario().ObterADPorSID(sid);
            Fornecedor fornecedor = new BMFornecedor().ObterPorLogin(loginFornecedor.Login);

            TokenAcesso tokenAcesso = bmTokenAcesso.ObterTokenValidoPorUsuarioFornecedor(usuario, fornecedor);

            if (tokenAcesso == null)
            {
                tokenAcesso = new TokenAcesso();
                tokenAcesso.Usuario = usuario;
                tokenAcesso.Fornecedor = fornecedor;
                tokenAcesso.Token = Guid.NewGuid();
                tokenAcesso.DataCriacao = DateTime.Now;
                tokenAcesso.IpAcesso = GetIPAddress();

                bmTokenAcesso.Salvar(tokenAcesso);
            }

            return tokenAcesso.Token.ToString();
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

         public DTOManterUsuario AutenticaUsuarioInformadoPorSID(string sid)
        {
            Usuario usuario = new BMUsuario().ObterADPorSID(CriptografiaHelper.Decriptografar(sid));

            string senhaDescriptografada = CriptografiaHelper.Decriptografar(usuario.Senha);

            return AutenticaUsuarioInformado(usuario.CPF, senhaDescriptografada, GetIPAddress(), string.Empty);
        }

         /*         
         Retorna o SID criptografado somente se esse existir no banco de dados
        */
        public string ConsultarSIDValido(string sid)
        {                       
            Usuario usuario = new BMUsuario().ObterADPorSID(sid);

            string token = "";

            if(usuario!=null){
                token = CriptografiaHelper.Base64Encode(CriptografiaHelper.Criptografar(sid));
            }

            return token;
        }
    }
}
