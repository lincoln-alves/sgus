using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using webaula = Sebrae.Academico.BP.waIntegracao;
using fgvOCW = Sebrae.Academico.BP.fgvIntegracaoOCW;
using moodle = Sebrae.Academico.BP.moodleIntegracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class NotificaFornecedor
    {
        #region Singleton

        public static NotificaFornecedor Instancia
        {
            get
            {

                if (Singleton.Instance == null)
                    return new NotificaFornecedor();
                else
                    return Singleton.Instance;

            }
        }

        private class Singleton
        {
            static Singleton() { }
            internal static readonly NotificaFornecedor Instance = new NotificaFornecedor();
        }


        #endregion

        public void Notificar(MatriculaOferta mo)
        {
            string retornows;
            switch (mo.Oferta.SolucaoEducacional.Fornecedor.ID)
            {
                case (int)enumFornecedor.MoodleSebrae:
                    moodle.IntegracaoSoapClient soapCliente = new moodle.IntegracaoSoapClient();
                    retornows = soapCliente.MatricularAluno(
                            mo.Usuario.Nome,
                            mo.Usuario.CPF,
                            mo.Usuario.Email,
                            mo.Usuario.Cidade,
                            mo.Oferta.CodigoMoodle.ToString(),
                            mo.MatriculaTurma.FirstOrDefault().Turma.IDChaveExterna.ToString());
                    mo.FornecedorNotificado = true;
                    break;
                case (int)enumFornecedor.WebAula:
                    Turma turma = mo.MatriculaTurma.FirstOrDefault().Turma;
                    webaula.waIntegracaoSoapClient wa = new webaula.waIntegracaoSoapClient();
                    webaula.AuthenticationProviderRequest aut = new webaula.AuthenticationProviderRequest();
                    webaula.DTOUsuario dtoUsuario = new webaula.DTOUsuario();
                    webaula.DTOTurma dtoTurma = new webaula.DTOTurma();
                    dtoTurma.IDChaveExterna = turma.IDChaveExterna;
                    dtoUsuario.CPF = mo.Usuario.CPF;
                    dtoUsuario.Email = mo.Usuario.Email;
                    dtoUsuario.Nome = mo.Usuario.Nome;
                    dtoUsuario.Sexo = mo.Usuario.Sexo;
                    dtoUsuario.UF = mo.Usuario.UF.Sigla;
                    aut.Login = mo.Oferta.SolucaoEducacional.Fornecedor.Login;
                    aut.Senha = CriptografiaHelper.Decriptografar(mo.Oferta.SolucaoEducacional.Fornecedor.Senha);
                    
                    
                    try
                    {
                        retornows = wa.Matricular(aut, dtoUsuario, dtoTurma);
                    }
                    catch (Exception e)
                    {
                        throw new AcademicoException(e.Message);
                    }
                    mo.FornecedorNotificado = true;
                    break;

                

            }
        }
    }
}
