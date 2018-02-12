using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCMS.DataLogic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class EthnicityViewModel
    {
        public string EthnicityName { get; set; }
    }
}
