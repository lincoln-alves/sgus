using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;

namespace Sebrae.Academico.Services
{
    public partial class SgusWebService
    {
        private readonly TrilhaServices _trilhaServices = new TrilhaServices();

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOTrilhaNivel> ObterNiveisTrilhaPorUsuario(int trilhaId, int usuarioId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return _trilhaServices.ObterNiveisPorTrilhaUsuario(trilhaId, usuarioId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService InscreverUsuarioTrilhaNivel(int trilhaNivelId, int usuarioId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return _trilhaServices.InscreverUsuarioTrilhaNivel(trilhaNivelId, usuarioId, autenticacao.Login);
            }
            catch
            {
                return new RetornoWebService { Erro = 1, Mensagem = "Erro ao efetuar inscrição no nível da trilha." };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoTokenTrilha ObterTokenMapa(int usuarioId, int trilhaNivelId, string experimenta = null)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return _trilhaServices.ObterTokenMapa(usuarioId, trilhaNivelId, experimenta);
            }
            catch
            {
                return new RetornoTokenTrilha { Erro = 1, Mensagem = "Erro ao obter o token do mapa da trilha." };
            }
        }
    }
}