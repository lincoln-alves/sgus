using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using System.Collections;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioTrilha : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<UsuarioTrilha> repositorio;

        #endregion

        #region "Construtor"

        public BMUsuarioTrilha()
        {
            repositorio = new RepositorioBase<UsuarioTrilha>();
        }

        #endregion
        /// <summary>
        /// Obtem a Carga Horária do Nivel da Trilha somado ao Total de Carga Horária de Soluções AutoIndicativas
        /// </summary>
        /// <param name="idUsuarioTrilha">ID do Usuário da trilha</param>
        /// <returns>Total Da Carga Horária do Nível da trilha</returns>
        public int ObterTotalCargaHoraria(int idUsuarioTrilha)
        {
            var usuarioTrilha = ObterPorId(idUsuarioTrilha);
            return usuarioTrilha == null ? 0 : ObterTotalCargaHoraria(usuarioTrilha);
        }

        /// <summary>
        /// Obtem a Carga Horária do Nivel da Trilha somado ao Total de Carga Horária de Soluções AutoIndicativas
        /// </summary>
        /// <param name="usuarioTrilha">Usuário da trilha</param>
        /// <returns>Total Da Carga Horária do Nível da trilha</returns>
        public int ObterTotalCargaHoraria(UsuarioTrilha usuarioTrilha)
        {
            var queryItemTrilhaParticipacao = repositorio.session.Query<ItemTrilhaParticipacao>();
            var listItemTrilha = queryItemTrilhaParticipacao
                                    .Where(p =>
                                        p.UsuarioTrilha.ID == usuarioTrilha.ID &&
                                        p.Monitor == null &&
                                        p.Autorizado == true)
                                    .Select(p =>
                                        p.ItemTrilha.ID)
                                    .Distinct();
            var queryItemTrilha = repositorio.session.Query<ItemTrilha>();
            queryItemTrilha = queryItemTrilha.Where(p => listItemTrilha.Contains(p.ID) && p.Usuario != null && p.FormaAquisicao != null);
            queryItemTrilha = queryItemTrilha.Where(p => p.FormaAquisicao.CargaHoraria != null);
            var totalCargaHorariaParticipacoes = queryItemTrilha.Any() ? queryItemTrilha.Sum(p => p.FormaAquisicao.CargaHoraria.Value) : 0;

            return totalCargaHorariaParticipacoes + usuarioTrilha.TrilhaNivel.CargaHoraria;
        }

        public UsuarioTrilha ObterPorId(int ID)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            //query = query.Fetch(x => x.TrilhaNivel).ThenFetch(x => x.Trilha);
            return repositorio.ObterPorID(ID);
        }

        public IList<UsuarioTrilha> ObterPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            return query.ToList<UsuarioTrilha>();
        }

        public void Salvar(UsuarioTrilha pUsuarioTrilha)
        {
            ValidarMatriculaTrilhaInformada(pUsuarioTrilha);

            AplicarRegraDoStatusNaoAprovado(pUsuarioTrilha);

            AplicarRegraDaDataLimite(pUsuarioTrilha);
            if (pUsuarioTrilha.StatusMatricula == enumStatusMatricula.Concluido) pUsuarioTrilha.StatusMatricula = enumStatusMatricula.Aprovado;
            repositorio.Salvar(pUsuarioTrilha);
        }

        private static void AplicarRegraDaDataLimite(UsuarioTrilha pUsuarioTrilha)
        {
            if (pUsuarioTrilha.DataLimite.Equals(DateTime.MinValue))
            {
                pUsuarioTrilha.DataLimite = DateTime.Today.AddDays(pUsuarioTrilha.TrilhaNivel.QuantidadeDiasPrazo);
            }
        }

        private void AplicarRegraDoStatusNaoAprovado(UsuarioTrilha pUsuarioTrilha)
        {
            if (pUsuarioTrilha.StatusMatricula.Equals(enumStatusMatricula.Reprovado) &&
               (pUsuarioTrilha.NovaProvaLiberada.Equals(true)))
            {
                //Se informou para liberar a nota da nova prova
                if (!pUsuarioTrilha.DataLiberacaoNovaProva.HasValue)
                {
                    throw new AcademicoException("Data de liberação da Nova Prova. Campo Obrigatório");
                }

            }
        }

        public IList<UsuarioTrilha> ObterUsuarioTrilhas()
        {
            return repositorio.ObterTodos();
        }

        public IQueryable<UsuarioTrilha> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public void Excluir(UsuarioTrilha pUsuarioTrilha)
        {
            if (ValidarDependencias(pUsuarioTrilha))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Matrícula.");

            repositorio.Excluir(pUsuarioTrilha);
        }

        public void ExcluirSemValidacao(UsuarioTrilha pUsuarioTrilha)
        {
            repositorio.Excluir(pUsuarioTrilha);
        }

        public bool ValidarDependencias(UsuarioTrilha pUsuarioTrilha)
        {
            ValidarInstancia(pUsuarioTrilha);
            UsuarioTrilha usuarioTrilha = (UsuarioTrilha)pUsuarioTrilha;

            return ((usuarioTrilha.ListaItemTrilhaParticipacao != null && usuarioTrilha.ListaItemTrilhaParticipacao.Count > 0) ||
                   (usuarioTrilha.ListaTrilhaAtividadeFormativaParticipacao != null && usuarioTrilha.ListaTrilhaAtividadeFormativaParticipacao.Count > 0));
        }

        public void VerificarCamposObrigatoriosDoFiltro(UsuarioTrilha pUsuarioTrilha)
        {
            ValidarInstancia(pUsuarioTrilha);

            if (pUsuarioTrilha.TrilhaNivel == null) throw new Exception("Nível da Trilha. Campo Obrigatório ");
        }

        public void ValidarMatriculaTrilhaInformada(UsuarioTrilha pUsuarioTrilha)
        {
            // Validando se a instância do usuário trilha está nula.
            ValidarInstancia(pUsuarioTrilha);

            if (pUsuarioTrilha.Usuario == null) throw new Exception("Usuário. Campo Obrigatório ");

            if (pUsuarioTrilha.TrilhaNivel == null) throw new Exception("Nível da Trilha. Campo Obrigatório ");

            if (pUsuarioTrilha.DataLimite == null) throw new Exception("Data Limite. Campo Obrigatório ");
        }

        public IList<Usuario> ObterPorTrilhaTrilhaNivel(int idTrilha, int idTrilhaNivel)
        {
            IList<Usuario> ListaUsuarios = null;
            var query = repositorio.session.Query<UsuarioTrilha>();
            query = query.Fetch(x => x.Usuario);
            query = query.Where(x => x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                     x.StatusMatricula != enumStatusMatricula.CanceladoAluno);

            if (idTrilhaNivel > 0)
            {
                query = query.Where(x => x.TrilhaNivel.ID == idTrilhaNivel);
            }

            var ListaUsuarioTrilha = query.ToList<UsuarioTrilha>().OrderBy(x => x.Usuario.Nome);

            ListaUsuarios = ListaUsuarioTrilha.Select(x => new Usuario() { ID = x.Usuario.ID, Nome = x.Usuario.Nome }).ToList<Usuario>();

            return ListaUsuarios;
        }


        public IList<UsuarioTrilha> ObterMatriculasDoUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            query = query.Fetch(x => x.Usuario);
            query = query.Fetch(x => x.NivelOcupacional);

            return query.ToList<UsuarioTrilha>();
        }

        public IList<DTOAlunoDaTrilha> ListarRelatorioDoAlunoDaTrilha(int idUsuarioTrilha)
        {
            ProcRelatorioAlunoDaTrilha procRelatorioAlunoDaTrilha = new ProcRelatorioAlunoDaTrilha();
            return procRelatorioAlunoDaTrilha.ListarRelatorioDoAlunoDaTrilha(idUsuarioTrilha);
        }

        public IList<UsuarioTrilha> ObterPorFiltro(UsuarioTrilha pUsuarioTrilha)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();

            if (pUsuarioTrilha.TrilhaNivel != null && pUsuarioTrilha.TrilhaNivel.ID > 0)
                query = query.Where(x => x.TrilhaNivel.ID == pUsuarioTrilha.TrilhaNivel.ID);

            if (pUsuarioTrilha.Usuario != null && pUsuarioTrilha.Usuario.ID > 0)
                query = query.Where(x => x.Usuario.ID == pUsuarioTrilha.Usuario.ID);

            if (pUsuarioTrilha.ID > 0)
                query = query.Where(x => x.ID == pUsuarioTrilha.ID);

            if (pUsuarioTrilha.NovasTrilhas != null)
                query = query.Where(x => x.NovasTrilhas == pUsuarioTrilha.NovasTrilhas);

            /* O Fetch faz o inner join / left outer join para buscar os dados do usuario.
               O método CriarSessionFactory da classe NHibernateHelper possui a instrução
               .UseOuterJoin() para indicar ao NHibernate que o left outer join será utilizado 
               nas queries */
            query = query.Fetch(x => x.Usuario);
            query = query.Fetch(x => x.NivelOcupacional);

            return query.ToList<UsuarioTrilha>();

        }


        public IList<UsuarioTrilha> ObterPorFiltro(UsuarioTrilha pUsuarioTrilha, IEnumerable<int> ufsSelecionados)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();

            if (pUsuarioTrilha.TrilhaNivel != null && pUsuarioTrilha.TrilhaNivel.ID > 0)
                query = query.Where(x => x.TrilhaNivel.ID == pUsuarioTrilha.TrilhaNivel.ID);

            if (pUsuarioTrilha.Usuario != null && pUsuarioTrilha.Usuario.ID > 0)
                query = query.Where(x => x.Usuario.ID == pUsuarioTrilha.Usuario.ID);

            if (pUsuarioTrilha.ID > 0)
                query = query.Where(x => x.ID == pUsuarioTrilha.ID);

            if (pUsuarioTrilha.NovasTrilhas != null)
                query = query.Where(x => x.NovasTrilhas == pUsuarioTrilha.NovasTrilhas);

            query = ufsSelecionados.Any() ? query.Where(x => ufsSelecionados.Contains(x.Uf.ID)) : query;

            /* O Fetch faz o inner join / left outer join para buscar os dados do usuario.
               O método CriarSessionFactory da classe NHibernateHelper possui a instrução
               .UseOuterJoin() para indicar ao NHibernate que o left outer join será utilizado 
               nas queries */
            query = query.Fetch(x => x.Usuario);
            query = query.Fetch(x => x.NivelOcupacional);

            return query.ToList<UsuarioTrilha>();

        }

        public IList<UsuarioTrilha> ObterMatriculasDataExpiracao(short pDiasExpiracao)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();

            query = query.Where(x => x.StatusMatricula == enumStatusMatricula.Inscrito ||
                                      x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno);

            return query.ToList().Where(x => DateTime.Now.Date.AddDays((pDiasExpiracao - 1) * -1).Equals(x.DataLimite.Date)).ToList();
        }

        public IList<UsuarioTrilha> ObterVencidos()
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            query = query.Where(x => x.DataLimite.Date <= DateTime.Now.Date);
            query = query.Where(x => x.StatusMatricula == enumStatusMatricula.Inscrito);
            return query.ToList();
        }

        public UsuarioTrilha ObterPorToken(string token)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            var usuario = query.FirstOrDefault(x => x.Token.Equals(token));
            return usuario;
        }

        public bool AutenticarUsuarioPorToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            using (var usuarioTrilhaBm = new BMUsuarioTrilha())
            {

                return usuarioTrilhaBm.ObterPorToken(token) != null;

            }
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IList<UsuarioTrilha> ObterPorFiltroTrilhaNivelSituacao(int idTrilha, int idNivel, string Situacao)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();

            if (idTrilha > 0)
                query = query.Where(x => x.TrilhaNivel.Trilha.ID == idTrilha);

            if (idNivel > 0)
                query = query.Where(x => x.TrilhaNivel.ID == idNivel);

            if (!string.IsNullOrWhiteSpace(Situacao))
            {
                enumStatusMatricula enumerator;
                Enum.TryParse<enumStatusMatricula>(Situacao, out enumerator);
                query = query.Where(x => x.StatusMatricula == enumerator);
            }

            return query.ToList();


        }

        public UsuarioTrilha ObterPorCodigoCertificao(string codigo, Usuario usuario = null)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();

            if (usuario != null)
            {
                query = query.Where(x => x.Usuario.ID == usuario.ID);
            }

            var usuarioTrilha = query.FirstOrDefault(x => x.CDCertificado == codigo);

            return usuarioTrilha;
        }

        public bool MatriculaPertenceStatus(int usuarioId, int trilhaNivelId, List<int> idsStatusMatricula)
        {
            return
                repositorio.session.Query<UsuarioTrilha>()
                    .Any(
                        x =>
                            x.Usuario.ID == usuarioId && x.TrilhaNivel.ID == trilhaNivelId &&
                            idsStatusMatricula.Contains((int)x.StatusMatricula));
        }

        public UsuarioTrilha ObterPorUsuarioTrilhaNivel(UsuarioTrilha matricula, TrilhaNivel nivel)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            var usuarioTrilha = query.FirstOrDefault(x => x.ID == matricula.ID && x.TrilhaNivel.ID == nivel.ID && x.NovasTrilhas == true);

            return usuarioTrilha;
        }

        public UsuarioTrilha ObterPorUsuarioTrilhaNivel(int usuario, int nivel)
        {
            var query = repositorio.session.Query<UsuarioTrilha>();
            return query.FirstOrDefault(x => x.Usuario.ID == usuario && x.TrilhaNivel.ID == nivel);
        }

        public IQueryable<UsuarioTrilha> ObterTodos()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public void LimparSessao()
        {
            repositorio.LimparSessao();
        }
    }
}