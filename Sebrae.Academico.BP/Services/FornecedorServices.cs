using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services
{
    public class FornecedorServices : BusinessProcessServicesBase
    {
        public FornecedorServices()
            : base()
        {
        }

        public string ConsultaURLAcesso(int idFornecedor, Usuario usuario)
        {
            try
            {
                Fornecedor fornecedor = new BMFornecedor().ObterPorID(idFornecedor);

                if (fornecedor == null)
                    throw new AcademicoException("Não foi possível encontrar o Fornecedor");
                if (usuario == null)
                    throw new AcademicoException("Não foi possível encontrar o Usuário");

                string linkAcesso = fornecedor.LinkAcesso;
                string textoCriptografia = fornecedor.TextoCriptografia;
                string token;
                if (string.IsNullOrEmpty(textoCriptografia))
                    token = CriptografiaHelper.Criptografar(usuario.CPF);
                else
                    token = CriptografiaHelper.Criptografar(usuario.CPF, textoCriptografia);
                string senhaAberta = CriptografiaHelper.Decriptografar(usuario.Senha);
                string senhaMD5 = CriptografiaHelper.ObterHashMD5(senhaAberta);

                return linkAcesso.Replace("#CPF", usuario.CPF)
                                  .Replace("#TOKEN", token)
                                  .Replace("#SENHAMD5", senhaMD5)
                                    ;
            }
            catch
            {
                return "";
            }

        }
    }
}
