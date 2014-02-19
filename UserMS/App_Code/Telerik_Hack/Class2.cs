using System;
using System.Globalization;

namespace Telerik.Pivot.Core.Groups
{
    /// <summary>
    /// Used for <see cref="IGroup.Name"/> values of <see cref="IGroup"/>s that are grouping by <see cref="DateTime"/>.
    /// The <see cref="MonthGroup"/> contains the items with <see cref="DateTime"/> values with the same <see cref="Month"/>.
    /// </summary>
#if WPF
    [Serializable]
#endif
    public struct MonthGroup2 : IComparable, IComparable<MonthGroup2>, IEquatable<MonthGroup2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthGroup" /> struct.
        /// </summary>
        /// <param name="month">The month.</param>
        public MonthGroup2(int month)
            : this()
        {
            this.Month = month;
        }

        /// <summary>
        /// Gets the Month this <see cref="MonthGroup"/> represents.
        /// </summary>
        public int Month
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(this.Month)+"S";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Month;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is MonthGroup)
            {
                return this.Equals((MonthGroup)obj);
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(MonthGroup2 other)
        {
            return this.Month == other.Month;
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is MonthGroup)
            {
                return this.CompareTo((MonthGroup)obj);
            }

            throw new ArgumentException("Can not compare.", "obj");
        }

        /// <inheritdoc />
        public int CompareTo(MonthGroup2 other)
        {
            return this.Month.CompareTo(other.Month);
        }

        /// <summary>
        /// Determines whether one specified <see cref="MonthGroup"/> is less than another specified <see cref="MonthGroup"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, false.</returns>
        public static bool operator <(MonthGroup2 left, MonthGroup2 right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one specified <see cref="MonthGroup"/> is greater than another specified <see cref="MonthGroup"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, false.</returns>
        public static bool operator >(MonthGroup2 left, MonthGroup2 right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether one specified <see cref="MonthGroup"/> is less than or equal to another specified <see cref="MonthGroup"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, false.</returns>
        public static bool operator <=(MonthGroup2 left, MonthGroup2 right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether one specified <see cref="MonthGroup"/> is greater than or equal to another specified <see cref="MonthGroup"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, false.</returns>
        public static bool operator >=(MonthGroup2 left, MonthGroup2 right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthGroup"/> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if <paramref name="left"/> and <paramref name="right"/> represent the same month group; otherwise, false.</returns>
        public static bool operator ==(MonthGroup2 left, MonthGroup2 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="MonthGroup"/> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if <paramref name="left"/> and <paramref name="right"/> do not represent the same month group; otherwise, false.</returns>
        public static bool operator !=(MonthGroup2 left, MonthGroup2 right)
        {
            return !left.Equals(right);
        }
    }
}