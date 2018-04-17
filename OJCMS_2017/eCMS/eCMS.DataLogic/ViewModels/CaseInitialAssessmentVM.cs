//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eCMS.DataLogic.Models
{
    public class CaseInitialAssessmentVM 
    {
        public Int32 CaseMemberID { get; set; }
        public Int32 IndicatorTypeID { get; set; }
        public int AssessmentValue { set; get; }
        public string CaseMemberName { get; set; }
        public int AssessmentVersion { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
