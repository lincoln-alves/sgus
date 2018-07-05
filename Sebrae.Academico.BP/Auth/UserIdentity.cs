using System.Collections.Generic;
using Nancy.Security;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.Auth
{
    public class UserIdentity : IUserIdentity
    {
        /// <summary>
        /// Gets or sets the name of the current user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or set the claims of the current user.
        /// </summary>
        public IEnumerable<string> Claims { get; set; }

        public IDictionary<string, object> Payload {get;set;}
        
        public long Expiration { get; set; }
        
        public TrilhaNivel Nivel { get; set; }
        public UsuarioTrilha Matricula { get; set; }
        public Usuario Usuario { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public string JwtToken { get; set; }
    }
}