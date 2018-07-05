using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSolucaoEducacionalPorCategoria
    {
        static string CategoriaPai(CategoriaConteudo categoria)
        {
            if(categoria == null)return "";
            if (categoria.CategoriaConteudoPai == null) return categoria.Nome;
            var ca = categoria.CategoriaConteudoPai;
            var resultado = categoria.Nome;
            while (ca != null){
                resultado = ca.Nome;
                ca = ca.CategoriaConteudoPai;
            }
            return resultado;
        }
        public IEnumerable<DTOSolucoesPorCategoria> Consultar(IEnumerable<int> idsCategorias, IEnumerable<int> pUfResponsavel)
        {
            return new BMSolucaoEducacional().ObterPorCategoria(idsCategorias.ToList(), pUfResponsavel.ToList())
                .OrderBy(se => se.CategoriaConteudo.Nome)
                .ThenBy(se => se.Nome)
                .Select(se => new DTOSolucoesPorCategoria
                {
                    SolucaoEducacional = se.Nome,
                    Categoria = CategoriaPai(se.CategoriaConteudo),
                    SubCategoria = se.CategoriaConteudo != null ? se.CategoriaConteudo.Nome : "-",
                    Fornecedor = se.Fornecedor != null ? se.Fornecedor.Nome : "-",
                    FormaAquisicao = se.FormaAquisicao != null ? se.FormaAquisicao.Nome : "-",
                    Ativo = se.Ativo ? "Sim" : "Não",
                    UFResponsavel = se.UFGestor != null ? se.UFGestor.Sigla : ""
                });
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }


    }
}
