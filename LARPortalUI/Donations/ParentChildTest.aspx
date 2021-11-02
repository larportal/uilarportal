<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="ParentChildTest.aspx.cs" Inherits="LarpPortal.ParentChildTest.ParentChildTest" %>

<asp:Content ID="ParentChildTestStyles" ContentPlaceHolderID="MainStyles" runat="Server">
</asp:Content>

<asp:Content ID="ParentChildTestScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>

<asp:Content ID="ParentChildTestBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Users</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-condensed" DataKeyNames="UserID" OnRowDataBound="OnRowDataBound">

                    <Columns>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <img src="images/plus.png" alt="" width="20" style="cursor: pointer"  />
                                <asp:Panel ID="pnlCharacters" runat="server" Style="display: none">
                                    <asp:GridView ID="gvCharacters" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="100px" DataField="CharacterID" HeaderText="Character Id" />
                                            <asp:BoundField ItemStyle-Width="100px" DataField="CharacterAKA" HeaderText="AKA" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField ItemStyle-Width="150px" DataField="FirstName" HeaderText="First" />
                        <asp:BoundField ItemStyle-Width="150px" DataField="LastName" HeaderText="Last" />
                        <asp:BoundField ItemStyle-Width="1000px" NullDisplayText="" HeaderText="" />
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
    <!-- /#page-wrapper -->
</asp:Content>
