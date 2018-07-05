using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Sebrae.Academico.Util.Classes
{
    public class DTOCacheData
    {
        public string Data { get; set; }
        public string DataType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class CacheData
    {
        private const string _cacheFolderName = "Cache";
        private string _cachePath;
        private DTOCacheData cacheData = null;

        private int duration;
        private string uniqueName;

        public CacheData(string uniqueName, int duration)
        {
            this.uniqueName = uniqueName;
            this.duration = duration;

            var rootPath = "";

            if (System.Web.HttpContext.Current != null)
            {
                rootPath = System.Web.HttpContext.Current.Server.MapPath("~");
            }
            // For unit tests
            else
            {
                rootPath = AppDomain.CurrentDomain.BaseDirectory;
                if (!Directory.Exists(string.Concat(rootPath, _cacheFolderName)))
                {
                    Directory.CreateDirectory(string.Concat(rootPath, _cacheFolderName));
                }
            }
            _cachePath = string.Concat(rootPath, _cacheFolderName);            
        }

        public dynamic GetCacheData()
        {
            dynamic result = false;
            if (HasCacheData())
            {   
                var dtoCacheData = GetCache();

                if (dtoCacheData.Data != "")
                {
                    var classData = Type.GetType(dtoCacheData.DataType);

                    if (classData != null)
                    {
                        return JsonConvert.DeserializeObject(dtoCacheData.Data, classData);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return result;
            }            
        }

        private DTOCacheData GetCache()
        {
            // Returns the data already stored on memory by the instance
            if (cacheData != null && cacheData.Data != null)
            {
                return cacheData;
            }
            else
            {
                // Reads data from disk if exists
                if (HasCacheData())
                {
                    var dtoCacheData = JsonConvert.DeserializeObject<DTOCacheData>(ReadCacheFileToString());
                    return dtoCacheData;
                }
                else
                {
                    return new DTOCacheData();
                }
            }            
        }


        public string ReadCacheFileToString()
        {
            var result = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream file = new FileStream(string.Concat(_cachePath, "/", GetCacheFileName()), FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                }

                ms.Position = 0;
                var sr = new StreamReader(ms);
                result = sr.ReadToEnd();
            }
            return result;
        }

        private bool HasCacheData()
        {
            return File.Exists( string.Concat(_cachePath, "/", GetCacheFileName()) );
        }

        public bool HasValidCacheData()
        {
            // If the cache exists and has not expired
            if( HasCacheData())
            {
                var dtoCacheData = GetCache();
                return dtoCacheData.ExpiresAt >= DateTime.Now;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Salva os dados passado por data para um arquivo de texto no formato JSON, o 
        /// dataType é importante para deserealizar o arquivo de texto.
        /// 
        /// Use o parâmetro expires pra especificar manualmente um DateTime para expiração do cache
        /// do contrário ele usará a data e hora atual acrescido do inteiro em horas passado (duration)
        /// passado no construtor
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataType"></param>
        /// <param name="expires"></param>
        public void SetCacheData(dynamic data, Type dataType, DateTime? expires = null)
        {
            var dtoCacheData = new DTOCacheData
            {
                CreatedAt = DateTime.Now,
                Data = JsonConvert.SerializeObject(data),
                ExpiresAt = expires ?? DateTime.Now.AddHours(duration),
                DataType = dataType.AssemblyQualifiedName
            };

            var cacheDtoContent = JsonConvert.SerializeObject(dtoCacheData);

            using (var file = new System.IO.StreamWriter(string.Concat(_cachePath, "/", GetCacheFileName() ) ) )
            {
                file.WriteLine(cacheDtoContent);
            }
        }

        /// <summary>
        /// Retorna o nome do arquivo salvo pelo mecanismo de cache
        /// </summary>
        /// <returns></returns>
        public string GetCacheFileName()
        {
            return string.Concat(uniqueName, "_" , duration);
        }

        /// <summary>
        /// Limpa um cache específico instanciado pela classe        
        /// </summary>
        /// <returns></returns>
        public bool PurgeCache()
        {
            // If the cache exists and has not expired
            if (HasCacheData())
            {                               
                try
                {
                    System.IO.File.Delete(string.Concat(_cachePath, "/", GetCacheFileName()));
                }catch(Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
    
        /// <summary>
        /// Limpa todos os caches da pasta /Cache
        /// </summary>
        public void PurgeAllCaches()
        {
            string rootFolderPath = _cachePath;
            var fileList = System.IO.Directory.GetFiles(rootFolderPath)
                .Where(fileName => !fileName.EndsWith(".gitkeep"));
            foreach (string file in fileList)
            {                
                System.IO.File.Delete(file);
            }            
        }
    }
}
