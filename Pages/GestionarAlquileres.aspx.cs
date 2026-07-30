using DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Renta_de_Videojuegos.Pages
{
    public partial class GestionarAlquileres : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.DropDownList ddlJugador;
        protected global::System.Web.UI.WebControls.TextBox txtFechaInicio;
        protected global::System.Web.UI.WebControls.TextBox txtFechaDevolucion;
        protected global::System.Web.UI.WebControls.Button btnBuscar;
        protected global::System.Web.UI.WebControls.Label lblMensajeError;
        protected global::System.Web.UI.WebControls.HyperLink lnkNuevoAlquiler;
        protected global::System.Web.UI.WebControls.GridView grdAlquileres;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdJugador"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx", true);
                return;
            }

            if (!Convert.ToBoolean(Session["EsAdministrador"]))
            {
                Response.Redirect("~/Pages/MisAlquileres.aspx", true);
                return;
            }

            ((SiteMaster)Master).EstablecerTituloSeccion("Gestionar alquileres");

            if (!IsPostBack)
            {
                try
                {
                    using (RentaVideojuegosDB db = new RentaVideojuegosDB("MyDatabase"))
                    {
                        ddlJugador.DataSource = db.SpObtenerJugadoresActivos().ToList();
                        ddlJugador.DataBind();
                    }
                }
                catch (Exception exc)
                {
                    Response.Write("<script>alert('Error: " + exc.Message + "');</script>");
                }

                CargarAlquileres(null, null, null);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            int? idJugador = string.IsNullOrEmpty(ddlJugador.SelectedValue)
                ? (int?)null
                : Convert.ToInt32(ddlJugador.SelectedValue);

            bool tieneFechaInicio = !string.IsNullOrEmpty(txtFechaInicio.Text);
            bool tieneFechaDevolucion = !string.IsNullOrEmpty(txtFechaDevolucion.Text);

            if (tieneFechaInicio != tieneFechaDevolucion)
            {
                MostrarError("Debe indicar tanto la fecha de inicio como la fecha de devolución para filtrar por fechas.");
                CargarAlquileres(idJugador, null, null);
                return;
            }

            DateTime? fechaInicio = null;
            DateTime? fechaDevolucion = null;

            if (tieneFechaInicio && tieneFechaDevolucion)
            {
                fechaInicio = DateTime.ParseExact(txtFechaInicio.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                fechaDevolucion = DateTime.ParseExact(txtFechaDevolucion.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaDevolucion < fechaInicio)
                {
                    MostrarError("La fecha de devolución debe ser mayor o igual a la fecha de inicio.");
                    CargarAlquileres(idJugador, null, null);
                    return;
                }
            }

            CargarAlquileres(idJugador, fechaInicio, fechaDevolucion);
        }

        private void CargarAlquileres(int? idJugador, DateTime? fechaInicio, DateTime? fechaDevolucion)
        {
            try
            {
                using (RentaVideojuegosDB db = new RentaVideojuegosDB("MyDatabase"))
                {
                    grdAlquileres.DataSource = db.SpObtenerTodosAlquileres(idJugador, fechaInicio, fechaDevolucion).ToList();
                    grdAlquileres.DataBind();
                }
            }
            catch (Exception exc)
            {
                Response.Write("<script>alert('Error: " + exc.Message + "');</script>");
            }
        }

        protected string ObtenerEstadoTexto(string estadoBD, DateTime fechaInicio, DateTime fechaDevolucion)
        {
            DateTime ahora = DateTime.Now;

            if (estadoBD == "I") return "Cancelado";
            if (fechaDevolucion < ahora) return "Finalizado";
            if (fechaInicio <= ahora) return "En proceso";
            return "En espera";
        }

        private void MostrarError(string mensaje)
        {
            lblMensajeError.Text = mensaje;
            lblMensajeError.Visible = true;
        }
    }
}