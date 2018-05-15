
using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
    public class CaseSummaryVM
    {
        public int CaseID { get; set; }
        public int ProgramID { get; set; }
        public string Program { get; set; }
        public string SubProgram { get; set; }
        public string Region { get; set; }
        public string Jamatkhaana { get; set; }
        public string ReferenceCase { get; set; }
        public DateTime? EnrolmentDate { get; set; }
        public string IntakeMethod { get; set; }
        public string ReferralSource { get; set; }
        public DateTime? ReferralDate { get; set; }
        public string RiskLevel { get; set; }
        public string PresentingProblem { get; set; }
        public string AreaOfNeed { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public string CaseStatus { get; set; }

        public CaseMember caseMember { get; set; }

        public int CurrentHouseholdIncomeID { get; set; }

        public CaseHouseholdIncomeVM CaseInitialHouseholdIncomeVM { get; set; }
        public CaseHouseholdIncomeVM CaseCurrentHouseholdIncomeVM { get; set; }

        public List<InitialAssessmentIndicatorsVM> AssesmentIndicators { get; set; }
        public List<CaseInitialAssessmentVM> CaseInitialAssessment { get; set; }

        public bool DoesHouseHoldIncomeExists { get; set; }
        public bool DoesInitialAssessmentExists { get; set; }
        public bool DoesFamilyMembersExists { get; set; }
        public CaseGoalNewVM caseGoalNewVM { get; set; }
    }
}