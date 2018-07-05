using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP.DTO.Services.ListaProgramas;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterCapacitacaoService : BusinessProcessServicesBase
    {
        private ManterMatriculaPrograma manterMatriculaPrograma = new ManterMatriculaPrograma();

        public List<DTOCapacitacao> ConsultarCapacitacoes(int idUsuario, int idCapacitacao = 0, int idPrograma = 0, string nome = "")
        {
            Usuario usuario = new BMUsuario().ObterPorId(idUsuario);

            if (usuario == null)
                throw new AcademicoException("Usuário não localizado na base");

            Capacitacao objCapacitacao = new Capacitacao();
            if (idPrograma > 0)
            {
                objCapacitacao.Programa.ID = idPrograma;
            }

            if (!string.IsNullOrEmpty(nome))
            {
                objCapacitacao.Nome = nome;
            }

            if (idCapacitacao > 0)
            {
                objCapacitacao.ID = idCapacitacao;
            }

            IList<Capacitacao> ListaCapacitacoes = new BMCapacitacao().ObterPorFiltroNoPeriodoInscricoes(objCapacitacao);

            if (ListaCapacitacoes.Count == 0 || ListaCapacitacoes == null)
                throw new AcademicoException("Não há capacitações disponíveis");

            List<DTOCapacitacao> retorno = new List<DTOCapacitacao>();

            IList<MatriculaCapacitacao> capacitacoesJaMatriculado = new BMMatriculaCapacitacao().ObterPorUsuario(usuario.ID);

            foreach (var cap in ListaCapacitacoes)
            {
                DTOCapacitacao listReturn = new DTOCapacitacao();
                listReturn.jaInscrito = false;
                if (capacitacoesJaMatriculado.Count > 0) 
                {
                    var jaInscrito = capacitacoesJaMatriculado.Where(x => x.Capacitacao.ID == cap.ID).ToList();
                    if (jaInscrito.Count > 0)
                    {
                        listReturn.jaInscrito = true;
                    }
                }

                listReturn.ID = cap.ID;
                listReturn.NomeCapacitacao = cap.Nome;
                listReturn.DataInicio = cap.DataInicio.ToString("dd/MM/yyyy");
                listReturn.DataFim = cap.DataFim.HasValue ? cap.DataFim.Value.ToString("dd/MM/yyyy") : "";
                listReturn.DataInicioInscricoes = cap.DataInicioInscricao.HasValue ? cap.DataInicioInscricao.Value.ToString("dd/MM/yyyy") : "";
                listReturn.DataFimInscricoes = cap.DataFimInscricao.HasValue ? cap.DataFimInscricao.Value.ToString("dd/MM/yyyy") : "";
                listReturn.Programa.ID = cap.Programa.ID;
                listReturn.Programa.Nome = cap.Programa.Nome;
                listReturn.Programa.Ativo = cap.Programa.Ativo;
                listReturn.descricao = cap.Descricao;
                listReturn.TurmaCapacitacao = cap.ListaTurmas.Select(f => new DTOTurmaCapacitacao { ID = f.ID, Nome = f.Nome }).ToList();
                listReturn.PodeRealizarIscricao = cap.DataInicioInscricao.HasValue && cap.DataFimInscricao.HasValue && (DateTime.Now > cap.DataInicioInscricao.Value && DateTime.Now < cap.DataFimInscricao.Value);

                retorno.Add(listReturn);
            }


            return retorno;
        }

        public string MatriculaCapacitacao(int idUsuario, int idCapacitacao, int idTurma){
            var usuario = new BMUsuario().ObterPorId(idUsuario);

            if (usuario == null) throw new AcademicoException("Usuário não localizado na base");

            var capacitacao = new BMCapacitacao().ObterPorId(idCapacitacao);

            if (capacitacao == null) throw new AcademicoException("Capacitação não localizado na base");

            var matCap = new BMMatriculaCapacitacao().ObterUsuariosPorCapacitacao(usuario.ID, capacitacao.ID);

            if (matCap.Count > 0) throw new AcademicoException("Usuário já matriculado nesta capacitação.");

            var turmaCap = new BMTurmaCapacitacao().ObterPorId(idTurma);

            if(turmaCap == null) throw new AcademicoException("Turma não localizada na base");

            var matriculaCapacitacao = new MatriculaCapacitacao {
                Usuario = usuario,
                Capacitacao = capacitacao,
                UF = usuario.UF,
                NivelOcupacional = usuario.NivelOcupacional,
                StatusMatricula = enumStatusMatricula.Inscrito,
                DataInicio = DateTime.Today
            };


            new BMMatriculaCapacitacao().Salvar(matriculaCapacitacao);

            var matriculaTurmaCapacitacao = new MatriculaTurmaCapacitacao {
                MatriculaCapacitacao = matriculaCapacitacao,
                TurmaCapacitacao = turmaCap,
                DataMatricula = DateTime.Today
            };

            new BMMatriculaTurmaCapacitacao().Salvar(matriculaTurmaCapacitacao);

            return string.Empty;
        }


    }
}
