using DataModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Renta_de_Videojuegos.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMensajeError;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
        protected global::System.Web.UI.WebControls.TextBox txtClave;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvClave;
        protected global::System.Web.UI.WebControls.Button btnIngresar;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["IdJugador"] != null)
            {
                RedirigirSegunRol();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim();
            string clave = txtClave.Text.Trim();

            try
            {
                using (RentaVideojuegosDB db = new RentaVideojuegosDB("MyDatabase"))
                {
                    RentaVideojuegosDBStoredProcedures.SpValidarCredencialesResult resultado =
                        db.SpValidarCredenciales(email, clave).FirstOrDefault();

                    if (resultado == null)
                    {
                        MostrarError("Las credenciales no son correctas.");
                        return;
                    }

                    // RF-001-c: crear las variables de sesion.
                    Session["IdJugador"] = resultado.IdJugador;
                    Session["NombreCompleto"] = resultado.NombreCompleto;
                    Session["EsAdministrador"] = resultado.EsAdministrador;
                }

                RedirigirSegunRol();
            }
            catch (Exception exc)
            {
                MostrarError("Ocurrió un error al iniciar sesión: " + exc.Message);
            }
        }

        private void RedirigirSegunRol()
        {
            bool esAdministrador = Convert.ToBoolean(Session["EsAdministrador"]);
            if (esAdministrador)
                Response.Redirect("~/Pages/GestionarAlquileres.aspx", true);
            else
                Response.Redirect("~/Pages/MisAlquileres.aspx", true);
        }

        private void MostrarError(string mensaje)
        {
            lblMensajeError.Text = mensaje;
            lblMensajeError.Visible = true;
        }
    }
}