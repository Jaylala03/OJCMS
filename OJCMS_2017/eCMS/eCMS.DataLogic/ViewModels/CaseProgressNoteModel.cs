//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class CaseProgressNoteModel
    {
        public int ID { get; set; }
        public int CaseID { get; set; }
        public string CaseMemberName { get; set; }
        public string ActivityTypeName { get; set; }
        public DateTime NoteDate { get; set; }
        public string Note { get; set; }

        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}
