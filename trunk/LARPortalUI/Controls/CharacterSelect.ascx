<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CharacterSelect.ascx.cs" Inherits="LarpPortal.Controls.CharacterSelect" %>

<style type="text/css">
    .selectWidth {
        width: auto !important;
    }

    .noborder {
        border: 0px none transparent;
          transition: none;
          -webkit-transition: none;
          box-shadow: none;
    }

    .row-flex-center {
        display: flex;
        align-items: center;
        vertical-align: middle;
    }

    .no-left-right-padding {
        padding-left: 0px;
        padding-right: 0px;
    }
</style>

<div class="row">
    <div class="col-sm-12 noborder">
        <%-- style="border: solid 1px black;">--%>
        <div class="input-group noborder">
            <label for="<%= ddlCharacterSelector.ClientID %>">Selected Character:</label>
            <div class="row col-lg-12 row-flex-center">
                <asp:DropDownList ID="ddlCharacterSelector" runat="server" AutoPostBack="true" Style="padding-right: 10px;" CssClass="form-control selectWidth" OnSelectedIndexChanged="ddlCharacterSelector_SelectedIndexChanged" />
                <asp:DropDownList ID="ddlCampCharSelector" runat="server" AutoPostBack="true" Style="padding-right: 10px;" CssClass="form-control selectWidth" OnSelectedIndexChanged="ddlCampCharSelector_SelectedIndexChanged" />
                <asp:Label ID="lblNoCharacters" runat="server" Text="There are no characters available for that campaign." CssClass="form-control noborder" Style="border: 0px solid white;" Visible="false" />
                <asp:RadioButton ID="rbMyCharacters" GroupName="CharacterGroup" runat="server" AutoPostBack="true" Text="My Characters" Style="padding-right: 10px; border: 0px solid white;" CssClass="form-control selectWidth noborder align-middle" OnCheckedChanged="rbMyCharacters_CheckedChanged" />
                <asp:RadioButton ID="rbCampaignCharacters" GroupName="CharacterGroup" runat="server" AutoPostBack="true" Text="Campaign Characters" Style="padding-right: 10px;" CssClass="form-control selectWidth noborder align-middle" OnCheckedChanged="rbMyCharacters_CheckedChanged" />
                <asp:Label ID="lblCampaign" runat="server" Text="Campaign: " CssClass="form-control selectWidth noborder align-middle" />
                <asp:DropDownList ID="ddlCampaigns" runat="server" AutoPostBack="true" CssClass="form-control selectWidth noborder align-middle" OnSelectedIndexChanged="ddlCampaigns_SelectedIndexChanged" />
                <asp:Label ID="lblSelectedCampaign" runat="server" Text="" CssClass="form-control selectWidth noborder no-left-right-padding align-middle" />
                <asp:Label ID="lblSelectedCharCampaign" runat="server" Text="" CssClass="form-control selectWidth noborder no-left-right-padding align-middle" />
                <asp:HiddenField ID="hidNumMyChar" runat="server" Value="0" />
                <asp:HiddenField ID="hidNumMyCamp" runat="server" Value="0" />
            </div>
        </div>
    </div>
</div>
