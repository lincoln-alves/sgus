using System.Collections.Generic;
using Nancy;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using JWT;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class GenericModule : BaseModule
    {
        public readonly ManterUsuarioTrilha _manterUsuarioTrilha = new ManterUsuarioTrilha();

        /// <summary>
        /// Módulo base para os outros, caso seja necessário customizar algo compartilhado.
        /// </summary>
        public GenericModule() { }

        /// <summary>
        /// Módulo base para os outros, caso seja necessário customizar algo compartilhado.
        /// </summary>
        /// <param name="path"></param>
        public GenericModule(string path) : base(path) { }

        public UserIdentity AcessoAtual
        {
            get
            {
                UserIdentity retorno = null;

                if (Context != null && Context.CurrentUser != null)
                {
                    retorno = (UserIdentity)Context.CurrentUser;

                    // Força a atualização dos dados da sessão.
                    // Feio mas funciona.
                    retorno.Nivel = retorno.Nivel != null
                        ? new ManterTrilhaNivel().ObterTrilhaNivelPorID(retorno.Nivel.ID)
                        : null;
                    retorno.Usuario = retorno.Usuario != null
                        ? new ManterUsuario().ObterUsuarioPorID(retorno.Usuario.ID)
                        : null;
                    retorno.Matricula = retorno.Nivel != null && retorno.Usuario != null
                        ? new ManterUsuarioTrilha().ObterPorUsuarioNivel(retorno.Usuario.ID, retorno.Nivel.ID)
                        : null;
                    if(retorno.Matricula == null)
                    {
                        var payload = JsonWebToken.DecodeToObject(retorno.JwtToken, "", false) as IDictionary<string, object>;
                        if (payload["experimenta"].Equals("experimente"))
                        {
                            retorno.Matricula = new Dominio.Classes.UsuarioTrilha
                            {
                                ID = 0,
                                TrilhaNivel = retorno.Nivel,
                                StatusMatricula = Dominio.Enumeracao.enumStatusMatricula.Inscrito
                            };
                        }
                    }
                    retorno.Fornecedor = retorno.Fornecedor != null
                        ? new ManterFornecedor().ObterFornecedorPorID(retorno.Fornecedor.ID)
                        : null;
                }

                return retorno;
            }
        }

        public void VerificarBloqueio()
        {
            List<enumStatusMatricula> Permitidos = new List<enumStatusMatricula>
            {
                enumStatusMatricula.Inscrito,
                enumStatusMatricula.Aprovado
            };

            AcessoAtual.Matricula = _manterUsuarioTrilha.AtualizarStatusComRegras(AcessoAtual.Matricula);

            if (!Permitidos.Contains(AcessoAtual.Matricula.StatusMatricula))
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, string.Format("Acesso não permitido para usuários com o status {0}", AcessoAtual.Matricula.StatusMatriculaFormatado));

            if (AcessoAtual.Matricula.Cronometro.TotalSeconds <= 0)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Você não pode mais executar esta ação, seu tempo, acabou, obrigado pela participação!");
        }
    }
}