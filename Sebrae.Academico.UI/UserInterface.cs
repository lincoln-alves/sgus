using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sebrae.Academico.UI
{
    public static class UserInterface
    {
        public static void Enviar(string titulo, string mensagem)
        {
            new UIHub().Enviar(titulo, mensagem);
        }
    }
}