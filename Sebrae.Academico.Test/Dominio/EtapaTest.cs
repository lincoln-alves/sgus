using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Sebrae.Academico.Dominio.Attributes.TestValidationAttributes;
using Sebrae.Academico.Dominio.Attributes.TestValidationAttributes.EtapaPermissao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Test.Dominio
{
    [TestFixture]
    public class EtapaTest
    {
        [Test]
        public void TodasProriedadesEtapaPermissaoPrecisamDeNotacao()
        {

            var propriedades =
                typeof (EtapaPermissao).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                      BindingFlags.DeclaredOnly);

            var propriedadesSemNotacao =
                propriedades.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(
                                y =>
                                    y.GetType() == typeof (IsSingularEtapaPermissaoField) ||
                                    y.GetType() == typeof (IsCompositeEtapaPermissaoField) ||
                                    y.GetType() == typeof (IsNotEtapaPermissaoField)) == null);

            Assert.AreEqual(0, propriedadesSemNotacao, "Todas as propriedades da EtapaPermissao precisam de uma notação. Verifique os atributos IsSingularEtapaPermissaoField, IsCompositeEtapaPermissaoField e IsNotEtapaPermissaoField para mais informações");
        }

        /// <summary>
        /// Testa se todas as permissões foram implementadas no método PermissaoExiste.
        /// </summary>
        [Test]
        public void DeveImplementarExistenciaDeTodasAsPermissoes()
        {
            var methods =
                typeof (Etapa).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var metodosSingularesImplementados =
                methods.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(y => y.GetType() == typeof (ShouldImplementOneEtapaPermissaoField)) != null);

            var metodosCompostosImplementados =
                methods.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(y => y.GetType() == typeof (ShouldImplementTwoEtapaPermissaoField)) != null);

            var propriedades =
                typeof (EtapaPermissao).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                      BindingFlags.DeclaredOnly);

            var propriedadesSingulares =
                propriedades.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(y => y.GetType() == typeof (IsSingularEtapaPermissaoField)) != null);

            var propriedadesCompostas =
                propriedades.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(y => y.GetType() == typeof (IsCompositeEtapaPermissaoField)) != null);

            Assert.AreEqual(metodosSingularesImplementados, propriedadesSingulares, 0,
                "A quantidade de métodos singulares implementados é diferente da quantidade de permissões singulares existentes da EtapaPermissao");

            Assert.AreEqual((metodosCompostosImplementados / 2), propriedadesCompostas, 0,
                "A quantidade de métodos compostos implementados é diferente da quantidade de permissões compostas existentes da EtapaPermissao");
        }

        [Test]
        public void ValidarTipoEtapaPermissao()
        {

            var qntTiposPermissoes =
                Enum.GetValues(typeof (enumTipoEtapaPermissao)).Cast<enumTipoEtapaPermissao>().Count();

            var propriedades =
                typeof (EtapaPermissao).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                      BindingFlags.DeclaredOnly);

            var propriedadesSingulares =
                propriedades.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(y => y.GetType() == typeof (IsSingularEtapaPermissaoField)) !=
                        null);

            var propriedadesCompostas =
                propriedades.Count(
                    x =>
                        x.GetCustomAttributes(false)
                            .FirstOrDefault(y => y.GetType() == typeof (IsCompositeEtapaPermissaoField)) !=
                        null);


            Assert.AreEqual(propriedadesSingulares + (propriedadesCompostas * 2), qntTiposPermissoes, 0,
                "A quantidade de enums implementados no Tipo da Etapa Permissão é diferente da quantidade de propriedades existentes da EtapaPermissao. Implemente as novas propriedades da EtapaPermissao no enumTipoEtapaPermissao");

            Assert.Throws<InvalidCastException>(() => new EtapaPermissao().ObterTipoEtapaPermissao(),
                "Novo tipo de permissão não implementado");

            // Testes de tipos COMPLEXOS

            // Perfil
            var etapaPermissaoPerfil = new EtapaPermissao {Perfil = new Perfil {ID = 10}};
            var tipoValuePerfil = etapaPermissaoPerfil.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.Perfil, tipoValuePerfil.Key);
            Assert.AreEqual(etapaPermissaoPerfil.Perfil.ID, tipoValuePerfil.Value);

            // Nível Ocupacional
            var etapaPermissaoNivelOcupacional = new EtapaPermissao {NivelOcupacional = new NivelOcupacional {ID = 10}};
            var tipoValueNivelOcupacional = etapaPermissaoNivelOcupacional.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.NivelOcupacional, tipoValueNivelOcupacional.Key);
            Assert.AreEqual(etapaPermissaoNivelOcupacional.NivelOcupacional.ID, tipoValueNivelOcupacional.Value);

            // Uf
            var etapaPermissaoUf = new EtapaPermissao {Uf = new Uf {ID = 10}};
            var tipoValueUf = etapaPermissaoUf.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.Uf, tipoValueUf.Key);
            Assert.AreEqual(etapaPermissaoUf.Uf.ID, tipoValueUf.Value);

            // Usuário
            // Notificar
            var etapaPermissaoNotificarUsuario = new EtapaPermissao { Notificar = true, Usuario = new Usuario { ID = 10 } };
            var tipoValueNotificarUsuario = etapaPermissaoNotificarUsuario.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.NotificarUsuario, tipoValueNotificarUsuario.Key);
            Assert.AreEqual(etapaPermissaoNotificarUsuario.Usuario.ID, tipoValueNotificarUsuario.Value);

            // Usuário
            // Analisar
            var etapaPermissaoAnalisarUsuario = new EtapaPermissao { Analisar = true, Usuario = new Usuario { ID = 10 } };
            var tipoValueAnalisarUsuario = etapaPermissaoAnalisarUsuario.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.AnalisarUsuario, tipoValueAnalisarUsuario.Key);
            Assert.AreEqual(etapaPermissaoAnalisarUsuario.Usuario.ID, tipoValueAnalisarUsuario.Value);

            // Testes de tipos SIMPLES

            // Solicitante
            // Notificar
            var etapaPermissaoNotificarSolicitante = new EtapaPermissao { Notificar = true, Solicitante = true};
            var tipoValueNotificarSolicitante = etapaPermissaoNotificarSolicitante.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.NotificarSolicitante, tipoValueNotificarSolicitante.Key);
            Assert.AreEqual(etapaPermissaoNotificarSolicitante.Notificar, tipoValueNotificarSolicitante.Value);

            // Analisa
            var etapaPermissaoAnalisarSolicitante = new EtapaPermissao { Analisar = true, Solicitante = true};
            var tipoValueAnalisarrSolicitante = etapaPermissaoAnalisarSolicitante.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.AnalisarSolicitante, tipoValueAnalisarrSolicitante.Key);
            Assert.AreEqual(etapaPermissaoAnalisarSolicitante.Solicitante, tipoValueAnalisarrSolicitante.Value);

            // ChefeImediato
            // Notificar
            var etapaPermissaoNotificarChefeImediato = new EtapaPermissao { Notificar = true, ChefeImediato = true};
            var tipoValueNotificarChefeImediato = etapaPermissaoNotificarChefeImediato.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.NotificarChefeImediato, tipoValueNotificarChefeImediato.Key);
            Assert.AreEqual(etapaPermissaoNotificarChefeImediato.ChefeImediato, tipoValueNotificarChefeImediato.Value);

            // Analisar
            var etapaPermissaoAnalisarChefeImediato = new EtapaPermissao { Analisar = true, ChefeImediato = true};
            var tipoValueAnalisarChefeImediato = etapaPermissaoAnalisarChefeImediato.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.AnalisarChefeImediato, tipoValueAnalisarChefeImediato.Key);
            Assert.AreEqual(etapaPermissaoAnalisarChefeImediato.ChefeImediato, tipoValueAnalisarChefeImediato.Value);

            // GerenteAdjunto
            // Notificar
            var etapaPermissaoNotificarGerenteAdjunto = new EtapaPermissao { Notificar = true, GerenteAdjunto = true };
            var tipoValueNotificarGerenteAdjunto = etapaPermissaoNotificarGerenteAdjunto.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.NotificarGerenteAdjunto, tipoValueNotificarGerenteAdjunto.Key);
            Assert.AreEqual(etapaPermissaoNotificarGerenteAdjunto.GerenteAdjunto, tipoValueNotificarGerenteAdjunto.Value);
            // Analisar
            var etapaPermissaoAnalisarGerenteAdjunto = new EtapaPermissao { Analisar = true, GerenteAdjunto = true };
            var tipoValueAnalisarGerenteAdjunto = etapaPermissaoAnalisarGerenteAdjunto.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.AnalisarGerenteAdjunto, tipoValueAnalisarGerenteAdjunto.Key);
            Assert.AreEqual(etapaPermissaoAnalisarGerenteAdjunto.GerenteAdjunto, tipoValueAnalisarGerenteAdjunto.Value);

            // DiretorCorrespondente
            // Notificar
            var etapaPermissaoNotificarDiretorCorrespondente = new EtapaPermissao { Notificar = true, DiretorCorrespondente = true };
            var tipoValueNotificarDiretorCorrespondente = etapaPermissaoNotificarDiretorCorrespondente.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.NotificarDiretorCorrespondente, tipoValueNotificarDiretorCorrespondente.Key);
            Assert.AreEqual(etapaPermissaoNotificarDiretorCorrespondente.DiretorCorrespondente, tipoValueNotificarDiretorCorrespondente.Value);
            // Analisar
            var etapaPermissaoAnalisarDiretorCorrespondente = new EtapaPermissao { Analisar = true, DiretorCorrespondente = true };
            var tipoValueAnalisarDiretorCorrespondente = etapaPermissaoAnalisarDiretorCorrespondente.ObterTipoEtapaPermissao();
            Assert.AreEqual(enumTipoEtapaPermissao.AnalisarDiretorCorrespondente, tipoValueAnalisarDiretorCorrespondente.Key);
            Assert.AreEqual(etapaPermissaoAnalisarDiretorCorrespondente.DiretorCorrespondente, tipoValueAnalisarDiretorCorrespondente.Value);
        }
    }
}
