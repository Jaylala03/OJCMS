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
    public class AssesmentIndicators : EntityBaseModel
    {

        [Display(Name = "Indicator Type")]
        [ForeignKey("IndicatorType")]
        public Int32 IndicatorTYpeID { get; set; }

        [Display(Name = "Description 1")]
        [StringLength(1000)]
        public String Description1 { get; set; }

        [Display(Name = "Description 2")]
        [StringLength(1000)]
        public String Description2 { get; set; }

        [Display(Name = "Description 3")]
        [StringLength(1000)]
        public String Description3 { get; set; }

        [Display(Name = "Description 4")]
        [StringLength(1000)]
        public String Description4 { get; set; }

        [Display(Name = "Description 5")]
        [StringLength(1000)]
        public String Description5 { get; set; }

        [Display(Name = "Description 6")]
        [StringLength(1000)]
        public String Description6 { get; set; }

        [Display(Name = "Description 7")]
        [StringLength(1000)]
        public String Description7 { get; set; }

        public virtual IndicatorType IndicatorType { get; set; }

        [Display(Name = "Indicator Type")]
        [NotMapped]
        public String IndicatorTypeName { get; set; }
        public AssesmentIndicators()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;
            Description4 = string.Empty;
            Description5 = string.Empty;
            Description6 = string.Empty;
            Description7 = string.Empty;
        }
    }
}
