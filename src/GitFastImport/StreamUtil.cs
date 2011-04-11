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
using System.IO;
namespace Hpdi.GitFastImport
{
	public class StreamUtil
	{
		public StreamUtil ()
		{
		}
		
		//Copied from http://stackoverflow.com/questions/1080442/how-to-convert-an-stream-into-a-byte-in-c
		public static void CopyStream(Stream input, Stream output)
		{
		    byte[] b = new byte[32768];
		    int r;
		    while ((r = input.Read(b, 0, b.Length)) > 0)
		        output.Write(b, 0, r);
		}
	}
}

