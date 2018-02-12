//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;

namespace eCMS.DataLogic.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IncludeInList : Attribute
    {
        public int Sequence
        {
            get;
            set;
        }

        public bool IncludeInGrid
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public bool AllowWidthInPercentage
        {
            get;
            set;
        }

        public bool AllowSorting
        {
            get;
            set;
        }

        public bool AllowGrouping
        {
            get;
            set;
        }

        public bool AllowFiltering
        {
            get;
            set;
        }

        public bool AllowSearch
        {
            get;
            set;
        }

        public bool IsForeignKey
        {
            get;
            set;
        }

        public String ColumnName
        {
            get;
            set;
        }

        public String DisplayName
        {
            get;
            set;
        }

        public IncludeInList()
        {
            Sequence = 0;
            Width = 100;
            AllowWidthInPercentage = false;
            AllowSorting = true;
            AllowGrouping = false;
            AllowFiltering = false;
            ColumnName = DisplayName = String.Empty;
            IncludeInGrid = true;
        }
    }
}
