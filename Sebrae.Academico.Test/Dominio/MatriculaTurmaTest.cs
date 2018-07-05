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
    public class MatriculaTurmaTest
    {
        private MatriculaTurma matriculaTurma;
        private MatriculaOferta matriculaOferta;
        private Oferta oferta;
        private Turma turma;

        private MatriculaTurma matriculaTurmaFechada;
        private Turma turmaFechada;

        [SetUp]
        protected void Setup()
        {
            oferta = new Oferta
            {
                ID = 1,
                DiasPrazo = 5,
                TipoOferta = Academico.Dominio.Enumeracao.enumTipoOferta.Continua,
                Nome = "Oferta Test"
            };

            turma = new Turma
            {
                ID = 1,
                Nome = "TURMA 1",
                DataInicio = DateTime.Today.AddDays(-1),
                DataFinal = DateTime.Today.AddDays(10),
                Oferta = oferta
            };

            matriculaOferta = new MatriculaOferta
            {
                ID = 1,
                Oferta = oferta
            };

            matriculaTurma = new MatriculaTurma
            {
                ID = 1,
                DataMatricula = DateTime.Today,
                DataLimite = DateTime.Today.AddDays(10),
                DataTermino = DateTime.Today.AddDays(5),
                MatriculaOferta = matriculaOferta,
                Turma = turma
            };

            turmaFechada = new Turma
            {
                ID = 2,
                Nome = "TURMA 2",
                DataInicio = DateTime.Today.AddDays(-30),
                DataFinal = DateTime.Today.AddDays(-10),
                Oferta = oferta
            };


            matriculaTurmaFechada = new MatriculaTurma
            {
                ID = 2,
                DataMatricula = DateTime.Today.AddDays(-10),
                DataLimite = DateTime.Today.AddDays(10),
                DataTermino = DateTime.Today.AddDays(5),
                MatriculaOferta = matriculaOferta,
                Turma = turmaFechada
            };
        }

        [Test]
        public void ValidarCalculoDataLimiteMatriculaTurma()
        {
            Assert.IsTrue((matriculaTurma.CalcularDataLimite(oferta) - DateTime.Today).Days >= 0);
            Assert.IsTrue((matriculaTurma.CalcularDataLimite() - DateTime.Today).Days >= 0);

        }

        [Test]
        public void ValidarCalculoDataLimiteMatriculaTurmaVencida()
        {
            Assert.IsFalse((matriculaTurmaFechada.CalcularDataLimite() - DateTime.Today).Days >= 0);
        }
    }
}
