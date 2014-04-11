<%@ Page Title="Labels for CJON/Forum/ONS Connect" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LabelApplication._Default" %>
<asp:Content ID="ContentScript" ContentPlaceHolderID="ContentPlaceholderScript" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        bindButton();
    });
    function bindButton() {
        $('#<%=btnCreateFiles.ClientID%>').on('click', function () {
            $('#<%=lblCaption.ClientID%>').html('<img src="/Images/updateProgress.gif"/> <font color="red"> Please wait while the label files are being created...</font> ');   
        });
    }
</script>
</asp:Content>
<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>           
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <section class="main">   
        <div class="content-wrapper">     
            <asp:Label ID="lblEnvironment" runat="server" Text="Current Environment Setting: " CssClass="label"></asp:Label>
            <asp:TextBox ID="txtBoxEnvironment" runat="server" Enabled="False" Height="19px" Width="100px" 
                Font-Bold="True" ReadOnly="True" style="text-align:center"></asp:TextBox>
            <br />
            <br /> 
            <asp:Panel ID="pnlPublicationSelection" runat="server" GroupingText="Select a Publication" Height="157px" Width="475px" HorizontalAlign="Left" BorderStyle="None" Font-Bold="true" style="margin-top: 53px" CssClass="label">
                 <br />
                <asp:RadioButton ID="rboJCON" runat="server"  GroupName="PubSelection" Text="Oncology Journal of Oncology Nursing (JCON)" Checked="true" CssClass="radiobutton"/>
                <br />
                <br />
                <asp:RadioButton ID="rboForum" runat="server"  GroupName="PubSelection" Text="Oncology Nursing Forum" CssClass="radiobutton"/>
                <br />
                <br />
                <asp:RadioButton ID="rboConnect" runat="server" GroupName="PubSelection" Text="ONS Connect"  CssClass="radiobutton"/>
                 <br />
            </asp:Panel>
            <br />  
            <asp:Label ID="lblSelectMonth" runat="server" Text="Select an Issue Month: " Font-Bold="true" CssClass="label"></asp:Label> 
            <asp:DropDownList ID="ddlMonth" runat="server" Height="25px" Width="130px">
                <asp:ListItem Selected="True">January</asp:ListItem>
                <asp:ListItem>February</asp:ListItem>
                <asp:ListItem>March</asp:ListItem>
                <asp:ListItem>April</asp:ListItem>
                <asp:ListItem>May</asp:ListItem>
                <asp:ListItem>June</asp:ListItem>
                <asp:ListItem>July</asp:ListItem>
                <asp:ListItem>August</asp:ListItem>
                <asp:ListItem>September</asp:ListItem>
                <asp:ListItem>October</asp:ListItem>
                <asp:ListItem>November</asp:ListItem>
                <asp:ListItem>December</asp:ListItem>
            </asp:DropDownList>      
            <br />
            <br />    
            <asp:UpdatePanel ID="ProgressUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate> 
                    <asp:Label ID="lblCaption" runat="server" CssClass="label" ForeColor="Red" Visible="true"></asp:Label>            
                    <asp:Button ID="btnCreateFiles" runat="server" Text="Create Labels Files" Width="206px" CssClass="label" Height="37px" 
                        OnClick="btnCreateFiles_Click" style="float: right"/>                               
                </ContentTemplate>
            </asp:UpdatePanel>           
            <br />
            <br />    
        </div>
     </section>
</asp:Content>
