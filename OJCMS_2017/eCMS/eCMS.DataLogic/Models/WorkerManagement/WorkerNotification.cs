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
    public class WorkerNotification : EntityBaseModel
    {
        [Display(Name = "Worker")]
        [ForeignKey("Worker")]
        public Int32 WorkerID { get; set; }

        public String Notification { get; set; }

        public Boolean IsRead { get; set; }

        public String ReferenceLink { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
