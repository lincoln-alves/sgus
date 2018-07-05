using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Solucao;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioTrilha : BusinessProcessBase, IDisposable
    {
        protected BMUsuarioTrilha BmUsuarioTrilha = null;

        public UsuarioTrilha ObterPorId(int id)
        {
            return BmUsuarioTrilha.ObterPorId(id);
        }

        public UsuarioTrilha ObterUsuarioPorToken(string token)
        {
            return BmUsuarioTrilha.ObterPorToken(token);
        }

        public ManterUsuarioTrilha() : base() {
            BmUsuarioTrilha = new BMUsuarioTrilha();
        }

        public UsuarioTrilha AtualizarStatusComRegras(UsuarioTrilha usuarioTrilha)
        {
            UsuarioTrilha usuSessaoAtual = null;
            if (usuarioTrilha.ID != 0)
            {

                usuSessaoAtual = this.ObterPorId(usuarioTrilha.ID);

                if (usuSessaoAtual.Cronometro.TotalSeconds <= 0)
                {
                    if (!usuSessaoAtual.AprovadoProvaFinal)
                        usuSessaoAtual.StatusMatricula = enumStatusMatricula.Reprovado;
                    if (!usuSessaoAtual.TeveParticipacao)
                        usuSessaoAtual.StatusMatricula = enumStatusMatricula.Abandono;
                }

                this.Salvar(usuSessaoAtual);
            }

            return usuSessaoAtual;
        }

        public IList<DTOObjetivosTrilhas> ObterObjetivostrilha(int idUsuario, int idTrilhaNivel) {
            return (new BMObjetivo()).ObterObjetivoPorTrilhaNivel(idTrilhaNivel).Select( x => new DTOObjetivosTrilhas() { IdObjetivo = x.ID, ChaveExterna = x.ChaveExterna, NomeObjetivo = x.Nome }).ToList<DTOObjetivosTrilhas>();
        }

        public IList<DTORankingUsuarioTrilha> ConsultarRankingTrilhaUsuario(int pIdTrilha)
        {
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("p_ID_Trilha", pIdTrilha);

            return BmUsuarioTrilha.ExecutarProcedure<DTORankingUsuarioTrilha>("SP_RankingTrilhaNivelUsuario", lstParam);
        }

        public IList<UsuarioTrilha> ObterUsuariosAusentes(DateTime dataLimite)
        {
            IList<UsuarioTrilha> lstUsuarioTrilha = new List<UsuarioTrilha>();
            IDictionary<string, object> lstParam = new Dictionary<string, object>();

            lstParam.Add("pDataAcessoLimite", dataLimite);

            var result = BmUsuarioTrilha.ExecutarProcedure<DTORankingUsuarioTrilha>("SP_OBTER_ACESSOS_TRILHAS", lstParam);

            if (result != null)
            {
                foreach (var obj in result)
                {
                    UsuarioTrilha usuarioTrilha = new BMUsuarioTrilha().ObterPorId(obj.ID_UsuarioTrilha);
                    if (usuarioTrilha != null)
                    {
                        lstUsuarioTrilha.Add(usuarioTrilha);
                    }
                }

            }

            return lstUsuarioTrilha;
        }

        public IList<DTOAlunoDaTrilha> ListarRelatorioDoAlunoDaTrilha(int idUsuarioTrilha){
            var procRelatorioAlunoDaTrilha = new ProcRelatorioAlunoDaTrilha();
            return procRelatorioAlunoDaTrilha.ListarRelatorioDoAlunoDaTrilha(idUsuarioTrilha);
        }

        public void Dispose(){
            GC.Collect();
        }

        public void SolicitarNovaProvaTrilha(int pUsuarioTrilha, AuthenticationRequest autenticacao){
            var ut = BmUsuarioTrilha.ObterPorId(pUsuarioTrilha);

            if (ut == null) throw new AcademicoException("Usuário não encontrado");

            if (ut.DataLiberacaoNovaProva.HasValue) throw new AcademicoException(string.Format("Você poderá fazer nova prova a partir do dia {0}", ut.DataLiberacaoNovaProva.Value.ToString("dd/MM/yyyy")));
            int diasProrrogacao;

            if (!int.TryParse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro.ToString(), out diasProrrogacao)){
                diasProrrogacao = 10;
            }

            ut.DataLiberacaoNovaProva = DateTime.Today.AddDays(diasProrrogacao);
            ut.NovaProvaLiberada = true;
            ut.Auditoria = new Auditoria(autenticacao.Login);
            ut.StatusMatricula = enumStatusMatricula.Inscrito;

            BmUsuarioTrilha.Salvar(ut);
        }

        public void EnviarEmailBoasVindas(UsuarioTrilha usuarioTrilha){
            var templateNovaInscricaoTrilha = TemplateUtil.ObterInformacoes(enumTemplate.BoasVindasInscricaoTriha);

            var assuntoDoEmail = string.IsNullOrEmpty(templateNovaInscricaoTrilha.Assunto) ? "Universidade Corporativa Sebrae - Nova inscrição em trilha" : templateNovaInscricaoTrilha.Assunto;
            var corpoEmail = ObterCorpoEmail(usuarioTrilha, templateNovaInscricaoTrilha.TextoTemplate);

            EmailUtil.Instancia.EnviarEmail(usuarioTrilha.Usuario.Email, assuntoDoEmail, corpoEmail);
        }

        private static string ObterCorpoEmail(UsuarioTrilha usuarioTrilha, string textoTemplate){
            var corpo = textoTemplate.Replace("#NOME_USUARIO", usuarioTrilha.Usuario.Nome);

            corpo = corpo.Replace("#NOME_TRILHA", usuarioTrilha.TrilhaNivel.Trilha.Nome + " - " + usuarioTrilha.TrilhaNivel.Nome);

            return corpo;
        }


        public IList<UsuarioTrilha> ObterPorFiltro(UsuarioTrilha usuarioTrilha)
        {
            return BmUsuarioTrilha.ObterPorFiltro(usuarioTrilha);
        }

        public void Salvar(UsuarioTrilha usuarioTrilha)
        {
            BmUsuarioTrilha.Salvar(usuarioTrilha);
        }

        /// <summary>
        /// Obtem a Carga Horária do Nivel da Trilha somado ao Total de Carga Horária de Soluções AutoIndicativas
        /// </summary>
        /// <param name="usuarioTrilha">Usuário da trilha</param>
        /// <returns>Total Da Carga Horária do Nível da trilha</returns>
        public int ObterTotalCargaHoraria(UsuarioTrilha usuarioTrilha)
        {
            return BmUsuarioTrilha.ObterTotalCargaHoraria(usuarioTrilha);
        }

        /// <summary>
        /// Obtem a Carga Horária do Nivel da Trilha somado ao Total de Carga Horária de Soluções AutoIndicativas
        /// </summary>
        /// <param name="idUsuarioTrilha">ID do Usuário da trilha</param>
        /// <returns>Total Da Carga Horária do Nível da trilha</returns>
        public int ObterTotalCargaHoraria(int idUsuarioTrilha) {
            return BmUsuarioTrilha.ObterTotalCargaHoraria(idUsuarioTrilha);
        }

        public UsuarioTrilha ObterPorCodigoCertificao(string codigo)
        {                
                
            return BmUsuarioTrilha.ObterPorCodigoCertificao(codigo);
        }

        public UsuarioTrilha ObterPorUsuarioTrilhaNivel(UsuarioTrilha matricula, TrilhaNivel trilhaNivel)
        {
            return BmUsuarioTrilha.ObterPorUsuarioTrilhaNivel(matricula, trilhaNivel);
        }

        public UsuarioTrilha ObterPorUsuarioTrilhaNivel(int usuario, int nivel)
        {
            return BmUsuarioTrilha.ObterPorUsuarioTrilhaNivel(usuario, nivel);
        }

        public IQueryable<UsuarioTrilha> ObterTodosUsuarioTrilha()
        {
            return BmUsuarioTrilha.ObterTodos();
        }

        public IQueryable<UsuarioTrilha> ObterTodosIQueryable()
        {
            return BmUsuarioTrilha.ObterTodosIQueryable();
        }

        public UsuarioTrilha ObterPorUsuarioNivel(int usuarioId, int nivelId)
        {
            // Refs #1861 Utilizando LastOrDefault para pegar a última matrícula válida, 
            // assim não pega por exemplo matrículas canceladas anteriormente pelo aluno
            // não filtrei por Status pois existem status diferentes de Inscrito que tem 
            // acesso a Trilha
            return
                ObterTodosUsuarioTrilha().OrderByDescending(x => x.ID)
                    .FirstOrDefault(
                        x => x.Usuario.ID == usuarioId && x.TrilhaNivel.ID == nivelId && x.NovasTrilhas == true);
        }
    }
}