<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaxaDeAprovacaoNoAno.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.TaxaDeAprovacaoNoAno" %>
<asp:Chart ID="chartMatriculasNoAnoPorUf"
    runat="server" 
    BackColor="White" 
    Height="350px" 
    Width="450px"
    BorderColor="26, 59, 105" 
    BorderWidth="2"
    Palette="Pastel"
    CssClass="force img-responsive" 
    ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" 
    RenderType="BinaryStreaming"
    >
    <Series>
        <asp:Series XValueType="String" Name="Ano">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisY LineColor="64, 64, 64, 64">
                <LabelStyle Font="Verdana, 8.25pt" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisY>
            <AxisX LineColor="64, 64, 64, 64">
                <LabelStyle Font="Verdana, 8.25pt" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisX>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
