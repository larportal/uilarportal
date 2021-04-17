<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="MasterSkillsEdit.aspx.cs" ValidateRequest="false" Inherits="LarpPortal.Campaigns.Setup.Skills.MasterSkillsEdit" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="MasterSkillsEditStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .ErrorDisplay {
            font-weight: bold;
            font-style: italic;
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="MasterSkillsEditScripts" ContentPlaceHolderID="MainScripts" runat="server">

    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="MasterSkillsEditBody" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Create a New Skill
                    </h1>
                </div>
            </div>
        </div>

        <div class="divide10"></div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Create a New Skill</div>
                    <div class="panel-body">
                        <%--                                <div class="pre-scrollable">--%>
                        <div class="row"></div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="<%# tbSkillName.ClientID %>">Skill Name:</label><asp:RequiredFieldValidator ID="rfvSkillName" runat="server" ControlToValidate="tbSkillName"
                                    CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                <asp:TextBox ID="tbSkillName" runat="server" CssClass="form-control" TextMode="MultiLine" Wrap="true" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbLongDescription %>">Long Description: </label>
                                    <asp:RequiredFieldValidator ID="rfvLongDescription"
                                        runat="server" ControlToValidate="tbLongDescription" CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbLongDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Wrap="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbShortDescription %>">Short Description: </label>
                                    <asp:RequiredFieldValidator ID="rfvShortDescription"
                                        runat="server" ControlToValidate="tbShortDescription" CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbShortDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Wrap="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbCardDescription %>">Card Description: </label>
                                    <asp:RequiredFieldValidator ID="rfvCardDescription"
                                        runat="server" ControlToValidate="tbCardDescription" CssClass="ErrorDisplay" Text="* Required" Display="Dynamic" />
                                    <asp:TextBox ID="tbCardDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Wrap="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="<%# tbIncant %>">Incant: </label>
                                    <asp:TextBox ID="tbIncant" runat="server" CssClass="form-control" TextMode="MultiLine" Wrap="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-1 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# chkCanUsePassively.ClientID %>">Can Be Used Passively</label>
                                    <asp:CheckBox ID="chkCanUsePassively" runat="server" Text="" CssClass="NoPadding" checked='<%# Eval("CanBeUsedPassively") %>' />
                                </div>
                            </div>
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label for="<%# ddlSkillType.ClientID %>">Skill Type</label>
                                    <asp:DropDownList ID="ddlSkillType" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-lg-5 col-xs-12"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-xs-12">
                                <div class="text-right">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="margin20"></div>
    </div>
    <asp:HiddenField ID="hidCampaignSkillsID" runat="server" />
    <asp:HiddenField ID="hidCampaignID" runat="server" />
</asp:Content>
