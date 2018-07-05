using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterPrograma : BusinessProcessBase
    {
        private BMPrograma bmPrograma = null;

        public ManterPrograma()
            : base()
        {
            bmPrograma = new BMPrograma();
        }

        public IList<DTORelatorioUsuarioMatriculadoPrograma> ConsultarUsuarioMatriculaPrograma(int pIdPrograma, int pStatusMatricula)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("P_ID_PROGRAMA", pIdPrograma);
            lstParam.Add("P_STATUS_MATRICULA", pStatusMatricula);

            return bmPrograma.ExecutarProcedure<DTORelatorioUsuarioMatriculadoPrograma>("SP_REL_USUARIO_MATRICULADO_PROGRAMA", lstParam);

        }

        public DataTable ConsultarProgramasTable(int? idPrograma, int? idCapacitacao, int? idTurmaCapacitacao,int? idModulo) {
            IDictionary<string, object> lstParametro = new Dictionary<string, object>();

            if (idPrograma.HasValue && idPrograma != 0) lstParametro.Add("p_idPrograma", idPrograma);

            if (idCapacitacao.HasValue && idCapacitacao != 0) lstParametro.Add("p_idCapacitacao", idCapacitacao);

            if (idTurmaCapacitacao.HasValue && idTurmaCapacitacao != 0) lstParametro.Add("p_idTurmaCapacitacao", idTurmaCapacitacao);

            if (idModulo.HasValue && idModulo != 0) lstParametro.Add("p_idModulo", idModulo);

            return bmPrograma.ExecutarProcedureTable("SP_REL_PROGRAMAS", lstParametro);
        }

        public IList<DTORelatorioPrograma> ConsultarProgramas(string pNome) {
            IDictionary<string, object> lstParametro = new Dictionary<string, object>();
            lstParametro.Add("p_Nome", pNome);

            return bmPrograma.ExecutarProcedure<DTORelatorioPrograma>("SP_REL_PROGRAMAS", lstParametro);
        } 

        public void AtualizarPrograma(Programa pPrograma) {
            if (pPrograma == null) throw new AcademicoException("Programa inválido.");
            if(pPrograma.ID == 0) IncluirPrograma(pPrograma);
            else AlterarPrograma(pPrograma);
        }

        public void IncluirPrograma(Programa pPrograma){
            try{
                PreencherInformacoesDeAuditoria(pPrograma);
                pPrograma.Sequencia = bmPrograma.ObterMaiorSequencia() + 1;
                bmPrograma.Salvar(pPrograma);
                AtualizarNodeIdDrupal(pPrograma);
            }catch (AcademicoException ex){
                throw ex;
            }
        }

        public void AtualizarNodeIdDrupal(Programa pPrograma, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var id = SalvaNodeDrupalRest(pPrograma, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);

            if (!id.HasValue)
                return;
            pPrograma.IdNodePortal = id.Value;

            bmPrograma.Salvar(pPrograma);
        }

        private void PreencherInformacoesDeAuditoria(Programa pPrograma){
            base.PreencherInformacoesDeAuditoria(pPrograma);
            pPrograma.ListaPermissao.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
            pPrograma.ListaTag.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public void AlterarPrograma(Programa pPrograma){
            try{
                PreencherInformacoesDeAuditoria(pPrograma);
                bmPrograma.Salvar(pPrograma);
                AtualizarNodeIdDrupal(pPrograma);
            }catch (Exception ex){
                throw ex;
            }
        }
        
        public void ExcluirPrograma(int IdPrograma){
            try{
                Programa programa = null;
                if (IdPrograma <= 0) return;
                programa = bmPrograma.ObterPorId(IdPrograma);
                if (programa == null) return;
                if (programa.IdNodePortal.HasValue) {
                    DrupalUtil.RemoverNodeDrupalRest(programa.IdNodePortal.Value);
                }
                bmPrograma.Excluir(programa);
            }catch (AcademicoException ex){
                throw ex;
            }

        }

        public IList<Programa> ObterProgramaPorFiltro(Programa pPrograma)
        {
            return bmPrograma.ObterPorFiltro(pPrograma);
        }

        public IQueryable<Programa> ObterTodosProgramas()
        {
            return bmPrograma.ObterTodos();
        }

        public Programa ObterProgramaPorID(int pId)
        {
            return bmPrograma.ObterPorId(pId);
        }

        internal IList<ProgramaSolucaoEducacional> ObterPrograSolucaoEducacional()
        {
            return bmPrograma.ObterPrograSolucaoEducacional();
        }

        public int? SalvaNodeDrupalRest(Programa registro, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var postParameters = DrupalUtil.InitPostParameters(registro.ID, registro.Nome, registro.Apresentacao,
                "solucao", registro.Ativo);

            postParameters.Add("data[field_tipo_de_solucao]", "5");

            DrupalUtil.PermissoesAreaTematica(
                registro.ListaAreasTematicas.Where(p => p.AreaTematica != null).Select(x => x.AreaTematica.ID).ToList(),
                ref postParameters);
            DrupalUtil.PermissoesUf(registro.ListaPermissao.Where(p => p.Uf != null).Select(x => x.Uf.ID).ToList(),
                ref postParameters);
            DrupalUtil.PermissoesPerfil(
                registro.ListaPermissao.Where(p => p.Perfil != null).Select(x => x.Perfil.ID).ToList(),
                ref postParameters);
            DrupalUtil.PermissoesNivelOcupacional(
                registro.ListaPermissao.Where(p => p.NivelOcupacional != null)
                    .Select(x => x.NivelOcupacional.ID)
                    .ToList(), ref postParameters);
            try
            {
                return DrupalUtil.SalvaNodeDrupalRest(postParameters, true, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IQueryable<Programa> ObterPorInscricoesAbertas()
        {
            return bmPrograma.ObterPorInscricoesAbertas();
        }
    }
}
