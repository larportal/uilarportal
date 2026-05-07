<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="JoinACampaign.aspx.cs" Inherits="LarpPortal.Campaigns.JoinACampaign" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="JoinACampaignStyles" ContentPlaceHolderID="MainStyles" runat="server">

    <style>
        @keyframes glowing {
            0% {
                background-color: #337ab7;
                box-shadow: 0 0 3px #337ab7;
            }

            50% {
                background-color: #f0ad4e;
                box-shadow: 0 0 12px #f0ad4e;
            }

            100% {
                background-color: #337ab7;
                box-shadow: 0 0 3px #337ab7;
            }
        }

        .button-glow {
            animation: glowing 1300ms infinite;
            color: white !important;
            font-weight: bold;
        }

        .campaign-description {
            max-height: 130px;
            overflow: hidden;
            position: relative;
        }

        .campaign-description-wrapper {
            border-bottom: 1px solid #ccc;
            padding-bottom: 8px;
            margin-bottom: 8px;
        }

        .campaign-description.expanded-description {
            max-height: none;
        }

        .see-more-link {
            display: inline-block;
            margin-top: 5px;
            cursor: pointer;
        }
    </style>
</asp:Content>

<asp:Content ID="JoinACampaignScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script>
        function Blink() {
            var btn = document.getElementById('<%= btnApplyFilters.ClientID %>');
            if (!btn) return;

            btn.classList.remove("button-glow");
            void btn.offsetWidth;
            btn.classList.add("button-glow");

            var hidBlink = document.getElementById('<%= hidBlink.ClientID %>');
            if (hidBlink) hidBlink.value = "Showing";
        }

        function updateAddCampaignButton(chk) {
            var row = chk.closest(".campaign-row");
            if (!row) return;

            var pc = row.querySelector("input[id*='chkPC']");
            var npc = row.querySelector("input[id*='chkNPC']");
            var btn = row.querySelector("input[id*='btnAddCampaign']");

            if (!btn) return;

            var pcAvailableChecked = pc && !pc.disabled && pc.checked;
            var npcAvailableChecked = npc && !npc.disabled && npc.checked;

            btn.disabled = !(pcAvailableChecked || npcAvailableChecked);
        }

        function looksLikePostalCode(value) {
            if (!value) return false;

            value = value.trim().toUpperCase();

            if (/^\d{5}(-\d{4})?$/.test(value)) return true;
            if (/^[A-Z]\d[A-Z]\s?\d[A-Z]\d$/.test(value)) return true;

            return false;
        }

        function updateDistanceState() {
            var zipBox = document.getElementById('<%= txtZipFilter.ClientID %>');
            var ddl = document.getElementById('<%= ddlDistanceFilter.ClientID %>');

            if (!zipBox || !ddl) return;

            var isValidFormat = looksLikePostalCode(zipBox.value);

            if (isValidFormat) {
                ddl.disabled = false;
            } else {
                ddl.selectedIndex = 0;
                ddl.disabled = true;
            }
        }

        function toggleDescription(link) {
            var desc = link.previousElementSibling;

            if (desc.classList.contains("expanded-description")) {
                desc.classList.remove("expanded-description");
                link.innerText = "See More";
            } else {
                desc.classList.add("expanded-description");
                link.innerText = "See Less";
            }
        }

        function initDescriptionToggles() {
            var wrappers = document.querySelectorAll(".campaign-description-wrapper");

            wrappers.forEach(function (w) {
                var desc = w.querySelector(".campaign-description");
                var link = w.querySelector(".see-more-link");
                if (!desc || !link) return;

                if (desc.scrollHeight <= desc.clientHeight + 1) {
                    link.style.display = "none";
                } else {
                    link.style.display = "inline-block";
                }
            });
        }

        document.addEventListener("DOMContentLoaded", function () {
            var zipBox = document.getElementById('<%= txtZipFilter.ClientID %>');

            if (zipBox) {
                zipBox.addEventListener("input", updateDistanceState);
                zipBox.addEventListener("input", Blink);
            }

            var filterControls = [
                '<%= ddlNameFilter.ClientID %>',
                '<%= ddlStateFilter.ClientID %>',
                '<%= ddlDistanceFilter.ClientID %>',
                '<%= ddlSystemFilter.ClientID %>',
                '<%= ddlGenreFilter.ClientID %>',
                '<%= ddlStyleFilter.ClientID %>',
                '<%= ddlTechFilter.ClientID %>',
                '<%= ddlSizeFilter.ClientID %>'
            ];

            filterControls.forEach(function (id) {
                var ctl = document.getElementById(id);
                if (ctl) ctl.addEventListener("change", Blink);
            });

            updateDistanceState();
            initDescriptionToggles();
        });

        if (typeof Sys !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                updateDistanceState();
                initDescriptionToggles();
            });
        }
    </script>
</asp:Content>

<asp:Content ID="JoinACampaignBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">

        <div class="row mb-2" style="padding-left: 5px;">
            <div class="col-xs-12" style="font-size: 20px;">
                <strong>Find a LARP</strong>
            </div>
        </div>

        <div style="height: 3px; background-color: darkblue; width: 100%; margin: 5px"></div>

        <div class="row mb-2" style="padding-left: 5px;">
            <div class="col-xs-12">
                <strong>FILTERS</strong>
                <asp:HiddenField ID="hidBlink" runat="server" />
            </div>
        </div>

        <div class="row mb-2" style="padding-left: 15px;">
            <div class="col-xs-2 form-group form-inline">
                <label for="ddlNameFilter">Name:&nbsp;</label>
                <asp:DropDownList ID="ddlNameFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-2 form-group form-inline">
                <label for="ddlStateFilter">State:&nbsp;</label>
                <asp:DropDownList ID="ddlStateFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="txtZipFilter">Zip:&nbsp;</label>
                <asp:TextBox ID="txtZipFilter"
                    runat="server"
                    CssClass="form-control"
                    Style="width: 75%;"
                    MaxLength="10"
                    placeholder="Enter zip"
                    oninput="Blink();" />
                <br />
                <asp:Label ID="lblZipError" runat="server" ForeColor="Red" Font-Size="Small" Visible="false" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="ddlDistanceFilter">Distance:&nbsp;</label>
                <asp:DropDownList ID="ddlDistanceFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-2 form-group form-inline">
                <label for="ddlSystemFilter">System:&nbsp;</label>
                <asp:DropDownList ID="ddlSystemFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>
        </div>

        <div class="row mb-2" style="padding-left: 15px;">
            <div class="col-xs-2 form-group form-inline">
                <label for="ddlGenreFilter">Genre:&nbsp;</label>
                <asp:DropDownList ID="ddlGenreFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-2 form-group form-inline">
                <label for="ddlStyleFilter">Style:&nbsp;</label>
                <asp:DropDownList ID="ddlStyleFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="ddlTechFilter">Tech:&nbsp;</label>
                <asp:DropDownList ID="ddlTechFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-3 form-group form-inline">
                <label for="ddlSizeFilter">Size:&nbsp;</label>
                <asp:DropDownList ID="ddlSizeFilter" runat="server" CssClass="form-control" Style="width: 75%;" />
            </div>

            <div class="col-xs-1 form-group">
                <asp:Button ID="btnClearAll"
                    runat="server"
                    CssClass="btn btn-primary btn-block"
                    Style="width: 90%;"
                    Text="Clear All"
                    ToolTip="Clear all filters"
                    OnClick="btnClearAll_Click" />
            </div>

            <div class="col-xs-1 form-group">
                <asp:Button ID="btnApplyFilters"
                    runat="server"
                    CssClass="btn btn-primary btn-block"
                    Style="width: 90%;"
                    Text="Apply Filters"
                    ToolTip="Apply all filters"
                    OnClick="btnApplyFilters_Click" />
            </div>
        </div>

        <div style="height: 3px; background-color: darkblue; width: 100%; margin: 5px"></div>

        <asp:Repeater ID="rptCampaigns" runat="server" OnItemCommand="rptCampaigns_ItemCommand" OnItemDataBound="rptCampaigns_ItemDataBound">
            <ItemTemplate>

                <div class="row campaign-row" style="padding: 15px 0;">

                    <div class="col-sm-2 text-center" style="min-height: 190px;">

                        <div style="width: 170px; height: 100px; display: flex; align-items: center; justify-content: center; margin: 0 auto 4px auto;">
                            <asp:PlaceHolder ID="phLogo" runat="server"
                                Visible='<%# !string.IsNullOrWhiteSpace(SafeEval(Eval("LogoUrl"))) %>'>

                                <img src='<%# GetLogoPath(Eval("LogoUrl")) %>'
                                    style='<%# GetLogoStyle(Eval("CampaignLogoWidth"), Eval("CampaignLogoHeight")) %>'
                                    alt="Campaign Logo"
                                    onerror="this.style.display='none';" />

                            </asp:PlaceHolder>
                        </div>

                        <h4 style="margin-top: 4px;">
                            <strong><%# SafeEval(Eval("CampaignName")) %></strong>
                        </h4>

                        <h5 style="margin-top: 4px;">
                            <%# SafeEval(Eval("City")) %>, <%# SafeEval(Eval("State")) %>
                        </h5>

                    </div>

                    <div class="col-sm-7">
                        <div class="campaign-description-wrapper">
                            <div class="campaign-description collapsed-description">
                                <%# SafeEval(Eval("Description")) %>
                            </div>
                            <a href="javascript:void(0);" class="see-more-link" onclick="toggleDescription(this);">See More</a>
                        </div>

                        <p>
                            <%# RenderLink(Eval("CampaignUrl"), "Website") %>&nbsp;&nbsp;
                        <%# RenderLink(Eval("RulesUrl"), "Rules") %>&nbsp;&nbsp;
                        <%# RenderLink(Eval("DiscordUrl"), "Discord") %>
                        </p>

                        <p>
                            <strong>Upcoming Events:</strong>
                            &nbsp;<%# Eval("Event1", "{0:MM/dd/yyyy}") %> &nbsp;&nbsp;
                        &nbsp;<%# Eval("Event2", "{0:MM/dd/yyyy}") %> &nbsp;&nbsp;
                        &nbsp;<%# Eval("Event3", "{0:MM/dd/yyyy}") %>
                        </p>
                    </div>

                    <div class="col-sm-3" style="line-height: 1.0">

                        <p><strong>Game System:</strong> <%# SafeEval(Eval("GameSystem")) %></p>
                        <p><strong>Genre:</strong> <%# SafeEval(Eval("Genre")) %></p>
                        <p><strong>Style:</strong> <%# SafeEval(Eval("Style")) %></p>
                        <p><strong>Tech Level:</strong> <%# SafeEval(Eval("TechLevel")) %></p>
                        <p><strong>Size:</strong> <%# SafeEval(Eval("Size")) %></p>

                        <p>
                            <strong>Primary Location:</strong>
                            <%# SafeEval(Eval("PrimaryLocation")) %>
                            <%# string.IsNullOrWhiteSpace(SafeEval(Eval("PrimaryCity")))
                            ? ""
                            : " - " + SafeEval(Eval("PrimaryCity")) + ", " + SafeEval(Eval("PrimaryState")) %>
                        </p>

                        <p>
                            <strong>Secondary Location:</strong>
                            <%# SafeEval(Eval("SecondaryLocation")) %>
                            <%# string.IsNullOrWhiteSpace(SafeEval(Eval("SecondaryCity")))
                            ? ""
                            : " - " + SafeEval(Eval("SecondaryCity")) + ", " + SafeEval(Eval("SecondaryState")) %>
                        </p>

                        <div class="checkbox">
                            <span id="spnPC" runat="server">
                                <label>
                                    <asp:CheckBox ID="chkPC" runat="server" onclick="updateAddCampaignButton(this);" />
                                    PC
                                </label>
                            </span>

                            <span id="spnNPC" runat="server">
                                <label>
                                    <asp:CheckBox ID="chkNPC" runat="server" onclick="updateAddCampaignButton(this);" />
                                    NPC
                                </label>
                            </span>
                        </div>

                        <asp:Button ID="btnAddCampaign"
                            runat="server"
                            CssClass="btn btn-primary btn-block"
                            Style="width: 30%;"
                            Text="Add Campaign"
                            CommandName="AddCampaign"
                            CommandArgument='<%# Eval("CampaignID") %>' />
                    </div>

                </div>

                <hr style="border: 0; border-top: 1px solid #ccc; margin: 10px 0;" />

            </ItemTemplate>
        </asp:Repeater>

        <div class="row mt-3">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnPreviousPage" runat="server"
                    Text="Previous"
                    CssClass="btn btn-secondary"
                    OnClick="btnPreviousPage_Click" />

                <asp:Label ID="lblPageInfo" runat="server"
                    CssClass="mx-3" />

                <asp:Button ID="btnNextPage" runat="server"
                    Text="Next"
                    CssClass="btn btn-secondary"
                    OnClick="btnNextPage_Click" />
            </div>
        </div>
    </div>
</asp:Content>
