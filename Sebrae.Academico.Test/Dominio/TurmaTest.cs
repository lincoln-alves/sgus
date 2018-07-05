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
    public class TurmaTest
    {
        private Turma turma;
        private Turma turmaFechada;

        [SetUp]
        protected void Setup()
        {
            turma = new Turma
            {
                ID = 1,
                Nome = "TURMA 1",
                DataInicio = DateTime.Today.AddDays(1),
                DataFinal = DateTime.Today.AddDays(10)
            };

            turmaFechada = new Turma
            {
                ID = 1,
                Nome = "TURMA 2",
                DataInicio = DateTime.Today.AddDays(-30),
                DataFinal = DateTime.Today.AddDays(-10)
            };
            
        }

        [Test]
        public void VerificaSeTurmaVigente()
        {
            Assert.IsTrue(turma.IsVigente());
        }

        [Test]
        public void VerificaSeTurmaNaoVigente()
        {
            Assert.IsFalse(turmaFechada.IsVigente());
        }

        
    }
}
