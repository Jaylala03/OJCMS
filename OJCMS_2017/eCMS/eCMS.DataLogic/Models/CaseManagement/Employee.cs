using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.Models
{
    public class Employee
    {
        public int Id
        {
            set;
            get;
        }
        public string FirstName
        {
            set;
            get;
        }
        public string LastName
        {
            set;
            get;
        }
        public int CompanyId
        {
            set;
            get;
        }
        public string CompanyName
        {
            set;
            get;
        }
    }
}
