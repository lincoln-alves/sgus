using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumTipoCampo
    {
        [Description("Texto")]
        Text = 1,

        [Description("Área de Texto")]
        TextArea = 2,

        [Description("Senha")]
        Password = 3,

        [Description("Lista de Opções")]
        DropDownList = 4,

        [Description("Radiobutton")]
        RadioButton = 5,

        [Description("Checkbox")]
        CheckBox = 6,

        [Description("Arquivo")]
        FileUpload = 7,

        [Description("Campo do Usuário")]
        Field = 8,

        [Description("Label")]
        Label = 9,

        [Description("Conteúdo HTML")]
        Html = 10,

        [Description("Somatório")]
        Somatório = 11,

        [Description("Divisor")]
        Divisor = 12,

        [Description("Multiplicador")]
        Multiplicador = 13,

        [Description("Percentual")]
        Percentual = 14,

        [Description("Questionário")]
        Questionário = 15,

        [Description("Subtração")]
        Subtracao = 16,

        [Description("Multiplos Arquivos")]
        MultipleFileUpload = 17
    }
}
