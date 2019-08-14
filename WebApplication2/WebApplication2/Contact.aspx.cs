using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=WIN-664V9396VK\SQLEXPRESS; Initial Catalog=ASPCRUD;Integrated Security=true; ");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                FillGridView();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            // llamo a la función que acabo de crear
            Clear();
        }

        //creo la funcion limpiar - para limpiar el formulario
        // implementar más adelante en javascript
        public void Clear()
        {
            hfContactID.Value = "";
            txtName.Text = "";
            txtMobile.Text = "";
            txtAddress.Text = "";
            lblSuccessMessage.Text = "";
            lblErrorMessage.Text= "";
            btnSave.Text = "Guardar";
            btnDelete.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                //abro la conexión a la BD
                sqlCon.Open();
                // llamo al procedimiento almacenado de SQL server
                SqlCommand sqlCmd = new SqlCommand("ContactCreateOrUpdate", sqlCon);
                // indico que la sentencia sql es de un procedimient almacenado
                sqlCmd.CommandType = CommandType.StoredProcedure;
                //parametros (id,nombre, etc.)
                sqlCmd.Parameters.AddWithValue("@contactID", (hfContactID.Value==""?0:Convert.ToInt32(hfContactID.Value)));
                sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                //ejecuto la sentencia como no query, ya que no devuelve datos
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
                string contactID = hfContactID.Value;
                Clear();
                if (contactID=="")
                {
                    lblSuccessMessage.Text = "Guardado correctamente.";
                }
                else
                {
                    lblSuccessMessage.Text = "Actualizado.";
                    FillGridView();
                }
            }

        }

        void FillGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                //abro la conexión a la BD
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewAll", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                sqlCon.Close();
                gvContact.DataSource = dtbl;
                gvContact.DataBind();
            }
        }
        // creo este evento
        protected void lnk_OnClick(object sender, EventArgs e)
        {
            int contactID = Convert.ToInt32((sender as LinkButton).CommandArgument);

            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewByID", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@ContactId", contactID);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                sqlCon.Close();
                // rellenamos los textbox con los datos de BD del contacto en concreto
                hfContactID.Value = contactID.ToString();
                txtName.Text = dtbl.Rows[0]["Name"].ToString();
                txtMobile.Text = dtbl.Rows[0]["Mobile"].ToString();
                txtAddress.Text = dtbl.Rows[0]["Address"].ToString();
                // cambio el boton guardar por actualizar
                btnSave.Text = "Actualizar";
                btnDelete.Enabled = true;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("ContactDeleteByID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@ContactID", Convert.ToInt32(hfContactID.Value));
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
                Clear();
                lblSuccessMessage.Text = "Borrado correctamente";

            }
        }
    }
}