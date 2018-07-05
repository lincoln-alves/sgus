using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.GestorUC;
using Sebrae.Academico.BP.DTO.Services.ListaProgramas;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterProgramaService : BusinessProcessServicesBase
    {
        private MatriculaPrograma matriculaPrograma = null;
        private ManterMatriculaPrograma manterMatriculaPrograma = new ManterMatriculaPrograma();
        private ManterPrograma manterPrograma = null;
        public List<DTOListaProgramaPrograma> ConsultarProgramaGestorUC(int idGestor, string filtroPrograma)
        {
            Usuario usuario = new BMUsuario().ObterPorId(idGestor);

            if (usuario == null)
                throw new AcademicoException("Usuário não localizado na base");

            if (usuario.ListaPerfil == null)
                throw new AcademicoException("Usuário sem permissão de acesso à funcionalidade");

            if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.GestorUC))
                throw new AcademicoException("Usuário sem perfil de gestor");

            Programa objPrograma = new Programa();
            objPrograma.Ativo = true;
            if (filtroPrograma != "" || filtroPrograma != null)
            {
                objPrograma.Nome = filtroPrograma;
            }
            IList<Programa> ListaPrograma = new BMPrograma().ObterPorFiltro(objPrograma, true);

            if (ListaPrograma.Count == 0 || ListaPrograma == null)
                throw new AcademicoException("Não há programas disponíveis");
            
            List<DTOListaProgramaPrograma> retorno = new List<DTOListaProgramaPrograma>();

            foreach (var prog in ListaPrograma)
            {
                DTOListaProgramaPrograma listReturn = new DTOListaProgramaPrograma();
                listReturn.CodigoPrograma = Convert.ToString(prog.ID);
                listReturn.NomePrograma = prog.Nome;
                retorno.Add(listReturn);
            }


            return retorno;
        }

        public DTOListaProgramaPrograma ConsultarProgramaMatriculaGestorUC(int idGestor, int idPrograma, string filtroAluno)
        {
            Usuario usuario = new BMUsuario().ObterPorId(idGestor);

            if (usuario == null)
                throw new AcademicoException("Usuário não localizado na base");

            if (usuario.ListaPerfil == null)
                throw new AcademicoException("Usuário sem permissão de acesso à funcionalidade");

            if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.GestorUC))
                throw new AcademicoException("Usuário sem perfil de gestor");

              Programa programa = new BMPrograma().ObterPorId(idPrograma);

              IList<MatriculaPrograma> matProg = new BMMatriculaPrograma().ObterUsuariosPorPrograma(programa.ID, filtroAluno, "");

            if (programa == null || programa.Ativo == false)
                throw new AcademicoException("Não há programas com esses dados informados");

            
            DTOListaProgramaPrograma retorno = new DTOListaProgramaPrograma();
            Programa objPrograma = new Programa();
            retorno.CodigoPrograma = Convert.ToString(programa.ID);
            retorno.NomePrograma = programa.Nome;

            if (matProg.Count > 0)
            {
                foreach (var aluno in matProg)
                {
                    DTOListaProgramaMatriculaPrograma listMatriculados = new DTOListaProgramaMatriculaPrograma();
                   listMatriculados.Id = aluno.Usuario.ID;
                    listMatriculados.Nome = aluno.Usuario.Nome;
                    listMatriculados.UF = aluno.UF.Sigla;
                    listMatriculados.NivelOcupacional = aluno.NivelOcupacional.Nome;
                    listMatriculados.StatusMatricula = Convert.ToString(aluno.StatusMatricula);
                    retorno.ListaMatriculaPrograma.Add(listMatriculados);
                }
            }


            return retorno;
        }

        public string MatriculaProgramaGestorUC(int idPrograma, string CPFuser, string DataInicio, string DataFim, string login)
        {
            Usuario usuario = new BMUsuario().ObterPorCPF(CPFuser);
            Usuario gestor = new BMUsuario().ObterPorCPF(login);

            if (!gestor.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.GestorUC))
                throw new AcademicoException("Usuário sem perfil de gestor");

            if (usuario == null)
                throw new AcademicoException("Usuário não localizado na base");

            Programa programa = new BMPrograma().ObterPorId(idPrograma);
            
            if(programa == null)
                throw new AcademicoException("Programa não localizado na base");

            IList<MatriculaPrograma> matProg = new BMMatriculaPrograma().ObterUsuariosPorPrograma(programa.ID, "", CPFuser);

            if(matProg.Count > 0)
                throw new AcademicoException("Usuário já matriculado neste pograma.");

            if(DataInicio == "" || DataFim == "")
                throw new AcademicoException("A data inicial e a data final são obrigatórias.");

            //var manterMatriculaPrograma = new ManterMatriculaPrograma();
            var matriculaPrograma = new MatriculaPrograma();

            matriculaPrograma.Usuario = usuario;
            matriculaPrograma.Programa = programa;
            matriculaPrograma.StatusMatricula = enumStatusMatricula.Inscrito;
            matriculaPrograma.DataInicio = CommonHelper.TratarDataObrigatoria(DataInicio, "Data Inicio");
            matriculaPrograma.DataFim = CommonHelper.TratarData(DataFim, "Data Fim");

            new BMMatriculaPrograma().Salvar(matriculaPrograma);
            
            return string.Empty;
        }

        public List<DTOListaProgramaPrograma> ConsultarProgramas(string filtroPrograma)
        {
            Programa objPrograma = new Programa();
            objPrograma.Ativo = true;
            if (filtroPrograma != "" || filtroPrograma != null)
            {
                objPrograma.Nome = filtroPrograma;
            }
            IList<Programa> ListaPrograma = new BMPrograma().ObterPorFiltro(objPrograma, true);

            if (ListaPrograma.Count == 0 || ListaPrograma == null)
                throw new AcademicoException("Não há programas disponíveis");

            List<DTOListaProgramaPrograma> retorno = new List<DTOListaProgramaPrograma>();

            foreach (var prog in ListaPrograma)
            {
                DTOListaProgramaPrograma listReturn = new DTOListaProgramaPrograma();
                listReturn.CodigoPrograma = Convert.ToString(prog.ID);
                listReturn.NomePrograma = prog.Nome;
                retorno.Add(listReturn);
            }


            return retorno;
        }
    }
}
