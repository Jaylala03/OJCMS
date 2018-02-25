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

namespace eCMS.DataLogic.Models
{
    public class CaseMemberEmergencyContact : EntityBaseModel
    {
        //[Required(ErrorMessage = "Please select case member")]
        [Display(Name = "Case Member")]
        [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please enter Contact Name")]
        [Display(Name = "Contact Name")]
        [StringLength(256)]
        public String ContactName { get; set; }

        [Required(ErrorMessage = "Please enter Contact Number")]
        [Display(Name = "Contact Number")]
        [StringLength(256)]
        public String ContactNumber { get; set; }

        public virtual CaseMember CaseMember { get; set; }
    }
}
