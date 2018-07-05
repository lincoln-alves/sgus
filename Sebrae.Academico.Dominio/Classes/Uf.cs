using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Uf
    {
        public Uf()
        {
            ListaProgramaPermissao = new List<ProgramaPermissao>();
            ListaOfertaPermissao = new List<OfertaPermissao>();
            ListaTrilhaPermissao = new List<TrilhaPermissao>();
            ListaNacionalizacaoUf = new List<NacionalizacaoUf>();
        }

        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual IList<ProgramaPermissao> ListaProgramaPermissao { get; set; }
        public virtual IList<OfertaPermissao> ListaOfertaPermissao { get; set; }
        public virtual IList<TrilhaPermissao> ListaTrilhaPermissao { get; set; }
        public virtual IList<CategoriaConteudoUF> ListaCategoriaConteudoUF { get; set; }
        public virtual IList<NacionalizacaoUf> ListaNacionalizacaoUf { get; set; }
        public virtual IEnumerable<PublicoAlvo> PublicosAlvos { get; set; }
        public virtual IEnumerable<TermoAceite> TermosAceite { get; set; }
        public virtual IEnumerable<RelatorioPaginaInicial> ListaRelatoriosPaginaInicial { get; set; }
        public virtual IList<EtapaPermissao> ListaEtapaPermissao { get; set; }
        public virtual Regiao Regiao { get; set; }

        public virtual bool IsNacionalizado()
        {
            return ListaNacionalizacaoUf != null ? ListaNacionalizacaoUf.Any() : false;
        }

        /// <summary>
        /// Encapsula a primeira regra de negócio explicitada na demanda #1649. Precisa de refatoração,
        /// dando a opção do usuário selecionar quais UFs podem cadastrar SEs com o mesmo nome.
        /// </summary>
        /// <returns></returns>
        public virtual bool PermiteSesMesmoNome()
        {
            return ID == (int) enumUF.SP || ID == (int) enumUF.NA || ID == (int) enumUF.MG;
        }
    }
}
