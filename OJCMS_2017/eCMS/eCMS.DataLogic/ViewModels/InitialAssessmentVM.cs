
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
   public class InitialAssessmentVM
    {
        [NotMapped]
        public int IndicatorTypeID { get; set; }

        [NotMapped]
        [Display(Name = "Indicator Name")]
        public string IndicatorName { get; set; }

        [NotMapped]
        [Display(Name = "Description 1")]
        public string AssesmentIndicatorDescription1 { get; set; }

        [NotMapped]
        [Display(Name = "Description 2")]
        public string AssesmentIndicatorDescription2 { get; set; }

        [NotMapped]
        [Display(Name = "Description 3")]
        public string AssesmentIndicatorDescription3 { get; set; }

        [NotMapped]
        [Display(Name = "Description 4")]
        public string AssesmentIndicatorDescription4 { get; set; }

        [NotMapped]
        [Display(Name = "Description 5")]
        public string AssesmentIndicatorDescription5 { get; set; }

        [NotMapped]
        [Display(Name = "Description 6")]
        public string AssesmentIndicatorDescription6 { get; set; }

        [NotMapped]
        public List<IndicatorType> IndicatorType {get; set;}

        [NotMapped]
        public List<AssesmentIndicators> AssesmentIndicators { get; set; }
    }
}