//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public interface IEntityBaseModel : IBaseModel
    {
        DateTime CreateDate { get; set; }

        Int32 CreatedByWorkerID { get; set; }

        DateTime LastUpdateDate { get; set; }

        Int32 LastUpdatedByWorkerID { get; set; }

        Worker CreatedByWorker { get; set; }
        Worker LastUpdatedByWorker { get; set; }

        [NotMapped]
        string CreatedByWorkerName { get; set; }
    }
}
