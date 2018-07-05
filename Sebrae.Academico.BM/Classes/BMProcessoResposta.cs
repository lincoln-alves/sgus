using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMProcessoResposta : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<ProcessoResposta> repositorio;

        #endregion

        #region "Construtor"

        public BMProcessoResposta()
        {
            repositorio = new RepositorioBase<ProcessoResposta>();
        }

        #endregion

        public IList<ProcessoResposta> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<ProcessoResposta>();
            return query.ToList<ProcessoResposta>();
        }

        public ProcessoResposta ObterPorEtapaRespostaId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<ProcessoResposta>();
            return query.FirstOrDefault(x => x.ListaEtapaResposta.Any(d => d.ID == pId));
        }

        public ProcessoResposta ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<ProcessoResposta>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<ProcessoResposta> ObterPorFiltro(ProcessoResposta processoResposta)
        {
            var query = repositorio.session.Query<ProcessoResposta>();

            if (processoResposta.Usuario != null)
            {
                query = query.Where(x => x.Usuario.ID == processoResposta.Usuario.ID);
            }

            return query.ToList<ProcessoResposta>();
        }

        public IList<ProcessoResposta> ObterPorProcessoUsuario(int idProcesso, int idUsuario)
        {
            var query = repositorio.session.Query<ProcessoResposta>();


            query = query.Where(x => x.Processo.ID == idProcesso && x.Usuario.ID == idUsuario);


            return query.ToList<ProcessoResposta>();
        }

        /*
         Recebe o ID do processo resposta e etapa resposta atual
         */
        public EtapaResposta ObterEtapaRespostaAtual(int pr_id)
        {

            EtapaResposta etapaResposta = new EtapaResposta();

            var query = (from p in repositorio.session.Query<ProcessoResposta>()

                         join er in repositorio.session.Query<EtapaResposta>() on
                         p.ID equals er.ProcessoResposta.ID

                         join e in repositorio.session.Query<Etapa>() on
                         er.Etapa.ID equals e.ID

                         where p.ID.Equals(pr_id) && er.Ativo

                         select er);

            // Caso a etapa esteja sendo realizada, ou seja alguma etapa ainda está em andamento
            IList<EtapaResposta> etapasRespostaFluxoNormal = query
                .Where(x => x.ProcessoResposta.Concluido == false && x.Status.Equals((int)enumStatusEtapaResposta.Aguardando)).OrderByDescending(x => x.Etapa.Ordem).ToList();

            if (etapasRespostaFluxoNormal.Count() > 0)
            {
                etapaResposta = etapasRespostaFluxoNormal.First();
            }
            else
            {

                // Se tiver finalizado pega o último status deixado
                IList<EtapaResposta> etapasRespostaFluxoFinalizado = query.Where(x => x.ProcessoResposta.Concluido == true &&
                                                                                !x.Status.Equals((int)enumStatusEtapaResposta.Aguardando)).OrderByDescending(x => x.Etapa.Ordem).ToList();

                if (etapasRespostaFluxoFinalizado.Count() > 0)
                {
                    etapaResposta = etapasRespostaFluxoFinalizado.First();
                }

            }

            return etapaResposta;
        }

        /*
            Retorna se o usuário deve voltar e realizar a primeira etapa do processo novamente.
         *  Só é usado para o caso particular da etapa ser reprovada e voltar para a etapa inicial
        */
        public bool demandanteDeveReIniciar(ProcessoResposta processoResposta, Usuario usuario)
        {

            bool deveReiniciar = false;

            // Se o usuário é dono do processo e já teve alguma reprovação nesse
            if (usuario.ID == processoResposta.Usuario.ID &&
                processoResposta.ListaEtapaResposta.Where(x => !x.Ativo).Count() > 0)
            {

                List<EtapaResposta> etapaResposta = processoResposta.ListaEtapaResposta.Where(x => x.Ativo && x.Status == (int)enumStatusEtapaResposta.Aguardando).ToList();

                // Se só tiver uma etapa aguardando e for a primeira etapa
                if (etapaResposta.Count() == 1 && etapaResposta.First().Etapa.PrimeiraEtapa)
                {

                    deveReiniciar = true;

                }

            }

            return deveReiniciar;
        }

        public EtapaResposta pegaUltimaEtapaReprovada(ProcessoResposta processoResposta, Usuario usuario)
        {

            EtapaResposta etapaResposta = new EtapaResposta();

            // Se o usuário é dono do processo e já teve alguma reprovação nesse
            if (usuario.ID == processoResposta.Usuario.ID &&
                processoResposta.ListaEtapaResposta.Where(x => !x.Ativo).Count() > 0)
            {

                // Pega a última negada
                etapaResposta = processoResposta.ListaEtapaResposta.Where(x => !x.Ativo && x.Status == (int)enumStatusEtapaResposta.Negado).OrderByDescending(x => x.ID).First();

            }

            return etapaResposta;
        }

        public List<EtapaResposta> pegaTodasEtapasReprovadas(ProcessoResposta processoResposta)
        {
            return processoResposta.ListaEtapaResposta.Where(x => x.Status == (int)enumStatusEtapaResposta.Negado).ToList();
        }

        public void Salvar(ProcessoResposta model)
        {

            //Caso seja unico, descomentar a linha baixo e implementar
            //a verificacao por nome do programa.
            if (model.ID == 0)
            {
                if (this.ObterPorId(model.ID) != null)
                {
                    throw new AcademicoException("Já existe um registro.");
                }
            }

            repositorio.Salvar(model);

        }

        public void Excluir(ProcessoResposta model)
        {
            repositorio.Excluir(model);
        }

        private void ValidarModuloInformada(ProcessoResposta model)
        {
            //throw new NotImplementedException();
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<ProcessoResposta> ObterTodosIQueryable()
        {
            return repositorio.session.Query<ProcessoResposta>().AsQueryable();
        }
    }
}
