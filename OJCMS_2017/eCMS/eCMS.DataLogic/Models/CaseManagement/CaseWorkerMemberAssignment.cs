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
    public class CaseWorkerMemberAssignment : EntityBaseModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int ID
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select case worker")]
        [Display(Name = "Worker")]
        [ForeignKey("CaseWorker")]
        public Int32 CaseWorkerID { get; set; }

        [Required(ErrorMessage = "Please select case member")]
        [Display(Name = "Member")]
        [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

        public virtual CaseWorker CaseWorker { get; set; }
        public virtual CaseMember CaseMember { get; set; }

        [NotMapped]
        public string CaseMemberName { get; set; }
    }
}
