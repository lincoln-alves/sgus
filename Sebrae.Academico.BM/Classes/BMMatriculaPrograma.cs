using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMatriculaPrograma : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        RepositorioBase<MatriculaPrograma> repositorio;

        #endregion

        #region "Construtor"

        public BMMatriculaPrograma()
        {
            repositorio = new RepositorioBase<MatriculaPrograma>();
        }

        #endregion

        #region "Atributos Privados"

        private void ValidarRegistroMatriculaEnviado(MatriculaPrograma pMatriculaPrograma)
        {
            this.ValidarInstancia(pMatriculaPrograma);

            if (pMatriculaPrograma.Programa == null || pMatriculaPrograma.Programa.ID == 0)
                throw new AcademicoException("Programa não informado. Campo Obrigatório!");

            if (pMatriculaPrograma.Usuario == null || pMatriculaPrograma.Usuario.ID == 0)
                throw new AcademicoException("Usuário não informado. Campo Obrigatório!");

            if (pMatriculaPrograma.UF == null || pMatriculaPrograma.UF.ID == 0)
                throw new AcademicoException("UF não informado. Campo Obrigatório!");

            if (pMatriculaPrograma.StatusMatricula == null)
                throw new AcademicoException("Status da Matrícula não informado. Campo Obrigatório!");

            if (pMatriculaPrograma.NivelOcupacional == null || pMatriculaPrograma.NivelOcupacional.ID == 0)

                throw new AcademicoException("Nível Ocupacional não informado. Campo Obrigatório!");

            if (pMatriculaPrograma.ID == 0)
            {

                bool registroDuplicado = VerificarDuplicidadeDeRegistroChave(pMatriculaPrograma);

                if (registroDuplicado)
                {
                    throw new AcademicoException("O usuário informado encontra-se matriculado neste programa.");
                }
            }




        }

        #endregion

        #region "Métodos Públicos"

        public bool VerificarDuplicidadeDeRegistroChave(MatriculaPrograma pMatriculaPrograma)
        {

            bool usuarioJaMatriculado = false;

            var query = repositorio.session.Query<MatriculaPrograma>();
            usuarioJaMatriculado = query.Any(x => x.Usuario.ID == pMatriculaPrograma.Usuario.ID &&
                                                  x.Programa.ID == pMatriculaPrograma.Programa.ID);

            return usuarioJaMatriculado;
        }

        public IList<MatriculaPrograma> ObterPorFiltros(MatriculaPrograma pMatriculaPrograma, bool vigente = false)
        {
            var query = repositorio.session.Query<MatriculaPrograma>();

            if (pMatriculaPrograma.ID != 0)
                query = query.Where(x => x.ID == pMatriculaPrograma.ID);

            if (pMatriculaPrograma.NivelOcupacional != null)
                query = query.Where(x => x.NivelOcupacional.ID == pMatriculaPrograma.NivelOcupacional.ID);

            if (pMatriculaPrograma.Programa != null)
                query = query.Where(x => x.Programa.ID == pMatriculaPrograma.Programa.ID);

            if (pMatriculaPrograma.StatusMatricula != 0)
                query = query.Where(x => x.StatusMatricula == pMatriculaPrograma.StatusMatricula);

            if (pMatriculaPrograma.UF != null)
                query = query.Where(x => x.UF.ID == pMatriculaPrograma.UF.ID);

            if (pMatriculaPrograma.Usuario != null)
                query = query.Where(x => x.Usuario.ID == pMatriculaPrograma.Usuario.ID);

            if (vigente)
            {
                query =
                    query.Where(
                        x =>
                            x.Programa.ListaCapacitacao.Any(
                                c =>
                                    c.ListaTurmas.Any(
                                        t => t.DataInicio.HasValue && t.DataFim.HasValue && t.DataInicio <= DateTime.Today && t.DataFim >= DateTime.Today)));
            }

            query = query.Fetch(x => x.NivelOcupacional);
            query = query.Fetch(x => x.Programa);
            query = query.Fetch(x => x.Usuario);

            return query.ToList();
        }

        public IList<MatriculaPrograma> ObterPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<MatriculaPrograma>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            return query.ToList();
        }

        public IList<MatriculaPrograma> ObterPorPrograma(int idPrograma)
        {
            var query = repositorio.session.Query<MatriculaPrograma>();
            query = query.Where(x => x.Programa.ID == idPrograma);
            return query.ToList<MatriculaPrograma>();
        }

        public IList<MatriculaPrograma> ObterUsuariosPorPrograma(int idPrograma, string filtro, string CPF)
        {
            var query = repositorio.session.Query<MatriculaPrograma>();
            query = query.Where(x => x.Programa.ID == idPrograma);

            if (!string.IsNullOrWhiteSpace(filtro))
                query = query.Where(x => x.Usuario.Nome.Contains(filtro));

            if (!string.IsNullOrWhiteSpace(CPF))
                query = query.Where(x => x.Usuario.CPF.Contains(CPF));

            return query.ToList<MatriculaPrograma>();
        }

        public MatriculaPrograma ObterPorId(int idMatricula)
        {
            return repositorio.ObterPorID(idMatricula);
        }

        public void RegistrarMatricula(MatriculaPrograma pMatriculaPrograma)
        {
            ValidarRegistroMatriculaEnviado(pMatriculaPrograma);
            //pMatriculaPrograma.DataAlteracao = DateTime.Now;

            repositorio.Salvar(pMatriculaPrograma);

        }

        public void Salvar(MatriculaPrograma pMatriculaPrograma)
        {
            if (pMatriculaPrograma.ID == 0 && pMatriculaPrograma.Usuario != null)
            {
                pMatriculaPrograma.NivelOcupacional = pMatriculaPrograma.Usuario.NivelOcupacional;
                pMatriculaPrograma.UF = pMatriculaPrograma.Usuario.UF;
            }

            ValidarRegistroMatriculaEnviado(pMatriculaPrograma);

            repositorio.Salvar(pMatriculaPrograma);
        }

        public void Excluir(MatriculaPrograma pMatriculaPrograma)
        {
            repositorio.Excluir(pMatriculaPrograma);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
