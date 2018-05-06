using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoEntityFrameworkApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public static string ConvertToDate(string strToDate)
        {
            string a = strToDate;
            String[] str = new String[3];
            str = a.Split('/');
            String dutydate = String.Empty;
            dutydate = str[0];
            str[0] = str[1];
            str[1] = dutydate;
            // dutydate = str[0] + "/" + str[1] + "/" + str[2];
            dutydate = str[0] + "/" + str[1] + "/" + str[2];
            return dutydate;
        }
        public static string ConvertFromDate(string strFromDate)
        {
            string a = strFromDate;
            String[] str = new String[3];
            str = a.Split('/');
            String dutydate = String.Empty;
            dutydate = str[0];
            str[0] = str[1];
            str[1] = dutydate;
            // dutydate = str[0] + "/" + str[1] + "/" + str[2];
            dutydate = str[0] + "/" + str[1] + "/" + str[2];
            return dutydate;
        }
        public DataTable Readexcel(string Fname)
        {
            String excelcolumns = string.Empty;
            bool isExcelvalid = true;
            using (DataSet Dtset = new DataSet())
            {
                String xlsconnstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;Importmixedtypes=text;typeguessrows=0;\"";
                xlsconnstring = string.Format(CultureInfo.CurrentCulture, xlsconnstring, Fname);

                using (System.Data.OleDb.OleDbConnection Myconnection = new System.Data.OleDb.OleDbConnection(xlsconnstring))
                {
                    Myconnection.Open();
                    DataTable Sheets = Myconnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    String SheetName = Sheets.Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtcolumns = Myconnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);

                    var columnCount = (from item in dtcolumns.AsEnumerable()
                                       where item["TABLE_NAME"].ToString() == SheetName.ToString()
                                       select item).ToList().Count();
                    if (columnCount != 7 && columnCount != 10)
                    {
                        TempData["excelinvalid"] = "1";
                        isExcelvalid = false;
                    }
                    if (isExcelvalid)
                    {
                        DataTable dataTable = new DataTable();
                        excelcolumns = "f1,f2,f3,f4,f5,f6,f7";
                        String query = String.Format("SELECT {0} FORM [{1}]", excelcolumns, Sheets.Rows[0]["TABLE_NAME"].ToString());
                        using (var MyCommand = new System.Data.OleDb.OleDbDataAdapter(query, Myconnection))
                        {
                            MyCommand.Fill(dataTable);
                            Dtset.Tables.Add(dataTable);
                            Myconnection.Close();
                        }
                        DataTable dtSheet = Dtset.Tables[0];

                        for (int columnIndex = 0; columnIndex < dtSheet.Columns.Count; columnIndex++)
                        {
                            if (!string.IsNullOrEmpty(dtSheet.Rows[0][columnIndex].ToString()))
                            {
                                dtSheet.Columns[columnIndex].ColumnName = dtSheet.Rows[0][columnIndex].ToString().Replace("?", "").Trim();
                            }
                        }
                        dtSheet.Rows[0].Delete();
                        dtSheet.Columns[6].ColumnName = "certificates";
                        dtSheet.AcceptChanges();
                        for (int rowindex = dtSheet.Rows.Count - 1; rowindex >= 0; rowindex--)
                        {
                            bool isEmptyRow = true;
                            for (int columnIndex = 0; columnIndex < dtSheet.Columns.Count; columnIndex++)
                            {
                                if (string.IsNullOrEmpty(dtSheet.Rows[rowindex][columnIndex].ToString()))
                                {
                                    isEmptyRow = false;
                                }
                                else if (dtSheet.Rows[rowindex][columnIndex].ToString().Trim() == String.Empty)
                                {
                                }
                            }
                            if (isEmptyRow)
                            {
                                dtSheet.Rows[rowindex].Delete();
                                dtSheet.AcceptChanges();
                            }
                        }
                        return dtSheet;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}