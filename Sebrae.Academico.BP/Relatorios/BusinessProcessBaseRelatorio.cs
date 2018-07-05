using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Relatorios
{
    public abstract class BusinessProcessBaseRelatorio {
        protected BMLogGeracaoRelatorio LogGeracaoRelatioBm;
        public int IdRelatorio { get; set; }

        /// <summary>
        /// Propriedade referente a um Relatório.
        /// </summary>
        protected abstract enumRelatorio Relatorio { get; }

        public string ProcedureName { get; set; }

        protected void RegistrarLogExecucao()
        {
            try
            {
                using (var lgExec = new BMLogGeracaoRelatorio())
                {
                    var idUsuario = ((Usuario)HttpContext.Current.Session["usuarioSGUS"]).ID;
                    var lg = new LogGeracaoRelatorio
                    {
                        DTGeracao = DateTime.Now,
                        Relatorio = new Relatorio { ID = (int)Relatorio },
                        Usuario = new Usuario { ID = idUsuario }
                    };

                    lgExec.Salvar(lg);
                }
            }
            catch
            {
                return;
            }
        }

        
    }
}
