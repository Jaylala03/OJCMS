using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.Models.Report
{
    public class CaseDashboardrpt
    {
        public string Region { get; set; }
        public string SubProgram { get; set; }
        //Section 1
        public int TotalFamilies { get; set; }
        public int NoofJKS { get; set; }
        public int WithLICO { get; set; }
        public int LicoPer { get; set; }
        //Section 2
        public int TotalFamilyMembers { get; set; }
        public int AvgFamilyMember { get; set; }
        public int TotalMemberProfile { get; set; }
        public int MemberProfilePer { get; set; }
        public int InitAssessment { get; set; }
        public int InitialAssessmentPer { get; set; }
        public int CaseGoalIdentified { get; set; }
        public int CaseGoalIdentifiedPer { get; set; }
        public int CaseGoalSet { get; set; }
        public int CaseGoalSetPer { get; set; }
        public int CaseActionDefined { get; set; }
        public int CaseActionDefinedPer { get; set; }
        //Section 3
        public int NoOfActiveQOLFamilies { get; set; }
        public int NoOfActiveQOLFamiliesPer { get; set; }
        public int ClosedGoalCount { get; set; }
        public int ClosedGoalCountPer { get; set; }
        public int ClosedActionCount { get; set; }
        public int ClosedActionCountPer { get; set; }
        //Section 4
        public int MonFamNotReady { get; set; }
        public int MonFamNotReadyPer { get; set; }
        public int MonRefExtAgency { get; set; }
        public int MonRefExtAgencyPer { get; set; }
        public int ClosedNotQualified { get; set; }
        public int ClosedNotQualifiedPer { get; set; }
        public int ActiveInProgress { get; set; }
        public int ActiveInProgressPer { get; set; }
        public int ActiveOnBoarding { get; set; }
        public int ActiveOnBoardingPer { get; set; }
        public int MonitoringCompleted { get; set; }
        public int MonitoringCompletedPer { get; set; }
        public int Hold { get; set; }
        public int HoldPer { get; set; }
        public int ClosedCompleted { get; set; }
        public int ClosedCompletedPer { get; set; }
        public int ClosedExternalAgencyFulfilled { get; set; }
        public int ClosedExternalAgencyFulfilledPer { get; set; }
        public int ClosedFamilyDeclineCasePlan { get; set; }
        public int ClosedFamilyDeclineCasePlanPer { get; set; }
        public int ClosedFamilyWithdrew { get; set; }
        public int ClosedFamilyWithdrewPer { get; set; }
        public int ClosedLackofFamilyEngagement { get; set; }
        public int ClosedLackofFamilyEngagementPer { get; set; }
    }

    
}
