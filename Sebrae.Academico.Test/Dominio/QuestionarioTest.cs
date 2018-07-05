using NUnit.Framework;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Test.Dominio
{
    [TestFixture]
    public class QuestionarioTest
    {

        /// <summary>
        /// Teste unitario
        /// 
        /// Para usuários gestores 
        /// quando o estado do questionário for diferente do estado que é vincualdo ao usuário 
        /// não é possível editar o questionário
        /// 
        /// </summary>
        [Test]
        public void ValidarEdicaoQuestionarioComPorGestorUfsDiferentes()
        {
            var listaPerfil = new List<UsuarioPerfil>();

            var usuario = new Usuario
            {
                UF = new Uf
                {
                    ID = (int)enumUF.NA
                }
            };

            var questionario = new Questionario
            {
                Uf = new Uf
                {
                    ID = (int)enumUF.MG
                }
            };

            usuario.ListaPerfil.Add(new UsuarioPerfil
            {
                Usuario = usuario,
                Perfil = new Perfil
                {
                    ID = (int)enumPerfil.GestorUC
                }
            });

            Assert.IsFalse(questionario.TratarEdicaoQuestionario(usuario));
        }

        /// <summary>
        /// Teste unitario
        /// 
        /// Para usuários gestores 
        /// quando o estado do questionário for igual do estado que é vincualdo ao usuário 
        /// é possível editar o questionário
        /// 
        /// </summary>
        [Test]
        public void ValidarEdicaoQuestionarioPorGestorUF()
        {
            var listaPerfil = new List<UsuarioPerfil>();

            var usuario = new Usuario
            {
                UF = new Uf
                {
                    ID = (int)enumUF.NA
                }
            };

            var questionario = new Questionario
            {
                Uf = new Uf
                {
                    ID = (int)enumUF.NA
                }
            };

            usuario.ListaPerfil.Add(new UsuarioPerfil
            {
                Usuario = usuario,
                Perfil = new Perfil
                {
                    ID = (int)enumPerfil.GestorUC
                }
            });

            Assert.IsTrue(questionario.TratarEdicaoQuestionario(usuario));
        }

        [Test]
        public void DeveRetornarUmaNovaInstaciaDeQuestionarioComNomeETipoQuestionario ()
        {
            var questionario = new Questionario();
            questionario.Nome = "Question 01";
            questionario.TipoQuestionario = new TipoQuestionario();
            var newQuestionario = questionario.NovoQuestionario(questionario);
            Assert.That(Object.ReferenceEquals(newQuestionario, questionario), "Não deve Ser a Mesma Referencia");
            Assert.True(newQuestionario.Nome == questionario.Nome);
            Assert.True(newQuestionario.TipoQuestionario == questionario.TipoQuestionario);
        }
    }
}
