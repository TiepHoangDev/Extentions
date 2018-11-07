using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCommon
{
    public static class LogExtention
    {
        public static void DebugLog(this object message, string description = null)
        {
            Debug.WriteLine($">>>>>>>>>>>>>>>>>>{MethodBase.GetCurrentMethod().DeclaringType.Namespace}>>>>>>>>>>>>>>>>>>");
            Debug.WriteLine($"\t\t[{DateTime.Now}] {description}\t\t");
            Debug.WriteLine(message);
            Debug.WriteLine($"<<<<<<<<<<<<<<<<<{MethodBase.GetCurrentMethod().DeclaringType.Namespace}<<<<<<<<<<<<<<<<<");
        }

        public static void Log(this object message)
        {

        }
    }
}
