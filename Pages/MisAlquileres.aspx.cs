using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Renta_de_Videojuegos.Pages
{
    public partial class MisAlquileres : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.HyperLink lnkNuevoAlquiler;
        protected global::System.Web.UI.WebControls.GridView grdAlquileres;

        protected void Page_Load(object sender, EventArgs e)
        {
            // RF-001-d: todas las paginas (menos el login) exigen sesion activa.
            if (Session["IdJugador"] == null)
            {
                Response.Redirect("~/Pages/Login.aspx", true);
                return;
            }

            ((SiteMaster)Master).EstablecerTituloSeccion("Mis alquileres");

            if (!IsPostBack)
            {
                CargarAlquileres();
            }
        }

        private void CargarAlquileres()
        {
            int idJugador = Convert.ToInt32(Session["IdJugador"]);

            try
            {
                using (RentaVideojuegosDB db = new RentaVideojuegosDB("MyDatabase"))
                {
                    grdAlquileres.DataSource = db.SpObtenerAlquileresPorJugador(idJugador).ToList();
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
    }
}