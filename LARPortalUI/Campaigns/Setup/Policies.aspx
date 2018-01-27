<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="Policies.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Policies" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CampaignsPoliciesStyles" ContentPlaceHolderID="MainStyles" runat="server">
    <style>
        .funkyradio div {
            /*clear: both;*/
            overflow: hidden;
        }

        .funkyradio label {
            width: 100%;
            border-radius: 3px;
            border: 1px solid #D1D3D4;
            font-weight: normal;
        }

        .funkyradio input[type="radio"]:empty,
        .funkyradio input[type="checkbox"]:empty {
            display: none;
        }

            .funkyradio input[type="radio"]:empty ~ label,
            .funkyradio input[type="checkbox"]:empty ~ label {
                position: relative;
                line-height: 2.5em;
                text-indent: 3.75em;
                margin-bottom: 1em;
                cursor: pointer;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
            }

                .funkyradio input[type="radio"]:empty ~ label:before,
                .funkyradio input[type="checkbox"]:empty ~ label:before {
                    position: absolute;
                    display: block;
                    top: 0;
                    bottom: 0;
                    left: 0;
                    content: '';
                    width: 2.5em;
                    background: #D1D3D4;
                    border-radius: 3px 0 0 3px;
                }

        /*.funkyradio input[type="radio"]:hover:not(:checked) ~ label,
        .funkyradio input[type="checkbox"]:hover:not(:checked) ~ label {
            color: #888;
        }

            .funkyradio input[type="radio"]:hover:not(:checked) ~ label:before,
            .funkyradio input[type="checkbox"]:hover:not(:checked) ~ label:before {
                font-family: FontAwesome;
                content: '\2714';
                text-indent: .9em;
                color: #C2C2C2;
            }*/

        .funkyradio input[type="radio"]:checked ~ label,
        .funkyradio input[type="checkbox"]:checked ~ label {
            color: #777;
        }

            .funkyradio input[type="radio"]:checked ~ label:before,
            .funkyradio input[type="checkbox"]:checked ~ label:before {
                font-family: FontAwesome;
                content: '\f046';
                font-size: larger;
                text-indent: 0.9em;
                color: #333;
                background-color: #ccc;
            }


        .funkyradio input[type="radio"]:not(:checked) ~ label:before,
        .funkyradio input[type="checkbox"]:not(:checked) ~ label:before {
            font-family: FontAwesome;
            content: '\f096';
            font-size: larger;
            text-indent: 0.9em;
            color: #333;
            background-color: #ccc;
        }

        .funkyradio input[type="radio"]:focus ~ label:before,
        .funkyradio input[type="checkbox"]:focus ~ label:before {
            box-shadow: 0 0 0 3px #999;
        }

        .funkyradio-default input[type="radio"]:checked ~ label:before,
        .funkyradio-default input[type="checkbox"]:checked ~ label:before {
            color: #333;
            background-color: #ccc;
        }

        .funkyradio-primary input[type="radio"]:checked ~ label:before,
        .funkyradio-primary input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #337ab7;
        }

        .funkyradio-success input[type="radio"]:checked ~ label:before,
        .funkyradio-success input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #5cb85c;
        }

        .funkyradio-danger input[type="radio"]:checked ~ label:before,
        .funkyradio-danger input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #d9534f;
        }

        .funkyradio-warning input[type="radio"]:checked ~ label:before,
        .funkyradio-warning input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #f0ad4e;
        }

        .funkyradio-info input[type="radio"]:checked ~ label:before,
        .funkyradio-info input[type="checkbox"]:checked ~ label:before {
            color: #fff;
            background-color: #5bc0de;
        }
    </style>
</asp:Content>

<asp:Content ID="CampaignsPoliciesScripts" ContentPlaceHolderID="MainScripts" runat="server">
    <script type="text/javascript">
        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CampaignsPoliciesBody" ContentPlaceHolderID="MainBody" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Campaign Setup Information<asp:Label ID="lblHeaderCampaignName" runat="server" /></h1>
                </div>
            </div>
        </div>
        <div class="margin20"></div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Policies</div>
                    <div class="panel-body">
                        <div class="funkyradio">
                            <div class="row funkyradio-primary">
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <input type="checkbox" name="chkAllowCharacterRebuilds" id="chkAllowCharacterRebuilds" runat="server" />
                                    <label for="<%= chkAllowCharacterRebuilds.ClientID %>">Allow Character Rebuilds</label>
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <input type="checkbox" name="chkAllowCPDonation" id="chkAllowCPDonation" runat="server" />
                                    <label for="<%= chkAllowCPDonation.ClientID %>">Allow CP Donation</label>
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <input type="checkbox" name="chkShareLocationUseNotes" id="chkShareLocationUseNotes" runat="server" />
                                    <label for="<%= chkShareLocationUseNotes.ClientID %>">Share Location Use Notes</label>
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <input type="checkbox" name="chkNPCApprovalRequired" id="chkNPCApprovalRequired" runat="server" />
                                    <label for="<%= chkNPCApprovalRequired.ClientID %>">NPC Approval Required</label>
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <input type="checkbox" name="chkUseCampaignCharacters" id="chkUseCampaignCharacters" runat="server" />
                                    <label for="<%= chkUseCampaignCharacters.ClientID %>">Use Campaign Characters</label>
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <input type="checkbox" name="chkPCApprovalRequired" id="chkPCApprovalRequired" runat="server" />
                                    <label for="<%= chkPCApprovalRequired.ClientID %>">PC Approval Required</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= ddlPELApprovalLevel.ClientID %>">PEL Approval Level</label>
                                <asp:DropDownList ID="ddlPELApprovalLevel" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="1" Value="1" />
                                    <asp:ListItem Text="This is a long one" Value="2" />
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= ddlCharacterApprovalLevel.ClientID %>">Character Approval Level</label>
                                <asp:DropDownList ID="ddlCharacterApprovalLevel" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Something goes here" Value="1" />
                                    <asp:ListItem Text="More things go here." Value="2" />
                                </asp:DropDownList>
                            </div>

                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbEarliestCPApplicationYear.ClientID %>">Earliest Point Application Year</label>
                                <asp:TextBox ID="tbEarliestCPApplicationYear" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbEarliestCPApplicationYear" runat="server" Enabled="True" TargetControlID="tbEarliestCPApplicationYear" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbEventCharacterCap.ClientID %>">Event Character Cap</label>
                                <asp:TextBox ID="tbEventCharacterCap" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbEventCharacterCap" runat="server" Enabled="True" TargetControlID="tbEventCharacterCap" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbMaximumCPPerYear.ClientID %>">Max Points Per Year</label>
                                <asp:TextBox ID="tbMaximumCPPerYear" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbMaximumCPPerYear" runat="server" Enabled="True" TargetControlID="tbMaximumCPPerYear" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <label for="<%= tbTotalCharacterCap.ClientID %>">Total Character Cap</label>
                                <asp:TextBox ID="tbTotalCharacterCap" runat="server" CssClass="form-control" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbTotalCharacterCap" runat="server" Enabled="True" TargetControlID="tbTotalCharacterCap" FilterType="Numbers" />
                            </div>
                            <div class="form-group col-lg-6 col-md-12 col-xs-12">
                                <label for="<%= tbCrossCampaignPosting.ClientID %>">Cross Campaign Posting</label>
                                <asp:TextBox ID="tbCrossCampaignPosting" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbCrossCampaignPosting" runat="server" Enabled="True" TargetControlID="tbCrossCampaignPosting" FilterType="Numbers" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="col-xs-12">
                                <div class="text-right">
                                    <asp:Button ID="btnSaveRepeat" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveChanges_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="margin20"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalMessage" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title text-center">Policies</h3>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="lblmodalMessage" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <div class="row">
                        <div class="col-xs-12 text-right">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(function () {
            $('.button-checkbox').each(function () {

                // Settings
                var $widget = $(this),
                    $button = $widget.find('button'),
                    $checkbox = $widget.find('input:checkbox'),
                    color = $button.data('color'),
                    settings = {
                        on: {
                            icon: 'glyphicon glyphicon-check'
                        },
                        off: {
                            icon: 'glyphicon glyphicon-unchecked'
                        }
                    };

                // Event Handlers
                $button.on('click', function () {
                    $checkbox.prop('checked', !$checkbox.is(':checked'));
                    $checkbox.triggerHandler('change');
                    updateDisplay();
                });
                $checkbox.on('change', function () {
                    updateDisplay();
                });

                // Actions
                function updateDisplay() {
                    var isChecked = $checkbox.is(':checked');

                    // Set the button's state
                    $button.data('state', (isChecked) ? "on" : "off");

                    // Set the button's icon
                    $button.find('.state-icon')
                        .removeClass()
                        .addClass('state-icon ' + settings[$button.data('state')].icon);

                    // Update the button's color
                    if (isChecked) {
                        $button
                            .removeClass('btn-default')
                            .addClass('btn-' + color + ' active');
                    }
                    else {
                        $button
                            .removeClass('btn-' + color + ' active')
                            .addClass('btn-default');
                    }
                }

                // Initialization
                function init() {

                    updateDisplay();

                    // Inject the icon if applicable
                    if ($button.find('.state-icon').length == 0) {
                        $button.prepend('<i class="state-icon ' + settings[$button.data('state')].icon + '"></i> ');
                    }
                }
                init();
            });
        });
    </script>

</asp:Content>
