using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMOfertaGerenciadorVaga : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<OfertaGerenciadorVaga> repositorio;

        public BMOfertaGerenciadorVaga()
        {
            repositorio = new RepositorioBase<OfertaGerenciadorVaga>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public OfertaGerenciadorVaga ObterPorID(int id)
        {
            return repositorio.ObterPorID(id);
        }

        public void Cadastrar(OfertaGerenciadorVaga ofertaGerenciadorVaga)
        {
            repositorio.Salvar(ofertaGerenciadorVaga);
        }

        public void Alterar(OfertaGerenciadorVaga ofertaGerenciadorVaga)
        {
            repositorio.Salvar(ofertaGerenciadorVaga);
        }
        
        /// <summary>
        /// (Usuario usuarioLogado) envie (null) se o usuário logado for Administrador.
        /// Caso o usuário seja Gestor, envie o objeto do usuário
        /// </summary>
        public IList<OfertaGerenciadorVaga> ObterSolicitacoes(Usuario usuarioLogado, string solicitacao)
        {
            var query = repositorio.session.Query<OfertaGerenciadorVaga>();

            if (usuarioLogado != null)
            {
                query = query.Where(x => x.Usuario.ID == usuarioLogado.ID);
            }
            
            if (solicitacao.Equals("Respondidas"))
                query = query.Where(x => (x.Contemplado.HasValue && x.Contemplado == true) || ((x.Contemplado.HasValue && x.Contemplado == false) && x.Resposta != null));
            else if (solicitacao.Equals("NaoRespondidas"))
                query = query.Where(x => (!x.Contemplado.HasValue || x.Contemplado == false) && x.Resposta == null);
            else if (solicitacao.Equals("Aprovadas"))
                query = query.Where(x => (x.Contemplado.HasValue && x.Contemplado.Value == true));
            else if (solicitacao.Equals("Recusadas"))
                query = query.Where(x => (!x.Contemplado.HasValue && x.Contemplado.Value == false) && x.Resposta != null);
            else
                query = query.Where(x => x.DataAlteracao > DateTime.Today.AddDays(-1));


            return query.ToList();
        }
    }
}
