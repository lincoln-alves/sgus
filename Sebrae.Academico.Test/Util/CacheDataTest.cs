using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sebrae.Academico.BP;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.Test.Util
{
    [TestFixture]
    public class CacheDataTest
    {
        [Test]
        public void CicloCompletoDadoPrimitivoString()
        {
            var cache = new CacheData("TipoPrimitivoString", 1);
            var testString = "Teste";

            cache.SetCacheData(testString, typeof(string) );

            Assert.AreEqual(cache.HasValidCacheData(), true);
            Assert.AreEqual(cache.GetCacheData(), testString);

            // Remove o cache feito
            cache.PurgeCache();
        }

        [Test]
        public void CicloCompletoDadoPrimitivoInteger()
        {
            var cache = new CacheData("TipoPrimitivoInteger", 1);
            var testString = 32;

            cache.SetCacheData(testString, typeof(int));

            Assert.AreEqual(cache.HasValidCacheData(), true);
            Assert.AreEqual(cache.GetCacheData(), testString);

            // Remove o cache feito
            cache.PurgeCache();
        }

        [Test]
        public void CicloCompletoDadoComplexData()
        {
            var cache = new CacheData("TipoComplexo", 1);
            var OriginalDtoTest = new DtoTest()
            {
                CreatedAt = DateTime.Now,
                Name = "Teste",
                SubTestsList = new List<DtoSubTest>()
                {
                    new DtoSubTest()
                    {
                        Name = "SubTeste1"
                    },
                    new DtoSubTest()
                    {
                        Name = "SubTeste2"
                    }
                }
            };

            cache.SetCacheData(OriginalDtoTest, typeof(DtoTest));

            Assert.AreEqual(cache.HasValidCacheData(), true);

            DtoTest cachedData = cache.GetCacheData();
            
            // Dados estão integros após deserialização?
            Assert.AreEqual(OriginalDtoTest.CreatedAt, cachedData.CreatedAt); // Importante por problemas de timezone
            Assert.AreEqual(OriginalDtoTest.Name, cachedData.Name);
            Assert.AreEqual(OriginalDtoTest.SubTestsList[0].Name, cachedData.SubTestsList[0].Name);
            Assert.AreEqual(OriginalDtoTest.SubTestsList[1].Name, cachedData.SubTestsList[1].Name);

            // Remove o cache feito
            cache.PurgeCache();
        }

        [Test]
        public void TestCacheExpireDate()
        {
            var cache = new CacheData("ExpireCache", -1);
            var testString = "Teste";

            cache.SetCacheData(testString, typeof(string));

            // Test Duration
            // Expired
            Assert.AreEqual(cache.HasValidCacheData(), false);

            // Test explicit expires
            // Not Expired
            cache.SetCacheData(testString, typeof(string), DateTime.Now.AddHours(1));
            Assert.AreEqual(cache.HasValidCacheData(), true);

            // Expired
            cache.SetCacheData(testString, typeof(string), DateTime.Now.AddHours(-1));
            Assert.AreEqual(cache.HasValidCacheData(), false);

            // Remove o cache feito
            cache.PurgeCache();
        }
    }

    class DtoSubTest
    {
        public string Name { get; set; }
    }

    class DtoTest
    {
        public IList<DtoSubTest> SubTestsList { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

