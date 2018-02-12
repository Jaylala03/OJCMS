//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.Shared.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public class EntityBaseModel : BaseModel, IEntityBaseModel
    {
        [Required(ErrorMessage = "Created by worker is required")]
        [Display(Name = "Created By")]
        [ForeignKey("CreatedByWorker")]
        [IncludeInList(IncludeInGrid = false)]
        public virtual Int32 CreatedByWorkerID { get; set; }

        [Required(ErrorMessage = "Last updated by worker is required")]
        [Display(Name = "Last Updated By")]
        [ForeignKey("LastUpdatedByWorker")]
        [IncludeInList(IncludeInGrid = false)]
        public virtual Int32 LastUpdatedByWorkerID { get; set; }

        [Display(Name = "Is Archived")]
        public bool IsArchived { get; set; }

        [Display(Name = "Created By")]
        public virtual Worker CreatedByWorker { get; set; }
        [Display(Name = "Last Updated By")]
        public virtual Worker LastUpdatedByWorker { get; set; }
        
        [Display(Name = "Created By")]
        [NotMapped]
        public virtual string CreatedByWorkerName { get; set; }

        [Display(Name = "Last Updated By")]
        [NotMapped]
        public virtual string LastUpdatedByWorkerName { get; set; }
    }
}
