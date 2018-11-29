<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.Master" AutoEventWireup="true" CodeBehind="HistoryDisplay.aspx.cs" Inherits="LarpPortal.Reports.HistoryDisplay" %>
<%@ MasterType TypeName="LarpPortal.LARPortal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainStyles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainScripts" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainBody" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Character History Display</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-inline col-xs-12">

            </div>
        </div>
        <div class="divide10"></div>
        <asp:Panel ID="pnlReport" runat="server" Visible="true">
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Character History Display</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="panel-container" style="max-height: 500px; overflow: auto;">
                        <asp:Button ID="btnCloseTop" runat="server" OnClick="btnCloseTop_Click" Text="Close" CssClass="StandardButton" />
						<div class="row">
							<div class="panel-wrapper col-md-10">
								<div class="panel">
									<div class="panel-header">
										<h2><asp:Label ID="lblPanelHeader" runat="server"></asp:Label></h2>
									</div>
									<div class="panel-body">
										<div class="panel-container">
                                            <asp:Label ID="lblWhatsNewDetail" runat="server"></asp:Label>
                                            <div class="row">
                                                <br /><br /><br />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" CssClass="StandardButton" />
                                            </div>
										</div>
									</div>
								</div>
							</div>
						</div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>