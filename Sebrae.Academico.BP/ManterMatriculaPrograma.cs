using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterMatriculaPrograma : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMMatriculaPrograma bmMatriculaPrograma = null;

        #endregion

        #region "Construtor"

        public ManterMatriculaPrograma()
            : base()
        {
            bmMatriculaPrograma = new BMMatriculaPrograma();
        }

        #endregion

        #region "Métodos Públicos"

        public MatriculaPrograma ObterMatriculaProgramaPorID(int pId)
        {

            MatriculaPrograma matriculaPrograma = null;

            try
            {
                bmMatriculaPrograma = new BMMatriculaPrograma();
                matriculaPrograma = bmMatriculaPrograma.ObterPorId(pId);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }

            return matriculaPrograma;
        }

        public void IncluirMatriculaPrograma(MatriculaPrograma pMatriculaPrograma)
        {
            try
            {
                //repositorio.ValidarMatriculaProgramaInformada(pMatriculaPrograma);
                //pMatriculaPrograma.NivelOcupacional = pMatriculaPrograma.Usuario.NivelOcupacional;
                //pMatriculaPrograma.UF = pMatriculaPrograma.Usuario.UF;
                bmMatriculaPrograma.Salvar(pMatriculaPrograma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AlterarMatriculaPrograma(MatriculaPrograma pMatriculaPrograma)
        {
            try
            {
                bmMatriculaPrograma.Salvar(pMatriculaPrograma);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public void ExcluirMatriculaPrograma(int IdMatriculaPrograma)
        {

            try
            {
                MatriculaPrograma matriculaPrograma = null;

                if (IdMatriculaPrograma > 0)
                {
                    matriculaPrograma = bmMatriculaPrograma.ObterPorId(IdMatriculaPrograma);
                }

                bmMatriculaPrograma.Excluir(matriculaPrograma);

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<MatriculaPrograma> ObterMatriculasDoUsuario(int pIdUsuario)
        {
            try
            {
                return bmMatriculaPrograma.ObterPorUsuario(pIdUsuario);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IList<MatriculaPrograma> ObterMatriculasPorPrograma(Programa pPrograma)
        {
            try
            {
                return bmMatriculaPrograma.ObterPorPrograma(pPrograma.ID);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IList<MatriculaPrograma> ObterMatriculaProgramaPorFiltro(MatriculaPrograma pMatriculaPrograma)
        {
            try
            {
                return bmMatriculaPrograma.ObterPorFiltros(pMatriculaPrograma);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }

        }

        #endregion

    }
}
