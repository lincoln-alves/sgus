using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioDadosProfessor : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.ListagemDadosPessoaisProfessoresTutores; }
        }

        public IList<DTORelatorioDadosProfessor> ConsultarDadosProfessor(string pNome, string pCpf, int? idUf)
        {
            RegistrarLogExecucao();

            return (new ManterProfessor()).ConsultarDadosProfessor(pNome, pCpf, idUf);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
