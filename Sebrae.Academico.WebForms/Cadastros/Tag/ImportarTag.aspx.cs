using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Tag
{
    public partial class ImportarTag : System.Web.UI.Page
    {

        #region "Atributos Privados"

        /// <summary>
        /// Lista Provisória para guardar os Ids dos Níveis Pais.
        /// Esta lista guardará os ids dos níveis 0,1,2,3...
        /// </summary>
        public IDictionary<int, Sebrae.Academico.Dominio.Classes.Tag> ListaProvisoriaComIDDosPais { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnviarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ProcessarArquivoCSV();
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Arquivo Processado com Sucesso !");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void ProcessarArquivoCSV()
        {

            //Imagem enviada
            if (this.ArquivoFoiEnviado)
            {

                if (fupldArquivoTags != null && fupldArquivoTags.PostedFile != null
                    && fupldArquivoTags.PostedFile.ContentLength > 0)
                {

                    var arquivoCSV = fupldArquivoTags.PostedFile.InputStream;

                    var reader = new StreamReader(arquivoCSV, System.Text.Encoding.UTF8);

                    // var reader = new StreamReader(File.OpenRead(@"C:\Users\rafael.nardo\Downloads\univoc\univoc_competitividade.csv"));

                    byte qtdTracosNivelAtual = 0;
                    Sebrae.Academico.Dominio.Classes.Tag tagAtual = null;
                    IList<Sebrae.Academico.Dominio.Classes.Tag> ListaTags = new List<Sebrae.Academico.Dominio.Classes.Tag>();
                    ManterTag manterTag = new ManterTag();
                    string descricaoComTracos = string.Empty;
                    string descricaoSemTracos = string.Empty;
                    this.InicializarLista();
                    char hifen = "-".ToArray()[0];

                    Sebrae.Academico.Dominio.Classes.Tag pai = null;

                    Sebrae.Academico.Dominio.Classes.Tag paiObtidoDoBanco = null;
                    while (!reader.EndOfStream)
                    {
                        var linhaDoArquivoCSV = reader.ReadLine();

                        descricaoComTracos = linhaDoArquivoCSV;
                        descricaoSemTracos = descricaoComTracos.Replace("-", "");

                        foreach (var caractere in descricaoComTracos)
                        {

                            if (caractere.Equals(hifen)) //"-"
                            {
                                qtdTracosNivelAtual++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        tagAtual = new Dominio.Classes.Tag();
                        tagAtual.Nome = descricaoSemTracos;

                        if (string.IsNullOrWhiteSpace(descricaoSemTracos))
                        {
                            qtdTracosNivelAtual = 0;
                            continue;
                        }

                        tagAtual.NumeroNivel = qtdTracosNivelAtual;

                        if (qtdTracosNivelAtual > 0)
                        {
                            pai = ListaProvisoriaComIDDosPais.FirstOrDefault(x => x.Key == qtdTracosNivelAtual - 1).Value;
                            if (pai.ID > 0)
                                paiObtidoDoBanco = new ManterTag().ObterTagPorID(pai.ID);
                            else
                                paiObtidoDoBanco = new ManterTag().ObterTagPorFiltro(new Dominio.Classes.Tag() { Nome = pai.Nome }).FirstOrDefault() ;
                            
                            if (paiObtidoDoBanco == null)
                                throw new AcademicoException(string.Format("Não foi possível encontrar a tag pai da tag {0}", pai.Nome));

                            tagAtual.TagPai = paiObtidoDoBanco;
                        }

                        //Antes de incluir, verifica primeiro se já existe
                        bool tagJaExiste = new ManterTag().VerificarExistenciaPorNome(tagAtual.Nome);

                        if (!tagJaExiste)
                        {
                            manterTag.IncluirTag(tagAtual);
                        }
                        //else
                        //{
                        //    qtdTracosNivelAtual = 0;
                        //    continue;
                        //}

                        if (ListaProvisoriaComIDDosPais.Any(x => x.Key == tagAtual.NumeroNivel.Value))
                        {
                            ListaProvisoriaComIDDosPais[qtdTracosNivelAtual] = tagAtual;
                        }
                        else
                        {
                            ListaProvisoriaComIDDosPais.Add(tagAtual.NumeroNivel.Value, tagAtual);
                        }

                        qtdTracosNivelAtual = 0;
                    }

                }
            }
        }

        private void InicializarLista()
        {
            this.ListaProvisoriaComIDDosPais = new Dictionary<int, Sebrae.Academico.Dominio.Classes.Tag>();
        }

        /// <summary>
        /// Obtéum um objeto Tag pelo id do nível
        /// </summary>
        /// <param name="nivel"></param>
        /// <returns></returns>
        private Sebrae.Academico.Dominio.Classes.Tag ObterTagPorNivel(byte nivel)
        {
            Sebrae.Academico.Dominio.Classes.Tag tag = this.ListaProvisoriaComIDDosPais[nivel];
            return tag;
        }

        private void AtualizarListaProvisoriaDeIdNiveis(Sebrae.Academico.Dominio.Classes.Tag tag, byte nivel)
        {
            //Senão existir, adiciona
            if (!this.ListaProvisoriaComIDDosPais.Any(x => x.Key == nivel))
            {
                this.ListaProvisoriaComIDDosPais.Add(nivel, tag);
            }
            else
            {
                //Se já existir, atualiza
                this.ListaProvisoriaComIDDosPais[nivel] = tag;
            }
        }

        public bool ArquivoFoiEnviado
        {

            get
            {

                bool arquivoFoienviado = false;

                if (fupldArquivoTags != null && fupldArquivoTags.PostedFile != null
                   && fupldArquivoTags.PostedFile.ContentLength > 0)
                {
                    arquivoFoienviado = true;
                }
                else
                {
                    /* Se imagem vazia, significa que o usuário não subiu uma imagem ou então
                        houve um postback e o controle fileupload não guardou a imagem enviada.
                        Apesar disso, verifica se a imagem foi persistida no user control de upload */
                    if (!string.IsNullOrWhiteSpace(this.Imagem))
                    {
                        arquivoFoienviado = true;
                    }
                }

                return arquivoFoienviado;
            }
        }

        /// <summary>
        /// Imagem Enviada.
        /// </summary>
        public string Imagem
        {
            get
            {
                if (ViewState["ViewStateImagem"] != null)
                {
                    return (string)ViewState["ViewStateImagem"];
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ViewStateImagem"] = value;
            }

        }

    }

}