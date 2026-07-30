using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Renta_de_Videojuegos.Pages
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdJugador"] != null)
            {
                bool esAdministrador = Convert.ToBoolean(Session["EsAdministrador"]);
                Response.Redirect(esAdministrador ? "~/Pages/GestionarAlquileres.aspx" : "~/Pages/MisAlquileres.aspx", true);
            }
            else
            {
                Response.Redirect("~/Pages/Login.aspx", true);
            }
        }
    }
}