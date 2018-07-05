using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.Test.BP.Manter
{
    public class ManterPontoSebraeTest
    {
        [Test]
        public void ObterPontosSebraeAtivos()
        {
            var pontosSebrae = new ManterPontoSebrae().ObterAtivos().ToList();

            pontosSebrae.ForEach(x => Assert.True(x.Ativo));
        }
    }
}

