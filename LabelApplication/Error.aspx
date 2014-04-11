<%@ Page Title="Error" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="LabelApplication.Error" %>

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
    <hgroup class="title">
        <h2>An Error Has Occurred.</h2>
    </hgroup>
         <p class="label">An error has occurred during the processing of the Label Application.</p>
         <br />
         <p class="label">More details of the error are stored in the NLog database.</p>    
 </div>       
</section>
</asp:Content>

