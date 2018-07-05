<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParticipacaoProporcionalAoNumeroDeFuncionarios.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.ParticipacaoProporcionalAoNumeroDeFuncionarios" %>
<asp:Chart ID="chartParticipacaoProporcionalAoNumeroDeFuncionarios" 
    runat="server"
    BackColor="White"  
    Height="450px"
    Width="650px"   
    BorderColor="26, 59, 105" 
    BorderWidth="2" 
    Palette="Pastel"
    CssClass="force img-responsive" 
    RenderType="BinaryStreaming"
    >
    <Series>
        <asp:Series Name="Series1" ChartType="Column" ChartArea="ChartArea1" CustomProperties="DrawingStyle=Default">
        </asp:Series>
        <asp:Series Name="Series2" ChartType="Column" ChartArea="ChartArea1" CustomProperties="DrawingStyle=Default">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisX>
                <LabelStyle Font="Verdana, 8.25pt" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisX>
            <AxisY Title="Número de inscritos">
                <LabelStyle Font="Verdana, 8.25pt" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisY>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
