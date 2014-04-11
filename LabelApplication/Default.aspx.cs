using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using System.Configuration;
using System.Drawing;

namespace LabelApplication
{
    public partial class _Default : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string strCurrentEnvironment = string.Empty;
        public string CurrentEnvironment
        {
            get
            {
                // Returns the current environment setting.
                return ConfigurationManager.AppSettings["Environment"];
            }

        }
      
        private string strCjonFolderPath = string.Empty;
        public string CjonFolderPath
        {
            get
            {
                // Returns the folder path for Cjon folder.
                return ConfigurationManager.AppSettings["CjonFolderPath"];
            }
        }
        
        private string strForumFolderPath = string.Empty;
        public string ForumFolderPath
        {
            get
            {
                // Returns the folder path for Cjon folder.
                return ConfigurationManager.AppSettings["ForumFolderPath"];
            }
        }

        private string strConnnectFolderPath = string.Empty;
        public string ConnnectFolderPath
        {
            get
            {
                // Returns the folder path for Cjon folder.
                return ConfigurationManager.AppSettings["ConnectFolderPath"];
            }
        }
        
        private string strCountFolderPath = string.Empty;
        public string CountFolderPath
        {
            get
            {
                // Returns the folder path for Cjon folder.
                return ConfigurationManager.AppSettings["CountFolderPath"];
            }
        }

        private enum PublicationType {CJON, Forum, Connect};
        private string strPublicationSelected = PublicationType.CJON.ToString();
        private PublicationType ePubSelected = PublicationType.CJON;
        private CPublication cPub;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtBoxEnvironment.Text = CurrentEnvironment;
                    DAL.setCurrentEnvironment(CurrentEnvironment);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                Response.Redirect("~/Error.aspx");
            }
        }

        private string getTextFileFolderPath(PublicationType pubType)
        {
            switch (pubType)
            {
                case PublicationType.CJON:
                    return CjonFolderPath;  

                case PublicationType.Forum:
                    return ForumFolderPath;
                   
                case PublicationType.Connect:
                    return ConnnectFolderPath;
                   
                default:
                    return string.Empty;
            }
        }

        private void setPublicationSelection()
        {
            if (rboJCON.Checked)
            {
                strPublicationSelected = PublicationType.CJON.ToString();
                ePubSelected = PublicationType.CJON;
            }
            else if (rboForum.Checked)
            {
                strPublicationSelected = PublicationType.Forum.ToString();
                ePubSelected = PublicationType.Forum;
            }
            else 
            {
                strPublicationSelected = PublicationType.Connect.ToString();
                ePubSelected = PublicationType.Connect;
            }
        }

        protected void btnCreateFiles_Click(object sender, EventArgs e)
        {
            string tstrIssueMonth = string.Empty;
            string tstrFolderPath = string.Empty;

            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(string), "bindButton", "bindButton();", true);

                //Make sure this variable is set
                DAL.setCurrentEnvironment(CurrentEnvironment);

                //Set Publication Type
                setPublicationSelection();

                //Get Issue Month - add 1 to the indext selected
                tstrIssueMonth = ddlMonth.SelectedValue.ToString();

                //Get the folder path from the publication selected
                tstrFolderPath = getTextFileFolderPath(ePubSelected);

                //Create the Publication Object
                cPub = new CPublication(strPublicationSelected, tstrIssueMonth, tstrFolderPath);

                //Insert label data into database for the selected publication
                DAL.insertLabels(cPub);

                //Get the label data and populate the data tables
                DAL.getLabels(cPub);

                //Create the text files from the collection of CTextLabelFiles
                cPub.createTextFiles();

                //Create the table of label counts
                cPub.setLabelCountTable();

                //Insert counts in database
                DAL.insertLabelCount(cPub, cPub.getCountTable());

                //Create count file from table counts
                cPub.createCountFile(CountFolderPath);

                ProgressUpdatePanel.ContentTemplateContainer.Controls.Add(lblCaption);
                ProgressUpdatePanel.Update();
                lblCaption.ForeColor = Color.Green;
                lblCaption.Text = "Processing completed...files have been created.";
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                Response.Redirect("~/Error.aspx");
            }
        }
    }
}