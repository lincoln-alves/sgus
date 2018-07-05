using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Util.Classes
{

    public class GenericEmailUtil<T> where T : class
    {
        private static IList<Template> listaTemplates = null;

        #region "Atributos Referentes aos Templates do Sistema"

        public static string Geral
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.Geral).TextoTemplate;
            }
        }

        public static string MatriculaOfertaExclusivo
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.MatriculaOfertaExclusivo).TextoTemplate;
            }
        }

        public static string MatriculaSolucaoEducacionalPortal
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.MatriculaSolucaoEducacionalPortal).TextoTemplate;
            }
        }

        public static string NotificacaoLimiteOferta
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.NotificacaoLimiteOferta).TextoTemplate;
            }
        }

        public static string NotificacaoTurma
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.NotificacaoTurma).TextoTemplate;
            }
        }

        public static string NotificacaoUsuarioTrilha
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.NotificacaoUsuarioTrilha).TextoTemplate;
            }
        }

        public static string RecuperacaoSenhaComConfirmacao
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.RecuperacaoSenhaComConfirmacao).TextoTemplate;
            }
        }

        public static string RecuperacaoSenhaSemConfirmacao
        {
            get
            {
                return ListaTemplates.FirstOrDefault(x => x.ID == (int)enumTemplate.RecuperacaoSenhaSemConfirmacao).TextoTemplate;
            }
        }


        #endregion

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public GenericEmailUtil()
        {
            listaTemplates = TemplateUtil.ObterTodasAsInformacoesDoTemplate();
        }


        private static IList<Template> ListaTemplates
        {
            get
            {
                return listaTemplates;
            }
        }


        public static void ObterCorpoDoEmail(IDictionary<string, T> dados, enumTemplate template)
        {
            //Template templateEncontrado = TemplateFacade.ObterInformacoes(template);
            
            //Busca o template na lista de templates obtida do banco de dados
            Template templateEncontrado = listaTemplates.FirstOrDefault(x => x.ID == (int)template);
            string textoTemplate = templateEncontrado.TextoTemplate;

            foreach (var item in dados)
            {
                textoTemplate = textoTemplate.Replace(item.Key.ToString(), item.Value.ToString());
            }

            string assunto = textoTemplate.Substring(0, textoTemplate.IndexOf(Environment.NewLine));


        }


        //              .Replace("@DATASISTEMA#", DateTime.Now.ToString("dd/MM/yyyy"))
        //              .Replace("@DATASISTEMAEXTENSO#", DateTime.Now.ToLongDateString().ToString())
    }
}
