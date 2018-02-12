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
    public class WorkerToDo : EntityBaseModel
    {
        [Display(Name = "Worker")]
        [ForeignKey("Worker")]
        public Int32 WorkerID { get; set; }

        public String Subject { get; set; }

        public String Description { get; set; }

        public Boolean IsReviewed { get; set; }

        public Boolean IsCompleted { get; set; }

        public DateTime? CompletedOn { get; set; }

        public String ReferenceLink { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
