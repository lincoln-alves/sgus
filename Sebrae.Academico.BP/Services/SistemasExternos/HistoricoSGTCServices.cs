using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP.Services
{
    public class HistoricoSGTCServices : BusinessProcessServicesBase
    {

        private BMHistoricoSGTC historicoSGTC;

        public HistoricoSGTCServices()
            : base()
        {

        }


        public void CadastrarHistoricoSGTC(int idUsuario, string nomeSolucaoEducacional, int idChaveExterna, 
                                           DateTime dtConclusao, string codCertificado, string LoginUsuarioAtualizacao)
        {

            try
            {

                historicoSGTC = new BMHistoricoSGTC();

                HistoricoSGTC atividade = new HistoricoSGTC()
                {
                    ID = 0,
                    NomeSolucaoEducacional = nomeSolucaoEducacional,
                    IDChaveExterna = idChaveExterna,
                    DataConclusao = dtConclusao,
                    CDCertificado = codCertificado,
                    Usuario = usuarioBM.ObterPorCPF(LoginUsuarioAtualizacao),
                };
                                
                historicoSGTC.Salvar(atividade);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}