﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
   public class CaseSummaryVM
    {
        public int CaseID { get; set; }
        public string Program { get; set; }
        public string SubProgram { get; set; }
        public string Region { get; set; }
        public string Jamatkhaana { get; set; }
        public string ReferenceCase { get; set; }
        public DateTime? EnrolmentDate { get; set; }
        public string IntakeMethod { get; set; }
        public string ReferralSource { get; set; }
        public DateTime? ReferralDate { get; set; }
        public string RiskLevel { get; set; }
        public string PresentingProblem { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}