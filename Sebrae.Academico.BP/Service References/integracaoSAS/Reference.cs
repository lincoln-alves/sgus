﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sebrae.Academico.BP.integracaoSAS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="integracaoSAS.ITreinamentoSas")]
    public interface ITreinamentoSas {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITreinamentoSas/CadastrarUsuario", ReplyAction="http://tempuri.org/ITreinamentoSas/CadastrarUsuarioResponse")]
        string CadastrarUsuario(string xml, string chaveAutenticacao);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITreinamentoSasChannel : Sebrae.Academico.BP.integracaoSAS.ITreinamentoSas, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TreinamentoSasClient : System.ServiceModel.ClientBase<Sebrae.Academico.BP.integracaoSAS.ITreinamentoSas>, Sebrae.Academico.BP.integracaoSAS.ITreinamentoSas {
        
        public TreinamentoSasClient() {
        }
        
        public TreinamentoSasClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TreinamentoSasClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TreinamentoSasClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TreinamentoSasClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string CadastrarUsuario(string xml, string chaveAutenticacao) {
            return base.Channel.CadastrarUsuario(xml, chaveAutenticacao);
        }
    }
}