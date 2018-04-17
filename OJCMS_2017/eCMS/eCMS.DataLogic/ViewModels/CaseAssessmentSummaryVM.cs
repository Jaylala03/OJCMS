//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************


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
    public class CaseAssessmentSummaryVM : EntityBaseModel
    {
        [Display(Name = "")]        
        public int? CaseMemberID { get; set; }

        public int CaseID { get; set; }

        public int ViewAsID { get; set; }

        public List<InitialAssessmentIndicatorsVM> AssesmentIndicators { get; set; }
        public List<CaseInitialAssessmentVM> CaseInitialAssessment { get; set; }
    }
}
