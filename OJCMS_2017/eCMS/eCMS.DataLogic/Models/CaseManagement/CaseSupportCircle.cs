//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public class CaseSupportCircle : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select case")]
        [Display(Name = "Case")]
        [ForeignKey("Case")]
        public Int32 CaseID { get; set; }

        [Required(ErrorMessage = "Please enter institution")]
        [Display(Name = "Institution")]
        [StringLength(256)]
        public String Institution { get; set; }

        [Required(ErrorMessage = "Please enter resource")]
        [Display(Name = "Resource")]
        [StringLength(256)]
        public String Resource { get; set; }

        [Required(ErrorMessage = "Please enter relationship")]
        [Display(Name = "Relationship")]
        [StringLength(256)]
        public String Relationship { get; set; }

        [Display(Name = "Primary Contact Information")]
        [StringLength(256)]
        public String ContactInformation { get; set; }

        [Display(Name = "Comments")]
        [StringLength(512)]
        public String Comments { get; set; }

        public virtual Case Case { get; set; }
    }
}
