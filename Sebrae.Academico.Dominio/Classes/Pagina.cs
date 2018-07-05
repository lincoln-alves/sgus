using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Pagina : EntidadeBasicaPorId
    {
        public Pagina()
        {
            Perfis = new List<Perfil>();
            Ufs = new List<Uf>();
            NiveisOcupacionais = new List<NivelOcupacional>();
        }

        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string DescricaoAdministrador { get; set; }
        public virtual string DescricaoGestor { get; set; }
        public virtual string CaminhoRelativo { get; set; }

        public virtual string IconeMenu { get; set; }
        public virtual string IconePaginas { get; set; }
        public virtual string Estilo { get; set; }

        public virtual bool? PaginaInicial { get; set; }

        public virtual bool TodosPerfis { get; set; }
        public virtual bool TodasUfs { get; set; }
        public virtual bool TodosNiveisOcupacionais { get; set; }
        
        public virtual IEnumerable<Perfil> Perfis { get; protected internal set; }
        public virtual IEnumerable<Uf> Ufs { get; protected internal set; }
        public virtual IEnumerable<NivelOcupacional> NiveisOcupacionais { get; protected internal set; }

        public virtual int Left { get; set; }
        public virtual int Right { get; set; }

        public virtual string Titulo { get; set; }
        public virtual string ChaveVerificadora { get; set; }

        public virtual bool? ConsiderarNacionalizacaoUf { get; set; }

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

        public virtual void RemoverTodosPerfis()
        {
            Perfis = new List<Perfil>();
        }

        public virtual void RemoverTodasUfs()
        {
            Ufs = new List<Uf>();
        }

        public virtual void RemoverTodosNiveisOcupacionais()
        {
            NiveisOcupacionais = new List<NivelOcupacional>();
        }

        public virtual void AdicionarUf(Uf uf)
        {
            if (!Ufs.Any(x => x.ID == uf.ID))
            {
                var lista = new List<Uf>();
                lista.AddRange(Ufs);
                lista.Add(uf);

                Ufs = lista;
            }
        }

        public virtual void AdicionarNivelOcupacional(NivelOcupacional nivelOcupacional)
        {
            if (!NiveisOcupacionais.Any(x => x.ID == nivelOcupacional.ID))
            {
                var lista = new List<NivelOcupacional>();
                lista.AddRange(NiveisOcupacionais);
                lista.Add(nivelOcupacional);

                NiveisOcupacionais = lista;
            }
        }

        public virtual int Profundidade { get; set; }

        public virtual bool PossuiFilho()
        {
            return Right - Left > 1;
        }

        public virtual bool IsPaginaInicial()
        {
            return PaginaInicial.HasValue && PaginaInicial.Value;
        }

        public virtual bool PossuiPermissao(Usuario usuario)
        {
            if (IsPaginaInicial())
                return true;

            if (ConsiderarNacionalizacaoUf == true && !TodosPerfis && !usuario.UF.IsNacionalizado())
                return false;

            if (!TodosPerfis)
                if (!Perfis.Any(p => usuario.ListaPerfil.Select(x => x.Perfil.ID).Contains(p.ID)))
                    return false;
            
            if (!TodasUfs)
                if(!Ufs.Select(x => x.ID).Contains(usuario.UF.ID))
                    return false;

            if(!TodosNiveisOcupacionais)
                if (!NiveisOcupacionais.Select(x => x.ID).Contains(usuario.NivelOcupacional.ID))
                    return false;

            return true;
        }

        public virtual bool IsFilhoDe(Pagina pagina)
        {
            return Left > pagina.Left && Right < pagina.Right;
        }

        public virtual bool IsPaiOuIguala(Pagina pagina)
        {
            return Left <= pagina.Left && Right >= pagina.Right;
        }

        public virtual enumTipoPagina ObterTipoPagina()
        {
            return (enumTipoPagina)Profundidade;
        }

        /// <summary>
        /// Replica as permissões da página a partir de uma página informada.
        /// </summary>
        /// <param name="pagina">Página com as permissões originais.</param>
        /// <param name="zerarPermissoes">Zerar permissões.</param>
        public virtual void ReplicarPermissao(Pagina pagina, bool zerarPermissoes = false)
        {
            // Verifica se é necessário remover todas as permissões antes de replicar.
            if (zerarPermissoes)
            {
                RemoverTodosPerfis();
                RemoverTodasUfs();
                RemoverTodosNiveisOcupacionais();
            }
            else
            {
                if (pagina.TodosPerfis)
                    RemoverTodosPerfis();

                if (pagina.TodasUfs)
                    RemoverTodasUfs();

                if (pagina.TodosNiveisOcupacionais)
                    RemoverTodosNiveisOcupacionais();
            }

            // Replicar permissões de perfil.
            TodosPerfis = pagina.TodosPerfis;

            foreach (var permissaoPerfil in pagina.Perfis)
            {
                AdicionarPerfil(permissaoPerfil);
            }

            ConsiderarNacionalizacaoUf = pagina.ConsiderarNacionalizacaoUf;

            // Replicar permissões de UF.
            TodasUfs = pagina.TodasUfs;

            foreach (var permissaoUf in pagina.Ufs)
            {
                AdicionarUf(permissaoUf);
            }

            // Replicar permissões de Nível Ocupacional.
            TodosNiveisOcupacionais = pagina.TodosNiveisOcupacionais;
            
            foreach (var permissaoNivel in pagina.NiveisOcupacionais)
            {
                AdicionarNivelOcupacional(permissaoNivel);
            }
        }

        #region Propriedades para mapeamento no GridView
        
        public virtual bool _PossuiFilho
        {
            set { }

            get { return PossuiFilho(); }
        }

        public virtual string _ObterTipoPagina
        {
            set { }

            get
            {
                switch (ObterTipoPagina())
                {
                    case enumTipoPagina.Container:
                        return "Container";
                    case enumTipoPagina.Menu:
                        return "Menu";
                    case enumTipoPagina.Agrupador:
                        return "Agrupador";
                    case enumTipoPagina.Pagina:
                        return "Página";
                    case enumTipoPagina.CadastroEdicao:
                        return "Cadastro/edição";
                    case enumTipoPagina.Subcadastro:
                        return "Subcadastro";
                    default:
                        return null;
                }
            }
        }

        public virtual int _ObterQntFiltrosPerfil
        {
            set { }

            get { return Perfis.Count(); }
        }

        public virtual string _ObterPerfis
        {
            set { }

            get
            {
                var retorno = "";

                var _perfis = Perfis.OrderBy(x => x.Nome);

                foreach (var perfil in _perfis)
                {
                    retorno += perfil.Nome;

                    if (perfil.ID == _perfis.LastOrDefault().ID)
                        retorno += ".";
                    else
                        retorno += ", ";
                }

                return retorno;
            }
        }

        public virtual int _ObterQntFiltrosUf
        {
            set { }

            get { return Ufs.Count(); }
        }

        public virtual string _ObterUfs
        {
            set { }

            get
            {
                var retorno = "";

                var _ufs = Ufs.OrderBy(x => x.Nome);

                foreach (var uf in _ufs)
                {
                    retorno += uf.Nome;

                    if (uf.ID == _ufs.LastOrDefault().ID)
                        retorno += ".";
                    else
                        retorno += ", ";
                }

                return retorno;
            }
        }

        public virtual int _ObterQntFiltrosNivelOcupacional
        {
            set { }

            get { return NiveisOcupacionais.Count(); }
        }

        public virtual string _ObterNiveisOcupacionais
        {
            set { }

            get
            {
                var retorno = "";

                var _niveis = NiveisOcupacionais.OrderBy(x => x.Nome);

                foreach (var nivel in _niveis)
                {
                    retorno += nivel.Nome;

                    if (nivel.ID == _niveis.LastOrDefault().ID)
                        retorno += ".";
                    else
                        retorno += ", ";
                }

                return retorno;
            }
        }
        
        #endregion
    }
}
