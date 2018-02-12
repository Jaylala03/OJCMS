//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;

namespace eCMS.DataLogic.Models
{
    public interface ILookupBaseModel : IBaseModel
    {
        string Name { get; set; }

        string Description { get; set; }

        bool IsActive { get; set; }
    }
}
