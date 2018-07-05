using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes{
    public class BMLogSincronia : BusinessManagerBase, IDisposable{
        private readonly RepositorioBase<LogSincronia> _repositorio;

        public BMLogSincronia(){
            _repositorio = new RepositorioBase<LogSincronia>();
        }

        public void Dispose(){
            _repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(LogSincronia model){
            _repositorio.Salvar(model);
        }

        public void Salvar(IList<LogSincronia> list){
            _repositorio.Salvar(list);
        }

        public IList<LogSincronia> ObterTodos(){
            return _repositorio.ObterTodos();
        }

        public LogSincronia ObterPorID(int pId){
            return _repositorio.session.Query<LogSincronia>().FirstOrDefault(x => x.ID == pId);
        }

        public LogSincronia ObterPorFiltro(LogSincronia model){
            var query = _repositorio.session.Query<LogSincronia>();
            if (model == null) return null;
            if (!string.IsNullOrEmpty(model.Hash)) query = query.Where(p => p.Hash.Equals(model.Hash));
            if (model.Usuario != null) query = query.Where(p => p.Usuario.ID == model.Usuario.ID);
            return query.FirstOrDefault();
        }
    }
}