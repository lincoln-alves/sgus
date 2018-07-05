using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterAbandono : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMManterAbandono abandono = null;

        #endregion

        #region "Construtor"

        public ManterAbandono()
            : base()
        {
            abandono = new BMManterAbandono();
        }

        #endregion

        #region "Métodos Públicos"

        /// <summary>
        /// Obtém informações sobre os abandonos do usuário.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public IList<UsuarioAbandono> ObterPorUsuario(int idUsuario)
        {

            IList<UsuarioAbandono> listaUsuarioAbandono = null;

            try
            {
                listaUsuarioAbandono = abandono.ObterPorUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaUsuarioAbandono;
        }

        public UsuarioAbandono ObterPorID(int pId)
        {
            UsuarioAbandono usuarioAbandono = null;

            try
            {
                usuarioAbandono = abandono.ObterPorID(pId);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return usuarioAbandono;
        }

        public void AtualizarUsuarioAbandono(UsuarioAbandono usuarioAbandono)
        {
            try
            {
                abandono.Salvar(usuarioAbandono);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public DateTime? VerificarExistenciaDeAbandonoValido(Usuario usuario)
        {
            try
            {
                UsuarioAbandono registroAbandono = abandono.VerificarExistenciaDeAbandonoValido(usuario);
                if (registroAbandono != null)
                    return registroAbandono.DataFimAbandono;
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return null;
        }

        #endregion
    }
}

