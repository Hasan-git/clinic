using System;
using System.Linq.Expressions;

namespace Clinic.Common.Core.Expressions
{
    /// Credits to Joseph and Ben Albahari http://www.albahari.com/nutshell/linqkit.aspx
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }
    }
}