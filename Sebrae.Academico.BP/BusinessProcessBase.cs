using System;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP.AutoMapper;

namespace Sebrae.Academico.BP
{
    /// <summary>
    /// Classe base para as classes da chamada de Business Process.
    /// </summary>
    public abstract class BusinessProcessBase
    {
        protected string MensagemErro = "Ocorreu um Erro no Sistema";
        
        protected BusinessProcessBase()
        {
            AutoMapperConfig.RegisterMappings();
        }

        protected void PreencherInformacoesDeAuditoria(EntidadeBasicaPorId entidade)
        {
            entidade.Auditoria.UsuarioAuditoria = ObterCpfDoUsuarioLogado();
            entidade.Auditoria.DataAuditoria = DateTime.Now;
        }

        protected string ObterCpfDoUsuarioLogado()
        {
            return new BMUsuario().ObterUsuarioLogado()?.CPF;
        }
        
    }
}
