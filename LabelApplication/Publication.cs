using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using NLog;

namespace LabelApplication
{
    public class CPublication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //These enumerations are to link the label counts to the counts listed in the count file.
        private enum CJON_CountFileRowsAndParameters { CJONConnectUSACount_CJON_USA, CjonOnlyUSACount_CJON_Only_USA, CjonConnectLiUSACount_CJON_LI_USA, 
                                                       CjonOnlyLiUSACount_CJON_Only_LI_USA, CjonConnectIntlCount_CJON_INTL };
        private enum FORUM_CountFileRowsAndParameters { ForumConnectUSACount_FORUM_USA, ForumOnlyUSACount_FORUM_Only_USA, 
                                                        ForumConnectLiUSACount_FORUM_LI_USA, ForumOnlyLiUSACount_FORUM_Only_LI, 
                                                        ForumConnectINTLCount_FORUM_INTL };
        private enum CONNECT_CountFileRowsAndParameters { ConnectOnlyUSACount_Connect_Only, ConnectOnlyLIUSACount_Connect_Only_LI, 
                                                          ConnectOnlyIntlCount_Connect_Only, ConnectOnlyLIIntlCount_Connect_Only_LI };

        private string strPublicationName = string.Empty;
        private string strPublicationIssue = string.Empty;
        private string strIssueMonth = string.Empty;
        private int iIssueYear = 0;
        private string strIssueDate = string.Empty;
        private string strFolderPath = string.Empty;
        private List<CTextLabelFile> lstTextLabelFiles;
        private DataTable dtLabelCountTable;

        private void setIssueDate()
        {
            switch (strIssueMonth)
            {
                case "January":
                    strIssueDate = "01/31/" + iIssueYear;
                    break;
                case "February":
                    if (DateTime.IsLeapYear(iIssueYear))
                    {
                        strIssueDate = "2/29/" + iIssueYear;
                    }
                    else
                    {
                        strIssueDate = "2/28/" + iIssueYear;
                    }
                    break;
                case "March":
                    strIssueDate = "03/31/" + iIssueYear;
                    break;
                case "April":
                    strIssueDate = "04/30/" + iIssueYear;
                    break;
                case "May":
                    strIssueDate = "05/31/" + iIssueYear;
                    break;
                case "June":
                    strIssueDate = "06/30/" + iIssueYear;
                    break;
                case "July":
                    strIssueDate = "07/31/" + iIssueYear;
                    break;
                case "August":
                    strIssueDate = "08/31/" + iIssueYear;
                    break;
                case "September":
                    strIssueDate = "09/30/" + iIssueYear;
                    break;
                case "October":
                    strIssueDate = "10/31/" + iIssueYear;
                    break;
                case "November":
                    strIssueDate = "11/30/" + iIssueYear;
                    break;
                case "December":
                    strIssueDate = "12/31/" + iIssueYear;
                    break;
                default:
                    strIssueMonth = string.Empty;
                    strIssueDate = string.Empty;
                    break;
            }
        }

        private void setIssueYear()
        {
            if ((strIssueMonth == "January") && (DateTime.Today.Month != 1))
            {
                iIssueYear =  DateTime.Today.Year + 1;
            }
            else
            {
                iIssueYear = DateTime.Today.Year;
            }
        }

        public CPublication() {}

        public CPublication(string strName, string strMonth, string strFolderPath)
        {
            strPublicationName = strName;
            strIssueMonth = strMonth;
            this.strFolderPath = strFolderPath;
            setIssueYear();
            setIssueDate();
            lstTextLabelFiles = new List<CTextLabelFile>();
            strPublicationIssue = strName + " - " + strIssueMonth + " " + iIssueYear.ToString();
        }

        public void createTextFiles()
        {
            foreach (CTextLabelFile txtFile in lstTextLabelFiles)
            {
                txtFile.exportDataToFile(strFolderPath);
            }
        }

        public string getPublicationName()
        {
            return strPublicationName;
        }

        public string getIssueDate()
        {
            return strIssueDate;
        }

        public DataTable getCountTable()
        {
            return dtLabelCountTable;
        }

        public string getPublicationIssue()
        {
            return strPublicationIssue;
        }

        public void addToTextFileCollection(DataTable dtLabelData)
        {
            CTextLabelFile cTextLabelFile = new CTextLabelFile(dtLabelData.TableName, dtLabelData);
            lstTextLabelFiles.Add(cTextLabelFile);
        }

        public void setLabelCountTable()
        {
            dtLabelCountTable = new DataTable();
            string tstrParameterName = string.Empty;
            int tiCountFileIndex = 0;
            string tstrCountFileRowName = string.Empty;
          
            //Create the columns for the DataTable
            dtLabelCountTable.Columns.Add("ParameterName", typeof(String));
            dtLabelCountTable.Columns.Add("LabelCount", typeof(int));
            dtLabelCountTable.Columns.Add("CountFileRowNames", typeof(String));
            dtLabelCountTable.Columns.Add("CountFileRowIndex", typeof(int));

            DataRow drParameterRow;

            foreach (CTextLabelFile txtFile in lstTextLabelFiles)
            {
                drParameterRow = dtLabelCountTable.NewRow();
                tstrParameterName = txtFile.getLabelTableName().Replace("_", "") + "Count";
               
                drParameterRow["ParameterName"] = tstrParameterName;
                drParameterRow["LabelCount"] = txtFile.getLabelCount();

                //Use the enumerated list to get the value of the row names for the count file
                getCountFileRowParameterValues(tstrParameterName, ref tiCountFileIndex, ref tstrCountFileRowName);

                drParameterRow["CountFileRowNames"] = tstrCountFileRowName;
                drParameterRow["CountFileRowIndex"] = tiCountFileIndex;

                dtLabelCountTable.Rows.Add(drParameterRow);

                //Clear the row name and index
                tstrCountFileRowName = string.Empty;
                tiCountFileIndex = 0;
            }
        }

        private void getCountFileRowParameterValues(string strParameterName, ref int iFileIndex, ref string strRowName)
        {
            string tstrRowValue = string.Empty;
            int tiUnderscoreIndex = 0;
            //This variable will track the index of the row so that creating the count file is easier  
            int tiRowIndex = 1;

            string enumType = "LabelApplication.CPublication+" + strPublicationName.ToUpper() + "_CountFileRowsAndParameters";
            Type parameterEnum = Type.GetType(enumType);
            
            //Iterate thru the enumerated list, depending on the publication name to find the name of the associated row name
            //that will be used in the count text file.
            foreach (string strValue in Enum.GetNames(parameterEnum))
            {
                //Get the first part of the enumerated value
                tiUnderscoreIndex = strValue.IndexOf("_");
                tstrRowValue = strValue.Substring(0, tiUnderscoreIndex);

                //Compare the first part of the enumeration value with the parameter name
                //If it is a match, this is a count that will be stored in the count file
                //Because the enumeration order is the same order that the count values are to be written to the count file,
                //write the row index to the iFileIndex. This value will be stored in the data table also.
                if (tstrRowValue.Equals(strParameterName, StringComparison.OrdinalIgnoreCase))
                {
                    //Get the second half of the parameter name to get the row name in the count file
                    strRowName = strValue.Substring(tiUnderscoreIndex + 1);
                    //Adjust the enum name so that the row name is correctly formatted with spaces instead of underscores between words.
                    strRowName = strRowName.Replace("_", " ");
                    iFileIndex = tiRowIndex;
                    break;
                }
                tiRowIndex++;
            }
        }

        public void createCountFile(string strFolderPath)
        {
            string tstrFileName = string.Empty;
            int i = 0;
            StreamWriter sw = null;
            string delim = ",";
            int tiRowBreak = 1;

            //Set the file name
            tstrFileName = strPublicationIssue.Replace("-", "_");

            //Set the file path
            string strFolderPathAndFileName = strFolderPath + tstrFileName + ".txt";

            try
            {
                sw = new StreamWriter(strFolderPathAndFileName, false);

                //Get the data from the label count table
                DataTable tdtFileData = createCountFileDataTable();

                //For Connect publications, add a TOTAL row.
                if (strPublicationName.Equals("Connect", StringComparison.OrdinalIgnoreCase))
                {
                    addConnectTotalToDataTable(ref tdtFileData);
                }

                //Write the file
                sw.Write(strPublicationIssue);
                sw.WriteLine();
                sw.WriteLine();

                sw.Write("USA");
                sw.WriteLine();

                foreach (DataRow row in tdtFileData.Rows)
                {
                    object[] array = row.ItemArray;

                    for (i = 0; i < array.Length - 1; i++)
                    {
                        sw.Write(array[i].ToString() + delim);
                    }
                    sw.Write(array[i].ToString());
                    sw.WriteLine();
                    if (tiRowBreak == 2)
                    {
                        sw.WriteLine();
                        sw.Write("INTL");
                        sw.WriteLine();
                    }
                    tiRowBreak++;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                return;
            }
            finally
            {
                sw.Close();
            }
        }

        private DataTable createCountFileDataTable()
        {
            DataTable tdtCountFileData = new DataTable();
            tdtCountFileData.Columns.Add("RowName", typeof(String));
            tdtCountFileData.Columns.Add("LabelCount", typeof(int));

            DataRow[] tdrRows;
            DataRow tdrCountRow;
            tdrRows = dtLabelCountTable.Select("[CountFileRowIndex] > 0", "[CountFileRowIndex] ASC");
            foreach (DataRow row in tdrRows)
            {
                tdrCountRow = tdtCountFileData.NewRow();
                tdrCountRow["RowName"] = row["CountFileRowNames"];
                tdrCountRow["LabelCount"] = row["LabelCount"];
                tdtCountFileData.Rows.Add(tdrCountRow);
            }

            return tdtCountFileData;
        }

        private void addConnectTotalToDataTable(ref DataTable dtCurrentTable)
        {
            DataRow tdrTotalRow;
           
            Object tobjTotalLabels = dtCurrentTable.Compute("Sum(LabelCount)", "");
            int tiLabelCount = Convert.ToInt32(tobjTotalLabels);

            tdrTotalRow = dtCurrentTable.NewRow();
            tdrTotalRow["RowName"] = "TOTAL:";
            tdrTotalRow["LabelCount"] = tiLabelCount;
            dtCurrentTable.Rows.Add(tdrTotalRow);
        }
    }
}