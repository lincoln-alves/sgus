using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;


namespace Sebrae.Academico.BP.Services.Mensageria
{
    public class Mensageria : BusinessProcessServicesBase
    {

        public void EnviarMensagensatraso()
        {
            try
            {
                IList<MensageriaParametros> lstMsgrParamentros = (new BMMensageriaParametro()).ObterTodos();

                foreach (MensageriaParametros msgrP in lstMsgrParamentros)
                {
                    if (msgrP.NotificaMatriculaTurma)
                        EnviarMensagematrasoPorMatriculaTurma(msgrP);

                    if (msgrP.NotificaUsuarioTrilha)
                        EnviarMensagemAtrasoPorUsuarioTrilha(msgrP);

                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }


        private void EnviarMensagematrasoPorMatriculaTurma(MensageriaParametros pMensgrParametro)
        {
            try
            {
                IList<MatriculaTurma> lstMatriculaTurma = new BMMatriculaTurma().ObterMatriculasDataExpiracao(pMensgrParametro.DiaAviso);

                BMMensageriaRegistro mensageriaRegistroBM = new BMMensageriaRegistro();

                foreach (MatriculaTurma mt in lstMatriculaTurma)
                {

                    if (!mensageriaRegistroBM.ValidarComunicacaoEfetuada(mt))
                    {
                        MensageriaRegistro mr = new MensageriaRegistro()
                        {
                            Auditoria = new Auditoria(null),
                            MatriculaTurma = mt,
                            MensageriaParametro = pMensgrParametro,
                            DataEnvio = DateTime.Now,
                            Usuario = mt.MatriculaOferta.Usuario
                        };

                        mr.TextoEnviado = TextoTemplateMensagem(mr);
                        //EnviarMensagem(mr);

                        mensageriaRegistroBM.Salvar(mr);
                    }

                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private void EnviarMensagemAtrasoPorUsuarioTrilha(MensageriaParametros pMensgrParametro)
        {

            IList<UsuarioTrilha> lstUsuarioTrilha = null;

            try
            {
                lstUsuarioTrilha = new BMUsuarioTrilha().ObterMatriculasDataExpiracao(pMensgrParametro.DiaAviso);

                BMMensageriaRegistro mensageriaRegistroBM = new BMMensageriaRegistro();

                foreach (UsuarioTrilha ut in lstUsuarioTrilha)
                {
                    if (!mensageriaRegistroBM.ValidarComunicacaoEfetuada(ut))
                    {
                        MensageriaRegistro mr = new MensageriaRegistro()
                        {
                            Auditoria = new Auditoria(null),
                            UsuarioTrilha = ut,
                            MensageriaParametro = pMensgrParametro,
                            DataEnvio = DateTime.Now,
                            Usuario = ut.Usuario
                        };

                        mr.TextoEnviado = TextoTemplateMensagem(mr);
                        EnviarMensagem(mr);

                        new BMMensageriaRegistro().Salvar(mr);
                    }
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

        }

        public void EnviarMensagem(MensageriaRegistro pMr)
        {
            try
            {
                // Pegando o arquivo de template;
                if (pMr.MensageriaParametro.EnviarNotificacao)
                {
                    Notificacao nt = new Notificacao()
                    {
                        DataGeracao = DateTime.Now,
                        TextoNotificacao = TextoTemplateMensagem(pMr)
                    };

                    new BMNotificacao().Salvar(nt);

                }

                if (pMr.MensageriaParametro.EnviarEmail)
                {
                    EmailUtil.Instancia.EnviarEmail(pMr.Usuario.Email, AssuntoTemplateMensagem(pMr), pMr.TextoEnviado);
                    //CommonHelper.EnviarEmail(string.Empty, pMr.Usuario.Email, AssuntoTemplateMensagem(pMr), pMr.TextoEnviado);
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private string TextoTemplateMensagem(MensageriaRegistro pMr)
        {
            StreamReader fl = null;
            string textoFormatado = null;

            try
            {
                fl = new StreamReader(CommonHelper.ObterArquivo("/template/" + pMr.MensageriaParametro.NomeArquivoTemplate));


                if (pMr.MensageriaParametro.EnviarNotificacao && !pMr.MensageriaParametro.EnviarEmail)
                    return FormatarTexto(fl.ReadToEnd(), pMr).Trim();



                //Eliminando a linha do assunto.
                fl.ReadLine();

                StringBuilder sb = new StringBuilder();

                while (!fl.EndOfStream)
                    sb.AppendLine(fl.ReadLine());

                return FormatarTexto(sb.ToString(), pMr).Trim();


            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
            finally
            {
                fl.Close();
                fl.Dispose();
            }

            return textoFormatado;

        }

        private string AssuntoTemplateMensagem(MensageriaRegistro pMr)
        {
            string textoFormatado = null;
            StreamReader fl = null;

            try
            {
                fl = new StreamReader(CommonHelper.ObterArquivo("/template/" + pMr.MensageriaParametro.NomeArquivoTemplate));
                string pTextoFinal = fl.ReadLine();
                textoFormatado = FormatarTexto(pTextoFinal, pMr).Trim();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
            finally
            {
                fl.Close();
                fl.Dispose();
            }

            return textoFormatado;
        }

        private string FormatarTexto(string pTexto, MensageriaRegistro pMr)
        {
            return pTexto
                          .Replace("@DATASISTEMA#", DateTime.Now.ToString("dd/MM/yyyy"))
                          .Replace("@DATASISTEMAEXTENSO#", DateTime.Now.ToLongDateString().ToString())
                          .Replace("@DATAHORASISTEMA#", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                          .Replace("@ALUNO#", pMr.MatriculaTurma == null ? pMr.UsuarioTrilha.Usuario.Nome : pMr.MatriculaTurma.MatriculaOferta.Usuario.Nome)
                          .Replace("@TURMA#", pMr.MatriculaTurma == null ? string.Empty : pMr.MatriculaTurma.Turma.Nome)
                          .Replace("@TRILHANIVEL#", pMr.UsuarioTrilha != null ? pMr.UsuarioTrilha.TrilhaNivel.Nome : string.Empty)
                          .Replace("@TRILHA#", pMr.UsuarioTrilha != null ? pMr.UsuarioTrilha.TrilhaNivel.Trilha.Nome : string.Empty)
                          .Replace("@PROFESSOR#", pMr.MatriculaTurma == null ? string.Empty : pMr.MatriculaTurma.Turma == null ? string.Empty : pMr.MatriculaTurma.Turma.Professor.Nome)
                          .Replace("@DATALIMITE#", pMr.UsuarioTrilha == null ? pMr.MatriculaTurma.DataLimite.ToString("dd/MM/yyyy") : pMr.UsuarioTrilha.DataLimite.ToString("dd/MM/yyyy"))
                          .Replace("@DIASAVISO#", pMr.MensageriaParametro.DiaAviso.ToString())
                          .Replace("@EMAILALUNO#", pMr.MatriculaTurma == null ? pMr.UsuarioTrilha.Usuario.Email : pMr.MatriculaTurma.MatriculaOferta.Usuario.Email);
        }

        
        public void RegistrarAbandono()
        {

            try
            {
                //Executando para Matrícula Turma
                IList<MatriculaTurma> lstMT = new BMMatriculaTurma().ObterVencidos();

                foreach (MatriculaTurma mt in lstMT)
                {
                    BMUsuarioAbandono usuarioAbandonoBM = new BMUsuarioAbandono();

                    //if (usuarioAbandonoBM.ValidarAbandonoAtivo(mt.MatriculaOferta.Usuario.ID))
                    //    continue;
                    //else if (mt.Turma.Oferta.ListaMatriculaOferta.Where(x => x.StatusMatricula != enumStatusMatricula.Inscrito).Count() > 0 ||
                    //        mt.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
                    //    continue;

                    if (!mt.MediaFinal.HasValue || mt.MediaFinal.Value == 0)
                    {

                        MatriculaOferta mof = mt.MatriculaOferta;

                        usuarioAbandonoBM.Salvar(new UsuarioAbandono()
                                                                    {
                                                                        DataFimAbandono = DateTime.Now.Date.AddDays(int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta).Registro)),
                                                                        DataInicioAbandono = DateTime.Now.Date,
                                                                        Desconsiderado = false,
                                                                        Usuario = new BMUsuario().ObterPorId(mt.MatriculaOferta.Usuario.ID),
                                                                        Origem = "Abandono SE: " + mof.Oferta.SolucaoEducacional.Nome
                                                                    });

                        if (mof != null)
                        {
                            mof.StatusMatricula = enumStatusMatricula.Abandono;
                            new BMMatriculaOferta().Salvar(mof);
                        }

                    }
                    else
                    {

                    }

                }


                //Executando para Usuário trilha
                IList<UsuarioTrilha> lstUserTrilha = new BMUsuarioTrilha().ObterVencidos();

                foreach (UsuarioTrilha ut in lstUserTrilha)
                {
                    BMUsuarioAbandono usuarioAbandonoBM = new BMUsuarioAbandono();

                    if (usuarioAbandonoBM.ValidarAbandonoAtivo(ut.Usuario.ID))
                        continue;


                    if (usuarioAbandonoBM.ObterPorUsuario(new BMUsuario().ObterPorId(ut.Usuario.ID)) == null)
                    {

                        //usuarioAbandonoBM.Salvar(new UsuarioAbandono()
                        //{
                        //    DataFimAbandono = DateTime.Now.Date.AddDays(ParametrosSistema.DiasFinalAbandono),
                        //    DataInicioAbandono = DateTime.Now.Date,
                        //    Desconsiderado = false,
                        //    Usuario = new BMUsuario().ObterPorId(ut.Usuario.ID)
                        //});


                        ut.StatusMatricula = enumStatusMatricula.Abandono;
                        new BMUsuarioTrilha().Salvar(ut);


                    }

                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }
        

        public IQueryable<SolucaoEducacional> ListaSolucoesEducacionais()
        {
            IQueryable<SolucaoEducacional> listaSolucoesEducacionais = null;

            try
            {
                listaSolucoesEducacionais = new BMSolucaoEducacional().ObterTodos();
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaSolucoesEducacionais;
        }
    }
}
