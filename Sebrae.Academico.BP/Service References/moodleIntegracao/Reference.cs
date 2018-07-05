﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sebrae.Academico.BP.moodleIntegracao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="moodleIntegracao.IntegracaoSoap")]
    public interface IntegracaoSoap {
        
        // CODEGEN: Generating message contract since element name nome from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/MatricularAluno", ReplyAction="*")]
        Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoResponse MatricularAluno(Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class MatricularAlunoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="MatricularAluno", Namespace="http://tempuri.org/", Order=0)]
        public Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequestBody Body;
        
        public MatricularAlunoRequest() {
        }
        
        public MatricularAlunoRequest(Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class MatricularAlunoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string nome;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string cpf;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string email;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string cidade;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string codCurso;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string codTurma;
        
        public MatricularAlunoRequestBody() {
        }
        
        public MatricularAlunoRequestBody(string nome, string cpf, string email, string cidade, string codCurso, string codTurma) {
            this.nome = nome;
            this.cpf = cpf;
            this.email = email;
            this.cidade = cidade;
            this.codCurso = codCurso;
            this.codTurma = codTurma;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class MatricularAlunoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="MatricularAlunoResponse", Namespace="http://tempuri.org/", Order=0)]
        public Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoResponseBody Body;
        
        public MatricularAlunoResponse() {
        }
        
        public MatricularAlunoResponse(Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class MatricularAlunoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string MatricularAlunoResult;
        
        public MatricularAlunoResponseBody() {
        }
        
        public MatricularAlunoResponseBody(string MatricularAlunoResult) {
            this.MatricularAlunoResult = MatricularAlunoResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IntegracaoSoapChannel : Sebrae.Academico.BP.moodleIntegracao.IntegracaoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IntegracaoSoapClient : System.ServiceModel.ClientBase<Sebrae.Academico.BP.moodleIntegracao.IntegracaoSoap>, Sebrae.Academico.BP.moodleIntegracao.IntegracaoSoap {
        
        public IntegracaoSoapClient() {
        }
        
        public IntegracaoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IntegracaoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntegracaoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntegracaoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoResponse Sebrae.Academico.BP.moodleIntegracao.IntegracaoSoap.MatricularAluno(Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequest request) {
            return base.Channel.MatricularAluno(request);
        }
        
        public string MatricularAluno(string nome, string cpf, string email, string cidade, string codCurso, string codTurma) {
            Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequest inValue = new Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequest();
            inValue.Body = new Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoRequestBody();
            inValue.Body.nome = nome;
            inValue.Body.cpf = cpf;
            inValue.Body.email = email;
            inValue.Body.cidade = cidade;
            inValue.Body.codCurso = codCurso;
            inValue.Body.codTurma = codTurma;
            Sebrae.Academico.BP.moodleIntegracao.MatricularAlunoResponse retVal = ((Sebrae.Academico.BP.moodleIntegracao.IntegracaoSoap)(this)).MatricularAluno(inValue);
            return retVal.Body.MatricularAlunoResult;
        }
    }
}
