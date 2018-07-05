using System;
using System.Linq;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Turma
{
    public partial class HistoricoTurma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int turmaId;

            if (int.TryParse(Request["Id"], out turmaId))
            {
                var turma = new ManterTurma().ObterTurmaPorID(turmaId);

                if (turma == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Turma inválida. Escolha uma turma.", "ListarTurma.aspx");
                    return;
                }

                nomeTurma.InnerText = turma.Nome;

                var logResponsaveis = new ManterLogResponsavel().ObterPorTurma(new Dominio.Classes.Turma { ID = turmaId }).ToList();

                WebFormHelper.PreencherGrid(logResponsaveis, dgvResponsaveis);

                var logConsultores = new ManterLogConsultorEducacional().ObterPorTurma(new Dominio.Classes.Turma { ID = turmaId }).ToList();

                WebFormHelper.PreencherGrid(logConsultores, dgvConsultores);
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro na busca do histórico. Tente novamente ou entre em contato com o suporte.", "ListarTurma.aspx");
            }
        }

        protected void btnVoltar_OnClick(object sender, EventArgs e)
        {

            Response.Redirect("ListarTurma.aspx");
        }
    }
}