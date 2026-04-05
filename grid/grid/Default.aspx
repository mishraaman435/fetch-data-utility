<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="grid._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="row">
            <asp:DropDownList ID="dropdown1" runat="server">
                <asp:ListItem Text="Select Your Name" Value="" Selected="True" Disabled="True" Hidden="True"></asp:ListItem>
                <asp:ListItem Text="Aman" Value="Aman"></asp:ListItem>
                <asp:ListItem Text="madhur" Value="Madhur"></asp:ListItem>
                <asp:ListItem Text="Priyanshu" Value="Priyanshu"></asp:ListItem>
                <asp:ListItem Text="Diksha" Value="Diksha"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <div class="row">
            <asp:Label ID="xyz" Text="Select Gender" runat="server"></asp:Label>
            <div>
                <asp:RadioButton ID="RadioButton1" runat="server" Text="Male" GroupName="gender" />
                <asp:RadioButton ID="RadioButton2" runat="server" Text="Female" GroupName="gender" />
            </div>
        </div>
        <br />
        <div>
            <asp:TextBox ID="textwa" runat="server" ToolTip="Write Something"></asp:TextBox>
        </div>
        <br />
        <asp:Button ID="submit" runat="server" OnClick="submit_Click" BackColor="burlywood" Text="Push" />
        <br />
        <div>
            <p>This DataGrid contains DataTable records </p>
            <asp:DataGrid ID="DataGrid1" runat="server" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" Width="608px">
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <ItemStyle BackColor="White" />
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" Mode="NumericPages" />
                <SelectedItemStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            </asp:DataGrid>
        </div>
    </main>
</asp:Content>
