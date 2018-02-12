using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySoft.Helper;

namespace eCMS.DataLogic.Models.Report
{
    public class CaseDashboardRptInput : IBaseModel
    {
        [Required(ErrorMessage = "Please enter start date.")]
        [Display(Name = "Enroll Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter end date.")]
        [Display(Name = "Enroll End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        [NotMapped]
        [Display(Name = "Program")]
        public string[] ProgramID { get; set; }

        [NotMapped]
        [Display(Name = "Primary Coordinator")]
        public string[] SubProgramID { get; set; }

        [NotMapped]
        [Display(Name = "Region")]
        public string[] RegionID { get; set; }

        [NotMapped]
        [Display(Name = "Jamatkhana")]
        public string[] JamatkhanaID { get; set; }

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
    }
}
