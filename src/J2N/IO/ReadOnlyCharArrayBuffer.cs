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
using System.Diagnostics.CodeAnalysis;


namespace J2N.IO
{
    using SR = J2N.Resources.Strings;

    /// <summary>
    /// <see cref="CharArrayBuffer"/>, <see cref="ReadWriteCharArrayBuffer"/> and <see cref="ReadOnlyCharArrayBuffer"/> compose
    /// the implementation of array based char buffers.
    /// <para/>
    /// <see cref="ReadOnlyCharArrayBuffer"/> extends <see cref="CharArrayBuffer"/> with all the write methods
    /// throwing read only exception.
    /// <para/>
    /// This class is marked sealed for runtime performance.
    /// </summary>
    internal sealed class ReadOnlyCharArrayBuffer : CharArrayBuffer
    {
        internal static ReadOnlyCharArrayBuffer Copy(CharArrayBuffer other, int markOfOther)
        {
            return new ReadOnlyCharArrayBuffer(other.Capacity, other.backingArray, other.offset)
            {
                limit = other.Limit,
                position = other.Position,
                mark = markOfOther
            };
        }

        internal ReadOnlyCharArrayBuffer(int capacity, char[] backingArray, int arrayOffset)
            : base(capacity, backingArray, arrayOffset)
        { }

        public override CharBuffer AsReadOnlyBuffer() => Duplicate();

        public override CharBuffer Compact()
        {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer Duplicate() => Copy(this, mark);

        public override bool IsReadOnly => true;

        [SuppressMessage("Microsoft.Performance", "CA1819", Justification = "design requires some writable array properties")]
        protected override char[] ProtectedArray
        {
            get { throw new ReadOnlyBufferException(); }
        }

        protected override int ProtectedArrayOffset
        {
            get { throw new ReadOnlyBufferException(); }
        }

        protected override bool ProtectedHasArray => false;

        public override CharBuffer Put(char value)
        {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer Put(int index, char value)
        {
            throw new ReadOnlyBufferException();
        }

        public override sealed CharBuffer Put(char[] source, int offset, int length)
        {
            throw new ReadOnlyBufferException();
        }

        public override sealed CharBuffer Put(CharBuffer src)
        {
            throw new ReadOnlyBufferException();
        }

        public override CharBuffer Put(string source, int startIndex, int length)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            int len = source.Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), SR.ArgumentOutOfRange_NeedNonNegNum);
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), SR.ArgumentOutOfRange_NeedNonNegNum);
            if (startIndex > len - length) // Checks for int overflow
                throw new ArgumentOutOfRangeException(nameof(length), SR.ArgumentOutOfRange_IndexLength);

            throw new ReadOnlyBufferException();
        }

        public override CharBuffer Slice()
        {
            return new ReadOnlyCharArrayBuffer(Remaining, backingArray, offset + position);
        }
    }
}
