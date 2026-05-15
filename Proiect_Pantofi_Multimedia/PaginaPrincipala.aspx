<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaginaPrincipala.aspx.cs" Inherits="Proiect_Pantofi_Multimedia.PaginaPrincipala" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pantofi - Galerie</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.6.0/css/all.min.css">
</head>
<body style="margin:0; padding:0; display:flex; justify-content:center; align-items:center; min-height:100vh; background-color:#f0f0f0;">
    <form id="form1" runat="server" enctype="multipart/form-data" style="width: 1200px; background-color: #ffffff; padding: 20px; box-shadow: 0 0 10px rgba(0,0,0,0.2);">
        <!-- Header -->
        <div style="text-align: center;">
            <asp:Label ID="Label2" runat="server" BackColor="#66CCFF" BorderStyle="Outset" Font-Bold="True" Font-Size="XX-Large" Height="45px" Text="PANTOFI" Width="100%"></asp:Label>

            <br /><br />
            <asp:TextBox ID="tb_status" runat="server" BackColor="#CCCCCC" Font-Bold="True" Height="30px" Visible="False" Width="449px" ReadOnly="True"></asp:TextBox>
            <br /><br />
        </div>

        <!-- Search / Display -->
        <div style="text-align: center;">
            <asp:Label ID="Label7" runat="server" BackColor="#66CCFF" BorderStyle="Dashed" BorderWidth="2px" Font-Bold="True" Font-Italic="True" Text="CAUTARE / AFISARE" Width="400px"></asp:Label>
            <br /><br />

            <asp:Label ID="Label6" runat="server" Text="ID Imagine:"></asp:Label>
            &nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tb_idImg" runat="server" Width="67px"></asp:TextBox>
            <br /><br />

            <asp:Button ID="btn_afisareImg" runat="server" BackColor="#CCFFFF" BorderStyle="Groove" Font-Bold="True" Height="46px" OnClick="btn_afisareImg_Click" Text="Afisare Imagine dupa ID" Width="400px" />
            <br /><br />
        </div>

        <!-- Main container: 3 columns -->
        <div style="display: flex; justify-content: center; align-items: flex-start; gap: 30px; width: 100%;">
            
            <!-- LEFT column: Form inputs -->
            <div style="text-align: center; width: 350px; flex-shrink: 0; margin-left:-3em;">
                <asp:Label ID="Label1" runat="server" Text="Categorie"></asp:Label>
                <br />
                <asp:DropDownList ID="ddl_categorie" runat="server" Width="130px">
                    <asp:ListItem>Sport</asp:ListItem>
                    <asp:ListItem>Bocanci</asp:ListItem>
                    <asp:ListItem>Sneakers</asp:ListItem>
                </asp:DropDownList>
                <br /><br />

                <asp:Label ID="Label3" runat="server" Text="Denumire"></asp:Label>
                <br />
                <asp:TextBox ID="tb_denumire" runat="server" Height="25px" Width="220px"></asp:TextBox>
                <br /><br />

                <asp:Label ID="Label4" runat="server" Text="Culoare"></asp:Label>
                <br />
                <asp:DropDownList ID="ddl_culoare" runat="server" Width="130px">
                    <asp:ListItem>Negru</asp:ListItem>
                    <asp:ListItem>Alb</asp:ListItem>
                    <asp:ListItem>Rosu</asp:ListItem>
                    <asp:ListItem>Albastru</asp:ListItem>
                    <asp:ListItem>Galben</asp:ListItem>
                    <asp:ListItem>Gri</asp:ListItem>
                    <asp:ListItem>Verde</asp:ListItem>
                    <asp:ListItem>Roz</asp:ListItem>
                    <asp:ListItem>Mov</asp:ListItem>
                    <asp:ListItem>Maro</asp:ListItem>
                    <asp:ListItem>Bej</asp:ListItem>
                    <asp:ListItem>Portocaliu</asp:ListItem>
                </asp:DropDownList>
                <br /><br />

                <asp:Label ID="LabelPret" runat="server" Text="Pret (lei)" visible="false"></asp:Label>
                <br />
                <asp:TextBox ID="tb_pret" runat="server" Height="25px" Width="120px" visible="false"></asp:TextBox>
                <br /><br />

                <asp:Label ID="Label5" runat="server" Text="Imagine" visible="false"></asp:Label>
                <br />
                <asp:FileUpload ID="uploader_img" runat="server" visible="false"/>
                <br /><br />

                <asp:Button ID="btn_addImg" runat="server" visible="false" BackColor="#CCFFFF" BorderStyle="Groove" Font-Bold="True" Height="43px" OnClick="btn_addImg_Click" Text="Adauga Imagine" Width="169px" />
                <br />
                <asp:CheckBox ID="cb_addImgBool" runat="server" AutoPostBack="true" Text="Mod adaugare" OnCheckedChanged="cb_addImgBool_CheckedChanged" />
            </div>

            <!-- CENTER column: Image -->
            <div style="width: 360px; flex-shrink: 0; text-align: center;">
                <asp:Image ID="Img_afisare" runat="server" BorderStyle="Solid" BorderWidth="2px" Height="400px" Width="360px" />
            </div>

            <!-- RIGHT column: Details -->
            <div style="text-align: left; width: 300px; flex-shrink: 0;">
                <br /><br /><br /><br /><br />
                <asp:Label ID="lbl_denumire" runat="server" Font-Bold="True"></asp:Label><br />
                <asp:Label ID="lbl_culoare" runat="server"></asp:Label><br />
                <asp:Label ID="lbl_categorie" runat="server"></asp:Label><br />
                <asp:Label ID="lbl_pret" runat="server" ForeColor="DarkBlue"></asp:Label>
                <br /><br />

                <div style="text-align: left;">
                    <asp:Button ID="btn_prev" runat="server" Text="&lt; Prev" Width="100px" Height="35px" BackColor="#FFFFCC" Font-Bold="True" OnClick="btn_prev_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_next" runat="server" Text="Next &gt;" Width="100px" Height="35px" BackColor="#FFFFCC" Font-Bold="True" OnClick="btn_next_Click" />
                </div>
                <br />

                <asp:Label ID="LabelFilt" runat="server" Font-Bold="True" Text="Selecteaza criteriile de filtrare:"></asp:Label>
                <br />
                <asp:CheckBoxList ID="chk_filters" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="denumire">Denumire</asp:ListItem>
                    <asp:ListItem Value="categorie">Categorie</asp:ListItem>
                    <asp:ListItem Value="culoare">Culoare</asp:ListItem>
                </asp:CheckBoxList>
                <br />

                <asp:Button ID="btn_filtrare" runat="server" BackColor="#CCFFCC" BorderStyle="Groove" Font-Bold="True" Height="46px" Text="Filtrare" Width="250px" OnClick="btn_filtrare_Click" />
                <br /> <br />
                <asp:Button ID="btn_fav" runat="server" BackColor="#ED5565" BorderStyle="Groove" Font-Bold="True" Height="39px" Text="Adauga la favorite" Width="158px" OnClick="btn_fav_Click" />
                <a href="ListaFavPantofi.aspx" target="_blank" style="margin-left:5px;">
                    <i class="fa-solid fa-heart" style=" color:#000000; font-size:24px;"></i>
                </a>
            </div>
        </div>

        <br /><br />

        <div style="text-align: center;">
            <asp:Label ID="Label_gasire" runat="server" Font-Italic="True" Text="Gasire imagine asemanatoare"></asp:Label>
            <br /><br />
            <asp:FileUpload ID="uploader_gasire_img" runat="server" />
            <br /><br />
            <asp:Button ID="btn_gasire" runat="server" BackColor="#CCFFFF" BorderStyle="Groove" Font-Bold="True" Height="42px" Text="Gaseste" Width="169px" OnClick="btn_gasire_Click" />
            <br />
            <br />
                <asp:CheckBox ID="cb_addVidBool" runat="server" AutoPostBack="true" Text="Adaugare Video" OnCheckedChanged="cb_addVidBool_CheckedChanged" />
            <br />

                <asp:Label ID="lb_desc_vid" runat="server" Text="Descriere " Visible="False"></asp:Label>
                <asp:TextBox ID="tb_descriere_vid" runat="server" Height="25px" Width="220px" Visible="False"></asp:TextBox>
                <br />
            <asp:FileUpload ID="fu_vid" runat="server" Visible="False" />
            <br />
            <asp:Button ID="btn_add_vid" runat="server" OnClick="btn_add_vid_Click" Text="Add" Visible="False" />
            <br />
            <br />
            <div style="text-align: center;">
                <asp:Label ID="LabelVideo" runat="server" Text="ID Video:" Font-Bold="True"></asp:Label>
                &nbsp;&nbsp;
                <asp:TextBox ID="tb_idVideo" runat="server" Width="67px"></asp:TextBox>
                <br /><br />

                <asp:Button ID="btn_afisareVideo" runat="server" 
                            BackColor="#CCFFFF" 
                            BorderStyle="Groove" 
                            Font-Bold="True" 
                            Height="46px" 
                            OnClick="btn_afisareVideo_Click" 
                            Text="Afisare Video" 
                            Width="400px" />
                <br /><br />

                <!-- Video Container -->
                <asp:PlaceHolder ID="videoContainer" runat="server"></asp:PlaceHolder>
            </div>

        </div>
    </form>
</body>

</html>