using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.Models
{
  public class CaseMemberEthinicity : BaseModel
    {

        [Required(ErrorMessage = "Please select case")]
        [Display(Name = "Case")]
        public Int32 CaseID { get; set; }

        public String EthinicityID { get; set; }

    }
}
