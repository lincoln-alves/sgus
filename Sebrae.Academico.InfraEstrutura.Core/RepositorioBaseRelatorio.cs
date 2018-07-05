using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using System.Collections;
using Sebrae.Academico.InfraEstrutura.Core.UnitOfWork;

namespace Sebrae.Academico.InfraEstrutura.Core
{
    //classe definida nesta declaração é uma classe dummy, servindo apenas para inicialização do repositório.
    public class RepositorioBaseRelatorio
    {

        private RepositorioBase<Trilha> repositorio;
        public RepositorioBaseRelatorio(IUnitOfWork unitOfWork)
        { 
            repositorio = new RepositorioBase<Trilha>(unitOfWork);
        }
        

        

        public IList ExecuteProcedure(string pProcedureName, IDictionary<string, object> pListaParametros)
        {

            string strExec = "EXEC " + pProcedureName + " ";

            foreach (var param in pListaParametros)
            {
                strExec += "@" + param.Key + "=:" + param.Key + ",";
            }

            strExec = strExec.Substring(0, strExec.Length - 1);




            var execSP = repositorio.Session.CreateSQLQuery(strExec);


            foreach (var obj in pListaParametros)
            {
                execSP.SetParameter(obj.Key, obj.Value);
            }

            return execSP.List();
        }
    }
}
