using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterNotificacao : BusinessProcessServicesBase
    {

        private BMNotificacao notificacaoBM;

        public void RegistrarVisualizacao(List<int> listaNotificacao, AuthenticationRequest autenticacao)
        {
            notificacaoBM = new BMNotificacao();

            foreach (var item in listaNotificacao)
            {
                Notificacao notificacao = notificacaoBM.ObterPorID(item);
                notificacao.Visualizado = true;
                notificacao.DataVisualizacao = DateTime.Now;
                notificacao.Auditoria = new Auditoria(autenticacao.Login);
                notificacaoBM.Salvar(notificacao);
            }


        }

        /// <summary>
        /// Método responsável pela obtenção de notificações do usuário realizando alguns filtros
        /// </summary>
        /// <param name="pIdUsuario"></param>
        /// <param name="DataGeracao"></param>
        /// <param name="ocultarVisualizadas"></param>
        /// <returns></returns>
        public IList<DTONotificacao> ConsultarNotificacaoPorUsuario(int pIdUsuario, DateTime? DataGeracao, bool ocultarVisualizadas = true)
        {
            notificacaoBM = new BMNotificacao();

            IList<Notificacao> lstNotificacao;
            if (!DataGeracao.HasValue)
            {
                lstNotificacao =
                    (notificacaoBM.ObterPorUsuario(new Usuario { ID = pIdUsuario }, null)).OrderByDescending(
                        x => x.DataGeracao).ToList();
            }
            else
            {
                lstNotificacao =
                    (notificacaoBM.ObterPorUsuario(new Usuario { ID = pIdUsuario }, DataGeracao)).OrderByDescending(
                        x => x.DataGeracao).ToList();
            }

            if (ocultarVisualizadas)
            {
                lstNotificacao = lstNotificacao.Where(n => !n.MensagemLida.Equals(Constantes.Sim)).ToList();
            }

            if (lstNotificacao.Count == 0)
                return new List<DTONotificacao>();

            return ConverterDominioDTO(lstNotificacao.ToList());
        }

        public DTONotificacoes ConsultarNotificacaoPorUsuarioPaginado(int pIdUsuario, DateTime? DataGeracao, int pagina, int limitePorPagina, bool ocultarVisualizadas = true)
        {
            notificacaoBM = new BMNotificacao();

            var dto = new DTONotificacoes();

            IList<Notificacao> lstNotificacao;
            if (!DataGeracao.HasValue)
            {
                lstNotificacao = (notificacaoBM.ObterPorUsuario(new Usuario() { ID = pIdUsuario }, null))
                                                    .OrderByDescending(x => x.DataGeracao).ToList();
            }
            else
            {
                lstNotificacao = (notificacaoBM.ObterPorUsuario(new Usuario() { ID = pIdUsuario }, DataGeracao))
                                                    .OrderByDescending(x => x.DataGeracao).ToList();
            }

            if (lstNotificacao.Count == 0)
                return new DTONotificacoes();

            if (ocultarVisualizadas)
            {
                lstNotificacao = lstNotificacao.Where(n => !n.MensagemLida.Equals(Constantes.Sim)).ToList();
            }

            // Retorna o total de páginas para contagem
            dto.TotalNotificacoes = lstNotificacao.Count;

            // Paginação
            lstNotificacao = lstNotificacao.Skip((pagina - 1) * limitePorPagina).Take(limitePorPagina).ToList();

            dto.Notificacoes = ConverterDominioDTO(lstNotificacao.ToList()).ToList();

            return dto;
        }


        private IList<DTONotificacao> ConverterDominioDTO(IList<Notificacao> pListaDominio)
        {

            IList<DTONotificacao> lstResult = new List<DTONotificacao>();

            foreach (Notificacao n in pListaDominio)
            {
                DTONotificacao ndto = new DTONotificacao()
                {
                    DataGeracao = n.DataGeracao,
                    DataVisualizacao = n.DataVisualizacao,
                    ID = n.ID,
                    Link = n.Link,
                    Usuario = n.Usuario.Nome,
                    Visualizado = n.Visualizado,
                    TextoNotificacao = n.TextoNotificacao,
                    TipoNotificacao = (int)n.TipoNotificacao
                };

                lstResult.Add(ndto);
            }

            return lstResult;
        }

        public void PublicarNotificacao(string link, string texto, DateTime? data, int[] ufs, int[] niveis, int[] perfis)
        {
            var ufBm = new BMUf();
            IList<Uf> lstUf = ufs.Select(uf => ufBm.ObterPorId(uf)).ToList();

            var nivelBm = new BMNivelOcupacional();
            IList<NivelOcupacional> lstNivel = niveis.Select(nivel => nivelBm.ObterPorID(nivel)).ToList();

            var perfilBm = new BMPerfil();
            IList<Perfil> lstPerfil = perfis.Select(perfil => perfilBm.ObterPorId(perfil)).ToList();


            var usuarios = new BMUsuario().ObterPorUfsNiveisPerfis(lstUf, lstNivel, lstPerfil);


            var notificacaoBm = new BMNotificacao();

            foreach (var u in usuarios)
            {
                notificacaoBm.Salvar(new Notificacao
                {
                    DataGeracao = DateTime.Now,
                    Link = link,
                    DataNotificacao = data,
                    TextoNotificacao = texto,
                    Usuario = new BMUsuario().ObterPorId(u.ID)
                });
            }
        }

        public void IncluirNotificacaoTrilha(UsuarioTrilha usuario, string texto, DateTime? data)
        {
            try
            {
                // Só inclui notificação caso exista um usuário vinculado
                if (usuario == null) throw new AcademicoException("Não foi possível incluir a notificação do usuário da trilha");

                notificacaoBM.IncluirNotificacaoTrilha(usuario, texto, data);
            }
            catch (Exception)
            {
            }
        }
    }
}
