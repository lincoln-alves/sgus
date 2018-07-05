<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TurmasPorStatus.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.TurmasPorStatus" %>
<asp:Chart ID="chartTurmasPorStatus" 
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
            <AxisY Title="Número de turmas">
                <LabelStyle Font="Verdana, 8.25pt" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisY>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
