using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSolucoesTrilhas : BusinessManagerBase
    {
        public enum EnumOrigemSolucao
        {
            Todas,
            SolucaoSebrae,
            SolucaoTrilheiro
        }

        public List<DtoTrilhaSolucaoSebrae> ObterSolucoesMochila(int usuarioId, int usuarioTrilhaId)
        {
            return ObterSolucoes(EnumOrigemSolucao.Todas, false, usuarioId, usuarioTrilhaId);
        }

        private List<DtoTrilhaSolucaoSebrae> ObterSolucoes(EnumOrigemSolucao origem = EnumOrigemSolucao.Todas,
            bool? apenasAtivas = null, int? usuarioId = null, int? usuarioTrilhaId = null, int? trilhaNivelId = null,
            int? pontoSebraeId = null)
        {

            var parametros = new Dictionary<string, object>
            {
                {"OrigemSolucao", (int) origem},
                {"ApenasSSAtiva", apenasAtivas},
                {"ID_Usuario", usuarioId},
                {"ID_UsuarioTrilha", usuarioTrilhaId},
                {"ID_TrilhaNivel", trilhaNivelId},
                {"ID_PontoSebrae", pontoSebraeId}
            };

            // Por limitações do método para os enums, é necessário fazer um map manual.
            // Seria interessante utilizar o AutoMapper.
            return ExecutarProcedure<SolucoesProcedureReturn>("SP_OBTER_SOLUCOES", parametros).ToList().Select(x => new DtoTrilhaSolucaoSebrae
            {
                Id = x.Id,
                FormaAquisicaoId = x.FormaAquisicaoId,
                FormaAquisicao = x.FormaAquisicao,
                PontoSebraeId = x.PontoSebraeId,
                Nome = x.Nome,
                Orientacao = x.Orientacao,
                Moedas = x.Moedas,
                DonoTrilha = x.DonoTrilha,
                Tipo = (enumTipoItemTrilha?)x.Tipo,
                Status = (enumStatusParticipacaoItemTrilha?)x.Status,
                Origem = (enumOrigemItemTrilha)x.Origem,
                MatriculaOfertaId = x.MatriculaOfertaId,
                LinkAcesso = x.LinkAcesso,
            }).ToList();
        }

        private class SolucoesProcedureReturn
        {
            public int Id { get; set; }
            public int FormaAquisicaoId { get; set; }
            public string FormaAquisicao { get; set; }
            public int PontoSebraeId { get; set; }
            public string Nome { get; set; }
            public string Orientacao { get; set; }
            public int? Moedas { get; set; }
            public bool DonoTrilha { get; set; }
            public int? Tipo { get; set; }
            public int? Status { get; set; }
            public int Origem { get; set; }
            public int? MatriculaOfertaId { get; set; }
            public string LinkAcesso { get; set; }
        }
    }
}
