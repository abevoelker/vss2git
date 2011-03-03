﻿/* Copyright 2009 HPDI, LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace Hpdi.VssPhysicalLib
{
    /// <summary>
    /// Exception thrown when a truncated record is found.
    /// </summary>
    /// <author>Trevor Robinson</author>
    public class RecordTruncatedException : RecordException
    {
        public RecordTruncatedException()
        {
        }

        public RecordTruncatedException(string message)
            : base(message)
        {
        }
    }
}
