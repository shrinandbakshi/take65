using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Website.App_Start
{
    public class TraceLog
    {
        private string logpath = string.Empty;
        private FileStream fs = null;
        public static TraceLog Instance = new TraceLog(); 
        private TraceLog()
        {
            logpath = HttpContext.Current.Server.MapPath("~/App_Data/TraceLog.txt");
        }
        public void log(string component, string msg)
        {
            fs = new FileStream(logpath, FileMode.Append, FileAccess.Write);
            string l = DateTime.Now.ToString() + " : " + component + "  " + msg; 
            using(StreamWriter writer = new StreamWriter(fs))
            {
                writer.WriteLine(l);               
            }
            fs.Close();
        }
    }
}