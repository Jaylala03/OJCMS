//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.BusinessLogic.Repositories
{
    public class ReportRepository : IReportRepository
    {
        RepositoryContext context = new RepositoryContext();

        public DataTable CaseDashboard(CaseDashboardRptInput model)
        {
            string constr = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(GetReportQuery(model)))
                {
                    cmd.Parameters.Add(new SqlParameter("@StartDate", model.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", model.EndDate));

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;

                        using (DataSet ds = new DataSet())
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            DataTable regionDT = dt.Select("Region is not NULL", "Region,SubProgram").CopyToDataTable();
                            DataTable regionGrpDT = dt.Select("Region is NULL").CopyToDataTable();

                            regionDT.Merge(regionGrpDT);

                            GetDatatableDefinition(ref regionDT);
                            int totalfamilies = 0, totalFamilyMembers = 0, totalActiveFamilies = 0;

                            foreach (DataRow row in regionDT.Rows)
                            {
                                if (row["SubProgram"] == DBNull.Value && row["Region"] == DBNull.Value) // getting the row to edit , change it as you need
                                {
                                    row["SubProgram"] = "National Summary";
                                    row["Region"] = "2"; //Set it with 1, Will be used in report for formatting.
                                }
                                else if (row["SubProgram"] == DBNull.Value)
                                {
                                    row["SubProgram"] = string.Format("{0} Summary", Convert.ToString(row["Region"]));
                                    row["Region"] = "1"; //Set it with 1, Will be used in report for formatting.
                                }
                                else if (row["Region"] == DBNull.Value) // getting the row to edit , change it as you need
                                {
                                    row["Region"] = "2"; //Set it with 1, Will be used in report for formatting.
                                }
                                totalfamilies = Convert.ToInt32(row["TotalFamilies"]);
                                if (totalfamilies > 0)
                                {
                                    //row["Lico%"] = (int)Math.Ceiling((Convert.ToInt32(row["WithLICO"]) * 100) / totalfamilies);
                                    row["Lico%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["WithLICO"]) * 100), totalfamilies));
                                }
                                totalFamilyMembers = Convert.ToInt32(row["TotalFamilyMembers"]);
                                if (totalFamilyMembers > 0)
                                {
                                    row["AvgFamilyMember"] = (int)Math.Round(Decimal.Divide(totalFamilyMembers, totalfamilies));
                                    row["MemberProfile%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["TotalMemberProfile"]) * 100), totalFamilyMembers));
                                    row["InitialAssessment%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["InitAssessment"]) * 100), totalFamilyMembers));
                                    row["CaseGoalIdentified%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["CaseGoalIdentified"]) * 100), totalFamilyMembers));
                                    row["CaseGoalSet%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["CaseGoalSet"]) * 100), totalFamilyMembers));
                                    row["CaseActionDefined%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["CaseActionDefined"]) * 100), totalFamilyMembers));
                                }
                                totalActiveFamilies = Convert.ToInt32(row["NoOfActiveQOLFamilies"]);
                                if (totalActiveFamilies > 0)
                                {
                                    row["ActiveFamily%"] = (int)Math.Round(Decimal.Divide((totalActiveFamilies * 100), totalfamilies));
                                    row["ClosedAction%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedActionCount"]) * 100), totalActiveFamilies));
                                    row["ClosedGoal%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedGoalCount"]) * 100), totalActiveFamilies));
                                }

                                row["MonFamNotReady%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["MonFamNotReady"]) * 100), totalfamilies));
                                row["MonRefExtAgency%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["MonRefExtAgency"]) * 100), totalfamilies));
                                row["ClosedNotQualified%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedNotQualified"]) * 100), totalfamilies));
                                row["ActiveInProgress%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ActiveInProgress"]) * 100), totalfamilies));
                                row["ActiveOnBoarding%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ActiveOnBoarding"]) * 100), totalfamilies));
                                row["MonitoringCompleted%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["MonitoringCompleted"]) * 100), totalfamilies));
                                row["Hold%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["Hold"]) * 100), totalfamilies));
                                row["ClosedCompleted%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedCompleted"]) * 100), totalfamilies));
                                row["ClosedExternalAgencyFulfilled%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedExternalAgencyFulfilled"]) * 100), totalfamilies));
                                row["ClosedFamilyDeclineCasePlan%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedFamilyDeclineCasePlan"]) * 100), totalfamilies));
                                row["ClosedFamilyWithdrew%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedFamilyWithdrew"]) * 100), totalfamilies));
                                row["ClosedLackofFamilyEngagement%"] = (int)Math.Round(Decimal.Divide((Convert.ToInt32(row["ClosedLackofFamilyEngagement"]) * 100), totalfamilies));
                            }

                            //ds.Tables.Add(regionDT);
                            return regionDT;
                            //return results;
                        }
                    }
                }
            }

        }

        public List<CaseDashboardrpt> CaseDashboardExcel(CaseDashboardRptInput model)
        {
            SqlParameter startdate = new SqlParameter("@StartDate", model.StartDate);
            SqlParameter enddate = new SqlParameter("@EndDate", model.EndDate);

            object[] parameters = new object[] { startdate, enddate };

            List<CaseDashboardrpt> reportlist = context.Database.SqlQuery<CaseDashboardrpt>(GetReportQuery(model), parameters).ToList();
            List<CaseDashboardrpt> regionlist = reportlist.Where(m => m.Region != null).OrderBy(m => m.Region).ThenBy(m => m.SubProgram).ToList();
            List<CaseDashboardrpt> regionGrplist = reportlist.Where(m => m.Region == null).OrderBy(m => m.Region).ToList();

            foreach (CaseDashboardrpt row in regionGrplist)
            {
                regionlist.Add(row);
            }
            //int totalfamilies = 0, totalFamilyMembers = 0, totalActiveFamilies = 0;

            foreach (CaseDashboardrpt row in regionlist)
            {
                if (row.SubProgram == null && row.Region == null) // getting the row to edit , change it as you need
                {
                    row.SubProgram = "National Summary";
                    row.Region = "2"; //Set it with 1, Will be used in report for formatting.
                }
                else if (row.SubProgram == null)
                {
                    row.SubProgram = string.Format("{0} Summary", row.Region);
                    row.Region = "1"; //Set it with 1, Will be used in report for formatting.
                }
                else if (row.Region == null) // getting the row to edit , change it as you need
                {
                    row.Region = "2"; //Set it with 1, Will be used in report for formatting.
                }
                //totalfamilies = Convert.ToInt32(row.TotalFamilies);
                if (row.TotalFamilies > 0)
                {
                    row.LicoPer = (int)Math.Round(Decimal.Divide((row.WithLICO * 100), row.TotalFamilies));
                }
                //totalFamilyMembers = Convert.ToInt32(row.TotalFamilyMembers);
                if (row.TotalFamilyMembers > 0)
                {
                    row.AvgFamilyMember = (int)Math.Round(Decimal.Divide(row.TotalFamilyMembers, row.TotalFamilies));
                    row.MemberProfilePer = (int)Math.Round(Decimal.Divide(row.TotalMemberProfile * 100, row.TotalFamilyMembers));
                    row.InitialAssessmentPer = (int)Math.Round(Decimal.Divide(row.InitAssessment * 100, row.TotalFamilyMembers));
                    row.CaseGoalIdentifiedPer = (int)Math.Round(Decimal.Divide(row.CaseGoalIdentified * 100, row.TotalFamilyMembers));
                    row.CaseGoalSetPer = (int)Math.Round(Decimal.Divide(row.CaseGoalSet * 100, row.TotalFamilyMembers));
                    row.CaseActionDefinedPer = (int)Math.Round(Decimal.Divide(row.CaseActionDefined * 100, row.TotalFamilyMembers));
                }
                //totalActiveFamilies = Convert.ToInt32(row.NoOfActiveQOLFamilies);
                if (row.NoOfActiveQOLFamilies > 0)
                {
                    row.NoOfActiveQOLFamiliesPer = (int)Math.Round(Decimal.Divide(row.NoOfActiveQOLFamilies * 100, row.TotalFamilies));
                    row.ClosedActionCountPer = (int)Math.Round(Decimal.Divide(row.ClosedActionCount * 100, row.NoOfActiveQOLFamilies));
                    row.ClosedGoalCountPer = (int)Math.Round(Decimal.Divide(row.ClosedGoalCount * 100, row.NoOfActiveQOLFamilies));
                }

                row.MonFamNotReadyPer = (int)Math.Round(Decimal.Divide(row.MonFamNotReady * 100, row.TotalFamilies));
                row.MonRefExtAgencyPer = (int)Math.Round(Decimal.Divide(row.MonRefExtAgency * 100, row.TotalFamilies));
                row.ClosedNotQualifiedPer = (int)Math.Round(Decimal.Divide(row.ClosedNotQualified * 100, row.TotalFamilies));
                row.ActiveInProgressPer = (int)Math.Round(Decimal.Divide(row.ActiveInProgress * 100, row.TotalFamilies));
                row.ActiveOnBoardingPer = (int)Math.Round(Decimal.Divide(row.ActiveOnBoarding * 100, row.TotalFamilies));
                row.MonitoringCompletedPer = (int)Math.Round(Decimal.Divide(row.MonitoringCompleted * 100, row.TotalFamilies));
                row.HoldPer = (int)Math.Round(Decimal.Divide(row.Hold * 100, row.TotalFamilies));
                row.ClosedCompletedPer = (int)Math.Round(Decimal.Divide(row.ClosedCompleted * 100, row.TotalFamilies));
                row.ClosedExternalAgencyFulfilledPer = (int)Math.Round(Decimal.Divide(row.ClosedExternalAgencyFulfilled * 100, row.TotalFamilies));
                row.ClosedFamilyDeclineCasePlanPer = (int)Math.Round(Decimal.Divide(row.ClosedFamilyDeclineCasePlan * 100, row.TotalFamilies));
                row.ClosedFamilyWithdrewPer = (int)Math.Round(Decimal.Divide(row.ClosedFamilyWithdrew * 100, row.TotalFamilies));
                row.ClosedLackofFamilyEngagementPer = (int)Math.Round(Decimal.Divide(row.ClosedLackofFamilyEngagement * 100, row.TotalFamilies));
            }

            //ds.Tables.Add(regionDT);
            return regionlist;
            //return results;
        }

        public DataTable ListOfIssues(ListOfIssuesVM model)
        {
            string constr = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;
            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append("SELECT CM.CaseID AS FamilyID,C.EnrollDate AS EnrollmentDate,R.Name AS Region,JK.Name AS Jamatkhana ");
            sqlquery.Append(",CM.ID AS MemberID, CM.LastUpdateDate AS MemberLastUpdateDate,QC.Name AS QOLCategory ");
            sqlquery.Append(",QS.Name AS QOLSubCategory,Q.Name AS QOLIssue ");
            sqlquery.Append("FROM [Case] AS C ");
            sqlquery.Append("INNER JOIN CaseMember AS CM ON C.ID = CM.CaseID ");
            sqlquery.Append("INNER JOIN Region AS R ON C.RegionID = R.ID ");
            sqlquery.Append("INNER JOIN CaseAssessment AS CAS ON CM.ID = CAS.CaseMemberID ");
            sqlquery.Append("INNER JOIN CaseAssessmentLivingCondition AS CAL ON CAS.ID = CAL.CaseAssessmentID ");
            sqlquery.Append("INNER JOIN QualityOfLife AS Q ON CAL.QualityOfLifeID = Q.ID ");
            sqlquery.Append("INNER JOIN QualityOfLifeSubCategory AS QS ON Q.QualityOfLifeSubCategoryID = QS.ID ");
            sqlquery.Append("INNER JOIN QualityOfLifeCategory AS QC ON QS.QualityOfLifeCategoryID = QC.ID ");
            sqlquery.Append("LEFT JOIN Jamatkhana AS JK ON C.JamatkhanaID = JK.ID ");
            sqlquery.Append("WHERE C.EnrollDate BETWEEN @FromDate AND @ToDate ");
            sqlquery.Append("ORDER BY C.EnrollDate");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlquery.ToString()))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;

                        cmd.Parameters.Add(new SqlParameter("@FromDate", model.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@ToDate", model.EndDate));

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }

                    }
                }
            }

        }

        public void GetDatatableDefinition(ref DataTable DTregion)
        {
            //Section1  : Families
            DTregion.Columns.Add("Lico%", typeof(int));
            DTregion.Columns["Lico%"].DefaultValue = 0;

            //Section2  : Familiy Members
            DTregion.Columns.Add("AvgFamilyMember", typeof(int));
            DTregion.Columns["AvgFamilyMember"].DefaultValue = 0;

            DTregion.Columns.Add("MemberProfile%", typeof(int));
            DTregion.Columns["MemberProfile%"].DefaultValue = 0;

            DTregion.Columns.Add("InitialAssessment%", typeof(int));
            DTregion.Columns["InitialAssessment%"].DefaultValue = 0;

            DTregion.Columns.Add("CaseGoalIdentified%", typeof(int));
            DTregion.Columns["CaseGoalIdentified%"].DefaultValue = 0;

            DTregion.Columns.Add("CaseGoalSet%", typeof(int));
            DTregion.Columns["CaseGoalSet%"].DefaultValue = 0;

            DTregion.Columns.Add("CaseActionDefined%", typeof(int));
            DTregion.Columns["CaseActionDefined%"].DefaultValue = 0;

            //Section3  : Active QOL Families
            DTregion.Columns.Add("ActiveFamily%", typeof(int));
            DTregion.Columns["ActiveFamily%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedAction%", typeof(int));
            DTregion.Columns["ClosedAction%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedGoal%", typeof(int));
            DTregion.Columns["ClosedGoal%"].DefaultValue = 0;

            //Section3  : Family Status
            DTregion.Columns.Add("MonFamNotReady%", typeof(int));
            DTregion.Columns["MonFamNotReady%"].DefaultValue = 0;

            DTregion.Columns.Add("MonRefExtAgency%", typeof(int));
            DTregion.Columns["MonRefExtAgency%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedNotQualified%", typeof(int));
            DTregion.Columns["ClosedNotQualified%"].DefaultValue = 0;

            DTregion.Columns.Add("ActiveInProgress%", typeof(int));
            DTregion.Columns["ActiveInProgress%"].DefaultValue = 0;

            DTregion.Columns.Add("ActiveOnBoarding%", typeof(int));
            DTregion.Columns["ActiveOnBoarding%"].DefaultValue = 0;

            DTregion.Columns.Add("MonitoringCompleted%", typeof(int));
            DTregion.Columns["MonitoringCompleted%"].DefaultValue = 0;

            DTregion.Columns.Add("Hold%", typeof(int));
            DTregion.Columns["Hold%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedCompleted%", typeof(int));
            DTregion.Columns["ClosedCompleted%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedExternalAgencyFulfilled%", typeof(int));
            DTregion.Columns["ClosedExternalAgencyFulfilled%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedFamilyDeclineCasePlan%", typeof(int));
            DTregion.Columns["ClosedFamilyDeclineCasePlan%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedFamilyWithdrew%", typeof(int));
            DTregion.Columns["ClosedFamilyWithdrew%"].DefaultValue = 0;

            DTregion.Columns.Add("ClosedLackofFamilyEngagement%", typeof(int));
            DTregion.Columns["ClosedLackofFamilyEngagement%"].DefaultValue = 0;
        }

        public string GetReportQuery(CaseDashboardRptInput model)
        {
            //string regionid = String.Join(",", model.RegionID);
            //string programid = String.Join(",", model.ProgramID);
            //string subprogramid = String.Join(",", model.SubProgramID);
            //string jamatkhanaid = String.Join(",", model.JamatkhanaID);

            //string sqlquery = string.Format(GetReportQuery(), regionid, programid, subprogramid, jamatkhanaid);

            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append("SELECT rpt.Region,rpt.SubProgram ");
            sqlquery.Append(",ISNULL(SUM(rpt.TotalFamilies),0) AS TotalFamilies,ISNULL(SUM(rpt.NoofJKS),0) AS NoofJKS,ISNULL(SUM(rpt.WithLICO),0) AS WithLICO,ISNULL(SUM(rpt.TotalFamilyMembers),0) AS TotalFamilyMembers ");
            sqlquery.Append(",ISNULL(SUM(rpt.TotalMemberProfile),0) AS TotalMemberProfile,ISNULL(SUM(rpt.InitAssessment),0) AS InitAssessment,ISNULL(SUM(rpt.CaseGoalIdentified),0) AS CaseGoalIdentified ");
            sqlquery.Append(",ISNULL(SUM(rpt.CaseGoalSet),0) AS CaseGoalSet,ISNULL(SUM(rpt.CaseActionDefined),0) AS CaseActionDefined,ISNULL(SUM(rpt.NoOfActiveQOLFamilies),0) AS NoOfActiveQOLFamilies ");
            sqlquery.Append(",ISNULL(SUM(rpt.ClosedGoalCount),0) AS ClosedGoalCount,ISNULL(SUM(rpt.ClosedActionCount),0) AS ClosedActionCount ");
            sqlquery.Append(",ISNULL(SUM(rpt.MonFamNotReady),0) AS MonFamNotReady,ISNULL(SUM(rpt.MonRefExtAgency),0) AS MonRefExtAgency ");
            sqlquery.Append(",ISNULL(SUM(rpt.MonRefExtAgency),0) AS MonRefExtAgency,ISNULL(SUM(rpt.ClosedNotQualified),0) AS ClosedNotQualified ");
            sqlquery.Append(",ISNULL(SUM(rpt.ActiveInProgress),0) AS ActiveInProgress,ISNULL(SUM(rpt.ActiveOnBoarding),0) AS ActiveOnBoarding ");
            sqlquery.Append(",ISNULL(SUM(rpt.MonitoringCompleted),0) AS MonitoringCompleted,ISNULL(SUM(rpt.Hold),0) AS Hold ");
            sqlquery.Append(",ISNULL(SUM(rpt.ClosedCompleted),0) AS ClosedCompleted,ISNULL(SUM(rpt.ClosedExternalAgencyFulfilled),0) AS ClosedExternalAgencyFulfilled ");
            sqlquery.Append(",ISNULL(SUM(rpt.ClosedFamilyDeclineCasePlan),0) AS ClosedFamilyDeclineCasePlan,ISNULL(SUM(rpt.ClosedFamilyWithdrew),0) AS ClosedFamilyWithdrew ");
            sqlquery.Append(",ISNULL(SUM(rpt.ClosedLackofFamilyEngagement),0) AS ClosedLackofFamilyEngagement ");
            sqlquery.Append("FROM ");
            sqlquery.Append("(SELECT C.RegionID, C.SubProgramID,R.Name AS Region,SP.Name AS SubProgram ");
            // sqlquery.Append("---section1------------------ ");
            sqlquery.Append(",COUNT(DISTINCT C.ID) AS TotalFamilies ");
            sqlquery.Append(",COUNT(DISTINCT C.JamatkhanaID) AS NoofJKS ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN (CAL.QualityOfLifeID = 101 OR CAL.QualityOfLifeID = 102) AND (CAL.CaseAssessmentID IS NOT NULL) THEN C.ID END) AS WithLICO ");
            //sqlquery.Append("---section2------------------ ");
            sqlquery.Append(",COUNT(DISTINCT CM.ID) AS TotalFamilyMembers ");
            //sqlquery.Append("-- We need distinct here as one case member can have multiple profiles. Also DISTINCT CMP.CaseMemberID should be used because of left join ");
            //sqlquery.Append("--with CaseMember ");
            sqlquery.Append(",COUNT(DISTINCT CMP.CaseMemberID) AS TotalMemberProfile ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN CAS.AssessmentTypeID = 1 THEN CAS.CaseMemberID END) AS InitAssessment ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN CG.CaseMemberID IS NOT NULL THEN  CG.CaseMemberID END) AS CaseGoalIdentified ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN CG.CaseMemberID IS NOT NULL AND CSG.CaseGoalID IS NOT NULL THEN  CG.CaseMemberID END) AS CaseGoalSet ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN CA.CaseMemberID IS NOT NULL THEN  CA.CaseMemberID END) AS CaseActionDefined ");
            //sqlquery.Append("---section3------------------ ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID < 7 THEN C.ID END) AS NoOfActiveQOLFamilies ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID < 7 AND CSG.IsCompleted = 1 AND CSG.CaseGoalID IS NOT NULL THEN C.ID END) AS ClosedGoalCount ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID < 7 AND CA.IsCompleted = 1 AND CA.CaseMemberID IS NOT NULL THEN C.ID END) AS ClosedActionCount ");
            //sqlquery.Append("---section4------------------ ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 4 THEN C.ID END) as MonFamNotReady ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 6 THEN C.ID END) as MonRefExtAgency ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 10 THEN C.ID END) as ClosedNotQualified ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 1 THEN  C.ID END) as ActiveInProgress ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 2 THEN C.ID END) as ActiveOnBoarding ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 5 THEN C.ID END) as MonitoringCompleted ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 3 THEN C.ID END) as Hold ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 9 THEN C.ID END) as ClosedCompleted ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 8 THEN C.ID END) as ClosedExternalAgencyFulfilled ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 7 THEN C.ID END) as ClosedFamilyDeclineCasePlan ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 14 THEN C.ID END) as ClosedFamilyWithdrew ");
            sqlquery.Append(",COUNT(DISTINCT CASE WHEN C.CaseStatusID = 13 THEN C.ID END) as ClosedLackofFamilyEngagement ");

            sqlquery.Append("FROM [dbo].[Case] AS C ");
            sqlquery.Append("INNER JOIN Region AS R ON C.RegionID = R.ID ");
            sqlquery.Append("INNER JOIN SubProgram AS SP ON C.SubProgramID = SP.ID ");
            sqlquery.Append("LEFT JOIN CaseMember AS CM ON C.ID = CM.CaseID ");
            sqlquery.Append("LEFT JOIN CaseMemberProfile AS CMP ON CM.ID = CMP.CaseMemberID ");
            //--Initial Assessment 
            sqlquery.Append("LEFT JOIN CaseAssessment AS CAS ON CM.ID = CAS.CaseMemberID ");
            sqlquery.Append("LEFT JOIN CaseAssessmentLivingCondition AS CAL ON CAS.ID = CAL.CaseAssessmentID ");
            sqlquery.Append("LEFT JOIN CaseGoal AS CG ON CM.ID = CG.CaseMemberID ");
            sqlquery.Append("LEFT JOIN CaseSmartGoal AS CSG ON CG.ID = CSG.CaseGoalID ");
            sqlquery.Append("LEFT JOIN CaseAction AS CA ON CM.ID = CA.CaseMemberID ");
            sqlquery.Append("WHERE 1=1 ");
            //sqlquery.Append("--C.ID =7006 AND ");

            if (model.RegionID != null)
                sqlquery.Append("AND C.RegionID IN (" + String.Join(",", model.RegionID) + ") ");
            if (model.ProgramID != null)
                sqlquery.Append("AND C.ProgramID IN (" + String.Join(",", model.ProgramID) + ") ");
            if (model.SubProgramID != null)
                sqlquery.Append("AND C.SubProgramID IN (" + String.Join(",", model.SubProgramID) + ") ");
            if (model.JamatkhanaID != null)
                sqlquery.Append("AND C.JamatkhanaID IN (" + String.Join(",", model.JamatkhanaID) + ") ");

            //sqlquery.Append("AND C.EnrollDate >= '2015-01-01 00:00:00.000' ");
            sqlquery.Append(" AND C.EnrollDate BETWEEN @StartDate AND @EndDate ");
            sqlquery.Append("AND C.Comments NOT LIKE '[Old%' AND C.CaseStatusID <> 15 "); //Case Status Not Added in error
            //sqlquery.Append("--GROUP BY GROUPING SETS((C.RegionID, C.SubProgramID,R.Name,SP.Name) ,(R.Name)) ");
            sqlquery.Append("GROUP BY C.RegionID, C.SubProgramID,R.Name,SP.Name ");
            sqlquery.Append(") AS rpt ");
            sqlquery.Append("WHERE rpt.SubProgram IS NOT NULL ");
            sqlquery.Append("GROUP BY GROUPING SETS((rpt.Region,rpt.SubProgram),(rpt.Region) ,(rpt.SubProgram),()) ");
            //sqlquery.Append("GROUP BY rpt.Region,rpt.SubProgram ");
            sqlquery.Append("ORDER BY rpt.Region,rpt.SubProgram ");

            return sqlquery.ToString();
        }
    }

    public interface IReportRepository
    {
        DataTable CaseDashboard(CaseDashboardRptInput model);
        List<CaseDashboardrpt> CaseDashboardExcel(CaseDashboardRptInput model);
        DataTable ListOfIssues(ListOfIssuesVM model);
    }
}
