<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="ProfilePics.aspx.cs" Inherits="LarpPortal.Reports.ProfilePics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
    <style type="text/css">
        .PicStyle {
            max-width: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <asp:DataList ID="rptrImages" runat="server" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <img src='<%#Eval("PictureFileName")%>' class="PicStyle" /><br />
                        <%#Eval("CharacterFirstName")%> <%#Eval("CharacterLastName")%><br />
                    </ItemTemplate>
                </asp:DataList>
                <asp:GridView ID="gvImages" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="CharacterFirstName" HeaderText="Name" />
                        <asp:BoundField DataField="PictureFileName" />
                        <asp:ImageField DataImageUrlField="PictureFileName" HeaderText="Image" />
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <asp:Image ID="Pict" runat="server" ImageUrl='<%# Eval("PictureFileName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>


    <asp:Image runat="server" ImageUrl="https://larportal.com/img/Profile/CP0000002385.jpg" />


</asp:Content>
