using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterInforme : BusinessProcessBase
    {
        private readonly BMInforme _bmInforme;

        public ManterInforme()
        {
            _bmInforme = new BMInforme();
        }

        /// <summary>
        /// Buscar onde o valor numérico informado está contido no mês ou no ano do Informe.
        /// </summary>
        /// <param name="numero">Valor numérico a ser comparado.</param>
        /// <returns></returns>
        public IQueryable<Informe> ObterPorNumero(int numero)
        {
            return _bmInforme.ObterTodos().Where(x => x.Ano == numero || x.Mes == numero);
        }

        /// <summary>
        /// Buscar onde a observação, o nome da SE/Oferta/Turma do Informe ou o nome do usuário do envio possuam o texto informado.
        /// </summary>
        /// <param name="palavra">Texto a ser comparado.</param>
        /// <returns></returns>
        public IEnumerable<Informe> ObterPorNome(string palavra)
        {
            palavra = palavra.ToLower();

            return
                _bmInforme.ObterTodos()
                    .Where(
                        x =>
                            x.Turmas.Any(t =>
                                (t.Nome != null && t.Nome.ToLower().Contains(palavra)) ||
                                (t.Oferta != null && t.Oferta.Nome != null && t.Oferta.Nome.ToLower().Contains(palavra)) ||
                                (t.Oferta != null && t.Oferta.SolucaoEducacional != null && t.Oferta.SolucaoEducacional.Nome != null && t.Oferta.SolucaoEducacional.Nome.ToLower().Contains(palavra)))
                                ||
                            x.Envios.Any(e => e.Usuario != null && e.Usuario.Nome.ToLower().Contains(palavra)));
        }

        public IEnumerable<Informe> ObterTodos()
        {
            return _bmInforme.ObterTodos();
        }

        public IEnumerable<Informe> ObterPorMesAno(int mes, int ano)
        {
            return _bmInforme.ObterTodos().Where(x => x.Mes == mes && x.Ano == ano);
        }

        public Informe ObterPorId(int id)
        {
            return _bmInforme.ObterPorId(id);
        }

        public void Excluir(int id)
        {
            _bmInforme.Excluir(ObterPorId(id));
        }

        public void Salvar(Informe informe)
        {
            _bmInforme.Salvar(informe);
        }

        /// <summary>
        /// Obter a template HTML para o envio do e-mail.
        /// </summary>
        /// <param name="informe"></param>
        /// <returns></returns>
        public string ObterTemplateHTML(Informe informe)
        {
            // Você sangra?
            // VAI SANGRAR!!!!!!!

            var gruposEmailTemplate = new List<GruposEmailTemplate>();

            // Buscar cursos online.
            var turmasOnline =
                informe.Turmas.Where(t => !t.Oferta.SolucaoEducacional.FormaAquisicao.Presencial).AsQueryable();

            if (turmasOnline.Any())
            {
                gruposEmailTemplate.Add(new GruposEmailTemplate
                {
                    Nome = "Cursos Online",
                    IdsTurmas = turmasOnline.Select(t => t.ID).ToList()
                });
            }

            // Buscar cursos presenciais.
            var turmasPresenciais =
                informe.Turmas.Where(t => t.Oferta.SolucaoEducacional.FormaAquisicao.Presencial).AsQueryable();

            if (turmasPresenciais.Any())
            {
                gruposEmailTemplate.Add(new GruposEmailTemplate
                {
                    Nome = "Cursos Presenciais",
                    IdsTurmas = turmasPresenciais.Select(t => t.ID).ToList()
                });
            }

            // Criar HTML com a turma informada.

            var grupos = "";

            var dateTimeFormat = "dd/MM/yyyy HH:mm";

            var dateFormat = "dd/MM/yyyy";

            foreach (var grupoEmailTemplate in gruposEmailTemplate)
            {
                var turmas = "";

                foreach (var idTurma in grupoEmailTemplate.IdsTurmas)
                {
                    var turma = informe.Turmas.FirstOrDefault(t => t.ID == idTurma);

                    var campos = string.Format(TemplateCampo, "Data Inicial", turma.DataInicio.ToString(dateTimeFormat));

                    campos += string.Format(TemplateCampo, "Data Final", turma.DataFinal.HasValue ? turma.DataFinal.Value.ToString(dateTimeFormat) : "");

                    campos += string.Format(TemplateCampo, "Local", turma.Local);

                    campos += string.Format(TemplateCampo, "Vagas", turma.Oferta.ObterVagasDisponiveis());

                    campos += string.Format(TemplateCampo, "Carga Horária", turma.Oferta.CargaHoraria);

                    campos += string.Format(TemplateCampo, "Data de início da inscrições", turma.Oferta.DataInicioInscricoes.HasValue ? turma.Oferta.DataInicioInscricoes.Value.ToString(dateFormat) : "");

                    campos += string.Format(TemplateCampo, "Data de fim  da inscrições", turma.Oferta.DataFimInscricoes.HasValue ? turma.Oferta.DataFimInscricoes.Value.ToString(dateFormat) : "");

                    campos += string.Format(TemplateCampo, "Público-Alvo", string.Join(",", turma.Oferta.ListaPublicoAlvo.Select(p => p.Nome)));

                    campos += string.Format(TemplateCampo, "Observação", turma.Oferta.InformacaoAdicional);

                    turmas += string.Format(TemplateCurso, turma.Oferta.SolucaoEducacional.Nome, campos);
                }

                grupos += string.Format(TemplateGrupo, grupoEmailTemplate.Nome, turmas);
            }

            var template = string.Format(TemplateSctructure, informe.ObterMesAno(), grupos);

            return template;
        }

        private static string TemplateSctructure = "<table width=\"100%\"><tr><td width=\"33%\" style=\"width: 33%\"></td><td width=\"33%\" style=\"width: 33%\"><img src=\"cid:Header\"/><br /><strong style=\"font-family: Verdana, sans-serif; font-size: 12px;\">{0}</strong><div style=\"border: 1pt solid rgb(128, 150, 195); padding: 10px;\"><table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width: 100%; border-collapse: collapse\" style=\"font-size: 12pt; line-height: 115%; font-family: Verdana,sans-serif;\">{1}</table><br /><table><tr><td style=\"background: rgb(30,69,149)\" width=\"498\"><span style=\"font-size:7.5pt;line-height:115%;font-family:Verdana,sans-serif;color:white\">Para mais informações sobre os cursos, acesse o Portal UC Sebrae</span></td><td width=\"1\" style=\"width:1px;padding:0;background:white\"></td><td width=\"164\" style=\"background: rgb(30,69,149); text-align: center;\"><span style=\"font-size:7.5pt;line-height:115%;font-family:Verdana,sans-serif; color: white; text-align: center;\"><a href=\"http://www.uc.sebrae.com.br\" target=\"_blank\" style=\"color:white\">http://www.uc.sebrae.com.br</a></span></td></tr></table><img src=\"cid:Footer\" width=\"666\" /></div></td><td width=\"33%\" style=\"width: 33%\"></td></tr></table>";

        private static string TemplateGrupo =
            "<tr><td style=\"background: rgb(127,186,63); padding: 0; width: 11.25pt;\" width=\"15\"><img src=\"cid:RightArrow\" /></td><td style=\"background: rgb(127, 186, 63); color: white; font-family: Verdana, sans-serif; font-size: 7.5pt; font-weight: bold;\">{0}</td></tr><tr><td colspan=\"2\"><table cellspacing=\"0\" cellpadding=\"0\" style=\"margin: auto; width: 100%; border: 1px solid white;\" width=\"100%\"><tr><td style=\"background: rgb(229,242,215)\"></td><td style=\"background: rgb(229,242,215)\">{1}</td><td width=\"9\" valign=\"top\" style=\"width: 7.1pt; padding: 0; background: rgb(229,242,215)\"><span style=\"margin-left: 0; margin-top: 0; width: 10px; min-height: 10px\"><img src=\"cid:PageFlip\"></span></td></tr></table><br /></td></tr>";

        private static string TemplateCurso =
            "<p style=\"line-height: 65%;margin-top: 30px;\"><b><span style=\"font-size: 9pt; font-family: Verdana,sans-serif\">{0}</span></b></p>{1}";

        private static string TemplateCampo =
            "<p style=\"line-height: 0\"><span style=\"font-size: 9pt; font-family: Verdana,sans-serif\"><b>{0}:</b>&nbsp;{1}</span></p>";
    }

    class GruposEmailTemplate
    {
        public string Nome { get; set; }
        public List<int> IdsTurmas { get; set; }
    }
}
