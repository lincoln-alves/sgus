using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Collections.Generic;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPublicacaoSaber : BusinessManagerBase
    {
        private RepositorioBase<PublicacaoSaber> repositorio;

        #region "Construtor"

        public BMPublicacaoSaber()
        {
            repositorio = new RepositorioBase<PublicacaoSaber>();
        }

        #endregion

        /// <summary>
        /// Verifica se o Id da chave externa existe na tabela.
        /// </summary>
        /// <param name="IdChaveExterna">Id da Chave Externa</param>
        /// <returns>Retorna true, se a chave externa existir. Retorna false, se a chave externa não existir.</returns>
        public bool VerificarExistenciaDeChaveExterna(int IdChaveExterna)
        {
            var query = repositorio.session.Query<PublicacaoSaber>();
            bool chaveExternaExiste = query.Any(x => x.IDChaveExterna == IdChaveExterna);
            return chaveExternaExiste;
        }

        /// <summary>
        /// Obtém um objeto PublicacaoSaber pelo Id da Chave Externa.
        /// </summary>
        /// <param name="IdChaveExterna">Id da Chave Externa</param>
        /// <returns>Objeto Publicacao Saber</returns>
        public PublicacaoSaber ObterPorChaveExterna(int IdChaveExterna)
        {
            var query = repositorio.session.Query<PublicacaoSaber>();
            PublicacaoSaber publicacaoSaber = query.FirstOrDefault(x => x.IDChaveExterna == IdChaveExterna);
            return publicacaoSaber;
        }

        /// <summary>
        /// Obtém uma lista de Publicações Saber
        /// </summary>
        /// <param name="qtdPublicacoes">Quantidade de Publicações Desejadas</param>
        /// <returns></returns>
        public IList<PublicacaoSaber> ObterPublicacaoSaber(int qtdPublicacoes)
        {
            var query = repositorio.session.Query<PublicacaoSaber>();
            return query.Where(x => x.Publicado).Take(qtdPublicacoes).OrderByDescending(x => x.DataPublicacao).ToList<PublicacaoSaber>();
        }

        public void Salvar(PublicacaoSaber publicacaoSaber)
        {
            ValidarPublicaoSaberInformada(publicacaoSaber);
            repositorio.Salvar(publicacaoSaber);
        }

        private void ValidarPublicaoSaberInformada(PublicacaoSaber pPublicacaoSaber)
        {
            this.ValidarInstancia(pPublicacaoSaber);
            if (string.IsNullOrWhiteSpace(pPublicacaoSaber.Nome)) throw new Exception("Nome Não Informado. Campo Obrigatório!");

            this.VerificarExistenciaDePublicacaoSaber(pPublicacaoSaber);
        }

        private void VerificarExistenciaDePublicacaoSaber(PublicacaoSaber pPublicacaoSaber)
        {

            PublicacaoSaber publicacaoSaber = this.ObterPorChaveExterna(pPublicacaoSaber.IDChaveExterna);

            if (publicacaoSaber != null)
            {
                if (pPublicacaoSaber.ID != publicacaoSaber.ID)
                {
                    throw new AcademicoException(string.Format("A Chave Externa '{0}' já está cadastrada", pPublicacaoSaber.Nome.Trim()));
                }
            }
        }
    }

}