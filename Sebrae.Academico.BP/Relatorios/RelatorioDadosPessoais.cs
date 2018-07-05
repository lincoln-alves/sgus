using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioDadosPessoais : BusinessProcessBaseRelatorio, IDisposable
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.DadosPessoais; }
        }

        public IList<DTORelatorioDadosPessoais> ConsultarDadosPessoais(string nome, string cpf, List<int> nivelOcupacionalIds, List<int> ufIds, List<int> perfilIds)
        {
            RegistrarLogExecucao();

            return (new ManterUsuario()).ConsultarDadosPessoais(nome, cpf, nivelOcupacionalIds, ufIds, perfilIds);
        }

        public IList<DTORelatorioDadosPessoais> DadosPessoaisPerfil(IList<DTORelatorioDadosPessoais> dadosPessoais)
        {
            RegistrarLogExecucao();

            return dadosPessoais;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
