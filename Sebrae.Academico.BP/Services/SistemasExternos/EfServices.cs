using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Services.EF;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SistemasExternos
{
    public class EfServices
    {
        private WebService _Api;

        private const string ServiceName = "ICorpAccountService";

        private const string XmlNsNamespace = "EFSchools.Englishtown.Commerce.Client.Partners.Corp";

        private const string DivisionCode = "SEBRAE";

        private const string CountryCode = "BR";

        private const string LanguageCode = "pt";

        private const string IdentityType = "EmployeeId";

        private const string RedemptionProvisionType = "Auto";

        private readonly string _efUsername;

        private readonly string _efPassword;

        private readonly string _redemptionCode;

        #region Envelopes

        private const string HeaderEnvelop =
            @"<Header>
                <Security xmlns=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
                    <UsernameToken>
                        <Username>{0}</Username>
                        <Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{1}</Password>
                    </UsernameToken>
                </Security>
            </Header>";

        private const string CreateMemberEnvelope =
            @" <Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                " + HeaderEnvelop + @"
                <Body>
                    <CreateMember xmlns=""http://tempuri.org/"">
                        <createMemberParams>
                            {2}
                        </createMemberParams>
                    </CreateMember>
                </Body>
            </Envelope>";

        private const string CreateMemberAndActivateSubscriptionEnvelope =
            @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                " + HeaderEnvelop + @"
                <Body>
                    <CreateMemberAndActivateSubscription xmlns=""http://tempuri.org/"">
                        <createAccountParams>
                            {2}
                            <RedemptionCodeProvision xmlns=""EFSchools.Englishtown.Commerce.Client.Partners.Corp"">
                                <RedemptionProvisionType>{3}</RedemptionProvisionType>
                                <RedemptionCode>{4}</RedemptionCode>
                            </RedemptionCodeProvision>
                        </createAccountParams>
                    </CreateMemberAndActivateSubscription>
                </Body>
            </Envelope>";

        private const string ActivateSubscriptionEnvelope =
            @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                " + HeaderEnvelop + @"
                <Body>
                    <ActivateSubscription xmlns=""http://tempuri.org/"">
                        <activateSubscriptionParams>
                            <StudentIdentity xmlns=""EFSchools.Englishtown.Commerce.Client.Partners.Corp"">
                                {2}
                            </StudentIdentity>
                            <RedemptionCodeProvisionInfo xmlns=""EFSchools.Englishtown.Commerce.Client.Partners.Corp"">
                                <RedemptionProvisionType>{3}</RedemptionProvisionType>
                                <RedemptionCode>{4}</RedemptionCode>
                            </RedemptionCodeProvisionInfo>
                        </activateSubscriptionParams>
                    </ActivateSubscription>
                </Body>
            </Envelope>";

        private const string CancelSubscriptionEnvelope =
            @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                " + HeaderEnvelop + @"
                <Body>
                    <CancelSubscription xmlns=""http://tempuri.org/"">
                        <cancelSubscriptionParams>
                            <StudentIdentity xmlns=""EFSchools.Englishtown.Commerce.Client.Partners.Corp"">
                                {2}
                            </StudentIdentity>
                        </cancelSubscriptionParams>
                    </CancelSubscription>
                </Body>
            </Envelope>";

        #endregion

        public EfServices()
        {
            _efUsername = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                    (int)enumConfiguracaoSistema.LoginWebServiceEf).Registro;

            _efPassword = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                    (int)enumConfiguracaoSistema.SenhaWebServiceEf).Registro;

            _redemptionCode = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                    (int)enumConfiguracaoSistema.RedemptionCodeServiceEf).Registro;

            _Api =
                new WebService(
                    new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                        (int) enumConfiguracaoSistema.UrlSoapEf).Registro);
        }

        public void ChangeUrl(string webserviceEndpoint)
        {
            _Api = new WebService(webserviceEndpoint);
        }

        public DTOCreateMemberAndActivateSubscription CreateMemberAndActivateSubscriptionApi(Usuario usuario, bool overwriteExistingMember)
        {
            _Api.PreInvoke();

            // Adicionar envelope utilizado na requisição.
            _Api.RequestEnvelop = CreateMemberAndActivateSubscriptionEnvelope;

            _Api.AddParameter("FirstName", usuario.GetFirstName());
            _Api.AddParameter("LastName", usuario.GetLastName());
            _Api.AddParameter("Email", usuario.Email);
            _Api.AddParameter("DivisionCode", DivisionCode);
            _Api.AddParameter("EmployeeId", usuario.ID.ToString());
            _Api.AddParameter("CountryCode", CountryCode);
            _Api.AddParameter("LanguageCode", LanguageCode);
            _Api.AddParameter("OverwriteExistingMember", overwriteExistingMember.ToString().ToLower());

            var redemptionCodeParams = new List<string> { RedemptionProvisionType, _redemptionCode };
            
            _Api.Invoke(_efUsername, _efPassword, ServiceName, "CreateMemberAndActivateSubscription",
                redemptionCodeParams, XmlNsNamespace);

            // Serializa o retorno para o objeto.
            try
            {
                return XmlUtil.FromXml<DTOCreateMemberAndActivateSubscription>(_Api.ResultXml);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DTOCreateMember CreateMemberApi(Usuario usuario, bool overwriteExistingMember = true)
        {
            _Api.PreInvoke();

            // Adicionar envelope utilizado na requisição.
            _Api.RequestEnvelop = CreateMemberEnvelope;

            _Api.AddParameter("FirstName", usuario.GetFirstName());
            _Api.AddParameter("LastName", usuario.GetLastName());
            _Api.AddParameter("Email", usuario.Email);
            _Api.AddParameter("DivisionCode", DivisionCode);
            _Api.AddParameter("EmployeeId", usuario.ID.ToString());
            _Api.AddParameter("CountryCode", CountryCode);
            _Api.AddParameter("LanguageCode", LanguageCode);
            _Api.AddParameter("OverwriteExistingMember", overwriteExistingMember.ToString().ToLower());

            _Api.Invoke(_efUsername, _efPassword, ServiceName, "CreateMember", null, XmlNsNamespace);

            // Serializa o retorno para o objeto.
            return XmlUtil.FromXml<DTOCreateMember>(_Api.ResultXml);
        }

        public EnglishTownUsuario GetSidMidApi(Usuario usuario)
        {
            if (usuario.Ativo)
            {
                var membroCriadoAtivado = CreateMemberAndActivateSubscriptionApi(usuario, true);

                if (membroCriadoAtivado != null)
                {
                    var url = membroCriadoAtivado.LaunchUrl;

                    var split = url.Split('?')[1].Split('&');

                    var sid = split[0].Split('=')[1];
                    var mid = split[1].Split('=')[1];

                    var dadosEnglishTown = new EnglishTownUsuario
                    {
                        Sid = sid,
                        Mid = mid,
                        Usuario = usuario
                    };

                    return dadosEnglishTown;
                }
            }

            return null;
        }

        public DTOActivateSubscription ActivateSubscriptionApi(Usuario usuario)
        {
            _Api.PreInvoke();

            // Adicionar envelope utilizado na requisição.
            _Api.RequestEnvelop = ActivateSubscriptionEnvelope;

            _Api.AddParameter("IdentityType", IdentityType);
            _Api.AddParameter("IdentityValue", usuario.ID.ToString());
            _Api.AddParameter("DivisionCode", DivisionCode);

            var redemptionCodeParams = new List<string> { RedemptionProvisionType, _redemptionCode };

            _Api.Invoke(_efUsername, _efPassword, ServiceName, "ActivateSubscription", redemptionCodeParams,
                XmlNsNamespace);

            // Serializa o retorno para o objeto.
            return XmlUtil.FromXml<DTOActivateSubscription>(_Api.ResultXml);
        }

        public DTOCancelSubscription CancelSubscriptionApi(Usuario usuario)
        {
            _Api.PreInvoke();

            // Adicionar envelope utilizado na requisição.
            _Api.RequestEnvelop = CancelSubscriptionEnvelope;

            _Api.AddParameter("IdentityType", IdentityType);
            _Api.AddParameter("IdentityValue", usuario.ID.ToString());
            _Api.AddParameter("DivisionCode", DivisionCode);

            _Api.Invoke(_efUsername, _efPassword, ServiceName, "CancelSubscription", null, XmlNsNamespace);

            try
            {
                return XmlUtil.FromXml<DTOCancelSubscription>(_Api.ResultXml);
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
