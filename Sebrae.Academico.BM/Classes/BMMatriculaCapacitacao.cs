using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMatriculaCapacitacao : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        RepositorioBase<MatriculaCapacitacao> repositorio;

        #endregion

        #region "Construtor"

        public BMMatriculaCapacitacao()
        {
            repositorio = new RepositorioBase<MatriculaCapacitacao>();
        }

        public IList<MatriculaCapacitacao> ObterUsuariosPorCapacitacao(int idUsuario, int idCapacitacao)
        {
            var query = repositorio.session.Query<MatriculaCapacitacao>();
            query = query.Where(x => x.Capacitacao.ID == idCapacitacao);
            query = query.Where(x => x.Usuario.ID == idUsuario);

            return query.ToList<MatriculaCapacitacao>();
        }

        #endregion

        #region "Atributos Privados"

        private void ValidarRegistroMatriculaEnviado(MatriculaCapacitacao pMatriculaCapacitacao)
        {
            this.ValidarInstancia(pMatriculaCapacitacao);

            if (pMatriculaCapacitacao.Capacitacao == null || pMatriculaCapacitacao.Capacitacao.ID == 0)
                throw new AcademicoException("Capacitação não informada. Campo Obrigatório!");

            if (pMatriculaCapacitacao.Usuario == null || pMatriculaCapacitacao.Usuario.ID == 0)
                throw new AcademicoException("Usuário não informado. Campo Obrigatório!");

            if (pMatriculaCapacitacao.UF == null || pMatriculaCapacitacao.UF.ID == 0)
                throw new AcademicoException("UF não informado. Campo Obrigatório!");

            if (pMatriculaCapacitacao.StatusMatricula == null)
                throw new AcademicoException("Status da Matrícula não informado. Campo Obrigatório!");

            if (pMatriculaCapacitacao.NivelOcupacional == null || pMatriculaCapacitacao.NivelOcupacional.ID == 0)

                throw new AcademicoException("Nível Ocupacional não informado. Campo Obrigatório!");

            if (pMatriculaCapacitacao.ID == 0)
            {

                bool registroDuplicado = VerificarDuplicidadeDeRegistroChave(pMatriculaCapacitacao);

                if (registroDuplicado)
                {
                    throw new AcademicoException("O usuário informado encontra-se matriculado neste programa.");
                }
            }




        }

        #endregion

        #region "Métodos Públicos"

        public bool VerificarDuplicidadeDeRegistroChave(MatriculaCapacitacao pMatriculaCapacitacao)
        {

            bool usuarioJaMatriculado = false;

            var query = repositorio.session.Query<MatriculaCapacitacao>();
            usuarioJaMatriculado = query.Any(x => x.Usuario.ID == pMatriculaCapacitacao.Usuario.ID &&
                                                  x.Capacitacao.ID == pMatriculaCapacitacao.Capacitacao.ID);

            return usuarioJaMatriculado;
        }

        public IList<MatriculaCapacitacao> ObterPorFiltros(MatriculaCapacitacao pMatriculaCapacitacao, bool vigente = false)
        {
            var query = repositorio.session.Query<MatriculaCapacitacao>();

            if (pMatriculaCapacitacao.ID != 0)
                query = query.Where(x => x.ID == pMatriculaCapacitacao.ID);
            if (pMatriculaCapacitacao.NivelOcupacional != null)
                query = query.Where(x => x.NivelOcupacional.ID == pMatriculaCapacitacao.NivelOcupacional.ID);
            if (pMatriculaCapacitacao.Capacitacao != null && pMatriculaCapacitacao.Capacitacao.ID != 0)
                query = query.Where(x => x.Capacitacao.ID == pMatriculaCapacitacao.Capacitacao.ID);
            if (pMatriculaCapacitacao.StatusMatricula != null)
                query = query.Where(x => x.StatusMatricula == pMatriculaCapacitacao.StatusMatricula);
            if (pMatriculaCapacitacao.UF != null)
                query = query.Where(x => x.UF.ID == pMatriculaCapacitacao.UF.ID);
            if (pMatriculaCapacitacao.Usuario != null)
                query = query.Where(x => x.Usuario.ID == pMatriculaCapacitacao.Usuario.ID);
            if (vigente)
            {
                query = query.Where(x => (x.Capacitacao.DataInicio <= DateTime.Today && x.Capacitacao.DataFim >= DateTime.Today));
            }


            query = query.Fetch(x => x.NivelOcupacional);
            query = query.Fetch(x => x.Capacitacao);
            query = query.Fetch(x => x.Usuario);


            return query.ToList();



        }

        public IList<MatriculaCapacitacao> ObterPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<MatriculaCapacitacao>();
            query = query.Where(x => x.Usuario.ID == idUsuario);
            return query.ToList<MatriculaCapacitacao>();
        }

        public IList<MatriculaCapacitacao> ObterPorCapacitacao(int idCapacitacao)
        {
            var query = repositorio.session.Query<MatriculaCapacitacao>();
            query = query.Where(x => x.Capacitacao.ID == idCapacitacao);
            return query.ToList<MatriculaCapacitacao>();
        }

        public IList<MatriculaCapacitacao> ObterUsuariosPorPrograma(int idCapacitacao, string filtro, string CPF)
        {
            var query = repositorio.session.Query<MatriculaCapacitacao>();
            query = query.Where(x => x.Capacitacao.ID == idCapacitacao);

            if (!string.IsNullOrWhiteSpace(filtro))
                query = query.Where(x => x.Usuario.Nome.Contains(filtro));

            if (!string.IsNullOrWhiteSpace(CPF))
                query = query.Where(x => x.Usuario.CPF.Contains(CPF));

            return query.ToList<MatriculaCapacitacao>();
        }

        public MatriculaCapacitacao ObterPorId(int idMatricula)
        {
            return repositorio.ObterPorID(idMatricula);
        }

        public void RegistrarMatricula(MatriculaCapacitacao pMatriculaCapacitacao)
        {
            ValidarRegistroMatriculaEnviado(pMatriculaCapacitacao);
            //pMatriculaPrograma.DataAlteracao = DateTime.Now;

            repositorio.Salvar(pMatriculaCapacitacao);

        }

        public void Salvar(MatriculaCapacitacao pMatriculaCapacitacao)
        {
            if (pMatriculaCapacitacao.ID == 0 && pMatriculaCapacitacao.Usuario != null)
            {
                pMatriculaCapacitacao.NivelOcupacional = pMatriculaCapacitacao.Usuario.NivelOcupacional;
                pMatriculaCapacitacao.UF = pMatriculaCapacitacao.Usuario.UF;
            }

            ValidarRegistroMatriculaEnviado(pMatriculaCapacitacao);

            repositorio.Salvar(pMatriculaCapacitacao);
        }

        public void Excluir(MatriculaCapacitacao pMatriculaCapacitacao)
        {
            repositorio.Excluir(pMatriculaCapacitacao);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        #endregion
    }
}
