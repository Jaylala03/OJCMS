﻿//*********************************************************
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
    public class CaseInitialAssessment : EntityBaseModel
    {
        public Int32 CaseID { get; set; }

        public Int32 CaseMemberID { get; set; }

        public Int32 IndicatorTypeID { get; set; }

        public string AssessmentValue { set; get; }        

        [NotMapped]
        public CaseHouseholdIncome CaseHouseholdIncome { get; set; }

        [NotMapped]
        public CaseWorkerNote CaseWorkerNote { get; set; }
    }
}