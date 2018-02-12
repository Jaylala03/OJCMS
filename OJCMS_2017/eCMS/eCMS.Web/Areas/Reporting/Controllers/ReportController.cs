using ClosedXML.Excel;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.Web.Controllers;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using eCMS.Web.Reports.PDF;
using eCMS.DataLogic.Models.Report;
using System;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using EasySoft.Helper;

namespace eCMS.Web.Areas.Reporting.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportRepository reportRepository;
        public ReportController(IWorkerRepository workerRepository,
          IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            , IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository
            , IReportRepository reportRepository)
            : base(workerroleactionpermissionRepository, workerroleactionpermissionnewRepository)
        {
            this.workerRepository = workerRepository;
            this.reportRepository = reportRepository;
        }

        // GET: Reporting/Report
        [WorkerAuthorize]
        public ActionResult Index()
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, Constants.Actions.ReportsCSV, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            return View();
        }

        // GET: Reporting/CashDashBoard
        [WorkerAuthorize]
        [HttpGet]
        public ActionResult CaseDashBoard()
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, Constants.Actions.CaseDashboard, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            return View();
        }

        // GET: Reporting/ListOfIssues
        [WorkerAuthorize]
        [HttpGet]
        public ActionResult ListOfIssues()
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, Constants.Actions.ListOfIssues, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            ListOfIssuesVM model = new ListOfIssuesVM();
            return View(model);
        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult GenerateCSV(int offset = 0, int limit = 0)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, Constants.Actions.ReportsCSV, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            string constr = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;
            string customquery = "SELECT  'SELECT  * FROM [' + TABLE_NAME + '] ' FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'  ";
            string query = "";
            List<string> tables = new List<string>();

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(customquery))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                tables.Add(dt.Rows[i]["Column1"].ToString().Split(new char[] { '[', ']' })[1].ToString());
                                query = query + dt.Rows[i]["Column1"].ToString();
                                query += (i < dt.Rows.Count) ? ";" : string.Empty;
                            }
                        }

                    }
                }
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                int counter = 0;
                                foreach (DataTable dt in ds.Tables)
                                {
                                    if (tables[counter].ToString() == "Case")
                                    {
                                        DataColumnCollection columns = dt.Columns;

                                        if (columns.Contains("FirstName"))
                                        {
                                            dt.Columns.Remove("FirstName");
                                        }
                                        if (columns.Contains("LastName"))
                                        {
                                            dt.Columns.Remove("LastName");
                                        }
                                        if (columns.Contains("EmailAddress"))
                                        {
                                            dt.Columns.Remove("EmailAddress");
                                        }
                                        if (columns.Contains("Phone"))
                                        {
                                            dt.Columns.Remove("Phone");
                                        }
                                    }

                                    dt.TableName = Truncate(tables[counter].ToString(), 30);

                                    //Add DataTable as Worksheet.
                                    wb.Worksheets.Add(dt);
                                    counter = counter + 1;
                                }

                                //Export the Excel file.
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment;filename=OneJamat.xlsx");
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }


        [WorkerAuthorize]
        //[HttpPost]
        public CrystalReportPdfResult CaseDashboardPDF(CaseDashboardRptInput model)
        {
            DataTable table = reportRepository.CaseDashboard(model);
            //List<CaseDashboardrpt> table = reportRepository.CaseDashboardExcel(model);
            string reportPath = Path.Combine(Server.MapPath("~/Reports"), "CaseDashboardNew.rpt");
            return new CrystalReportPdfResult(reportPath, table);
        }

        [WorkerAuthorize]
        //[HttpPost]
        public ActionResult CaseDashboardExcel(CaseDashboardRptInput model)
        {
            List<CaseDashboardrpt> table = reportRepository.CaseDashboardExcel(model);
            //table.Columns.Remove("Region");
            //Closed XML
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable as Worksheet.
                using (var ws = wb.AddWorksheet("CaseDashboard"))
                {
                    int headcount = 1;
                    ws.Cell("B1").SetValue("Families With Enrollment Date >=January 1, 2015");
                    ws.Cell("B1").Style.Alignment.WrapText = true;
                    ws.Cell("B1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("B1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell("B1").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ws.Cell("B1").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ws.Cell("B1").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Cell("B1").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ws.Range("B1:E1").Merge();

                    ws.Cell("F1").SetValue("Family Members Entered in OJCMS -Data Quality Context");
                    //ws.Cell("F1").Style.Alignment.WrapText = true;
                    ws.Cell("F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("F1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell("F1").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ws.Cell("F1").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ws.Cell("F1").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Cell("F1").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ws.Range("F1:Q1").Merge();

                    ws.Cell("R1").SetValue("Active QoL Families");
                    //ws.Cell("R1").Style.Alignment.WrapText = true;
                    ws.Cell("R1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("R1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell("R1").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ws.Cell("R1").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ws.Cell("R1").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Cell("R1").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ws.Range("R1:W1").Merge();

                    ws.Cell("X1").SetValue("Family Status  - case overview / case progress");
                    //ws.Cell("X1").Style.Alignment.WrapText = true;
                    ws.Cell("X1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell("X1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell("X1").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    ws.Cell("X1").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    ws.Cell("X1").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Cell("X1").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ws.Range("X1:AU1").Merge();

                    ws.Rows(headcount, headcount).Style.Fill.BackgroundColor = XLColor.FromArgb(91, 155, 213);
                    ws.Rows(headcount, headcount).Style.Font.Bold = true;
                    ws.Rows(headcount, headcount).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Rows(headcount, headcount).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ws.Row(1).Height = 50;
                    headcount += 1;

                    string[] columnsheader = GetColumnHeader();
                    ws.Column(1).Width = 15;
                    for (int clcnt = 0; clcnt < columnsheader.Length; clcnt++)
                    {
                        // Adding HeaderRow.
                        ws.Cell(headcount, clcnt + 1).SetValue(columnsheader[clcnt]);
                        ws.Cell(headcount, clcnt + 1).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        ws.Cell(headcount, clcnt + 1).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        ws.Cell(headcount, clcnt + 1).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        ws.Cell(headcount, clcnt + 1).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        ws.Column(clcnt + 2).Width = 4;
                    }
                    //Alignment align = new Alignment();
                    //align.TextRotation.Value = (UInt32Value)90U;
                    //CellFormat cellFormat1 = new CellFormat(){ 
                    //    NumberFormatId = (UInt32Value)0U, 
                    //    FontId = (UInt32Value)0U, 
                    //    FillId = (UInt32Value)0U, 
                    //    BorderId = (UInt32Value)0U, 
                    //    FormatId = (UInt32Value)0U, 
                    //    ApplyAlignment = true };
                    //cellFormat1.Append(align);

                    ws.Rows(headcount, headcount).Style.Fill.BackgroundColor = XLColor.FromArgb(91, 155, 213);
                    ws.Rows(headcount, headcount).Style.Font.Bold = true;
                    ws.Rows(headcount, headcount).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Rows(headcount, headcount).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    ws.Rows(headcount, headcount).Style.Alignment.TextRotation = -90;
                    ws.Rows(headcount, headcount).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                    headcount += 1;
                    int columncnt;
                    // Adding DataRows.
                    foreach (CaseDashboardrpt row in table)
                    {
                        columncnt = 0;
                        ws.Cell(headcount, ++columncnt).SetValue(row.SubProgram);
                        //Section1
                        ws.Cell(headcount, ++columncnt).SetValue(row.TotalFamilies);
                        ws.Cell(headcount, ++columncnt).SetValue(row.NoofJKS);
                        ws.Cell(headcount, ++columncnt).SetValue(row.WithLICO);
                        ws.Cell(headcount, ++columncnt).SetValue(row.LicoPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";
                        //Section2
                        ws.Cell(headcount, ++columncnt).SetValue(row.TotalFamilyMembers);
                        ws.Cell(headcount, ++columncnt).SetValue(row.AvgFamilyMember);
                        ws.Cell(headcount, ++columncnt).SetValue(row.TotalMemberProfile);
                        ws.Cell(headcount, ++columncnt).SetValue(row.MemberProfilePer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.InitAssessment);
                        ws.Cell(headcount, ++columncnt).SetValue(row.InitialAssessmentPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.CaseGoalIdentified);
                        ws.Cell(headcount, ++columncnt).SetValue(row.CaseGoalIdentifiedPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.CaseGoalSet);
                        ws.Cell(headcount, ++columncnt).SetValue(row.CaseGoalSetPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.CaseActionDefined);
                        ws.Cell(headcount, ++columncnt).SetValue(row.CaseActionDefinedPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        //Section3
                        ws.Cell(headcount, ++columncnt).SetValue(row.NoOfActiveQOLFamilies);
                        ws.Cell(headcount, ++columncnt).SetValue(row.NoOfActiveQOLFamiliesPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedGoalCount);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedGoalCountPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedActionCount);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedActionCountPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        //Section4
                        ws.Cell(headcount, ++columncnt).SetValue(row.MonFamNotReady);
                        ws.Cell(headcount, ++columncnt).SetValue(row.MonFamNotReadyPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.MonRefExtAgency);
                        ws.Cell(headcount, ++columncnt).SetValue(row.MonRefExtAgencyPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedNotQualified);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedNotQualifiedPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ActiveInProgress);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ActiveInProgressPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ActiveOnBoarding);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ActiveOnBoardingPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.MonitoringCompleted);
                        ws.Cell(headcount, ++columncnt).SetValue(row.MonitoringCompletedPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.Hold);
                        ws.Cell(headcount, ++columncnt).SetValue(row.HoldPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedCompleted);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedCompletedPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedExternalAgencyFulfilled);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedExternalAgencyFulfilledPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedFamilyDeclineCasePlan);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedFamilyDeclineCasePlanPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedFamilyWithdrew);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedFamilyWithdrewPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedLackofFamilyEngagement);
                        ws.Cell(headcount, ++columncnt).SetValue(row.ClosedLackofFamilyEngagementPer);
                        ws.Cell(headcount, columncnt).Style.NumberFormat.Format = @"0\%;[Red](0\%)";

                        if (row.SubProgram.Contains("Summary"))
                        {
                            // Changing color to green.
                            ws.Rows(headcount, headcount).Style.Font.Bold = true;
                            ws.Rows(headcount, headcount).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                            ws.Rows(headcount, headcount).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        }
                        if (row.Region == "2")
                        {
                            // Changing color to green.
                            ws.Rows(headcount, headcount).Style.Font.Bold = true;
                            ws.Rows(headcount, headcount).Style.Fill.BackgroundColor = XLColor.FromArgb(91, 155, 213);
                            ws.Rows(headcount, headcount).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                            ws.Rows(headcount, headcount).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        }

                        headcount += 1;
                    }
                }

                //wb.Worksheets.Add(table);
                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CaseDashboard.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return new FileStreamResult(MyMemoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }

            //Open XML
            //MemoryStream memoryStream = new ExcelHelper().WriteToStream(table);
            //string excelfile = "CaseDashboard.xlsx";

            //// Prepare the response
            //Response.Clear();
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", "attachment;filename=\"" + excelfile + "");
            //memoryStream.WriteTo(Response.OutputStream);
            //memoryStream.Close();
            //Response.End();

            //return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            //Crystal Report
            //ReportDocument rd = new ReportDocument();
            //rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CaseDashboardNew.rpt"));

            //rd.SetDataSource(table);
            ////rd.Subreports[0].SetDataSource(table);
            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();

            //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //stream.Seek(0, SeekOrigin.Begin);
            //return File(stream, "application/pdf", "CustomerList.pdf");  

        }

        [WorkerAuthorize]
        [HttpPost]
        public ActionResult CaseDashboard(string Command, CaseDashboardRptInput model)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, Constants.Actions.CaseDashboard, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (Command == "CaseDashboardExcel")
                    {
                        return CaseDashboardExcel(model);
                    }
                    if (Command == "CaseDashboardPDF")
                    {
                        return CaseDashboardPDF(model);
                    }

                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            model.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (model.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                model.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                model.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(model);
        }


        [WorkerAuthorize]
        [HttpPost]
        public ActionResult ListOfIssues(ListOfIssuesVM model)
        {
            bool hasAccess = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.Reporting, Constants.Controllers.Report, Constants.Actions.ListOfIssues, true);
            if (!hasAccess)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    DataTable table = reportRepository.ListOfIssues(model);
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        table.TableName = "ListOfIssues";
                        //Add DataTable as Worksheet.
                        wb.Worksheets.Add(table);
                        //Export the Excel file.
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=ListOfIssues.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            model.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (model.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                model.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                model.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return View(model);
            //RedirectToAction("ListOfIssues", model);
        }
        public string[] GetColumnHeader()
        {
            return new string[]
            {
                "",
                "Total # Families","Total # JKs","With LICO or Ultra Poor","%",

                "Total # Family Members","Ave Members / Family", "With Profile","%",
                "With an Initial Assessment","%","With 1 or more Goals Identified","%",
                "With 1 or more Goals Set","%","With 1 or more Actions Defined","%",

                "Total # Active","% of Total # of Families","Closed 1 or more Actions","%",
                "Closed 1 or more Goals","%",
                
               "Monitoring - Family not ready","%","Monitoring - Referred to external agency","%",
               "Closed - Not Qualified","%","Active - In progress","%",
               "Active - Onboarding","%","Monitoring - Completed","%",
               "Hold","%","Closed - Completed","%",
               "Closed - External Agency Fulfilled","%","Closed - Family Declined Case Plan","%",
               "Closed - Family Withdrew","%","Closed - Lack of Family Engagement","%",

            };

        }
    }
}