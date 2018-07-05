using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EtapaEncaminhamentoUsuario : EntidadeBasica
    {
        public virtual EtapaResposta EtapaResposta { get; set; }   
        public virtual EtapaPermissaoNucleo EtapaPermissaoNucleo { get; set; }
        public virtual Usuario UsuarioEncaminhamento { get; set; }

        public virtual int StatusEncaminhamento { get; set; }
        public virtual DateTime? DataSolicitacaoEncaminhamento { get; set; }
        public virtual string Justificativa { get; set; }
        
    }
}
