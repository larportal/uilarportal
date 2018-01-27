<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="Preferences.aspx.cs" Inherits="LarpPortal.Profile.Preferences" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="PlayerPreferencesStyles" ContentPlaceHolderID="MainStyles" runat="Server">
    <style>
        .BottomAlign {
            vertical-align: middle !important;
        }

        .DataRadioButton {
            font-weight: normal !important;
            width: 200px;
        }

            .DataRadioButton label {
                font-weight: normal !important;
                color: black;
            }

            .DataRadioButton input[type="radio"]:checked + label {
                font-weight: bold !important;
                /*font-size: larger !important;*/
            }

            .DataRadioButton input[type="radio"]:disabled + label {
                color: gray;
            }
    </style>
</asp:Content>
<asp:Content ID="PlayerPreferencesScripts" ContentPlaceHolderID="MainScripts" runat="Server">
</asp:Content>
<asp:Content ID="PlayerPreferencesBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <div class="header-background-image">
                    <h1>Player Preferences</h1>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Preferences</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="panel-container" style="max-height: 500px; overflow: auto;">
                                    <asp:GridView ID="gvPref" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvPref_RowDataBound"
                                        CssClass="table table-striped table-hover table-condensed" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="99%">
                                        <RowStyle VerticalAlign="Bottom" />
                                        <HeaderStyle CssClass="panel-heading" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="false" CssClass="" Width="200px" />
                                                <ItemTemplate>
                                                    <asp:RadioButtonList ID="rblOptions" runat="server" OnSelectedIndexChanged="rblOptions_SelectedIndexChanged" CssClass="DataRadioButton"
                                                        AutoPostBack="true" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="None" Value="None" />
                                                        <asp:ListItem Text="EMail" Value="EMail" />
                                                        <asp:ListItem Text="Text" Value="Text" />
                                                    </asp:RadioButtonList>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="false" Width="200px" />
                                                <HeaderTemplate>
                                                    <asp:RadioButtonList ID="rblHeaderOptions" runat="server" CssClass="DataRadioButton" Font-Bold="true" OnSelectedIndexChanged="rblHeaderOptions_SelectedIndexChanged"
                                                        AutoPostBack="true" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="None" Value="None" />
                                                        <asp:ListItem Text="EMail" Value="EMail" />
                                                        <asp:ListItem Text="Text" Value="Text" />
                                                    </asp:RadioButtonList>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle CssClass="BottomAlign" Wrap="false" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDisplayDesc" runat="server" Text='<%# Eval("DisplayDesc") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="BottomAlign" Font-Size="larger" Wrap="false" />
                                                <HeaderTemplate>
                                                    <b>Set all records.</b>
                                                    <asp:Label ID="lblHeaderEMail" runat="server" Style="padding-left: 50px;" Font-Bold="false" />
                                                    <asp:Label ID="lblHeaderText" runat="server" Style="padding-left: 50px;" Font-Bold="false" />
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="push"></div>
        <asp:HiddenField ID="hidPlayerProfileID" runat="server" />
        <asp:HiddenField ID="hidMobileNumber" runat="server" />
        <asp:HiddenField ID="hidEMail" runat="server" />
    </div>
    <!-- /#page-wrapper -->
</asp:Content>

