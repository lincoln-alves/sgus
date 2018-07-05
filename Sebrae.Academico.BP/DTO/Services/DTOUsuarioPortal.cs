using Sebrae.Academico.BP.DTO.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOUsuarioPortal
    {
        // Dados editáveis por meio do portal
        public virtual int ID { get; set; }
        public virtual string NomeExibicao { get; set; }                        //EDITAVEL PORTAL
        public virtual string LinkImagem { get; set; }                  //EDITAVEL PORTAL
        public virtual string Sexo { get; set; }                        //EDITAVEL PORTAL        
        public virtual string Email { get; set; }                       //EDITAVEL PORTAL
        public virtual string TelefoneExibicao { get; set; }            //EDITAVEL PORTAL
        public virtual string TelCelular { get; set; }                  //EDITAVEL PORTAL
        public virtual string GuidUsuario { get; set; }
        public virtual string Senha { get; set; }

        public virtual string MensagemLogin { get; set; }
        // Rever necessidade
        public virtual DateTime? DataUltimaAtualizacao { get; set; }    // Dado de controle de atualização

        // Permissões de acesso a informação
        public virtual List<DTOUf> ListaUF { get; set; }
        public virtual List<DTONivelOcupacional> ListaNivelOcupacional { get; set; }
        public virtual List<DTOPerfil> ListaPerfil { get; set; }

        // Metas individuais usadas na tela de matrícula
        public virtual List<DTOMetaIndividual> ListaMetaIndividual { get; set; }
        public virtual List<DTOMetaInstitucional> ListaMetaInstitucional { get; set; }

        public DTOUsuarioPortal()
        {
            ListaPerfil = new List<DTOPerfil>();
            ListaUF = new List<DTOUf>();
            ListaNivelOcupacional = new List<DTONivelOcupacional>();
            ListaMetaIndividual = new List<DTOMetaIndividual>();
            ListaMetaInstitucional = new List<DTOMetaInstitucional>();
        }
    }
}
