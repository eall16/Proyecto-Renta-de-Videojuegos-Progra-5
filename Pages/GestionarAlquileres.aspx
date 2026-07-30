<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarAlquileres.aspx.cs" Inherits="Renta_de_Videojuegos.Pages.GestionarAlquileres" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <div>
        <label for="ddlJugador" class="form-label">Jugador</label>
        <asp:DropDownList ID="ddlJugador" runat="server" CssClass="form-select" DataTextField="NombreCompleto" DataValueField="IdJugador">
            <asp:ListItem Text="-- Todos los jugadores --" Value="" />
        </asp:DropDownList>

        <label for="txtFechaInicio" class="form-label">Fecha inicio</label>
        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>

        <label for="txtFechaDevolucion" class="form-label">Fecha devolución</label>
        <asp:TextBox ID="txtFechaDevolucion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>

        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />

        <asp:Label ID="lblMensajeError" runat="server" CssClass="mensaje-error" Visible="false" />
    </div>

    <p>
        <asp:HyperLink ID="lnkNuevoAlquiler" runat="server" NavigateUrl="~/Pages/CrearAlquiler.aspx" CssClass="btn btn-primary btn-sm">+ Nuevo alquiler</asp:HyperLink>
    </p>

    <asp:GridView ID="grdAlquileres" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped tabla-datos"
        EmptyDataText="No se encontraron alquileres con los criterios indicados.">
        <Columns>
            <asp:BoundField DataField="IdAlquiler" HeaderText="# Alquiler" />
            <asp:BoundField DataField="NombreCompleto" HeaderText="Jugador" />
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
