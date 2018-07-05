using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Notificacao : EntidadeBasicaPorId
    {
        public virtual string Link { get; set; }
        public virtual bool Visualizado { get; set; }
        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime? DataVisualizacao { get; set; }
        public virtual DateTime? DataNotificacao { get; set; }
        public virtual string TextoNotificacao { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual enumTipoNotificacao TipoNotificacao { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual NotificacaoEnvio NotificacaoEnvio { get; set; }

        #region "Atributos Que não serao Mapeados" 

        public virtual string MensagemLida
        {
            get
            {
                string mensagemLida = string.Empty;

                if (this.Visualizado)
                {
                    mensagemLida = "Sim";
                }
                else
                {
                    mensagemLida = "Não";
                }

                return mensagemLida;
            }
        }

        #endregion

    }
}