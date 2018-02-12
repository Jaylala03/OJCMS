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

namespace eCMS.DataLogic.Models
{
    public class Worker:BaseModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(256)]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(256)]
        public String LastName { get; set; }

        [Display(Name = "Email")]
        [StringLength(256)]
        public String EmailAddress { get; set; }

        [Required(ErrorMessage = "worker name is required")]
        [Display(Name = "Login Name")]
        [StringLength(256)]
        public String LoginName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        public String Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please confirm the password")]
        [Display(Name = "Confirm Pass")]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Allow Login is required")]
        [Display(Name = "Allow Login")]
        public Boolean AllowLogin { get; set; }

        [Required(ErrorMessage = "Is Active is required")]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [ForeignKey("CreatedByWorker")]
        public Int32? CreatedByworkerID { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Last Password Changed On")]
        public DateTime? LastPasswordChangeDate { get; set; }

        public virtual Worker CreatedByWorker { get; set; }

        [NotMapped]
        public bool RememberMe
        {
            get;
            set;
        }
        [NotMapped]
        public bool IsProgramRestricted
        {
            get;
            set;
        }
        [NotMapped]
        public bool IsRegionRestricted
        {
            get;
            set;
        }
        [NotMapped]
        public bool IsCaseRestricted
        {
            get;
            set;
        }

        [NotMapped]
        [Display(Name="Role")]
        public int RoleID { get; set; }

        [NotMapped]
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [NotMapped]
        [Display(Name="Program")]
        public int ProgramID { get; set; }

        [NotMapped]
        [Display(Name="Region")]
        public int RegionID { get; set; }

        [NotMapped]
        public WorkerInRole WorkerInRole { get; set; }

        [NotMapped]
        public WorkerInRoleNew WorkerInRoleNew { get; set; }

        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public bool HasPermissionToEdit { get; set; }
        [NotMapped]
        public bool HasPermissionToReadmit { get; set; }
        [NotMapped]
        public bool HasPermissionToDelete { get; set; }
    }
}
