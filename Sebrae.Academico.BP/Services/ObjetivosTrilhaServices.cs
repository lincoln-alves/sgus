using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.Services
{
    public class ObjetivosTrilhaServices : BusinessProcessServicesBase
    {
        public DTOTrilhaObjetivos ConsultarObjetivosTrilha(UsuarioTrilha usuarioTrilha, int idTrilhaNivel) {
            var manterUsuariotrilha = new ManterUsuarioTrilha();

            var trilhaNivel = (new ManterTrilhaNivel()).ObterTrilhaNivelPorID(idTrilhaNivel);

            if (trilhaNivel == null) return null;

            var lista = manterUsuariotrilha.ObterObjetivostrilha(usuarioTrilha.Usuario.ID, idTrilhaNivel);

            var manterItemTrilhaPart = new ManterItemTrilhaParticipacao();

            foreach(var objetivo in lista)
            {
                objetivo.StatusObjetivo = manterItemTrilhaPart.UsuarioAprovadoObjetivo(usuarioTrilha, objetivo.IdObjetivo);
                objetivo.SolucoesObrigatorias = manterItemTrilhaPart.obtemSolucoesJogo(usuarioTrilha, objetivo.IdObjetivo);
            }

            var item = lista.FirstOrDefault();

            if (item == null) return null;            

            return new DTOTrilhaObjetivos{
                Objetivos = lista.ToList(),
                IdTrilha = trilhaNivel.Trilha.ID,
                IdTrilhaNivel = trilhaNivel.ID,
                NomeTrilha = trilhaNivel.Trilha.Nome,
                NomeTrilhaNivel = trilhaNivel.Nome
            };
        }

        public DTOTrilhaObjetivos ConsultarObjetivosPorChaveExterna(UsuarioTrilha usuarioTrilha, string chaveExterna)
        {            
            var result = (new ManterObjetivo()).ObterObjetivoPorFiltro( new Objetivo() { ChaveExterna = chaveExterna })
                                                 .Select(x => new DTOObjetivosTrilhas() { IdObjetivo = x.ID, ChaveExterna = x.ChaveExterna, NomeObjetivo = x.Nome }).FirstOrDefault<DTOObjetivosTrilhas>();

            var listaObj = new List<DTOObjetivosTrilhas>();

            if (result != null) {
                var manterItemTrilhaPart = new ManterItemTrilhaParticipacao();
                result.StatusObjetivo = manterItemTrilhaPart.UsuarioAprovadoObjetivo(usuarioTrilha, result.IdObjetivo);
                result.SolucoesObrigatorias = manterItemTrilhaPart.obtemSolucoesJogo(usuarioTrilha, result.IdObjetivo);

                listaObj.Add(result);
            }                        

            return new DTOTrilhaObjetivos
            {
                Objetivos = listaObj,
                IdTrilha = usuarioTrilha.TrilhaNivel.Trilha.ID,
                IdTrilhaNivel = usuarioTrilha.TrilhaNivel.ID,
                NomeTrilha = usuarioTrilha.TrilhaNivel.Trilha.Nome,
                NomeTrilhaNivel = usuarioTrilha.TrilhaNivel.Nome
            };            
        }
    }
}
