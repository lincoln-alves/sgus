<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MatriculasPorAnoPorFormaDeAquisicao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Graficos.MatriculasPorAnoPorFormaDeAquisicao" %>
<asp:Chart ID="chartMatriculasNoAnoPorFormaDeAquisicao" 
    runat="server" 
    BackColor="White" 
    Height="350px" 
    Width="450px"
    BorderColor="26, 59, 105" 
    BorderWidth="2"
    RenderType="BinaryStreaming"
    Palette="Pastel"
    CssClass="force img-responsive"
    >
    <%--Palette="None" PaletteCustomColors="97,142,206; 209,98,96; 168,203,104; 142,116,178; 93,186,215; 255,155,83; 148,172,215; 217,148,147; 189,213,151; 173,158,196; 145,201,221; 255,180,138"--%>
    <Series>
        <asp:Series XValueType="String" Name="Quantidade">
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
    