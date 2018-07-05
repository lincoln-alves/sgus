using Newtonsoft.Json;
using NUnit.Framework;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.ManterSolucaoEducacionalService;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Test.BP.Service
{
    // TestFixture indica que esta classe contem testes.
    [TestFixture]
    public class TreinamentoSASTest
    {

        /// <summary>
        /// Teste de integração
        /// 
        /// Testa se a validação do campo SID está funcionando corretamente
        /// </summary>
        [Test]
        public void ValidacaoCadastroSAS_SemSID()
        {
            var manter = new ManterSolucaoEducacionalService();

            // Usuario com SSID inválido
            var usuario = new Usuario
            {
                CPF = "04482645109",
                DataNascimento = new DateTime(1991, 1, 14),
                Nome = "VILSON PEREIRA DOS SANTOS",
                Sexo = "1",
                Email = "vilson.santos@mt.sebrae.com.br",
                SID_Usuario = "S-1-5-21-881038022-2994736296-2543795088-13118",
                TelefoneExibicao = "6592777831",
                UF = new Uf
                {
                    ID = 1,
                    Nome = "Nacional"
                }
            };

            try
            {
                manter.CadastrarSAS(usuario, new Academico.BP.integracaoSAS.TreinamentoSasClient());
            }
            catch (Exception e)
            {
                Assert.That(e.Message.Contains("SSID"));
            }
        }
    }
}

