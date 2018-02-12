//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models.Lookup
{
    public class QualityOfLifeSubCategory : LookupBaseModel
    {
        [Display(Name = "Category")]
        [ForeignKey("QualityOfLifeCategory")]
        public Int32 QualityOfLifeCategoryID { get; set; }

        public virtual QualityOfLifeCategory QualityOfLifeCategory { get; set; }

        [NotMapped]
        public List<QualityOfLife> QualityOfLifeList { get; set; }

        [NotMapped]
        public String QualityOfLifeCategoryName { get; set; }
    }
}
