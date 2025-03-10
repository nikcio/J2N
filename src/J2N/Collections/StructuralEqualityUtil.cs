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

using J2N.Collections.Generic;
using System;
using System.Reflection;


namespace J2N.Collections
{
    /// <summary>
    /// Utilities for structural equality of arrays and collections.
    /// </summary>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    internal static class StructuralEqualityUtil
    {
        internal static Func<T, int> LoadGetHashCodeDelegate<T>(bool isValueType, bool isObject, StructuralEqualityComparer structuralEqualityComparer)
        {
            if (isValueType)
                return (value) => EqualityComparer<T>.Default.GetHashCode(value!); // J2N TODO: Note that value can be null here, need to investigate how to override the interface
            if (isObject)
                return (value) => IsValueType(value) ?
                    EqualityComparer<T>.Default.GetHashCode(value!) : // J2N TODO: Note that value can be null here, need to investigate how to override the interface
                    (value == null ? 0 : structuralEqualityComparer.GetHashCode(value));
            else
                return (value) => value == null ? 0 : structuralEqualityComparer.GetHashCode(value);
        }

        internal static Func<T, T, bool> LoadEqualsDelegate<T>(bool isValueType, bool isObject, StructuralEqualityComparer structuralEqualityComparer)
        {
            if (isValueType)
                return (valueA, valueB) => EqualityComparer<T>.Default.Equals(valueA, valueB);
            else if (isObject)
                return (valueA, valueB) => IsValueType(valueA) || IsValueType(valueB) ?
                                EqualityComparer<T>.Default.Equals(valueA, valueB) :
                                (valueA is null ? valueB is null : structuralEqualityComparer.Equals(valueA, valueB));
            else // Reference type
                return (valueA, valueB) => valueA is null ? valueB is null : structuralEqualityComparer.Equals(valueA, valueB);
        }

        internal static bool IsValueType<TElement>(TElement value) => !(value is null) &&
#if FEATURE_TYPEEXTENSIONS_GETTYPEINFO
            value.GetType().GetTypeInfo().IsValueType;
#else
            value.GetType().IsValueType;
#endif
    }
}
