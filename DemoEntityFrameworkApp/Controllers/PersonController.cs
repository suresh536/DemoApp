using DemoEntityFrameworkApp.Common;
using DemoEntityFrameworkApp.DataAccess;
using DemoEntityFrameworkApp.Models;
using DemoEntityFrameworkApp.Services;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DemoEntityFrameworkApp.Controllers
{
    public class PersonController : Controller
    {
        DBCommon objCommon = new DBCommon();
        private readonly IpersonService _personService;
        private readonly IUSP_GET_ALL_PERSONSservice _USP_GET_ALL_PERSONSservice;
        public PersonController(IpersonService PersonService, IUSP_GET_ALL_PERSONSservice USP_GET_ALL_PERSONSservice)
        {
            this._personService = PersonService;
            this._USP_GET_ALL_PERSONSservice = USP_GET_ALL_PERSONSservice;
        }

        private string ConvertDate(string dates)
        {
            string a = dates;
            String[] str = new String[3];
            str = a.Split('/');
            String date = String.Empty;
            date = str[0];
            str[0] = str[1];
            str[1] = date;
            date = str[0] + "/" + str[1] + "/" + str[2];
            return date;
        }

        // GET: Person
        public ActionResult Index()
        {
            var data = _personService.GetAllPersons();
            return View(data);
        }

        public void SampleSendMail()
        {
            MailMessage objMail = new MailMessage();
            objMail.From = new MailAddress("frommail");
            string emails = string.Empty;
            emails = string.Join(";", string.Empty);
            objMail.Subject = "subject";
            objMail.Body = "this is the body <html><body></body></html>";
            foreach (var item in emails.Split(';'))
            {
                objMail.To.Add(new MailAddress(item));
            }
            objMail.IsBodyHtml = true;
            SendMail(objMail);


        }

        public void SendMail(MailMessage objMailMessage)
        {
            using (SmtpClient client = new SmtpClient())
            {
                string mailhost = string.Empty;
                mailhost = string.IsNullOrEmpty(ConfigurationManager.AppSettings["email"].ToStr()) ? "" : ConfigurationManager.AppSettings["email"].ToStr();
                client.Host = mailhost;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = false;
                client.UseDefaultCredentials = true;
                client.Send(objMailMessage);
            }
        }

        public ActionResult GetAllPersonsWithPaging()
        {
            pagedlist objpagedlist = new pagedlist()
            {
                draw = Request.Form.GetValues("draw").FirstOrDefault().ToInt32(),
                start = Convert.ToInt32(Request.Params["start"]),
                length = Request.Form.GetValues("length").FirstOrDefault().ToInt32(),
                search = Request.Form.GetValues("search[value]").FirstOrDefault().ToStr(),
                sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault()
            };
            Dictionary<object, object> dic = new Dictionary<object, object>();

            dic.Add("@PageNo", objpagedlist.start);
            dic.Add("@PageSize", objpagedlist.length);
            if (string.IsNullOrEmpty(objpagedlist.search))
            {
                dic.Add("@Search", DBNull.Value);
            }
            else
            {
                dic.Add("@Search", objpagedlist.search);
            }
            dic.Add("@SortColumn", objpagedlist.sortColumn);
            dic.Add("@SortOrder", objpagedlist.sortColumnDir);

            Hashtable ht = new Hashtable();
            ht.Add("PageNo", objpagedlist.start == 0 ? 1 : objpagedlist.start);
            ht.Add("PageSize", objpagedlist.length);
            ht.Add("Search", objpagedlist.search == null ? DBNull.Value : (object)objpagedlist.search);
            ht.Add("SortColumn", objpagedlist.sortColumn);
            ht.Add("SortOrder", objpagedlist.sortColumnDir);
            DataTable dt = objCommon.TableforSP("USP_GET_ALL_PERSONS", ht);
            List<USP_GET_ALL_PERSONS> list = new List<USP_GET_ALL_PERSONS>();
            list = (from DataRow row in dt.Rows

                    select new USP_GET_ALL_PERSONS()
                    {
                        LastName = row["LastName"].ToString(),
                        FirstName = row["FirstName"].ToString(),
                        HireDate = row["HireDate"].ToStr(),
                        EnrollmentDate = row["EnrollmentDate"].ToString(),
                        Salary = row["Salary"].ToString(),
                        totalcount = row["totalcount"].ToInt32()
                    }).ToList();

            //var list = _USP_GET_ALL_PERSONSservice.GetAllPersonsFromDB(new USP_GET_ALL_PERSONS(), dic);
            int totalrecords = list.Select(x => x.totalcount).FirstOrDefault();

            return Json(new { draw = objpagedlist.draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RdLcExportExcel(string type)
        {
            byte[] renderedBytes = Report(type);
            if (type == "Excel")
            {
                return File(renderedBytes, "application/vnd.ms-excel", "Report.xls");
            }
            else
            {
                return File(renderedBytes, "application/vnd.ms-excel", "Report.pdf");
            }
        }

        public byte[] Report(string type)
        {
            Hashtable ht = new Hashtable();
            using (DataSet ds = objCommon.DataSetforSP("USP_GET_ALL_PERSONS_REPORT", ht))
            {
                using (LocalReport localreport = new LocalReport())
                {
                    localreport.ReportPath = System.Web.HttpContext.Current.Server.MapPath("~/Reports/SampleReport.rdlc");
                    ReportDataSource reportdatasource = new ReportDataSource("dsPerson", ds.Tables[0]);
                    localreport.DataSources.Add(reportdatasource);
                    string reporttype = type;
                    string mimetype;
                    string encoding;
                    string filenameextension;
                    string deviceinfo = null;

                    //"<DeviceInfo>" +
                    //"  <OutputFormat>" + reporttype + "</OutputFormat>" +
                    //"  <PageWidth>8.15in</PageWidth>" +
                    //"  <PageHeight>11in</<PageHeight>" +
                    //"  <MarginTop>0.5in</MarginTop>" +
                    //"  <MarginLeft>1in</MarginLeft>" +
                    //"  <MarginRight>1in</MarginRight>" +
                    //"  <MarginBottom>0.5in</MarginBottom>" +
                    //"</DeviceInfo>";
                    Warning[] warnings;
                    string[] streams;
                    byte[] renderBytes = null;
                    renderBytes = localreport.Render(reporttype, deviceinfo, out mimetype, out encoding, out filenameextension, out streams, out warnings);
                    return renderBytes;
                }
            }
        }


    }
}