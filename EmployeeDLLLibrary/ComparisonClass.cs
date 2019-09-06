using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDLLLibrary
{
    public static class ComparisonClass
    {
        /// <summary>
        /// Determines if a specific value is a number.
        /// </summary>
        /// <returns><c>true</c> if the value is a number; otherwise, <c>false</c>.</returns>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The Type of value.</typeparam>
        public static bool IsNumber<T>(this T value)
        {
            if (value is sbyte) return true;
            if (value is byte) return true;
            if (value is short) return true;
            if (value is ushort) return true;
            if (value is int) return true;
            if (value is uint) return true;
            if (value is long) return true;
            if (value is ulong) return true;
            if (value is float) return true;
            if (value is double) return true;
            if (value is decimal) return true;
            return false;
        }

        public static bool IsEqualTo<T>(this T n1Value, T n2Value) where T : IComparable<T>
        {
            return n1Value.Equals(n2Value);
        }

        public static bool IsGreaterThan<T>(this T n1Value, T n2Value) where T : IComparable<T>
        {
            return n1Value.CompareTo(n2Value) > 0;
        }

        public static bool IsLessThan<T>(this T n1Value, T n2Value) where T : IComparable<T>
        {
            return n1Value.CompareTo(n2Value) < 0;
        }

        public static bool IsGreaterThanOrEqualTo<T>(this T n1Value, T n2Value) where T : IComparable<T>
        {
            return (n1Value.IsEqualTo(n2Value) || n1Value.IsGreaterThan(n2Value));
        }

        public static bool IsLessThanOrEqualTo<T>(this T n1Value, T n2Value) where T : IComparable<T>
        {
            return (n1Value.IsEqualTo(n2Value) || n1Value.IsLessThan(n2Value));
        }
    }
}
