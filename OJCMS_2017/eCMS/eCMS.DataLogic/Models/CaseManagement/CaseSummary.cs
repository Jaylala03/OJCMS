using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.Models
{
    public class CaseSummary : EntityBaseModel
    {
        [StringLength(32)]
        public String DisplayID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollDate { get; set; }

        public Case caseModel { get; set; }

    }
}
