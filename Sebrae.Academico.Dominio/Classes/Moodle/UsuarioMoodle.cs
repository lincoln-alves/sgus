using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class UsuarioMoodle
    {
        /// <summary>Campo "id" da tabela "mdl_user" do Moodle</summary>            
        public virtual Int64 ID { get; set; }

        /// <summary>Campo "auth" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Autorizacao { get; set; }

        /// <summary>Campo "confirmed" da tabela "mdl_user" do Moodle</summary>  
        public virtual bool Confirmado { get; set; }

        /// <summary>Campo "policyagreed" da tabela "mdl_user" do Moodle</summary>  
        public virtual bool PoticaAceita { get; set; }

        /// <summary>Campo "deleted" da tabela "mdl_user" do Moodle</summary>  
        public virtual bool Excluido { get; set; }

        /// <summary>Campo "suspended" da tabela "mdl_user" do Moodle</summary>  
        public virtual bool Suspenso { get; set; }

        /// <summary>Campo "mnethostid" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 IdHostNet { get; set; }

        /// <summary>Campo "username" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Usuario { get; set; }

        /// <summary>Campo "password" da tabela "mdl_user" do Moodle. Obs.: Deve ser salva em MD5</summary>  
        public virtual string Senha { get; set; }

        /// <summary>Campo "idnumber" da tabela "mdl_user" do Moodle</summary>  
        public virtual string IdNumero { get; set; }

        /// <summary>Campo "firstname" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Nome { get; set; }

        /// <summary>Campo "lastname" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Sobrenome { get; set; }

        /// <summary>Campo "email" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Email { get; set; }

        /// <summary>Campo "emailstop" da tabela "mdl_user" do Moodle</summary>  
        public virtual bool EmailParado { get; set; }

        /// <summary>Campo "icq" da tabela "mdl_user" do Moodle</summary>  
        public virtual string ICQ { get; set; }

        /// <summary>Campo "skype" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Skype { get; set; }

        /// <summary>Campo "yahoo" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Yahoo { get; set; }

        /// <summary>Campo "aim" da tabela "mdl_user" do Moodle</summary>  
        public virtual string AIM { get; set; }

        /// <summary>Campo "msn" da tabela "mdl_user" do Moodle</summary>  
        public virtual string MSN { get; set; }

        /// <summary>Campo "phone1" da tabela "mdl_user" do Moodle</summary>  
        public virtual string TelefoneUm { get; set; }

        /// <summary>Campo "phone2" da tabela "mdl_user" do Moodle</summary>  
        public virtual string TelefoneDois { get; set; }

        /// <summary>Campo "institution" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Instituicao { get; set; }

        /// <summary>Campo "department" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Departamento { get; set; }

        /// <summary>Campo "address" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Endereco { get; set; }

        /// <summary>Campo "city" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Cidade { get; set; }

        /// <summary>Campo "country" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Pais { get; set; }

        /// <summary>Campo "lang" da tabela "mdl_user" do Moodle. Ex: pt-br</summary>  
        public virtual string Idioma { get; set; }

        /// <summary>Campo "theme" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Tema { get; set; }

        /// <summary>Campo "timezone" da tabela "mdl_user" do Moodle</summary>  
        public virtual string ZonaHoraria { get; set; }

        /// <summary>Campo "firstaccess" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 PrimeiroAcesso { get; set; }

        /// <summary>Campo "lastaccess" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 UltimoAcesso { get; set; }

        /// <summary>Campo "lastlogin" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 UltimoLogin { get; set; }

        /// <summary>Campo "currentlogin" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 LoginAtual { get; set; }

        /// <summary>Campo "lastip" da tabela "mdl_user" do Moodle</summary>  
        public virtual string UltimoIP { get; set; }

        /// <summary>Campo "secret" da tabela "mdl_user" do Moodle</summary>  
        public virtual string PalavraSecreta { get; set; }

        /// <summary>Campo "picture" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 Foto { get; set; }

        /// <summary>Campo "url" da tabela "mdl_user" do Moodle</summary>  
        public virtual string URL { get; set; }

        /// <summary>Campo "description" da tabela "mdl_user" do Moodle</summary>  
        public virtual string Descricao { get; set; }

        /// <summary>Campo "descriptionformat" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte DescricaoFormato { get; set; }

        /// <summary>Campo "mailformat" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte EmailFormato { get; set; }

        /// <summary>Campo "maildigest" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte EmailResumo { get; set; }

        /// <summary>Campo "maildisplay" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte EmailExibe { get; set; }

        /// <summary>Campo "htmleditor" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte EditorHTML { get; set; }

        /// <summary>Campo "autosubscribe" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte AutoInscricao { get; set; }

        /// <summary>Campo "trackforums" da tabela "mdl_user" do Moodle</summary>  
        public virtual byte LocalizaForuns { get; set; }

        /// <summary>Campo "timecreated" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 TempoCriado { get; set; }

        /// <summary>Campo "timemodified" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 TempoModificado { get; set; }

        /// <summary>Campo "trustbitmask" da tabela "mdl_user" do Moodle</summary>  
        public virtual Int64 ConfiancaBitMask { get; set; }

        /// <summary>Campo "imagealt" da tabela "mdl_user" do Moodle</summary>  
        public virtual string ImagemAlterada { get; set; }

        public virtual IList<UsuarioMoodleInscricao> ListaMoodleInscricoes {get; set; }

        public UsuarioMoodle()
        {
            this.Confirmado = false;
            this.PoticaAceita = false;
            this.Excluido = false;
            this.Suspenso = false;
            this.IdHostNet = 1;
            this.Cidade = "";
            this.ICQ = "";
            this.Skype = "";
            this.Yahoo = "";
            this.AIM = "";
            this.MSN = "";
            this.TelefoneUm = "";
            this.TelefoneDois = "";
            this.Instituicao = "";
            this.Departamento = "";
            this.Endereco = "";
            this.PrimeiroAcesso = 0;
            this.UltimoAcesso = 0;
            this.UltimoLogin = 0;
            this.LoginAtual = 0;
            this.Foto = 0;
            this.DescricaoFormato = 1;
            this.EmailFormato = 1;
            this.EmailResumo = 0;
            this.EmailExibe = 2;
            this.EditorHTML = 1;
            this.AutoInscricao = 1;
            this.LocalizaForuns = 0;
            this.TempoCriado = 0;//.ToBinary();
            this.TempoModificado = 0;//.ToBinary();
            this.ConfiancaBitMask = 0;
            this.Tema = "";
            this.UltimoIP = "";
            this.PalavraSecreta = "";
            this.URL = "";
        }
    }
}
