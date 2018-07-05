using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Usuario : EntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
        public virtual Uf UF { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual string TipoDocumento { get; set; }
        public virtual string MiniCurriculo { get; set; }
        public virtual string NumeroIdentidade { get; set; }
        public virtual string OrgaoEmissor { get; set; }
        public virtual DateTime? DataExpedicaoIdentidade { get; set; }
        public virtual DateTime? DataNascimento { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string Nacionalidade { get; set; }
        public virtual string EstadoCivil { get; set; }
        public virtual string NomeMae { get; set; }
        public virtual string NomePai { get; set; }
        public virtual string Email { get; set; }
        public virtual string SID_Usuario { get; set; }
        public virtual DateTime? DataInsercao { get; set; }
        public virtual bool SuperAdministrador { get; set; }

      

        public virtual string TrilhaToken { get; set; }
        public virtual DateTime? TrilhaTokenExpiry { get; set; }

        /// <summary>
        /// É o mesmo que Chapa
        /// </summary>
        public virtual string Matricula { get; set; }

        public virtual string CPF { get; set; }
        public virtual DateTime? DataAdmissao { get; set; }
        public virtual DateTime? DataAtualizacaoCarga { get; set; }
        public virtual string Situacao { get; set; }
        public virtual string Endereco { get; set; }
        //public virtual string Endereco2 { get; set; }
        public virtual string Senha { get; set; }
        public virtual string LoginLms { get; set; }
        public virtual string TelResidencial { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string TelCelular { get; set; }
        public virtual string Cidade { get; set; }
        //public virtual string MaterialDidatico { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Pais { get; set; }
        //public virtual string Pais2 { get; set; }
        public virtual string Unidade { get; set; }
        public virtual string Instituicao { get; set; }
        //public virtual string Cep2 { get; set; }
        //public virtual string Complemento2 { get; set; }
        //public virtual string Bairro2 { get; set; }
        //public virtual string Cidade2 { get; set; }
        public virtual string Escolaridade { get; set; }
        //public virtual string Estado2 { get; set; }
        public virtual int? CodigoCampoConhecimento { get; set; }
        public virtual string AnoConclusao { get; set; }
        public virtual string TipoInstituicao { get; set; }
        public virtual string Naturalidade { get; set; }
        public virtual string Imagem { get; set; }
        public virtual string NomeExibicao { get; set; }
        public virtual string TelefoneExibicao { get; set; }
        public virtual string TipoTelefoneExibicao { get; set; }
        public virtual string RamalExibicao { get; set; }
        public virtual FileServer FileServer { get; set; }
        public virtual DateTime? DataAtualizacaoCadastralUsuario { get; set; }

        #region "Listas"

        public virtual IList<UsuarioPerfil> ListaPerfil { get; set; }
        public virtual IList<UsuarioTag> ListaTag { get; set; }
        public virtual IList<MatriculaOferta> ListaMatriculaOferta { get; set; }
        public virtual IList<UsuarioTrilha> ListaMatriculasNaTrilha { get; set; }
        public virtual IList<HistoricoExtraCurricular> ListaHistoricoExtraCurricular { get; set; }
        public virtual IList<MatriculaPrograma> ListaMatriculaPrograma { get; set; }
        public virtual IList<MetaIndividual> ListaMetaIndividual { get; set; }
        public virtual IList<UsuarioPagamento> ListaHistoricoPagamento { get; set; }
        public virtual IList<EtapaPermissao> ListaEtapaPermissao { get; set; }

        //public virtual IList<DTOSolucaoEducacionalPermissao> ListaDTOPermissaoSolucaoEducacional { get; set; }
        //public virtual IList<DTOfertaPermissao> ListaDTOPermissaoOfertas { get; set; }
        public virtual IList<DTOProgramaPermissao> ListaPermissaoProgramas { get; set; }
        public virtual IList<DTOSistemaExternoPermissao> ListaPermissaoSistemasExternos { get; set; }

        public virtual IList<DTOConfiguracaoPagamentoPublicoAlvo> ListaConfiguracaoPagamentoPublicoAlvo { get; set; }

        //public virtual IList<ViewConfiguracaoPagamentoPublicoAlvo> ListaConfiguracaoPagamentoPublicoAlvo { get; set; }

        //public virtual IEnumerable<UsuarioCategoriaConteudo> ListaUsuarioCategoriaConteudo { get; protected internal set; }

        public virtual IEnumerable<CategoriaConteudo> ListaCategoriaConteudo { get; set; }

        public virtual IEnumerable<UsuarioCargo> ListaUsuarioCargo { get; set; }

        public virtual IList<UsuarioCertificadoCertame> ListaUsuarioCertificadoCertame { get; set; }


        #endregion

        public virtual bool? NotificaConteudo { get; set; }
        public virtual bool? NotificaOferta { get; set; }

        #region "Atributos que não serão mapeados"

        // TODO: Remover campo não mais utilizado no banco
        //public virtual string SenhaMD5 { get; set; } //pode ser alterado na tela de cadastro.

        public virtual string ConfirmarSenhaLms { get; set; }

        public virtual bool Ativo
        {
            get
            {
                bool ativo = false;

                if (string.IsNullOrEmpty(this.Situacao))
                {
                    ativo = false;
                }
                else
                {
                    var situacao = Situacao.Trim().ToLower();

                    if (situacao == "ativo" || situacao == "férias")
                    {
                        ativo = true;
                    }

                }
                return ativo;
            }
        }

        public virtual IList<ProcessoResposta> ListaProcessoResposta { get; set; }

        /// <summary>
        /// Utilizado com a tabela do PDi_Registro
        /// </summary>
        public virtual DateTime? DataDemissao { get; set; }

        #endregion

        //public virtual IList<UsuarioPagamento> ListaUsuarioPagamento { get; set; }

        public Usuario()
        {
            ListaPerfil = new List<UsuarioPerfil>();
            ListaTag = new List<UsuarioTag>();
            ListaMatriculaOferta = new List<MatriculaOferta>();
            ListaMatriculasNaTrilha = new List<UsuarioTrilha>();
            ListaHistoricoExtraCurricular = new List<HistoricoExtraCurricular>();
            ListaMatriculaPrograma = new List<MatriculaPrograma>();
            ListaMetaIndividual = new List<MetaIndividual>();
            ListaHistoricoPagamento = new List<UsuarioPagamento>();
            ListaPermissaoProgramas = new List<DTOProgramaPermissao>();
            ListaPermissaoSistemasExternos = new List<DTOSistemaExternoPermissao>();
        }

        #region "Tag"

        public virtual void AdicionarTag(Tag tag)
        {
            IList<Tag> ListaTags = ListaTag.Where(x => x.Tag != null).Select(x => new Tag() { ID = x.Tag.ID, Nome = x.Tag.Nome }).ToList<Tag>();
            UsuarioTag usuarioTag = new UsuarioTag() { Tag = tag, Usuario = this };
            //Antes de adicionar, verifica se já existe na lista
            if (!ListaTags.Where(x => x.ID == tag.ID).Any())
            {
                this.ListaTag.Add(usuarioTag);
            }
        }

        public virtual void RemoverTag(Tag tag)
        {
            IList<Tag> ListaTags = ListaTag.Where(x => x.Tag != null).Select(x => new Tag() { ID = x.Tag.ID, Nome = x.Tag.Nome }).ToList<Tag>();

            if (ListaTags.Where(x => x.ID == tag.ID).Any())
            {
                var programaTagASerExcluido = ListaTag.FirstOrDefault(x => x.Tag != null && x.Tag.ID == tag.ID);
                this.ListaTag.Remove(programaTagASerExcluido);
            }
        }

        #endregion

        public virtual void AdicionarCategoriaConteudo(CategoriaConteudo categoriaConteudo)
        {
            var lista = (IList<CategoriaConteudo>)ListaCategoriaConteudo;

            if (!this.ListaCategoriaConteudo.Any(x => x.ID == categoriaConteudo.ID))
                lista.Add(categoriaConteudo);
        }

        public virtual void RemoverListaCategoriaConteudo(int idCategoria)
        {
            var lista = (IList<CategoriaConteudo>)ListaCategoriaConteudo;
            lista.Remove(lista.FirstOrDefault(x => x.ID == idCategoria));
        }

        public virtual int ObterUfSeGestor()
        {
            bool isGestorOuAdmin = ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.GestorUC || p.Perfil.ID == (int)enumPerfil.Administrador);

            return isGestorOuAdmin ? UF.ID : 0;
        }

        public virtual bool IsConsultorEducacional()
        {
            return
                ListaPerfil.All(
                    p => p.Perfil.ID != (int)enumPerfil.GestorUC && p.Perfil.ID != (int)enumPerfil.Administrador) &&
                ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.ConsultorEducacional);
        }

        public virtual bool IsGestor()
        {
            return ListaPerfil.All(p => p.Perfil.ID != (int)enumPerfil.Administrador) &&
                   ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.GestorUC);
        }

        public virtual bool IsGestorContrato()
        {
            var isGestorContrato = ListaPerfil
                .Select(usuarioPerefil => usuarioPerefil.Perfil.ID)
                .ToList()
                .Exists(id => (int)enumPerfil.GestorContrato == id);

            return isGestorContrato;
        }

        public virtual bool IsNacional()
        {
            //Comentário para retestar a atualização do jenkins (comentário temporário...)
            return (UF.ID == 1 ? true : false);
        }

        public virtual bool IsAdministrador()
        {
            return ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.Administrador);
        }


        public virtual bool IsMonitorTrilha()
        {
            return ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.MonitorTrilha);
        }

        public virtual bool IsAdministradorTrilha()
        {
            return ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.AdministradorTrilha);
        }

        public virtual bool IsGestorDeProtocolo()
        {
            return ListaPerfil.Any(p => p.Perfil.ID == (int)enumPerfil.GestorDeProtocolo);
        }

        public virtual string GetFirstName()
        {
            return Nome.Split(' ')[0];
        }

        public virtual List<int> ObterIdsPerfis()
        {
            return ListaPerfil.Select(x => x.Perfil.ID).ToList();
        }

        public virtual string GetLastName()
        {
            var splitName = Nome.Split(' ');

            if (splitName.Length == 1)
                return Nome;

            var retorno = "";

            for (var i = 1; i < splitName.Length; i++)
                retorno = retorno + " " + splitName[i];

            return retorno.Trim();
        }

        public virtual string ObterLinkImagem(string enderecoSgus)
        {
            return FileServer != null ? enderecoSgus + "MediaServer.ashx?Identificador=" + FileServer.ID : null;
        }

        public virtual string ObterSexo()
        {
            return ObterSexo(Sexo);
        }

        public static string ObterSexo(string sexo)
        {
            if (sexo == null)
                return null;

            switch (sexo.ToLower())
            {
                case "m":
                case "masculino":
                    return "M";

                case "f":
                case "feminino":
                    return "F";

                default:
                    return null;
            }
        }

        public static string ObterPrimeirosNomes(string nome)
        {
            while (nome.Contains("  "))
            {
                nome = nome.Replace("  ", " ");
            }

            var nomes = nome.Split(' ');

            if (nomes.Length > 1)
                return nomes[0] + " " +
                       // Buscar mais sobrenomes caso a segunda palavra nome da pessoa seja "de" ou "da"
                       (new List<string> { "de", "da" }.Contains(nomes[1].ToLower()) && nomes.Length > 2
                           ? nomes[1] + " " + nomes[2]
                           : nomes[1]);

            return nomes[0];
        }

        public virtual string ObterPrimeirosNomes()
        {
            return ObterPrimeirosNomes(Nome);
        }

        /// <summary>
        /// Obter o Cargo do usuário de acordo com sua UF atual.
        /// </summary>
        /// <returns></returns>
        public virtual UsuarioCargo ObterCargo()
        {
            return ListaUsuarioCargo.FirstOrDefault(x => x.Cargo.Uf.ID == UF.ID);
        }

        public virtual bool AtivoNoPDI()
        {
            // Usuários podem logar no sistema até dois anos da demissão
            return DataDemissao.HasValue && DateTime.Now <= DataDemissao.Value.AddYears(2);
        }

    }
  
}