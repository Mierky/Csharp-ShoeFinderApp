<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaFavPantofi.aspx.cs" Inherits="Proiect_Pantofi_Multimedia.ListaFavPantofi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista Favorite - Pantofi</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
        }
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        .header {
            text-align: center;
            background-color: #66CCFF;
            padding: 20px;
            margin-bottom: 30px;
            border-radius: 8px;
        }
        .favorites-table {
            width: 100%;
            border-collapse: collapse;
            background-color: white;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .favorites-table th {
            background-color: #66CCFF;
            color: white;
            padding: 15px;
            text-align: left;
            font-weight: bold;
            border-bottom: 2px solid #4db8ff;
        }
        .favorites-table td {
            padding: 15px;
            border-bottom: 1px solid #e0e0e0;
            vertical-align: middle;
        }
        .favorites-table tr:hover {
            background-color: #f9f9f9;
        }
        .favorites-table img {
            border-radius: 4px;
            border: 2px solid #ddd;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Lista de Favorite</h1>
            </div>

            <asp:Label ID="lblNumere" runat="server" Font-Size="Large" Font-Bold="True"></asp:Label>
            <br /><br />

            <asp:Table ID="tblFavorites" runat="server" CssClass="favorites-table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Imagine</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Denumire</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Culoare</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Pret</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>

            <br /><br />

            <div style="text-align: center;">
                <asp:Button ID="btnClearAll" runat="server" 
                            Text="Goleste Lista" 
                            OnClick="btnClearAll_Click"
                            BackColor="#FF6B6B"
                            ForeColor="White"
                            Font-Bold="True"
                            Height="40px"
                            Width="200px"
                            BorderStyle="None"
                            Style="border-radius: 4px; cursor: pointer;" />
            </div>
        </div>
    </form>
</body>
</html>
