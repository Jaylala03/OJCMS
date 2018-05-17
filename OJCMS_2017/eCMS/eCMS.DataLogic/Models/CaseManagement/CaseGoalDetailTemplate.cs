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
    public class CaseGoalDetailTemplate : EntityBaseModel
    {

        [Display(Name = "Indicator Type")]
        [ForeignKey("IndicatorType")]
        public Int32 IndicatorTypeID { get; set; }

        [Display(Name = "Name")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please enter name")]
        public String Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(4000)]
        [Required(ErrorMessage = "Please enter description")]
        public String Description { get; set; }
        
        public virtual IndicatorType IndicatorType { get; set; }

        [Display(Name = "Indicator Type")]
        [NotMapped]
        public String IndicatorTypeName { get; set; }
        public CaseGoalDetailTemplate()
        {
            Description = string.Empty;
        }
    }
}
