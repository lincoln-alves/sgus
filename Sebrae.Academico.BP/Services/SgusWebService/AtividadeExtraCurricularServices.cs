using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.Services
{
    public class AtividadeExtraCurricularServices : BusinessProcessServicesBase
    {
        public DTOHistoricoExtraCurricular ConsultarExtraCurricular(int idAtividadeExtraCurricular)
        {
            var resultado = new DTOHistoricoExtraCurricular();
            var ativExtraCurricular = new BMHistoricoExtraCurricular().ObterPorID(idAtividadeExtraCurricular);
            //TODO: CARREGAR OS FORNECEDORES DINAMICAMENTE VIA AJAX para não ter problema de ter um fornecedor retornado no webservice que não esta aqui na hora
            if (ativExtraCurricular == null) return resultado;
            resultado.ID = ativExtraCurricular.ID;
            resultado.IdUsuario = ativExtraCurricular.Usuario.ID;
            resultado.NomeSolucaoExtraCurricular = ativExtraCurricular.SolucaoEducacionalExtraCurricular;
            resultado.TextoAtividade = ativExtraCurricular.TextoAtividade;
            resultado.CargaHoraria = ativExtraCurricular.CargaHoraria;
            resultado.DataFimAtividade = ativExtraCurricular.DataFimAtividade.HasValue ? ativExtraCurricular.DataFimAtividade.Value.ToString("dd/MM/yyyy") : "";
            resultado.DataInicioAtividade = ativExtraCurricular.DataInicioAtividade.HasValue ? ativExtraCurricular.DataInicioAtividade.Value.ToString("dd/MM/yyyy") : "";
            resultado.NomeInstituicao = ativExtraCurricular.Instituicao;
            if (ativExtraCurricular.FileServer == null) return resultado;
            resultado.NomeArquivoComprovacao = ativExtraCurricular.FileServer.NomeDoArquivoNoServidor;
            resultado.TipoArquivoComprovacao = ativExtraCurricular.FileServer.TipoArquivo;
            resultado.NomeArquivoOriginal = ativExtraCurricular.FileServer.NomeDoArquivoOriginal;
            if (ativExtraCurricular.Fornecedor != null && ativExtraCurricular.Fornecedor.ID != 0) resultado.IdFornecedor = ativExtraCurricular.Fornecedor.ID;
            return resultado;
        }

    }
}
