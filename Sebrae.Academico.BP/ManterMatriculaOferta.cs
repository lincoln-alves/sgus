using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterMatriculaOferta : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMMatriculaOferta _bmMatriculaOferta = null;

        #endregion

        #region "Construtor"

        public ManterMatriculaOferta()
        {
            _bmMatriculaOferta = new BMMatriculaOferta();
        }

        #endregion

        #region "Métodos Públicos"

        public MatriculaOferta ObterInformacoesDaMatricula(int IdMatriculaOferta)
        {
            try
            {
                //TODO: verificar motivo pelo qual ele não está carregando a oferta;
                var mo = new MatriculaOferta();
                mo = _bmMatriculaOferta.ObterPorID(IdMatriculaOferta);
                mo.Oferta = new BMOferta().ObterPorId(mo.Oferta.ID);
                return mo;
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void ValidarMatriculaOfertaInformada(MatriculaOferta pMatriculaOferta)
        {
            if ((pMatriculaOferta.Usuario == null) || (pMatriculaOferta.Usuario != null && pMatriculaOferta.Usuario.ID <= 0))
            {
                throw new AcademicoException("Usuário. Campo Obrigatório.");
            }

            if (pMatriculaOferta.StatusMatricula <= 0)
            {
                throw new AcademicoException("Status. Campo Obrigatório.");
            }
        }

        public void IncluirMatriculaOferta(MatriculaOferta pMatriculaOferta, bool verificarPoliticaDeConsequencia = true)
        {
            try
            {
                ValidarMatriculaOfertaInformada(pMatriculaOferta);

                ValidarPreRequisitosDaMatricula(pMatriculaOferta);

                pMatriculaOferta.DataSolicitacao = DateTime.Now;

                AtualizarMatriculaOferta(pMatriculaOferta, verificarPoliticaDeConsequencia);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void ValidarPreRequisitosDaMatricula(MatriculaOferta pMatriculaOferta)
        {
            new BMSolucaoEducacional().ValidarPreRequisitosDaMatricula(pMatriculaOferta);
        }

        public void AtualizarMatriculaOferta(MatriculaOferta pMatriculaOferta, bool verificarPoliticaDeConsequencia = true, bool fazerMerge = false)
        {
            try
            {
                ValidarMatriculaOfertaInformada(pMatriculaOferta);

                // Verifica se o usuário está bloqueado pelas políticas de consequência.
                if (verificarPoliticaDeConsequencia)
                    _bmMatriculaOferta.VerificarPoliticaDeConsequencia(pMatriculaOferta);

                var bmMatriculaOferta = new BMMatriculaOferta();

                if (fazerMerge)
                    bmMatriculaOferta.FazerMerge(pMatriculaOferta);
                else
                    bmMatriculaOferta.Salvar(pMatriculaOferta);

                //TODO: para retirar esse try catch é preciso atualizar os lugares que dependem desse metodo.
            }
            catch (PoliticaConsequenciaException)
            {
                throw;
            }
            catch
            {
                throw new AcademicoException(string.Format(
                    "Houve um erro no salvamento da matrícula do aluno {0} ({1})", pMatriculaOferta.Usuario.Nome,
                    pMatriculaOferta.Usuario.CPF));
            }
        }

        public List<MatriculaOferta> ObterMatriculaOfertaPorOferta(int ofertaId, int limit = 30, int page = 0, Usuario usuarioLogado = null)
        {
            try
            {
                if (ofertaId <= 0)
                    throw new Exception("Oferta. Campo Obrigatório ");

                var query = _bmMatriculaOferta.ObterTodosIQueryable()
                    .Select(x => new MatriculaOferta
                    {
                        ID = x.ID,
                        FornecedorNotificado = x.FornecedorNotificado,
                        StatusMatricula = x.StatusMatricula,
                        DataSolicitacao = x.DataSolicitacao,
                        LinkAcesso = x.LinkAcesso,
                        Oferta = new Oferta
                        {
                            ID = x.Oferta.ID
                        },
                        Usuario = new Usuario
                        {
                            ID = x.Usuario.ID,
                            Nome = x.Usuario.Nome
                        },
                        UF = new Uf
                        {
                            ID = x.UF.ID
                        }
                    })
                    .Where(x => x.Oferta.ID == ofertaId)
                    .OrderBy(x => x.Usuario.Nome).AsQueryable();

                if (usuarioLogado != null)
                    query = query.Where(x => x.UF.ID == usuarioLogado.UF.ID);

                query = page == 0 ? query.Take(limit) : query.Skip(page * limit).Take(limit);

                var result = query.ToList();

                // Preencher matrículas turmas.
                PreencherMatriculaTurma(result);

                // Preencher dados dos usuários.
                PreencherUsuarios(result);

                // Preencher dados das ofertas.
                PreencherOfertas(result);

                return result;
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        private static void PreencherMatriculaTurma(List<MatriculaOferta> result)
        {
            // Pegar os dados das matrículas turmas vindos de apenas uma consulta. Muito mais performático
            // do que executar várias consultas seguidas.

            var matriculasIds = result.Select(x => x.ID).Distinct().ToList();

            var matriculasTurmas = new ManterMatriculaTurma().ObterTodosIQueryable().Select(x => new MatriculaTurma
            {
                ID = x.ID,
                DataTermino = x.DataTermino,
                MediaFinal = x.MediaFinal,
                MatriculaOferta = new MatriculaOferta
                {
                    ID = x.MatriculaOferta.ID
                },
                Turma = new Turma
                {
                    ID = x.Turma.ID,
                    Nome = x.Turma.Nome
                }
            })
                .Where(x => matriculasIds.Contains(x.MatriculaOferta.ID)).ToList();

            var turmasIds = matriculasTurmas.Select(x => x.Turma.ID).Distinct().ToList();

            var avaliacoes = new ManterAvaliacao().ObterTodasIQueryable()
                .Select(x => new Avaliacao
                {
                    ID = x.ID,
                    Turma = new Turma
                    {
                        ID = x.Turma.ID
                    }
                })
                .Where(x => turmasIds.Contains(x.Turma.ID));

            // Atualizar avaliações.
            foreach (var matriculaTurma in matriculasTurmas)
            {
                matriculaTurma.Turma.Avaliacoes = avaliacoes.Where(x => x.Turma.ID == matriculaTurma.Turma.ID).ToList();
            }

            // Atualizar matrículas turmas direto nas matrículas ofertas.
            foreach (var matricula in result)
            {
                matricula.MatriculaTurma =
                    matriculasTurmas.Where(x => x.MatriculaOferta.ID == matricula.ID).ToList();
            }
        }

        private static void PreencherOfertas(List<MatriculaOferta> result)
        {
            var ofertasIds = result.Select(x => x.Oferta.ID).Distinct().ToList();

            // Pegar os dados das ofertas vindos de apenas uma consulta. Muito mais performático
            // do que executar várias consultas seguidas.
            var ofertas = new ManterOferta().ObterTodasOfertas().Select(x => new Oferta
            {
                ID = x.ID,
                AlteraPeloGestorUC = x.AlteraPeloGestorUC,
                SolucaoEducacional = new SolucaoEducacional
                {
                    ID = x.SolucaoEducacional.ID
                },
                CertificadoTemplate = x.CertificadoTemplate != null
                ? new CertificadoTemplate
                {
                    ID = x.CertificadoTemplate.ID
                }
                : null
            })
                .Where(x => ofertasIds.Contains(x.ID)).ToList();

            // Preencher dados das SEs.
            var idsSes = ofertas.Select(x => x.SolucaoEducacional.ID).Distinct().ToList();

            var solucoes = new ManterSolucaoEducacional().ObterTodosIQueryable()
                .Select(x => new SolucaoEducacional
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    Ativo = x.Ativo,
                    Fornecedor = new Fornecedor
                    {
                        ID = x.Fornecedor.ID
                    },
                    UFGestor = new Uf
                    {
                        ID = x.UFGestor.ID
                    }
                })
                .Where(x => idsSes.Contains(x.ID));

            var turmas = new ManterTurma().ObterTodosIQueryable()
                .Select(x => new Turma
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    Oferta = new Oferta
                    {
                        ID = x.Oferta.ID
                    }
                })
                .Where(x => ofertasIds.Contains(x.Oferta.ID));

            foreach (var oferta in ofertas)
            {
                oferta.SolucaoEducacional = solucoes.FirstOrDefault(x => x.ID == oferta.SolucaoEducacional.ID);
                oferta.ListaTurma = turmas.Where(x => x.Oferta.ID == oferta.ID).ToList();

                var matriculasOferta = result.Where(x => x.Oferta.ID == oferta.ID);

                foreach (var matricula in matriculasOferta)
                {
                    matricula.Oferta = oferta;
                }
            }
        }

        private static void PreencherUsuarios(List<MatriculaOferta> result)
        {
            var usuariosIds = result.Select(x => x.Usuario.ID).Distinct().ToList();

            // Pegar os dados dos usuários vindos de apenas uma consulta. Muito mais performático
            // do que executar várias consultas seguidas.
            var usuarios = new ManterUsuario().ObterTodosIQueryable().Select(x => new Usuario
            {
                ID = x.ID,
                Nome = x.Nome,
                Email = x.Email,
                NomeExibicao = x.NomeExibicao,
                UF = new Uf
                {
                    ID = x.UF.ID,
                    Sigla = x.UF.Sigla
                }
            })
                .Where(x => usuariosIds.Contains(x.ID)).ToList();

            foreach (var usuario in usuarios)
            {
                var matriculasUsuario = result.Where(x => x.Usuario.ID == usuario.ID);

                foreach (var matricula in matriculasUsuario)
                {
                    matricula.Usuario = usuario;
                }
            }
        }

        public IQueryable<MatriculaOferta> ObterPorOferta(int ofertaId)
        {
            return _bmMatriculaOferta.ObterPorOferta(ofertaId);
        }

        public int ObterQuantidadeMatriculaOfertaPorOferta(int ofertaId)
        {
            try
            {
                if (ofertaId <= 0)
                    throw new Exception("Oferta. Campo Obrigatório ");

                return _bmMatriculaOferta.ObterTodosIQueryable().Select(x => new MatriculaOferta
                {
                    ID = x.ID,
                    Oferta = new Oferta
                    {
                        ID = x.Oferta.ID
                    }
                }).Where(x => x.Oferta.ID == ofertaId).Select(x => new { x.ID }).Count();
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IQueryable<Turma> ObterTurmasDaOferta(int IdOferta)
        {
            try
            {
                return _bmMatriculaOferta.ObterTurmasDaOferta(IdOferta);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }

        }

        /// <summary>
        /// Obtém os cursos onde o usuário tem matrícula oferta
        /// </summary>
        /// <param name="ListaMatriculaOferta"></param>
        /// <returns></returns>
        public IList<SolucaoEducacional> ObterCursos(IList<MatriculaOferta> ListaMatriculaOferta)
        {
            IList<SolucaoEducacional> listaCursos = ListaMatriculaOferta.Select(x => x.Oferta.SolucaoEducacional).ToList();
            return listaCursos;
        }

        public IQueryable<MatriculaOferta> ObterPorUsuario(int usuarioId)
        {
            try
            {
                return _bmMatriculaOferta.ObterPorUsuario(usuarioId);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IQueryable<MatriculaOferta> ObterPorUsuarioESolucaoEducacional(int usuarioId, int solucaoId)
        {
            try
            {
                return _bmMatriculaOferta.ObterPorUsuarioESolucaoEducacional(usuarioId, solucaoId);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public MatriculaOferta ObterMatriculaOfertaPorID(int Id)
        {
            try
            {
                return _bmMatriculaOferta.ObterPorID(Id);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public void VerificarPoliticaDeConsequencia(MatriculaOferta matricula)
        {
            _bmMatriculaOferta.VerificarPoliticaDeConsequencia(matricula);
        }

        public void VerificarPoliticaDeConsequencia(int idUsuario, int idSolucaoEducacional)
        {
            _bmMatriculaOferta.VerificarPoliticaDeConsequencia(idUsuario, idSolucaoEducacional);
        }

        public IQueryable<MatriculaOferta> ObterTodosIQueryable()
        {
            return _bmMatriculaOferta.ObterTodosIQueryable();
        }

        public IEnumerable<MatriculaOferta> ObterTodos()
        {
            return _bmMatriculaOferta.ObterTodos();
        }

        public IQueryable<MatriculaOferta> ObterTodosParaConsultarLinkAcessoFornecedor()
        {
            return _bmMatriculaOferta.ObterTodosParaConsultarLinkAcessoFornecedor();
        }

        public void Salvar(MatriculaOferta matriculaOferta)
        {
            _bmMatriculaOferta.Salvar(matriculaOferta);
        }
        #endregion

        public MatriculaOferta ObterPorCodigoCertificado(string codigo)
        {
            return _bmMatriculaOferta.ObterPorCodigoCertificado(codigo);
        }


        public IQueryable<MatriculaOferta> ObterPorUsuarioETurma(int usuarioId, int turmaId)
        {
            try
            {
                return _bmMatriculaOferta.ObterPorUsuarioETurma(usuarioId, usuarioId);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public bool AprovacaoPorUsuarioESolucaoEducacional(int usuarioId, int solucaoId)
        {
            return _bmMatriculaOferta.AprovacaoPorUsuarioESolucaoEducacional(usuarioId, solucaoId);
        }

        public void Dispose()
        {
            _bmMatriculaOferta.Dispose();
        }

        public MatriculaOferta VerificarFilaEspera(MatriculaOferta matricula)
        {
            var matriculaOferta =  ObterTodosIQueryable()
               .Where(x => x.Oferta.ID == matricula.Oferta.ID)
               .OrderByDescending(x => x.DataSolicitacao)
               .FirstOrDefault(x => x.StatusMatricula == enumStatusMatricula.FilaEspera);

            if (matriculaOferta != null)
            {
                matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                Salvar(matriculaOferta);
                NotificarAlunos(matriculaOferta, enumTemplate.NotificarFilaEspera);
            }

            return ObterMatriculaOfertaPorID(matricula.ID);
        }

        public IEnumerable<Usuario> NotificarAlunosFilaDeEsperaAoFinalDaOferta()
        {
            var usuariosNotificados = ObterTodosIQueryable()
                 .Where(x => x.StatusMatricula == enumStatusMatricula.FilaEspera)
                 .Where(x => x.Oferta.ListaTurma.Any(y => y.DataInicio == DateTime.Today))
                 .ToList()
                 .Select(x => NotificarAlunos(x, enumTemplate.NotificarParaIncricaoTurmaFutura));

            return usuariosNotificados;
        }

        private static Usuario NotificarAlunos(MatriculaOferta matriculaOferta, enumTemplate tipoTemplate )
        {
            var usuario = new ManterUsuario().ObterPorID(matriculaOferta.Usuario.ID);

            var template = new ManterTemplate().ObterTemplatePorID((int)tipoTemplate);
            template.TextoTemplate = template.TextoTemplate.Replace("#ALUNO", usuario?.NomeExibicao ?? "");
            template.TextoTemplate = template.TextoTemplate.Replace("#TURMA", matriculaOferta.Oferta.ListaTurma?.FirstOrDefault()?.Nome ?? "");

            EmailUtil.Instancia.EnviarEmail(usuario.Email, template.Assunto, template.TextoTemplate);
            return usuario;
        }

    }
}