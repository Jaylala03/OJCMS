//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;

namespace eCMS.Shared
{
    public static class Constants
    {
        public static class CommonConstants
        {
            public const string RememberMeCookieName = ".eCMSCOOKIE";
            public const int DefaultPageSize = 20;
            public const string FancyBoxLink = "#inline";
        }

        public static class Paths
        {
            public const string TemporaryFileUploadPath = "~/UploadedFiles/Temporary";
            public const string DownloadFilePath = "~/Download/";
        }

        public static class RegularExpressions
        {
            public const string PhoneNumber = @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}";
        }

        public static class PartialViews
        {
            public const string Progress = "_AjaxProgress";
            public const string Alert = "_Alert";
            public const string AlertSliding = "_AlertSliding";
            public const string CreateOrEdit = "_CreateOrEdit";
            public const string SubProgram = "_SubProgram";
            public const string Details = "_Details";
            public const string EditorPopUp = "EditorPopUp";
            public const string EditorPopUpList = "EditorPopUpList";
            public const string WorkerInRole = "_WorkerInRole";
            public const string WorkerInRoleNew = "_WorkerInRoleNew";
            public const string PermissionRegion = "_PermissionRegion";
            public const string AddModule = "AddModule";
        }

        public static class Views
        {
            public const string Index = "Index";
            public const string Edit = "Edit";
        }

        public static class Params
        {
            
        }

        public static class Actions
        {
            public const string SmartGoalAction = "SmartGoalAction";
            public const string Readmit = "Readmit";
            public const string Index = "Index";
            public const string IndexRead = "IndexRead";
            public const string ServiceProvider = "ServiceProvider";
            public const string MyToDo = "MyToDo";
            public const string MyCase = "MyCase";
            public const string MyNotification = "MyNotification";
            public const string InitialContact = "InitialContact";
            public const string InitialContactRead = "InitialContactRead";
            public const string CreateInitialContact = "CreateInitialContact";
            public const string CreateInitialContactRead = "CreateInitialContactRead";
            public const string IndexAjax = "IndexAjax";
            public const string Edit = "Edit";
            public const string Read = "Read";
            public const string Create = "Create";
            public const string Error = "Error";
            public const string AccessDenied = "AccessDenied";
            public const string Login = "Login";
            public const string ChangePassword = "ChangePassword";
            public const string Logout = "Logout";
            public const string ForgotPassword = "ForgotPassword";
            public const string Delete = "Delete";
            public const string EditorPopUp = "EditorPopUp";
            public const string Submit = "Submit";
            public const string SaveAjax = "SaveAjax";
            public const string EditorPopUpList = "EditorPopUpListAjax";
            public const string Termination = "Termination";
            public const string CaseAuditLog = "CaseAuditLog";
            public const string CaseAuditLogForMember = "CaseAuditLogForMember";
            public const string CaseDashboard = "CaseDashboard";
            public const string ListOfIssues = "ListOfIssues";
            public const string ReportsCSV = "ReportsCSV";
            public const string ServicePlanHistory = "ServicePlanHistory";
        }

        public static class Controllers
        {
            public const string Home = "Home";
            public const string Account = "Account";
            public const string Worker = "Worker";
            public const string Feedback = "Feedback";
            public const string Case = "Case";
            public const string CaseSummary = "CaseSummary";
            public const string CaseMember = "CaseMember";
            public const string CaseInitialAssessment = "CaseInitialAssessment";
            public const string CaseAssessmentSummary = "CaseAssessmentSummary"; 
            public const string CaseWorker = "CaseWorker";
            public const string CaseProgressNote = "CaseProgressNote";
            public const string CaseAction = "CaseAction";
            public const string CaseActionNew = "CaseActionNew";
            public const string RegionRole = "RegionRole";
            public const string RegionSubProgram = "RegionSubProgram";
            public const string WorkerRole = "WorkerRole";
            public const string Permission = "Permission";
            public const string WorkerRolePermission = "WorkerRolePermission";
            public const string Program = "Program";
            public const string SubProgram = "SubProgram";
            public const string Region = "Region";
            public const string CaseMemberProfile = "CaseMemberProfile";
            public const string CaseAssessment = "CaseAssessment";
            public const string CaseGoal = "CaseGoal";
            public const string CaseGoalNew = "CaseGoalNew";
            public const string CaseSmartGoal = "CaseSmartGoal";
            public const string CaseSmartGoalProgress = "CaseSmartGoalProgress";
            public const string CaseSmartGoalServiceProvider = "CaseSmartGoalServiceProvider";
            public const string CaseSmartGoalServiceLevelOutcome = "CaseSmartGoalServiceLevelOutcome";
            public const string QualityOfLifeCategory = "QualityOfLifeCategory";
            public const string QualityOfLifeSubCategory = "QualityOfLifeSubCategory";
            public const string QualityOfLife = "QualityOfLife";
            public const string ServiceProvider = "ServiceProvider";
            public const string Service = "Service";
            public const string Report = "Report";
            public const string CaseTraining = "CaseTraining";
            public const string CaseHouseholdIncome = "CaseHouseholdIncome";
            public const string AssesmentIndicators = "AssesmentIndicators";
            public const string CaseGoalDetailTemplate = "CaseGoalDetailTemplate";
            public const string GoalStatus = "GoalStatus";
        }

        public static class Areas
        {
            public const string CaseManagement = "CaseManagement";
            public const string WorkerManagement = "WorkerManagement";
            public const string Lookup = "Lookup";
            public const string Reporting = "Reporting";
        }

        public static class Messages
        {
            public const string UnhandelledError = "We are facing some problem while processing the current request. Please try again later or submit a feedback.";
            public const string UserLogin_UnknownError = "We are facing some problem while processing the current request. Please try again later.";
        }

        public static class UIConstants
        {
            public const string DropDownListDefaulLabel = "Please select";
            public const string NotAvailable = "N/A";
        }
    }
}
