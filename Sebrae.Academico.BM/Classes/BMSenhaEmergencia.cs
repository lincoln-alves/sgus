using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSenhaEmergencia : BusinessManagerBase
    {

        private RepositorioBase<SenhaEmergencia> repositorio;

        public BMSenhaEmergencia()
        {
            repositorio = new RepositorioBase<SenhaEmergencia>();
        }
       

        public SenhaEmergencia ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<SenhaEmergencia> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        protected override bool ValidarDependencias(object pTag)
        {
            return true;
        }

        public void Excluir(SenhaEmergencia pSenhaEmergencia)
        {
            repositorio.Excluir(pSenhaEmergencia);
        }


        public void Salvar(SenhaEmergencia se)
        {
            //se.Senha = WebFormHelper.ObterHashMD5(se.Senha);
            se.Senha = CriptografiaHelper.Criptografar(se.Senha);
            repositorio.Salvar(se);
        }

        public void ExpirarSenhaAtual()
        {
            SenhaEmergencia ex = repositorio.session.Query<SenhaEmergencia>().OrderByDescending(x => x.DataValidade).FirstOrDefault<SenhaEmergencia>();

            if (ex != null)
            {
                ex.DataValidade = DateTime.Now;
                repositorio.Salvar(ex);
            }
        }

        public SenhaEmergencia ObterSenhaAtiva()
        {
            var query = repositorio.session.Query<SenhaEmergencia>();
            SenhaEmergencia se = query.OrderByDescending(x => x.DataValidade).FirstOrDefault();

            if (se == null || se.DataValidade <= DateTime.Now)
                throw new AcademicoException("Usuário ou senha Inválidos !");

            return se;

        }
    }
}
