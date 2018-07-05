using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Sebrae.Academico.BP;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.UI
{
    public partial class UiHub : Hub
    {
        /// <summary>
        /// Envia uma notificação para todos os clientes que estiver escutando essa URL
        /// </summary>
        /// <param name="cpf">CPF do usuário usado para mapear os grupos de conexão</param>
        /// <param name="message"></param>
        public void Enviar(string cpf, string notificacao)
        {
            Clients.Group(cpf).atualizarNotificacao(notificacao);
        }

        public override Task OnConnected()
        {
            if (!string.IsNullOrEmpty(Context.QueryString["cpf"]))
            {
                // Recupera o cpf do usuário que é enviado por querystring para criar grupos de conexão
                var cpf = Context.QueryString["cpf"];
                Groups.Add(Context.ConnectionId, cpf);
            }
            return base.OnConnected();
        }

        public void AtualizarTurma(string jsonTurma)
        {
            Clients.All.atualizarTurma(jsonTurma);
        }
    }
}