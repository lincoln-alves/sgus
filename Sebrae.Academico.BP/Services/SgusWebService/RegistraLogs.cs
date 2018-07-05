using System;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.Services
{
    public class RegistraLogs : BusinessProcessServicesBase
    {

        public void RegistraAcessoTrilha(int IdUsuarioTrilha)
        {

            LogAcessoTrilha ltr = new LogAcessoTrilha(IdUsuarioTrilha);
            
            try
            {
                new BMLogAcessoTrilha().Salvar(ltr);
            }
            catch
            {
                return;
            }
        }

        public void RegistraAcessoSolucaoEducacional(int IdMatriculaTurma)
        {
            LogAcessoTurma lt = new LogAcessoTurma(IdMatriculaTurma);
            
            try
            {
                new BMLogAcessoTurma().Salvar(lt);
            }
            catch
            {
                return;
            }
        }

        public void RegistraSolicitacaoContato(string CPF, string Nome, string Email, string Assunto, string Mensagem, string Ip)
        {

            LogFaleConosco lgFc = new LogFaleConosco()
            {
                Assunto = Assunto,
                CPF = CPF,
                DataSolicitacao = DateTime.Now,
                Email = Email,
                IP = Ip,
                Mensagem = Mensagem,
                Nome = Nome
            };

            try
            {
                new BMFaleConosco().Salvar(lgFc);
            }
            catch
            {
                return;
            }

        }

        public void RegistraAcessoPagina(int IdUsuario, string URL, string NomePagina, string IP)
        {
            LogAcessoPagina lgAp = new LogAcessoPagina();

            lgAp.DTSolicitacao = DateTime.Now;
            lgAp.IDUsuario.ID = IdUsuario;
            lgAp.IP = IP;
            //lgAp.Nome = NomePagina;
            //lgAp.Link = URL;


            try
            {
                new BMLogAcessoPagina().Salvar(lgAp);
                this.AtualizarNotificacoesNaoVisualizadas(IdUsuario, URL);
            }
            catch
            {
                return;
            }

            
        
        }

        private void AtualizarNotificacoesNaoVisualizadas(int IdUsuario, string URL)
        {
            BMNotificacao bmNotificacao = new BMNotificacao();
            List<Notificacao> ListaNotificacoesNaoVisualizadas = bmNotificacao.ObterNotificacoesNaoVisualizadas(IdUsuario, URL).ToList();

            if (ListaNotificacoesNaoVisualizadas != null && ListaNotificacoesNaoVisualizadas.Count > 0)
            {
                foreach (Notificacao notificacao in ListaNotificacoesNaoVisualizadas)
                {
                    notificacao.Visualizado = true;
                    notificacao.DataVisualizacao = DateTime.Now;
                    bmNotificacao.Salvar(notificacao);
                }
            }
        }

        public void RegistraBusca(int idUsuario, string busca)
        {
            LogBuscaSite log = new LogBuscaSite(idUsuario, busca);
            try
            {
                new BMLogBuscaSite().Salvar(log);
                
            }
            catch
            {
                return;
            }
        }
    }
}
