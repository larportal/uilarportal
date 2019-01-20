<%@ Page Title="" Language="C#" MasterPageFile="~/LARPortal.master" AutoEventWireup="true" CodeBehind="DisplayTestSkills.aspx.cs" Inherits="LarpPortal.Campaigns.Setup.Skills.DisplayTestSkills" %>

<%@ MasterType TypeName="LarpPortal.LARPortal" %>

<asp:Content ID="CharSkillsStyles" ContentPlaceHolderID="MainStyles" runat="Server"></asp:Content>
<asp:Content ID="CharSkillsScripts" ContentPlaceHolderID="MainScripts" runat="Server">
    <script type="text/javascript">
<%--        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                var hiddenStatusFlag = document.getElementById('<%= hidScrollPos.ClientID%>');
                if (hiddenStatusFlag != null) {
                    hiddenStatusFlag.value = $get('<%=pnlTreeView.ClientID%>').scrollTop;
                }
                __doPostBack("", "");
            }
        }--%>

        function ShowContent(d) {
            if (d.length < 1) { return; }
            var dd = document.getElementById(d);
            dd.style.display = "block";
        }

        function GetContent(d) {
            $.ajax({
                contentType: "application/json; charset=utf-8",
                data: "{'SkillNodeID':'" + d.toString() + "'}",
                url: "/Webservices/TestSkills2.asmx/GetSkillNodeInfo",
                type: "POST",
                dataType: 'json',
                success: function (result) {
                    var divDesc = document.getElementsByName('divDesc');
                    divDesc.innerHTML = result.d;
                    return true;
                },
                error: function() { alert("error"); }
            });
        }

        function Callback(result) {
            var outDiv = document.getElementById("outputDiv");
            outDiv.innerText = result;
        }
        function OnSuccessCall(response) {
            alert(response.d);
        }

        function OnErrorCall(response) {
            alert(response.status + " " + response.statusText);
        }

        function openMessageWithText(Msg) {
            parent.openMessageWithText(Msg);
        }

        function openErrorWithText(Msg) {
            parent.openErrorWithText(Msg);
        }

        function openMessage() {
            $('#modalMessage').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="CharSkillsBody" ContentPlaceHolderID="MainBody" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="header-background-image">
                    <h1>Display Character Test Skills</h1>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">Character Test Skills from TestSkills database.</div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSkill" runat="server">
                            <ContentTemplate>
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-xs-7">
                                            <asp:Panel ID="pnlTreeView" runat="server" ScrollBars="Auto" Height="500px">
                                                <asp:TreeView ID="tvDisplaySkills" runat="server" SkipLinkText="" BorderColor="Black" BorderStyle="Solid" BorderWidth="0"
                                                    ShowLines="true" Font-Underline="false" CssClass="form-control" EnableClientScript="false" HoverNodeStyle-Font-Bold="true"
                                                    LeafNodeStyle-CssClass="TreeItems" NodeStyle-CssClass="TreeItems">
                                                    <LevelStyles>
                                                        <asp:TreeNodeStyle Font-Underline="false" />
                                                    </LevelStyles>
                                                    
                                                </asp:TreeView>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-xs-5">
                                            <asp:Panel ID="pnlDescription" runat="server" ScrollBars="Auto" Height="500px" Visible="true">
                                                <div id="divDesc">This is the start of the div.</div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
