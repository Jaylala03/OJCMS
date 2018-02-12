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
namespace eCMS.DataLogic.Models.Lookup
{
    public class SmartGoal : LookupBaseModel
    {
        [Display(Name = "Category")]
        [ForeignKey("QualityOfLifeCategory")]
        public Int32 QualityOfLifeCategoryID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [StringLength(1024)]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        [Display(Name = "Description")]
        [StringLength(1024)]
        public override string Description
        {
            get
            {
                return base.Description;
            }
            set
            {
                base.Description = value;
            }
        }

        public virtual QualityOfLifeCategory QualityOfLifeCategory { get; set; }

        [NotMapped]
        public string Checked { get; set; }
    }
}
