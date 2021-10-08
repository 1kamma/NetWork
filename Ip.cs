using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NetWork
{
    interface Ip
    {
        [DllImport("IPHLPAPI.DLL")]
        public static extern int ipconfig();
        public static void Ma()
        {

        }
    }
}
