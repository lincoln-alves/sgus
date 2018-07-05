﻿using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class ItemTrilhaParticipacaoDTO
    {
        public virtual int ID { get; set; }
        public virtual string ArquivoParticipacao { get; set; }
        public virtual string TextoParticipacao { get; set; }
        public virtual string Orientacao { get; set; }
        public virtual string TipoArquivoParticipacao { get; set; }
        public virtual string NomeArquivoParticipacao { get; set; }
        public virtual string DataEnvio { get; set; }
        public virtual string Status { get; set; }
        public virtual string LinkAnexo { get; set; }
        public virtual bool Visualizado { get; set; }
        public virtual string Monitor { get; set; }
        public virtual string AutorParticipacao { get; set; }
        public virtual int TipoParticipacao { get; set; }

        public ItemTrilhaParticipacaoDTO()
        {
            Visualizado = false;
        }
    }
}