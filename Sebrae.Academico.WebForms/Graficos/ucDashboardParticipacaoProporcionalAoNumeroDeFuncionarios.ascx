<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios" %>

<asp:Chart ID="chartParticipacaoProporcionalAoNumeroDeFuncionarios" 
    runat="server"
    BackColor="White"  
    Palette="Pastel"
    BorderColor="26, 59, 105" 
    BorderWidth="2" 
    Height="450px"
    Width="650px"
    ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" 
    CssClass="force img-responsive"
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
