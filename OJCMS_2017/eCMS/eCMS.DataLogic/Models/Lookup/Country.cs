using System;
using System.ComponentModel.DataAnnotations;

namespace eCMS.DataLogic.Models.Lookup
{
    public class Country : LookupBaseModel
    {
        [Display(Name = "Country Code")]
        [StringLength(16)]
        public String Code { get; set; }

        [Display(Name = "Area Code")]
        [StringLength(10)]
        public String AreaCode { get; set; }
    }
}
