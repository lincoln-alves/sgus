using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPosicaoDadoCertificadoCertame : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<PosicaoDadoCertificadoCertame> _repositorio;

        public BMPosicaoDadoCertificadoCertame()
        {
            _repositorio = new RepositorioBase<PosicaoDadoCertificadoCertame>();
        }

        public PosicaoDadoCertificadoCertame ObterPorDadoAno(string dado, int ano)
        {
            return _repositorio.ObterTodosIQueryable().FirstOrDefault(x => x.Dado == dado && x.Ano == ano);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}