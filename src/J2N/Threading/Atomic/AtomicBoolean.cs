﻿#region Copyright 2010 by Apache Harmony, Licensed under the Apache License, Version 2.0
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
using System.Diagnostics;
using System.Threading;


namespace J2N.Threading.Atomic
{
    /// <summary>
    /// A <see cref="bool"/> value that may be updated atomically.
    /// An <see cref="AtomicBoolean"/> is used in applications such as
    /// atomically updated flags, and cannot be used as a replacement for a
    /// <see cref="System.Boolean"/>. However, this class does
    /// implement implicit conversion to <see cref="bool"/>, so it can
    /// be utilized with language features, tools and utilities that deal
    /// with binary operations.
    /// </summary>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    [DebuggerDisplay("{Value}")]
    public class AtomicBoolean : IEquatable<AtomicBoolean>, IEquatable<bool>
    {
        private int value = 0;

        // The true value.
        //
        internal const int True = 1;

        // The false value.
        //
        internal const int False = 0;

        /// <summary>
        /// Creates a new <see cref="AtomicBoolean"/> with initial value of <c>false</c>.
        /// </summary>
        public AtomicBoolean()
            : this(false)
        { }

        /// <summary>
        /// Creates a new <see cref="AtomicBoolean"/> with the given initial <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The inital value.</param>
        public AtomicBoolean(bool value)
        {
            this.value = value ? True : False;
        }

        /// <summary>
        /// Gets or sets the current value. Note that these operations can be done
        /// implicitly by setting the <see cref="AtomicInt32"/> to an <see cref="int"/>.
        /// <code>
        /// AtomicBoolean abool = new AtomicBoolean(true);
        /// bool x = abool;
        /// </code>
        /// </summary>
        public bool Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0) == True ? true : false;
            set => Interlocked.Exchange(ref this.value, value ? True : False);
        }

        /// <summary>
        /// Atomically sets the value to the given updated value
        /// if the current value <c>==</c> the expected value.
        /// </summary>
        /// <param name="expect">The expected value (the comparand).</param>
        /// <param name="update">The new value.</param>
        /// <returns><c>true</c> if successful. A <c>false</c> return value indicates that
        /// the actual value was not equal to the expected value.</returns>
        public bool CompareAndSet(bool expect, bool update)
        {
            int e = expect ? True : False;
            int u = update ? True : False;

            int original = Interlocked.CompareExchange(ref value, u, e);

            return original == e;
        }

        /// <summary>
        /// Atomically sets to the given value and returns the previous value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>The previous value.</returns>
        public bool GetAndSet(bool newValue)
        {
            return Interlocked.Exchange(ref value, newValue ? 1 : 0) == True;
        }

        /// <summary>
        /// Determines whether the specified <see cref="AtomicBoolean"/> is equal to the current <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="other">The <see cref="AtomicBoolean"/> to compare with the current <see cref="AtomicBoolean"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current <see cref="AtomicBoolean"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(AtomicBoolean? other)
        {
            if (other is null)
                return false;
            return Value == other.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="bool"/> is equal to the current <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="other">The <see cref="bool"/> to compare with the current <see cref="AtomicBoolean"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current <see cref="AtomicBoolean"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(bool other)
        {
            return Value == other;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="AtomicBoolean"/>.
        /// <para/>
        /// If <paramref name="other"/> is a <see cref="AtomicBoolean"/>, the comparison is not done atomically.
        /// </summary>
        /// <param name="other">The <see cref="object"/> to compare with the current <see cref="AtomicBoolean"/>.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current <see cref="AtomicBoolean"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? other)
        {
            if (other is AtomicBoolean ab)
                return Equals(ab);
            if (other is bool b)
                return Equals(b);
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns the <see cref="string"/> representation of the current value.
        /// </summary>
        /// <returns>The <see cref="string"/> representation of the current value.</returns>
        public override string ToString()
        {
            return Value ? "true" : "false";
        }

        #region Operator Overrides

        /// <summary>
        /// Implicitly converts an <see cref="AtomicBoolean"/> to an <see cref="bool"/>.
        /// </summary>
        /// <param name="atomicBoolean">The <see cref="AtomicBoolean"/> to convert.</param>
        public static implicit operator bool(AtomicBoolean atomicBoolean)
        {
            return atomicBoolean.Value;
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value equality.
        /// </summary>
        /// <param name="a1">The first value.</param>
        /// <param name="a2">The second value.</param>
        /// <returns><c>true</c> if the given values are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(AtomicBoolean a1, AtomicBoolean a2)
        {
            return a1.Value == a2.Value;
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value inequality.
        /// </summary>
        /// <param name="a1">The first value.</param>
        /// <param name="a2">The second value.</param>
        /// <returns><c>true</c> if the given values are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(AtomicBoolean a1, AtomicBoolean a2)
        {
            return !(a1 == a2);
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value equality.
        /// </summary>
        /// <param name="a1">The first value.</param>
        /// <param name="a2">The second value.</param>
        /// <returns><c>true</c> if the given values are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(AtomicBoolean a1, bool a2)
        {
            return a1.Value == a2;
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value inequality.
        /// </summary>
        /// <param name="a1">The first value.</param>
        /// <param name="a2">The second value.</param>
        /// <returns><c>true</c> if the given values are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(AtomicBoolean a1, bool a2)
        {
            return !(a1 == a2);
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value equality.
        /// </summary>
        /// <param name="a1">The first value.</param>
        /// <param name="a2">The second value.</param>
        /// <returns><c>true</c> if the given values are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(bool a1, AtomicBoolean a2)
        {
            return a1 == a2.Value;
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value inequality.
        /// </summary>
        /// <param name="a1">The first number.</param>
        /// <param name="a2">The second number.</param>
        /// <returns><c>true</c> if the given numbers are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(bool a1, AtomicBoolean a2)
        {
            return !(a1 == a2);
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value equality.
        /// </summary>
        /// <param name="a1">The first number.</param>
        /// <param name="a2">The second number.</param>
        /// <returns><c>true</c> if the given numbers are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(AtomicBoolean a1, bool? a2)
        {
            return a1.Value == a2.GetValueOrDefault();
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value inequality.
        /// </summary>
        /// <param name="a1">The first number.</param>
        /// <param name="a2">The second number.</param>
        /// <returns><c>true</c> if the given numbers are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(AtomicBoolean a1, bool? a2)
        {
            return !(a1 == a2);
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value equality.
        /// </summary>
        /// <param name="a1">The first number.</param>
        /// <param name="a2">The second number.</param>
        /// <returns><c>true</c> if the given numbers are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(bool? a1, AtomicBoolean a2)
        {
            return a1.GetValueOrDefault() == a2.Value;
        }

        /// <summary>
        /// Compares <paramref name="a1"/> and <paramref name="a2"/> for value inequality.
        /// </summary>
        /// <param name="a1">The first number.</param>
        /// <param name="a2">The second number.</param>
        /// <returns><c>true</c> if the given numbers are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(bool? a1, AtomicBoolean a2)
        {
            return !(a1 == a2);
        }

        #endregion Operator Overrides
    }
}
