using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioUsuarioCadastrado : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.UsuarioCadastrado; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<Perfil> ObterPerfilTodos()
        {
            using (BMPerfil bmPerfil = new BMPerfil())
            {
                return bmPerfil.ObterTodos();
            }
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodos()
        {
            using (BMNivelOcupacional bmNivelOcupacional = new BMNivelOcupacional())
            {
                return bmNivelOcupacional.ObterTodos();
            }
        }

        public IList<StatusMatricula> ObterStatusMatriculaTodos()
        {
            BMStatusMatricula bmStatusMatricula = new BMStatusMatricula();
            return bmStatusMatricula.ObterTodos();
        }

        public IList<Uf> ObterUfTodos()
        {
            using (var bmUf = new BMUf())
            {
                return bmUf.ObterTodos();
            }
        }

        public IList<DTOUsuarioPorFiltro> ConsultarUsuarioPorFiltro(int pPerfil, int pNivelOcupacional, int pUF)
        {
            RegistrarLogExecucao();

            return (new ManterUsuario()).ConsultarUsuarioPorFiltro(pPerfil, pNivelOcupacional, pUF);
        }

        public DataSet PrepararDataSetParaRelatorio(IList<DTOUsuarioPorFiltro> lista,int pUf) {
            var ds = new DataSet();

            var listaUfs = ObterUfTodos();

            listaUfs = pUf == 0 ? listaUfs : listaUfs.Where(p => p.ID == pUf).ToList();

            var i = 1;
            foreach (var item in listaUfs) {
                if (!lista.Any(p => p.UF.Equals(item.Sigla))) continue;

                var tabela = new DataTable {
                    TableName = "Uf: " + item.Sigla,
                };

                tabela.Columns.Add("Nome");
                tabela.Columns.Add("Quantidade");

                var subQuery = lista.Where(p => p.UF.Equals(item.Sigla) && p.Tipo.ToLower().Equals("perfil")).ToList();
                foreach (var linha in subQuery) {
                    var row = tabela.NewRow();
                    row["Nome"] = linha.Nome;
                    row["Quantidade"] = linha.Quantidade;
                    tabela.Rows.Add(row);
                }

                var linhaVazia = tabela.NewRow();
                linhaVazia["Nome"] = "";
                linhaVazia["Quantidade"] = "";
                tabela.Rows.Add(linhaVazia);

                subQuery = lista.Where(p => p.UF.Equals(item.Sigla) && p.Tipo.ToLower().Equals("nivelocupacional")).ToList();
                foreach (var linha in subQuery){
                    var row = tabela.NewRow();
                    row["Nome"] = linha.Nome;
                    row["Quantidade"] = linha.Quantidade;
                    tabela.Rows.Add(row);
                }

                ds.Tables.Add(tabela);

                i++;
            }

            return ds;
        }
    }
}
