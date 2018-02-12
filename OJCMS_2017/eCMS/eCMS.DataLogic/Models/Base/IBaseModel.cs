//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public interface IBaseModel
    {
        [NotMapped]
        string SuccessMessage { get; set; }

        [NotMapped]
        string ErrorMessage { get; set; }
    }
}
