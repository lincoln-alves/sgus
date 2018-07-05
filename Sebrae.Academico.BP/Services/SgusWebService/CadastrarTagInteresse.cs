using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class CadastrarTagInteresse : BusinessProcessServicesBase
    {

        BMUsuarioTag usuarioTagBM;

        //public IList<DTOUsuarioTag> ConsultarTagInteresse(DTOUsuario pUsuario)
        //{
        //    usuarioTagBM = null;
        //    IList<DTOUsuarioTag> lstResult = null;

        //    try
        //    {

        //        usuarioTagBM = new BMUsuarioTag();
        //        lstResult = new List<DTOUsuarioTag>();

        //        foreach (UsuarioTag ust in usuarioTagBM.ObterPorFiltro(new UsuarioTag() { Usuario = pUsuario == null ? null : new Usuario() { ID = pUsuario.ID } }))
        //        {
        //            DTOUsuarioTag ustdto = new DTOUsuarioTag();
        //            CommonHelper.SincronizarDominioParaDTO(ust, ustdto);
        //            lstResult.Add(ustdto);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErroUtil.Instancia.TratarErro(ex);
        //    }
                        
        //    return lstResult;
        //}

        public void ExcluirTagInteresse(int pIdUsuarioTag)
        {

            usuarioTagBM = null;

            try
            {
                usuarioTagBM = new BMUsuarioTag();
                UsuarioTag ut = usuarioTagBM.ObterPorFiltro((new UsuarioTag() { ID = pIdUsuarioTag })).FirstOrDefault();

                if (ut.DataValidade.Value.Date <= DateTime.Now.Date)
                {
                    ut.Adicionado = false;
                    new BMUsuarioTag().Salvar(ut);
                }
                else
                    usuarioTagBM.Excluir(ut);

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

        }


        public enumOperacao AlternarTagInteresse(int IdUsuario, int IdTag, AuthenticationRequest autenticacao)
        {
            BMUsuarioTag bmUsuarioTag = new BMUsuarioTag();
            enumOperacao? EnumOperacao = null;

            try
            {
                bool usuarioTemATag = bmUsuarioTag.VerificarSeUsuarioPossuiTag(IdUsuario, IdTag);
      

                if (usuarioTemATag)
                {
                    bmUsuarioTag.Excluir(IdUsuario, IdTag);
                    EnumOperacao = enumOperacao.Exclusao;
                }
                else
                {
                    usuarioTagBM = new BMUsuarioTag();
                    UsuarioTag usuarioTag = new UsuarioTag()
                    {
                        ID = 0,
                        Auditoria = new Auditoria(autenticacao.Login),
                        DataValidade = null,
                        Adicionado = true
                    };
                    usuarioTag.Tag = new Tag();
                    usuarioTag.Tag.ID = IdTag;
                    usuarioTag.Usuario = new Usuario();
                    usuarioTag.Usuario.ID = IdUsuario;

                    usuarioTagBM.Salvar(usuarioTag);

                    EnumOperacao = enumOperacao.Inclusao;
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return EnumOperacao.Value; 
        }
    }
}
