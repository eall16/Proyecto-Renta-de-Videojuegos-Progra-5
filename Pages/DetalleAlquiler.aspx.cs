using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Renta_de_Videojuegos.Pages
{
    public partial class DetalleAlquiler : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblIdAlquiler;
        protected global::System.Web.UI.WebControls.Label lblSucursal;
        protected global::System.Web.UI.WebControls.Label lblVideojuego;
        protected global::System.Web.UI.WebControls.Label lblJugador;
        protected global::System.Web.UI.WebControls.Label lblFechaInicio;
        protected global::System.Web.UI.WebControls.Label lblFechaDevolucion;
        protected global::System.Web.UI.WebControls.Label lblDias;
        protected global::System.Web.UI.WebControls.Label lblCostoTotal;
        protected global::System.Web.UI.WebControls.Label lblEstado;
        protected global::System.Web.UI.WebControls.GridView grdBitacora;
        protected global::System.Web.UI.WebControls.HyperLink lnkEditar;
        protected global::System.Web.UI.WebControls.Button btnCancelar;
        protected global::System.Web.UI.WebControls.HyperLink lnkRegresar;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdJugador"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx", true);
                return;
            }

            ((SiteMaster)Master).EstablecerTituloSeccion("Detalle de alquiler");

            int idAlquiler;
            if (!int.TryParse(Request.QueryString["id"], out idAlquiler))
            {
                RedirigirALista();
                return;
            }

            bool esAdministrador = Convert.ToBoolean(Session["EsAdministrador"]);
            int idJugadorSesion = Convert.ToInt32(Session["IdJugador"]);

            try
            {
                using (RentaVideojuegosDB db = new RentaVideojuegosDB("MyDatabase"))
                {
                    RentaVideojuegosDBStoredProcedures.SpObtenerDetalleAlquilerResult d =
                        db.SpObtenerDetalleAlquiler(idAlquiler).FirstOrDefault();

                    if (d == null)
                    {
                        RedirigirALista();
                        return;
                    }

                    // Un jugador regular no puede ver el detalle de un alquiler
                    // que no es suyo, aunque manipule la URL.
                    if (!esAdministrador && d.IdJugador != idJugadorSesion)
                    {
                        RedirigirALista();
                        return;
                    }

                    MostrarDetalle(d, esAdministrador);
                    ConfigurarBotones(d, esAdministrador);

                    grdBitacora.DataSource = db.SpObtenerBitacoraPorAlquiler(idAlquiler).ToList();
                    grdBitacora.DataBind();
                }
            }
            catch (Exception exc)
            {
                Response.Write("<script>alert('Error: " + exc.Message + "');</script>");
            }
        }

        private void MostrarDetalle(RentaVideojuegosDBStoredProcedures.SpObtenerDetalleAlquilerResult d, bool esAdministrador)
        {
            lblIdAlquiler.Text = d.IdAlquiler.ToString();
            lblSucursal.Text = d.NombreSucursal;
            lblVideojuego.Text = d.Titulo;
            lblJugador.Text = d.NombreCompleto;
            lblFechaInicio.Text = d.FechaInicio.ToString("dd/MM/yyyy");
            lblFechaDevolucion.Text = d.FechaDevolucion.ToString("dd/MM/yyyy");
            lblDias.Text = d.TotalDiasAlquiler.ToString();
            lblCostoTotal.Text = d.CostoTotal.ToString("C");
            lblEstado.Text = ObtenerEstadoTexto(d.Estado, d.FechaInicio, d.FechaDevolucion);

            lnkRegresar.NavigateUrl = esAdministrador ? "~/Pages/GestionarAlquileres.aspx" : "~/Pages/MisAlquileres.aspx";
        }

        private void ConfigurarBotones(RentaVideojuegosDBStoredProcedures.SpObtenerDetalleAlquilerResult d, bool esAdministrador)
        {
            DateTime ahora = DateTime.Now;
            bool estaActivo = d.Estado == "A";

            // RF-002: reglas del boton Editar (distintas para jugador y administrador).
            bool puedeEditar = esAdministrador
                ? estaActivo && d.FechaDevolucion > ahora
                : estaActivo && d.FechaInicio > ahora;

            lnkEditar.Visible = puedeEditar;
            lnkEditar.NavigateUrl = "~/Pages/ModificarAlquiler.aspx?id=" + d.IdAlquiler;

            // RF-002: regla del boton Cancelar (igual para ambos roles).
            btnCancelar.Visible = estaActivo && d.FechaInicio > ahora;
            ViewState["IdAlquiler"] = d.IdAlquiler;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            int idAlquiler = Convert.ToInt32(ViewState["IdAlquiler"]);
            int idJugadorSesion = Convert.ToInt32(Session["IdJugador"]);

            try
            {
                using (RentaVideojuegosDB db = new RentaVideojuegosDB("MyDatabase"))
                {
                    db.SpCancelarAlquiler(idAlquiler, idJugadorSesion);
                }
            }
            catch (Exception)
            {
                // Si el SP rechaza la cancelacion, simplemente se recarga el
                // detalle; el boton ya no debería estar visible en ese caso.
            }

            Response.Redirect(Request.RawUrl, true);
        }

        private string ObtenerEstadoTexto(string estadoBD, DateTime fechaInicio, DateTime fechaDevolucion)
        {
            DateTime ahora = DateTime.Now;

            if (estadoBD == "I") return "Cancelado";
            if (fechaDevolucion < ahora) return "Finalizado";
            if (fechaInicio <= ahora) return "En proceso";
            return "En espera";
        }

        private void RedirigirALista()
        {
            bool esAdministrador = Convert.ToBoolean(Session["EsAdministrador"]);
            Response.Redirect(esAdministrador ? "~/Pages/GestionarAlquileres.aspx" : "~/Pages/MisAlquileres.aspx", true);
        }
    }
}