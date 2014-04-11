using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Security.Principal;

namespace LabelApplication
{
    public class DAL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private const int INSERT_TIME_OUT = 400;
        private const int SELECT_TIME_OUT = 200;
        private const int LABEL_INDEX = 8;

        private const string INSERT_COUNT_STORED_PROCEDURE_NAME_CJON = "ONS_LBL_InsertCjonConnect_Count";
        private const string INSERT_COUNT_STORED_PROCEDURE_NAME_FORUM = "ONS_LBL_InsertForumConnect_Count";
        private const string INSERT_COUNT_STORED_PROCEDURE_NAME_CONNECT = "ONS_LBL_InsertConnect_Count";
        private const string PARAMETER_AT_SIGN = "@";
        private const string PREFIX_ENUM_NAME = "LabelApplication.DAL+";

        public enum StoredProcedureNamesInsertLabelDataCJON { ONS_LBL_CJON_INTL, ONS_LBL_CJON_USA, ONS_LBL_CJON_LI_INTL, ONS_LBL_CJON_LI_USA };
        private enum StoredProcedureNamesInsertLabelDataFORUM { ONS_LBL_FORUM_INTL, ONS_LBL_FORUM_USA, ONS_LBL_FORUM_LI_INTL, ONS_LBL_FORUM_LI_USA };
        private enum StoredProcedureNamesInsertLabelDataCONNECT { ONS_LBL_NEWS_INTL_SP, ONS_LBL_NEWS_USA_SP, ONS_LBL_NEWS_LI_INTL_SP, ONS_LBL_NEWS_LI_USA_SP };

        private enum StoredProcedureNamesGetLabelDataCJON { ONS_LBL_CJON_NEWS_USA, ONS_LBL_CJON_NEWS_INTL, ONS_LBL_CJON_NEWS_LI_USA, ONS_LBL_CJON_NEWS_LI_INTL,
                                                            ONS_LBL_CJON_ONLY_USA, ONS_LBL_CJON_ONLY_INTL, ONS_LBL_CJON_ONLY_LI_USA, ONS_LBL_CJON_ONLY_LI_INTL};

        private enum StoredProcedureNamesGetLabelDataFORUM { ONS_LBL_FORUM_NEWS_USA, ONS_LBL_FORUM_NEWS_INTL, ONS_LBL_FORUM_NEWS_LI_USA, ONS_LBL_FORUM_NEWS_LI_INTL,
                                                             ONS_LBL_FORUM_ONLY_USA, ONS_LBL_FORUM_ONLY_INTL, ONS_LBL_FORUM_ONLY_LI_USA, ONS_LBL_FORUM_ONLY_LI_INTL};

        private enum StoredProcedureNamesGetLabelDataCONNECT { ONS_LBL_NEWS_ONLY_USA, ONS_LBL_NEWS_ONLY_INTL, ONS_LBL_NEWS_ONLY_LI_USA, ONS_LBL_NEWS_ONLY_LI_INTL };

        private enum ParameterNamesFORUM { ForumConnectUSACount, ForumConnectINTLCount, ForumConnectLiUSACount, ForumConnectLiINTLCount,
                                           ForumOnlyUSACount, ForumOnlyINTLCount, ForumOnlyLiUSACount, ForumOnlyLiIntlCount};

        private enum ParameterNamesCONNECT { ConnectOnlyUSACount, ConnectOnlyIntlCount, ConnectOnlyLIUSACount, ConnectOnlyLIIntlCount };

        private static string strDBConnectionName = "LabelApp";
        private static string strDBConnection = string.Empty;
        private static SqlConnection sqlConnection;
        private static string strCurrentEnvironmentSetting = string.Empty;

        private static SqlCommand createSQLCommand(string strStoredProcedureName, int iTimeOut)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.CommandTimeout = iTimeOut;
                cmd.CommandText = strStoredProcedureName;

                return cmd;
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        private static void insertLabelData(string strIssueDate, string strStoredProcedureName)
        {
            setConnectionString();
            SqlCommand cmdInsert = createSQLCommand(strStoredProcedureName, INSERT_TIME_OUT);
            cmdInsert.Parameters.Add("@Issuedate", SqlDbType.Char, 10).Value = strIssueDate;

            try
            {
                sqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string strMsg = "Stored Procedure: " + strStoredProcedureName + " " + ex.Message;
                logger.ErrorException(strMsg, ex);
                throw;
            }
            finally
            {
                sqlConnection.Close();
                cmdInsert = default(SqlCommand);
            }         
        }

        private static void insertCountData(CPublication pub, string strStoredProcedureName, DataTable dtCountData)
        {
            setConnectionString();
            SqlCommand cmdInsert = createSQLCommand(strStoredProcedureName, INSERT_TIME_OUT);
            try
            {
                string tstrInsertedBy = WindowsIdentity.GetCurrent().Name.ToString().Replace("ONSMASTER\\", "");

                //Set the parameter list and values depending on the publication
                cmdInsert.Parameters.Add("@PublicationName", SqlDbType.VarChar).Value = pub.getPublicationIssue();

                switch (pub.getPublicationName())
                {
                    case "CJON":
                        addTableParameters(ref cmdInsert, dtCountData);
                        addDefaultParameters("CONNECT", ref cmdInsert, 0);
                        break;

                    case "Forum":
                        addTableParameters(ref cmdInsert, dtCountData);
                        addDefaultParameters("CONNECT", ref cmdInsert, -1);
                        break;

                    case "Connect":
                        addDefaultParameters("FORUM", ref cmdInsert, -1);
                        addTableParameters(ref cmdInsert, dtCountData);
                        break;
                    default: break;
                }
                cmdInsert.Parameters.Add("@InsertedBy", SqlDbType.VarChar).Value = tstrInsertedBy;

                sqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
            finally
            {
                sqlConnection.Close();
                cmdInsert = default(SqlCommand);
            }         
        }

        private static void addTableParameters(ref SqlCommand sqlCmd, DataTable dtCountData)
        {
            try
            {
                string tstrParameterName = string.Empty;
                int tiCount = 0;

                foreach (DataRow row in dtCountData.Rows)
                {
                    tstrParameterName = PARAMETER_AT_SIGN + row["ParameterName"].ToString();
                    tiCount = (int)row["LabelCount"];
                    sqlCmd.Parameters.Add(tstrParameterName, SqlDbType.Int).Value = tiCount;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        private static void addDefaultParameters(string strName, ref SqlCommand sqlCmd, int iCount)
        {
            try
            {
                string tstrParameterName = string.Empty;
                string enumType = PREFIX_ENUM_NAME + "ParameterNames" + strName;
                Type parameterEnum = Type.GetType(enumType);

                foreach (string strParameterName in Enum.GetNames(parameterEnum))
                {
                    tstrParameterName = PARAMETER_AT_SIGN + strParameterName;
                    sqlCmd.Parameters.Add(tstrParameterName, SqlDbType.Int).Value = iCount;
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        private static DataTable getLabelData(string strStoredProcedureName)
        {
            setConnectionString();
            SqlCommand cmdSelect = createSQLCommand(strStoredProcedureName, SELECT_TIME_OUT);
            DataSet dsLabelData = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

            try
            {    
                string tstrTableName = setLabelTableName(strStoredProcedureName);

                sqlDataAdapter.SelectCommand = cmdSelect;
                sqlDataAdapter.Fill(dsLabelData, tstrTableName);
                int temp = dsLabelData.Tables[tstrTableName].Rows.Count;

                return dsLabelData.Tables[tstrTableName];
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
            finally
            {
                sqlConnection.Close();
                cmdSelect = default(SqlCommand);
            }
        }

        private static string setLabelTableName(string strStoredProcedureName)
        {
            string strTableName = string.Empty;
            string tSPSubstring = string.Empty;

            //Remove the ONS_LBL_ substring - start at index 7
            tSPSubstring = strStoredProcedureName.Substring(LABEL_INDEX);

            //Replace 'NEWS' with 'CONNECT'
            strTableName = tSPSubstring.Replace("NEWS", "CONNECT");
            return strTableName;
        }

        private static void setConnectionString()
        {
            try
            {
                if (sqlConnection == null)
                {
                    strDBConnection = ConfigurationManager.ConnectionStrings[strCurrentEnvironmentSetting].ConnectionString;
                    sqlConnection = new SqlConnection(strDBConnection);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        internal static void setCurrentEnvironment(string strEnvironment)
        {
            strCurrentEnvironmentSetting = strDBConnectionName + strEnvironment;
        }

        internal static void insertLabels(CPublication pub)
        {
            try
            {
                //All of the publications are require the 'Connect' tables to be updated with the latest labels.
                //So, update these tables first, regardless of the publication selected.
                foreach(string insertConnectSP in Enum.GetNames(typeof(StoredProcedureNamesInsertLabelDataCONNECT)))
                {
                    insertLabelData(pub.getIssueDate(), insertConnectSP);
                }

                //Only update the CJONFORUM tables if one of these publications were selected.
                if ((pub.getPublicationName().Equals("CJON")) || pub.getPublicationName().Equals("Forum"))
                {
                    //Create the string that identifies the enumerated type
                    string enumType = PREFIX_ENUM_NAME + "StoredProcedureNamesInsertLabelData" + pub.getPublicationName().ToUpper();
                    Type insertEnum = Type.GetType(enumType);

                    foreach (string insertSP in Enum.GetNames(insertEnum))
                    {
                        insertLabelData(pub.getIssueDate(), insertSP);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        internal static void getLabels(CPublication pub)
        {
            try
            {
                //Create the string that identifies the enumerated type
                string enumType = PREFIX_ENUM_NAME + "StoredProcedureNamesGetLabelData" + pub.getPublicationName().ToUpper();
                Type getEnum = Type.GetType(enumType);
                DataTable dtLabelData;

                //Go thru the list of stored procedures to get the label data, populate the data in a DataTable and add to collection.
                foreach (string getSP in Enum.GetNames(getEnum))
                {
                    dtLabelData = getLabelData(getSP);
                    pub.addToTextFileCollection(dtLabelData);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        internal static void insertLabelCount(CPublication pub, DataTable dtCountData)
        {
            try
            {
                //Determine which stored procedure to use
                string insertConstantName = "INSERT_COUNT_STORED_PROCEDURE_NAME_" + pub.getPublicationName().ToUpper();
                Type thisClass = typeof(DAL);
                FieldInfo insertStoredProcedureConstant = thisClass.GetField(insertConstantName, BindingFlags.NonPublic | BindingFlags.Static);
                string tstrInsertStoredProcedure = insertStoredProcedureConstant.GetValue(null).ToString();

                //insert into count table
                insertCountData(pub, tstrInsertStoredProcedure, dtCountData);
            }
            catch (Exception ex)
            {
                logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

    }
}