using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes{
    public class LogSincronia : EntidadeBasicaPorId{
        public virtual Usuario Usuario { get; set; }
        public virtual string Url { get; set; }
        public virtual string Action { get; set; }
        public virtual string Method { get; set; }
        public virtual string Hash { get; set; }
        public virtual enumAcao Acao { get; set; }

        public virtual DateTime DataCriacao { get; set; }
        public virtual bool Sincronizado { get; set; }

        public virtual IList<LogSincroniaPostParameters> ListaPostParameters { get; set; }

        public LogSincronia(){
            DataCriacao = DateTime.Now;
            ListaPostParameters = new List<LogSincroniaPostParameters>();
            Acao = enumAcao.Atualizar;
        }

        public virtual string NomeAcao{
            get { return Acao == enumAcao.Atualizar ? "Atualizar" : "Remover"; }
        }

        public virtual string HashObj(){
            var result = "URL=" + Url + ";";
            result += "ACTION=" + Action + ";";
            result += "METHOD=" + Method + ";";
            result += "SYNC=" + (Sincronizado ? "SIM" : "NÃO") + ";";
            //result += "USUARIO=" + (Usuario != null ? Usuario.CPF : "") + ";";
            using (var md5Hash = MD5.Create()){
                if (ListaPostParameters == null) return GetMd5Hash(md5Hash, result);
                if (ListaPostParameters.Count <= 0) return GetMd5Hash(md5Hash, result);
                result = ListaPostParameters.Aggregate(result, (current, item) => current + (item.Registro.ToUpper() + "=" + item.Descricao + ";"));
                return GetMd5Hash(md5Hash, result);
            }
        }

        public virtual Dictionary<string, string> GetDictionaryPostParameters(){
            var result = new Dictionary<string, string>();
            if (ListaPostParameters == null) return result;
            foreach (var item in ListaPostParameters){
                result.Add(item.Registro,item.Descricao);
            }
            return result;
        }
        bool VerifyMd5Hash(MD5 md5Hash, string input, string hash){
            var hashOfInput = GetMd5Hash(md5Hash, input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
        //existe um metodo em Sebrae.Academico.InfraEstrutura.Core.CriptografiaHelper 
        //mas pode causar possivel loop references
        string GetMd5Hash(MD5 md5Hash, string input){
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var t in data){
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        #region ICloneable Members
        public virtual object Clone(){
            return this.MemberwiseClone();
        }
        #endregion
    }
}
