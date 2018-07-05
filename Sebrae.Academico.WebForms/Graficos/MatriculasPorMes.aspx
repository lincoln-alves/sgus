<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MatriculasPorMes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.MatriculasPorMes" %>

<asp:Chart ID="chartMatriculasMes" 
    runat="server" 
    BackColor="White" 
    Height="350px" 
    Width="650px"
    BorderColor="26, 59, 105" 
    BorderWidth="2"
    RenderType="BinaryStreaming"
    Palette="Pastel"
    CssClass="force img-responsive"
    >
    <Series>
        <asp:Series XValueType="String" Name="QuantidadeOnline" Legend="legend1"></asp:Series>
        <asp:Series XValueType="String" Name="QuantidadeCompany" Legend="legend1"></asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisY LineColor="64, 64, 64, 64">
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisY>
            <AxisX LineColor="64, 64, 64, 64">
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisX>
        </asp:ChartArea>
    </ChartAreas>
    <Legends>
         <asp:Legend Name="legend1" ></asp:Legend>
    </Legends>
</asp:Chart>
