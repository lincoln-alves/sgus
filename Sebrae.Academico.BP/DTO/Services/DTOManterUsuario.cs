using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOManterUsuario
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        //Lista apenas para atender DRUPAL
        public virtual List<DTOUf> ListaUF { get; set; }
        public virtual List<DTONivelOcupacional> ListaNivelOcupacional { get; set; }
        public virtual string TipoDocumento { get; set; }
        public virtual string MiniCurriculo { get; set; }               //EDITAVEL PORTAL//Competencias no Conexao
        public virtual string NumeroIdentidade { get; set; }            //EDITAVEL PORTAL
        public virtual string OrgaoEmissor { get; set; }                //EDITAVEL PORTAL
        public virtual DateTime? DataExpedicaoIdentidade { get; set; }  //EDITAVEL PORTAL
        public virtual DateTime? DataNascimento { get; set; }
        public virtual string Sexo { get; set; }                        //EDITAVEL PORTAL
        public virtual string Nacionalidade { get; set; }               //EDITAVEL PORTAL
        public virtual string EstadoCivil { get; set; }                 //EDITAVEL PORTAL
        public virtual string NomeMae { get; set; }                     //EDITAVEL PORTAL
        public virtual string NomePai { get; set; }                     //EDITAVEL PORTAL
        public virtual string Email { get; set; }                       
        public virtual string Matricula { get; set; }
        public virtual string CPF { get; set; }
        public virtual DateTime? DataAdmissao { get; set; }
        public virtual DateTime? DataAtualizacaoCarga { get; set; }
        public virtual string Situacao { get; set; }
        public virtual string Endereco { get; set; }                    //EDITAVEL PORTAL
        //public virtual string Endereco2 { get; set; }
        public virtual string Senha { get; set; }                       //USADO APENAS NA ALTERAÇAO DA SENHA //EDITAVEL PORTAL
        public virtual string SenhaMD5 { get; set; }
        public virtual string LoginLms { get; set; }
        public virtual string TelResidencial { get; set; }              //EDITAVEL PORTAL
        public virtual string Estado { get; set; }                      //EDITAVEL PORTAL
        public virtual string Cep { get; set; }                         //EDITAVEL PORTAL
        public virtual string Bairro { get; set; }                      //EDITAVEL PORTAL
        public virtual string TelCelular { get; set; }                  //EDITAVEL PORTAL
        public virtual string Cidade { get; set; }                      //EDITAVEL PORTAL
        //public virtual string MaterialDidatico { get; set; }
        public virtual string Complemento { get; set; }                 //EDITAVEL PORTAL
        public virtual string Pais { get; set; }                        //EDITAVEL PORTAL
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
        public virtual string Naturalidade { get; set; }                //EDITAVEL PORTAL
        public virtual string Imagem { get; set; }                      //EDITAVEL PORTAL
        public virtual string NomeExibicao { get; set; }                //EDITAVEL PORTAL
        public virtual string TelefoneExibicao { get; set; }            //EDITAVEL PORTAL
        public virtual string TipoTelefoneExibicao { get; set; }        //EDITAVEL PORTAL
        public virtual string RamalExibicao { get; set; }               //EDITAVEL PORTAL
        public virtual string LinkImagem { get; set; }
        public virtual string ImagemBase64 { get; set; }
        public virtual string GuidUsuario { get; set; }
        public virtual string LinkConexao { get; set; }
        public virtual bool PendenciaCadastral { get; set; }
        public virtual string SMoodle { get; set; }
        public virtual DateTime? DataUltimaAtualizacao { get; set; }
        public virtual bool? IsGestor { get; set; }

        public virtual string MensagemLogin { get; set; }

        public virtual List<DTOPerfil> ListaPerfil { get; set; }
        public virtual List<DTOTag> ListaTag { get; set; }
        public virtual List<DTOMetaIndividual> ListaMetaIndividual { get; set; }
        public virtual List<DTOMetaInstitucional> ListaMetaInstitucional { get; set; }
        public virtual List<DTOSistemaExterno> ListaSistemaExterno { get; set; }

        public DTOManterUsuario()
        {
            ListaPerfil = new List<DTOPerfil>();
            ListaTag = new List<DTOTag>();
            ListaUF = new List<DTOUf>();
            ListaNivelOcupacional = new List<DTONivelOcupacional>();
            ListaMetaIndividual = new List<DTOMetaIndividual>();
            ListaMetaInstitucional = new List<DTOMetaInstitucional>();
            ListaSistemaExterno = new List<DTOSistemaExterno>();
            PendenciaCadastral = false;
        }

        #region "Dados do Pagamento"


        /// <summary>
        /// Indica se o usuário é obrigado a fazer pagamento.
        /// </summary>
        public bool? TemPagamentoAssociado { get; set; }
        public bool? TemPagamentoValido { get; set; }
        public string CodigoPagamentoAssociado { get; set; }
        public decimal? ValorPagamentoAssociado { get; set; }
        
        #region "Dados do Último Pagamento"

        public DateTime? DataInicioVigenciaPagamento { get; set; }
        public DateTime? DataFimVigenciaPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorPagamento { get; set; }
        public DateTime? DataInicioRenovacaoPagamento { get; set; }
        public DateTime? DataMaximaInadimplenciaPagamento { get; set; }
        public bool? PagamentoEfetuado { get; set; }
        public int? FormaPagamento { get; set; }
        public string TermoAdesaoPagamento { get; set; }
        
        #endregion

        #endregion

    }

    public class DTOManterUsuarioConexao : DTOManterUsuario 
    {
        public string SID { get; set; }
    }
}