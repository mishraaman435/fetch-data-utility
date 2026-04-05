<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Mapview._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <asp:TextBox ID="lati" runat="server"></asp:TextBox>
        <asp:TextBox ID="longi" runat="server"></asp:TextBox>
        <asp:Button ID="subm" runat="server" Text="Show Location" OnClick="subm_Click" />
        <div id="map" style="width: 100%; height: 500px;"></div>
        <%--        <link rel="https//unpkg/leaflet@1.7.1/dist/leaflet.css" />--%>
        <%--        <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>--%>
        <link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" />
        <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>
        <script>
            var map = L.map('map').setView([22.719568, 75.857727], 8);
            L.tileLayer('https://{s}.tile.openStreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19 }).addTo(map);
            var marker;
            function showbutn(lat, lng){
                var loc = [lat, lng];
                if (marker) map.removeLayer(marker);
                else L.marker(loc).addTo(map).bindPopup(loc.toString()).openPopup();
                mao.setView(loc, 8);
            }
            var marker;

        </script>

    </main>

</asp:Content>
