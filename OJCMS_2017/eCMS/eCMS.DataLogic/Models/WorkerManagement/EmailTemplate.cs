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
using System.Web.Mvc;
namespace eCMS.DataLogic.Models
{
    public class EmailTemplate : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select category")]
        [Display(Name = "Category")]
        [ForeignKey("EmailTemplateCategory")]
        public Int32 EmailTemplateCategoryID { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [Display(Name = "Template Name")]
        [StringLength(256)]
        public String Name { get; set; }

        [Required(ErrorMessage = "Please enter email subject")]
        [Display(Name = "Subject")]
        [StringLength(256)]
        public String EmailSubject { get; set; }

        [Required(ErrorMessage = "Please enter email body")]
        [Display(Name = "Body")]
        [MaxLength]
        [DataType(DataType.Html)]
        [AllowHtml]
        public String EmailBody { get; set; }

        public virtual EmailTemplateCategory EmailTemplateCategory { get; set; }

        [NotMapped]
        public string EmailTemplateCategoryName { get; set; }

    }
}












