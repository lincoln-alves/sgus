using System;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Data;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Sebrae.Academico.Extensions;

namespace Sebrae.Academico.Util.Classes
{
    public class CertificadoTemplateUtil
    {

        public static DataTable GerarDataTableComCertificado(int pIdMatriculaPrograma, int pIdMatriculaOferta,
            int pIdUsuarioTrilha, CertificadoTemplate certificadoTemplate, string jwtToken = null)
        {
            if (certificadoTemplate == null)
                throw new Exception("Não existem certificados disponíveis para emissão.");

            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("TX_Certificado"));
            dt.Columns.Add(new DataColumn("OB_Imagem", typeof (byte[])));

            var dr = dt.NewRow();

            dr["TX_Certificado"] = FormatarTextoCertificadoTrilhaTurma(certificadoTemplate.TextoDoCertificado,
                pIdMatriculaPrograma,
                pIdMatriculaOferta, pIdUsuarioTrilha);

            dr["OB_Imagem"] = ObterImagemBase64(certificadoTemplate.Imagem);

            if (certificadoTemplate.CertificadoTrilhas == true ||
                (!string.IsNullOrEmpty(certificadoTemplate.TextoCertificado2) &&
                 !string.IsNullOrEmpty(certificadoTemplate.Imagem2)))
            {
                dt.Columns.Add(new DataColumn("TX_Certificado2"));
                dt.Columns.Add(new DataColumn("OB_Image2", typeof (byte[])));

                // Se for certificado de trilhas, obtém o progresso do trilheiro e joga no verso.
                // Caso contrário, joga o texto do verso normalmente.
                if (certificadoTemplate.CertificadoTrilhas == true && jwtToken != null)
                {
                    // Semelhantemente ao relatório de questionários, existe um .aspx que gera
                    // o HTML, que será lido pelo reader e escrito no verso do certificado.
                    var requestUrl = new BMConfiguracaoSistema().ObterPorID(
                        (int) enumConfiguracaoSistema.EnderecoSGUS).Registro +
                                     "/Relatorios/CertificadoTemplate/VersoCertificadoTrilhas.aspx?token=" + jwtToken;

                    // Ler requisição a partir de uma URL informada.
                    var webRequest = (HttpWebRequest) WebRequest.Create(requestUrl);
                    webRequest.Method = "GET";
                    var myResp = webRequest.GetResponse();
                    var sr = new StreamReader(myResp.GetResponseStream(), System.Text.Encoding.UTF8);
                    var result = sr.ReadToEnd();
                    sr.Close();
                    myResp.Close();

                    // Escrever o verso do certificado. Em resumo, pega tudo entre o <start> e o </start>.
                    // Tem que forçar a remoção de toda a sujeira que o WebForms coloca no output, então
                    // foi criada a tag fictícia <start> só pra esse código abaixo.
                    // É bem feio, mas funciona bem.

                    var startTag = "<start>";
                    var endTag = "</start>";

                    var indexStart = result.IndexOf(startTag) + startTag.Length;
                    var indexEnd = result.IndexOf(endTag);

                    var verso = result.Substring(indexStart, indexEnd - indexStart);

                    // Substituir \r\n.
                    while (verso.Contains("\r\n"))
                        verso = verso.Replace("\r\n", "");

                    // Substituir espaços duplos.
                    while (verso.Contains("  "))
                        verso = verso.Replace("  ", " ");

                    dr["TX_Certificado2"] = verso;
                }
                else
                {
                    dr["TX_Certificado2"] = FormatarTextoCertificadoTrilhaTurma(certificadoTemplate.TextoCertificado2,
                        pIdMatriculaPrograma,
                        pIdMatriculaOferta, pIdUsuarioTrilha);
                }
                if (certificadoTemplate.Imagem2 != null)
                    dr["OB_Image2"] = ObterImagemBase64(certificadoTemplate.Imagem2);
            }

            dt.Rows.Add(dr);

            return dt;
        }

        public static DataTable GerarDataTableComCertificadoTutor(int pIdOferta, int pIdTurma, int pIdUsuario,
            CertificadoTemplate cf)
        {
            if (cf == null)
                throw new Exception("Não existem certificados disponíveis para emissão.");

            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("TX_Certificado"));
            dt.Columns.Add(new DataColumn("OB_Imagem", typeof (byte[])));

            var dr = dt.NewRow();
            dr["TX_Certificado"] = FormatarTextoCertificadoTutor(cf.TextoDoCertificado, pIdOferta, pIdTurma, pIdUsuario);
            dr["OB_Imagem"] = ObterImagemBase64(cf.Imagem);

            if (!string.IsNullOrEmpty(cf.TextoDoCertificado) && !string.IsNullOrEmpty(cf.Imagem2))
            {
                dt.Columns.Add(new DataColumn("TX_Certificado2"));
                dt.Columns.Add(new DataColumn("OB_Image2", typeof (byte[])));
                dr["TX_Certificado2"] = FormatarTextoCertificadoTutor(cf.TextoCertificado2, pIdOferta, pIdTurma,
                    pIdUsuario);
                dr["OB_Image2"] = ObterImagemBase64(cf.Imagem2);
            }

            dt.Rows.Add(dr);

            return dt;
        }

        /// <summary>
        /// Método usado para gerar o certificado de cursos normais e de trilha
        /// </summary>
        /// <param name="pTexto"></param>
        /// <param name="pIdMatriculaOferta"></param>
        /// <param name="pIdUsuarioTrilha"></param>
        /// <returns></returns>
        public static string FormatarTextoCertificadoTrilhaTurma(string pTexto, int pIdMatriculaPrograma,
            int pIdMatriculaOferta, int pIdUsuarioTrilha)
        {
            var tI = new CultureInfo("pt-Br", true).TextInfo;

            if (pIdMatriculaPrograma > 0)
            {
                var matriculaCapacitacao = new BMMatriculaCapacitacao().ObterPorId(pIdMatriculaPrograma);
                if (matriculaCapacitacao != null)
                {
                    var nomeUsuario = tI.ToTitleCase(matriculaCapacitacao.Usuario.Nome.ToLower());
                    var dataInicioTurma = string.Empty;
                    var dataFimTurma = string.Empty;

                    if (matriculaCapacitacao.ListaMatriculaTurmaCapacitacao != null &&
                        matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.Any())
                    {
                        if (matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault().TurmaCapacitacao !=
                            null &&
                            matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                                .TurmaCapacitacao.DataInicio.HasValue)
                        {
                            dataInicioTurma =
                                matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                                    .TurmaCapacitacao.DataInicio.Value.ToString("dd/MM/yyyy");
                        }
                        if (matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault().TurmaCapacitacao !=
                            null &&
                            matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                                .TurmaCapacitacao.DataFim.HasValue)
                        {
                            dataFimTurma =
                                matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                                    .TurmaCapacitacao.DataFim.Value.ToString("dd/MM/yyyy");
                        }
                    }

                    pTexto = pTexto
                        .Replace("#CODIGOCERTIFICADO", matriculaCapacitacao.CDCertificado)
                        .Replace("#DATEGERACAOCERTIFICADO",
                            matriculaCapacitacao.DataGeracaoCertificado.HasValue
                                ? matriculaCapacitacao.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#DATAGERACAOCERTIFICADO",
                            matriculaCapacitacao.DataGeracaoCertificado.HasValue
                                ? matriculaCapacitacao.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#DATASISTEMAEXTENSO", DateTime.Now.ToLongDateString().ToString())
                        .Replace("#DATASISTEMA", DateTime.Now.ToString("dd/MM/yyyy"))
                        .Replace("#DATAHORASISTEMA", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                        .Replace("#ALUNO", nomeUsuario)
                        .Replace("#CPF", WebFormHelper.FormatarCPF(matriculaCapacitacao.Usuario.CPF))
                        .Replace("#EMAILALUNO", matriculaCapacitacao.Usuario.Email)
                        .Replace("#OFERTA", matriculaCapacitacao.Capacitacao.Nome)
                        .Replace("#NOMEPROGRAMA", matriculaCapacitacao.Capacitacao.Programa.Nome)
                        .Replace("#DATAINICIOOFERTA", matriculaCapacitacao.Capacitacao.DataInicio.ToString("dd/MM/yyyy"))
                        .Replace("#DATAFIMOFERTA",
                            matriculaCapacitacao.Capacitacao.DataFim.HasValue
                                ? matriculaCapacitacao.Capacitacao.DataFim.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#DATAMATRICULA",
                            matriculaCapacitacao.ListaMatriculaTurmaCapacitacao != null
                                ? matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                                    .DataMatricula.ToString("dd/MM/yyyy")
                                : string.Empty);

                    var matriculaTurmaCapacitacao = matriculaCapacitacao.ListaMatriculaTurmaCapacitacao != null
                        ? matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                        : null;

                    var dataTermino = matriculaTurmaCapacitacao != null &&
                                      matriculaTurmaCapacitacao.MatriculaCapacitacao != null &&
                                      matriculaTurmaCapacitacao.MatriculaCapacitacao.DataFim.HasValue
                        ? matriculaTurmaCapacitacao.MatriculaCapacitacao.DataFim.Value.ToString("dd/MM/yyyy")
                        : "";

                    pTexto = pTexto.Replace("#DATATERMINO", dataTermino);

                    pTexto.Replace("#TURMA",
                        matriculaCapacitacao.ListaMatriculaTurmaCapacitacao != null
                            ? matriculaCapacitacao.ListaMatriculaTurmaCapacitacao.FirstOrDefault()
                                .TurmaCapacitacao.Nome
                            : string.Empty)
                        .Replace("#STATUS", matriculaCapacitacao.StatusMatriculaFormatado)
                        .Replace("#DATAINICIOTURMA", dataInicioTurma)
                        .Replace("#DATAFIMTURMA", dataFimTurma);


                    return pTexto;
                }
            }

            if (pIdMatriculaOferta > 0)
            {
                MatriculaOferta matriculaOferta = new BMMatriculaOferta().ObterPorID(pIdMatriculaOferta);
                if (matriculaOferta != null)
                {
                    var nomeUsuario = tI.ToTitleCase(matriculaOferta.Usuario.Nome.ToLower());

                    var dataInicioTurma = string.Empty;
                    var dataFimTurma = string.Empty;

                    var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();

                    if (matriculaOferta.MatriculaTurma != null && matriculaTurma != null)
                    {
                        if (matriculaTurma.Turma != null)
                        {
                            dataInicioTurma = matriculaTurma.Turma.DataInicio.ToString("dd/MM/yyyy");
                        }
                        if (matriculaTurma.Turma != null && matriculaTurma.Turma.DataFinal.HasValue)
                        {
                            dataFimTurma = matriculaTurma.Turma.DataFinal.Value.ToString("dd/MM/yyyy");
                        }
                    }

                    int totalHoras = matriculaOferta.Oferta.SolucaoEducacional.ListaItemTrilha
                        .Where(x => x.Aprovado == enumStatusSolucaoEducacionalSugerida.Aprovado)
                        .Sum(x => x.CargaHoraria);

                    return pTexto
                        .Replace("#DATASISTEMAEXTENSO", DateTime.Now.ToLongDateString().ToString())
                        .Replace("#DATASISTEMA", DateTime.Now.ToString("dd/MM/yyyy"))
                        .Replace("#DATAHORASISTEMA", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                        .Replace("#ALUNO", nomeUsuario)
                        .Replace("#CPF", WebFormHelper.FormatarCPF(matriculaOferta.Usuario.CPF))
                        .Replace("#EMAILALUNO", matriculaOferta.Usuario.Email)
                        .Replace("#CARGAHORARIASOLUCAOSEBRAE", totalHoras.ToString())
                        .Replace("#OFERTA", matriculaOferta.Oferta.Nome)
                        .Replace("#CARGAHORARIA", matriculaOferta.Oferta.CargaHoraria.ToString())
                        .Replace("#NOMESE", matriculaOferta.Oferta.SolucaoEducacional.Nome)
                        .Replace("#DATAINICIOOFERTA",
                            matriculaTurma != null ? matriculaTurma.Turma.DataInicio.ToString("dd/MM/yyyy") : "")
                        .Replace("#DATAFIMOFERTA",
                            matriculaTurma.Turma.DataFinal.HasValue
                                ? matriculaTurma.Turma.DataFinal.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#DATAMATRICULA",
                            matriculaOferta.MatriculaTurma != null
                                ? matriculaTurma.DataMatricula.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#DATATERMINO",
                            matriculaOferta.MatriculaTurma != null &&
                            matriculaTurma.DataTermino.HasValue
                                ? matriculaTurma
                                    .DataTermino.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#EMENTA", matriculaOferta.Oferta.SolucaoEducacional.Ementa)
                        .Replace("#TURMA",
                            matriculaOferta.MatriculaTurma != null ? matriculaTurma.Turma.Nome : string.Empty)
                        .Replace("#PROFESSOR", matriculaTurma != null && matriculaTurma.Turma.Professores.Any()
                            ? string.Join(", ", matriculaTurma.Turma.Professores.Select(x => x.Nome).ToArray())
                            : string.Empty)
                        .Replace("#LOCAL", matriculaTurma.Turma.Local != null ? matriculaTurma.Turma.Local : "Sem Local")
                        .Replace("#CODIGOCERTIFICADO", matriculaOferta.CDCertificado)
                        .Replace("#MEDIAFINALTURMA",
                            matriculaOferta.MatriculaTurma != null &&
                            matriculaTurma.MediaFinal.HasValue
                                ? matriculaTurma.MediaFinal.Value.ToString()
                                : string.Empty)
                        .Replace("#DATEGERACAOCERTIFICADO",
                            matriculaOferta.DataGeracaoCertificado.HasValue
                                ? matriculaOferta.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#DATAGERACAOCERTIFICADO",
                            matriculaOferta.DataGeracaoCertificado.HasValue
                                ? matriculaOferta.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy")
                                : string.Empty)
                        .Replace("#NOTAFINAL",
                            matriculaOferta.MatriculaTurma != null &&
                            matriculaTurma.MediaFinal.HasValue
                                ? matriculaTurma.MediaFinal.Value.ToString()
                                : string.Empty)
                        .Replace("#STATUS", matriculaOferta.StatusMatriculaFormatado)
                        .Replace("#DATAINICIOTURMA", dataInicioTurma)
                        .Replace("#DATAFIMTURMA", dataFimTurma)
                        .Replace("#TEXTOPORTAL", matriculaOferta.Oferta.SolucaoEducacional.Apresentacao != null
                            ? Regex.Replace(matriculaOferta.Oferta.SolucaoEducacional.Apresentacao, "<.*?>",
                                string.Empty)
                            : string.Empty)
                        .Replace("#INFORMACOESADICIONAIS", matriculaOferta.Oferta.InformacaoAdicional != null
                            ? Regex.Replace(matriculaOferta.Oferta.InformacaoAdicional, "<.*?>", string.Empty)
                            : string.Empty)
                        .Replace("#AREATEMATICA", matriculaOferta.Oferta.SolucaoEducacional.ListaAreasTematicas.Any()
                            ? string.Join(", ",
                                matriculaOferta.Oferta.SolucaoEducacional.ListaAreasTematicas.Select(
                                    x => x.AreaTematica.Nome).ToArray())
                            : string.Empty)
                        ;
                }
            }

            if (pIdUsuarioTrilha > 0)
            {
                MatriculaOferta matriculaOferta = new BMMatriculaOferta().ObterPorID(pIdMatriculaOferta);

                var bmUsuarioTrilha = new BMUsuarioTrilha();
                var usuarioTrilha = bmUsuarioTrilha.ObterPorId(pIdUsuarioTrilha);
                if (usuarioTrilha == null || !usuarioTrilha.StatusMatricula.Equals(enumStatusMatricula.Aprovado))
                    return "Erro ao gerar o certificado";
                var nomeUsuario = tI.ToTitleCase(usuarioTrilha.Usuario.Nome.ToLower());

                var cargaHoraria = bmUsuarioTrilha.ObterTotalCargaHoraria(usuarioTrilha);

                return pTexto
                    .Replace("#DATASISTEMAEXTENSO", DateTime.Now.ToLongDateString().ToString())
                    .Replace("#DATASISTEMA", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("#DATAHORASISTEMA", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                    .Replace("#ALUNO", nomeUsuario)
                    .Replace("#CPF", WebFormHelper.FormatarCPF(usuarioTrilha.Usuario.CPF))
                    .Replace("#EMAILALUNO", usuarioTrilha.Usuario.Email)
                    .Replace("#TRILHANIVEL", usuarioTrilha.TrilhaNivel.Nome)
                    .Replace("#TRILHA", usuarioTrilha.TrilhaNivel.Trilha.Nome)
                    .Replace("#DATALIMITE", usuarioTrilha.DataLimite.ToString("dd/MM/yyyy"))
                    .Replace("#DATAINICIOTRILHA", usuarioTrilha.DataInicio.ToString("dd/MM/yyyy"))
                    .Replace("#CARGAHORARIASOLUCAOSEBRAE", cargaHoraria.ToString())
                    .Replace("#DATAFIMTRILHA",
                        usuarioTrilha.DataFim.HasValue
                            ? usuarioTrilha.DataFim.Value.ToString("dd/MM/yyyy")
                            : string.Empty)
                    .Replace("#MEDIAFINALTRILHA",
                        usuarioTrilha.NotaProva.HasValue ? usuarioTrilha.NotaProva.Value.ToString() : string.Empty)
                    .Replace("#CODIGOCERTIFICADO", usuarioTrilha.CDCertificado)
                    .Replace("#CARGAHORARIA", cargaHoraria.ToString())
                    .Replace("#DATEGERACAOCERTIFICADO",
                        usuarioTrilha.DataGeracaoCertificado.HasValue
                            ? usuarioTrilha.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy")
                            : string.Empty)
                    .Replace("#DATAGERACAOCERTIFICADO",
                        usuarioTrilha.DataGeracaoCertificado.HasValue
                            ? usuarioTrilha.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy")
                            : string.Empty);
            }
            return "Erro ao gerar o certificado";
        }

        public static string FormatarTextoCertificadoTutor(string texto, int idOferta, int idTurma, int idUsuario)
        {
            var tI = new CultureInfo("pt-Br", true).TextInfo;

            if (idOferta > 0 && idUsuario > 0)
            {
                var oferta = new BMOferta().ObterPorId(idOferta);

                var professor = new BMUsuario().ObterPorId(idUsuario);

                if (oferta == null || professor == null) return "Erro ao gerar o certificado";

                var turma = new BMTurma().ObterPorID(idTurma);

                if (turma == null)
                    return "Erro ao gerar o certificado";

                var dataInicioTurma = turma.DataInicio.ToString("dd/MM/yyyy");

                string dataFimTurma = turma.DataFinal.HasValue
                    ? turma.DataFinal.Value.ToString("dd/MM/yyyy")
                    : "";

                var nomeUsuario = tI.ToTitleCase(professor.Nome.ToLower());
                                
                int totalHoras = oferta.SolucaoEducacional.ListaItemTrilha
                        .Where(x => x.Aprovado == enumStatusSolucaoEducacionalSugerida.Aprovado)
                        .Sum(x => x.CargaHoraria);

                return texto
                    .Replace("#DATASISTEMAEXTENSO", DateTime.Now.ToLongDateString())
                    .Replace("#DATASISTEMA", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("#DATAHORASISTEMA", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                    .Replace("#PROFESSOR", nomeUsuario != null ? nomeUsuario : string.Empty)
                    .Replace("#LOCAL", turma.Local != null ? turma.Local : "Sem Local")
                    .Replace("#CPF", WebFormHelper.FormatarCPF(professor.CPF))
                    .Replace("#EMAILPROFESSOR", professor.Email)
                    .Replace("#OFERTA", oferta.Nome)
                    .Replace("#CARGAHORARIA", oferta.CargaHoraria.ToString())
                    .Replace("#NOMESE", oferta.SolucaoEducacional.Nome)
                    .Replace("#EMENTA", oferta.SolucaoEducacional.Ementa)
                    .Replace("#DATEGERACAOCERTIFICADO", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("#DATAGERACAOCERTIFICADO", DateTime.Now.ToString("dd/MM/yyyy"))
                    .Replace("#CODIGOCERTIFICADO",
                        oferta.CertificadoTemplateProfessor.ObterCodigoCertificadoTutor(idOferta, idTurma, idUsuario))
                    .Replace("#DATAINICIOTURMA", dataInicioTurma)
                    .Replace("#DATAFIMTURMA.", dataFimTurma)
                    .Replace("#TEXTOPORTAL", oferta.SolucaoEducacional.Apresentacao != null
                        ? Regex.Replace(oferta.SolucaoEducacional.Apresentacao, "<.*?>", string.Empty)
                        : string.Empty)
                    .Replace("#INFORMACOESADICIONAIS", oferta.InformacaoAdicional != null
                        ? Regex.Replace(oferta.InformacaoAdicional, "<.*?>", string.Empty)
                        : string.Empty)
                    .Replace("#AREATEMATICA", oferta.SolucaoEducacional.ListaAreasTematicas.Any()
                        ? string.Join(", ",
                            oferta.SolucaoEducacional.ListaAreasTematicas.Select(x => x.AreaTematica.Nome).ToArray())
                        : string.Empty)
                    .Replace("#CARGAHORARIASOLUCAOSEBRAE", totalHoras.ToString());
            }

            return "Erro ao gerar o certificado";
        }

        public static byte[] ObterImagemBase64(string pImagem)
        {
            string str64 = pImagem.Substring(pImagem.IndexOf(",") + 1);
            return Convert.FromBase64String(str64);
        }

        public static CertificadoTemplate ConsultarCertificado(int pIdMatriculaPrograma, int pIdMatriculaOferta,
            int pIdUsuarioTrilha)
        {
            return ConsultarCertificado(pIdMatriculaPrograma, pIdMatriculaOferta, pIdUsuarioTrilha, 0);
        }

        public static CertificadoTemplate ConsultarCertificadoTutor(int idOferta)
        {
            return new BMCertificadoTemplate().ObterCertificadoTutorPorOferta(idOferta);
        }

        public static CertificadoTemplate ConsultarCertificado(int pIdMatriculaPrograma, int pIdMatriculaOferta,
            int pIdUsuarioTrilha, int emitidoPeloGestor)
        {
            if (pIdMatriculaPrograma > 0)
            {
                var matriculaCapacitacao = (new BMMatriculaCapacitacao()).ObterPorId(pIdMatriculaPrograma);
                if (matriculaCapacitacao == null) return null;
                if (matriculaCapacitacao.Capacitacao.Certificado == null) return null;
                if (matriculaCapacitacao.Capacitacao.Certificado.ID == 0) return null;
                if (matriculaCapacitacao.StatusMatricula == enumStatusMatricula.Aprovado ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.Concluido ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoMultiplicador ||
                    matriculaCapacitacao.StatusMatricula ==
                    enumStatusMatricula.AprovadoComoMultiplicadorComAcompanhamento ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitador ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitadorComAcompanhamento ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoConsultor ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoConsultorComAcompanhamento ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoModerador ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoModeradorComAcompanhamento |
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitadorConsultor ||
                    matriculaCapacitacao.StatusMatricula == enumStatusMatricula.AprovadoComoGestor)
                {

                    if (string.IsNullOrEmpty(matriculaCapacitacao.CDCertificado))
                    {
                        matriculaCapacitacao.CDCertificado = WebFormHelper.ObterStringAleatoria();
                        matriculaCapacitacao.DataGeracaoCertificado = DateTime.Now;
                        if (emitidoPeloGestor > 0)
                        {
                            matriculaCapacitacao.CertificadoEmitidoPorGestor = emitidoPeloGestor;
                        }
                        new BMMatriculaCapacitacao().Salvar(matriculaCapacitacao);
                    }

                    return new BMCertificadoTemplate().ObterPorID(matriculaCapacitacao.Capacitacao.Certificado.ID);
                }
                return null;
            }

            if (pIdMatriculaOferta > 0)
            {
                var mo = new BMMatriculaOferta().ObterPorID(pIdMatriculaOferta);

                if (!(mo != null && mo.Oferta.CertificadoTemplate != null && mo.Oferta.CertificadoTemplate.ID > 0))
                    return null;

                if (mo.StatusMatricula == enumStatusMatricula.Aprovado ||
                    mo.StatusMatricula == enumStatusMatricula.Concluido ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoMultiplicador ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoMultiplicadorComAcompanhamento ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitador ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitadorComAcompanhamento ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoConsultor ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoConsultorComAcompanhamento ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoModerador ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoModeradorComAcompanhamento |
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitadorConsultor ||
                    mo.StatusMatricula == enumStatusMatricula.AprovadoComoGestor
                    )
                {
                    if (string.IsNullOrEmpty(mo.CDCertificado))
                    {
                        mo.CDCertificado = WebFormHelper.ObterStringAleatoria();
                        mo.DataGeracaoCertificado = DateTime.Now;
                        if (emitidoPeloGestor > 0)
                        {
                            mo.CertificadoEmitidoPorGestor = emitidoPeloGestor;
                        }
                        new BMMatriculaOferta().Salvar(mo);
                    }

                    return new BMCertificadoTemplate().ObterPorID(mo.Oferta.CertificadoTemplate.ID);
                }

                return null;
            }

            if (pIdUsuarioTrilha > 0)
            {
                var bmUsuarioTrilha = new BMUsuarioTrilha();
                UsuarioTrilha ut = bmUsuarioTrilha.ObterPorId(pIdUsuarioTrilha);

                if (
                    !(ut != null && ut.TrilhaNivel.CertificadoTemplate != null &&
                      ut.TrilhaNivel.CertificadoTemplate.ID > 0))
                    return null;

                // inicio #2720
                // ajuste feito pois não existe status concluido para usuariotrilha, deve ser aprovado.
                if (ut.StatusMatricula == enumStatusMatricula.Concluido)
                {
                    ut.StatusMatricula = enumStatusMatricula.Aprovado;
                    bmUsuarioTrilha.Salvar(ut);
                }
                // fim #2720

                if (ut.StatusMatricula == enumStatusMatricula.Aprovado)
                {
                    if (string.IsNullOrEmpty(ut.CDCertificado))
                    {
                        ut.CDCertificado = WebFormHelper.ObterStringAleatoria();
                        ut.DataGeracaoCertificado = DateTime.Now;
                        bmUsuarioTrilha.Salvar(ut);
                    }

                    return new BMCertificadoTemplate().ObterPorID(ut.TrilhaNivel.CertificadoTemplate.ID);
                }
                else
                    return null;
            }

            else
                return null;
        }

        public static byte[] RetornarCertificado(CertificadoTemplate cf, DataTable dt)
        {
            try
            {
                string caminhoReport;
                if (cf.CertificadoTrilhas == true)
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate2paginas.rdlc";
                else if (string.IsNullOrWhiteSpace(cf.Imagem2) && string.IsNullOrWhiteSpace(cf.TextoCertificado2))
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate.rdlc";
                else
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate2paginas.rdlc";

                var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                var assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
                var stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);
                var rv = new ReportViewer();
                rv.LocalReport.LoadReportDefinition(stream);
                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("dsCertificadoTemplate", dt));
                return rv.LocalReport.Render("PDF");
            }
            catch(Exception e)
            {
                throw new ResponseException(enumResponseStatusCode.CertificadoInexistente, "Erro ao gerar o certificado");
            }
        }

        public static byte[] ConsultarCertificadoCertame(int certificadoId, string cpf)
        {
            var usuario = new BMUsuario().ObterPorCPF(cpf);

            if (usuario == null)
                return null;

            var certificadoUsuario =
                usuario.ListaUsuarioCertificadoCertame.FirstOrDefault(x => x.CertificadoCertame.ID == certificadoId);

            if (certificadoUsuario == null)
                return null;

            var certificado = certificadoUsuario.CertificadoCertame;

            // Removendo caracteres especiais que travam no chrome.
            certificado.Certificado.NomeDoArquivoOriginal =
                RemoverCaracterEspecial(certificado.Certificado.NomeDoArquivoOriginal);

            // Create the output document
            var outputDocument = new PdfDocument {PageLayout = PdfPageLayout.TwoPageLeft};

            var font = new XFont("Verdana", 13);
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Center
            };

            try
            {

                var repositorioUpload =
                    ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                var filePath = string.Concat(repositorioUpload, @"\", certificado.Certificado.NomeDoArquivoNoServidor);

                using (var ms = new MemoryStream())
                {
                    using (
                        var file =
                            new FileStream(
                                filePath,
                                FileMode.Open,
                                FileAccess.Read
                                )
                        )
                    {
                        var bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int) file.Length);
                        ms.Write(bytes, 0, (int) file.Length);
                    }

                    // Por alguma bizarrice, tem que recriar o Stream.
                    var form = XPdfForm.FromStream(new MemoryStream(ms.ToArray()));

                    // Escrever FRENTE do certificado.
                    EscreverFrente(outputDocument, form, certificadoUsuario, font, format);

                    // Escrever VERSO do certificado.
                    EscreverVerso(outputDocument, form, certificadoUsuario, font, format);

                    var streamOutput = new MemoryStream();
                    outputDocument.Save(streamOutput, false);

                    return streamOutput.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        private static void EscreverVerso(PdfDocument outputDocument, XPdfForm form,
            UsuarioCertificadoCertame certificadoUsuario,
            XFont font, XStringFormat format)
        {
            if (form.PageCount > 1)
            {

                var paginaVerso = outputDocument.AddPage();
                paginaVerso.Orientation = PageOrientation.Landscape;
                paginaVerso.Size = PageSize.A4;

                form.PageNumber = 2;

                var posicaoId = new BMPosicaoDadoCertificadoCertame().ObterPorDadoAno("id",
                    certificadoUsuario.CertificadoCertame.Ano);

                if (posicaoId == null)
                    throw new Exception("Posição de dado obrigatório não informada no verso");

                var gfx = XGraphics.FromPdfPage(paginaVerso);
                gfx.DrawImage(form, new XRect(0, 0, form.PointWidth, form.PointHeight));

                EscreverDado(paginaVerso, posicaoId, gfx, font, format, certificadoUsuario.Chave);
            }
        }

        private static void EscreverFrente(PdfDocument outputDocument, XPdfForm form,
            UsuarioCertificadoCertame certificadoUsuario,
            XFont font, XStringFormat format)
        {
            var paginaFrente = outputDocument.AddPage();
            paginaFrente.Orientation = PageOrientation.Landscape;
            paginaFrente.Size = PageSize.A4;

            form.PageNumber = 1;

            var manterPosicaoDado = new BMPosicaoDadoCertificadoCertame();

            var posicaoNome = manterPosicaoDado.ObterPorDadoAno("nome", certificadoUsuario.CertificadoCertame.Ano);
            var posicaoCpf1 = manterPosicaoDado.ObterPorDadoAno("cpf1", certificadoUsuario.CertificadoCertame.Ano);
            var posicaoCpf2 = manterPosicaoDado.ObterPorDadoAno("cpf2", certificadoUsuario.CertificadoCertame.Ano);

            if (posicaoNome == null || posicaoCpf1 == null || posicaoCpf2 == null)
                throw new Exception("Posição de dado obrigatório não informada na frente");

            var gfx = XGraphics.FromPdfPage(paginaFrente);
            gfx.DrawImage(form, new XRect(0, 0, form.PointWidth, form.PointHeight));

            EscreverDado(paginaFrente, posicaoNome, gfx, font, format, certificadoUsuario.Usuario.Nome);
            EscreverDado(paginaFrente, posicaoCpf1, gfx, font, format,
                certificadoUsuario.Usuario.CPF.Substring(0, 9).Insert(3, ".").Insert(7, "."));
            EscreverDado(paginaFrente, posicaoCpf2, gfx, font, format, certificadoUsuario.Usuario.CPF.Substring(9, 2));

            if (certificadoUsuario.CertificadoCertame.Data.HasValue)
            {
                var data = certificadoUsuario.CertificadoCertame.Data.Value;

                var dataFormatada = string.Format("{0} de {1} de {2}", data.Day.ToString().PadLeft(2, '0'),
                    data.ObterNomeMes(), data.Year);

                var posicaoData = manterPosicaoDado.ObterPorDadoAno("data", certificadoUsuario.CertificadoCertame.Ano);

                EscreverDado(paginaFrente, posicaoData, gfx, font, format, dataFormatada);
            }

            if (!string.IsNullOrWhiteSpace(certificadoUsuario.CertificadoCertame.NomeCertificado))
            {
                var posicaoNomeCertificado = manterPosicaoDado.ObterPorDadoAno("nomecertificado",
                    certificadoUsuario.CertificadoCertame.Ano);

                // Somente nesse caso, só insere o nome no PDF caso tenha o dado, pois alguns PDFs possuem o
                // nome neles, mas o campo "Nome" é obrigatório em todos os certificados.
                if (posicaoNomeCertificado != null)
                {
                    var fontNomeCertificado = new XFont("Verdana", 13, XFontStyle.Bold);

                    EscreverDado(paginaFrente, posicaoNomeCertificado, gfx, fontNomeCertificado, format,
                        certificadoUsuario.CertificadoCertame.NomeCertificado.ToUpper());
                }
            }
        }

        private static void EscreverDado(PdfPage paginaFrente, PosicaoDadoCertificadoCertame posicao, XGraphics gfx,
            XFont font, XStringFormat format, string dado)
        {
            var boxNome = paginaFrente.MediaBox.ToXRect();
            boxNome.Inflate(0, -10);
            boxNome.Location = new XPoint(posicao.X, posicao.Y);
            gfx.DrawString(dado, font, XBrushes.Black, boxNome, format);
        }

        public static string RemoverCaracterEspecial(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public static byte[] ConsultarBoletimCertame(int certificadoId, string cpf, DateTime horaAtual)
        {
            var usuario = new BMUsuario().ObterPorCPF(cpf);

            var certificadoUsuario =
                usuario?.ListaUsuarioCertificadoCertame.FirstOrDefault(x => x.CertificadoCertame.ID == certificadoId);

            if (certificadoUsuario == null)
                return null;

            var repositorioArquivos =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;

            var boletimPath = string.IsNullOrWhiteSpace(certificadoUsuario.ArquivoBoletim)
                ? null
                : Path.Combine(repositorioArquivos, "BoletinsCertame", certificadoUsuario.ArquivoBoletim);

            // Para certificados de 2014 para trás, gera o certificado na hora.
            // Para 2015 em diante, busca do repositório de PDFs.
            // Também gera o boletim caso não exista ou o arquivo não seja encontrado.
            if (certificadoUsuario.CertificadoCertame.Ano <= 2014 ||
                string.IsNullOrWhiteSpace(certificadoUsuario.ArquivoBoletim) ||
                (boletimPath != null && File.Exists(boletimPath) == false))
            {
                return GerarBoletim(certificadoUsuario, repositorioArquivos, horaAtual);
            }

            return File.ReadAllBytes(boletimPath);
        }

        private static byte[] GerarBoletim(UsuarioCertificadoCertame certificadoUsuario, string repostorioArquivo, DateTime horaAtual)
        {
            var outputDocument = new PdfDocument {PageLayout = PdfPageLayout.SinglePage};

            var font = new XFont("Verdana", 12);
            var fontBold = new XFont("Verdana", 12, XFontStyle.Bold);
            var centerFormat = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Center
            };
            var leftFormat = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Center
            };

            var pagina = outputDocument.AddPage();
            pagina.Orientation = PageOrientation.Portrait;
            pagina.Size = PageSize.A4;

            var gfx = XGraphics.FromPdfPage(pagina);

            // Desenhar a logo do CESPE.
            gfx.DrawImage(XImage.FromFile(Path.Combine(repostorioArquivo, "BoletinsCertame", "cespe.png")),
                new XRect(new XPoint(10, 10), new XPoint(130, 50)));

            // Desenhar data e hora 
            gfx.DrawString(horaAtual.ToString("G", new CultureInfo("pt-BR")), new XFont("Verdana", 8), XBrushes.Black, pagina.Width / 2, 10, centerFormat);

            // Desenhar cabeçalho.
            gfx.DrawString("SEBRAE NACIONAL - Serviço de Apoio às Micro e Pequenas Empresas", font, XBrushes.Black,
                pagina.Width/2, 70, centerFormat);
            gfx.DrawString($"CERTIFICAÇÃO INTERNA DE CONHECIMENTOS - {certificadoUsuario.CertificadoCertame.Ano}", font,
                XBrushes.Black, pagina.Width/2, 90, centerFormat);
            gfx.DrawString("ESPELHO DE DESEMPENHO", font, XBrushes.Black, pagina.Width/2, 110, centerFormat);

            // Desenhar dados do usuário
            gfx.DrawString("Nome:", fontBold, XBrushes.Black, 50, 140, leftFormat);
            gfx.DrawString(certificadoUsuario.Usuario.Nome, font, XBrushes.Black, 130, 140, leftFormat);

            gfx.DrawString("CPF:", fontBold, XBrushes.Black, 50, 160, leftFormat);
            gfx.DrawString(certificadoUsuario.Usuario.CPF, font, XBrushes.Black, 130, 160, leftFormat);

            gfx.DrawString("Inscrição:", fontBold, XBrushes.Black, 50, 180, leftFormat);
            gfx.DrawString(certificadoUsuario.NumeroInscricao ?? "", font, XBrushes.Black, 130, 180, leftFormat);

            gfx.DrawString("Unidade:", fontBold, XBrushes.Black, 50, 200, leftFormat);
            gfx.DrawString(certificadoUsuario.Usuario.Unidade ?? "", font, XBrushes.Black, 130, 200, leftFormat);

            // Desenhar a tabela.
            DesenharTabela(gfx, certificadoUsuario, pagina);

            var streamOutput = new MemoryStream();
            outputDocument.Save(streamOutput, false);

            return streamOutput.ToArray();
        }

        private static void DesenharTabela(XGraphics gfx, UsuarioCertificadoCertame certificadoUsuario, PdfPage pagina)
        {
            var pen = new XPen(XColor.FromName("black"));

            double topoTabela = 250;

            DesenharCelula(gfx, 10, topoTabela, 220, 20, pen, "Área", true);
            DesenharCelula(gfx, 220, topoTabela, 440, 20, pen, "Situação de Comparecimento", true);
            DesenharCelula(gfx, 440, topoTabela, 500, 20, pen, "Nota", true);
            DesenharCelula(gfx, 500, topoTabela, 585, 20, pen, "Situação", true);

            topoTabela += 20;

            DesenharCelula(gfx, 10, topoTabela, 220, 20, pen, certificadoUsuario.CertificadoCertame.NomeCertificado);
            DesenharCelula(gfx, 220, topoTabela, 440, 20, pen, certificadoUsuario.Situacao?.GetDescription());
            DesenharCelula(gfx, 440, topoTabela, 500, 20, pen, certificadoUsuario.Nota?.ToString("0.00") ?? "0.00");
            DesenharCelula(gfx, 500, topoTabela, 585, 20, pen, $"{certificadoUsuario.Status.GetDescription()}");

            var medidaJustificativa = gfx.MeasureString(certificadoUsuario.Justificativa ?? " " , new XFont("Verdana", 12));

            var alturaJustificativa =
                medidaJustificativa.Height * Math.Ceiling(medidaJustificativa.Width / 585);

            topoTabela = pagina.Height - 50 - alturaJustificativa;

            DesenharCelula(gfx, 10, topoTabela, 585, 20, pen,
                "Resultado da apresentação de justificativa à ausência no dia da prova", true, true);

            topoTabela += 20;

            DesenharCelula(gfx, 10, topoTabela, 585, alturaJustificativa, pen,
                certificadoUsuario.Justificativa);
        }

        private static void DesenharCelula(XGraphics gfx, int inicioCelula, double topoTabela, int larguraCelula,
            double alturaLinha, XPen pen, string texto, bool titulo = false, bool tituloCentralizado = false)
        {
            var baseCelula = topoTabela + alturaLinha;
            
            gfx.DrawLine(pen, inicioCelula, topoTabela, larguraCelula, topoTabela);
            gfx.DrawLine(pen, inicioCelula, topoTabela, inicioCelula, baseCelula);
            gfx.DrawLine(pen, inicioCelula, baseCelula, larguraCelula, baseCelula);
            gfx.DrawLine(pen, larguraCelula, topoTabela, larguraCelula, baseCelula);

            if (string.IsNullOrWhiteSpace(texto) == false)
            {
                var font = new XFont("Verdana", 12, (titulo ? XFontStyle.Bold : XFontStyle.Regular));

                var format = new XStringFormat
                {
                    Alignment = (tituloCentralizado ? XStringAlignment.Center : XStringAlignment.Near),
                    LineAlignment = XLineAlignment.BaseLine
                };

                var tx = new XTextFormatter(gfx);

                if (tituloCentralizado)
                {
                    var textMeasure = gfx.MeasureString(texto, font);

                    gfx.DrawString(texto, font, XBrushes.Black, ((double)larguraCelula / 2), topoTabela + textMeasure.Height, format);
                }
                else
                {
                    tx.DrawString(texto, font, XBrushes.Black, new XRect(new XPoint(inicioCelula + 5, topoTabela + 5), new XPoint(larguraCelula, baseCelula)));
                }
            }
        }
    }
}