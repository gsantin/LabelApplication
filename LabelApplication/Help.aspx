<%@ Page Title="Label Application Instructions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="LabelApplication.Help" %>

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
        <h2>Application Description</h2>
    </hgroup>
    <article>
        <p>        
            This application generates label files for the bi-monthly issues of the following publications:
        </p>
        <ul>
            <li>Clinical Journal of Oncology Nursing (CJON)</li>
            <li>Oncology Nursing Forum (Forum)</li>
            <li>ONS Connect</li>
        </ul>
        <p>        
           This application is to be run once every month for one or more of the publications.
            <br />
            Below is a detailed listing:
        </p>

        <asp:Table ID="IssueTable" runat="server" BorderColor="Black" BorderStyle="Solid" GridLines="Both" 
            HorizontalAlign="Center"  Width="400px">
            <asp:TableHeaderRow HorizontalAlign="Center" Font-Bold="True" CssClass="myHeight20" BackColor="#bcdfec">
                 <asp:TableCell>App Run Date</asp:TableCell> 
                <asp:TableCell>Issue Month</asp:TableCell>
                <asp:TableCell>Journal(s)</asp:TableCell>
            </asp:TableHeaderRow> 

            <asp:TableRow ID="TableRow1" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">
                <asp:TableCell>January 9th</asp:TableCell>
                <asp:TableCell>February</asp:TableCell>
                <asp:TableCell>CJON</asp:TableCell>              
            </asp:TableRow>

             <asp:TableRow ID="TableRow2" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">
                <asp:TableCell>February 9th</asp:TableCell>
                <asp:TableCell>March</asp:TableCell>
                <asp:TableCell>Forum</asp:TableCell>            
            </asp:TableRow>

            <asp:TableRow ID="TableRow3" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">
                <asp:TableCell RowSpan="2">March 9th</asp:TableCell>
                <asp:TableCell RowSpan="2">April</asp:TableCell>
                <asp:TableCell>CJON</asp:TableCell>              
            </asp:TableRow>

             <asp:TableRow ID="TableRow4" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">           
                <asp:TableCell>Connect</asp:TableCell>              
            </asp:TableRow>

            <asp:TableRow ID="TableRow5" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">
                <asp:TableCell>April 9th</asp:TableCell>
                <asp:TableCell>May</asp:TableCell>
                <asp:TableCell>Forum</asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="TableRow6" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">
                <asp:TableCell>May 9th</asp:TableCell>
                <asp:TableCell>June</asp:TableCell>
                <asp:TableCell>CJON</asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="TableRow7" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">
                <asp:TableCell RowSpan="2">June 9th</asp:TableCell>
                <asp:TableCell RowSpan="2">July</asp:TableCell>
                <asp:TableCell>Forum</asp:TableCell>   
            </asp:TableRow>

            <asp:TableRow ID="TableRow8" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">       
                <asp:TableCell>Connect</asp:TableCell>              
            </asp:TableRow>

            <asp:TableRow ID="TableRow9" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">
                <asp:TableCell>July 9th</asp:TableCell>
                <asp:TableCell>August</asp:TableCell>
                <asp:TableCell>CJON</asp:TableCell>          
            </asp:TableRow>

            <asp:TableRow ID="TableRow10" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">
                <asp:TableCell>August 9th</asp:TableCell>
                <asp:TableCell>September</asp:TableCell>
                <asp:TableCell>Forum</asp:TableCell>
            </asp:TableRow>

             <asp:TableRow ID="TableRow11" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">
                <asp:TableCell RowSpan="2">September 9th</asp:TableCell>
                <asp:TableCell RowSpan="2">October</asp:TableCell>
                <asp:TableCell>CJON</asp:TableCell>
             </asp:TableRow>

            <asp:TableRow ID="TableRow12" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">         
                <asp:TableCell>Connect</asp:TableCell>              
            </asp:TableRow>

            <asp:TableRow ID="TableRow13" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">
                <asp:TableCell>October 9th</asp:TableCell>
                <asp:TableCell>November</asp:TableCell>
                <asp:TableCell>Forum</asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="TableRow14" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="White">
                <asp:TableCell>November 9th</asp:TableCell>
                <asp:TableCell>December</asp:TableCell>
                <asp:TableCell>CJON</asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="TableRow15" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">
                <asp:TableCell RowSpan="2">December 9th</asp:TableCell>
                <asp:TableCell RowSpan="2">January</asp:TableCell>
                <asp:TableCell>Forum</asp:TableCell>
            </asp:TableRow>
            
             <asp:TableRow ID="TableRow16" runat="server" HorizontalAlign="Left" CssClass="myHeight15" BackColor="#bcdfec">       
                <asp:TableCell>Connect</asp:TableCell>              
            </asp:TableRow>

        </asp:Table>

    </article>

    <hgroup class="title">
        <h2>Steps to Creating Label Files</h2>
    </hgroup>
        <ol>
            <li>Select a publication type. (CJON / Forum / ONS Connect)</li>
            <li>Select an Issue Month. (the Issue Date is automatically created.)</li>
            <li>Click the button, <b>Create Label Files</b>.</li>
        </ol>
        <p>
            After the process has completed, text files will be stored in the locations set in the web.config file.
            Below is a list of the number of files created for each publication:
       </p>
      
         <asp:Table ID="TextTable" runat="server" BorderColor="Black" BorderStyle="Solid" GridLines="Both" HorizontalAlign="Center" Width="262px">
             <asp:TableHeaderRow HorizontalAlign="Center" Font-Bold="True" CssClass="myHeight15" BackColor="#bcdfec">
                 <asp:TableHeaderCell>Folder Name</asp:TableHeaderCell>
                 <asp:TableHeaderCell>Files Created</asp:TableHeaderCell>
             </asp:TableHeaderRow> 
             <asp:TableRow runat="server" HorizontalAlign="Center" CssClass="myHeight10" BackColor="White">
                 <asp:TableCell runat="server">CJON</asp:TableCell>
                 <asp:TableCell runat="server">8</asp:TableCell>
             </asp:TableRow>
             <asp:TableRow runat="server" HorizontalAlign="Center" CssClass="myHeight10" BackColor="#bcdfec">
                 <asp:TableCell runat="server">Forum</asp:TableCell>
                 <asp:TableCell runat="server">8</asp:TableCell>
             </asp:TableRow>
             <asp:TableRow runat="server" HorizontalAlign="Center" CssClass="myHeight10"  BackColor="White">
                 <asp:TableCell runat="server">Connect</asp:TableCell>
                 <asp:TableCell runat="server">4</asp:TableCell>
             </asp:TableRow>
             <asp:TableRow runat="server" HorizontalAlign="Center" CssClass="myHeight10" BackColor="#bcdfec">
                 <asp:TableCell runat="server">Count</asp:TableCell>
                 <asp:TableCell runat="server">1</asp:TableCell>
             </asp:TableRow>         
         </asp:Table>
         <p>
             The files will have to zipped manually before sending them to the publication house.  
             The application also creates a count file by label types for the selected publication.  It is stored in the Count folder.
         </p>

    <hgroup class="title">
        <h2>Configuration Settings</h2>
    </hgroup>
    <h3>Environment</h3>
    <p>The database in which the application will connect is displayed as <b>Current Environment.</b>
        This variable is set in the web.config file under <b>AppSettings</b>, with the key <b>Environment</b>.
        The options for this variable are:
    </p>
        <ul>
            <li>Live</li>
            <li>Test</li>
            <li>Dev</li>
        </ul>

    <h3>Folder Paths</h3>
        <p>
            The folders in which the text files will be stored are stored in the web.config file under <b>AppSettings</b> with the following
            keys:
        </p>
            <ul>
                <li><b>CjonFolderPath</b></li>
                <li><b>ForumFolderPath</b></li>
                <li><b>ConnectFolderPath</b></li>
                <li><b>CountFolderPath</b></li>
            </ul>
        
        </div>       
    </section>
</asp:Content>