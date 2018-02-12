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
    public class ChangePasswordModel:BaseModel
    {
        [Required(ErrorMessage = "Please enter your current password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password", Prompt = "Current Password")]
        public String CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter new password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password", Prompt = "New Password")]
        public String NewPassword { get; set; }

        [DataType(DataType.Password)]

        [Required(ErrorMessage = "Please confirm your password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        public String ConfirmPassword { get; set; }

        public Boolean IsFirstLogin { get; set; }
    }
}
