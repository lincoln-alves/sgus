using Sebrae.Academico.BP.DTO.Dominio;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.Questionario
{
    public class DTOItemQuestionarioParticipacaoOpcoes: DTOEntidadeBasica
    {
        public virtual List<bool?> RespostaSelecionada { get; set; }
        public virtual bool? RespostaCorreta { get; set; }
        public virtual bool? PossuiOpcaoVinculada { get; set; }
        public virtual int? IndexOpcaoSelecionada { get; set; }
        public virtual DTOItemQuestionarioParticipacaoOpcoes OpcaoVinculada { get; set; }
        public virtual string ValorRespostaSelecionada { get; set; }
    }
}
