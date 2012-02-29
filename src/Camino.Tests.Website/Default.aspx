<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Camino.Tests.Website._Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
	<ul>
		<li><a href="/realfile.aspx">Real file (/realfile.aspx)</a></li>
		<li><a href="/realfile2.aspx">Real file based on embedded master page (/realfile2.aspx)</a></li>
		<li><a href="/test/embedded.aspx">Embedded file (/test/embedded.aspx)</a></li>
		<li><a href="/test">Embedded directory with default document (/test)</a></li>
		<li><a href="/test/">Embedded directory with trailing slash with default document (/test/)</a></li>
		<li><a href="/test/childfolder/embeddedunderfolder.aspx">Embedded file under folder (/test/childfolder/embeddedunderfolder.aspx)</a></li>
		<li><a href="/test/nested">Embedded directory with default document nested under same prefix as another assembly (/test/nested)</a></li>
		<li><a href="/test/nested/">Embedded directory with trailing slash with default document nested under same prefix as another assembly (/test/nested/)</a></li>
		<li><a href="/test/nested/nested.aspx">Embedded file nested under same prefix as another assembly (/test/nested/nested.aspx)</a></li>
	</ul>
</asp:Content>