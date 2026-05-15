using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proiect_Pantofi_Multimedia
{
    public partial class ListaFavPantofi : System.Web.UI.Page
    {
        OracleConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cons = "";
            con = new OracleConnection(cons);

            LoadFavorites();
        }

        protected void LoadFavorites()
        {
            List<int> favorites = (List<int>)Session["Favorite"];

            if (favorites == null || favorites.Count == 0)
            {
                lblNumere.Text = "Nu exista produse in lista de favorite!";
                tblFavorites.Visible = false;
                return;
            }

            lblNumere.Text = $"Ai {favorites.Count} produse in lista de favorite:";
            tblFavorites.Visible = true;

            try
            {
                con.Open();

                foreach (int id in favorites)
                {
                    AddProductRow(id);
                }
            }
            catch (Exception ex)
            {
                lblNumere.Text = "Eroare la incarcare: " + ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        protected void AddProductRow(int id)
        {
            OracleCommand cmd = new OracleCommand("SELECT denumire, culoare, pret FROM pantofi WHERE id = :id", con);
            cmd.Parameters.Add(new OracleParameter(":id", id));
            OracleDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                string denumire = dr["denumire"].ToString();
                string culoare = dr["culoare"].ToString();
                string pret = dr["pret"].ToString();

                // Get image
                OracleCommand imgCmd = new OracleCommand("psafisarepantofi", con);
                imgCmd.CommandType = CommandType.StoredProcedure;
                imgCmd.Parameters.Add("vid", OracleDbType.Int32).Value = id;
                imgCmd.Parameters.Add("flux", OracleDbType.Blob).Direction = ParameterDirection.Output;

                imgCmd.ExecuteNonQuery();

                Byte[] blob = new Byte[((OracleBlob)imgCmd.Parameters["flux"].Value).Length];
                ((OracleBlob)imgCmd.Parameters["flux"].Value).Read(blob, 0, blob.Length);
                string base64Img = Convert.ToBase64String(blob);

                // TABEL
                TableRow row = new TableRow();

                // Imagine 
                TableCell imgCell = new TableCell();
                Image img = new Image();
                img.ImageUrl = "data:image/jpg;base64," + base64Img;
                img.Width = Unit.Pixel(100);
                img.Height = Unit.Pixel(100);
                img.Style.Add("object-fit", "cover");
                imgCell.Controls.Add(img);
                row.Cells.Add(imgCell);

                // Denumire 
                TableCell denumireCell = new TableCell();
                denumireCell.Text = denumire;
                denumireCell.Font.Bold = true;
                row.Cells.Add(denumireCell);

                // Culoare 
                TableCell culoareCell = new TableCell();
                culoareCell.Text = culoare;
                row.Cells.Add(culoareCell);

                // Pret 
                TableCell pretCell = new TableCell();
                pretCell.Text = pret + " lei";
                pretCell.Font.Bold = true;
                pretCell.ForeColor = System.Drawing.Color.DarkBlue;
                row.Cells.Add(pretCell);

                tblFavorites.Rows.Add(row);
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            Session["Favorite"] = null;
            LoadFavorites();
        }
    }
}