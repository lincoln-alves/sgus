using Sebrae.Academico.Dominio.Classes;
using System;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações das Permissões de cada oferta, 
    /// que um usuário possui.
    /// </summary>
    public class DTOfertaPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual int IdOferta { get; set; }
        public virtual int IdUsuario { get; set; }
        public virtual int IdTipoOferta { get; set; }
        public virtual int IdSolucaoEducacional { get; set; }
        public virtual bool PermiteGestorUC { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual DateTime? DataInicioInscricoes { get; set; }
        public virtual DateTime? DataFimInscricoes { get; set; }
    }
}
