using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMFaleConosco : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<LogFaleConosco> repositorio;


        public BMFaleConosco()
        {
            repositorio = new RepositorioBase<LogFaleConosco>();
        }

        public LogFaleConosco ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        /// <summary>
        /// Obtém as mensagens enviadas por um usuário.
        /// </summary>
        /// <param name="pCPF">CPF do usuário que enviou a mensagem</param>
        /// <returns>Lista de mensagens do usuário</returns>
        public IList<LogFaleConosco> ListarPorCPF(string pCPF)
        {
            if (string.IsNullOrWhiteSpace(pCPF)) throw new AcademicoException("CPF não informado. Informe o CPF");
            
            var query = repositorio.session.Query<LogFaleConosco>();
            return query.Where(x => x.CPF == pCPF).ToList<LogFaleConosco>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(LogFaleConosco lgFc)
        {
            repositorio.Salvar(lgFc);
        }
    }
}
