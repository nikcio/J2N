﻿#region Copyright 2019-2021 by Shad Storhaug, Licensed under the Apache License, Version 2.0
/*  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace J2N.Text
{
    /// <summary>
    /// Represents a character sequence comparison operation that uses specific case and culture-based ordinal comparison rules.
    /// </summary>
    public abstract class CharSequenceComparer :
        IComparer, IEqualityComparer,
        IComparer<ICharSequence>, IEqualityComparer<ICharSequence>
    {
        private static readonly CharSequenceComparer ordinal = new OrdinalComparer();

        /// <summary>
        /// Gets a <see cref="CharSequenceComparer"/> object that performs a case-sensitive ordinal string comparison.
        /// </summary>
        public static CharSequenceComparer Ordinal => ordinal;

        /// <summary>
        /// Compares two objects and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">An object to compare to <paramref name="y"/>.</param>
        /// <param name="y">An object to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public virtual int Compare(object? x, object? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (x is ICharSequence sa)
            {
                if (y is ICharSequence otherCharSequence)
                    return Compare(sa, otherCharSequence);
                else if (y is string otherString)
                    return Compare(sa, otherString);
                else if (y is StringBuilder otherStringBuilder)
                    return Compare(sa, otherStringBuilder);
                else if (y is char[] otherCharArray)
                    return Compare(sa, otherCharArray);
            }

            if (x is IComparable comparable)
                return comparable.CompareTo(y);

            throw new ArgumentException($"Argument '{nameof(x)}' must implement IComparable");
        }

        /// <summary>
        /// Compares two <see cref="ICharSequence"/>s and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public abstract int Compare(ICharSequence? x, ICharSequence? y);

        /// <summary>
        /// Compares two character sequences and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public abstract int Compare(ICharSequence? x, char[]? y);

        /// <summary>
        /// Compares two character sequences and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public abstract int Compare(ICharSequence? x, StringBuilder? y);

        /// <summary>
        /// Compares two character sequences and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public abstract int Compare(ICharSequence? x, string? y);

        /// <summary>
        /// Compares two character sequences and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public virtual int Compare(ICharSequence? x, CharArrayCharSequence? y) => Compare(x, y?.Value);

        /// <summary>
        /// Compares two character sequences and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public virtual int Compare(ICharSequence? x, StringBuilderCharSequence? y) => Compare(x, y?.Value);

        /// <summary>
        /// Compares two character sequences and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">An <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <term>Meaning</term>
        ///     </listheader>
        ///     <item>
        ///         <term>Less than zero </term>
        ///         <term><paramref name="x"/> precedes y in the sort order. -or- <paramref name="x"/> is <c>null</c> and <paramref name="y"/> is not <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Zero </term>
        ///         <term><paramref name="x"/> is equal to <paramref name="y"/>. -or- <paramref name="x"/> and <paramref name="y"/> are both <c>null</c>.</term>
        ///     </item>
        ///     <item>
        ///         <term>Greater than zero </term>
        ///         <term><paramref name="x"/> follows <paramref name="y"/> in the sort order. -or- <paramref name="y"/> is <c>null</c> and <paramref name="x"/> is not <c>null</c>.</term>
        ///     </item>
        /// </list>
        /// </returns>
        public virtual int Compare(ICharSequence? x, StringCharSequence? y) => Compare(x, y?.Value);

        /// <summary>
        /// Indicates whether two objects or character sequences are equal.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">A <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object,
        /// or <paramref name="x"/> and <paramref name="y"/> both contain the same sequence of characters,
        /// or both <paramref name="x"/> and <paramref name="y"/> are <c>null</c>; otherwise <c>false</c>.</returns>
        public virtual new bool Equals(object? x, object? y)
        {
            if (x == y) return true;
            if (x is null || y is null) return false;

            if (x is ICharSequence sa)
            {
                if (y is ICharSequence otherCharSequence)
                    return Equals(sa, otherCharSequence);
                else if (y is string otherString)
                    return Equals(sa, otherString);
                else if (y is StringBuilder otherStringBuilder)
                    return Equals(sa, otherStringBuilder);
                else if (y is char[] otherCharArray)
                    return Equals(sa, otherCharArray);
            }
            return x.Equals(y);
        }

        /// <summary>
        /// Indicates whether two character sequences are equal.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">A <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object,
        /// or <paramref name="x"/> and <paramref name="y"/> both contain the same sequence of characters,
        /// or both <paramref name="x"/> and <paramref name="y"/> are <c>null</c>; otherwise <c>false</c>.</returns>
        public abstract bool Equals(ICharSequence? x, ICharSequence? y);

        /// <summary>
        /// Indicates whether two character sequences are equal.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">A <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object,
        /// or <paramref name="x"/> and <paramref name="y"/> both contain the same sequence of characters,
        /// or both <paramref name="x"/> and <paramref name="y"/> are <c>null</c>; otherwise <c>false</c>.</returns>
        public abstract bool Equals(ICharSequence? x, char[]? y);

        /// <summary>
        /// Indicates whether two character sequences are equal.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">A <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object,
        /// or <paramref name="x"/> and <paramref name="y"/> both contain the same sequence of characters,
        /// or both <paramref name="x"/> and <paramref name="y"/> are <c>null</c>; otherwise <c>false</c>.</returns>
        public abstract bool Equals(ICharSequence? x, StringBuilder? y);

        /// <summary>
        /// Indicates whether two character sequences are equal.
        /// </summary>
        /// <param name="x">A <see cref="ICharSequence"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">A <see cref="ICharSequence"/> to compare to <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> and <paramref name="y"/> refer to the same object,
        /// or <paramref name="x"/> and <paramref name="y"/> both contain the same sequence of characters,
        /// or both <paramref name="x"/> and <paramref name="y"/> are <c>null</c>; otherwise <c>false</c>.</returns>
        public abstract bool Equals(ICharSequence? x, string? y);

        /// <summary>
        /// Gets the hash code for the specified object.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter,
        /// or <see cref="int.MaxValue"/> if <paramref name="obj"/> is <c>null</c>.</returns>
        public virtual int GetHashCode(object? obj)
        {
            if (obj is null)
                return int.MaxValue;

            if (obj is string otherString)
                return GetHashCode(otherString);
            else if (obj is StringBuilder otherStringBuilder)
                return GetHashCode(otherStringBuilder);
            else if (obj is char[] otherCharArray)
                return GetHashCode(otherCharArray);
            else if (obj is StringCharSequence otherStringCharSequence)
                return GetHashCode(otherStringCharSequence);
            else if (obj is StringBuilderCharSequence otherStringBuilderCharSequence)
                return GetHashCode(otherStringBuilderCharSequence);
            else if (obj is CharArrayCharSequence otherCharArrayCharSequence)
                return GetHashCode(otherCharArrayCharSequence);

            return obj.GetHashCode();
        }

        /// <summary>
        /// Gets the hash code for the specified character sequence.
        /// </summary>
        /// <param name="obj">A character sequence.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter,
        /// or <see cref="int.MaxValue"/> if <paramref name="obj"/> is <c>null</c>.</returns>
        public abstract int GetHashCode(ICharSequence? obj);

        /// <summary>
        /// Gets the hash code for the specified character sequence.
        /// </summary>
        /// <param name="obj">A character sequence.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter,
        /// or <see cref="int.MaxValue"/> if <paramref name="obj"/> is <c>null</c>.</returns>
        public abstract int GetHashCode(char[]? obj);

        /// <summary>
        /// Gets the hash code for the specified character sequence.
        /// </summary>
        /// <param name="obj">A character sequence.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter,
        /// or <see cref="int.MaxValue"/> if <paramref name="obj"/> is <c>null</c>.</returns>
        public abstract int GetHashCode(StringBuilder? obj);

        /// <summary>
        /// Gets the hash code for the specified character sequence.
        /// </summary>
        /// <param name="obj">A character sequence.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter,
        /// or <see cref="int.MaxValue"/> if <paramref name="obj"/> is <c>null</c>.</returns>
        public abstract int GetHashCode(string? obj);


        private class OrdinalComparer : CharSequenceComparer
        {
            public override int Compare(ICharSequence? x, ICharSequence? y)
            {
                if (x is null || !x.HasValue) return (y is null || y.HasValue) ? 0 : -1;
                if (y is null || !y.HasValue) return 1;

                int length = Math.Min(x.Length, y.Length);
                int result;
                for (int i = 0; i < length; i++)
                {
                    if ((result = x[i] - y[i]) != 0)
                        return result;
                }

                // At this point, we have compared all the characters in at least one string.
                // The longer string will be larger.
                return x.Length - y.Length;
            }

            public override int Compare(ICharSequence? x, char[]? y)
            {
                if (x is null || !x.HasValue) return (y is null) ? 0 : -1;
                if (y is null) return 1;

                int length = Math.Min(x.Length, y.Length);
                int result;
                for (int i = 0; i < length; i++)
                {
                    if ((result = x[i] - y[i]) != 0)
                        return result;
                }

                // At this point, we have compared all the characters in at least one string.
                // The longer string will be larger.
                return x.Length - y.Length;
            }

            public override int Compare(ICharSequence? x, string? y)
            {
                if (x is null) return (y is null) ? 0 : -1;
                if (y is null) return 1;

                int length = Math.Min(x.Length, y.Length);
                int result;
                for (int i = 0; i < length; i++)
                {
                    if ((result = x[i] - y[i]) != 0)
                        return result;
                }

                // At this point, we have compared all the characters in at least one string.
                // The longer string will be larger.
                return x.Length - y.Length;
            }

            public override int Compare(ICharSequence? x, StringBuilder? y)
            {
                if (x == null) return -1;
                if (y == null) return 1;

                // NOTE: This benchmarked to be faster than looping through the StringBuilder
                string temp = y.ToString();

                int length = Math.Min(x.Length, temp.Length);
                int result;
                for (int i = 0; i < length; i++)
                {
                    if ((result = x[i] - temp[i]) != 0)
                        return result;
                }

                // At this point, we have compared all the characters in at least one string.
                // The longer string will be larger.
                return x.Length - temp.Length;
            }

            public override bool Equals(ICharSequence? x, ICharSequence? y)
            {
                if (x is null || !x.HasValue)
                    return y is null || !y.HasValue;
                if (y is null || !y.HasValue)
                    return false;

                int len = x.Length;
                if (len != y.Length) return false;
                for (int i = 0; i < len; i++)
                {
                    if (!x[i].Equals(y[i])) return false;
                }
                return true;
            }

            public override bool Equals(ICharSequence? x, char[]? y)
            {
                if (x is null || !x.HasValue)
                    return y is null;
                if (y is null)
                    return false;

                int len = x.Length;
                if (len != y.Length) return false;
                for (int i = 0; i < len; i++)
                {
                    if (!x[i].Equals(y[i])) return false;
                }
                return true;
            }

            public override bool Equals(ICharSequence? x, StringBuilder? y)
            {
                if (x is null || !x.HasValue)
                    return y is null;
                if (y is null)
                    return false;

                // NOTE: This benchmarked to be faster than looping through the StringBuilder
                return EqualsImpl(x, y.ToString());
            }

            public override bool Equals(ICharSequence? x, string? y)
            {
                if (x is null || !x.HasValue)
                    return y is null;
                if (y is null)
                    return false;

                return EqualsImpl(x, y);
            }

            private static bool EqualsImpl(ICharSequence x, string y)
            {
                int len = x.Length;
                if (len != y.Length) return false;
                for (int i = 0; i < len; i++)
                {
                    if (!x[i].Equals(y[i])) return false;
                }
                return true;
            }

            public override int GetHashCode(ICharSequence? obj)
            {
                if (obj is null ||!obj.HasValue)
                    return int.MaxValue;

                // From Apache Harmony
                int length = obj.Length;
                if (length == 0)
                    return 0;
                int hash = 0;
                for (int i = 0; i < length; i++)
                {
                    hash = obj[i] + ((hash << 5) - hash);
                }
                return hash;
            }

            public override int GetHashCode(char[]? obj)
            {
                if (obj is null)
                    return int.MaxValue;

                // From Apache Harmony
                int length = obj.Length;
                if (length == 0)
                    return 0;
                int hash = 0;
                for (int i = 0; i < length; i++)
                {
                    hash = obj[i] + ((hash << 5) - hash);
                }
                return hash;
            }

            public override int GetHashCode(StringBuilder? obj)
            {
                if (obj is null)
                    return int.MaxValue;

                // NOTE: This benchmarked to be faster than looping through the StringBuilder
                return GetHashCodeImpl(obj.ToString());
            }

            public override int GetHashCode(string? obj)
            {
                if (obj is null)
                    return int.MaxValue;

                return GetHashCodeImpl(obj);
            }

            private static int GetHashCodeImpl(string obj)
            {
                // From Apache Harmony
                int length = obj.Length;
                if (length == 0)
                    return 0;
                int hash = 0;
                for (int i = 0; i < length; i++)
                {
                    hash = obj[i] + ((hash << 5) - hash);
                }
                return hash;
            }
        }
    }
}
