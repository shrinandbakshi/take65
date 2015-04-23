using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ImportFeed
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ImportFeedNews() 
            };
            ServiceBase.Run(ServicesToRun);
            

            /*
        #if(DEBUG)

            ImportFeedNews oDebug = new ImportFeedNews();
                            oDebug.Debug();
                        #endif
             
            * * */

        }
    }
}
