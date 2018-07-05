using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioCargo : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<UsuarioCargo> _repositorio;

        public BMUsuarioCargo()
        {
            _repositorio = new RepositorioBase<UsuarioCargo>();
        }

        public IQueryable<UsuarioCargo> ObterPorUfTipoCargo(Uf uf, EnumTipoCargo tipoCargo)
        {
            return
                _repositorio.session.Query<UsuarioCargo>()
                    .Where(x => x.Cargo.Uf.ID == uf.ID && x.Cargo.TipoCargo == tipoCargo);
        }

        public UsuarioCargo ObterPorCargo(Cargo cargo)
        {
            return
                _repositorio.session.Query<UsuarioCargo>()
                    .FirstOrDefault(x => x.Cargo.ID == cargo.ID);
        }

        public void Salvar(UsuarioCargo usuarioCargo)
        {
            _repositorio.Salvar(usuarioCargo);
        }

        /// <summary>
        /// Inserir um usuário num cargo. Caso o cargo informado seja Diretoria ou Gabinere
        /// e caso exista outro usuário neste cargo, remove o usuário e insere o novo,
        /// pois não permite mais de um usuário no mesmo cargo, na mesma UF, para diretor
        /// e chefe de gabinete.
        /// </summary>
        /// <param name="usuario">Usuário a assumir o cargo.</param>
        /// <param name="cargo">Cargo a ser assumido.</param>
        /// <param name="mover">Caso true, move o usuário mesmo nos cargos repetitíveis</param>
        public void Salvar(Usuario usuario, Cargo cargo, bool mover = false)
        {
            // Executa tudo numa transação para evitar operações feitas no banco pela metade.
            using (var transacao = _repositorio.session.BeginTransaction())
            {
                try
                {
                    // Verificar se o indivíduo está em algum cargo que pode repetir várias vezes, e,
                    // caso esteja, remove suas participações nesses outros cargos, pois ele está
                    // sendo movido.
                    var todosCargosSaoRepetiveis = usuario.ListaUsuarioCargo.All(x => x.Cargo.UsuarioPodeRepetirNoCargo());

                    if (mover || (todosCargosSaoRepetiveis == false && cargo.UsuarioPodeRepetirNoCargo() == false))
                    {
                        foreach (var usuarioCargo in usuario.ListaUsuarioCargo)
                        {
                            _repositorio.session.Delete(usuarioCargo);
                        }
                    }

                    // Só remove o atual caso seja Diretor ou Chefe de gabinete.
                    if (cargo.TipoCargo == EnumTipoCargo.Diretoria || cargo.TipoCargo == EnumTipoCargo.Gabinete)
                    {
                        var posseAtual = ObterPorCargo(cargo);

                        if (posseAtual != null)
                        {
                            // Se já for o próprio usuário em posse do cargo, não faz nada.
                            if (posseAtual.Usuario.ID == usuario.ID)
                                return;

                            // Caso já exista alguém no cargo, remove e insere antes de inserir a nova pessoa.
                            _repositorio.session.Delete(posseAtual);

                            cargo.UsuariosCargos.Remove(posseAtual);
                        }
                    }

                    var novaPosse = new UsuarioCargo
                    {
                        Usuario = usuario,
                        Cargo = cargo
                    };

                    _repositorio.session.SaveOrUpdate(novaPosse);
                    transacao.Commit();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
                finally
                {
                    _repositorio.LimparSessao();
                }
            }
        }

        public UsuarioCargo ObterPorId(int pId)
        {
            return _repositorio.session.Query<UsuarioCargo>().FirstOrDefault(x => x.ID == pId);
        }


        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public void Remover(int usuarioCargoId)
        {
            _repositorio.Excluir(new UsuarioCargo { ID = usuarioCargoId });
        }

        public void Remover(UsuarioCargo usuarioCargo)
        {
            _repositorio.Excluir(usuarioCargo);
        }

        public IQueryable<UsuarioCargo> ObterTodos()
        {
            return _repositorio.ObterTodosIQueryable();
        }

        public IQueryable<UsuarioCargo> ObterPorTipoCargo(EnumTipoCargo diretoria)
        {
            return ObterTodos().Where(x => x.Cargo.TipoCargo == diretoria);
        }
    }
}