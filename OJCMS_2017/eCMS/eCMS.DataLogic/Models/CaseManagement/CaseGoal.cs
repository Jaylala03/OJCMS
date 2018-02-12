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
    public class CaseGoal : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select family or family member")]
        [Display(Name = "Family or Family Member")]
        [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please enter start date")]
        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter end date")]
        [Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "What do you wish to do differently in your life?")]
        [Display(Name = "What do you wish to do differently in your life?")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String WishInLife { get; set; }

        [Display(Name = "Comments")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [System.Web.Mvc.AllowHtml]
        public String Comments { get; set; }

        public virtual CaseMember CaseMember { get; set; }

        [NotMapped]
        public int CaseID { get; set; }

        [NotMapped]
        [Display(Name="Family or Family Member")]
        public string CaseMemberName { get; set; }

        [NotMapped]
        [Display(Name = "Select Quality of Life Category")]
        public String QualityOfLifeCategoryIDs { get; set; }

        [NotMapped]
        [Display(Name = "Quality of Life Category")]
        public String QualityOfLifeCategoryNames { get; set; }

        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public string HasPermissionToCreateSmartGoal { get; set; }

    }
}