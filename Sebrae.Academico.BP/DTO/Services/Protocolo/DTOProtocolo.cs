using System;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.Protocolo
{
    public class DTOProtocolo
    {
        public virtual int ID { get; set; }

        public virtual long Numero { get; set; }

        public virtual int ID_UsuarioRemetente { get; set; }

        public virtual int ID_UsuarioDestinatario { get; set; }

        public string NomeRemetente { get; set; }

        public string NomeDestinatario { get; set; }

        public virtual string DataEnvio { get; set; }

        public virtual string DataRecebimento { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string AssinaturaRecebimento { get; set; }

        public virtual string Despacho { get; set; }

        public virtual string DespachoReencaminhamento { get; set; }

        public virtual int? ProtocoloPai { get; set; }

        public virtual int Reencaminhado { get; set; }

        public virtual string Status { get; set; }

        public bool Arquivado { get; set; }

        public List<DTOFileServer> Anexos { get; set; }

        public DTOProtocolo()
        {
            Anexos = new List<DTOFileServer>();
        }
    }


    public class DtoProtocoloNovo
    {
        public int? IdProtocoloPai { get; set; }
        public int IdUsuarioRemetente { get; set; }
        public int IdUsuarioDestinatario { get; set; }
        public string NomeRemetente { get; set; }
        public string NomeDestinatario { get; set; }
        public string DataEnvio { get; set; }
        public string DataRecebimento { get; set; }
        public string Descricao { get; set; }
        public string DespachoReencaminhamento { get; set; }
        public string TipoArquivo { get; set; }
        public List<DTOFileServer> Anexos { get; set; }
        public bool? Arquivado { get; set; }
    }
}
