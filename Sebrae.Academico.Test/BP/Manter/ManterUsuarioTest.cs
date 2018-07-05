using NUnit.Framework;
using NUnit.Framework.Constraints;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Test.BP.Manter
{
    [TestFixture]
    public class ManterUsuarioTest
    {
        /// <summary>
        /// Testa se as validações de usuário estão funcionando
        /// </summary>
        [Test]
        public void ValidarDadosDeLogin()
        {
            var usuario = new Usuario
            {
                CPF = "04570064124",
                Senha = ""
            };

            ActualValueDelegate<Usuario> testeLoginSenha = () => new ManterUsuario().Login(usuario.CPF, usuario.Senha);
            Assert.That(testeLoginSenha, Throws.TypeOf<AcademicoException>());
        }
    }
}
