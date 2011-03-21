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

