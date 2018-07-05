using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services.UsuarioPorSID;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SistemasExternos
{
    public class UsuariosADServices : BusinessProcessServicesBase
    {
        public ConsultaUsuarioPorSID ConsultarUsuarioPorSID(string sid)
        {
            ConsultaUsuarioPorSID consultarUsuarioPorSID = new ConsultaUsuarioPorSID();
            Usuario usuario = new BMUsuario().ObterADPorSID(sid);

            if (usuario != null)
            {
                consultarUsuarioPorSID.UsuarioEncontrado = true;
                consultarUsuarioPorSID.Usuario.Nome = usuario.Nome;
                consultarUsuarioPorSID.Usuario.Email = usuario.Email;
            }
            else
            {
                consultarUsuarioPorSID.UsuarioEncontrado = false;
                consultarUsuarioPorSID.Mensagem = "Usuário não encontrado";
                consultarUsuarioPorSID.Usuario = null;
            }

            return consultarUsuarioPorSID;
        }

        public CadastraUsuarioPorSID CadastrarUsuarioPorSID(string sid, string nome, string email, string cpf, string senha)
        {
            BMUsuario bmUsuario = new BMUsuario();
            CadastraUsuarioPorSID consultarUsuarioPorSID = new CadastraUsuarioPorSID();
            consultarUsuarioPorSID.UsuarioRegistrado = false;
            consultarUsuarioPorSID.UsuarioEncontrado = false;
            consultarUsuarioPorSID.Usuario = null;
            
            Usuario usuarioPorSid = bmUsuario.ObterADPorSID(sid);
            if (usuarioPorSid == null)
            {
                if (bmUsuario.AutenticarUsuario(cpf, senha))
                {
                    Usuario usuarioAutenticado = bmUsuario.ObterPorCPF(cpf);
                    if (!string.IsNullOrEmpty(usuarioAutenticado.SID_Usuario))
                    {
                        consultarUsuarioPorSID.UsuarioEncontrado = true;
                        consultarUsuarioPorSID.Mensagem = "O usuário do SGUS já está sincronizado";
                    }
                    else
                    {
                        string erros = string.Empty;

                        if (usuarioAutenticado.Nome.ToLower() != nome.ToLower())
                        {
                            erros = "O nome não foi sincronizado";
                        }
                        if (usuarioAutenticado.Email.ToLower() != email.ToLower())
                        {
                            erros = (string.IsNullOrEmpty(erros) ? "" : ",") + "O email não foi sincronizado";
                        }
                        //if (usuarioAutenticado.NivelOcupacional.ID != (int)enumNivelOcupacional.ADL)
                        //{
                        //    erros = (string.IsNullOrEmpty(erros) ? "" : ",") + "O usuário não está como AD no SGUS";
                        //}

                        if (!string.IsNullOrEmpty(erros))
                        {
                            consultarUsuarioPorSID.UsuarioEncontrado = true;
                            consultarUsuarioPorSID.Mensagem = erros;
                        }
                        else
                        {
                            usuarioAutenticado.SID_Usuario = sid;
                            bmUsuario.Salvar(usuarioAutenticado);

                            consultarUsuarioPorSID.UsuarioEncontrado = true;
                            consultarUsuarioPorSID.UsuarioRegistrado = true;
                            consultarUsuarioPorSID.Usuario = new DTOUsuarioRecuperado();
                            consultarUsuarioPorSID.Usuario.Nome = usuarioAutenticado.Nome;
                            consultarUsuarioPorSID.Usuario.Email = usuarioAutenticado.Email;
                        }
                    }
                }
                else
                {
                    consultarUsuarioPorSID.Mensagem = "Usuário SGUS não foi autenticado";
                }
            }
            else
            {
                consultarUsuarioPorSID.UsuarioEncontrado = true;
                consultarUsuarioPorSID.Mensagem = "O usuário AD já está sincronizado com o SGUS";
            }

            return consultarUsuarioPorSID;
        }

        public CadastraUsuarioPorSID AtualizaSIDPorCPF(string sid, string cpf)
        {

            BMUsuario bmUsuario = new BMUsuario();

            CadastraUsuarioPorSID consultarUsuarioPorSID = new CadastraUsuarioPorSID();
            consultarUsuarioPorSID.UsuarioRegistrado = false;
            consultarUsuarioPorSID.UsuarioEncontrado = false;
            consultarUsuarioPorSID.Usuario = null;

            Usuario usuarioPorCpf = bmUsuario.ObterPorCPF(cpf);

            if (usuarioPorCpf != null)
            {
                usuarioPorCpf.SID_Usuario = sid;
                bmUsuario.Salvar(usuarioPorCpf);
                consultarUsuarioPorSID.UsuarioEncontrado = true;
                consultarUsuarioPorSID.Mensagem = "Usuário atualizado com sucesso";
                consultarUsuarioPorSID.UsuarioRegistrado = true;
            }
            else
            {
                consultarUsuarioPorSID.Mensagem = "Não foi encontrado o usuário";                
            }

            return consultarUsuarioPorSID;
        }

        public DTOConsultaUsuarioPorFiltro ConsultarUsuarioPorFiltro(string nome, string email, int ID_UF, int ID_NivelOcupacional, int page, int maxPerPage)
        {
            DTOConsultaUsuarioPorFiltro dtoConsultaUsuarioPorFiltro = new DTOConsultaUsuarioPorFiltro();

            BMUsuario bmUsuario = new BMUsuario();

            Usuario usuario = new Usuario{
                Nome = nome,
                Email = email
            };

            // Obtem o estado especificado
            if (ID_UF != 0)
            {
                Uf uf = new BMUf().ObterPorId(ID_UF);

                if (uf!=null && uf.ID != 0)
                {
                    usuario.UF = uf;
                }
            }

            // Obtem o nivel ocupacional especificado
            if (ID_NivelOcupacional != 0)
            {
                NivelOcupacional nivelOcupacional = new BMNivelOcupacional().ObterPorID(ID_NivelOcupacional);

                if (nivelOcupacional != null && nivelOcupacional.ID != 0)
                {
                    usuario.NivelOcupacional = nivelOcupacional;
                }
            }

            bmUsuario = new BMUsuario();

            // Máximo por página
            maxPerPage = maxPerPage > 500 ? 500 : maxPerPage;

            var query = bmUsuario.ObterQueryPorFiltros(usuario);

            IList<Usuario> usuarios;

            var total = query.Count();

            query = query.OrderBy(x => x.Nome);

            if (total > maxPerPage)
            {                
                query = query.Select(x => new Usuario { ID = x.ID, Nome = x.Nome, Email = x.Email, CPF = x.CPF, DataNascimento = x.DataNascimento, SID_Usuario = x.SID_Usuario, Senha = x.Senha, Situacao = x.Situacao.Trim(), Sexo = x.Sexo });
                query = query.Skip(page * maxPerPage).Take(maxPerPage);
                usuarios = query.ToList();

                dtoConsultaUsuarioPorFiltro.totalPages = (int) Math.Ceiling( (double) total / (double) maxPerPage );
            }
            else
            {
                usuarios = query.ToList();
                dtoConsultaUsuarioPorFiltro.totalPages = 1;
            }

            if (usuarios.Any()) {
                foreach (var u in usuarios)
                {
                    dtoConsultaUsuarioPorFiltro.ListaConsultaUsuario.Add(
                        new DTOConsultaUsuarioPorFiltroItem
                        {
                            Nome = u.Nome,
                            Email = u.Email,
                            CPF = u.CPF,
                            DataNascimento = u.DataNascimento,
                            SID_Usuario = u.SID_Usuario,
                            SenhaMD5 = CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(u.Senha)),
                            Situacao = u.Situacao.Trim(),
                            Sexo = u.Sexo
                        }
                    );
                }
            }

            dtoConsultaUsuarioPorFiltro.totalRegistros = total;
            dtoConsultaUsuarioPorFiltro.currentPage = page;            

            return dtoConsultaUsuarioPorFiltro;
        }
    }
}
