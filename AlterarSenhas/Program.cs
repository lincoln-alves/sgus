using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlterarSenhas
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Forneca um ID do perfil ou o nome do mesmo como 1° argumento.\nPressione qualquer tecla para fechar.");
                Console.ReadKey();
            }
            
            enumPerfil perfil;
            if (Enum.TryParse(args[0], out perfil))
            {
                Console.WriteLine("Verificando usuários com perfil {0}", perfil.ToString());

                ManterUsuario manterUsuario = new ManterUsuario();

                List<Usuario> usuarios = manterUsuario.ObterTodosPorPerfilIQueryable(perfil).ToList();

                Console.WriteLine("Deseja realmente resetar a senha para o cpf de {0} usuários?", usuarios.Count());
                switch (Console.ReadLine().ToUpper()) {
                    case "S":
                    case "Y":
                        usuarios.ForEach(usuario =>
                        {
                            Console.WriteLine("{0} - {1}", usuario.CPF, usuario.Nome);
                            usuario.Senha = CriptografiaHelper.Criptografar(usuario.CPF);
                        });

                        manterUsuario.SalvarEmLote(usuarios, 100);

                        Console.WriteLine("Operação realizada, as senhas de usuários {0} foram resetadas.\nPressione qualquer tecla para fechar.", usuarios.Count());
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Operação cancelada pelo utilizador.\nPressione qualquer tecla para fechar.");
                        Console.ReadKey();
                        break;
                };
            }
            else
            {
                Console.WriteLine("Perfil não encontrado.\nPressione qualquer tecla para fechar.");
                Console.ReadKey();
            }
        }
    }
}
