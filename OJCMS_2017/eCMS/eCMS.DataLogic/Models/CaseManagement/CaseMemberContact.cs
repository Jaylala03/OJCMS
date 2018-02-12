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
    public class CaseMemberContact : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select case member")]
        [Display(Name = "Case Member")]
        [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please select type")]
        [Display(Name = "Type")]
        [ForeignKey("ContactMedia")]
        public Int32 ContactMediaID { get; set; }

        [Required(ErrorMessage = "Please enter contact info")]
        [Display(Name = "Phone/Email")]
        [StringLength(128)]
        public String Contact { get; set; }

        [Required(ErrorMessage = "Please enter comments")]
        [Display(Name = "Comments")]
        [StringLength(256)]
        public String Comments { get; set; }


        [Required(ErrorMessage = "Please enter Contact Name")]
        [Display(Name = "Contact Name")]
        [StringLength(256)]
        public String EmergencyContactName { get; set; }

        [Required(ErrorMessage = "Please enter Contact Number")]
        [Display(Name = "Contact Number")]
        [StringLength(256)]
        public String EmergencyContactNumber { get; set; }

        public virtual CaseMember CaseMember { get; set; }
        public virtual ContactMedia ContactMedia { get; set; }


        [NotMapped]
        [Display(Name = "Contact Media")]
        public string ContactMediaName { get; set; }
    }
}
