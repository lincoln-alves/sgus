using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections;
using Sebrae.Academico.BP.Relatorios;


namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class RankingTrilhas : BusinessProcessServicesBase
    {

        public IList<DTORankingTrilha> ConsultarRankingTrilha(int pIdTrilha, int pIdTrilhaNivel)
        {

            IList<UsuarioTrilha> lstUt;
            IList<DTORankingTrilha> lstResult = new List<DTORankingTrilha>();

            // Se especificado o id de Nivel pega somente o ranking do nível
            var statusBanidos = new List<enumStatusMatricula>();
            statusBanidos.Add(enumStatusMatricula.CanceladoAdm);
            statusBanidos.Add(enumStatusMatricula.CanceladoAluno);
            statusBanidos.Add(enumStatusMatricula.CanceladoGestor);
            statusBanidos.Add(enumStatusMatricula.Abandono);

            if (pIdTrilhaNivel != 0)
            {
                var trilhaNivelBM = new BMTrilhaNivel();
                var tn = trilhaNivelBM.ObterPorID(pIdTrilhaNivel);

                // Remover status cancelados e retornar os resultados.
                lstUt = tn.ListaUsuarioTrilha.Where(y => !statusBanidos.Contains(y.StatusMatricula)).OrderByDescending(x => x.QTEstrelas).ToList();

                byte posicaoRank = 1;
                //lstResult = new List<DTORankingTrilha>();
                foreach (var ut in lstUt)
                {
                    lstResult.Add(new DTORankingTrilha()
                    {
                        Posicaoranking = posicaoRank,
                        IdUsuario = ut.ID,
                        EmailUsuario = ut.Usuario.Email,
                        CPFUsuario = ut.Usuario.CPF,
                        EstadoUsuario = ut.Usuario.Estado,
                        NomeUsuario = ut.Usuario.Nome,
                        UF = ut.Usuario.UF.Nome,
                        UFSigla = ut.Usuario.UF.Sigla,
                        DataInicio = ut.DataInicio.ToString("dd/MM/yyyy"),
                        DataFim = ut.DataFim.HasValue ? ut.DataFim.Value.ToString("dd/MM/yyyy") : string.Empty,
                        QuantidadeEstrelas = ut.QTEstrelas == null ? byte.Parse("0") : ut.QTEstrelas.Value,
                        QuantidadeEstrelasPossiveis = ut.QTEstrelasPossiveis == null ? byte.Parse("0") : ut.QTEstrelasPossiveis.Value,
                        StatusMatricula = ut.StatusMatricula
                    });
                    posicaoRank++;
                }

                // Se somente foi especificado o ID de trilha
            }
            else if (pIdTrilha != 0)
            {
                var rel = new RelatorioUsuarioTrilha();

                var ranking = rel.ConsultarRankingTrilhaUsuario(pIdTrilha).Where(x =>
                !statusBanidos.Contains((enumStatusMatricula)x.StatusMatricula)).ToList();

                byte posicaoRank = 1;

                lstResult = new List<DTORankingTrilha>();

                foreach (var ut in ranking)
                {
                    lstResult.Add(new DTORankingTrilha
                    {
                        Posicaoranking = posicaoRank,
                        IdUsuario = ut.ID_Usuario, // ut.ID
                        EmailUsuario = ut.Email, //ut.Usuario.Email
                        CPFUsuario = ut.CPF, //ut.Usuario.CPF
                        EstadoUsuario = ut.Estado, // ut.Usuario.Estado
                        NomeUsuario = ut.Nome, // ut.Usuario.Nome
                        UF = ut.UF_nome, //ut.Usuario.UF.Nome
                        UFSigla = ut.UF_sigla, // ut.Usuario.UF.Sigla
                        DataInicio = ut.DataInicio.ToString("dd/MM/yyyy"), // ut.DataInicio
                        DataFim = ut.DataFim.HasValue ? ut.DataFim.Value.ToString("dd/MM/yyyy") : string.Empty, // ut.DataFim
                        QuantidadeEstrelas = (byte)ut.QT_Estrelas, // ut.QTEstrelas
                        QuantidadeEstrelasPossiveis = (byte)ut.QT_EstrelasPossiveis, // ut.QTEstrelasPossiveis
                        StatusMatricula = (enumStatusMatricula)ut.StatusMatricula, // ut.StatusMatricula
                        Nivel = ut.Nivel
                    });
                    posicaoRank++;
                }

            }
            else
            {
                throw new Exception("Deve ser passada o ID da Trilha ou o ID do Nível da Trilha.");
            }

            return lstResult;
        }
    }
}
