using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterMatriculaTrilha : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMUsuarioTrilha bmMatriculaTrilha = null;

        #endregion

        #region "Construtor"

        public ManterMatriculaTrilha()
            : base()
        {
            bmMatriculaTrilha = new BMUsuarioTrilha();
        }

        #endregion

        #region "Métodos Públicos"

        public UsuarioTrilha ObterMatriculaTrilhaPorID(int pId)
        {

            UsuarioTrilha usuarioTrilha = null;

            try
            {
                bmMatriculaTrilha = new BMUsuarioTrilha();
                usuarioTrilha = bmMatriculaTrilha.ObterPorId(pId);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }

            return usuarioTrilha;
        }

        public void IncluirMatriculaTrilha(UsuarioTrilha pUsuarioTrilha)
        {
            try
            {
                bmMatriculaTrilha.ValidarMatriculaTrilhaInformada(pUsuarioTrilha);

                pUsuarioTrilha.DataInicio = DateTime.Now;

                //Status Inicial no cadastro.
                pUsuarioTrilha.StatusMatricula = enumStatusMatricula.Inscrito;
                pUsuarioTrilha.NivelOcupacional = pUsuarioTrilha.Usuario.NivelOcupacional;
                pUsuarioTrilha.Uf = pUsuarioTrilha.Usuario.UF;
                pUsuarioTrilha.AcessoBloqueado = false;
                pUsuarioTrilha.NovasTrilhas = true;

                bmMatriculaTrilha.Salvar(pUsuarioTrilha);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IList<Usuario> ObterPorTrilhaTrilhaNivel(int idTrilha, int idTrilhaNivel)
        {
            IList<Usuario> ListaUsuarioTrilha = null;

            try
            {
                ListaUsuarioTrilha = bmMatriculaTrilha.ObterPorTrilhaTrilhaNivel(idTrilha, idTrilhaNivel);

                if ((ListaUsuarioTrilha == null) || (ListaUsuarioTrilha != null && ListaUsuarioTrilha.Count == 0))
                {
                    throw new AcademicoException("Não há Usuários Matriculados");
                }
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }

            return ListaUsuarioTrilha;
        }

        public void AlterarMatriculaTrilha(UsuarioTrilha pUsuarioTrilha)
        {
            try
            {
                bmMatriculaTrilha.ValidarMatriculaTrilhaInformada(pUsuarioTrilha);
                bmMatriculaTrilha.Salvar(pUsuarioTrilha);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public void ExcluirMatriculaTrilha(int IdUsuarioTrilha)
        {
            try
            {
                UsuarioTrilha usuarioTrilha = null;

                if (IdUsuarioTrilha > 0)
                {
                    usuarioTrilha = bmMatriculaTrilha.ObterPorId(IdUsuarioTrilha);
                }

                bmMatriculaTrilha.Excluir(usuarioTrilha);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<UsuarioTrilha> ObterMatriculaTrilhaPorFiltro(UsuarioTrilha usuarioTrilha, IEnumerable<int> ufsSelecionados)
        {
            try
            {
                bmMatriculaTrilha.VerificarCamposObrigatoriosDoFiltro(usuarioTrilha);
                return bmMatriculaTrilha.ObterPorFiltro(usuarioTrilha, ufsSelecionados);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IList<UsuarioTrilha> ObterMatriculaTrilhaPorFiltro(UsuarioTrilha usuarioTrilha)
        {
            try
            {
                bmMatriculaTrilha.VerificarCamposObrigatoriosDoFiltro(usuarioTrilha);
                return bmMatriculaTrilha.ObterPorFiltro(usuarioTrilha);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }

        }

        public IList<UsuarioTrilha> ObterMatriculasDoUsuario(int idUsuario)
        {
            try
            {
                return bmMatriculaTrilha.ObterMatriculasDoUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        #endregion

    }

}