using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Cargo
    {
        public Cargo()
        {
            UsuariosCargos = new List<UsuarioCargo>();
        }

        public virtual int ID { get; set; }

        public virtual string Nome { get; set; }

        public virtual EnumTipoCargo TipoCargo { get; set; }

        public virtual Cargo CargoPai { get; set; }

        public virtual Uf Uf { get; set; }
        public virtual string Sigla { get; set; }

        public virtual IList<Cargo> CargosFilhos { get; set; }

        public virtual bool Ativo { get; set; }
        public virtual int Ordem { get; set; }

        /// <summary>
        /// Obter os usuários alocados nesta unidade. Nunca obter diretamente, mas obter pelos métodos desse objeto que recebem a UF como filtro.
        /// </summary>
        public virtual IList<UsuarioCargo> UsuariosCargos { get; set; }

        public virtual IEnumerable<UsuarioCargo> ObterDiretores(Uf uf)
        {
            return UsuariosCargos.Where(x => x.Cargo.TipoCargo == EnumTipoCargo.Diretoria);
        }

        public virtual IEnumerable<UsuarioCargo> ObterChefesGabinete(Uf uf)
        {
            return UsuariosCargos.Where(x => x.Cargo.TipoCargo == EnumTipoCargo.Gabinete);
        }

        public virtual IEnumerable<UsuarioCargo> ObterGerentes(Uf uf)
        {
            return UsuariosCargos.Where(x => x.Cargo.TipoCargo == EnumTipoCargo.Gerencia).OrderBy(x => x.Usuario.Nome);
        }

        public virtual IEnumerable<UsuarioCargo> ObterGerentesAdjuntos(Uf uf)
        {
            return UsuariosCargos.Where(x => x.Cargo.TipoCargo == EnumTipoCargo.GerenciaAdjunta).OrderBy(x => x.Usuario.Nome);
        }

        public virtual IEnumerable<UsuarioCargo> ObterFuncionarios(Uf uf)
        {
            return UsuariosCargos.Where(x => x.Cargo.TipoCargo == EnumTipoCargo.Funcionario).OrderBy(x => x.Usuario.Nome);
        }

        public virtual string ObterNome()
        {
            switch (TipoCargo)
            {
                case EnumTipoCargo.GerenciaAdjunta:
                case EnumTipoCargo.Gabinete:
                    return CargoPai.Nome;
                case EnumTipoCargo.Funcionario:
                    if (CargoPai.TipoCargo == EnumTipoCargo.Gabinete)
                        return CargoPai.Nome;

                    return Nome;
                default:
                    return Nome;
            }
        }

        public virtual string ObterNomeCompleto()
        {
            switch (TipoCargo)
            {
                case EnumTipoCargo.Diretoria:
                    return $"{Nome}/Diretor";
                case EnumTipoCargo.Gabinete:
                    return $"{CargoPai.Nome}/{Nome}/Chefe de Gabinete";
                case EnumTipoCargo.Gerencia:
                    return $"{CargoPai.CargoPai.Nome}/{Nome}/Gerente";
                case EnumTipoCargo.GerenciaAdjunta:
                    return $"{CargoPai.CargoPai.Nome}/{Nome}/Gerente Adjunto";
                case EnumTipoCargo.Funcionario:
                    if (CargoPai.TipoCargo == EnumTipoCargo.Gabinete)
                        return $"{CargoPai.CargoPai.Nome}/{CargoPai.Nome}/Funcionário";
                    
                    return $"{CargoPai.CargoPai.Nome}/{Nome}/Funcionário";
                default:
                    throw new Exception("Tipo de cargo não implementado");
            }
        }

        /// <summary>
        /// Retorna o primeiro usuário da lista de usuários de um carco, ciclando pelos cargos pais até o ciclo chegar a 0.
        /// </summary>
        /// <param name="ciclos">Quantidade de ciclos de Cargos pais.</param>
        /// <returns></returns>
        public virtual UsuarioCargo ObterCargoPai(int ciclos)
        {
            if (ciclos != 0)
            {
                if (CargoPai != null)
                    return CargoPai.ObterCargoPai(ciclos - 1);

                return null;
            }

            return UsuariosCargos.FirstOrDefault();
        }

        public virtual Cargo DiretoriaCargo()
        {
            Cargo cargoDiretoria = new Classes.Cargo();
            switch (TipoCargo)
            {
                case EnumTipoCargo.Gabinete:
                    // Se o demandante for Chefe de Gabinete, retorna o seu Diretor.
                    cargoDiretoria = ObterUnidadePai(0);

                    break;
                case EnumTipoCargo.Gerencia:
                case EnumTipoCargo.GerenciaAdjunta:
                    // Se o demandante for Gerente, retorna seu Diretor.
                    // Se o demandante for Gerente Adjunto, retorna seu Chefe de Gabinete.
                    cargoDiretoria = ObterUnidadePai(1);

                    break;
                case EnumTipoCargo.Funcionario:
                    // Se o demandante for Funcionário, retorna seu Diretor.
                    cargoDiretoria = ObterUnidadePai(3);

                    cargoDiretoria = cargoDiretoria == null ? ObterUnidadePai(1) : cargoDiretoria;

                    break;
                default:
                    cargoDiretoria = this;
                    break;
            }

            //CASO AINDA EXISTA UM CARGO ACIMA, RETORNA O PAI
            cargoDiretoria = cargoDiretoria != null && cargoDiretoria.CargoPai != null ? cargoDiretoria.CargoPai : cargoDiretoria;

            return cargoDiretoria;
        }

        public virtual Cargo ObterUnidadePai(int ciclos)
        {
            if (ciclos != 0)
            {
                if (CargoPai != null)
                {
                    return CargoPai.ObterUnidadePai(ciclos - 1);
                }

                return null;
            }

            return CargoPai;
        }

        public virtual bool UsuarioPodeRepetirNoCargo()
        {
            return TipoCargo == EnumTipoCargo.Gerencia || TipoCargo == EnumTipoCargo.GerenciaAdjunta;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}