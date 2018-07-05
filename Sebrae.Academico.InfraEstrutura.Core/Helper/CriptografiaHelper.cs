using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class CriptografiaHelper
    {

        private static string vetorInicializacao = "12345678123456781234567812345678";
        private static string algoritmoHASH = "SHA1";
        private static int quantidadeIteracoes = 1;
        private static ICryptoTransform decriptografador = null;
        private static int tamanhoChave = 256;
        private static string valorAleatorio = "12345678123456781234567812345678";
        //private static string contraSenha = "universidade corporativa sebrae 2013";
        private static MemoryStream valorEmMemoria = null;

        /// <summary>
        /// Criptografa com a ecriptação AES (Rijndael class)
        /// </summary>
        /// <param name="textoCifrado"></param>
        /// <returns></returns>
        public static string Decriptografar(string textoCifrado, string contraSenha = "UniversidadeCorporativaSEBRAE200")
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(Base64Encode(contraSenha));
            aes.IV = Convert.FromBase64String(Base64Encode(vetorInicializacao));

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(textoCifrado);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }

        /// <summary>
        /// Criptografa com a ecriptação AES (Rijndael class)
        /// </summary>
        /// <param name="textoEmClaro"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public static string Criptografar(string textoEmClaro, string contraSenha = "UniversidadeCorporativaSEBRAE200")
        {
            if (string.IsNullOrEmpty(contraSenha))
                return string.Empty;

            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(Base64Encode(contraSenha));

            aes.IV = Convert.FromBase64String(Base64Encode(vetorInicializacao));

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(textoEmClaro);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Convert.ToBase64String(xBuff);
            return Output;

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private static byte[] GerarArrayDeBytesParaVetorDeInicializacao(string vetorInicializacao)
        {
            byte[] bytesVetorInicializacao = Encoding.ASCII.GetBytes(vetorInicializacao);
            return bytesVetorInicializacao;
        }

        private static byte[] GerarArrayDeBytesParaValorAleatorio(string valorAleatorio)
        {
            byte[] bytesValorAleatorio = Encoding.ASCII.GetBytes(valorAleatorio);
            return bytesValorAleatorio;
        }

        public static string ObterHashMD5(string texto)
        {
            var md5 = MD5.Create();
            byte[] md5bytes = Encoding.UTF8.GetBytes(texto);
            byte[] md5hashBytes = md5.ComputeHash(md5bytes);

            string stringComHash = string.Empty;
            foreach (var item in md5hashBytes)
                stringComHash += item.ToString("X2");

            return stringComHash.ToLower();
        }
    }
}
