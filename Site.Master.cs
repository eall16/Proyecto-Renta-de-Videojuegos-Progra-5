using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Renta_de_Videojuegos
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdJugador"] == null)
            {
                lnkMisAlquileres.Visible = false;
                lnkGestionarAlquileres.Visible = false;
                lnkGestionarVideojuegos.Visible = false;
                lnkCerrarSesion.Visible = false;
                return;
            }

            lblNombreUsuario.Text = "Hola, " + Session["NombreCompleto"].ToString();

            bool esAdministrador = Convert.ToBoolean(Session["EsAdministrador"]);
            lnkGestionarAlquileres.Visible = esAdministrador;
            lnkGestionarVideojuegos.Visible = esAdministrador;
        }

        public void EstablecerTituloSeccion(string titulo)
        {
            lblTituloSeccion.Text = titulo;
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Pages/Login.aspx", true);
        }
    }
}