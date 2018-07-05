using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Dominio;
using System.Text;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterPublicacaoSaber : BusinessProcessServicesBase
    {

        BMPublicacaoSaber bmPublicacaoSaber;

        public void AlterarPublicacaoSaber(string Titulo, int ChaveExterna, List<string> ListaCPFAutor,
                                           bool publicado, string TextoResenha, DateTime DataPublicacao,
                                           string siglaUF, string NomeCompleto, string TextoAssunto, string Login)
        {
            BMPublicacaoSaber bmPublicacaoSaber = new BMPublicacaoSaber();
            PublicacaoSaber publicacaoSaber = this.ObterObjetoPublicacaoSaber(Titulo, ChaveExterna, TextoResenha, DataPublicacao, siglaUF, NomeCompleto, TextoAssunto, Login);

            //Se existe, faz update
            if (publicacaoSaber != null && publicacaoSaber.ID > 0)
            {
                IList<PublicacaoSaberUsuario> ListaPublicacaoSaberUsuario = publicacaoSaber.ListaPublicacaoSaberUsuario;
                PublicacaoSaberUsuario publicacaoSaberUsuario = null;

                foreach (string cpf in ListaCPFAutor)
                {
                    if (!string.IsNullOrWhiteSpace(cpf))
                    {
                        bool cpfJaExiste = VerificarExistenciaDeCPF(cpf, ListaPublicacaoSaberUsuario);

                        if (!cpfJaExiste)
                        {
                            publicacaoSaberUsuario = this.ObterObjetoUsuario(publicacaoSaberUsuario, cpf);
                            publicacaoSaberUsuario.PublicacaoSaber = publicacaoSaber;
                            publicacaoSaber.ListaPublicacaoSaberUsuario.Add(publicacaoSaberUsuario);
                        }

                    }
                }

                bmPublicacaoSaber.Salvar(publicacaoSaber);
            }
            //Senão existe, insere
            else
            {
                this.InserePublicacaoSaber(0, Titulo, ChaveExterna, ListaCPFAutor, publicado, TextoResenha, DataPublicacao, siglaUF,
                                           NomeCompleto, TextoAssunto, Login);
            }


        }

        private bool VerificarExistenciaDeCPF(string cpf, IList<PublicacaoSaberUsuario> ListaPublicacaoSaberUsuario)
        {
            bool cpfJaExisteNaLista = false;

            if (ListaPublicacaoSaberUsuario != null && ListaPublicacaoSaberUsuario.Count > 0)
            {
                foreach (PublicacaoSaberUsuario publicacaoSaberUsuario in ListaPublicacaoSaberUsuario)
                {
                    if (publicacaoSaberUsuario.Usuario.CPF.Trim().Equals(cpf.Trim()))
                    {
                        cpfJaExisteNaLista = true;
                        //Sai do Loop, pois o cpf está na lista de cpfs do usuário
                        break;
                    }
                }
            }

            return cpfJaExisteNaLista;
        }

        private void InserePublicacaoSaber(int IdPublicacaoSaber, string Titulo, int ChaveExterna, List<string> ListaCPFAutor, bool publicado,
                                           string TextoResenha, DateTime DataPublicacao, string siglaUF,
                                           string NomeCompleto, string TextoAssunto, string Login)
        {
            BMPublicacaoSaber bmPublicacaoSaber = new BMPublicacaoSaber();
            PublicacaoSaber publicacaoSaber = this.ObterObjetoPublicacaoSaber(Titulo, ChaveExterna, TextoResenha, DataPublicacao, siglaUF, NomeCompleto, TextoAssunto, Login);

            if (ListaCPFAutor != null)
            {
                publicacaoSaber.ListaPublicacaoSaberUsuario = new List<PublicacaoSaberUsuario>();
                PublicacaoSaberUsuario publicacaoSaberUsuario = null;

                //Percorre a lista de CPFs dos Autores para adicionar na lista de PublicacaoSaber do Usuario
                foreach (string cpf in ListaCPFAutor)
                {
                    if (!string.IsNullOrWhiteSpace(cpf))
                    {
                        bool cpfJaExiste = VerificarExistenciaDeCPF(cpf, publicacaoSaber.ListaPublicacaoSaberUsuario);

                        if (!cpfJaExiste)
                        {
                            publicacaoSaberUsuario = this.ObterObjetoUsuario(publicacaoSaberUsuario, cpf);
                            publicacaoSaberUsuario.PublicacaoSaber = publicacaoSaber;
                            publicacaoSaber.ListaPublicacaoSaberUsuario.Add(publicacaoSaberUsuario);
                        }

                    }
                }
            }


            if (publicado)
            {
                publicacaoSaber.Publicado = true;
            }
            else
            {
                publicacaoSaber.Publicado = false;
            }

            bmPublicacaoSaber.Salvar(publicacaoSaber);
        }

        private PublicacaoSaber ObterObjetoPublicacaoSaber(string Titulo, int ChaveExterna,
                                                           string TextoResenha, DateTime DataPublicacao,
                                                           string siglaUF, string NomeCompleto, string TextoAssunto, string Login)
        {

            PublicacaoSaber publicacaoSaber = bmPublicacaoSaber.ObterPorChaveExterna(ChaveExterna);

            if (publicacaoSaber == null ||
               (publicacaoSaber != null && publicacaoSaber.ID == 0))
            {
                publicacaoSaber = new PublicacaoSaber();
            }

            publicacaoSaber.TextoTitulo = Titulo;
            publicacaoSaber.IDChaveExterna = ChaveExterna;
            publicacaoSaber.DataPublicacao = DataPublicacao;
            publicacaoSaber.Nome = NomeCompleto;
            publicacaoSaber.TextoResenha = TextoResenha;
            publicacaoSaber.TextoAssunto = TextoAssunto;
            publicacaoSaber.Auditoria = new Auditoria(Login);
            publicacaoSaber.UF = new ManterUf().ObterUfPorSigla(siglaUF);

            return publicacaoSaber;
        }

        private PublicacaoSaberUsuario ObterObjetoUsuario(PublicacaoSaberUsuario publicacaoSaberUsuario, string cpf)
        {
            publicacaoSaberUsuario = new PublicacaoSaberUsuario();
            Usuario usuario = new ManterUsuario().ObterPorCPF(cpf);
            publicacaoSaberUsuario.Usuario = usuario;
            return publicacaoSaberUsuario;
        }

        public List<DTOPublicacaoSaber> ConsultarPublicacaoSaber()
        {
            List<PublicacaoSaber> lstPublicacaoSaber = new BMPublicacaoSaber().ObterPublicacaoSaber(5).ToList();

            if (lstPublicacaoSaber.Count == 0)
                return new List<DTOPublicacaoSaber>();

            return ConverterDominioEmDTO(lstPublicacaoSaber).ToList();
        }

        private IList<DTOPublicacaoSaber> ConverterDominioEmDTO(List<PublicacaoSaber> pListaPublicacaoSaber)
        {
            IList<DTOPublicacaoSaber> lstResult = new List<DTOPublicacaoSaber>();
            StringBuilder sbAutores = new StringBuilder();

            foreach (PublicacaoSaber n in pListaPublicacaoSaber)
            {
                DTOPublicacaoSaber ndto = new DTOPublicacaoSaber()
                {
                    DataPublicacao = n.DataPublicacao,
                    Nome = n.Nome,
                    ID = n.ID,
                    TextoAssunto = n.TextoAssunto,
                    TextoResenha = n.TextoResenha,
                    TextoTitulo = n.TextoTitulo
                };

                n.ListaPublicacaoSaberUsuario.ToList().ForEach(x => sbAutores.Append(string.Concat(x.Usuario.Nome, ", ")));

                string autores = sbAutores.ToString().Trim();

                if (autores.EndsWith(","))
                {
                    autores = autores.Remove(autores.Length - 1, 1);
                }

                ndto.ListaAutores = autores;
                lstResult.Add(ndto);

            }

            return lstResult;
        }


    }
}
