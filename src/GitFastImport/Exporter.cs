using System;
using System.Collections.Generic;
using System.IO;
using Hpdi.VssLogicalLib;
using GitFastImport;

namespace Hpdi.GitFastImport
{
	public class Exporter
	{
		private List<SimpleRevision> blobMarks = new List<SimpleRevision>();
		private int markCount = 0;
		
		public Exporter ()
		{
		}
		
		public void PushBlob(VssFileRevision vfr, Stream blobRawData){
			
			MemoryStream mem = new MemoryStream();
			StreamUtil.CopyStream(blobRawData, mem);
			
			System.Console.Write("blob\nmark :{0}\ndata {1}\n", ++markCount, mem.Length);
			blobMarks.Add(new SimpleRevision(markCount, vfr.File.Name));
			
			//Write out the file data to console
			mem.Seek(0, SeekOrigin.Begin); //reset file pointer
			using (Stream stdout = Console.OpenStandardOutput())
			mem.WriteTo(stdout);
			System.Console.Write('\n');
		}
		
		public class SimpleRevision {
			public int mark { get; private set; }
			public String name { get; private set; }
			public SimpleRevision(int mark, String name){ this.mark = mark; this.name = name;}
		}
	}
}

