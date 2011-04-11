/* Copyright 2011 Abe Voelker
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
namespace GitFastImport
{
	public class TimeStamp
	{
		
		//Default timestamp used by git-fast-import
		public static int GetTimeSinceUnixEpoch(DateTime date)
		{
			TimeSpan t = (date - new DateTime(1970, 1, 1));
			return (int) t.TotalSeconds;
		}
        
        public static string FormatTimeZone(TimeSpan ts)
        {
            return String.Format("{0:00}{1:00}", ts.Hours, ts.Minutes);
        }
	}
}

