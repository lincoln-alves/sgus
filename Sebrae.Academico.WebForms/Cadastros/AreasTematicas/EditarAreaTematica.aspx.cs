using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.AreasTematicas
{
    public partial class EditarAreaTematica : System.Web.UI.Page
    {
        private AreaTematica _areaTematica = null;
        private readonly ManterAreaTematica _manterAreaTematica = new ManterAreaTematica();
        protected void Page_Load(object sender, EventArgs e){
            if (IsPostBack) return;

            this.ucPermissoes1.PreencherListas();

            if (Request["Id"] == null) return;

            var id = Convert.ToInt32(Request["ID"]);
            _areaTematica = _manterAreaTematica.ObterPorId(id);

            if (_areaTematica == null){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Registro não encontrado.", "/Cadastros/AreasTematicas/ListarAreasTematicas.aspx");
                return;
            }
            PreencherCampos();
        }

        void PreencherCampos(){
            txtNome.Text = _areaTematica.Nome;
            txtTextoApresentacao.Text = _areaTematica.Apresentacao;

            PreencherListaPerfil(_areaTematica);
            PreencherListaNivelOcupacional(_areaTematica);
            PreencherListaUfs(_areaTematica);
            hdClassFont.Value = _areaTematica.Icone;
        }

        AreaTematica ObterObjetoAreaTematica(){
            var novo = Request["Id"] == null;
            var obj = novo ? new AreaTematica() : _manterAreaTematica.ObterPorId(Convert.ToInt32(Request["id"]));

            obj.Nome = txtNome.Text;
            obj.Apresentacao = txtTextoApresentacao.Text;
            obj.Icone = hdClassFont.Value;

            AdicionarOuRemoverPerfil(obj);
            AdicionarOuRemoverUf(obj);
            AdicionarOuRemoverNivelOcupacional(obj);

            return obj;
        }

        private void PreencherListaPerfil(AreaTematica obj){
            IList<Perfil> listaPerfil = obj.ListaPermissao.Where(x => x.Perfil != null).Select(x => new Perfil() { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList();
            var temPerfilPublico = false;
            if (listaPerfil.Count == 0){
                temPerfilPublico = obj.ListaPermissao.Any(x => x.Perfil == null && x.NivelOcupacional == null && x.Uf == null);
            }
            this.ucPermissoes1.PreencherListBoxComPerfisGravadosNoBanco(listaPerfil, temPerfilPublico);
        }

        private void PreencherListaNivelOcupacional(AreaTematica obj){
            this.ucPermissoes1.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(obj.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList());
        }

        private void PreencherListaUfs(AreaTematica obj){
            this.ucPermissoes1.PreencherListBoxComUfsGravadasNoBanco(obj.ListaPermissao.Where(x => x.Uf != null).Select(x => new Uf() { ID = x.Uf.ID, Nome = x.Uf.Nome }).ToList());
        }

        private void AdicionarOuRemoverPerfil(AreaTematica obj){
            var todosPerfis = this.ucPermissoes1.ObterTodosPerfis;
            if (todosPerfis != null && todosPerfis.Count > 0){
                for (var i = 0; i < todosPerfis.Count; i++){
                    if (string.IsNullOrWhiteSpace(todosPerfis[i].Value)) continue;
                    var perfilSelecionado = new Perfil{
                        ID = int.Parse(todosPerfis[i].Value),
                        Nome = todosPerfis[i].Text
                    };

                    if (todosPerfis[i].Selected){
                        obj.AdicionarPerfil(perfilSelecionado);
                    }else{
                        obj.RemoverPerfil(perfilSelecionado);
                    }
                }
            }else{
                if (obj.ListaPermissao == null) return;
                var ofertaPermissao = new AreaTematicaPermissao() { AreaTematica = obj };
                obj.ListaPermissao.Add(ofertaPermissao);
            }
        }

        private void AdicionarOuRemoverUf(AreaTematica obj)
        {
            try
            {
                Repeater rptUFs = (Repeater)ucPermissoes1.FindControl("rptUFs");
                for (int i = 0; i < rptUFs.Items.Count; i++)
                {
                    CheckBox ckUF = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    Label lblUF = (Label)rptUFs.Items[i].FindControl("lblUF");
                    TextBox txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                    int idUf = int.Parse(ckUF.Attributes["ID_UF"]);
                    var ufSelecionado = new Uf
                    {
                        ID = idUf,
                        Nome = lblUF.Text,
                    };

                    if (ckUF.Checked)
                    {
                        int vagas = 0;
                        if (!string.IsNullOrEmpty(txtVagas.Text)) vagas = int.Parse(txtVagas.Text);

                        obj.AdicionarUfs(ufSelecionado);
                    }
                    else
                    {
                        obj.RemoverUf(ufSelecionado);
                    }
                }
            }
            catch
            {
                //throw new ExecutionEngineException("Você deve informar a quantidade de vagas do estado");
            }
        }

        private void AdicionarOuRemoverNivelOcupacional(AreaTematica obj){
            var todosNiveisOcupacionais = this.ucPermissoes1.ObterTodosNiveisOcupacionais;
            if (todosNiveisOcupacionais == null || todosNiveisOcupacionais.Count <= 0) return;
            for (var i = 0; i < todosNiveisOcupacionais.Count; i++){
                if (string.IsNullOrWhiteSpace(todosNiveisOcupacionais[i].Value)) continue;
                var nivelOcupacionalSelecionado = new NivelOcupacional{
                    ID = int.Parse(todosNiveisOcupacionais[i].Value),
                    Nome = todosNiveisOcupacionais[i].Text
                };
                if (todosNiveisOcupacionais[i].Selected){
                    obj.AdicionarNivelOcupacional(nivelOcupacionalSelecionado);
                }else{
                    obj.RemoverNivelOcupacional(nivelOcupacionalSelecionado);
                }
            }
        }

        private static string RecuperarDadosArquivo(FileUpload fuImg){
            if (fuImg != null && fuImg.PostedFile != null && fuImg.PostedFile.ContentLength > 0){
                var imagem = fuImg.PostedFile.InputStream;
                var imagemConvertidaEmBase64String = CommonHelper.ObterBase64String(imagem);
                var informacoesDoArquivoParaBase64 = CommonHelper.GerarInformacoesDoArquivoParaBase64(fuImg);
                return string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);
            }else{
                return string.Empty;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e){
            _manterAreaTematica.AtualizarAreaTematica(ObterObjetoAreaTematica());

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!", "/Cadastros/AreasTematicas/ListarAreasTematicas.aspx");
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarAreasTematicas.aspx");
        }
    }
}