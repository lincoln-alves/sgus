using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using System;
using System.Linq;
using System.Threading;

namespace Sebrae.Academico.BP
{
    public class ManterLogSincronia : BusinessProcessBase
    {
        private static ManterLogSincronia _instance;

        public static ManterLogSincronia Instance
        {
            get { return _instance ?? (_instance = new ManterLogSincronia()); }
        }

        private readonly BMLogSincronia _bmLogSincronia;

        public ManterLogSincronia()
        {
            _bmLogSincronia = new BMLogSincronia();
        }

        public void Sincronizar(LogSincronia log)
        {
            if (log == null) return;
            try
            {
                var result = DrupalUtil.NodeDrupalRest(log).HasValue;

                if (!result) return;

                log.Sincronizado = true;

                _bmLogSincronia.Salvar(log);
            }
            catch
            {

            }
        }

        public LogSincronia ObterPorId(int id)
        {
            return _bmLogSincronia.ObterPorID(id);
        }

        public IList<LogSincronia> ObterTodos()
        {
            return _bmLogSincronia.ObterTodos();
        }

        private Thread _thread;

        public void IniciarThread()
        {
            Thread.MemoryBarrier();

            if (_thread != null && _thread.ThreadState != ThreadState.StopRequested &&
                _thread.ThreadState != ThreadState.Aborted && _thread.ThreadState != ThreadState.Suspended) return;
            try
            {
                _thread = new Thread(ThreadCheckLogSincronia)
                {
                    IsBackground = true,
                    Name = "thread_sincronia_portal"
                };
                _thread.Start();
            }
            catch 
            {
                //TODO: REALIZAR LOG DE FALHAS  
            }
            Thread.MemoryBarrier();
        }

        private void ThreadCheckLogSincronia()
        {
            try
            {
                var lista = ObterTodos().Where(p => p.Sincronizado == false);
                foreach (var item in lista)
                {
                    Sincronizar(item);
                }
            }
            catch
            {
                //TODO: REALIZAR LOG DE FALHAS  
            }
        }
    }
}