using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewTrilha
    {
        public virtual Int64 Linha { get; set; }
        public virtual Trilha TrilhaOrigem { get; set; }
        public virtual TrilhaNivel TrilhaNivelOrigem { get; set; }
        public virtual TrilhaTopicoTematico TopicoTematico { get; set; }
        public virtual Usuario UsuarioOrigem { get; set; }
        public virtual ItemTrilha ItemTrilha { get; set; }
        public virtual FileServer Anexo { get; set; }
        public virtual int OrdemTrilhaNivel { get; set; }
        public virtual string Objetivo { get; set; }
        public virtual enumStatusSolucaoEducacionalSugerida Aprovado { get; set; }

        public virtual string StatusAprovacaoFormatado
        {
            get{
                switch (Aprovado){
                    case enumStatusSolucaoEducacionalSugerida.NaoAprovado:{
                            return "Não Aprovado";
                        }
                    case enumStatusSolucaoEducacionalSugerida.Aprovado:{
                            return "Aprovado";
                        }
                    default:{
                            return "Aguardando Aprovação";
                        }
                }
            }
        }
    }
}
