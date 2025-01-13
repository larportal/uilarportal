<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlayVideo.ascx.cs" Inherits="LarpPortal.Controls.PlayVideo" %>

<asp:HyperLink runat="server" ID="hlVideo" Target="_blank" Text="">
    <i class="fa fa-solid fa-youtube-play fa-1x" style="color: red;" aria-hidden="true"></i><asp:Label ID="lblText" runat="server" Font-Size="small" />
</asp:HyperLink>
