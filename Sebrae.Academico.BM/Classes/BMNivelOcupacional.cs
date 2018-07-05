using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.BM.Mapeamentos;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMNivelOcupacional : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<NivelOcupacional> repositorio = null;

        #endregion

        #region Construtor

        public BMNivelOcupacional()
        {
            repositorio = new RepositorioBase<NivelOcupacional>();
        }

        #endregion

        public void Salvar(NivelOcupacional pNivelOcupacional)
        {
            ValidarNivelOcupacionalInformado(pNivelOcupacional);
            repositorio.Salvar(pNivelOcupacional);
        }

        private void VerificarExistenciaDeNivelOcupacional(NivelOcupacional pNivelOcupacional)
        {
            NivelOcupacional nivelOcupacional = this.ObterPorNome(pNivelOcupacional);

            if (nivelOcupacional != null)
            {
                if (pNivelOcupacional.ID != nivelOcupacional.ID)
                {
                    throw new AcademicoException(string.Format("O Nível Ocupacional '{0}' já está cadastrado", pNivelOcupacional.Nome));
                }
            }
        }

        private NivelOcupacional ObterPorNome(NivelOcupacional pNivelOcupacional)
        {
            NivelOcupacional nivelOcupacional = null;
            var query = repositorio.session.Query<NivelOcupacional>();
            nivelOcupacional = query.FirstOrDefault(x => x.Nome.Trim().ToUpper() == pNivelOcupacional.Nome);
            return nivelOcupacional;
        }

        public IList<NivelOcupacional> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList();
        }

        public NivelOcupacional ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<NivelOcupacional> ObterPorNome(string pNome)
        {
            return repositorio.LikeByProperty("Nome", pNome);
        }

        public IQueryable<NivelOcupacional> ObterObrigatorios()
        {
            return
                repositorio.session.Query<SolucaoEducacionalObrigatoria>()
                    .Select(x => x.NivelOcupacional)
                    .OrderBy(x => x.Nome)
                    .Distinct();
        }

        /// <summary>
        /// Validação das informações de um Nivel Ocupacional.
        /// </summary>
        /// <param name="pNivelOcupacional"></param>
        public void ValidarNivelOcupacionalInformado(NivelOcupacional pNivelOcupacional)
        {
            this.ValidarInstancia(pNivelOcupacional);

            if (string.IsNullOrWhiteSpace(pNivelOcupacional.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            this.VerificarExistenciaDeNivelOcupacional(pNivelOcupacional);

        }

        public void Excluir(NivelOcupacional pNivelOcupacional)
        {
            if (this.ValidarDependencias(pNivelOcupacional))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Nível Ocupacional.");

            repositorio.Excluir(pNivelOcupacional);
        }

        
        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<NivelOcupacional> ObterTodosIQueryable()
        {
            return repositorio.session.Query<NivelOcupacional>().AsQueryable();
        }
    }
}
