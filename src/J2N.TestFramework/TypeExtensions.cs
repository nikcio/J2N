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
using System.IO;
using System.Reflection;

namespace J2N
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Locates resources in the same directory as this type
        /// </summary>
        public static Stream getResourceAsStream(this Type t, string name)
        {
#if FEATURE_TYPEEXTENSIONS_GETTYPEINFO
            return t.GetTypeInfo().Assembly.FindAndGetManifestResourceStream(t, name);
#else
            return t.Assembly.FindAndGetManifestResourceStream(t, name);
#endif
        }
    }
}
