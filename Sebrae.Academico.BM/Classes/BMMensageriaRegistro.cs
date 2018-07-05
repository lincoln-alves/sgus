using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMensageriaRegistro : BusinessManagerBase
    {
        #region "Atributos Privados"

        private RepositorioBase<MensageriaRegistro> repositorio;

        #endregion

        #region "Construtor"

        public BMMensageriaRegistro()
        {
            repositorio = new RepositorioBase<MensageriaRegistro>();
        }

        #endregion

        #region "Métodos Privados"

        private void ValidarMensageriaRegistroEnviados(MensageriaRegistro mr)
        {
            ValidarInstancia(mr);
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(MensageriaRegistro mr)
        {
            ValidarMensageriaRegistroEnviados(mr);
            repositorio.Salvar(mr);
        }

        public bool ValidarComunicacaoEfetuada(MatriculaTurma mt)
        {
            return (repositorio.session.Query<MensageriaRegistro>().Where(x => x.MatriculaTurma.ID == mt.ID &&
                                                                              x.DataEnvio.Date <= DateTime.Now.Date).FirstOrDefault() != null);
        }

        public bool ValidarComunicacaoEfetuada(UsuarioTrilha ut)
        {
            return (repositorio.session.Query<MensageriaRegistro>().Where(x => x.DataEnvio.Date <= DateTime.Now.Date &&
                                                                               x.UsuarioTrilha.ID == ut.ID).FirstOrDefault() != null);
        }

        #endregion
    }
}
