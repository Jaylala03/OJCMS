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
    public class TrainingModule 
    {
        public int ID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created_date { get; set; }
        public String FileName { get; set; }

        public string FileLocation { get; set; }
        public int FileType { get; set; }
        [NotMapped]
        public virtual string SuccessMessage
        {
            get;
            set;
        }

        [NotMapped]
        public virtual string ErrorMessage
        {
            get;
            set;
        }
    }
}
