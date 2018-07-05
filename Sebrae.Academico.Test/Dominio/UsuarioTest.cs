using NUnit.Framework;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Test.Dominio
{
    [TestFixture]
    public class UsuarioTest
    {
        /// <summary>
        /// Testa liberação de acesso as plataformas da UC Sebrae (portal e Moodle) para os usuário do programa de demissão voluntária do Sebrae (PDI)
        /// 
        /// Regra: Estes usuários estão cadastrados na tabela [PDI_Registro] tabela criada por meio da demanda #1837 do redmine. 
        /// Caso não seja liberado o acesso quando o usuário tentar fazer login nos sistemas será emitido um alerta de usuário inativo.
        /// </summary>
        [Test]
        public void ValidarUsuarioAtivoPDI()
        {
            var usuario = new Usuario
            {
                // 1 ano atrás
                DataDemissao = new DateTime(DateTime.Now.Year - 1, 9, 3)
            };

            Assert.True(usuario.AtivoNoPDI());
        }

        /// <summary>
        /// Testa liberação de acesso as plataformas da UC Sebrae (portal e Moodle) para os usuário do programa de demissão voluntária do Sebrae (PDI)
        /// 
        /// Regra: Estes usuários estão cadastrados na tabela [PDI_Registro] tabela criada por meio da demanda #1837 do redmine. 
        /// Caso não seja liberado o acesso quando o usuário tentar fazer login nos sistemas será emitido um alerta de usuário inativo.
        /// </summary>
        [Test]
        public void ValidarUsuarioInativoPDI()
        {
            var usuario = new Usuario
            {
                // 1 ano atrás
                DataDemissao = new DateTime(DateTime.Now.Year - 3, 9, 3)
            };

            Assert.False(usuario.AtivoNoPDI());
        }
    }
}
