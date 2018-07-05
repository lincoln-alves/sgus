using NUnit.Framework;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.Test.Dominio
{
    [TestFixture]
    public class ItemTrilhaTest
    {
        private ItemTrilha itemTrilha;
        private UsuarioTrilha usuarioTrilha;
        List<ItemTrilhaAvaliacao> avaliacoes;
        int somaValores = 0;
        int totalAvaliacoes = 5;

        [SetUp]
        protected void Setup()
        {
            itemTrilha = new ItemTrilha();
            usuarioTrilha = new UsuarioTrilha()
            {
                ID = 1
            };
            avaliacoes = new List<ItemTrilhaAvaliacao>();

            for (int i = 0; i < totalAvaliacoes; i++)
            {
                avaliacoes.Add(new ItemTrilhaAvaliacao("", i, usuarioTrilha, itemTrilha));
                somaValores += i;
            }

            itemTrilha.Avaliacoes = avaliacoes;
        }

        [Test]
        public void ObterMediaAvaliacoes()
        {
            var mediaAvaliacoes = itemTrilha.ObterMediaAvaliacoes();
            Assert.GreaterOrEqual(mediaAvaliacoes, somaValores / totalAvaliacoes);
        }

        [Test]
        public void ChecarAvaliacao()
        {
            var usuario = new UsuarioTrilha
            {
                ID = 1
            };

            Assert.IsTrue(itemTrilha.ChecarAvaliacao(usuario));
        }
    }
}
