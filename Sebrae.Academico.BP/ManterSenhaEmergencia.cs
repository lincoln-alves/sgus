using System;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterSenhaEmergencia : BusinessProcessBase
    {
        private BMSenhaEmergencia senhaEmergenciaBM;

        public SenhaEmergencia ObterNovaSenhaEmergencia()
        {

            senhaEmergenciaBM = new BMSenhaEmergencia();

            senhaEmergenciaBM.ExpirarSenhaAtual();

            char[] arSenhaNova = DateTime.Now.Ticks.ToString().ToCharArray();

            string SenhaNova = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                Random r = new Random();
                int indx = r.Next(i, arSenhaNova.Count());
                int rasao = (i + 1) % 8 == 0 ? 31 :
                            (i + 1) % 7 == 0 ? 33 :
                            (i + 1) % 6 == 0 ? 35 :
                            (i + 1) % 5 == 0 ? 20 :
                            (i + 1) % 4 == 0 ? 30 :
                            (i + 1) % 3 == 0 ? 34 :
                            (i + 1) % 2 == 0 ? 30 :
                            28;
                char chr = (char)(rasao + arSenhaNova[indx]);
                SenhaNova += chr;

            }


            SenhaEmergencia se = new SenhaEmergencia()
            {
                Senha = SenhaNova,
                DataValidade = DateTime.Now.AddMinutes(30),

            };
            this.PreencherInformacoesDeAuditoria(se);

            var uf = new BMUsuario().ObterUsuarioLogado().UF;

            senhaEmergenciaBM.Salvar(new SenhaEmergencia()
            {
                DataValidade = se.DataValidade,
                Senha = se.Senha,
                Auditoria = se.Auditoria,
                UF = uf
            });

            return se;
        }

       
    }
}
