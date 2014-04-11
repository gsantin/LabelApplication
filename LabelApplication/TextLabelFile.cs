using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NLog;
using System.IO;

namespace LabelApplication
{
    public class CTextLabelFile
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string strFileName = string.Empty;
        private DataTable dtFileData;
        private string FILE_EXTENSION = ".txt";

        public CTextLabelFile() {}

        public CTextLabelFile(string strName, DataTable dtData)
        {
            strFileName = strName + FILE_EXTENSION;
            dtFileData = dtData.Copy();
        }

        public void exportDataToFile(string strFilePath)
        {
            int i = 0;
            StreamWriter sw = null;
            string delim = ",";
            string strFolderPathAndFileName = strFilePath + strFileName;

            try
            {
                sw = new StreamWriter(strFolderPathAndFileName, false);

                for (i = 0; i < dtFileData.Columns.Count - 1; i++)
                {
                    sw.Write(dtFileData.Columns[i].ColumnName + delim);
                }
                sw.Write(dtFileData.Columns[i].ColumnName);
                sw.WriteLine();

                foreach (DataRow row in dtFileData.Rows)
                {
                    object[] array = row.ItemArray;

                    for (i = 0; i < array.Length - 1; i++)
                    {
                        sw.Write(array[i].ToString() + delim);
                    }
                    sw.Write(array[i].ToString());
                    sw.WriteLine();
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

        public int getLabelCount()
        {
            return dtFileData.Rows.Count;
        }

        public string getLabelTableName()
        {
            return dtFileData.TableName;
        }
    }
}