using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Web;
using FluentNHibernate.Conventions;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuario : BusinessManagerBase, IDisposable
    {

        #region Atributos

        public RepositorioBase<Usuario> repositorio;
        private const string sessaoDoUsuario = "usuarioSGUS";
        private const string sessaoDoPerfilSimuladoDoUsuario = "perfilSimuladoSGUS";
        private const string sessaoDosPerfisOriginaisDoUsuario = "perfisOriginaisSGUS";

        #endregion

        #region "Construtor"

        public BMUsuario()
        {
            repositorio = new RepositorioBase<Usuario>();
        }

        #endregion


        public Usuario ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public IList<Usuario> BuscarporNome(Usuario ptrilha)
        {
            //return repositorio.GetByProperty("Nome", ptrilha.Nome);
            var query = repositorio.session.Query<Usuario>();
            return query.Where(x => x.Nome == ptrilha.Nome).ToList<Usuario>();
        }

        public Usuario ValidarInformacoesParaSalvarImagem(int idUsuario, string imagem)
        {
            this.ValidarInformacoes(idUsuario, imagem);
            Usuario usuario = this.VerificarExistenciaDoUsuario(idUsuario);
            return usuario;
        }

        private Usuario VerificarExistenciaDoUsuario(int idUsuario)
        {
            Usuario usuario = this.ObterPorId(idUsuario);

            if (usuario == null)
            {
                throw new AcademicoException("Usuário Inexistente");
            }

            return usuario;
        }

        public Usuario ObterADPorSID(string sID)
        {
            var query = repositorio.session.Query<Usuario>();
            return query.FirstOrDefault(x => x.SID_Usuario == sID);
        }

        public void ValidarInformacoes(int idUsuario, string Imagem)
        {
            if (idUsuario <= 0) throw new AcademicoException("Usuário. Campo Obrigatório");

            if (string.IsNullOrWhiteSpace(Imagem)) throw new AcademicoException("Imagem. Campo Obrigatório");
        }

        public void Salvar(Usuario pUsuario, bool ignorarValidacao = false)
        {
            if (ignorarValidacao == false)
                ValidarUsuarioInformado(pUsuario);

            repositorio.Salvar(pUsuario);
        }

        public void Commit()
        {
            repositorio.Commit();
        }
        public void SalvarSemValidacao(Usuario pUsuario)
        {
            repositorio.SalvarSemCommit(pUsuario);
        }

        public void SalvarSemCommit(Usuario pUsuario)
        {
            ValidarUsuarioInformado(pUsuario);
            repositorio.SalvarSemCommit(pUsuario);
        }

        private void ValidarUsuarioInformado(Usuario pUsuario)
        {
            if (string.IsNullOrWhiteSpace(pUsuario.CPF))
                throw new AcademicoException("CPF não informado. Campo Obrigatório!");

            if (string.IsNullOrWhiteSpace(pUsuario.Nome))
                throw new AcademicoException("Nome não informado. Campo Obrigatório!");

            if (pUsuario.NivelOcupacional == null)
                throw new AcademicoException("Nível ocupacional. Campo Obrigatório!");
            if (pUsuario.NivelOcupacional.ID == 0)
                throw new AcademicoException("Nível ocupacional. Campo Obrigatório!");


            if (pUsuario.UF == null)
                throw new AcademicoException("UF. Campo Obrigatório!");
            if (pUsuario.UF.ID == 0)
                throw new AcademicoException("UF. Campo Obrigatório!");

            this.VerificarExistenciaDeUsuario(pUsuario);

            this.TratarDatasDefault(pUsuario);

        }

        private void TratarDatasDefault(Usuario pUsuario)
        {
            if (pUsuario.DataAdmissao.HasValue && pUsuario.DataAdmissao.Value.Equals(DateTime.MinValue))
            {
                pUsuario.DataAdmissao = null;
            }

            if (pUsuario.DataAtualizacaoCarga.HasValue && pUsuario.DataAtualizacaoCarga.Value.Equals(DateTime.MinValue))
            {
                pUsuario.DataAtualizacaoCarga = null;
            }

            if (pUsuario.DataExpedicaoIdentidade.HasValue && pUsuario.DataExpedicaoIdentidade.Value.Equals(DateTime.MinValue))
            {
                pUsuario.DataExpedicaoIdentidade = null;
            }

            if (pUsuario.DataNascimento.HasValue && pUsuario.DataNascimento.Value.Equals(DateTime.MinValue))
            {
                pUsuario.DataNascimento = null;
            }
        }

        private void VerificarExistenciaDeUsuario(Usuario pUsuario)
        {
            Usuario usuario = this.ObterPorCPF(pUsuario.CPF.Trim());

            if (pUsuario != null && usuario != null)
            {
                if (pUsuario.ID != usuario.ID)
                {
                    throw new AcademicoException(string.Format("O usuário '{0}' já está cadastrado",
                                                 pUsuario.Nome.Trim()));
                }
            }
        }

        private void ValidarDadosDeLogin(string usuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new AcademicoException("Usuário. Campo Obrigatório!");

            if (string.IsNullOrWhiteSpace(senha))
                throw new AcademicoException("Senha. Campo Obrigatório!");
        }

        public IList<Usuario> ObterUsuarios()
        {
            return repositorio.ObterTodos();
        }

        public IList<Usuario> ObterUsuariosQueDesejamReceberNotificacaoOferta()
        {
            var query = repositorio.session.Query<Usuario>();
            IList<Usuario> ListaUsuarios = query.Where(x => x.NotificaOferta == true).ToList();
            return ListaUsuarios;
        }

        public void ExcluirUsuario(Usuario pUsuario)
        {
            repositorio.Excluir(pUsuario);
        }

        private bool ValidarSenhaPorUsuarioUf(Usuario usuario, SenhaEmergencia senha)
        {
            if (senha.UF.ID == (int)enumUF.NA || usuario.UF.ID == senha.UF.ID)
                return true;
            else
                return false;
        }

        private static void ValidarSituacaoUsuario(Usuario usuarioEncontrado)
        {
            if (!string.IsNullOrEmpty(usuarioEncontrado.Situacao))
            {
                switch (usuarioEncontrado.Situacao.ToLower())
                {
                    case "licença mater. compl. 180 dias":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença maternidade, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "outros":
                        throw new AcademicoException("Usuário inativo no sistema!");
                    case "licença s/venc":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença sem vencimento, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "licenca mater.":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença maternidade, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "admissão prox.mês":
                        throw new AcademicoException("Acesso bloqueado pois usuário ainda não está ativo na base de empregados, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "demitido":
                        throw new AcademicoException("Acesso bloqueado. Por gentileza, entre em contato com o suporte para solicitar revisão de seu acesso");
                    case "Af.Ac.Trabalho":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em afastamento, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "apos. invalidez":
                        throw new AcademicoException("Acesso bloqueado pois usuário está aposentado, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    //case "férias":
                    //    throw new AcademicoException("Acesso bloqueado pois usuário está férias, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "licença mater.":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença maternidade, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "licença remun.":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença remunerada, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "af.previdencia":
                        throw new AcademicoException("Acesso bloqueado pois usuário está em licença médica, caso não seja este o caso, procure o gestor da UC em seu estado.");
                    case "inativo":
                        throw new AcademicoException("Acesso bloqueado pois usuário ainda não está ativo na base de empregados, caso não seja este o caso, procure o gestor da UC em seu estado");
                }

            }
            if (!usuarioEncontrado.Ativo)
            {
                throw new AcademicoException("Usuário inativo no sistema!");
            }
        }

        public bool AutenticarUsuario(string CPF, string Senha)
        {

            if (string.IsNullOrEmpty(CPF))
                return false;

            if (CPF.Trim().ToLower().Equals("usuariodrupal"))
            {
                ConfiguracaoSistema configuracaoSistema = new BMConfiguracaoSistema().ObterPorID((int)enumConfiguracaoSistema.SenhaUsuarioDefaultDrupal);
                Senha = configuracaoSistema.Registro;
                return true;
            }


            using (BMUsuario userBM = new BMUsuario())
            {

                Usuario us = userBM.ObterPorCPF(CPF);

                if (us == null)
                    return false;
                else
                {
                    if (string.IsNullOrEmpty(us.Senha))
                    {
                        return false;
                    }

                    string senhaCriptografada = CriptografiaHelper.Criptografar(Senha);

                    if (us.Senha.ToLower().Equals(senhaCriptografada.ToLower()))
                    {
                        return true;
                    }
                    else if (Senha.ToLower() == CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(us.Senha)).ToLower())
                    {
                        return true;
                    }
                }
                return false;

            }

            //return !(usuario == null || usuario.ID <= 0);
        }

        public Usuario ObterPorCPF(Usuario usuario)
        {
            this.ValidarInstancia(usuario);
            Usuario usuarioRetorno = this.ObterPorCPF(usuario.CPF);
            return usuarioRetorno;
        }

        public IQueryable<Usuario> ObterTodos()
        {
            return repositorio.session.Query<Usuario>().Fetch(x => x.ListaPerfil);
        }

        public IQueryable<Usuario> ObterTodosIQueryable()
        {
            return repositorio.session.Query<Usuario>();
        }

        public Usuario ObterPorEmail(string email)
        {
            var query = repositorio.session.Query<Usuario>();
            return query.FirstOrDefault(x => x.Email.Equals(email));
        }

        public int ContarAtivosPorNivelOcupacional(int idNivelOcupacional)
        {
            var query = repositorio.session.Query<Usuario>();
            query = query.Where(x => x.Situacao == "ativo");
            query = query.Where(x => x.NivelOcupacional.ID == idNivelOcupacional);
            return query.Count();
        }

        public int ContarAtivosPorPerfil(int idPerfil)
        {
            var query = repositorio.session.Query<Usuario>();
            query = query.Where(x => x.Situacao == "ativo");
            query = query.Where(x => x.ListaPerfil.Any(y => y.Perfil.ID == idPerfil));
            return query.Count();
        }

        private void VerificarSeCPFFoiInformado(string pCPF)
        {
            if (string.IsNullOrWhiteSpace(pCPF)) throw new AcademicoException("CPF. Campo Obrigatório");
        }

        public Usuario ObterPorCPF(string pCPF)
        {

            this.VerificarSeCPFFoiInformado(pCPF);

            var query = repositorio.session.Query<Usuario>();

            var usuario = new Usuario();

            try
            {
                usuario = pCPF.Length == 11 ? query.FirstOrDefault(x => x.CPF.Equals(pCPF)) : query.FirstOrDefault(x => x.CPF.Contains(pCPF));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuario;
        }

        public IList<Usuario> ObterPorFiltros(Usuario userFiltro, int qtdRegistros = 0)
        {
            var query = ObterQueryPorFiltros(userFiltro);

            if (userFiltro != null)
            {
                if (qtdRegistros > 0)
                    query.Take(qtdRegistros);
            }

            return query.ToList();
        }

        public IQueryable<Usuario> ObterQueryPorFiltros(Usuario userFiltro)
        {
            var query = repositorio.session.Query<Usuario>();

            query = query.Fetch(x => x.NivelOcupacional);
            query = query.Fetch(x => x.ListaPerfil);

            if (userFiltro != null)
            {
                if (userFiltro.ID > 0)
                    query = query.Where(x => x.ID == userFiltro.ID);

                try
                {
                    if (userFiltro.ListaPerfil != null && userFiltro.ListaPerfil.Any())
                    {
                        var perfisId = userFiltro.ListaPerfil.Select(x => x.Perfil.ID).Distinct().ToList();

                        query =
                            query.Where(x => x.ListaPerfil.Select(up => up.Perfil.ID).Any(pId => perfisId.Contains(pId)));
                    }
                }
                catch { }

                if (!string.IsNullOrWhiteSpace(userFiltro.Nome))
                    query = query.Where(x => x.Nome.Trim().ToUpper().Contains(userFiltro.Nome.Trim().ToUpper()));

                if (!string.IsNullOrWhiteSpace(userFiltro.CPF))
                    query = query.Where(x => x.CPF == userFiltro.CPF);

                if (userFiltro.UF != null && userFiltro.UF.ID != 0)
                    query = query.Where(x => x.UF.ID == userFiltro.UF.ID);

                if (userFiltro.NivelOcupacional != null && userFiltro.NivelOcupacional.ID != 0)
                    query = query.Where(x => x.NivelOcupacional.ID == userFiltro.NivelOcupacional.ID);

                if (!string.IsNullOrEmpty(userFiltro.Email))
                    query = query.Where(x => x.Email.Trim().ToUpper().Contains(userFiltro.Email.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(userFiltro.Situacao))
                    query = query.Where(x => x.Situacao.ToUpper() == userFiltro.Situacao.ToUpper());

            }
            return query;
        }


        public IList<Usuario> ObterPorEstadoComPerfil(int idUF)
        {
            var query = repositorio.session.Query<Usuario>();
            query = query.Fetch(x => x.NivelOcupacional);
            query = query.Fetch(x => x.ListaPerfil);
            query = query.Where(x => x.UF.ID == idUF);
            return query.ToList();
        }

        public IQueryable<Usuario> ObterTodosPorPerfilIQueryable(enumPerfil perfil)
        {
            var query = repositorio.session.Query<UsuarioPerfil>();
            var consulta = from u in query
                           where u.Perfil.ID == (int)perfil
                           orderby u.Usuario.Nome ascending
                           select u.Usuario;
            return consulta;
        }

        public IList<Usuario> ObterTodosPorPerfil(enumPerfil perfil)
        {
            var query = repositorio.session.Query<UsuarioPerfil>();
            var consulta = from u in query
                           where u.Perfil.ID == (int)perfil
                           orderby u.Usuario.Nome ascending
                           select u.Usuario;
            return consulta.ToList();
        }

        public void SetarUsuarioLogado(Usuario usuario)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session[sessaoDoUsuario] = usuario;
            }
        }

        public void SetarPerfilSimulado(UsuarioPerfil perfilSimulado)
        {
            try
            {
                var usuarioLogado = (Usuario)HttpContext.Current.Session[sessaoDoUsuario];

                if (HttpContext.Current.Session[sessaoDosPerfisOriginaisDoUsuario] == null)
                    HttpContext.Current.Session[sessaoDosPerfisOriginaisDoUsuario] = usuarioLogado.ListaPerfil.ToList();

                HttpContext.Current.Session[sessaoDoPerfilSimuladoDoUsuario] = perfilSimulado;
            }
            catch (Exception)
            {
                throw new AcademicoException("Não foi possível simular o acesso com outros perfis");
            }
        }

        public void SetarPerfisOriginais()
        {
            if (HttpContext.Current != null)
            {
                var perfisOriginais = HttpContext.Current.Session[sessaoDosPerfisOriginaisDoUsuario];

                if (perfisOriginais != null)
                {

                    var usuarioLogado = (Usuario)HttpContext.Current.Session[sessaoDoUsuario];

                    usuarioLogado.ListaPerfil = (List<UsuarioPerfil>)perfisOriginais;

                    HttpContext.Current.Session[sessaoDoUsuario] = usuarioLogado;

                    HttpContext.Current.Session[sessaoDoPerfilSimuladoDoUsuario] = null;
                }
            }
        }

        public Usuario ObterUsuarioLogado(bool obterPerfisOriginais = false)
        {
            Usuario usuarioLogado = null;

            try
            {
                //TODO: -> USAR SOMENTE PARA DEBUG
                //if ((Debugger.IsAttached || HttpContext.Current.IsDebuggingEnabled)
                //#if DEBUG
                //                SetarUsuarioLogado(this.ObterPorId(1));
                //#endif

                if (HttpContext.Current != null)
                {
                    usuarioLogado = (Usuario)HttpContext.Current.Session[sessaoDoUsuario];

                    if (usuarioLogado != null)
                    {
                        var perfisOriginais = (List<UsuarioPerfil>)HttpContext.Current.Session[sessaoDosPerfisOriginaisDoUsuario];

                        // Verificar se o usuário logado é administrador e está simulando o acesso com outros perfis.
                        if ((perfisOriginais != null && perfisOriginais.Any(x => x.Perfil == enumPerfil.Administrador)) ||
                            usuarioLogado.ListaPerfil.Any(x => x.Perfil == enumPerfil.Administrador))
                        {
                            var perfilSimulado = HttpContext.Current.Session[sessaoDoPerfilSimuladoDoUsuario];

                            var perfisSimulados = perfilSimulado == null ? null : new List<UsuarioPerfil> { (UsuarioPerfil)perfilSimulado };

                            // Caso não queira obter os perfis simulados (obterPerfisOriginais = true),
                            // obtém os perfis originais do usuário.
                            usuarioLogado.ListaPerfil = obterPerfisOriginais
                                ? (perfisOriginais ?? usuarioLogado.ListaPerfil)
                                : (perfisSimulados ?? perfisOriginais) ?? usuarioLogado.ListaPerfil;
                        }
                    }
                }
            }
            catch
            {
                //usuarioLogado = null;

                //throw new AcademicoException("Erro ao Obter o usuário logado");
            }

            return usuarioLogado;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(repositorio);
        }

        public IQueryable<Usuario> ObterPorUfsNiveisPerfis(IList<Uf> lstUf, IList<NivelOcupacional> lstNivel, IList<Perfil> lstPerfil, bool somenteIds = true)
        {
            var query = repositorio.session.Query<Usuario>();
            query = query.Fetch(x => x.ListaPerfil);


            if (lstUf != null && lstUf.Any())
                query = query.Where(x => lstUf.Contains(x.UF));

            if (lstNivel != null && lstNivel.Any())
                query = query.Where(x => lstNivel.Contains(x.NivelOcupacional));

            if (lstPerfil != null && lstPerfil.Any())
            {
                var perfis = lstPerfil.Select(p => p.ID).ToList();
                query = query.Where(x => x.ListaPerfil.Any(up => perfis.Contains(up.Perfil.ID)));
            }

            if (somenteIds)
                query = query.Select(x => new Usuario
                {
                    ID = x.ID
                });

            return query;
        }

        public IList<Usuario> ObterUsuariosQueResponderamItensTrilha(Usuario usuario, List<int> cpfParticipantes)
        {
            var query = repositorio.session.Query<Usuario>();

            query = query.Where(x => cpfParticipantes.Contains(x.ID));

            //query = query.Where(x=>x

            return query.ToList();
        }

        public bool PerfilGestor()
        {
            var usuarioLogado = ObterUsuarioLogado();
            return usuarioLogado.ListaPerfil.Any(x => x.Perfil == enumPerfil.GestorUC);
        }

        public bool PerfilAdministrador()
        {
            return ObterUsuarioLogado().IsAdministrador();
        }

        public int ObterUfLogadoSeGestor(Usuario usuarioLogado = null)
        {
            usuarioLogado = usuarioLogado ?? ObterUsuarioLogado();

            return usuarioLogado.IsGestor() ? usuarioLogado.UF.ID : 0;
        }

        public List<Usuario> ObterMonitoresParaNotificar()
        {
            return repositorio.session.Query<TrilhaNivel>()
                        .Where(x => x.AvisarMonitor && x.Monitor != null)
                        .Select(x => x.Monitor).OrderBy(x => x.ID)
                        .ToList();
        }

        public bool IsSimulandoPerfil()
        {
            return HttpContext.Current.Session[sessaoDoPerfilSimuladoDoUsuario] != null;
        }

        public Perfil ObterPerfilSimulado()
        {
            if (HttpContext.Current != null)
                return ((UsuarioPerfil)HttpContext.Current.Session[sessaoDoPerfilSimuladoDoUsuario]).Perfil;

            return null;
        }

        public IQueryable<Usuario> ObterPorNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            return repositorio.session.Query<Usuario>().Where(x => x.NivelOcupacional.ID == nivelOcupacional.ID);
        }

        public IQueryable<Usuario> ObterPorNiveisOcupacionais(List<int> niveisOcupacionais)
        {
            return repositorio.session.Query<Usuario>().Where(x => niveisOcupacionais.Contains(x.NivelOcupacional.ID));
        }

        public void SalvarEmLote(List<Usuario> usuarios, int batchSize)
        {
            using (var transacao = repositorio.session.BeginTransaction())
            {
                repositorio.session.SetBatchSize(batchSize);

                try
                {
                    foreach (var usuario in usuarios)
                    {
                        repositorio.session.Save(usuario);
                    }
                    transacao.Commit(); //flush to database
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
        }

        public bool PerfilAdministradorTrilha()
        {
            return ObterUsuarioLogado().IsAdministradorTrilha();
        }

        public IList<Usuario> ObterUsuariosPDI()
        {
            NHibernate.IQuery query = repositorio.session.CreateSQLQuery("SELECT CPF as CPF, DATADEMISSAO as DataDemissao FROM PDI_Registro")
                .AddScalar("CPF", NHibernate.NHibernateUtil.String)
                .AddScalar("DataDemissao", NHibernate.NHibernateUtil.DateTime)
                .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Usuario)));

            return query.List<Usuario>();
        }
    }
}
