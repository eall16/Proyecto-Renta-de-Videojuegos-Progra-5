<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisAlquileres.aspx.cs" Inherits="Renta_de_Videojuegos.Pages.MisAlquileres" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <p>
        <asp:HyperLink ID="lnkNuevoAlquiler" runat="server" NavigateUrl="~/Pages/CrearAlquiler.aspx" CssClass="btn btn-primary btn-sm">+ Nuevo alquiler</asp:HyperLink>
    </p>

    <asp:GridView ID="grdAlquileres" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped tabla-datos"
        EmptyDataText="Todavía no tienes alquileres registrados.">
        <Columns>
            <asp:BoundField DataField="IdAlquiler" HeaderText="# Alquiler" />
            <asp:BoundField DataField="NombreSucursal" HeaderText="Sucursal" />
            <asp:BoundField DataField="Titulo" HeaderText="Videojuego" />
            <asp:BoundField DataField="FechaInicio" HeaderText="Fecha inicio" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="FechaDevolucion" HeaderText="Fecha devolución" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="CostoTotal" HeaderText="Costo total" DataFormatString="{0:C}" />
            <asp:TemplateField HeaderText="Estado">
                <ItemTemplate><%# ObtenerEstadoTexto((string)Eval("Estado"), (DateTime)Eval("FechaInicio"), (DateTime)Eval("FechaDevolucion")) %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:HyperLink runat="server" CssClass="btn btn-sm btn-outline-info" NavigateUrl='<%# "~/Pages/DetalleAlquiler.aspx?id=" + Eval("IdAlquiler") %>'>Ver detalle</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
