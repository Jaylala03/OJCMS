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

namespace eCMS.DataLogic.Models
{
    public class CaseProgressNoteMembers 
    {
        [Required]
        [Display(AutoGenerateField = false)]
        [Key]
        public virtual Int32 ID { get; set; }
        [Display(Name = "Family or Family Member")]
       
        [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

            
       
        public Int32 CaseProgressNoteID { get; set; }
        public virtual CaseMember CaseMember { get; set; }
      
    }
}
