//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class WorkerSearchViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public int RoleID { get; set; }

        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [Display(Name="Program")]
        public int ProgramID { get; set; }

        [Display(Name="Region")]
        public int RegionID { get; set; }

      
        [Display(Name = "Role")]
        public string RoleName { get; set; }

      
        [Display(Name = "Region")]
        public string RegionName { get; set; }

       
        [Display(Name = "Worker")]
        public string WorkerName { set; get; }

       
        [Display(Name = "Assigned Members")]
        public string AssignedMembers { set; get; }


       
        [Display(Name = "Is Active?")]
        public Boolean IsActive { get; set; }

       
        [Display(Name = "Allow Notification?")]
        public Boolean AllowNotification { get; set; }

      
        [Display(Name = "Is Primary?")]
        public Boolean IsPrimary { get; set; }

        public int ID { get; set; }

        public int CaseID { get; set; }

        public bool HasPermissionToCreate { get; set; }
        public bool HasPermissionToEdit { get; set; }
        public bool HasPermissionToReadmit { get; set; }
        public bool HasPermissionToDelete { get; set; }

    }
}
