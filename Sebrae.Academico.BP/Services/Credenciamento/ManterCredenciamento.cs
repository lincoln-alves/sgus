using Newtonsoft.Json;
using Sebrae.Academico.BP.DTO.Services.Credenciamento;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sebrae.Academico.BP.Services.Credenciamento
{
    public class ManterCredenciamento : BusinessProcessServicesBase
    {
        public IEnumerable<DTOEvento> ObterEventos()
        {
            try
            {
                var url = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UrlCredenciamento).Registro;
                var json = JsonUtil.ObterJson(url + "evento/listartodos");

                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<DTOEvento>>(json).ToArray();
                }

                return new List<DTOEvento>();
            }
            catch (System.Exception)
            {
                throw new AcademicoException("Não foi possível recuperar os eventos do sistema de credenciamento");
            }
        }

        public IEnumerable<DTOEvento> ObterPresencasEventoFinalizado(DateTime data)
        {
            try
            {
                var url = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UrlCredenciamento).Registro;
                var json = JsonUtil.ObterJson(url + "evento/listarParticipacoesEmFinalizados/" + DateTime.Now.ToString("yyyy-MM-dd"));

                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<DTOEvento>>(json).ToArray();
                }

                return null;
            }
            catch (System.Exception)
            {
                throw new AcademicoException("Não foi possível recuperar os eventos do sistema de credenciamento");
            }
        }

        public void AtualizarMatriculas()
        {
            try
            {
                // Atualiza as matrículas de acordo com a data atual
                var presencas = ObterPresencasEventoFinalizado(DateTime.Now);

                AtualizarPresencas(presencas);

                using (var manter = new ManterMatriculaTurma())
                {
                    // Filtrando matriculas que contem algum dos eventos vinculado
                    var matriculasTurma = manter.ObterMatriculasComEventos(presencas.ToList());

                    foreach (var matricula in matriculasTurma)
                    {
                        var evento = presencas.FirstOrDefault(x => x.UsuarioCPF == matricula.MatriculaOferta.Usuario.CPF &&
                        x.ID == matricula.MatriculaOferta.Oferta.SolucaoEducacional.IDEvento);

                        if (evento != null)
                        {

                            if(evento.Presencas == 0)
                            {
                                matricula.MatriculaOferta.StatusMatricula = enumStatusMatricula.Abandono;
                            }

                            if (evento.Presencas < evento.PresencasMinimas)
                            {
                                matricula.MatriculaOferta.StatusMatricula = enumStatusMatricula.Reprovado;
                            }

                            if (evento.Presencas > evento.PresencasMinimas)
                            {
                                matricula.MatriculaOferta.StatusMatricula = enumStatusMatricula.Aprovado;
                            }
                        }
                    }

                    manter.Salvar(matriculasTurma.ToList());
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Atualiza Matriculas de acordo com os dados dos eventos do sistema de credenciamento
        /// </summary>
        /// <param name="presencas"></param>
        private void AtualizarPresencas(IEnumerable<DTOEvento> presencas)
        {
            using (var manter = new ManterMatriculaTurma())
            {
                // Filtrando matriculas que contem algum dos eventos vinculado
                var matriculasTurma = manter.ObterMatriculasComEventos(presencas.ToList());

                foreach (var matricula in matriculasTurma)
                {
                    var evento = presencas.FirstOrDefault(x => x.ID == matricula.MatriculaOferta.Oferta.SolucaoEducacional.IDEvento);

                    matricula.Presencas = evento.Presencas;
                    matricula.TotalPresencas = evento.PresencasMinimas != null ? evento.PresencasMinimas.Value : 0;
                }

                manter.Salvar(matriculasTurma.ToList());
            }
        }
    }
}
