//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eCMS.DataLogic.Models
{
    public class CaseWorker : EntityBaseModel
    {
        [Index("UIX_CaseID_WorkerID", IsUnique = true, Order = 1)]
        [Required(ErrorMessage = "Please select case")]
        [Display(Name = "Case")]
        [ForeignKey("Case")]
        public Int32 CaseID { get; set; }

        [Index("UIX_CaseID_WorkerID", IsUnique = true, Order=2)]
        [Required(ErrorMessage = "Please select worker")]
        [Display(Name = "Worker Name")]
        [ForeignKey("Worker")]
        public Int32 WorkerID { get; set; }

        [Required(ErrorMessage = "Please check if the worker is active or not")]
        [Display(Name = "Is Active?")]
        public Boolean IsActive { get; set; }

        [Required(ErrorMessage = "Please check if the worker is allowed to get notification")]
        [Display(Name = "Allow Notification?")]
        public Boolean AllowNotification { get; set; }

        [Required(ErrorMessage = "Please check if the worker is primary")]
        [Display(Name = "Is Primary?")]
        public Boolean IsPrimary { get; set; }

        public virtual Case Case { get; set; }
        public virtual Worker Worker { get; set; }
            
        [NotMapped]
        [Display(Name = "Worker")]
        public string WorkerName { set; get;}

        [NotMapped]
        [Display(Name = "Assigned Members")]
        public string AssignedMembers { set; get; }

        [NotMapped]
        [Display(Name = "Assign Family Members to Worker")]
        public List<SelectListItem> CaseMemberList { set; get; }
        [NotMapped]
        [Display(Name = "Role")]
        public int? RoleID { get; set; }

        [NotMapped]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [NotMapped]
        [Display(Name = "Region")]
        public string RegionName { get; set; }
    }
}
