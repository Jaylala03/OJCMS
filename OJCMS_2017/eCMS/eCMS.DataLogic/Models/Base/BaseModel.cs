//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.Shared;
using eCMS.Shared.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public class BaseModel : IBaseModel
    {
        [Required]
        [Display(AutoGenerateField = false)]
        [Key]
        public virtual Int32 ID { get; set; }

        [Required(ErrorMessage = "Created date is required")]
        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        [IncludeInList(IncludeInGrid = false)]
        public virtual DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "Last update date is required")]
        [Display(Name = "Last Updated On")]
        [DataType(DataType.Date)]
        [IncludeInList(IncludeInGrid = false)]
        public virtual DateTime LastUpdateDate { get; set; }

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

        private bool isAjax = false;
        [NotMapped]
        public virtual bool IsAjax
        {
            get
            {
                return isAjax;
            }
            set
            {
                isAjax = value;
            }
        }

        [NotMapped]
        public virtual bool IsSuccess
        {
            get
            {
                return SuccessMessage.IsNotNullOrEmpty();
            }
        }

        private string fancyBoxLink = Constants.CommonConstants.FancyBoxLink;
        [NotMapped]
        public virtual string FancyBoxLink
        {
            get { return fancyBoxLink; }
            set { fancyBoxLink = value; }
        }

        public BaseModel()
        {
        }
    }
}
