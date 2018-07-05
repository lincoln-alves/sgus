using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms
{
    public partial class Administracao : System.Web.UI.Page
    {
        BMUsuario bmUsuario = new BMUsuario();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCPF.Text))
            {
                var usuario = bmUsuario.ObterPorCPF(txtCPF.Text.Trim());
                if (usuario != null)
                {
                    try
                    {
                        liRetorno.Text = CriptografiaHelper.Decriptografar(usuario.Senha);
                    }
                    catch
                    {
                        liRetorno.Text = "Ocorreu um erro, ou o usuário não tem senha registrada";
                    }
                }
                else
                {
                    liRetorno.Text = "Usuário não encontrado";
                }
            }
        }

        protected void btnAtualizarChavesExternas_OnClick(object sender, EventArgs e)
        {
            var lista = new List<int>();

            using (var conn = new SqlConnection(CommonHelper.ConnectionString))
            {
                using (var command = new SqlCommand("SP_OBTER_CHAVE_EXTERNA_DUPLICADA", conn) { CommandType = CommandType.StoredProcedure })
                {
                    conn.Open();

                    using (var rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(int.Parse(rdr["ID_Oferta"].ToString()));
                        }
                    }
                }
            }

            var manterOferta = new BP.ManterOferta();

            var listaSync = new List<int>();
            var listaNaoSync = new List<int>();


            foreach (var id in lista)
            {
                var oferta = manterOferta.ObterOfertaPorID(id);

                if (oferta.IDChaveExterna != null)
                {
                    var url = "http://ava.uc.sebrae.com.br/auth/sgus/updateAllUserGrades.php?oferta_chave_externa_id=" + oferta.IDChaveExterna;

                    var request = (HttpWebRequest)WebRequest.Create(url);
                    try
                    {
                        var response = request.GetResponse();
                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                var reader = new StreamReader(responseStream, Encoding.UTF8);
                                reader.ReadToEnd();
                            }
                        }

                        if (oferta.DataFimInscricoes.HasValue && oferta.DataFimInscricoes.Value.Date < DateTime.Now.Date)
                        {
                            oferta.IDChaveExterna = null;

                            manterOferta.AlterarOferta(oferta);

                            listaSync.Add(oferta.ID);
                        }
                    }
                    catch
                    {
                        listaNaoSync.Add(oferta.ID);
                    }
                }
            }

            foreach (var id in listaSync)
            {
                labelSync.Text += "<p>" + id + "</p>";
            }
            foreach (var id in listaNaoSync)
            {
                labelNaoSync.Text += "<p>" + id + "</p>";
            }
        }

        protected void btnCleanCache_Click(object sender, EventArgs e)
        {
            new CacheData("PURGE", 0).PurgeAllCaches();            
        }
    }
}