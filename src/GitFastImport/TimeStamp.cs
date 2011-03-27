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
