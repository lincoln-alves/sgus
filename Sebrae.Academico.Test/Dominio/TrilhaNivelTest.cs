using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Test.Dominio
{
    [TestFixture]
    public class TrilhaNivelTest
    {
        private TrilhaNivel nivel;

        [SetUp]
        public void Init()
        {
            var nivel = new TrilhaNivel
            {
                Nome = "Gestão de Risco"
            };

            var pontosSebrae = new List<PontoSebrae>
            {
                new PontoSebrae
                {
                    Nome = "Conhecimento",
                    Ativo = false
                },
                new PontoSebrae
                {
                    Nome = "Gestão de Risco",
                    Ativo = true
                },
                new PontoSebrae
                {
                    Nome = "Empreendedorismo",
                    Ativo = false
                },
                new PontoSebrae
                {
                    Nome = "Empreendedorismo",
                    Ativo = true
                }
            };

            nivel.ListaPontoSebrae = pontosSebrae;

            this.nivel = nivel;
        }

        [Test]
        public void ValidarPontosSebraeAtivos()
        {
            Assert.AreEqual(2, nivel.ObterPontosSebraeAtivos().ToList().Count);
        }
    }
}