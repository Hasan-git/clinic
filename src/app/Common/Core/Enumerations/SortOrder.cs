﻿
namespace Clinic.Common.Core.Enumerations
{
    public class SortOrder : Enumeration
    {
        public static readonly SortOrder Ascending = new SortOrder(1, "Ascending");
        public static readonly SortOrder Descending = new SortOrder(2, "Descending");

        public SortOrder()
        {
        }

        public SortOrder(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}