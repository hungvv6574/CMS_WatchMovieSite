using System;
using System.IO;

namespace CMSSolutions.Websites.Extensions
{
    public class LogFiles
    {
        public static void WriteLogSms(string msg)
        {
            try
            {
                var logFile = string.Format(@"{0}\{1}_{2}.txt", Constants.SmsLogFiles, "SMS", DateTime.Now.ToString("dd-MM-yyyy"));
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(DateTime.Now.ToShortTimeString() + ": " + msg);
                    sw.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}