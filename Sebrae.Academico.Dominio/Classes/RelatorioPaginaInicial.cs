using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class RelatorioPaginaInicial
    {
        public RelatorioPaginaInicial()
        {
            Ufs = new List<Uf>();
            Perfis = new List<Perfil>();
        }

        public virtual int ID { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Tag { get; set; }

        public virtual string Funcao { get; set; }

        public virtual bool TodosPerfis { get; set; }

        public virtual bool TodasUfs { get; set; }

        public virtual IEnumerable<Uf> Ufs { get; set; }

        public virtual IEnumerable<Perfil> Perfis { get; set; }

        public virtual bool UsuarioPodeVisualizarRelatorio(Usuario usuario)
        {
            return (TodosPerfis || (Perfis != null && Perfis.Any(x => usuario.ListaPerfil.Any(p => p.Perfil.ID == x.ID))))
                   &&
                   (TodasUfs|| (Ufs != null && Ufs.Any(x => usuario.UF.ID == x.ID)));
        }

        public virtual void AdicionarPerfil(Perfil perfil)
        {
            if (Perfis.All(x => x.ID != perfil.ID))
            {
                var lista = new List<Perfil>();
                lista.AddRange(Perfis);
                lista.Add(perfil);

                Perfis = lista;
            }
        }

        public virtual void AdicionarUf(Uf uf)
        {
            if (Ufs.All(x => x.ID != uf.ID))
            {
                var lista = new List<Uf>();
                lista.AddRange(Ufs);
                lista.Add(uf);

                Ufs = lista;
            }
        }

        public virtual void RemoverTodosPerfis()
        {
            Perfis = new List<Perfil>();
        }

        public virtual void RemoverTodasUfs()
        {
            Ufs = new List<Uf>();
        }
    }
}
