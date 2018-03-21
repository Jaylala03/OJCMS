
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
    public class InitialAssessmentVM : EntityBaseModel
    {
        public List<InitialAssessmentIndicatorsVM> AssesmentIndicators { get; set; }
        public List<CaseInitialAssessmentVM> CaseInitialAssessment { get; set; }
        public CaseHouseholdIncome CaseHouseholdIncome { get; set; }
        public CaseWorkerNote CaseWorkerNote { get; set; }
        public int CaseID { get; set; }

        [Display(Name = "Family has reviewed and agreed with the Assessment")]
        public bool CaseAssessmentReviewed { get; set; }
    }
}