using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Proiect_Pantofi_Multimedia
{
    public partial class PaginaPrincipala : System.Web.UI.Page
    {
        OracleConnection con;

        //CONEXIUNE LA BAZA DE DATE ************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            string cons = "";
            con = new OracleConnection(cons);
        }

        //FUNCTII PROPRII ************************************************************
        protected void generare_semnatura() { 
            try { 
                con.Open(); 
                OracleCommand cmd = new OracleCommand("psgenerare_semn_pantofi", con); 
                cmd.CommandType = System.Data.CommandType.StoredProcedure; 
                cmd.ExecuteNonQuery(); 
                btn_gasire.BackColor = System.Drawing.Color.LightGreen; 
            } 
            catch (OracleException ex) {
                btn_gasire.BackColor = System.Drawing.Color.Red; 
            } 
            con.Close(); 
        }

        private List<int> GetPantofiFiltrati()
        {
            List<int> ids = new List<int>();

            StringBuilder query = new StringBuilder("SELECT id FROM pantofi WHERE 1=1 ");
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;

            foreach (System.Web.UI.WebControls.ListItem item in chk_filters.Items)
            {
                if (item.Selected)
                {
                    if (item.Value == "denumire" && !string.IsNullOrEmpty(tb_denumire.Text))
                    {
                        query.Append("AND LOWER(denumire) LIKE :den ");
                        cmd.Parameters.Add(new OracleParameter(":den", "%" + tb_denumire.Text.ToLower() + "%"));
                    }
                    else if (item.Value == "categorie")
                    {
                        query.Append("AND categorie = :cat ");
                        cmd.Parameters.Add(new OracleParameter(":cat", ddl_categorie.Text));
                    }
                    else if (item.Value == "culoare")
                    {
                        query.Append("AND culoare = :col ");
                        cmd.Parameters.Add(new OracleParameter(":col", ddl_culoare.Text));
                    }
                }
            }

            query.Append("ORDER BY id");
            cmd.CommandText = query.ToString();

            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ids.Add(Convert.ToInt32(dr["id"]));
            }
            return ids;
        }

        protected void afisare_img_dupa_id(int id)
        {
            OracleCommand cmd = new OracleCommand("psafisarepantofi", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("vid", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("flux", OracleDbType.Blob).Direction = ParameterDirection.Output;

            cmd.ExecuteScalar();
            Byte[] blob = new Byte[((OracleBlob)cmd.Parameters["flux"].Value).Length];
            ((OracleBlob)cmd.Parameters["flux"].Value).Read(blob, 0, blob.Length);
            string myimg = Convert.ToBase64String(blob);

            Img_afisare.ImageUrl = "data:image/jpg;base64," + myimg;

            // Detalii pantof
            OracleCommand detaliiCmd = new OracleCommand("SELECT denumire, culoare, categorie, pret FROM pantofi WHERE id = :id", con);
            detaliiCmd.Parameters.Add(new OracleParameter(":id", id));
            OracleDataReader dr = detaliiCmd.ExecuteReader();
            if (dr.Read())
            {
                tb_idImg.Text = id.ToString();
                lbl_denumire.Text = "Denumire: " + dr["denumire"].ToString();
                lbl_culoare.Text = "Culoare: " + dr["culoare"].ToString();
                lbl_categorie.Text = "Categorie: " + dr["categorie"].ToString();
                lbl_pret.Text = "Pret: " + dr["pret"].ToString() + " lei";
            }
        }

        //AFISARE ************************************************************
        protected void btn_afisareImg_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                afisare_img_dupa_id(Convert.ToInt32(tb_idImg.Text));
            }
            catch (Exception ex)
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.Red;
                tb_status.Text = "Eroare la afisare: " + ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        //FILTRARE ************************************************************
        protected void btn_filtrare_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                var ids = GetPantofiFiltrati();

                if (ids.Count > 0)
                {
                    Session["PantofiList"] = ids;
                    Session["PantofiIndex"] = 0;

                    afisare_img_dupa_id(ids[0]);

                    tb_status.Visible = true;
                    tb_status.BackColor = System.Drawing.Color.LightGreen;
                    tb_status.Text = $"S-au gasit {ids.Count} pantofi dupa criteriile selectate.";
                }
                else
                {
                    tb_status.Visible = true;
                    tb_status.BackColor = System.Drawing.Color.LightCoral;
                    tb_status.Text = "Nu s-au gasit pantofi care sa corespunda filtrului.";
                }
            }
            catch (Exception ex)
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.Red;
                tb_status.Text = "Eroare la filtrare: " + ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        protected void btn_next_Click(object sender, EventArgs e)
        {
            if (Session["PantofiList"] != null)
            {
                var ids = (List<int>)Session["PantofiList"];
                int index = (int)Session["PantofiIndex"];

                if (index < ids.Count - 1)
                {
                    index++;
                    Session["PantofiIndex"] = index;

                    con.Open();
                    afisare_img_dupa_id(ids[index]);
                    con.Close();
                }
            }
        }

        protected void btn_prev_Click(object sender, EventArgs e)
        {
            if (Session["PantofiList"] != null)
            {
                var ids = (List<int>)Session["PantofiList"];
                int index = (int)Session["PantofiIndex"];

                if (index > 0)
                {
                    index--;
                    Session["PantofiIndex"] = index;

                    con.Open();
                    afisare_img_dupa_id(ids[index]);
                    con.Close();
                }
            }
        }

        //ADAUGARE IN BD ************************************************************
        protected void btn_addImg_Click(object sender, EventArgs e)
        {
            tb_status.Text = "";
            tb_status.Visible = false;

            if (!uploader_img.HasFile)
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.LightCoral;
                tb_status.Text = "Va rugam sa selectati o imagine.";
                return;
            }

            string filePath = @"C:\Users\Mihai\Desktop\Scoala\Master\baze de date multimedia\GalerieProiectPantofi\" + uploader_img.FileName;

            try
            {
                uploader_img.SaveAs(filePath);

                byte[] imageBytes;
                using (var img = System.IO.File.OpenRead(filePath))
                {
                    imageBytes = new byte[img.Length];
                    img.Read(imageBytes, 0, (int)img.Length);
                }

                con.Open();
                using (OracleCommand cmd = new OracleCommand("psInserarePantof", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vcategorie", OracleDbType.Varchar2).Value = ddl_categorie.Text;
                    cmd.Parameters.Add("vdenumire", OracleDbType.Varchar2).Value = tb_denumire.Text;
                    cmd.Parameters.Add("vculoare", OracleDbType.Varchar2).Value = ddl_culoare.Text;
                    cmd.Parameters.Add("vpret", OracleDbType.Decimal).Value = Convert.ToDecimal(tb_pret.Text);
                    cmd.Parameters.Add("fis", OracleDbType.Blob).Value = imageBytes;

                    cmd.ExecuteNonQuery();
                }

                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.LightGreen;
                tb_status.Text = "Imaginea a fost inserata cu succes!";

                try
                {
                    // Selectarea id-ului imaginii inserate
                    using (OracleCommand cmdLastId = new OracleCommand("SELECT MAX(id) FROM pantofi", con))
                    {
                        int newId = Convert.ToInt32(cmdLastId.ExecuteScalar());

                        // Generare semnatura pentru imaginea inserata
                        using (OracleCommand cmdGen = new OracleCommand("psgenerare_semn_pantofi_id", con))
                        {
                            cmdGen.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdGen.Parameters.Add("vid", OracleDbType.Int32).Value = newId;
                            cmdGen.ExecuteNonQuery();
                        }
                    }

                    btn_gasire.BackColor = System.Drawing.Color.LightGreen;
                }
                catch (Exception)
                {
                    btn_gasire.BackColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.Red;
                tb_status.Text = "A aparut o eroare la inserare! " + ex.Message;
            }
            finally
            {
                con.Close();
            }
        }


        //AFISARE IMG SIMILARA ************************************************************
        protected void btn_gasire_Click(object sender, EventArgs e)
        {
            if (uploader_gasire_img.HasFile)
            {
                try
                {
                    con.Open();

                    byte[] imageBytes = uploader_gasire_img.FileBytes;
                    OracleCommand cmd = new OracleCommand("psregasire_pantofi", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("fis", OracleDbType.Blob).Value = imageBytes;
                    cmd.Parameters.Add("cculoare", OracleDbType.Decimal).Value = Convert.ToDecimal(0.2);
                    cmd.Parameters.Add("ctextura", OracleDbType.Decimal).Value = Convert.ToDecimal(0.2);
                    cmd.Parameters.Add("cforma", OracleDbType.Decimal).Value = Convert.ToDecimal(0.9);
                    cmd.Parameters.Add("clocatie", OracleDbType.Decimal).Value = Convert.ToDecimal(0.05);
                    cmd.Parameters.Add("idrez", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int rezultatId = Convert.ToInt32(cmd.Parameters["idrez"].Value.ToString());
                    tb_status.Visible = true;
                    tb_status.BackColor = System.Drawing.Color.LightGreen;
                    tb_status.Text = "Imaginea asemanatoare are ID: " + rezultatId;

                    afisare_img_dupa_id(rezultatId);
                }
                catch (Exception ex)
                {
                    tb_status.Visible = true;
                    tb_status.BackColor = System.Drawing.Color.Red;
                    tb_status.Text = "Eroare la regasire: " + ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        protected void cb_addImgBool_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_addImgBool.Checked)
            {
                LabelPret.Visible = true;
                tb_pret.Visible = true;
                Label5.Visible = true;
                uploader_img.Visible = true;
                btn_addImg.Visible = true;
            }
            else
            {
                LabelPret.Visible = false;
                tb_pret.Visible = false;
                Label5.Visible = false;
                uploader_img.Visible = false;
                btn_addImg.Visible = false;
            }
        }

        protected void btn_fav_Click(object sender, EventArgs e)
        {
            string currentId = tb_idImg.Text;

            if (string.IsNullOrEmpty(currentId))
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.LightCoral;
                tb_status.Text = "Nu exista nicio imagine afisata pentru a adauga la favorite.";
                return;
            }

            List<int> favorites = (List<int>)Session["Favorite"];
            if (favorites == null)
            {
                favorites = new List<int>();
            }

            int imageId = Convert.ToInt32(currentId);

            if (favorites.Contains(imageId))
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.LightYellow;
                tb_status.Text = "Aceasta imagine este deja in lista de favorite!";
                return;
            }

            // Add to favorites
            favorites.Add(imageId);
            Session["Favorite"] = favorites;

            tb_status.Visible = true;
            tb_status.BackColor = System.Drawing.Color.LightGreen;
            tb_status.Text = $"Imaginea cu ID {imageId} a fost adaugata la favorite! Total: {favorites.Count}";

        }

        protected void cb_addVidBool_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_addVidBool.Checked)
            {
                lb_desc_vid.Visible = true;
                tb_descriere_vid.Visible = true;
                fu_vid.Visible = true;
                btn_add_vid.Visible = true;
            }
            else
            {
                lb_desc_vid.Visible = false;
                tb_descriere_vid.Visible = false;
                fu_vid.Visible = false;
                btn_add_vid.Visible = false;
            }
        }

        protected void btn_add_vid_Click(object sender, EventArgs e)
        {
            tb_status.Text = "";
            tb_status.Visible = false;

            if (!fu_vid.HasFile)
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.LightCoral;
                tb_status.Text = "Va rugam sa selectati un video.";
                return;
            }

            try
            {
                con.Open();

                byte[] videoBytes;
                using (var stream = fu_vid.PostedFile.InputStream)
                {
                    videoBytes = new byte[fu_vid.PostedFile.ContentLength];
                    stream.Read(videoBytes, 0, videoBytes.Length);
                }

                using (OracleCommand cmd = new OracleCommand("psInserareVideo", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("vdescriere", OracleDbType.Varchar2).Value = tb_descriere_vid.Text;
                    cmd.Parameters.Add("fis", OracleDbType.Blob).Value = videoBytes;

                    cmd.ExecuteNonQuery();
                }

                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.LightGreen;
                tb_status.Text = "Video-ul a fost inserat cu succes!";
            }
            catch(Exception ex)
            {
                tb_status.Visible = true;
                tb_status.BackColor = System.Drawing.Color.Red;
                tb_status.Text = "Eroare la inserare video: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

        }

        protected void btn_afisareVideo_Click(object sender, EventArgs e)
        {
            try
            {
                int videoId;
                if (!int.TryParse(tb_idVideo.Text, out videoId))
                {
                    return;
                }

                con.Open();

                OracleCommand cmd = new OracleCommand("psafisarevideo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("vid", OracleDbType.Int32).Value = videoId;
                cmd.Parameters.Add("flux", OracleDbType.Blob).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                OracleBlob videoBlob = (OracleBlob)cmd.Parameters["flux"].Value;
                byte[] videoBytes = new byte[videoBlob.Length];
                videoBlob.Read(videoBytes, 0, (int)videoBlob.Length);

                string base64Video = Convert.ToBase64String(videoBytes);
                string videoSrc = "data:video/mp4;base64," + base64Video;

                string videoHtml = $@"
            <video width='640' height='480' controls>
                <source src='{videoSrc}' type='video/mp4'>
                Your browser does not support the video tag.
            </video>";

                videoContainer.Controls.Clear();
                videoContainer.Controls.Add(new LiteralControl(videoHtml));

                con.Close();
            }
            catch (Exception ex)
            {
                tb_status.Visible = true;
                tb_status.Text = "Error: " + ex.Message;
            }
        }


    }
}
